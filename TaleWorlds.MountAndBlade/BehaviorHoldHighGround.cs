using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200011A RID: 282
	public class BehaviorHoldHighGround : BehaviorComponent
	{
		// Token: 0x06000E2C RID: 3628 RVA: 0x0001FD7A File Offset: 0x0001DF7A
		public BehaviorHoldHighGround(Formation formation)
			: base(formation)
		{
			this._isAllowedToChangePosition = true;
			this.RangedAllyFormation = null;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0001FD98 File Offset: 0x0001DF98
		protected override void CalculateCurrentOrder()
		{
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			WorldPosition worldPosition;
			Vec2 vec;
			if (cachedClosestEnemyFormation != null)
			{
				worldPosition = base.Formation.CachedMedianPosition;
				if (base.Formation.AI.ActiveBehavior != this)
				{
					this._isAllowedToChangePosition = true;
				}
				else
				{
					float num = Math.Max((this.RangedAllyFormation != null) ? (this.RangedAllyFormation.QuerySystem.MissileRangeAdjusted * 0.8f) : 0f, 30f);
					this._isAllowedToChangePosition = base.Formation.CachedAveragePosition.DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) > num * num;
				}
				if (this._isAllowedToChangePosition)
				{
					worldPosition.SetVec2(base.Formation.QuerySystem.HighGroundCloseToForeseenBattleGround);
					this._lastChosenPosition = worldPosition;
				}
				else
				{
					worldPosition = this._lastChosenPosition;
				}
				vec = ((base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.HighGroundCloseToForeseenBattleGround) > 25f) ? (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - worldPosition.AsVec2).Normalized() : ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized());
			}
			else
			{
				vec = base.Formation.Direction;
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0001FF9B File Offset: 0x0001E19B
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0001FFC8 File Offset: 0x0001E1C8
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0002002E File Offset: 0x0001E22E
		protected override float GetAiWeight()
		{
			if (base.Formation.CachedClosestEnemyFormation == null)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x04000363 RID: 867
		public Formation RangedAllyFormation;

		// Token: 0x04000364 RID: 868
		private bool _isAllowedToChangePosition;

		// Token: 0x04000365 RID: 869
		private WorldPosition _lastChosenPosition;
	}
}
