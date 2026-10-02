using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A7 RID: 167
	public class MultiplayerLobbyHomeScreenWidget : Widget
	{
		// Token: 0x060008CD RID: 2253 RVA: 0x00019363 File Offset: 0x00017563
		public MultiplayerLobbyHomeScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001936C File Offset: 0x0001756C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				if (base.IsVisible)
				{
					base.OnPropertyChanged(true, "IsVisible");
				}
				this._initialized = true;
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00019398 File Offset: 0x00017598
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this.FindGameButton.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled && !isSearchRequested && (isPartyLeader || !isInParty);
			this.FindGameButton.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
			this.SelectionInfo.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled;
			this.SelectionInfo.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00019419 File Offset: 0x00017619
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x00019421 File Offset: 0x00017621
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

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0001943F File Offset: 0x0001763F
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00019447 File Offset: 0x00017647
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

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00019465 File Offset: 0x00017665
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x0001946D File Offset: 0x0001766D
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

		// Token: 0x040003FE RID: 1022
		private bool _initialized;

		// Token: 0x040003FF RID: 1023
		private ButtonWidget _findGameButton;

		// Token: 0x04000400 RID: 1024
		private Widget _selectionInfo;

		// Token: 0x04000401 RID: 1025
		private bool _hasUnofficialModulesLoaded;
	}
}
