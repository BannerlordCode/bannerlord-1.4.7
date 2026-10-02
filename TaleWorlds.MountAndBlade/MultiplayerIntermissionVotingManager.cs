using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000308 RID: 776
	public class MultiplayerIntermissionVotingManager
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000A9CEC File Offset: 0x000A7EEC
		public static MultiplayerIntermissionVotingManager Instance
		{
			get
			{
				MultiplayerIntermissionVotingManager multiplayerIntermissionVotingManager;
				if ((multiplayerIntermissionVotingManager = MultiplayerIntermissionVotingManager._instance) == null)
				{
					multiplayerIntermissionVotingManager = (MultiplayerIntermissionVotingManager._instance = new MultiplayerIntermissionVotingManager());
				}
				return multiplayerIntermissionVotingManager;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000A9D02 File Offset: 0x000A7F02
		// (set) Token: 0x06002C4D RID: 11341 RVA: 0x000A9D0A File Offset: 0x000A7F0A
		public List<IntermissionVoteItem> MapVoteItems { get; private set; }

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x000A9D13 File Offset: 0x000A7F13
		// (set) Token: 0x06002C4F RID: 11343 RVA: 0x000A9D1B File Offset: 0x000A7F1B
		public List<IntermissionVoteItem> CultureVoteItems { get; private set; }

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x000A9D24 File Offset: 0x000A7F24
		// (set) Token: 0x06002C51 RID: 11345 RVA: 0x000A9D2C File Offset: 0x000A7F2C
		public List<CustomGameUsableMap> UsableMaps { get; private set; }

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06002C52 RID: 11346 RVA: 0x000A9D38 File Offset: 0x000A7F38
		// (remove) Token: 0x06002C53 RID: 11347 RVA: 0x000A9D70 File Offset: 0x000A7F70
		public event MultiplayerIntermissionVotingManager.MapItemAddedDelegate OnMapItemAdded;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06002C54 RID: 11348 RVA: 0x000A9DA8 File Offset: 0x000A7FA8
		// (remove) Token: 0x06002C55 RID: 11349 RVA: 0x000A9DE0 File Offset: 0x000A7FE0
		public event MultiplayerIntermissionVotingManager.CultureItemAddedDelegate OnCultureItemAdded;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06002C56 RID: 11350 RVA: 0x000A9E18 File Offset: 0x000A8018
		// (remove) Token: 0x06002C57 RID: 11351 RVA: 0x000A9E50 File Offset: 0x000A8050
		public event MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate OnMapItemVoteCountChanged;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06002C58 RID: 11352 RVA: 0x000A9E88 File Offset: 0x000A8088
		// (remove) Token: 0x06002C59 RID: 11353 RVA: 0x000A9EC0 File Offset: 0x000A80C0
		public event MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate OnCultureItemVoteCountChanged;

		// Token: 0x06002C5A RID: 11354 RVA: 0x000A9EF8 File Offset: 0x000A80F8
		public MultiplayerIntermissionVotingManager()
		{
			this.MapVoteItems = new List<IntermissionVoteItem>();
			this.CultureVoteItems = new List<IntermissionVoteItem>();
			this.UsableMaps = new List<CustomGameUsableMap>();
			this._votesOfPlayers = new Dictionary<PlayerId, List<string>>();
			this.IsMapVoteEnabled = true;
			this.IsCultureVoteEnabled = true;
			this.IsDisableMapVoteOverride = false;
			this.IsDisableCultureVoteOverride = false;
			this.IsMapSelectedByAdmin = false;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000A9F5C File Offset: 0x000A815C
		public void AddMapItem(string mapID)
		{
			if (!this.MapVoteItems.ContainsItem(mapID))
			{
				IntermissionVoteItem intermissionVoteItem = this.MapVoteItems.Add(mapID);
				MultiplayerIntermissionVotingManager.MapItemAddedDelegate onMapItemAdded = this.OnMapItemAdded;
				if (onMapItemAdded != null)
				{
					onMapItemAdded(intermissionVoteItem.Id);
				}
				this.SortVotesAndPickBest();
			}
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000A9FA1 File Offset: 0x000A81A1
		public void AddUsableMap(CustomGameUsableMap usableMap)
		{
			this.UsableMaps.Add(usableMap);
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000A9FB0 File Offset: 0x000A81B0
		public List<string> GetUsableMaps(string gameType)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.UsableMaps.Count; i++)
			{
				if (this.UsableMaps[i].IsCompatibleWithAllGameTypes || this.UsableMaps[i].CompatibleGameTypes.Contains(gameType))
				{
					list.Add(this.UsableMaps[i].Map);
				}
			}
			return list;
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000AA020 File Offset: 0x000A8220
		public void AddCultureItem(string cultureID)
		{
			if (!this.CultureVoteItems.ContainsItem(cultureID))
			{
				IntermissionVoteItem intermissionVoteItem = this.CultureVoteItems.Add(cultureID);
				MultiplayerIntermissionVotingManager.CultureItemAddedDelegate onCultureItemAdded = this.OnCultureItemAdded;
				if (onCultureItemAdded != null)
				{
					onCultureItemAdded(intermissionVoteItem.Id);
				}
				this.SortVotesAndPickBest();
			}
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000AA068 File Offset: 0x000A8268
		public void AddVote(PlayerId voterID, string itemID, int voteCount)
		{
			if (this.MapVoteItems.ContainsItem(itemID))
			{
				IntermissionVoteItem item = this.MapVoteItems.GetItem(itemID);
				item.IncreaseVoteCount(voteCount);
				MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate onMapItemVoteCountChanged = this.OnMapItemVoteCountChanged;
				if (onMapItemVoteCountChanged != null)
				{
					onMapItemVoteCountChanged(item.Index, item.VoteCount);
				}
			}
			else if (this.CultureVoteItems.ContainsItem(itemID))
			{
				IntermissionVoteItem item2 = this.CultureVoteItems.GetItem(itemID);
				item2.IncreaseVoteCount(voteCount);
				MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate onCultureItemVoteCountChanged = this.OnCultureItemVoteCountChanged;
				if (onCultureItemVoteCountChanged != null)
				{
					onCultureItemVoteCountChanged(item2.Index, item2.VoteCount);
				}
			}
			else
			{
				Debug.FailedAssert("Item with ID does not exist.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerIntermissionVotingManager.cs", "AddVote", 120);
			}
			if (!this._votesOfPlayers.ContainsKey(voterID))
			{
				this._votesOfPlayers.Add(voterID, new List<string>());
			}
			if (voteCount == 1)
			{
				this._votesOfPlayers[voterID].Add(itemID);
			}
			else if (voteCount == -1)
			{
				this._votesOfPlayers[voterID].Remove(itemID);
			}
			this.SortVotesAndPickBest();
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000AA161 File Offset: 0x000A8361
		public void SetVotesOfMap(int mapItemIndex, int voteCount)
		{
			this.MapVoteItems[mapItemIndex].SetVoteCount(voteCount);
			MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate onMapItemVoteCountChanged = this.OnMapItemVoteCountChanged;
			if (onMapItemVoteCountChanged == null)
			{
				return;
			}
			onMapItemVoteCountChanged(mapItemIndex, voteCount);
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000AA187 File Offset: 0x000A8387
		public void SetVotesOfCulture(int cultureItemIndex, int voteCount)
		{
			this.CultureVoteItems[cultureItemIndex].SetVoteCount(voteCount);
			MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate onCultureItemVoteCountChanged = this.OnCultureItemVoteCountChanged;
			if (onCultureItemVoteCountChanged == null)
			{
				return;
			}
			onCultureItemVoteCountChanged(cultureItemIndex, voteCount);
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000AA1B0 File Offset: 0x000A83B0
		public void ClearVotes()
		{
			foreach (IntermissionVoteItem intermissionVoteItem in this.MapVoteItems)
			{
				intermissionVoteItem.SetVoteCount(0);
				MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate onMapItemVoteCountChanged = this.OnMapItemVoteCountChanged;
				if (onMapItemVoteCountChanged != null)
				{
					onMapItemVoteCountChanged(intermissionVoteItem.Index, intermissionVoteItem.VoteCount);
				}
			}
			foreach (IntermissionVoteItem intermissionVoteItem2 in this.CultureVoteItems)
			{
				intermissionVoteItem2.SetVoteCount(0);
				MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate onCultureItemVoteCountChanged = this.OnCultureItemVoteCountChanged;
				if (onCultureItemVoteCountChanged != null)
				{
					onCultureItemVoteCountChanged(intermissionVoteItem2.Index, intermissionVoteItem2.VoteCount);
				}
			}
			this._votesOfPlayers.Clear();
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000AA28C File Offset: 0x000A848C
		public void ClearItems()
		{
			this.MapVoteItems.Clear();
			this.CultureVoteItems.Clear();
			this._votesOfPlayers.Clear();
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000AA2AF File Offset: 0x000A84AF
		public bool IsCultureItem(string itemID)
		{
			return this.CultureVoteItems.ContainsItem(itemID);
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000AA2BD File Offset: 0x000A84BD
		public bool IsMapItem(string itemID)
		{
			return this.MapVoteItems.ContainsItem(itemID);
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000AA2CC File Offset: 0x000A84CC
		public void HandlePlayerDisconnect(PlayerId playerID)
		{
			if (this._votesOfPlayers.ContainsKey(playerID))
			{
				foreach (string text in this._votesOfPlayers[playerID].ToList<string>())
				{
					this.AddVote(playerID, text, -1);
				}
				this._votesOfPlayers.Remove(playerID);
			}
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000AA348 File Offset: 0x000A8548
		public void SelectRandomCultures(MultiplayerOptions.MultiplayerOptionsAccessMode accessMode)
		{
			string[] array = new string[] { "khuzait", "aserai", "battania", "vlandia", "sturgia", "empire" };
			Random random = new Random();
			string text = array[random.Next(0, array.Length)];
			string text2 = array[random.Next(0, array.Length)];
			MultiplayerOptions.OptionType.CultureTeam1.SetValue(text, accessMode);
			MultiplayerOptions.OptionType.CultureTeam2.SetValue(text2, accessMode);
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000AA3BE File Offset: 0x000A85BE
		public bool IsPeerVotedForItem(NetworkCommunicator peer, string itemID)
		{
			return this._votesOfPlayers.ContainsKey(peer.VirtualPlayer.Id) && this._votesOfPlayers[peer.VirtualPlayer.Id].Contains(itemID);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000AA3F8 File Offset: 0x000A85F8
		public void SortVotesAndPickBest()
		{
			if (GameNetwork.IsServer)
			{
				if (this.IsMapVoteEnabled)
				{
					List<IntermissionVoteItem> list = this.MapVoteItems.ToList<IntermissionVoteItem>();
					if (list.Count > 1)
					{
						list.Sort((IntermissionVoteItem m1, IntermissionVoteItem m2) => -m1.VoteCount.CompareTo(m2.VoteCount));
						string text = list[0].Id;
						if (list[0].VoteCount <= 0)
						{
							Random random = new Random();
							text = list[random.Next(0, list.Count)].Id;
						}
						MultiplayerOptions.OptionType.Map.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
					else if (list.Count == 1)
					{
						MultiplayerOptions.OptionType.Map.SetValue(list[0].Id, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
				}
				if (this.IsCultureVoteEnabled)
				{
					List<IntermissionVoteItem> list2 = this.CultureVoteItems.ToList<IntermissionVoteItem>();
					if (list2.Count > 2)
					{
						list2.Sort((IntermissionVoteItem c1, IntermissionVoteItem c2) => -c1.VoteCount.CompareTo(c2.VoteCount));
						string id = list2[0].Id;
						string text2 = list2[1].Id;
						if (list2[0].VoteCount > 0)
						{
							if (10 * list2[0].VoteCount >= 7 * list2.Select<IntermissionVoteItem, int>((IntermissionVoteItem item) => item.VoteCount).Sum())
							{
								text2 = list2[0].Id;
							}
							MultiplayerOptions.OptionType.CultureTeam1.SetValue(id, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
							MultiplayerOptions.OptionType.CultureTeam2.SetValue(text2, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
							return;
						}
						this.SelectRandomCultures(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
				}
			}
		}

		// Token: 0x04001181 RID: 4481
		public const int MaxAllowedMapCount = 100;

		// Token: 0x04001182 RID: 4482
		private static MultiplayerIntermissionVotingManager _instance;

		// Token: 0x04001183 RID: 4483
		public bool IsAutomatedBattleSwitchingEnabled;

		// Token: 0x04001184 RID: 4484
		public bool IsMapVoteEnabled;

		// Token: 0x04001185 RID: 4485
		public bool IsCultureVoteEnabled;

		// Token: 0x04001186 RID: 4486
		public bool IsDisableMapVoteOverride;

		// Token: 0x04001187 RID: 4487
		public bool IsDisableCultureVoteOverride;

		// Token: 0x04001188 RID: 4488
		public bool IsMapSelectedByAdmin;

		// Token: 0x04001189 RID: 4489
		public string InitialGameType;

		// Token: 0x0400118D RID: 4493
		private readonly Dictionary<PlayerId, List<string>> _votesOfPlayers;

		// Token: 0x0400118E RID: 4494
		public MultiplayerIntermissionState CurrentVoteState;

		// Token: 0x020005E9 RID: 1513
		// (Invoke) Token: 0x06003F09 RID: 16137
		public delegate void MapItemAddedDelegate(string mapId);

		// Token: 0x020005EA RID: 1514
		// (Invoke) Token: 0x06003F0D RID: 16141
		public delegate void CultureItemAddedDelegate(string cultureId);

		// Token: 0x020005EB RID: 1515
		// (Invoke) Token: 0x06003F11 RID: 16145
		public delegate void MapItemVoteCountChangedDelegate(int mapItemIndex, int voteCount);

		// Token: 0x020005EC RID: 1516
		// (Invoke) Token: 0x06003F15 RID: 16149
		public delegate void CultureItemVoteCountChangedDelegate(int cultureItemIndex, int voteCount);
	}
}
