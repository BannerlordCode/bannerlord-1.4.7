using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000025 RID: 37
	public sealed class ActionVisualOrder : VisualOrder
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000CF87 File Offset: 0x0000B187
		public ActionVisualOrder(string iconId, ActionVisualOrder.OrderActionDelegate orderAction, TextObject name)
			: base(iconId)
		{
			this._name = name;
			this._orderAction = orderAction;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000CF9E File Offset: 0x0000B19E
		public override TextObject GetName(OrderController orderController)
		{
			return this._name;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000CFA6 File Offset: 0x0000B1A6
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000CFA9 File Offset: 0x0000B1A9
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			ActionVisualOrder.OrderActionDelegate orderAction = this._orderAction;
			if (orderAction == null)
			{
				return;
			}
			orderAction(orderController, executionParameters);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000CFBD File Offset: 0x0000B1BD
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(false);
		}

		// Token: 0x0400017A RID: 378
		private readonly ActionVisualOrder.OrderActionDelegate _orderAction;

		// Token: 0x0400017B RID: 379
		private readonly TextObject _name;

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x06000C4D RID: 3149
		public delegate void OrderActionDelegate(OrderController orderController, VisualOrderExecutionParameters executionParameters);
	}
}
