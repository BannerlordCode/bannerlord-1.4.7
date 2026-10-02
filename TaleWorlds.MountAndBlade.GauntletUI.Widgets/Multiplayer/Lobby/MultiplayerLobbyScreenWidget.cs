using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Matchmaking;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000AC RID: 172
	public class MultiplayerLobbyScreenWidget : Widget
	{
		// Token: 0x060008F6 RID: 2294 RVA: 0x000197B0 File Offset: 0x000179B0
		public MultiplayerLobbyScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000197BC File Offset: 0x000179BC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.IsLoggedIn)
			{
				foreach (TextureWidget textureWidget in base.GetAllChildrenOfTypeRecursive<TextureWidget>(null))
				{
					if (((textureWidget != null) ? textureWidget.TextureProvider : null) != null && !textureWidget.SetForClearNextFrame)
					{
						textureWidget.OnClearTextureProvider();
					}
				}
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00019834 File Offset: 0x00017A34
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.OnLobbyStateChanged();
				this._initialized = true;
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00019854 File Offset: 0x00017A54
		private void OnLoggedInChanged()
		{
			if (!this._isLoggedIn)
			{
				this._stateChangeLocked = true;
				this.IsSearchGameRequested = false;
				this.IsSearchingGame = false;
				this.IsCustomBattleEnabled = false;
				this.IsMatchmakingEnabled = false;
				this.IsPartyLeader = false;
				this.IsInParty = false;
				this._stateChangeLocked = false;
				this.OnLobbyStateChanged();
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x000198A8 File Offset: 0x00017AA8
		private void OnLobbyStateChanged()
		{
			if (!this._stateChangeLocked)
			{
				this.MenuWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
				this.HomeScreenWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
				this.MatchmakingScreenWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
				this.ProfileScreenWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001997C File Offset: 0x00017B7C
		private void HomeScreenWidgetPropertyChanged(PropertyOwnerObject owner, string property, bool value)
		{
			this.ToggleFriendListOnTabToggled(property, value);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00019986 File Offset: 0x00017B86
		private void SocialScreenWidgetPropertyChanged(PropertyOwnerObject owner, string property, bool value)
		{
			this.ToggleFriendListOnTabToggled(property, value);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00019990 File Offset: 0x00017B90
		private void ToggleFriendListOnTabToggled(string property, bool value)
		{
			if (this.FriendsPanelWidget != null && property == "IsVisible")
			{
				bool flag = value;
				if (!flag)
				{
					MultiplayerLobbyHomeScreenWidget homeScreenWidget = this.HomeScreenWidget;
					if (homeScreenWidget == null || !homeScreenWidget.IsVisible)
					{
						MultiplayerLobbyProfileScreenWidget profileScreenWidget = this.ProfileScreenWidget;
						if (profileScreenWidget == null || !profileScreenWidget.IsVisible)
						{
							goto IL_0044;
						}
					}
					flag = true;
				}
				IL_0044:
				this.FriendsPanelWidget.IsForcedOpen = flag;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x000199ED File Offset: 0x00017BED
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x000199F5 File Offset: 0x00017BF5
		[Editor(false)]
		public bool IsLoggedIn
		{
			get
			{
				return this._isLoggedIn;
			}
			set
			{
				if (value != this._isLoggedIn)
				{
					this._isLoggedIn = value;
					base.OnPropertyChanged(value, "IsLoggedIn");
					this.OnLoggedInChanged();
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00019A19 File Offset: 0x00017C19
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00019A21 File Offset: 0x00017C21
		[Editor(false)]
		public bool IsSearchGameRequested
		{
			get
			{
				return this._isSearchGameRequested;
			}
			set
			{
				if (this._isSearchGameRequested != value)
				{
					this._isSearchGameRequested = value;
					base.OnPropertyChanged(value, "IsSearchGameRequested");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00019A45 File Offset: 0x00017C45
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00019A4D File Offset: 0x00017C4D
		[Editor(false)]
		public bool IsSearchingGame
		{
			get
			{
				return this._isSearchingGame;
			}
			set
			{
				if (this._isSearchingGame != value)
				{
					this._isSearchingGame = value;
					base.OnPropertyChanged(value, "IsSearchingGame");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00019A71 File Offset: 0x00017C71
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00019A79 File Offset: 0x00017C79
		[Editor(false)]
		public bool IsCustomBattleEnabled
		{
			get
			{
				return this._isCustomBattleEnabled;
			}
			set
			{
				if (this._isCustomBattleEnabled != value)
				{
					this._isCustomBattleEnabled = value;
					base.OnPropertyChanged(value, "IsCustomBattleEnabled");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00019A9D File Offset: 0x00017C9D
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00019AA5 File Offset: 0x00017CA5
		[Editor(false)]
		public bool IsMatchmakingEnabled
		{
			get
			{
				return this._isMatchmakingEnabled;
			}
			set
			{
				if (this._isMatchmakingEnabled != value)
				{
					this._isMatchmakingEnabled = value;
					base.OnPropertyChanged(value, "IsMatchmakingEnabled");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00019AC9 File Offset: 0x00017CC9
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00019AD1 File Offset: 0x00017CD1
		[Editor(false)]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (this._isPartyLeader != value)
				{
					this._isPartyLeader = value;
					base.OnPropertyChanged(value, "IsPartyLeader");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00019AF5 File Offset: 0x00017CF5
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00019AFD File Offset: 0x00017CFD
		[Editor(false)]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (this._isInParty != value)
				{
					this._isInParty = value;
					base.OnPropertyChanged(value, "IsInParty");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00019B21 File Offset: 0x00017D21
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x00019B29 File Offset: 0x00017D29
		[Editor(false)]
		public MultiplayerLobbyMenuWidget MenuWidget
		{
			get
			{
				return this._menuWidget;
			}
			set
			{
				if (this._menuWidget != value)
				{
					this._menuWidget = value;
					base.OnPropertyChanged<MultiplayerLobbyMenuWidget>(value, "MenuWidget");
				}
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00019B47 File Offset: 0x00017D47
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00019B50 File Offset: 0x00017D50
		[Editor(false)]
		public MultiplayerLobbyHomeScreenWidget HomeScreenWidget
		{
			get
			{
				return this._homeScreenWidget;
			}
			set
			{
				if (this._homeScreenWidget != value)
				{
					if (this._homeScreenWidget != null)
					{
						this._homeScreenWidget.boolPropertyChanged -= this.HomeScreenWidgetPropertyChanged;
					}
					this._homeScreenWidget = value;
					if (this._homeScreenWidget != null)
					{
						this._homeScreenWidget.boolPropertyChanged += this.HomeScreenWidgetPropertyChanged;
					}
					base.OnPropertyChanged<MultiplayerLobbyHomeScreenWidget>(value, "HomeScreenWidget");
				}
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00019BB7 File Offset: 0x00017DB7
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00019BBF File Offset: 0x00017DBF
		[Editor(false)]
		public MultiplayerLobbyMatchmakingScreenWidget MatchmakingScreenWidget
		{
			get
			{
				return this._matchmakingScreenWidget;
			}
			set
			{
				if (this._matchmakingScreenWidget != value)
				{
					this._matchmakingScreenWidget = value;
					base.OnPropertyChanged<MultiplayerLobbyMatchmakingScreenWidget>(value, "MatchmakingScreenWidget");
				}
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00019BDD File Offset: 0x00017DDD
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00019BE8 File Offset: 0x00017DE8
		[Editor(false)]
		public MultiplayerLobbyProfileScreenWidget ProfileScreenWidget
		{
			get
			{
				return this._profileScreenWidget;
			}
			set
			{
				if (this._profileScreenWidget != value)
				{
					if (this._profileScreenWidget != null)
					{
						this._profileScreenWidget.boolPropertyChanged -= this.SocialScreenWidgetPropertyChanged;
					}
					this._profileScreenWidget = value;
					if (this._profileScreenWidget != null)
					{
						this._profileScreenWidget.boolPropertyChanged += this.SocialScreenWidgetPropertyChanged;
					}
					base.OnPropertyChanged<MultiplayerLobbyProfileScreenWidget>(value, "ProfileScreenWidget");
				}
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00019C4F File Offset: 0x00017E4F
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00019C57 File Offset: 0x00017E57
		[Editor(false)]
		public MultiplayerLobbyFriendsPanelWidget FriendsPanelWidget
		{
			get
			{
				return this._friendsPanelWidget;
			}
			set
			{
				if (this._friendsPanelWidget != value)
				{
					this._friendsPanelWidget = value;
					base.OnPropertyChanged<MultiplayerLobbyFriendsPanelWidget>(value, "FriendsPanelWidget");
				}
			}
		}

		// Token: 0x0400040E RID: 1038
		private bool _initialized;

		// Token: 0x0400040F RID: 1039
		private bool _stateChangeLocked;

		// Token: 0x04000410 RID: 1040
		private bool _isLoggedIn;

		// Token: 0x04000411 RID: 1041
		private bool _isSearchGameRequested;

		// Token: 0x04000412 RID: 1042
		private bool _isSearchingGame;

		// Token: 0x04000413 RID: 1043
		private bool _isMatchmakingEnabled;

		// Token: 0x04000414 RID: 1044
		private bool _isPartyLeader;

		// Token: 0x04000415 RID: 1045
		private bool _isInParty;

		// Token: 0x04000416 RID: 1046
		private bool _isCustomBattleEnabled;

		// Token: 0x04000417 RID: 1047
		private MultiplayerLobbyMenuWidget _menuWidget;

		// Token: 0x04000418 RID: 1048
		private MultiplayerLobbyHomeScreenWidget _homeScreenWidget;

		// Token: 0x04000419 RID: 1049
		private MultiplayerLobbyMatchmakingScreenWidget _matchmakingScreenWidget;

		// Token: 0x0400041A RID: 1050
		private MultiplayerLobbyFriendsPanelWidget _friendsPanelWidget;

		// Token: 0x0400041B RID: 1051
		private MultiplayerLobbyProfileScreenWidget _profileScreenWidget;
	}
}
