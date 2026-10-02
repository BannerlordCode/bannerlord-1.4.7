using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A9 RID: 169
	public class MultiplayerLobbyMenuWidget : Widget
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x0001953A File Offset: 0x0001773A
		public MultiplayerLobbyMenuWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00019543 File Offset: 0x00017743
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this.MatchmakingButtonWidget.IsEnabled = isMatchmakingEnabled || isCustomBattleEnabled;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00019554 File Offset: 0x00017754
		private void SelectedItemIndexChanged()
		{
			if (this.MenuItemListPanel == null)
			{
				return;
			}
			this.MenuItemListPanel.IntValue = this.SelectedItemIndex - 3;
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00019572 File Offset: 0x00017772
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0001957A File Offset: 0x0001777A
		[Editor(false)]
		public int SelectedItemIndex
		{
			get
			{
				return this._selectedItemIndex;
			}
			set
			{
				if (this._selectedItemIndex != value)
				{
					this._selectedItemIndex = value;
					base.OnPropertyChanged(value, "SelectedItemIndex");
					this.SelectedItemIndexChanged();
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0001959E File Offset: 0x0001779E
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x000195A6 File Offset: 0x000177A6
		[Editor(false)]
		public ListPanel MenuItemListPanel
		{
			get
			{
				return this._menuItemListPanel;
			}
			set
			{
				if (this._menuItemListPanel != value)
				{
					this._menuItemListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "MenuItemListPanel");
					this.SelectedItemIndexChanged();
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x000195CA File Offset: 0x000177CA
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x000195D2 File Offset: 0x000177D2
		[Editor(false)]
		public ButtonWidget MatchmakingButtonWidget
		{
			get
			{
				return this._matchmakingButtonWidget;
			}
			set
			{
				if (this._matchmakingButtonWidget != value)
				{
					this._matchmakingButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "MatchmakingButtonWidget");
				}
			}
		}

		// Token: 0x04000405 RID: 1029
		private int _selectedItemIndex;

		// Token: 0x04000406 RID: 1030
		private ListPanel _menuItemListPanel;

		// Token: 0x04000407 RID: 1031
		private ButtonWidget _matchmakingButtonWidget;
	}
}
