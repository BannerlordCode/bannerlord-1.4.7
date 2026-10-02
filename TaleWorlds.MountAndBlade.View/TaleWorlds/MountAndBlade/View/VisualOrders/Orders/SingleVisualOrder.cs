using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.View.VisualOrders.Orders
{
	// Token: 0x0200002C RID: 44
	public class SingleVisualOrder : VisualOrder
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00008ED0 File Offset: 0x000070D0
		public SingleVisualOrder(string stringId, TextObject name, OrderType orderType, bool useFormationTarget, bool useWorldPositionTarget)
			: base(stringId)
		{
			this._name = name;
			this._orderType = orderType;
			this._useFormationTarget = useFormationTarget;
			this._useWorldPositionTarget = useWorldPositionTarget;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008EF8 File Offset: 0x000070F8
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			if (executionParameters.HasFormation && this._useFormationTarget)
			{
				orderController.SetOrderWithFormation(this._orderType, executionParameters.Formation);
				return;
			}
			if (executionParameters.HasWorldPosition && this._useWorldPositionTarget)
			{
				orderController.SetOrderWithPosition(this._orderType, executionParameters.WorldPosition);
				return;
			}
			orderController.SetOrder(this._orderType);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008F57 File Offset: 0x00007157
		public override TextObject GetName(OrderController orderController)
		{
			return this._name;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008F5F File Offset: 0x0000715F
		public override bool IsTargeted()
		{
			return this._useFormationTarget || this._useWorldPositionTarget;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008F71 File Offset: 0x00007171
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(VisualOrderHelper.DoesFormationHaveOrderType(formation, this._orderType));
		}

		// Token: 0x04000054 RID: 84
		private TextObject _name;

		// Token: 0x04000055 RID: 85
		private OrderType _orderType;

		// Token: 0x04000056 RID: 86
		private bool _useFormationTarget;

		// Token: 0x04000057 RID: 87
		private bool _useWorldPositionTarget;
	}
}
