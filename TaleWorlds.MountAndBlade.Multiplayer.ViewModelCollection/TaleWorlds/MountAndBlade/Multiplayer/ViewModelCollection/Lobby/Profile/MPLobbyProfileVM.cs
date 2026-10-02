using System;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003A RID: 58
	public class MPLobbyProfileVM : ViewModel
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000549 RID: 1353 RVA: 0x000122F4 File Offset: 0x000104F4
		// (remove) Token: 0x0600054A RID: 1354 RVA: 0x0001232C File Offset: 0x0001052C
		public event Action OnFindGameRequested;

		// Token: 0x0600054B RID: 1355 RVA: 0x00012364 File Offset: 0x00010564
		public MPLobbyProfileVM(LobbyState lobbyState, Action<MPLobbyVM.LobbyPage> onChangePageRequest, Action onOpenRecentGames)
		{
			this._onChangePageRequest = onChangePageRequest;
			this._onOpenRecentGames = onOpenRecentGames;
			this.HasUnofficialModulesLoaded = NetworkMain.GameClient.HasUnofficialModulesLoaded;
			this.PlayerInfo = new MPLobbyPlayerProfileVM(lobbyState);
			this.RecentGamesSummary = new MBBindingList<MPLobbyRecentGameItemVM>();
			this.RefreshValues();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000123B4 File Offset: 0x000105B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FindGameText = new TextObject("{=yA45PqFc}FIND GAME", null).ToString();
			this.MatchFindNotPossibleText = new TextObject("{=BrYUHFsg}CHOOSE GAME", null).ToString();
			this.ShowMoreText = new TextObject("{=aBCi76ig}Show More", null).ToString();
			this.RecentGamesTitleText = new TextObject("{=NJolh9ye}Recent Games", null).ToString();
			this.RecentGamesSummary.ApplyActionOnAllItems(delegate(MPLobbyRecentGameItemVM r)
			{
				r.RefreshValues();
			});
			this.PlayerInfo.RefreshValues();
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00012454 File Offset: 0x00010654
		public void RefreshRecentGames(MBReadOnlyList<MatchHistoryData> recentGames)
		{
			this.RecentGamesSummary.Clear();
			IOrderedEnumerable<MatchHistoryData> orderedEnumerable = recentGames.OrderByDescending<MatchHistoryData, DateTime>((MatchHistoryData m) => m.MatchDate);
			int num = Math.Min(3, orderedEnumerable.Count<MatchHistoryData>());
			for (int i = 0; i < num; i++)
			{
				MPLobbyRecentGameItemVM mplobbyRecentGameItemVM = new MPLobbyRecentGameItemVM(null);
				mplobbyRecentGameItemVM.FillFrom(orderedEnumerable.ElementAt<MatchHistoryData>(i));
				this.RecentGamesSummary.Add(mplobbyRecentGameItemVM);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000124CB File Offset: 0x000106CB
		public void OnMatchSelectionChanged(string selectionInfo, bool isMatchFindPossible)
		{
			this.SelectionInfoText = selectionInfo;
			this.IsMatchFindPossible = isMatchFindPossible;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000124DB File Offset: 0x000106DB
		public void UpdatePlayerData(PlayerData playerData, bool updateStatistics = true, bool updateRating = true)
		{
			this.PlayerInfo.UpdatePlayerData(playerData, updateStatistics, updateRating);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000124EB File Offset: 0x000106EB
		public void OnPlayerNameUpdated(string playerName)
		{
			MPLobbyPlayerProfileVM playerInfo = this.PlayerInfo;
			if (playerInfo == null)
			{
				return;
			}
			playerInfo.OnPlayerNameUpdated(playerName);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000124FE File Offset: 0x000106FE
		public void OnNotificationReceived(LobbyNotification notification)
		{
			if (notification.Type == NotificationType.BadgeEarned)
			{
				this.HasBadgeNotification = true;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001250F File Offset: 0x0001070F
		private void ExecuteFindGame()
		{
			if (this.IsMatchFindPossible)
			{
				Action onFindGameRequested = this.OnFindGameRequested;
				if (onFindGameRequested == null)
				{
					return;
				}
				onFindGameRequested();
				return;
			}
			else
			{
				Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
				if (onChangePageRequest == null)
				{
					return;
				}
				onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
				return;
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001253B File Offset: 0x0001073B
		private void ExecuteOpenMatchmaking()
		{
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001254E File Offset: 0x0001074E
		private void ExecuteOpenRecentGames()
		{
			Action onOpenRecentGames = this._onOpenRecentGames;
			if (onOpenRecentGames == null)
			{
				return;
			}
			onOpenRecentGames();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00012560 File Offset: 0x00010760
		public void OnClanInfoChanged()
		{
			this.PlayerInfo.OnClanInfoChanged();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001256D File Offset: 0x0001076D
		private void OnEnabledChanged()
		{
			MPLobbyPlayerProfileVM playerInfo = this.PlayerInfo;
			if (((playerInfo != null) ? playerInfo.Player : null) != null)
			{
				PlatformServices.Instance.CheckPermissionWithUser(Permission.ViewUserGeneratedContent, this.PlayerInfo.Player.ProvidedID, delegate(bool hasBannerlordIDPrivilege)
				{
					this.PlayerInfo.Player.IsBannerlordIDSupported = hasBannerlordIDPrivilege;
				});
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000125AA File Offset: 0x000107AA
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x000125B2 File Offset: 0x000107B2
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x000125D0 File Offset: 0x000107D0
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x000125D8 File Offset: 0x000107D8
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x000125F6 File Offset: 0x000107F6
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x000125FE File Offset: 0x000107FE
		[DataSourceProperty]
		public string ShowMoreText
		{
			get
			{
				return this._showMoreText;
			}
			set
			{
				if (value != this._showMoreText)
				{
					this._showMoreText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShowMoreText");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00012621 File Offset: 0x00010821
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00012629 File Offset: 0x00010829
		[DataSourceProperty]
		public string FindGameText
		{
			get
			{
				return this._findGameText;
			}
			set
			{
				if (value != this._findGameText)
				{
					this._findGameText = value;
					base.OnPropertyChangedWithValue<string>(value, "FindGameText");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001264C File Offset: 0x0001084C
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x00012654 File Offset: 0x00010854
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

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00012677 File Offset: 0x00010877
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x0001267F File Offset: 0x0001087F
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
					this.OnEnabledChanged();
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x000126A3 File Offset: 0x000108A3
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x000126AB File Offset: 0x000108AB
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x000126CE File Offset: 0x000108CE
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x000126D6 File Offset: 0x000108D6
		[DataSourceProperty]
		public string RecentGamesTitleText
		{
			get
			{
				return this._recentGamesTitleText;
			}
			set
			{
				if (value != this._recentGamesTitleText)
				{
					this._recentGamesTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecentGamesTitleText");
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x000126F9 File Offset: 0x000108F9
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x00012701 File Offset: 0x00010901
		[DataSourceProperty]
		public bool HasBadgeNotification
		{
			get
			{
				return this._hasBadgeNotification;
			}
			set
			{
				if (value != this._hasBadgeNotification)
				{
					this._hasBadgeNotification = value;
					base.OnPropertyChangedWithValue(value, "HasBadgeNotification");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001271F File Offset: 0x0001091F
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x00012727 File Offset: 0x00010927
		[DataSourceProperty]
		public MBBindingList<MPLobbyRecentGameItemVM> RecentGamesSummary
		{
			get
			{
				return this._recentGamesSummary;
			}
			set
			{
				if (value != this._recentGamesSummary)
				{
					this._recentGamesSummary = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyRecentGameItemVM>>(value, "RecentGamesSummary");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00012745 File Offset: 0x00010945
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x0001274D File Offset: 0x0001094D
		[DataSourceProperty]
		public MPLobbyPlayerProfileVM PlayerInfo
		{
			get
			{
				return this._playerInfo;
			}
			set
			{
				if (value != this._playerInfo)
				{
					this._playerInfo = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerProfileVM>(value, "PlayerInfo");
				}
			}
		}

		// Token: 0x04000287 RID: 647
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x04000288 RID: 648
		private readonly Action _onOpenRecentGames;

		// Token: 0x04000289 RID: 649
		private bool _isEnabled;

		// Token: 0x0400028A RID: 650
		private bool _isMatchFindPossible;

		// Token: 0x0400028B RID: 651
		private bool _hasUnofficialModulesLoaded;

		// Token: 0x0400028C RID: 652
		private bool _hasBadgeNotification;

		// Token: 0x0400028D RID: 653
		private string _showMoreText;

		// Token: 0x0400028E RID: 654
		private string _findGameText;

		// Token: 0x0400028F RID: 655
		private string _matchFindNotPossibleText;

		// Token: 0x04000290 RID: 656
		private string _selectionInfoText;

		// Token: 0x04000291 RID: 657
		private string _recentGamesTitleText;

		// Token: 0x04000292 RID: 658
		private MBBindingList<MPLobbyRecentGameItemVM> _recentGamesSummary;

		// Token: 0x04000293 RID: 659
		private MPLobbyPlayerProfileVM _playerInfo;
	}
}
