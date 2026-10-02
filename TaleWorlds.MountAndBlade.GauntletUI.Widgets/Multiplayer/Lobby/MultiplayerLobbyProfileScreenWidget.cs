using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000AA RID: 170
	public class MultiplayerLobbyProfileScreenWidget : Widget
	{
		// Token: 0x060008E7 RID: 2279 RVA: 0x000195F0 File Offset: 0x000177F0
		public MultiplayerLobbyProfileScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000195FC File Offset: 0x000177FC
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this.FindGameButton.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled && !isSearchRequested && (isPartyLeader || !isInParty);
			this.FindGameButton.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
			this.SelectionInfo.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled;
			this.SelectionInfo.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001967D File Offset: 0x0001787D
		private void OnSubpageIndexChange()
		{
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0001967F File Offset: 0x0001787F
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x00019687 File Offset: 0x00017887
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

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x000196AB File Offset: 0x000178AB
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x000196B3 File Offset: 0x000178B3
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

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x000196D1 File Offset: 0x000178D1
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x000196D9 File Offset: 0x000178D9
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x000196F7 File Offset: 0x000178F7
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x000196FF File Offset: 0x000178FF
		[Editor(false)]
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
					base.OnPropertyChanged(value, "HasUnofficialModulesLoaded");
				}
			}
		}

		// Token: 0x04000408 RID: 1032
		private ButtonWidget _findGameButton;

		// Token: 0x04000409 RID: 1033
		private Widget _selectionInfo;

		// Token: 0x0400040A RID: 1034
		private int _selectedModeIndex;

		// Token: 0x0400040B RID: 1035
		private bool _hasUnofficialModulesLoaded;
	}
}
