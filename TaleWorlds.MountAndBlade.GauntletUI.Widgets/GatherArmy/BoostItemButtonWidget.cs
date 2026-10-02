using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GatherArmy
{
	// Token: 0x0200014D RID: 333
	public class BoostItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x0003148E File Offset: 0x0002F68E
		// (set) Token: 0x060011B6 RID: 4534 RVA: 0x00031496 File Offset: 0x0002F696
		public BoostCohesionPopupWidget ParentPopupWidget { get; private set; }

		// Token: 0x060011B7 RID: 4535 RVA: 0x0003149F File Offset: 0x0002F69F
		public BoostItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x000314B0 File Offset: 0x0002F6B0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.BoostCurrencyIconWidget != null)
			{
				int boostCurrencyType = this.BoostCurrencyType;
				if (boostCurrencyType != 0)
				{
					if (boostCurrencyType == 1)
					{
						this.BoostCurrencyIconWidget.SetState("Influence");
					}
				}
				else
				{
					this.BoostCurrencyIconWidget.SetState("Gold");
				}
			}
			if (this.ParentPopupWidget == null)
			{
				this.ParentPopupWidget = this.FindParentPopupWidget();
				if (this.ParentPopupWidget != null)
				{
					this.ClickEventHandlers.Add(new Action<Widget>(this.ParentPopupWidget.ClosePopup));
				}
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00031538 File Offset: 0x0002F738
		private BoostCohesionPopupWidget FindParentPopupWidget()
		{
			Widget widget = this;
			while (widget != base.EventManager.Root && this.ParentPopupWidget == null)
			{
				if (widget is BoostCohesionPopupWidget)
				{
					return widget as BoostCohesionPopupWidget;
				}
				widget = widget.ParentWidget;
			}
			return null;
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00031576 File Offset: 0x0002F776
		// (set) Token: 0x060011BB RID: 4539 RVA: 0x0003157E File Offset: 0x0002F77E
		[Editor(false)]
		public int BoostCurrencyType
		{
			get
			{
				return this._boostCurrencyType;
			}
			set
			{
				if (this._boostCurrencyType != value)
				{
					this._boostCurrencyType = value;
					base.OnPropertyChanged(value, "BoostCurrencyType");
				}
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x0003159C File Offset: 0x0002F79C
		// (set) Token: 0x060011BD RID: 4541 RVA: 0x000315A4 File Offset: 0x0002F7A4
		[Editor(false)]
		public Widget BoostCurrencyIconWidget
		{
			get
			{
				return this._boostCurrencyIconWidget;
			}
			set
			{
				if (this._boostCurrencyIconWidget != value)
				{
					this._boostCurrencyIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "BoostCurrencyIconWidget");
				}
			}
		}

		// Token: 0x04000817 RID: 2071
		private int _boostCurrencyType = -1;

		// Token: 0x04000818 RID: 2072
		private Widget _boostCurrencyIconWidget;
	}
}
