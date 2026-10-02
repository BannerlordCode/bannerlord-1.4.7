using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GatherArmy
{
	// Token: 0x0200014C RID: 332
	public class BoostCohesionPopupWidget : Widget
	{
		// Token: 0x060011B0 RID: 4528 RVA: 0x000313F9 File Offset: 0x0002F5F9
		public BoostCohesionPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00031404 File Offset: 0x0002F604
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.ClosePopupButton != null && !this.ClosePopupButton.ClickEventHandlers.Contains(new Action<Widget>(this.ClosePopup)))
			{
				this.ClosePopupButton.ClickEventHandlers.Add(new Action<Widget>(this.ClosePopup));
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0003145A File Offset: 0x0002F65A
		public void ClosePopup(Widget widget)
		{
			base.ParentWidget.IsVisible = false;
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00031468 File Offset: 0x0002F668
		// (set) Token: 0x060011B4 RID: 4532 RVA: 0x00031470 File Offset: 0x0002F670
		[Editor(false)]
		public ButtonWidget ClosePopupButton
		{
			get
			{
				return this._closePopupButton;
			}
			set
			{
				if (this._closePopupButton != value)
				{
					this._closePopupButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ClosePopupButton");
				}
			}
		}

		// Token: 0x04000815 RID: 2069
		private ButtonWidget _closePopupButton;
	}
}
