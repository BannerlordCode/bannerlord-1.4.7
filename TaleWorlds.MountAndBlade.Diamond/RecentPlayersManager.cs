using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000154 RID: 340
	public static class RecentPlayersManager
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0000DD7C File Offset: 0x0000BF7C
		private static PlatformFilePath RecentPlayerFilePath
		{
			get
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Data");
				return new PlatformFilePath(platformDirectoryPath, "RecentPlayers.json");
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0000DDA1 File Offset: 0x0000BFA1
		public static MBReadOnlyList<RecentPlayerInfo> RecentPlayers
		{
			get
			{
				return RecentPlayersManager._recentPlayers;
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0000DE14 File Offset: 0x0000C014
		public static async void Initialize()
		{
			await RecentPlayersManager.LoadRecentPlayers();
			RecentPlayersManager.DecayPlayers();
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0000DE48 File Offset: 0x0000C048
		private static async Task LoadRecentPlayers()
		{
			if (RecentPlayersManager.IsRecentPlayersCacheDirty)
			{
				if (Common.PlatformFileHelper.FileExists(RecentPlayersManager.RecentPlayerFilePath))
				{
					try
					{
						TaskAwaiter<string> taskAwaiter = FileHelper.GetFileContentStringAsync(RecentPlayersManager.RecentPlayerFilePath).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
						}
						RecentPlayersManager._recentPlayers = JsonConvert.DeserializeObject<MBList<RecentPlayerInfo>>(taskAwaiter.GetResult());
						if (RecentPlayersManager._recentPlayers == null)
						{
							RecentPlayersManager._recentPlayers = new MBList<RecentPlayerInfo>();
							throw new Exception("_recentPlayers were null.");
						}
					}
					catch (Exception ex)
					{
						Debug.FailedAssert("Could not recent players. " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\RecentPlayersManager.cs", "LoadRecentPlayers", 83);
						try
						{
							FileHelper.DeleteFile(RecentPlayersManager.RecentPlayerFilePath);
						}
						catch (Exception ex2)
						{
							Debug.FailedAssert("Could not delete recent players file. " + ex2.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\RecentPlayersManager.cs", "LoadRecentPlayers", 90);
						}
					}
				}
				RecentPlayersManager.IsRecentPlayersCacheDirty = false;
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0000DE88 File Offset: 0x0000C088
		public static async Task<MBReadOnlyList<RecentPlayerInfo>> GetRecentPlayerInfos()
		{
			await RecentPlayersManager.LoadRecentPlayers();
			return RecentPlayersManager.RecentPlayers;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0000DEC5 File Offset: 0x0000C0C5
		public static PlayerId[] GetRecentPlayerIds()
		{
			return RecentPlayersManager._recentPlayers.Select<RecentPlayerInfo, PlayerId>((RecentPlayerInfo p) => PlayerId.FromString(p.PlayerId)).ToArray<PlayerId>();
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		public static void AddOrUpdatePlayerEntry(PlayerId playerId, string playerName, InteractionType interactionType, int forcedIndex)
		{
			if (forcedIndex == -1)
			{
				object lockObject = RecentPlayersManager._lockObject;
				lock (lockObject)
				{
					RecentPlayersManager.InteractionTypeInfo interactionTypeInfo = RecentPlayersManager.InteractionTypeScoreDictionary[interactionType];
					RecentPlayerInfo recentPlayerInfo = RecentPlayersManager.TryGetPlayer(playerId);
					if (recentPlayerInfo != null)
					{
						if (interactionTypeInfo.ProcessType == RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Cumulative)
						{
							recentPlayerInfo.ImportanceScore += interactionTypeInfo.Score;
						}
						else if (interactionTypeInfo.ProcessType == RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Fixed)
						{
							recentPlayerInfo.ImportanceScore += Math.Max(interactionTypeInfo.Score, recentPlayerInfo.ImportanceScore);
						}
						recentPlayerInfo.PlayerName = playerName;
						recentPlayerInfo.InteractionTime = DateTime.Now;
					}
					else
					{
						recentPlayerInfo = new RecentPlayerInfo();
						recentPlayerInfo.PlayerId = playerId.ToString();
						recentPlayerInfo.ImportanceScore = interactionTypeInfo.Score;
						recentPlayerInfo.InteractionTime = DateTime.Now;
						recentPlayerInfo.PlayerName = playerName;
						RecentPlayersManager._recentPlayers.Add(recentPlayerInfo);
					}
					Action<PlayerId, InteractionType> onRecentPlayerInteraction = RecentPlayersManager.OnRecentPlayerInteraction;
					if (onRecentPlayerInteraction != null)
					{
						onRecentPlayerInteraction(playerId, interactionType);
					}
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600098C RID: 2444 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		// (remove) Token: 0x0600098D RID: 2445 RVA: 0x0000E030 File Offset: 0x0000C230
		public static event Action<PlayerId, InteractionType> OnRecentPlayerInteraction;

		// Token: 0x0600098E RID: 2446 RVA: 0x0000E064 File Offset: 0x0000C264
		private static void DecayPlayers()
		{
			object lockObject = RecentPlayersManager._lockObject;
			lock (lockObject)
			{
				List<RecentPlayerInfo> list = new List<RecentPlayerInfo>();
				DateTime now = DateTime.Now;
				foreach (RecentPlayerInfo recentPlayerInfo in RecentPlayersManager._recentPlayers)
				{
					recentPlayerInfo.ImportanceScore -= (int)(now - recentPlayerInfo.InteractionTime).TotalHours;
					if (recentPlayerInfo.ImportanceScore <= 0)
					{
						list.Add(recentPlayerInfo);
					}
				}
				foreach (RecentPlayerInfo recentPlayerInfo2 in list)
				{
					RecentPlayersManager._recentPlayers.Remove(recentPlayerInfo2);
				}
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0000E160 File Offset: 0x0000C360
		public static void TrimPlayers()
		{
			if (RecentPlayersManager._recentPlayers.Count > 200)
			{
				object lockObject = RecentPlayersManager._lockObject;
				lock (lockObject)
				{
					List<RecentPlayerInfo> list = RecentPlayersManager._recentPlayers.OrderByDescending<RecentPlayerInfo, int>((RecentPlayerInfo p) => p.ImportanceScore).Take<RecentPlayerInfo>(160).ToList<RecentPlayerInfo>();
					RecentPlayersManager._recentPlayers.Clear();
					RecentPlayersManager._recentPlayers.AddRange(list);
				}
			}
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0000E1F8 File Offset: 0x0000C3F8
		public static void Serialize()
		{
			try
			{
				byte[] array = Common.SerializeObjectAsJson(RecentPlayersManager._recentPlayers);
				FileHelper.SaveFile(RecentPlayersManager.RecentPlayerFilePath, array);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0000E238 File Offset: 0x0000C438
		public static IEnumerable<PlayerId> GetPlayersOrdered()
		{
			return from p in RecentPlayersManager._recentPlayers
				orderby p.InteractionTime descending
				select PlayerId.FromString(p.PlayerId);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0000E294 File Offset: 0x0000C494
		private static RecentPlayerInfo TryGetPlayer(PlayerId playerId)
		{
			string text = playerId.ToString();
			foreach (RecentPlayerInfo recentPlayerInfo in RecentPlayersManager._recentPlayers)
			{
				if (recentPlayerInfo.PlayerId == text)
				{
					return recentPlayerInfo;
				}
			}
			return null;
		}

		// Token: 0x040003FE RID: 1022
		private const string RecentPlayersDirectoryName = "Data";

		// Token: 0x040003FF RID: 1023
		private const string RecentPlayersFileName = "RecentPlayers.json";

		// Token: 0x04000400 RID: 1024
		private const int MaxRecentPlayersSize = 200;

		// Token: 0x04000401 RID: 1025
		private const int DownsizedRecentPlayersSize = 160;

		// Token: 0x04000402 RID: 1026
		private static bool IsRecentPlayersCacheDirty = true;

		// Token: 0x04000403 RID: 1027
		private static readonly object _lockObject = new object();

		// Token: 0x04000404 RID: 1028
		private static MBList<RecentPlayerInfo> _recentPlayers = new MBList<RecentPlayerInfo>();

		// Token: 0x04000405 RID: 1029
		private static readonly Dictionary<InteractionType, RecentPlayersManager.InteractionTypeInfo> InteractionTypeScoreDictionary = new Dictionary<InteractionType, RecentPlayersManager.InteractionTypeInfo>
		{
			{
				InteractionType.Killed,
				new RecentPlayersManager.InteractionTypeInfo(5, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Cumulative)
			},
			{
				InteractionType.KilledBy,
				new RecentPlayersManager.InteractionTypeInfo(5, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Cumulative)
			},
			{
				InteractionType.InGameTogether,
				new RecentPlayersManager.InteractionTypeInfo(24, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Fixed)
			},
			{
				InteractionType.InPartyTogether,
				new RecentPlayersManager.InteractionTypeInfo(48, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Fixed)
			}
		};

		// Token: 0x020001CD RID: 461
		private class InteractionTypeInfo
		{
			// Token: 0x17000361 RID: 865
			// (get) Token: 0x06000B4C RID: 2892 RVA: 0x000163BA File Offset: 0x000145BA
			// (set) Token: 0x06000B4D RID: 2893 RVA: 0x000163C2 File Offset: 0x000145C2
			public int Score { get; private set; }

			// Token: 0x17000362 RID: 866
			// (get) Token: 0x06000B4E RID: 2894 RVA: 0x000163CB File Offset: 0x000145CB
			// (set) Token: 0x06000B4F RID: 2895 RVA: 0x000163D3 File Offset: 0x000145D3
			public RecentPlayersManager.InteractionTypeInfo.InteractionProcessType ProcessType { get; private set; }

			// Token: 0x06000B50 RID: 2896 RVA: 0x000163DC File Offset: 0x000145DC
			public InteractionTypeInfo(int score, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType type)
			{
				this.Score = score;
				this.ProcessType = type;
			}

			// Token: 0x020001DC RID: 476
			public enum InteractionProcessType
			{
				// Token: 0x040006E8 RID: 1768
				Cumulative,
				// Token: 0x040006E9 RID: 1769
				Fixed
			}
		}
	}
}
