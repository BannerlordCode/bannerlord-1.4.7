using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000132 RID: 306
	public class BehaviorSkirmishLine : BehaviorComponent
	{
		// Token: 0x06000EDF RID: 3807 RVA: 0x000265FC File Offset: 0x000247FC
		public BehaviorSkirmishLine(Formation formation)
			: base(formation)
		{
			this._behaviorSide = FormationAI.BehaviorSide.BehaviorSideNotSet;
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00026654 File Offset: 0x00024854
		protected override void CalculateCurrentOrder()
		{
			this._targetFormation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation ?? base.Formation.CachedClosestEnemyFormation;
			Vec2 vec;
			WorldPosition worldPosition;
			if (this._targetFormation == null || this._mainFormation == null)
			{
				vec = base.Formation.Direction;
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				if (this._mainFormation.AI.ActiveBehavior is BehaviorCautiousAdvance)
				{
					vec = this._mainFormation.Direction;
				}
				else
				{
					vec = ((base.Formation.Direction.DotProduct((this._targetFormation.Formation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2).Normalized()) < 0.5f) ? (this._targetFormation.Formation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2) : base.Formation.Direction).Normalized();
				}
				Vec2 vec2 = this._mainFormation.OrderPosition - this._mainFormation.CachedMedianPosition.AsVec2;
				float num = this._mainFormation.CachedMovementSpeed * 7f;
				float length = vec2.Length;
				if (length > 0f)
				{
					float num2 = num / length;
					if (num2 < 1f)
					{
						vec2 *= num2;
					}
				}
				worldPosition = this._mainFormation.CachedMedianPosition;
				worldPosition.SetVec2(worldPosition.AsVec2 + vec * 8f + vec2);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			if (!this.CurrentFacingOrder.GetDirection(base.Formation, null).IsValid || this.CurrentFacingOrder.OrderEnum == FacingOrder.FacingOrderEnum.LookAtEnemy || (this._targetFormation != null && (base.Formation.CachedAveragePosition.DistanceSquared(this._targetFormation.Formation.CachedMedianPosition.GetNavMeshVec3().AsVec2) >= base.Formation.QuerySystem.MissileRangeAdjusted * base.Formation.QuerySystem.MissileRangeAdjusted || (!this._targetFormation.IsRangedCavalryFormation && this.CurrentFacingOrder.GetDirection(base.Formation, null).DotProduct(vec) <= MBMath.Lerp(0.5f, 1f, 1f - MBMath.ClampFloat(base.Formation.Width, 1f, 20f) * 0.05f, 1E-05f)))))
			{
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00026930 File Offset: 0x00024B30
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			if (this._mainFormation != null)
			{
				behaviorString.SetTextVariable("AI_SIDE", GameTexts.FindText("str_formation_ai_side_strings", this._mainFormation.AI.Side.ToString()));
				behaviorString.SetTextVariable("CLASS", GameTexts.FindText("str_formation_class_string", this._mainFormation.PhysicalClass.GetName()));
			}
			return behaviorString;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x000269A8 File Offset: 0x00024BA8
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000269F8 File Offset: 0x00024BF8
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._mainFormation != null && base.Formation.Width > this._mainFormation.Width * 1.5f)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._mainFormation.Width * 1.2f), true);
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00026A78 File Offset: 0x00024C78
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWider, true);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00026AE0 File Offset: 0x00024CE0
		protected override float GetAiWeight()
		{
			if (this._mainFormation == null || !this._mainFormation.AI.IsMainFormation)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			if (this._behaviorSide != base.Formation.AI.Side)
			{
				this._behaviorSide = base.Formation.AI.Side;
			}
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (this._mainFormation == null || base.Formation.AI.IsMainFormation || cachedClosestEnemyFormation == null)
			{
				return 0f;
			}
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			float num = MBMath.Lerp(0.1f, 1f, MBMath.ClampFloat(querySystem.RangedUnitRatio + querySystem.RangedCavalryUnitRatio, 0f, 0.5f) * 2f, 1E-05f);
			float num2 = base.Formation.CachedAveragePosition.Distance((querySystem.ClosestSignificantlyLargeEnemyFormation ?? cachedClosestEnemyFormation).Formation.CachedMedianPosition.AsVec2) / (querySystem.ClosestSignificantlyLargeEnemyFormation ?? cachedClosestEnemyFormation).MovementSpeedMaximum;
			float num3 = MBMath.Lerp(0.5f, 1.2f, (MBMath.ClampFloat(num2, 4f, 8f) - 4f) / 4f, 1E-05f);
			return num * querySystem.MainFormationReliabilityFactor * num3;
		}

		// Token: 0x04000398 RID: 920
		private Formation _mainFormation;

		// Token: 0x04000399 RID: 921
		private FormationQuerySystem _targetFormation;
	}
}
