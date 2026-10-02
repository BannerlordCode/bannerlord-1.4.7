using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000110 RID: 272
	public class BehaviorDefend : BehaviorComponent
	{
		// Token: 0x06000DDB RID: 3547 RVA: 0x0001CEA9 File Offset: 0x0001B0A9
		public BehaviorDefend(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec;
			if (this.TacticalDefendPosition != null)
			{
				vec = ((!this.TacticalDefendPosition.IsInsurmountable) ? this.TacticalDefendPosition.Direction : (base.Formation.Team.QuerySystem.AverageEnemyPosition - this.TacticalDefendPosition.Position.AsVec2).Normalized());
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
				if (!this.TacticalDefendPosition.IsInsurmountable)
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(this.TacticalDefendPosition.Position);
				}
				else
				{
					Vec2 vec2 = this.TacticalDefendPosition.Position.AsVec2 + this.TacticalDefendPosition.Width * 0.5f * vec;
					WorldPosition position = this.TacticalDefendPosition.Position;
					position.SetVec2(vec2);
					base.CurrentOrder = MovementOrder.MovementOrderMove(position);
				}
				this.CurrentFacingOrder = ((!this.TacticalDefendPosition.IsInsurmountable) ? FacingOrder.FacingOrderLookAtDirection(vec) : FacingOrder.FacingOrderLookAtEnemy);
				return;
			}
			if (this.DefensePosition.IsValid)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this.DefensePosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				return;
			}
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < 100f)
			{
				if (base.Formation.QuerySystem.HasShield)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				}
				else if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) > 100f && base.Formation.QuerySystem.UnderRangedAttackRatio > 0.2f - ((base.Formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Loose) ? 0.1f : 0f))
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
				}
				if (this.TacticalDefendPosition != null)
				{
					float num;
					if (this.TacticalDefendPosition.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.ChokePoint)
					{
						num = this.TacticalDefendPosition.Width;
					}
					else
					{
						int countOfUnits = base.Formation.CountOfUnits;
						float num2 = base.Formation.Interval * (float)(countOfUnits - 1) + base.Formation.UnitDiameter * (float)countOfUnits;
						num = MathF.Min(this.TacticalDefendPosition.Width, num2 / 3f);
					}
					base.Formation.SetFormOrder(FormOrder.FormOrderCustom(num), true);
					return;
				}
			}
			else
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0001D2A0 File Offset: 0x0001B4A0
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0001D306 File Offset: 0x0001B506
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this.DefensePosition = WorldPosition.Invalid;
			this.TacticalDefendPosition = null;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0001D320 File Offset: 0x0001B520
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400033F RID: 831
		public WorldPosition DefensePosition = WorldPosition.Invalid;

		// Token: 0x04000340 RID: 832
		public TacticalPosition TacticalDefendPosition;
	}
}
