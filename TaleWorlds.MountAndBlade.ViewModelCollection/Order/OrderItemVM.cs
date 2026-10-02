using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001E RID: 30
	public class OrderItemVM : OrderItemBaseVM
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002D8 RID: 728 RVA: 0x0000BB2C File Offset: 0x00009D2C
		// (remove) Token: 0x060002D9 RID: 729 RVA: 0x0000BB60 File Offset: 0x00009D60
		public static event Action<OrderItemVM> OnExecuteOrder;

		// Token: 0x060002DA RID: 730 RVA: 0x0000BB93 File Offset: 0x00009D93
		public OrderItemVM(OrderController orderController, VisualOrder order)
			: base(orderController)
		{
			this.Order = order;
			base.OrderIconId = order.IconId;
			base.IsActive = true;
			this.RefreshValues();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000BBBC File Offset: 0x00009DBC
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject name = this.Order.GetName(this._orderController);
			base.Name = ((name != null) ? name.ToString() : null);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		protected override void OnRefreshState()
		{
			OrderState activeState = this.Order.GetActiveState(this._orderController);
			base.IsActive = activeState == OrderState.Active;
			base.SelectionState = activeState.ToString();
			base.Name = this.Order.GetName(this._orderController).ToString();
			base.OrderIconId = this.Order.IconId;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000BC51 File Offset: 0x00009E51
		protected override void OnExecuteAction(VisualOrderExecutionParameters executionParameters)
		{
			this.Order.BeforeExecuteOrder(this._orderController, executionParameters);
			this.Order.ExecuteOrder(this._orderController, executionParameters);
			Action<OrderItemVM> onExecuteOrder = OrderItemVM.OnExecuteOrder;
			if (onExecuteOrder == null)
			{
				return;
			}
			onExecuteOrder(this);
		}

		// Token: 0x04000143 RID: 323
		public readonly VisualOrder Order;
	}
}
