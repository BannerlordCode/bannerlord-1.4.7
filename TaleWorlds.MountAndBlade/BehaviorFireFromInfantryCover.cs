using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000117 RID: 279
	public class BehaviorFireFromInfantryCover : BehaviorComponent
	{
		// Token: 0x06000E1C RID: 3612 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		public BehaviorFireFromInfantryCover(Formation formation)
			: base(formation)
		{
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0001F318 File Offset: 0x0001D518
		protected unsafe override void CalculateCurrentOrder()
		{
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			Vec2 vec = base.Formation.Direction;
			if (this._mainFormation == null)
			{
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				MovementOrder movementOrder = *this._mainFormation.GetReadonlyMovementOrderReference();
				Vec2 position = movementOrder.GetPosition(this._mainFormation);
				if (position.IsValid)
				{
					vec = (position - this._mainFormation.CachedAveragePosition).Normalized();
					Vec2 vec2 = position - vec * this._mainFormation.Depth * 0.33f;
					cachedMedianPosition.SetVec2(vec2);
				}
				else
				{
					cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				}
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < 100f)
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderSquare);
			}
			Vec2 position = base.CurrentOrder.GetPosition(base.Formation);
			bool flag = base.Formation.CachedClosestEnemyFormation == null || this._mainFormation.CachedAveragePosition.DistanceSquared(base.Formation.CachedAveragePosition) <= base.Formation.Depth * base.Formation.Width || base.Formation.CachedAveragePosition.DistanceSquared(position) <= (this._mainFormation.Depth + base.Formation.Depth) * (this._mainFormation.Depth + base.Formation.Depth) * 0.25f;
			if (flag != this._isFireAtWill)
			{
				this._isFireAtWill = flag;
				base.Formation.SetFiringOrder(this._isFireAtWill ? FiringOrder.FiringOrderFireAtWill : FiringOrder.FiringOrderHoldYourFire);
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0001F548 File Offset: 0x0001D748
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			int num = (int)MathF.Sqrt((float)base.Formation.CountOfUnits);
			float num2 = (float)num * base.Formation.UnitDiameter + (float)(num - 1) * base.Formation.Interval;
			base.Formation.SetFormOrder(FormOrder.FormOrderCustom(num2), true);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0001F5E4 File Offset: 0x0001D7E4
		protected override float GetAiWeight()
		{
			if (this._mainFormation == null || !this._mainFormation.AI.IsMainFormation)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			if (this._mainFormation == null || base.Formation.AI.IsMainFormation || base.Formation.CachedClosestEnemyFormation == null || !base.Formation.QuerySystem.IsRangedFormation)
			{
				return 0f;
			}
			return 2f;
		}

		// Token: 0x04000360 RID: 864
		private Formation _mainFormation;

		// Token: 0x04000361 RID: 865
		private bool _isFireAtWill = true;
	}
}
