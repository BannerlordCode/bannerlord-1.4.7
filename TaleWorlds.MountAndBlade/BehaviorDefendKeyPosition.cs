using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000112 RID: 274
	public class BehaviorDefendKeyPosition : BehaviorComponent
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0001DC6B File Offset: 0x0001BE6B
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x0001DC78 File Offset: 0x0001BE78
		public WorldPosition DefensePosition
		{
			get
			{
				return this._behaviorPosition.Value;
			}
			set
			{
				this._defensePosition = value;
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0001DC84 File Offset: 0x0001BE84
		public BehaviorDefendKeyPosition(Formation formation)
			: base(formation)
		{
			this._behaviorPosition = new QueryData<WorldPosition>(() => Mission.Current.FindBestDefendingPosition(this.EnemyClusterPosition, this._defensePosition), 5f);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0001DCD0 File Offset: 0x0001BED0
		protected override void CalculateCurrentOrder()
		{
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			Vec2 vec;
			if (cachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				vec = ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			if (this.DefensePosition.IsValid)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this.DefensePosition);
			}
			else
			{
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			}
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0001DDDC File Offset: 0x0001BFDC
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.QuerySystem.HasShield && base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < base.Formation.Depth * base.Formation.Depth * 4f)
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				return;
			}
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0001DE8A File Offset: 0x0001C08A
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0001DEBD File Offset: 0x0001C0BD
		protected override float GetAiWeight()
		{
			return 10f;
		}

		// Token: 0x0400034E RID: 846
		private WorldPosition _defensePosition = WorldPosition.Invalid;

		// Token: 0x0400034F RID: 847
		public WorldPosition EnemyClusterPosition = WorldPosition.Invalid;

		// Token: 0x04000350 RID: 848
		private readonly QueryData<WorldPosition> _behaviorPosition;
	}
}
