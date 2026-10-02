using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010D RID: 269
	public class SliderPopupWidget : Widget
	{
		// Token: 0x06000E49 RID: 3657 RVA: 0x000276B6 File Offset: 0x000258B6
		public SliderPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000276C0 File Offset: 0x000258C0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.SliderValueTextWidget != null && this.ReserveAmountSlider != null)
			{
				this.SliderValueTextWidget.Text = this.ReserveAmountSlider.ValueInt.ToString();
			}
			if (base.ParentWidget.IsVisible && base.EventManager.LatestMouseDownWidget != this && base.EventManager.LatestMouseDownWidget != base.ParentWidget && base.EventManager.LatestMouseDownWidget != this.PopupParentWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget))
			{
				base.EventFired("ClosePopup", Array.Empty<object>());
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x00027766 File Offset: 0x00025966
		// (set) Token: 0x06000E4C RID: 3660 RVA: 0x0002776E File Offset: 0x0002596E
		[Editor(false)]
		public Widget PopupParentWidget
		{
			get
			{
				return this._popupParentWidget;
			}
			set
			{
				if (this._popupParentWidget != value)
				{
					this._popupParentWidget = value;
					base.OnPropertyChanged<Widget>(value, "PopupParentWidget");
				}
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0002778C File Offset: 0x0002598C
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x00027794 File Offset: 0x00025994
		[Editor(false)]
		public ButtonWidget ClosePopupWidget
		{
			get
			{
				return this._closePopupWidget;
			}
			set
			{
				if (this._closePopupWidget != value)
				{
					this._closePopupWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ClosePopupWidget");
				}
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x000277B2 File Offset: 0x000259B2
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x000277BA File Offset: 0x000259BA
		[Editor(false)]
		public TextWidget SliderValueTextWidget
		{
			get
			{
				return this._sliderValueTextWidget;
			}
			set
			{
				if (this._sliderValueTextWidget != value)
				{
					this._sliderValueTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SliderValueTextWidget");
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x000277D8 File Offset: 0x000259D8
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x000277E0 File Offset: 0x000259E0
		[Editor(false)]
		public SliderWidget ReserveAmountSlider
		{
			get
			{
				return this._reserveAmountSlider;
			}
			set
			{
				if (this._reserveAmountSlider != value)
				{
					this._reserveAmountSlider = value;
					base.OnPropertyChanged<SliderWidget>(value, "ReserveAmountSlider");
				}
			}
		}

		// Token: 0x0400067B RID: 1659
		private ButtonWidget _closePopupWidget;

		// Token: 0x0400067C RID: 1660
		private TextWidget _sliderValueTextWidget;

		// Token: 0x0400067D RID: 1661
		private SliderWidget _reserveAmountSlider;

		// Token: 0x0400067E RID: 1662
		private Widget _popupParentWidget;
	}
}
