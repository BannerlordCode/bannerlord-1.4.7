using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000114 RID: 276
	public class BehaviorDefensiveRing : BehaviorComponent
	{
		// Token: 0x06000DFF RID: 3583 RVA: 0x0001E49B File Offset: 0x0001C69B
		public BehaviorDefensiveRing(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0001E4AC File Offset: 0x0001C6AC
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec;
			if (this.TacticalDefendPosition != null)
			{
				vec = this.TacticalDefendPosition.Direction;
			}
			else if (base.Formation.CachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				vec = ((base.Formation.Direction.DotProduct((base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			if (this.TacticalDefendPosition != null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this.TacticalDefendPosition.Position);
			}
			else
			{
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			}
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0001E5DC File Offset: 0x0001C7DC
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.Distance(base.CurrentOrder.GetPosition(base.Formation)) - base.Formation.Arrangement.Depth * 0.5f < 10f)
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderCircle);
				if (base.Formation.Team.FormationsIncludingEmpty.AnyQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation))
				{
					Formation formation = base.Formation.Team.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation).MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
					int num = (int)MathF.Sqrt((float)formation.CountOfUnits);
					float num2 = ((float)num * formation.UnitDiameter + (float)(num - 1) * formation.Interval) * 0.5f * 1.414213f;
					int i = base.Formation.Arrangement.UnitCount;
					int num3 = 0;
					while (i > 0)
					{
						double num4 = (double)(num2 + base.Formation.Distance * (float)num3 + base.Formation.UnitDiameter * (float)(num3 + 1)) * 3.141592653589793 * 2.0 / (double)(base.Formation.UnitDiameter + base.Formation.Interval);
						i -= (int)Math.Ceiling(num4);
						num3++;
					}
					float num5 = num2 + (float)num3 * base.Formation.UnitDiameter + (float)(num3 - 1) * base.Formation.Distance;
					base.Formation.SetFormOrder(FormOrder.FormOrderCustom(num5 * 2f), true);
					return;
				}
			}
			else
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0001E800 File Offset: 0x0001CA00
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0001E866 File Offset: 0x0001CA66
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this.TacticalDefendPosition = null;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0001E875 File Offset: 0x0001CA75
		protected override float GetAiWeight()
		{
			if (this.TacticalDefendPosition == null)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x04000354 RID: 852
		public TacticalPosition TacticalDefendPosition;
	}
}
