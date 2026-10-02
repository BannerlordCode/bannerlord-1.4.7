using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend
{
	// Token: 0x020000B0 RID: 176
	public class MultiplayerLobbyFriendGroupToggleWidget : ToggleButtonWidget
	{
		// Token: 0x06000933 RID: 2355 RVA: 0x0001A0C1 File Offset: 0x000182C1
		public MultiplayerLobbyFriendGroupToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001A0CA File Offset: 0x000182CA
		protected override void OnClick(Widget widget)
		{
			base.OnClick(widget);
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001A0D9 File Offset: 0x000182D9
		protected override void RefreshState()
		{
			base.RefreshState();
			Widget titleContainer = this.TitleContainer;
			if (titleContainer == null)
			{
				return;
			}
			titleContainer.SetState(base.CurrentState);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001A0F7 File Offset: 0x000182F7
		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001A11F File Offset: 0x0001831F
		private void UpdateCollapseIndicator()
		{
			if (base.WidgetToClose != null && this.CollapseIndicator != null)
			{
				if (base.WidgetToClose.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001A15F File Offset: 0x0001835F
		private void PlayerCountUpdated()
		{
			if (this.PlayerCountText == null)
			{
				return;
			}
			this.PlayerCountText.Text = "(" + this.PlayerCount + ")";
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001A18F File Offset: 0x0001838F
		private void InitialClosedStateUpdated()
		{
			base.IsSelected = !this.InitialClosedState;
			this.CollapseIndicatorUpdated();
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0001A1A6 File Offset: 0x000183A6
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x0001A1AE File Offset: 0x000183AE
		[Editor(false)]
		public Widget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (this._collapseIndicator != value)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged<Widget>(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001A1D2 File Offset: 0x000183D2
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x0001A1DA File Offset: 0x000183DA
		[Editor(false)]
		public Widget TitleContainer
		{
			get
			{
				return this._titleContainer;
			}
			set
			{
				if (this._titleContainer != value)
				{
					this._titleContainer = value;
					base.OnPropertyChanged<Widget>(value, "TitleContainer");
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0001A1F8 File Offset: 0x000183F8
		// (set) Token: 0x0600093F RID: 2367 RVA: 0x0001A200 File Offset: 0x00018400
		[Editor(false)]
		public TextWidget PlayerCountText
		{
			get
			{
				return this._playerCountText;
			}
			set
			{
				if (this._playerCountText != value)
				{
					this._playerCountText = value;
					base.OnPropertyChanged<TextWidget>(value, "PlayerCountText");
				}
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0001A21E File Offset: 0x0001841E
		// (set) Token: 0x06000941 RID: 2369 RVA: 0x0001A226 File Offset: 0x00018426
		[Editor(false)]
		public int PlayerCount
		{
			get
			{
				return this._playerCount;
			}
			set
			{
				if (this._playerCount != value)
				{
					this._playerCount = value;
					base.OnPropertyChanged(value, "PlayerCount");
					this.PlayerCountUpdated();
				}
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0001A24A File Offset: 0x0001844A
		// (set) Token: 0x06000943 RID: 2371 RVA: 0x0001A252 File Offset: 0x00018452
		[Editor(false)]
		public bool InitialClosedState
		{
			get
			{
				return this._initialClosedState;
			}
			set
			{
				if (this._initialClosedState != value)
				{
					this._initialClosedState = value;
					base.OnPropertyChanged(value, "InitialClosedState");
					this.InitialClosedStateUpdated();
				}
			}
		}

		// Token: 0x0400042C RID: 1068
		private Widget _collapseIndicator;

		// Token: 0x0400042D RID: 1069
		private Widget _titleContainer;

		// Token: 0x0400042E RID: 1070
		private TextWidget _playerCountText;

		// Token: 0x0400042F RID: 1071
		private int _playerCount;

		// Token: 0x04000430 RID: 1072
		private bool _initialClosedState;
	}
}
