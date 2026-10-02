using System;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x0200004F RID: 79
	public class MPLobbyHomeVM : ViewModel
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006EA RID: 1770 RVA: 0x000162B4 File Offset: 0x000144B4
		// (remove) Token: 0x060006EB RID: 1771 RVA: 0x000162EC File Offset: 0x000144EC
		public event Action OnFindGameRequested;

		// Token: 0x060006EC RID: 1772 RVA: 0x00016324 File Offset: 0x00014524
		public MPLobbyHomeVM(NewsManager newsManager, Action<MPLobbyVM.LobbyPage> onChangePageRequest)
		{
			this._onChangePageRequest = onChangePageRequest;
			this.HasUnofficialModulesLoaded = NetworkMain.GameClient.HasUnofficialModulesLoaded;
			this.Player = new MPLobbyPlayerBaseVM(NetworkMain.GameClient.PlayerID, "", null, null);
			this.News = new MPNewsVM(newsManager);
			this.IsNewsAvailable = true;
			this.Announcements = new MPAnnouncementsVM(this.IsNewsAvailable ? new float?(30f) : null);
			this.RefreshValues();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000163AC File Offset: 0x000145AC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FindGameText = new TextObject("{=yA45PqFc}FIND GAME", null).ToString();
			this.MatchFindNotPossibleText = new TextObject("{=BrYUHFsg}CHOOSE GAME", null).ToString();
			this.OpenProfileText = new TextObject("{=aBCi76ig}Show More", null).ToString();
			this.Player.RefreshValues();
			this.News.RefreshValues();
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00016417 File Offset: 0x00014617
		public void OnTick(float dt)
		{
			this.Announcements.OnTick(dt);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00016425 File Offset: 0x00014625
		public void RefreshPlayerData(PlayerData playerData, bool updateRating = true)
		{
			this.Player.UpdateWith(playerData);
			if (updateRating)
			{
				this.Player.UpdateRating(new Action(this.OnRatingReceived));
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001644D File Offset: 0x0001464D
		private void OnRatingReceived()
		{
			this.Player.RefreshSelectableGameTypes(true, new Action<string>(this.Player.UpdateDisplayedRankInfo), "");
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00016471 File Offset: 0x00014671
		public void OnMatchSelectionChanged(string selectionInfo, bool isMatchFindPossible)
		{
			this.SelectionInfoText = selectionInfo;
			this.IsMatchFindPossible = isMatchFindPossible;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00016481 File Offset: 0x00014681
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

		// Token: 0x060006F3 RID: 1779 RVA: 0x000164AD File Offset: 0x000146AD
		private void ExecuteOpenMatchmaking()
		{
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000164C0 File Offset: 0x000146C0
		private void ExecuteOpenProfile()
		{
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Profile);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000164D3 File Offset: 0x000146D3
		public void OnClanInfoChanged()
		{
			this.Player.UpdateClanInfo();
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000164E0 File Offset: 0x000146E0
		public void OnPlayerNameUpdated(string playerName)
		{
			this.Player.UpdateNameAndAvatar(true);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000164EE File Offset: 0x000146EE
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.News.OnFinalize();
			this.News = null;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00016508 File Offset: 0x00014708
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00016510 File Offset: 0x00014710
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

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001652E File Offset: 0x0001472E
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x00016536 File Offset: 0x00014736
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

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00016554 File Offset: 0x00014754
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0001655C File Offset: 0x0001475C
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

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001657A File Offset: 0x0001477A
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x00016582 File Offset: 0x00014782
		[DataSourceProperty]
		public bool IsNewsAvailable
		{
			get
			{
				return this._isNewsAvailable;
			}
			set
			{
				if (value != this._isNewsAvailable)
				{
					this._isNewsAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsNewsAvailable");
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000165A0 File Offset: 0x000147A0
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x000165A8 File Offset: 0x000147A8
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

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000165CB File Offset: 0x000147CB
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x000165D3 File Offset: 0x000147D3
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

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x000165F6 File Offset: 0x000147F6
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x000165FE File Offset: 0x000147FE
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

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00016621 File Offset: 0x00014821
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x00016629 File Offset: 0x00014829
		[DataSourceProperty]
		public string OpenProfileText
		{
			get
			{
				return this._openProfileText;
			}
			set
			{
				if (value != this._openProfileText)
				{
					this._openProfileText = value;
					base.OnPropertyChangedWithValue<string>(value, "OpenProfileText");
				}
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001664C File Offset: 0x0001484C
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x00016654 File Offset: 0x00014854
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM Player
		{
			get
			{
				return this._player;
			}
			set
			{
				if (value != this._player)
				{
					this._player = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "Player");
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00016672 File Offset: 0x00014872
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x0001667A File Offset: 0x0001487A
		[DataSourceProperty]
		public MPNewsVM News
		{
			get
			{
				return this._news;
			}
			set
			{
				if (value != this._news)
				{
					this._news = value;
					base.OnPropertyChangedWithValue<MPNewsVM>(value, "News");
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00016698 File Offset: 0x00014898
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x000166A0 File Offset: 0x000148A0
		[DataSourceProperty]
		public MPAnnouncementsVM Announcements
		{
			get
			{
				return this._announcements;
			}
			set
			{
				if (value != this._announcements)
				{
					this._announcements = value;
					base.OnPropertyChangedWithValue<MPAnnouncementsVM>(value, "Announcements");
				}
			}
		}

		// Token: 0x0400033B RID: 827
		private const float _announcementUpdateIntervalInSeconds = 30f;

		// Token: 0x0400033C RID: 828
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x0400033E RID: 830
		private bool _isEnabled;

		// Token: 0x0400033F RID: 831
		private bool _isMatchFindPossible;

		// Token: 0x04000340 RID: 832
		private bool _hasUnofficialModulesLoaded;

		// Token: 0x04000341 RID: 833
		private bool _isNewsAvailable;

		// Token: 0x04000342 RID: 834
		private string _findGameText;

		// Token: 0x04000343 RID: 835
		private string _matchFindNotPossibleText;

		// Token: 0x04000344 RID: 836
		private string _selectionInfoText;

		// Token: 0x04000345 RID: 837
		private string _openProfileText;

		// Token: 0x04000346 RID: 838
		private MPLobbyPlayerBaseVM _player;

		// Token: 0x04000347 RID: 839
		private MPNewsVM _news;

		// Token: 0x04000348 RID: 840
		private MPAnnouncementsVM _announcements;
	}
}
