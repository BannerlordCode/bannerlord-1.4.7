using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012E RID: 302
	public class BehaviorShootFromCliff : BehaviorComponent
	{
		// Token: 0x06000EC8 RID: 3784 RVA: 0x00025120 File Offset: 0x00023320
		public BehaviorShootFromCliff(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0002513A File Offset: 0x0002333A
		public void SetTacticalDefendPosition(TacticalPosition tacticalPosition)
		{
			this._tacticalDefendPosition = tacticalPosition;
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00025144 File Offset: 0x00023344
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec;
			if (this._tacticalDefendPosition != null)
			{
				vec = ((!this._tacticalDefendPosition.IsInsurmountable) ? this._tacticalDefendPosition.Direction : (base.Formation.Team.QuerySystem.AverageEnemyPosition - this._tacticalDefendPosition.Position.AsVec2).Normalized());
			}
			else if (base.Formation.CachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				vec = ((base.Formation.Direction.DotProduct((base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			if (this._tacticalDefendPosition != null)
			{
				if (!this._tacticalDefendPosition.IsInsurmountable)
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(this._tacticalDefendPosition.Position);
				}
				else
				{
					Vec2 vec2 = this._tacticalDefendPosition.Position.AsVec2 + this._tacticalDefendPosition.Width * 0.5f * vec;
					WorldPosition position = this._tacticalDefendPosition.Position;
					position.SetVec2(vec2);
					base.CurrentOrder = MovementOrder.MovementOrderMove(position);
				}
				this.CurrentFacingOrder = ((!this._tacticalDefendPosition.IsInsurmountable) ? FacingOrder.FacingOrderLookAtDirection(vec) : FacingOrder.FacingOrderLookAtEnemy);
				return;
			}
			if (this._defensePosition.IsValid)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this._defensePosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				return;
			}
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00025368 File Offset: 0x00023568
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
				if (this._tacticalDefendPosition != null)
				{
					float num;
					if (this._tacticalDefendPosition.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.ChokePoint)
					{
						num = this._tacticalDefendPosition.Width;
					}
					else
					{
						int countOfUnits = base.Formation.CountOfUnits;
						float num2 = base.Formation.Interval * (float)(countOfUnits - 1) + base.Formation.UnitDiameter * (float)countOfUnits;
						num = MathF.Min(this._tacticalDefendPosition.Width, num2 / 3f);
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

		// Token: 0x06000ECC RID: 3788 RVA: 0x00025520 File Offset: 0x00023720
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00025586 File Offset: 0x00023786
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this._defensePosition = WorldPosition.Invalid;
			this._tacticalDefendPosition = null;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000255A0 File Offset: 0x000237A0
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400038C RID: 908
		private WorldPosition _defensePosition = WorldPosition.Invalid;

		// Token: 0x0400038D RID: 909
		private TacticalPosition _tacticalDefendPosition;
	}
}
