using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000032 RID: 50
	public class MPMatchmakingVM : ViewModel
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000FC8F File Offset: 0x0000DE8F
		public MPMatchmakingVM.MatchmakingSubPages CurrentSubPage
		{
			get
			{
				return this._currentSubPage;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000FC97 File Offset: 0x0000DE97
		private bool IsServerQuickPlayAvailable
		{
			get
			{
				return NetworkMain.GameClient.IsAbleToSearchForGame;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000FCA3 File Offset: 0x0000DEA3
		private bool IsServerCustomGameListAvailable
		{
			get
			{
				return NetworkMain.GameClient.IsCustomBattleAvailable && !this.IsFindingMatch;
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000FCBC File Offset: 0x0000DEBC
		public MPMatchmakingVM(LobbyState lobbyState, Action<MPLobbyVM.LobbyPage> onChangePageRequest, Action<string, bool> onMatchSelectionChanged, Action<bool> onGameFindRequestStateChanged)
		{
			this._lobbyState = lobbyState;
			this._onChangePageRequest = onChangePageRequest;
			this._onMatchSelectionChanged = onMatchSelectionChanged;
			this._onGameFindRequestStateChanged = onGameFindRequestStateChanged;
			this.HasUnofficialModulesLoaded = NetworkMain.GameClient.HasUnofficialModulesLoaded;
			this.RankedGameTypes = new MBBindingList<MPMatchmakingItemVM>();
			this.CustomGameTypes = new MBBindingList<MPMatchmakingItemVM>();
			this.QuickplayGameTypes = new MBBindingList<MPMatchmakingItemVM>();
			this.CustomServer = new MPCustomGameVM(lobbyState, MPCustomGameVM.CustomGameMode.CustomServer);
			this.PremadeMatches = new MPCustomGameVM(lobbyState, MPCustomGameVM.CustomGameMode.PremadeGame);
			this.RefreshSubPageStates();
			this._selectionInfoTextObject = new TextObject("{=wuKqRvc3}Game: {GAME_TYPES}  |  Region: {REGIONS}", null);
			InformationManager.OnHideInquiry += this.OnHideInquiry;
			this._defaultSelectedGameTypes = MultiplayerMain.GetUserSelectedGameTypes();
			this.SelectionInfo = new MPMatchmakingSelectionInfoVM();
			this.UpdateQuickPlayGameTypeList();
			this.UpdateCustomGameTypeList();
			this._heroClasses = MultiplayerClassDivisions.GetMPHeroClasses();
			this.IsRanked = true;
			this.Regions = new SelectorVM<MPMatchmakingRegionSelectorItemVM>(0, null);
			this.RefreshValues();
			this.RefreshWaitingTime();
			this.OnSelectionChanged(true, true);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PlayText = new TextObject("{=wTtyFa89}PLAY", null).ToString();
			this.MatchFindNotPossibleText = new TextObject("{=BrYUHFsg}CHOOSE GAME", null).ToString();
			this.AutoFindText = new TextObject("{=S2bKbhTc}AUTO FIND GAME", null).ToString();
			this.RankedText = GameTexts.FindText("str_multiplayer_ranked", null).ToString();
			this.CasualText = new TextObject("{=GXosklej}Casual", null).ToString();
			this.RankedText = GameTexts.FindText("str_multiplayer_ranked", null).ToString();
			this.QuickPlayText = GameTexts.FindText("str_multiplayer_quick_play", null).ToString();
			this.CustomGameText = GameTexts.FindText("str_multiplayer_custom_game", null).ToString();
			this.CustomServerListText = GameTexts.FindText("str_multiplayer_custom_server_list", null).ToString();
			this.TeamMatchesText = new TextObject("{=PE5LqC9O}Team Matches", null).ToString();
			this.CommunityGameText = new TextObject("{=SIIgjILk}Community Games", null).ToString();
			this.RegionsHint = new HintViewModel(new TextObject("{=LzdUwRJo}Select a region for Quick Play and Custom Game", null), null);
			this.QuickplayGameTypes.ApplyActionOnAllItems(delegate(MPMatchmakingItemVM x)
			{
				x.RefreshValues();
			});
			this.RankedGameTypes.ApplyActionOnAllItems(delegate(MPMatchmakingItemVM x)
			{
				x.RefreshValues();
			});
			this.CustomGameTypes.ApplyActionOnAllItems(delegate(MPMatchmakingItemVM x)
			{
				x.RefreshValues();
			});
			this.CustomServer.RefreshValues();
			this.PremadeMatches.RefreshValues();
			this._regionsRequireRefresh = true;
			this.Regions.RefreshValues();
			this.SelectionInfo.RefreshValues();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000FF84 File Offset: 0x0000E184
		private void RefreshRegionsList()
		{
			string currentRegion = MultiplayerMain.GetUserCurrentRegion();
			string[] availableMatchmakerRegions = MultiplayerMain.GetAvailableMatchmakerRegions();
			List<MPMatchmakingRegionSelectorItemVM> list = new List<MPMatchmakingRegionSelectorItemVM>();
			if (this._isTestRegionAvailable)
			{
				MPMatchmakingRegionSelectorItemVM mpmatchmakingRegionSelectorItemVM = new MPMatchmakingRegionSelectorItemVM("Test", new TextObject("{=!}Test", null));
				list.Add(mpmatchmakingRegionSelectorItemVM);
			}
			foreach (string text in availableMatchmakerRegions)
			{
				TextObject textObject = GameTexts.FindText("str_multiplayer_region_name", text);
				list.Add(new MPMatchmakingRegionSelectorItemVM(text, textObject));
			}
			list.Add(new MPMatchmakingRegionSelectorItemVM("None", GameTexts.FindText("str_multiplayer_region_name_none", null)));
			int num = list.FindIndex((MPMatchmakingRegionSelectorItemVM r) => r.RegionCode == currentRegion);
			int num2;
			if (num == -1)
			{
				num2 = list.FindIndex((MPMatchmakingRegionSelectorItemVM r) => r.IsRegionNone);
			}
			else
			{
				num2 = num;
			}
			int num3 = num2;
			this.Regions.Refresh(list, num3, new Action<SelectorVM<MPMatchmakingRegionSelectorItemVM>>(this.OnRegionSelectionChanged));
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00010080 File Offset: 0x0000E280
		internal void OnTick(float dt)
		{
			this.CustomServer.OnTick(dt);
			this.PremadeMatches.OnTick(dt);
			if (this._regionsRequireRefresh)
			{
				this.RefreshRegionsList();
				this._regionsRequireRefresh = false;
				this.OnSelectionChanged(true, true);
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000100B8 File Offset: 0x0000E2B8
		public void TrySetMatchmakingSubPage(MPMatchmakingVM.MatchmakingSubPages newPage)
		{
			if (this._currentSubPage != newPage)
			{
				if ((newPage == MPMatchmakingVM.MatchmakingSubPages.CustomGameList || newPage == MPMatchmakingVM.MatchmakingSubPages.CustomGame) && !this.IsServerCustomGameListAvailable)
				{
					return;
				}
				if (newPage == MPMatchmakingVM.MatchmakingSubPages.QuickPlay && !this.IsServerQuickPlayAvailable)
				{
					return;
				}
				if (newPage == MPMatchmakingVM.MatchmakingSubPages.Default)
				{
					if (this.IsServerQuickPlayAvailable && !this.HasUnofficialModulesLoaded)
					{
						newPage = MPMatchmakingVM.MatchmakingSubPages.QuickPlay;
					}
					else
					{
						if (!this.IsServerCustomGameListAvailable)
						{
							Debug.FailedAssert("Trying to open matchmaking when nothing is available", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\OfficialGame\\MPMatchmakingVM.cs", "TrySetMatchmakingSubPage", 182);
							return;
						}
						newPage = MPMatchmakingVM.MatchmakingSubPages.CustomGameList;
					}
				}
				this._currentSubPage = newPage;
				this.SelectedSubPageIndex = (int)newPage;
				this.CustomServer.IsEnabled = newPage == MPMatchmakingVM.MatchmakingSubPages.CustomGameList;
				this.PremadeMatches.IsEnabled = newPage == MPMatchmakingVM.MatchmakingSubPages.PremadeMatchList;
				this.IsCustomGameStageFindEnabled = this.CustomGameTypes.Any<MPMatchmakingItemVM>((MPMatchmakingItemVM g) => g.IsSelected);
				this.OnSelectionChanged(false, false);
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00010194 File Offset: 0x0000E394
		public void RefreshPlayerData(PlayerData playerData)
		{
			if (!string.IsNullOrEmpty(playerData.LastRegion))
			{
				for (int i = 0; i < this.Regions.ItemList.Count; i++)
				{
					if (playerData.LastRegion == this.Regions.ItemList[i].RegionCode)
					{
						this.Regions.SelectedIndex = i;
						break;
					}
				}
			}
			else
			{
				this.Regions.SelectedIndex = this.Regions.ItemList.FindIndex<MPMatchmakingRegionSelectorItemVM>((MPMatchmakingRegionSelectorItemVM r) => r.IsRegionNone);
			}
			string[] lastGameTypes = playerData.LastGameTypes;
			if (lastGameTypes != null)
			{
				using (IEnumerator<MPMatchmakingItemVM> enumerator = this.QuickplayGameTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MPMatchmakingItemVM mpmatchmakingItemVM = enumerator.Current;
						mpmatchmakingItemVM.IsSelected = lastGameTypes.Contains(mpmatchmakingItemVM.Type);
					}
					return;
				}
			}
			foreach (MPMatchmakingItemVM mpmatchmakingItemVM2 in this.QuickplayGameTypes)
			{
				mpmatchmakingItemVM2.IsSelected = false;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000102C8 File Offset: 0x0000E4C8
		private void GameModeOnSetFocusItem(MPMatchmakingItemVM sender)
		{
			this.SelectionInfo.UpdateForGameType(sender.Type);
			this.SelectionInfo.SetEnabled(true);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000102E7 File Offset: 0x0000E4E7
		private void GameModeOnRemoveFocus()
		{
			this.SelectionInfo.SetEnabled(false);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000102F5 File Offset: 0x0000E4F5
		private void GameModeOnSelectionChanged(MPMatchmakingItemVM sender, bool isSelected)
		{
			this.OnSelectionChanged(false, true);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000102FF File Offset: 0x0000E4FF
		private void OnRegionSelectionChanged(SelectorVM<MPMatchmakingRegionSelectorItemVM> selectorVM)
		{
			if (selectorVM.SelectedItem != null)
			{
				this.OnSelectionChanged(true, false);
				return;
			}
			this._regionsRequireRefresh = true;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001031C File Offset: 0x0000E51C
		private void OnSelectionChanged(bool updatedRegion = false, bool updatedGameTypes = false)
		{
			string[] array;
			bool selectedGameTypesInfo = this.GetSelectedGameTypesInfo(out array);
			this._selectionInfoTextObject.SetTextVariable("GAME_TYPES", MPLobbyVM.GetLocalizedGameTypesString(array));
			this.IsMatchFindPossible = this.SelectedSubPageIndex == 0 && selectedGameTypesInfo && NetworkMain.GameClient.IsAbleToSearchForGame;
			this.IsCustomGameStageFindEnabled = this.SelectedSubPageIndex == 1 && selectedGameTypesInfo;
			if (!this._regionsRequireRefresh && updatedRegion)
			{
				MPMatchmakingRegionSelectorItemVM selectedItem = this.Regions.SelectedItem;
				if (selectedItem != null && !selectedItem.IsRegionNone)
				{
					MPMatchmakingRegionSelectorItemVM selectedItem2 = this.Regions.SelectedItem;
					string text = ((selectedItem2 != null) ? selectedItem2.StringItem : null);
					this._selectionInfoTextObject.SetTextVariable("REGIONS", text);
					string regionCode = this.Regions.SelectedItem.RegionCode;
					NetworkMain.GameClient.ChangeRegion(regionCode);
				}
			}
			if (updatedGameTypes && this.IsMatchFindPossible)
			{
				NetworkMain.GameClient.ChangeGameTypes(array.ToArray<string>());
			}
			this.SelectionInfoText = this._selectionInfoTextObject.ToString();
			Action<string, bool> onMatchSelectionChanged = this._onMatchSelectionChanged;
			if (onMatchSelectionChanged == null)
			{
				return;
			}
			onMatchSelectionChanged(this.SelectionInfoText, this.IsMatchFindPossible);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00010434 File Offset: 0x0000E634
		internal void OnServerStatusReceived(ServerStatus serverStatus)
		{
			this._isServerStatusReceived = true;
			this.CustomServer.IsPlayerBasedCustomBattleEnabled = serverStatus.IsPlayerBasedCustomBattleEnabled;
			this.PremadeMatches.IsPremadeGameEnabled = serverStatus.IsPremadeGameEnabled;
			if (this._isTestRegionAvailable != serverStatus.IsTestRegionEnabled)
			{
				this._isTestRegionAvailable = serverStatus.IsTestRegionEnabled;
				this._regionsRequireRefresh = true;
			}
			this.RefreshSubPageStates();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00010491 File Offset: 0x0000E691
		public void OnFindingGame()
		{
			this.IsFindingMatch = true;
			this.RefreshSubPageStates();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000104A0 File Offset: 0x0000E6A0
		public void OnCancelFindingGame()
		{
			this.IsFindingMatch = false;
			this.RefreshSubPageStates();
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000104B0 File Offset: 0x0000E6B0
		public override void OnFinalize()
		{
			InformationManager.OnHideInquiry -= this.OnHideInquiry;
			this.CustomServer.OnFinalize();
			foreach (MPMatchmakingItemVM mpmatchmakingItemVM in this.RankedGameTypes)
			{
				mpmatchmakingItemVM.OnSelectionChanged -= this.GameModeOnSelectionChanged;
				mpmatchmakingItemVM.OnSetFocusItem -= this.GameModeOnSetFocusItem;
				mpmatchmakingItemVM.OnRemoveFocus -= this.GameModeOnRemoveFocus;
			}
			foreach (MPMatchmakingItemVM mpmatchmakingItemVM2 in this.QuickplayGameTypes)
			{
				mpmatchmakingItemVM2.OnSelectionChanged -= this.GameModeOnSelectionChanged;
				mpmatchmakingItemVM2.OnSetFocusItem -= this.GameModeOnSetFocusItem;
				mpmatchmakingItemVM2.OnRemoveFocus -= this.GameModeOnRemoveFocus;
			}
			foreach (MPMatchmakingItemVM mpmatchmakingItemVM3 in this.CustomGameTypes)
			{
				mpmatchmakingItemVM3.OnSelectionChanged -= this.GameModeOnSelectionChanged;
				mpmatchmakingItemVM3.OnSetFocusItem -= this.GameModeOnSetFocusItem;
				mpmatchmakingItemVM3.OnRemoveFocus -= this.GameModeOnRemoveFocus;
			}
			base.OnFinalize();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00010620 File Offset: 0x0000E820
		public bool GetSelectedGameTypesInfo(out string[] gameTypes)
		{
			List<string> list = new List<string>();
			bool flag = false;
			MBBindingList<MPMatchmakingItemVM> currentSubPageList = this.GetCurrentSubPageList();
			for (int i = 0; i < currentSubPageList.Count; i++)
			{
				MPMatchmakingItemVM mpmatchmakingItemVM = currentSubPageList[i];
				if (mpmatchmakingItemVM.IsSelected)
				{
					list.Add(mpmatchmakingItemVM.Type);
					flag = true;
				}
			}
			gameTypes = list.ToArray();
			return flag;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00010678 File Offset: 0x0000E878
		private MBBindingList<MPMatchmakingItemVM> GetCurrentSubPageList()
		{
			switch (this.SelectedSubPageIndex)
			{
			case 1:
				return this.CustomGameTypes;
			}
			return this.QuickplayGameTypes;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000106B5 File Offset: 0x0000E8B5
		private void OnHideInquiry()
		{
			if (this.IsFindingMatch)
			{
				this.ExecuteCancelFindingGame();
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000106C5 File Offset: 0x0000E8C5
		public void RefreshWaitingTime()
		{
			MBTextManager.SetTextVariable("WAIT_TIME", 10);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000106D4 File Offset: 0x0000E8D4
		public void ExecuteAutoFindGame()
		{
			if (!this.QuickplayGameTypes.Where<MPMatchmakingItemVM>((MPMatchmakingItemVM q) => q.IsSelected).Any<MPMatchmakingItemVM>())
			{
				for (int i = 0; i < this.QuickplayGameTypes.Count; i++)
				{
					this.QuickplayGameTypes[i].IsSelected = true;
				}
			}
			this.ExecuteFindGame();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00010740 File Offset: 0x0000E940
		private async void ExecuteFindGame()
		{
			if (!this.IsFindingMatch)
			{
				if (this.Regions.SelectedItem.IsRegionNone)
				{
					InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_multiplayer_no_region_query_title", null).ToString(), GameTexts.FindText("str_multiplayer_no_region_query_description", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", delegate
					{
						Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
						if (onChangePageRequest == null)
						{
							return;
						}
						onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
					}, null, "", 0f, null, null, null), false, false);
				}
				else
				{
					string[] array = (from q in this.GetCurrentSubPageList()
						where q.IsSelected
						select q.Type).ToArray<string>();
					if (array.Length != 0)
					{
						if (this.SelectedSubPageIndex == 1)
						{
							TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.FindCustomGame(array, this._lobbyState.HasCrossplayPrivilege, MultiplayerMain.GetUserCurrentRegion()).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (!taskAwaiter.GetResult())
							{
								InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=NaZ6xg33}Couldn't find an applicable server to join.", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
							}
						}
						else
						{
							Action<bool> onGameFindRequestStateChanged = this._onGameFindRequestStateChanged;
							if (onGameFindRequestStateChanged != null)
							{
								onGameFindRequestStateChanged(true);
							}
							PlatformServices.Instance.CheckPrivilege(Privilege.Crossplay, true, delegate(bool result)
							{
								Action<bool> onGameFindRequestStateChanged2 = this._onGameFindRequestStateChanged;
								if (onGameFindRequestStateChanged2 != null)
								{
									onGameFindRequestStateChanged2(false);
								}
								if (result)
								{
									NetworkMain.GameClient.FindGame();
								}
							});
						}
					}
				}
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001077C File Offset: 0x0000E97C
		public void RefreshSubPageStates()
		{
			if (this._isServerStatusReceived)
			{
				this.IsCustomServerListEnabled = this.IsServerCustomGameListAvailable;
				this.IsQuickplayGamesEnabled = this.IsServerQuickPlayAvailable && !this.HasUnofficialModulesLoaded;
				this.IsCustomGamesEnabled = this.IsServerCustomGameListAvailable && !this.HasUnofficialModulesLoaded;
				this.IsRankedGamesEnabled = false;
				this._isEligibleForPremadeMatches = NetworkMain.GameClient.IsEligibleToCreatePremadeGame;
				this.IsPremadeGamesEnabled = this._isEligibleForPremadeMatches && !this.IsFindingMatch && !this.HasUnofficialModulesLoaded;
				if ((this.CurrentSubPage == MPMatchmakingVM.MatchmakingSubPages.CustomGame || this.CurrentSubPage == MPMatchmakingVM.MatchmakingSubPages.CustomGameList) && !this.IsCustomServerListEnabled)
				{
					this.TrySetMatchmakingSubPage(MPMatchmakingVM.MatchmakingSubPages.Default);
					return;
				}
				if (this.CurrentSubPage == MPMatchmakingVM.MatchmakingSubPages.QuickPlay && !this.IsServerQuickPlayAvailable)
				{
					this.TrySetMatchmakingSubPage(MPMatchmakingVM.MatchmakingSubPages.Default);
					return;
				}
			}
			else
			{
				this.IsCustomServerListEnabled = false;
				this.IsQuickplayGamesEnabled = false;
				this.IsRankedGamesEnabled = false;
				this.IsCustomGamesEnabled = false;
				this.IsPremadeGamesEnabled = false;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001086A File Offset: 0x0000EA6A
		private void ExecuteCancelFindingGame()
		{
			if (!this.IsFindingMatch)
			{
				return;
			}
			this.OnCancelFindingGame();
			NetworkMain.GameClient.CancelFindGame();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00010888 File Offset: 0x0000EA88
		private void IsFindingMatchUpdated()
		{
			foreach (MPMatchmakingItemVM mpmatchmakingItemVM in this.RankedGameTypes)
			{
				mpmatchmakingItemVM.IsAvailable = !this.IsFindingMatch;
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000108DC File Offset: 0x0000EADC
		private void ExecuteChangeEnabledSubPage(int subpageIndex)
		{
			this.TrySetMatchmakingSubPage((MPMatchmakingVM.MatchmakingSubPages)subpageIndex);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000108E8 File Offset: 0x0000EAE8
		private void UpdateRankedGameTypesList()
		{
			MultiplayerGameType[] availableRankedGameModes = MultiplayerMain.GetAvailableRankedGameModes();
			for (int i = 0; i < availableRankedGameModes.Length; i++)
			{
				MPMatchmakingItemVM mpmatchmakingItemVM = new MPMatchmakingItemVM(availableRankedGameModes[i]);
				mpmatchmakingItemVM.IsSelected = this._defaultSelectedGameTypes.Contains(mpmatchmakingItemVM.Type);
				mpmatchmakingItemVM.OnSelectionChanged += this.GameModeOnSelectionChanged;
				mpmatchmakingItemVM.OnSetFocusItem += this.GameModeOnSetFocusItem;
				mpmatchmakingItemVM.OnRemoveFocus += this.GameModeOnRemoveFocus;
				this.RankedGameTypes.Add(mpmatchmakingItemVM);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001096C File Offset: 0x0000EB6C
		private void UpdateQuickPlayGameTypeList()
		{
			MultiplayerGameType[] availableQuickPlayGameModes = MultiplayerMain.GetAvailableQuickPlayGameModes();
			for (int i = 0; i < availableQuickPlayGameModes.Length; i++)
			{
				MPMatchmakingItemVM mpmatchmakingItemVM = new MPMatchmakingItemVM(availableQuickPlayGameModes[i]);
				mpmatchmakingItemVM.IsSelected = this._defaultSelectedGameTypes.Contains(mpmatchmakingItemVM.Type);
				mpmatchmakingItemVM.OnSelectionChanged += this.GameModeOnSelectionChanged;
				mpmatchmakingItemVM.OnSetFocusItem += this.GameModeOnSetFocusItem;
				mpmatchmakingItemVM.OnRemoveFocus += this.GameModeOnRemoveFocus;
				this.QuickplayGameTypes.Add(mpmatchmakingItemVM);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000109F0 File Offset: 0x0000EBF0
		private void UpdateCustomGameTypeList()
		{
			MultiplayerGameType[] availableCustomGameModes = MultiplayerMain.GetAvailableCustomGameModes();
			for (int i = 0; i < availableCustomGameModes.Length; i++)
			{
				MPMatchmakingItemVM mpmatchmakingItemVM = new MPMatchmakingItemVM(availableCustomGameModes[i]);
				mpmatchmakingItemVM.IsSelected = this._defaultSelectedGameTypes.Contains(mpmatchmakingItemVM.Type);
				mpmatchmakingItemVM.OnSelectionChanged += this.GameModeOnSelectionChanged;
				mpmatchmakingItemVM.OnSetFocusItem += this.GameModeOnSetFocusItem;
				mpmatchmakingItemVM.OnRemoveFocus += this.GameModeOnRemoveFocus;
				this.CustomGameTypes.Add(mpmatchmakingItemVM);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00010A73 File Offset: 0x0000EC73
		public void OnPremadeGameEligibilityStatusReceived(bool isEligible)
		{
			this._isEligibleForPremadeMatches = isEligible;
			this.IsPremadeGamesEnabled = this._isEligibleForPremadeMatches && !this.IsFindingMatch && !this.HasUnofficialModulesLoaded;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00010A9E File Offset: 0x0000EC9E
		public void OnSupportedFeaturesRefreshed(SupportedFeatures supportedFeatures)
		{
			this.IsCustomServerFeatureSupported = supportedFeatures.SupportsFeatures(Features.CustomGame);
			this.IsClansFeatureSupported = supportedFeatures.SupportsFeatures(Features.Clan);
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00010ABA File Offset: 0x0000ECBA
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x00010AC2 File Offset: 0x0000ECC2
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00010AE0 File Offset: 0x0000ECE0
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		[DataSourceProperty]
		public bool HasUnofficialModulesLoaded
		{
			get
			{
				return this._hasUnofficialModulesLoaded;
			}
			set
			{
				if (value != this._hasUnofficialModulesLoaded)
				{
					this._hasUnofficialModulesLoaded = value;
					base.OnPropertyChangedWithValue(value, "HasUnofficialModulesLoaded");
				}
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00010B06 File Offset: 0x0000ED06
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x00010B0E File Offset: 0x0000ED0E
		[DataSourceProperty]
		public bool IsCustomGameStageFindEnabled
		{
			get
			{
				return this._isCustomGameStageFindEnabled;
			}
			set
			{
				if (value != this._isCustomGameStageFindEnabled)
				{
					this._isCustomGameStageFindEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCustomGameStageFindEnabled");
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00010B2C File Offset: 0x0000ED2C
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x00010B34 File Offset: 0x0000ED34
		[DataSourceProperty]
		public bool IsRankedGamesEnabled
		{
			get
			{
				return this._isRankedGamesEnabled;
			}
			set
			{
				if (value != this._isRankedGamesEnabled)
				{
					this._isRankedGamesEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsRankedGamesEnabled");
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x00010B52 File Offset: 0x0000ED52
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x00010B5A File Offset: 0x0000ED5A
		[DataSourceProperty]
		public bool IsCustomGamesEnabled
		{
			get
			{
				return this._isCustomGamesEnabled;
			}
			set
			{
				if (value != this._isCustomGamesEnabled)
				{
					this._isCustomGamesEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCustomGamesEnabled");
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00010B78 File Offset: 0x0000ED78
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x00010B80 File Offset: 0x0000ED80
		[DataSourceProperty]
		public bool IsQuickplayGamesEnabled
		{
			get
			{
				return this._isQuickplayGamesEnabled;
			}
			set
			{
				if (value != this._isQuickplayGamesEnabled)
				{
					this._isQuickplayGamesEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsQuickplayGamesEnabled");
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00010B9E File Offset: 0x0000ED9E
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x00010BA6 File Offset: 0x0000EDA6
		[DataSourceProperty]
		public bool IsCustomServerListEnabled
		{
			get
			{
				return this._isCustomServerListEnabled;
			}
			set
			{
				if (value != this._isCustomServerListEnabled)
				{
					this._isCustomServerListEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCustomServerListEnabled");
				}
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00010BC4 File Offset: 0x0000EDC4
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x00010BCC File Offset: 0x0000EDCC
		[DataSourceProperty]
		public bool IsPremadeGamesEnabled
		{
			get
			{
				return this._isPremadeGamesEnabled;
			}
			set
			{
				if (value != this._isPremadeGamesEnabled)
				{
					this._isPremadeGamesEnabled = value;
					base.OnPropertyChanged("IsPremadeGamesEnabled");
				}
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00010BE9 File Offset: 0x0000EDE9
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x00010BF1 File Offset: 0x0000EDF1
		[DataSourceProperty]
		public bool IsCustomServerFeatureSupported
		{
			get
			{
				return this._isCustomServerFeatureSupported;
			}
			set
			{
				if (value != this._isCustomServerFeatureSupported)
				{
					this._isCustomServerFeatureSupported = value;
					base.OnPropertyChanged("IsCustomServerFeatureSupported");
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00010C0E File Offset: 0x0000EE0E
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x00010C16 File Offset: 0x0000EE16
		[DataSourceProperty]
		public bool IsClansFeatureSupported
		{
			get
			{
				return this._isClansFeatureSupported;
			}
			set
			{
				if (value != this._isClansFeatureSupported)
				{
					this._isClansFeatureSupported = value;
					base.OnPropertyChanged("IsClansFeatureSupported");
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00010C33 File Offset: 0x0000EE33
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x00010C3B File Offset: 0x0000EE3B
		[DataSourceProperty]
		public MPCustomGameVM CustomServer
		{
			get
			{
				return this._customServer;
			}
			set
			{
				if (value != this._customServer)
				{
					this._customServer = value;
					base.OnPropertyChangedWithValue<MPCustomGameVM>(value, "CustomServer");
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00010C59 File Offset: 0x0000EE59
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x00010C61 File Offset: 0x0000EE61
		[DataSourceProperty]
		public MPCustomGameVM PremadeMatches
		{
			get
			{
				return this._premadeMatches;
			}
			set
			{
				if (value != this._premadeMatches)
				{
					this._premadeMatches = value;
					base.OnPropertyChanged("PremadeMatches");
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00010C7E File Offset: 0x0000EE7E
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x00010C86 File Offset: 0x0000EE86
		[DataSourceProperty]
		public bool IsRanked
		{
			get
			{
				return this._isRanked;
			}
			set
			{
				if (value != this._isRanked)
				{
					this._isRanked = value;
					base.OnPropertyChangedWithValue(value, "IsRanked");
					this.OnSelectionChanged(false, false);
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00010CAC File Offset: 0x0000EEAC
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x00010CB4 File Offset: 0x0000EEB4
		[DataSourceProperty]
		public MBBindingList<MPMatchmakingItemVM> RankedGameTypes
		{
			get
			{
				return this._rankedGameTypes;
			}
			set
			{
				if (value != this._rankedGameTypes)
				{
					this._rankedGameTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPMatchmakingItemVM>>(value, "RankedGameTypes");
				}
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00010CD2 File Offset: 0x0000EED2
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x00010CDA File Offset: 0x0000EEDA
		[DataSourceProperty]
		public MBBindingList<MPMatchmakingItemVM> QuickplayGameTypes
		{
			get
			{
				return this._quickplayGameTypes;
			}
			set
			{
				if (value != this._quickplayGameTypes)
				{
					this._quickplayGameTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPMatchmakingItemVM>>(value, "QuickplayGameTypes");
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00010CF8 File Offset: 0x0000EEF8
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x00010D00 File Offset: 0x0000EF00
		[DataSourceProperty]
		public MBBindingList<MPMatchmakingItemVM> CustomGameTypes
		{
			get
			{
				return this._customGameTypes;
			}
			set
			{
				if (value != this._customGameTypes)
				{
					this._customGameTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPMatchmakingItemVM>>(value, "CustomGameTypes");
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00010D1E File Offset: 0x0000EF1E
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00010D26 File Offset: 0x0000EF26
		[DataSourceProperty]
		public SelectorVM<MPMatchmakingRegionSelectorItemVM> Regions
		{
			get
			{
				return this._regions;
			}
			set
			{
				if (value != this._regions)
				{
					this._regions = value;
					base.OnPropertyChangedWithValue<SelectorVM<MPMatchmakingRegionSelectorItemVM>>(value, "Regions");
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00010D44 File Offset: 0x0000EF44
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x00010D4C File Offset: 0x0000EF4C
		[DataSourceProperty]
		public MPMatchmakingSelectionInfoVM SelectionInfo
		{
			get
			{
				return this._selectionInfo;
			}
			set
			{
				if (value != this._selectionInfo)
				{
					this._selectionInfo = value;
					base.OnPropertyChangedWithValue<MPMatchmakingSelectionInfoVM>(value, "SelectionInfo");
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00010D6A File Offset: 0x0000EF6A
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x00010D72 File Offset: 0x0000EF72
		[DataSourceProperty]
		public bool IsMatchFindPossible
		{
			get
			{
				return this._isMatchFindPossible;
			}
			set
			{
				if (value != this._isMatchFindPossible)
				{
					this._isMatchFindPossible = value;
					base.OnPropertyChangedWithValue(value, "IsMatchFindPossible");
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00010D90 File Offset: 0x0000EF90
		// (set) Token: 0x0600049D RID: 1181 RVA: 0x00010D98 File Offset: 0x0000EF98
		[DataSourceProperty]
		public bool IsFindingMatch
		{
			get
			{
				return this._isFindingMatch;
			}
			set
			{
				if (value != this._isFindingMatch)
				{
					this._isFindingMatch = value;
					base.OnPropertyChangedWithValue(value, "IsFindingMatch");
					this.IsFindingMatchUpdated();
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00010DBC File Offset: 0x0000EFBC
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		[DataSourceProperty]
		public string PlayText
		{
			get
			{
				return this._playText;
			}
			set
			{
				if (value != this._playText)
				{
					this._playText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayText");
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00010DE7 File Offset: 0x0000EFE7
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x00010DEF File Offset: 0x0000EFEF
		[DataSourceProperty]
		public string QuickPlayText
		{
			get
			{
				return this._quickPlayText;
			}
			set
			{
				if (value != this._quickPlayText)
				{
					this._quickPlayText = value;
					base.OnPropertyChangedWithValue<string>(value, "QuickPlayText");
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00010E12 File Offset: 0x0000F012
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x00010E1A File Offset: 0x0000F01A
		[DataSourceProperty]
		public string CustomGameText
		{
			get
			{
				return this._customGameText;
			}
			set
			{
				if (value != this._customGameText)
				{
					this._customGameText = value;
					base.OnPropertyChangedWithValue<string>(value, "CustomGameText");
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00010E3D File Offset: 0x0000F03D
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x00010E45 File Offset: 0x0000F045
		[DataSourceProperty]
		public string CommunityGameText
		{
			get
			{
				return this._communityGameText;
			}
			set
			{
				if (value != this._communityGameText)
				{
					this._communityGameText = value;
					base.OnPropertyChangedWithValue<string>(value, "CommunityGameText");
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00010E68 File Offset: 0x0000F068
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x00010E70 File Offset: 0x0000F070
		[DataSourceProperty]
		public string CustomServerListText
		{
			get
			{
				return this._customServerListText;
			}
			set
			{
				if (value != this._customServerListText)
				{
					this._customServerListText = value;
					base.OnPropertyChangedWithValue<string>(value, "CustomServerListText");
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00010E93 File Offset: 0x0000F093
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x00010E9B File Offset: 0x0000F09B
		[DataSourceProperty]
		public string AutoFindText
		{
			get
			{
				return this._autoFindText;
			}
			set
			{
				if (value != this._autoFindText)
				{
					this._autoFindText = value;
					base.OnPropertyChangedWithValue<string>(value, "AutoFindText");
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00010EBE File Offset: 0x0000F0BE
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x00010EC6 File Offset: 0x0000F0C6
		[DataSourceProperty]
		public string MatchFindNotPossibleText
		{
			get
			{
				return this._matchFindNotPossibleText;
			}
			set
			{
				if (value != this._matchFindNotPossibleText)
				{
					this._matchFindNotPossibleText = value;
					base.OnPropertyChangedWithValue<string>(value, "MatchFindNotPossibleText");
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00010EE9 File Offset: 0x0000F0E9
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x00010EF1 File Offset: 0x0000F0F1
		[DataSourceProperty]
		public string RankedText
		{
			get
			{
				return this._rankedText;
			}
			set
			{
				if (value != this._rankedText)
				{
					this._rankedText = value;
					base.OnPropertyChangedWithValue<string>(value, "RankedText");
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00010F14 File Offset: 0x0000F114
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00010F1C File Offset: 0x0000F11C
		[DataSourceProperty]
		public string CasualText
		{
			get
			{
				return this._casualText;
			}
			set
			{
				if (value != this._casualText)
				{
					this._casualText = value;
					base.OnPropertyChangedWithValue<string>(value, "CasualText");
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00010F3F File Offset: 0x0000F13F
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x00010F47 File Offset: 0x0000F147
		[DataSourceProperty]
		public string SelectionInfoText
		{
			get
			{
				return this._selectionInfoText;
			}
			set
			{
				if (value != this._selectionInfoText)
				{
					this._selectionInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionInfoText");
				}
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00010F6A File Offset: 0x0000F16A
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00010F72 File Offset: 0x0000F172
		[DataSourceProperty]
		public int SelectedSubPageIndex
		{
			get
			{
				return this._selectedSubPageIndex;
			}
			set
			{
				if (value != this._selectedSubPageIndex)
				{
					this._selectedSubPageIndex = value;
					base.OnPropertyChangedWithValue(value, "SelectedSubPageIndex");
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00010F90 File Offset: 0x0000F190
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00010F98 File Offset: 0x0000F198
		[DataSourceProperty]
		public string TeamMatchesText
		{
			get
			{
				return this._teamMatchesText;
			}
			set
			{
				if (value != this._teamMatchesText)
				{
					this._teamMatchesText = value;
					base.OnPropertyChanged("TeamMatchesText");
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00010FBA File Offset: 0x0000F1BA
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00010FC2 File Offset: 0x0000F1C2
		[DataSourceProperty]
		public HintViewModel RegionsHint
		{
			get
			{
				return this._regionsHint;
			}
			set
			{
				if (value != this._regionsHint)
				{
					this._regionsHint = value;
					base.OnPropertyChanged("RegionsHint");
				}
			}
		}

		// Token: 0x0400021A RID: 538
		private LobbyState _lobbyState;

		// Token: 0x0400021B RID: 539
		private bool _isTestRegionAvailable;

		// Token: 0x0400021C RID: 540
		private bool _regionsRequireRefresh;

		// Token: 0x0400021D RID: 541
		private MPMatchmakingVM.MatchmakingSubPages _currentSubPage;

		// Token: 0x0400021E RID: 542
		private IEnumerable<MultiplayerClassDivisions.MPHeroClass> _heroClasses;

		// Token: 0x0400021F RID: 543
		private TextObject _selectionInfoTextObject;

		// Token: 0x04000220 RID: 544
		private string[] _defaultSelectedGameTypes;

		// Token: 0x04000221 RID: 545
		private bool _isServerStatusReceived;

		// Token: 0x04000222 RID: 546
		private bool _isEligibleForPremadeMatches;

		// Token: 0x04000223 RID: 547
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x04000224 RID: 548
		private readonly Action<string, bool> _onMatchSelectionChanged;

		// Token: 0x04000225 RID: 549
		private readonly Action<bool> _onGameFindRequestStateChanged;

		// Token: 0x04000226 RID: 550
		private bool _isEnabled;

		// Token: 0x04000227 RID: 551
		private bool _isRanked;

		// Token: 0x04000228 RID: 552
		private bool _isCustomGameStageFindEnabled;

		// Token: 0x04000229 RID: 553
		private bool _hasUnofficialModulesLoaded;

		// Token: 0x0400022A RID: 554
		private int _selectedSubPageIndex;

		// Token: 0x0400022B RID: 555
		private MBBindingList<MPMatchmakingItemVM> _quickplayGameTypes;

		// Token: 0x0400022C RID: 556
		private MBBindingList<MPMatchmakingItemVM> _rankedGameTypes;

		// Token: 0x0400022D RID: 557
		private MBBindingList<MPMatchmakingItemVM> _customGameTypes;

		// Token: 0x0400022E RID: 558
		private SelectorVM<MPMatchmakingRegionSelectorItemVM> _regions;

		// Token: 0x0400022F RID: 559
		private MPMatchmakingSelectionInfoVM _selectionInfo;

		// Token: 0x04000230 RID: 560
		private MPCustomGameVM _customServer;

		// Token: 0x04000231 RID: 561
		private MPCustomGameVM _premadeMatches;

		// Token: 0x04000232 RID: 562
		private bool _isMatchFindPossible;

		// Token: 0x04000233 RID: 563
		private bool _isFindingMatch;

		// Token: 0x04000234 RID: 564
		private bool _isRankedGamesEnabled;

		// Token: 0x04000235 RID: 565
		private bool _isCustomGamesEnabled;

		// Token: 0x04000236 RID: 566
		private bool _isQuickplayGamesEnabled;

		// Token: 0x04000237 RID: 567
		private bool _isCustomServerListEnabled;

		// Token: 0x04000238 RID: 568
		private bool _isCustomServerFeatureSupported;

		// Token: 0x04000239 RID: 569
		private bool _isPremadeGamesEnabled;

		// Token: 0x0400023A RID: 570
		private bool _isClansFeatureSupported;

		// Token: 0x0400023B RID: 571
		private string _playText;

		// Token: 0x0400023C RID: 572
		private string _autoFindText;

		// Token: 0x0400023D RID: 573
		private string _matchFindNotPossibleText;

		// Token: 0x0400023E RID: 574
		private string _rankedText;

		// Token: 0x0400023F RID: 575
		private string _casualText;

		// Token: 0x04000240 RID: 576
		private string _selectionInfoText;

		// Token: 0x04000241 RID: 577
		private string _quickPlayText;

		// Token: 0x04000242 RID: 578
		private string _customGameText;

		// Token: 0x04000243 RID: 579
		private string _customServerListText;

		// Token: 0x04000244 RID: 580
		private string _communityGameText;

		// Token: 0x04000245 RID: 581
		private string _teamMatchesText;

		// Token: 0x04000246 RID: 582
		private HintViewModel _regionsHint;

		// Token: 0x020000E2 RID: 226
		public enum MatchmakingSubPages
		{
			// Token: 0x04000858 RID: 2136
			QuickPlay,
			// Token: 0x04000859 RID: 2137
			CustomGame,
			// Token: 0x0400085A RID: 2138
			CustomGameList,
			// Token: 0x0400085B RID: 2139
			PremadeMatchList,
			// Token: 0x0400085C RID: 2140
			Default
		}
	}
}
