using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001F RID: 31
	public class OrderSetVM : OrderItemBaseVM
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002DE RID: 734 RVA: 0x0000BC88 File Offset: 0x00009E88
		// (remove) Token: 0x060002DF RID: 735 RVA: 0x0000BCBC File Offset: 0x00009EBC
		public static event OrderSetVM.OnOrderSetSelectionStateChangedDelegate OnSelectionStateChanged;

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000BCEF File Offset: 0x00009EEF
		public bool HasSingleOrder
		{
			get
			{
				return this.SoloOrder != null;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000BCFA File Offset: 0x00009EFA
		public VisualOrderSet OrderSet { get; }

		// Token: 0x060002E2 RID: 738 RVA: 0x0000BD02 File Offset: 0x00009F02
		public OrderSetVM(OrderController orderController, VisualOrderSet collection)
			: base(orderController)
		{
			this.OrderSet = collection;
			this.Orders = new MBBindingList<OrderItemVM>();
			this.RefreshOrders();
			this.RefreshValues();
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000BD29 File Offset: 0x00009F29
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Name = this.OrderSet.GetName(this._orderController).ToString();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000BD50 File Offset: 0x00009F50
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this.SoloOrder != null)
			{
				this.SoloOrder = null;
			}
			for (int i = 0; i < this.Orders.Count; i++)
			{
				this.Orders[i].OnFinalize();
			}
			InputKeyItemVM shortcutKey = base.ShortcutKey;
			if (shortcutKey == null)
			{
				return;
			}
			shortcutKey.OnFinalize();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BDA9 File Offset: 0x00009FA9
		protected override void OnExecuteAction(VisualOrderExecutionParameters executionParameters)
		{
			if (this.OrderSet.IsSoloOrder)
			{
				this.SoloOrder.ExecuteAction(executionParameters);
			}
			else
			{
				OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
				if (onSelectionStateChanged != null)
				{
					onSelectionStateChanged(this, true);
				}
			}
			this.RefreshOrderStates();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		protected override void OnRefreshState()
		{
			base.Name = this.OrderSet.GetName(this._orderController).ToString();
			if (this.OrderSet.IsSoloOrder)
			{
				OrderState activeState = this.OrderSet.SoloOrder.GetActiveState(this._orderController);
				base.SelectionState = activeState.ToString();
				base.IsActive = activeState == OrderState.Active;
				return;
			}
			base.IsActive = false;
			base.SelectionState = OrderState.Default.ToString();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000BE67 File Offset: 0x0000A067
		public void ExecuteSelect()
		{
			OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
			if (onSelectionStateChanged == null)
			{
				return;
			}
			onSelectionStateChanged(this, true);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000BE7A File Offset: 0x0000A07A
		public void ExecuteDeSelect()
		{
			OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
			if (onSelectionStateChanged == null)
			{
				return;
			}
			onSelectionStateChanged(this, false);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000BE8D File Offset: 0x0000A08D
		public void OnOrderExecuted(OrderItemVM order)
		{
			this.RefreshOrderStates();
			this.RefreshValues();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000BE9C File Offset: 0x0000A09C
		public void RefreshOrders()
		{
			this.Orders.Clear();
			if (this.SoloOrder != null)
			{
				this.SoloOrder = null;
			}
			if (this.OrderSet != null)
			{
				MBReadOnlyList<VisualOrder> orders = this.OrderSet.Orders;
				for (int i = 0; i < orders.Count; i++)
				{
					this.Orders.Add(new OrderItemVM(this._orderController, orders[i]));
				}
				if (this.OrderSet.IsSoloOrder)
				{
					this.SoloOrder = this.Orders[0];
				}
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000BF24 File Offset: 0x0000A124
		protected override void OnSelectedStateChanged(bool isSelected)
		{
			base.OnSelectedStateChanged(isSelected);
			OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
			if (onSelectionStateChanged != null)
			{
				onSelectionStateChanged(this, isSelected);
			}
			if (this.SoloOrder != null)
			{
				this.SoloOrder.IsSelected = isSelected;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000BF54 File Offset: 0x0000A154
		public void RefreshOrderStates()
		{
			base.OrderIconId = this.OrderSet.IconId;
			base.RefreshState();
			for (int i = 0; i < this.Orders.Count; i++)
			{
				this.Orders[i].RefreshState();
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public void UpdateCanUseShortcuts(bool value)
		{
			base.CanUseShortcuts = value;
			for (int i = 0; i < this.Orders.Count; i++)
			{
				this.Orders[i].CanUseShortcuts = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000BFDC File Offset: 0x0000A1DC
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000BFE4 File Offset: 0x0000A1E4
		[DataSourceProperty]
		public string SelectedOrderText
		{
			get
			{
				return this._selectedOrderText;
			}
			set
			{
				if (value != this._selectedOrderText)
				{
					this._selectedOrderText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedOrderText");
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000C007 File Offset: 0x0000A207
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000C00F File Offset: 0x0000A20F
		[DataSourceProperty]
		public OrderItemVM SoloOrder
		{
			get
			{
				return this._soloOrder;
			}
			set
			{
				if (value != this._soloOrder)
				{
					this._soloOrder = value;
					base.OnPropertyChangedWithValue<OrderItemVM>(value, "SoloOrder");
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000C02D File Offset: 0x0000A22D
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000C035 File Offset: 0x0000A235
		[DataSourceProperty]
		public MBBindingList<OrderItemVM> Orders
		{
			get
			{
				return this._orders;
			}
			set
			{
				if (value != this._orders)
				{
					this._orders = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderItemVM>>(value, "Orders");
				}
			}
		}

		// Token: 0x04000146 RID: 326
		private string _selectedOrderText;

		// Token: 0x04000147 RID: 327
		private OrderItemVM _soloOrder;

		// Token: 0x04000148 RID: 328
		private MBBindingList<OrderItemVM> _orders;

		// Token: 0x020000C8 RID: 200
		// (Invoke) Token: 0x06000C49 RID: 3145
		public delegate void OnOrderSetSelectionStateChangedDelegate(OrderSetVM orderSet, bool isSelected);
	}
}
