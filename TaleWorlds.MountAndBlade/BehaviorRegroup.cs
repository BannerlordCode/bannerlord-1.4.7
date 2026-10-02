using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000120 RID: 288
	public class BehaviorRegroup : BehaviorComponent
	{
		// Token: 0x06000E4E RID: 3662 RVA: 0x000219AB File Offset: 0x0001FBAB
		public BehaviorRegroup(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 1f;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x000219C8 File Offset: 0x0001FBC8
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec = ((base.Formation.CachedClosestEnemyFormation != null) ? (base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized() : base.Formation.Direction);
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00021A5B File Offset: 0x0001FC5B
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00021A88 File Offset: 0x0001FC88
		protected override float GetAiWeight()
		{
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			if (base.Formation.AI.ActiveBehavior == null)
			{
				return 0f;
			}
			float behaviorCoherence = base.Formation.AI.ActiveBehavior.BehaviorCoherence;
			return MBMath.Lerp(0.1f, 1.2f, MBMath.ClampFloat(behaviorCoherence * (base.Formation.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents + 1f) / (querySystem.IdealAverageDisplacement + 1f), 0f, 3f) / 3f, 1E-05f);
		}
	}
}
