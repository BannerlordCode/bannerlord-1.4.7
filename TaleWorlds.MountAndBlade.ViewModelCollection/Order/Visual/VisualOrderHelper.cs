using System;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002C RID: 44
	public static class VisualOrderHelper
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0000D35C File Offset: 0x0000B55C
		public unsafe static bool DoesFormationHaveOrderType(Formation formation, OrderType type)
		{
			MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
			if (type == OrderType.LookAtEnemy)
			{
				return formation.FacingOrder.OrderEnum == FacingOrder.FacingOrderEnum.LookAtEnemy;
			}
			if (type != OrderType.LookAtDirection)
			{
				switch (type)
				{
				case OrderType.HoldFire:
					return formation.FiringOrder.OrderEnum == FiringOrder.RangedWeaponUsageOrderEnum.HoldYourFire;
				case OrderType.FireAtWill:
					return formation.FiringOrder.OrderEnum == FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill;
				case OrderType.Mount:
					return formation.RidingOrder.OrderEnum == RidingOrder.RidingOrderEnum.Mount;
				case OrderType.Dismount:
					return formation.RidingOrder.OrderEnum == RidingOrder.RidingOrderEnum.Dismount;
				case OrderType.AIControlOn:
					return formation.IsAIControlled;
				case OrderType.AIControlOff:
					return !formation.IsAIControlled;
				}
				return movementOrder.OrderType == type || formation.ArrangementOrder.OrderType == type || formation.FacingOrder.OrderType == type || formation.FiringOrder.OrderType == type || formation.FormOrder.OrderType == type || formation.RidingOrder.OrderType == type;
			}
			return formation.FacingOrder.OrderEnum == FacingOrder.FacingOrderEnum.LookAtDirection;
		}
	}
}
