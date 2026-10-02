using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Matchmaking
{
	// Token: 0x020000AE RID: 174
	public class MultiplayerLobbyMatchmakingScreenWidget : Widget
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00019D2B File Offset: 0x00017F2B
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00019D33 File Offset: 0x00017F33
		public MultiplayerLobbyCustomServerScreenWidget CustomServerParentWidget { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00019D3C File Offset: 0x00017F3C
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00019D44 File Offset: 0x00017F44
		public MultiplayerLobbyCustomServerScreenWidget PremadeMatchesParentWidget { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00019D4D File Offset: 0x00017F4D
		private MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages _selectedMode
		{
			get
			{
				return (MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages)this.SelectedModeIndex;
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00019D55 File Offset: 0x00017F55
		public MultiplayerLobbyMatchmakingScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00019D60 File Offset: 0x00017F60
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this._latestIsSearchRequested = isSearchRequested;
			this._latestIsSearching = isSearching;
			this._latestIsMatchmakingEnabled = isMatchmakingEnabled;
			this._latestIsCustomBattleEnabled = isCustomBattleEnabled;
			this._latestIsPartyLeader = isPartyLeader;
			this._latestIsInParty = isInParty;
			if (this.CustomServerParentWidget != null)
			{
				this.CustomServerParentWidget.IsInParty = isInParty;
				this.CustomServerParentWidget.IsPartyLeader = isPartyLeader;
			}
			if (this.PremadeMatchesParentWidget != null)
			{
				this.PremadeMatchesParentWidget.IsInParty = isInParty;
				this.PremadeMatchesParentWidget.IsPartyLeader = isPartyLeader;
			}
			this.UpdateStates();
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00019DE4 File Offset: 0x00017FE4
		private void UpdateStates()
		{
			this.FindGameButton.IsEnabled = ((this._selectedMode != MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame && this._latestIsMatchmakingEnabled && this.IsMatchFindPossible) || (this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame && this.IsCustomGameFindEnabled)) && (this._latestIsPartyLeader || !this._latestIsInParty) && !this._latestIsSearchRequested;
			this.FindGameButton.IsVisible = !this._latestIsSearching && ((this._latestIsCustomBattleEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame) || (this._latestIsMatchmakingEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.QuickPlay));
			this.SelectionInfo.IsEnabled = this._latestIsMatchmakingEnabled;
			this.SelectionInfo.IsVisible = !this._latestIsSearching && !this._latestIsCustomBattleEnabled;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00019EB0 File Offset: 0x000180B0
		private void OnSubpageIndexChange()
		{
			this.FindGameButton.IsVisible = !this._latestIsSearching && ((this._latestIsCustomBattleEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame) || (this._latestIsMatchmakingEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.QuickPlay));
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x00019EF0 File Offset: 0x000180F0
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x00019EF8 File Offset: 0x000180F8
		[Editor(false)]
		public bool IsMatchFindPossible
		{
			get
			{
				return this._isMatchFindPossible;
			}
			set
			{
				if (this._isMatchFindPossible != value)
				{
					this._isMatchFindPossible = value;
					base.OnPropertyChanged(value, "IsMatchFindPossible");
					this.UpdateStates();
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00019F1C File Offset: 0x0001811C
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00019F24 File Offset: 0x00018124
		[Editor(false)]
		public bool IsCustomGameFindEnabled
		{
			get
			{
				return this._isCustomGameFindEnabled;
			}
			set
			{
				if (this._isCustomGameFindEnabled != value)
				{
					this._isCustomGameFindEnabled = value;
					base.OnPropertyChanged(value, "IsCustomGameFindEnabled");
					this.UpdateStates();
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00019F48 File Offset: 0x00018148
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x00019F50 File Offset: 0x00018150
		[Editor(false)]
		public int SelectedModeIndex
		{
			get
			{
				return this._selectedModeIndex;
			}
			set
			{
				if (this._selectedModeIndex != value)
				{
					this._selectedModeIndex = value;
					base.OnPropertyChanged(value, "SelectedModeIndex");
					this.OnSubpageIndexChange();
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00019F74 File Offset: 0x00018174
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x00019F7C File Offset: 0x0001817C
		[Editor(false)]
		public ButtonWidget FindGameButton
		{
			get
			{
				return this._findGameButton;
			}
			set
			{
				if (this._findGameButton != value)
				{
					this._findGameButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FindGameButton");
				}
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x00019F9A File Offset: 0x0001819A
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x00019FA2 File Offset: 0x000181A2
		[Editor(false)]
		public Widget SelectionInfo
		{
			get
			{
				return this._selectionInfo;
			}
			set
			{
				if (this._selectionInfo != value)
				{
					this._selectionInfo = value;
					base.OnPropertyChanged<Widget>(value, "SelectionInfo");
				}
			}
		}

		// Token: 0x0400041F RID: 1055
		private bool _latestIsSearchRequested;

		// Token: 0x04000420 RID: 1056
		private bool _latestIsSearching;

		// Token: 0x04000421 RID: 1057
		private bool _latestIsMatchmakingEnabled;

		// Token: 0x04000422 RID: 1058
		private bool _latestIsCustomBattleEnabled;

		// Token: 0x04000423 RID: 1059
		private bool _latestIsPartyLeader;

		// Token: 0x04000424 RID: 1060
		private bool _latestIsInParty;

		// Token: 0x04000425 RID: 1061
		private ButtonWidget _findGameButton;

		// Token: 0x04000426 RID: 1062
		private Widget _selectionInfo;

		// Token: 0x04000427 RID: 1063
		private int _selectedModeIndex;

		// Token: 0x04000428 RID: 1064
		private bool _isMatchFindPossible;

		// Token: 0x04000429 RID: 1065
		private bool _isCustomGameFindEnabled;

		// Token: 0x020001B9 RID: 441
		private enum MatchmakingSubPages
		{
			// Token: 0x040009F6 RID: 2550
			QuickPlay,
			// Token: 0x040009F7 RID: 2551
			CustomGame,
			// Token: 0x040009F8 RID: 2552
			CustomGameList,
			// Token: 0x040009F9 RID: 2553
			PremadeMatchList,
			// Token: 0x040009FA RID: 2554
			Default
		}
	}
}
