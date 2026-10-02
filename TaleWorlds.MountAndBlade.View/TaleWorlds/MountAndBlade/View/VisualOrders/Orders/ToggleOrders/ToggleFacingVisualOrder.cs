using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.View.VisualOrders.Orders.ToggleOrders
{
	// Token: 0x0200002D RID: 45
	public class ToggleFacingVisualOrder : VisualOrder
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00008F84 File Offset: 0x00007184
		public ToggleFacingVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008F90 File Offset: 0x00007190
		public override TextObject GetName(OrderController orderController)
		{
			OrderState activeState = base.GetActiveState(orderController);
			if (activeState == OrderState.Active || activeState == OrderState.PartiallyActive)
			{
				return new TextObject("{=qWzBa3KT}Facing Enemy", null);
			}
			return new TextObject("{=LWVwNcRA}Facing Direction", null);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008FC4 File Offset: 0x000071C4
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			if (ToggleFacingVisualOrder.IsFacingEnemy(base.GetActiveState(orderController)))
			{
				orderController.SetOrderWithPosition(OrderType.LookAtDirection, executionParameters.WorldPosition);
				return;
			}
			orderController.SetOrder(OrderType.LookAtEnemy);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008FEB File Offset: 0x000071EB
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008FEE File Offset: 0x000071EE
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(OrderController.GetActiveFacingOrderOf(formation) == OrderType.LookAtEnemy);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009000 File Offset: 0x00007200
		protected override string GetIconId()
		{
			string iconId = base.GetIconId();
			if (this._lastActiveState == OrderState.Active)
			{
				return iconId + "_active";
			}
			return iconId;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000902A File Offset: 0x0000722A
		private static bool IsFacingEnemy(OrderState activeState)
		{
			return activeState == OrderState.Active;
		}
	}
}
