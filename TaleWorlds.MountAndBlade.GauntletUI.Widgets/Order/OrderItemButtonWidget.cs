using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000071 RID: 113
	public class OrderItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000610 RID: 1552 RVA: 0x00011E54 File Offset: 0x00010054
		public OrderItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00011E5D File Offset: 0x0001005D
		private void SelectionStateChanged()
		{
			if (!string.IsNullOrEmpty(this.SelectionState))
			{
				ImageWidget selectionVisualWidget = this.SelectionVisualWidget;
				if (selectionVisualWidget != null && selectionVisualWidget.ContainsState(this.SelectionState))
				{
					this.SelectionVisualWidget.SetState(this.SelectionState);
				}
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00011E97 File Offset: 0x00010097
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x00011E9F File Offset: 0x0001009F
		[Editor(false)]
		public string SelectionState
		{
			get
			{
				return this._selectionState;
			}
			set
			{
				if (this._selectionState != value)
				{
					this._selectionState = value;
					base.OnPropertyChanged<string>(value, "SelectionState");
					this.SelectionStateChanged();
				}
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x00011EC8 File Offset: 0x000100C8
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x00011ED0 File Offset: 0x000100D0
		[Editor(false)]
		public ImageWidget SelectionVisualWidget
		{
			get
			{
				return this._selectionVisualWidget;
			}
			set
			{
				if (this._selectionVisualWidget != value)
				{
					this._selectionVisualWidget = value;
					base.OnPropertyChanged<ImageWidget>(value, "SelectionVisualWidget");
					if (value != null)
					{
						value.AddState("Disabled");
						value.AddState("PartiallyActive");
						value.AddState("Active");
					}
					this.SelectionStateChanged();
				}
			}
		}

		// Token: 0x0400029B RID: 667
		private string _selectionState;

		// Token: 0x0400029C RID: 668
		private ImageWidget _selectionVisualWidget;
	}
}
