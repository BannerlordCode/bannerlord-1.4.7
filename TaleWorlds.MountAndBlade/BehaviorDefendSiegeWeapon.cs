using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000113 RID: 275
	public class BehaviorDefendSiegeWeapon : BehaviorComponent
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x0001DEDC File Offset: 0x0001C0DC
		public BehaviorDefendSiegeWeapon(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0001DEF6 File Offset: 0x0001C0F6
		public void SetDefensePositionFromTactic(WorldPosition defensePosition)
		{
			this._defensePosition = defensePosition;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0001DEFF File Offset: 0x0001C0FF
		public void SetDefendedSiegeWeaponFromTactic(SiegeWeapon siegeWeapon)
		{
			this._defendedSiegeWeapon = siegeWeapon;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0001DF08 File Offset: 0x0001C108
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0001DF64 File Offset: 0x0001C164
		protected override void CalculateCurrentOrder()
		{
			float num = 5f;
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
				FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
				if (this._defendedSiegeWeapon != null)
				{
					vec = cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - this._defendedSiegeWeapon.GameEntity.GlobalPosition.AsVec2;
					num = vec.Normalize();
					num = MathF.Min(num, 5f);
					float num2 = ((this._defendedSiegeWeapon.WaitEntity != null) ? (this._defendedSiegeWeapon.WaitEntity.GlobalPosition - this._defendedSiegeWeapon.GameEntity.GlobalPosition).Length : 3f);
					num = MathF.Max(num, num2);
				}
				else
				{
					vec = ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
				}
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
				WorldPosition defensePosition = this._defensePosition;
				defensePosition.SetVec2(this._defensePosition.AsVec2 + vec * num);
				base.CurrentOrder = MovementOrder.MovementOrderMove(defensePosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				return;
			}
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0001E25C File Offset: 0x0001C45C
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < 100f)
			{
				if (base.Formation.QuerySystem.HasShield)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
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

		// Token: 0x06000DFC RID: 3580 RVA: 0x0001E414 File Offset: 0x0001C614
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0001E47A File Offset: 0x0001C67A
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this._defensePosition = WorldPosition.Invalid;
			this._tacticalDefendPosition = null;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0001E494 File Offset: 0x0001C694
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x04000351 RID: 849
		private WorldPosition _defensePosition = WorldPosition.Invalid;

		// Token: 0x04000352 RID: 850
		private TacticalPosition _tacticalDefendPosition;

		// Token: 0x04000353 RID: 851
		private SiegeWeapon _defendedSiegeWeapon;
	}
}
