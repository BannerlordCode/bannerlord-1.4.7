using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000131 RID: 305
	public class BehaviorSkirmishBehindFormation : BehaviorComponent
	{
		// Token: 0x06000ED9 RID: 3801 RVA: 0x0002607F File Offset: 0x0002427F
		public BehaviorSkirmishBehindFormation(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x000260A8 File Offset: 0x000242A8
		protected override void CalculateCurrentOrder()
		{
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			Vec2 vec;
			if (cachedClosestEnemyFormation != null)
			{
				vec = ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) > 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			else
			{
				vec = base.Formation.Direction;
			}
			WorldPosition worldPosition;
			if (this.ReferenceFormation == null)
			{
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				worldPosition = this.ReferenceFormation.CachedMedianPosition;
				worldPosition.SetVec2(worldPosition.AsVec2 - vec * ((this.ReferenceFormation.Depth + base.Formation.Depth) * 0.5f));
			}
			if (base.CurrentOrder.GetPosition(base.Formation).IsValid)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			}
			else
			{
				FormationQuerySystem closestSignificantlyLargeEnemyFormation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
				if ((closestSignificantlyLargeEnemyFormation != null && (!closestSignificantlyLargeEnemyFormation.IsRangedCavalryFormation || base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.GetNavMeshVec3().AsVec2) >= closestSignificantlyLargeEnemyFormation.MissileRangeAdjusted * closestSignificantlyLargeEnemyFormation.MissileRangeAdjusted)) || base.CurrentOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3().DistanceSquared(worldPosition.GetNavMeshVec3()) >= base.Formation.Depth * base.Formation.Depth)
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
				}
			}
			if (!this.CurrentFacingOrder.GetDirection(base.Formation, null).IsValid || this.CurrentFacingOrder.OrderEnum == FacingOrder.FacingOrderEnum.LookAtEnemy || base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null || base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.GetNavMeshVec3().AsVec2) >= base.Formation.QuerySystem.MissileRangeAdjusted * base.Formation.QuerySystem.MissileRangeAdjusted || (!base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedCavalryFormation && this.CurrentFacingOrder.GetDirection(base.Formation, null).DotProduct(vec) <= MBMath.Lerp(0.5f, 1f, 1f - MBMath.ClampFloat(base.Formation.Width, 1f, 20f) * 0.05f, 1E-05f)))
			{
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000263E0 File Offset: 0x000245E0
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			bool flag = cachedClosestEnemyFormation == null || this.ReferenceFormation.CachedMedianPosition.AsVec2.DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) <= base.Formation.CachedAveragePosition.DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) || base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) <= (this.ReferenceFormation.Depth + base.Formation.Depth) * (this.ReferenceFormation.Depth + base.Formation.Depth) * 0.25f;
			if (flag != this._isFireAtWill)
			{
				this._isFireAtWill = flag;
				base.Formation.SetFiringOrder(this._isFireAtWill ? FiringOrder.FiringOrderFireAtWill : FiringOrder.FiringOrderHoldYourFire);
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00026514 File Offset: 0x00024714
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWider, true);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002657C File Offset: 0x0002477C
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			if (this.ReferenceFormation != null)
			{
				behaviorString.SetTextVariable("AI_SIDE", GameTexts.FindText("str_formation_ai_side_strings", this.ReferenceFormation.AI.Side.ToString()));
				behaviorString.SetTextVariable("CLASS", GameTexts.FindText("str_formation_class_string", this.ReferenceFormation.PhysicalClass.GetName()));
			}
			return behaviorString;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000265F3 File Offset: 0x000247F3
		protected override float GetAiWeight()
		{
			return 10f;
		}

		// Token: 0x04000396 RID: 918
		public Formation ReferenceFormation;

		// Token: 0x04000397 RID: 919
		private bool _isFireAtWill = true;
	}
}
