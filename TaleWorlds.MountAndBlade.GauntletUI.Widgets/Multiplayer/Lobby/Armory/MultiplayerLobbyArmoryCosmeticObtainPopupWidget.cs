using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B9 RID: 185
	public class MultiplayerLobbyArmoryCosmeticObtainPopupWidget : Widget
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x0001B371 File Offset: 0x00019571
		public MultiplayerLobbyArmoryCosmeticObtainPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0001B384 File Offset: 0x00019584
		private void OnObtainStateChanged(int newState)
		{
			if (newState == 0)
			{
				this.ItemPreviewListPanel.IsVisible = true;
				this.ActionButtonWidget.IsEnabled = true;
				this.CancelButtonWidget.IsEnabled = true;
				this.ResultSuccessfulIconWidget.IsVisible = false;
				this.ResultFailedIconWidget.IsVisible = false;
				this.ResultTextWidget.IsVisible = false;
				this.LoadingAnimationWidget.IsVisible = false;
				return;
			}
			if (newState == 1)
			{
				this.LoadingAnimationWidget.IsVisible = true;
				this.CancelButtonWidget.IsEnabled = false;
				this.ActionButtonWidget.IsEnabled = false;
				this.ItemPreviewListPanel.IsVisible = false;
				this.ResultSuccessfulIconWidget.IsVisible = false;
				this.ResultFailedIconWidget.IsVisible = false;
				this.ResultTextWidget.IsVisible = false;
				return;
			}
			if (newState == 2 || newState == 3)
			{
				this.CancelButtonWidget.IsEnabled = true;
				this.ActionButtonWidget.IsEnabled = true;
				this.ResultTextWidget.IsVisible = true;
				if (newState == 2)
				{
					this.ResultSuccessfulIconWidget.IsVisible = true;
				}
				else
				{
					this.ResultFailedIconWidget.IsVisible = true;
				}
				this.ItemPreviewListPanel.IsVisible = false;
				this.LoadingAnimationWidget.IsVisible = false;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0001B4A4 File Offset: 0x000196A4
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x0001B4AC File Offset: 0x000196AC
		[Editor(false)]
		public int ObtainState
		{
			get
			{
				return this._obtainState;
			}
			set
			{
				if (value != this._obtainState)
				{
					this._obtainState = value;
					base.OnPropertyChanged(value, "ObtainState");
					this.OnObtainStateChanged(value);
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0001B4D1 File Offset: 0x000196D1
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x0001B4D9 File Offset: 0x000196D9
		[Editor(false)]
		public ButtonWidget CancelButtonWidget
		{
			get
			{
				return this._cancelButtonWidget;
			}
			set
			{
				if (value != this._cancelButtonWidget)
				{
					this._cancelButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "CancelButtonWidget");
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0001B4F7 File Offset: 0x000196F7
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x0001B4FF File Offset: 0x000196FF
		[Editor(false)]
		public ListPanel ItemPreviewListPanel
		{
			get
			{
				return this._itemPreviewListPanel;
			}
			set
			{
				if (value != this._itemPreviewListPanel)
				{
					this._itemPreviewListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "ItemPreviewListPanel");
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0001B51D File Offset: 0x0001971D
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x0001B525 File Offset: 0x00019725
		[Editor(false)]
		public ButtonWidget ActionButtonWidget
		{
			get
			{
				return this._actionButtonWidget;
			}
			set
			{
				if (value != this._actionButtonWidget)
				{
					this._actionButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ActionButtonWidget");
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0001B543 File Offset: 0x00019743
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x0001B54B File Offset: 0x0001974B
		[Editor(false)]
		public Widget ResultSuccessfulIconWidget
		{
			get
			{
				return this._resultSuccessfulIconWidget;
			}
			set
			{
				if (value != this._resultSuccessfulIconWidget)
				{
					this._resultSuccessfulIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResultSuccessfulIconWidget");
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0001B569 File Offset: 0x00019769
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0001B571 File Offset: 0x00019771
		[Editor(false)]
		public Widget ResultFailedIconWidget
		{
			get
			{
				return this._resultFailedIconWidget;
			}
			set
			{
				if (value != this._resultFailedIconWidget)
				{
					this._resultFailedIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResultFailedIconWidget");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0001B58F File Offset: 0x0001978F
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x0001B597 File Offset: 0x00019797
		[Editor(false)]
		public TextWidget ResultTextWidget
		{
			get
			{
				return this._resultTextWidget;
			}
			set
			{
				if (value != this._resultTextWidget)
				{
					this._resultTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "ResultTextWidget");
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0001B5B5 File Offset: 0x000197B5
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x0001B5BD File Offset: 0x000197BD
		[Editor(false)]
		public Widget LoadingAnimationWidget
		{
			get
			{
				return this._loadingAnimationWidget;
			}
			set
			{
				if (value != this._loadingAnimationWidget)
				{
					this._loadingAnimationWidget = value;
					base.OnPropertyChanged<Widget>(value, "LoadingAnimationWidget");
				}
			}
		}

		// Token: 0x04000463 RID: 1123
		private int _obtainState = -1;

		// Token: 0x04000464 RID: 1124
		private ButtonWidget _cancelButtonWidget;

		// Token: 0x04000465 RID: 1125
		private ListPanel _itemPreviewListPanel;

		// Token: 0x04000466 RID: 1126
		private ButtonWidget _actionButtonWidget;

		// Token: 0x04000467 RID: 1127
		private Widget _resultSuccessfulIconWidget;

		// Token: 0x04000468 RID: 1128
		private Widget _resultFailedIconWidget;

		// Token: 0x04000469 RID: 1129
		private TextWidget _resultTextWidget;

		// Token: 0x0400046A RID: 1130
		private Widget _loadingAnimationWidget;
	}
}
