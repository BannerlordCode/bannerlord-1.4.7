using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200013E RID: 318
	public static class PermaMuteList
	{
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0000C6DE File Offset: 0x0000A8DE
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0000C6E5 File Offset: 0x0000A8E5
		public static bool HasMutedPlayersLoaded { get; private set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
		private static PlatformFilePath PermaMuteFilePath
		{
			get
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Data");
				return new PlatformFilePath(platformDirectoryPath, "Muted.json");
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0000C718 File Offset: 0x0000A918
		[TupleElementNames(new string[] { "Id", "Name" })]
		public static IReadOnlyList<ValueTuple<string, string>> MutedPlayers
		{
			[return: TupleElementNames(new string[] { "Id", "Name" })]
			get
			{
				List<ValueTuple<string, string>> list;
				if (!PermaMuteList.HasMutedPlayersLoaded || !PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
				{
					return new List<ValueTuple<string, string>>();
				}
				return list;
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0000C752 File Offset: 0x0000A952
		public static void SetPermanentMuteAvailableCallback(Func<bool> getPermanentMuteAvailable)
		{
			PermaMuteList._getPermanentMuteAvailable = getPermanentMuteAvailable;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0000C75C File Offset: 0x0000A95C
		public static async Task LoadMutedPlayers(PlayerId currentPlayerId)
		{
			PermaMuteList.CurrentPlayerId = currentPlayerId.ToString();
			if (FileHelper.FileExists(PermaMuteList.PermaMuteFilePath))
			{
				try
				{
					Dictionary<string, List<ValueTuple<string, string>>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<ValueTuple<string, string>>>>(await FileHelper.GetFileContentStringAsync(PermaMuteList.PermaMuteFilePath));
					if (dictionary != null)
					{
						PermaMuteList._mutedPlayers = dictionary;
					}
					PermaMuteList.HasMutedPlayersLoaded = true;
				}
				catch (Exception ex)
				{
					Debug.FailedAssert("Could not load muted players. " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\PermaMuteList.cs", "LoadMutedPlayers", 61);
					try
					{
						FileHelper.DeleteFile(PermaMuteList.PermaMuteFilePath);
					}
					catch (Exception ex2)
					{
						Debug.FailedAssert("Could not delete muted players file. " + ex2.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\PermaMuteList.cs", "LoadMutedPlayers", 68);
					}
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000C7A4 File Offset: 0x0000A9A4
		public static async void SaveMutedPlayers()
		{
			try
			{
				byte[] array = Common.SerializeObjectAsJson(PermaMuteList._mutedPlayers);
				await FileHelper.SaveFileAsync(PermaMuteList.PermaMuteFilePath, array);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Could not save muted players. " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\PermaMuteList.cs", "SaveMutedPlayers", 83);
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		public static bool IsPlayerMuted(PlayerId player)
		{
			Func<bool> getPermanentMuteAvailable = PermaMuteList._getPermanentMuteAvailable;
			if ((getPermanentMuteAvailable == null || getPermanentMuteAvailable()) && PermaMuteList.CurrentPlayerId != null)
			{
				string text = player.ToString();
				Dictionary<string, List<ValueTuple<string, string>>> mutedPlayers = PermaMuteList._mutedPlayers;
				lock (mutedPlayers)
				{
					List<ValueTuple<string, string>> list;
					if (!PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
					{
						return false;
					}
					using (List<ValueTuple<string, string>>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Item1 == text)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		public static void MutePlayer(PlayerId player, string name)
		{
			Func<bool> getPermanentMuteAvailable = PermaMuteList._getPermanentMuteAvailable;
			if (getPermanentMuteAvailable == null || getPermanentMuteAvailable())
			{
				Dictionary<string, List<ValueTuple<string, string>>> mutedPlayers = PermaMuteList._mutedPlayers;
				lock (mutedPlayers)
				{
					List<ValueTuple<string, string>> list;
					if (!PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
					{
						list = new List<ValueTuple<string, string>>();
						PermaMuteList._mutedPlayers.Add(PermaMuteList.CurrentPlayerId, list);
					}
					list.Add(new ValueTuple<string, string>(player.ToString(), name));
				}
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000C930 File Offset: 0x0000AB30
		public static void RemoveMutedPlayer(PlayerId player)
		{
			Func<bool> getPermanentMuteAvailable = PermaMuteList._getPermanentMuteAvailable;
			if (getPermanentMuteAvailable == null || getPermanentMuteAvailable())
			{
				string text = player.ToString();
				Dictionary<string, List<ValueTuple<string, string>>> mutedPlayers = PermaMuteList._mutedPlayers;
				lock (mutedPlayers)
				{
					List<ValueTuple<string, string>> list;
					if (PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
					{
						int num = -1;
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].Item1 == text)
							{
								num = i;
								break;
							}
						}
						if (num >= 0)
						{
							list.RemoveAt(num);
						}
					}
				}
			}
		}

		// Token: 0x0400039B RID: 923
		[TupleElementNames(new string[] { "Id", "Name" })]
		private static Dictionary<string, List<ValueTuple<string, string>>> _mutedPlayers = new Dictionary<string, List<ValueTuple<string, string>>>();

		// Token: 0x0400039C RID: 924
		private static string CurrentPlayerId;

		// Token: 0x0400039D RID: 925
		private static Func<bool> _getPermanentMuteAvailable;
	}
}
