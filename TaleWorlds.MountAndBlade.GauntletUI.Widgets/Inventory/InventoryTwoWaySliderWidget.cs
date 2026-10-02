using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000144 RID: 324
	public class InventoryTwoWaySliderWidget : TwoWaySliderWidget
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x0002F117 File Offset: 0x0002D317
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x0002F11F File Offset: 0x0002D31F
		public bool IsExtended
		{
			get
			{
				return this._isExtended;
			}
			set
			{
				if (this._isExtended != value)
				{
					this.CheckFillerState();
					this._isExtended = value;
				}
			}
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0002F137 File Offset: 0x0002D337
		public InventoryTwoWaySliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0002F140 File Offset: 0x0002D340
		protected override void OnParallelUpdate(float dt)
		{
			if (this._initFiller == null && base.Filler != null)
			{
				this._initFiller = base.Filler;
			}
			if (this.IsExtended)
			{
				base.OnParallelUpdate(dt);
				this.CheckFillerState();
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0002F174 File Offset: 0x0002D374
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isBeingDragged && !base.IsPressed)
			{
				Widget handle = base.Handle;
				if (handle != null && !handle.IsPressed)
				{
					this._shouldRemoveZeroCounts = true;
				}
			}
			bool flag;
			if (!base.IsPressed)
			{
				Widget handle2 = base.Handle;
				flag = handle2 != null && handle2.IsPressed;
			}
			else
			{
				flag = true;
			}
			this._isBeingDragged = flag;
			if (this._shouldRemoveZeroCounts)
			{
				base.EventFired("RemoveZeroCounts", Array.Empty<object>());
				this._shouldRemoveZeroCounts = false;
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0002F1F8 File Offset: 0x0002D3F8
		private void CheckFillerState()
		{
			if (this._initFiller != null)
			{
				if (this.IsExtended && base.Filler == null)
				{
					base.Filler = this._initFiller;
					return;
				}
				if (!this.IsExtended && base.Filler != null)
				{
					base.Filler = null;
				}
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0002F236 File Offset: 0x0002D436
		private void OnStockChangeClick(Widget obj)
		{
			this._manuallyIncreased = true;
			this._shouldRemoveZeroCounts = true;
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x0002F246 File Offset: 0x0002D446
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x0002F24E File Offset: 0x0002D44E
		[Editor(false)]
		public ButtonWidget IncreaseStockButtonWidget
		{
			get
			{
				return this._increaseStockButtonWidget;
			}
			set
			{
				if (this._increaseStockButtonWidget != value)
				{
					this._increaseStockButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "IncreaseStockButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnStockChangeClick));
				}
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x0002F283 File Offset: 0x0002D483
		// (set) Token: 0x06001120 RID: 4384 RVA: 0x0002F28B File Offset: 0x0002D48B
		[Editor(false)]
		public ButtonWidget DecreaseStockButtonWidget
		{
			get
			{
				return this._decreaseStockButtonWidget;
			}
			set
			{
				if (this._decreaseStockButtonWidget != value)
				{
					this._decreaseStockButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DecreaseStockButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnStockChangeClick));
				}
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x0002F2C0 File Offset: 0x0002D4C0
		// (set) Token: 0x06001122 RID: 4386 RVA: 0x0002F2C8 File Offset: 0x0002D4C8
		[Editor(false)]
		public bool IsRightSide
		{
			get
			{
				return this._isRightSide;
			}
			set
			{
				if (this._isRightSide != value)
				{
					this._isRightSide = value;
					base.OnPropertyChanged(value, "IsRightSide");
				}
			}
		}

		// Token: 0x040007BB RID: 1979
		private bool _isExtended;

		// Token: 0x040007BC RID: 1980
		private Widget _initFiller;

		// Token: 0x040007BD RID: 1981
		private bool _isBeingDragged;

		// Token: 0x040007BE RID: 1982
		private bool _shouldRemoveZeroCounts;

		// Token: 0x040007BF RID: 1983
		private ButtonWidget _increaseStockButtonWidget;

		// Token: 0x040007C0 RID: 1984
		private ButtonWidget _decreaseStockButtonWidget;

		// Token: 0x040007C1 RID: 1985
		private bool _isRightSide;
	}
}
