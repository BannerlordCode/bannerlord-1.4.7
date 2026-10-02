using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000119 RID: 281
	public class BehaviorGeneral : BehaviorComponent
	{
		// Token: 0x06000E27 RID: 3623 RVA: 0x0001F990 File Offset: 0x0001DB90
		public BehaviorGeneral(Formation formation)
			: base(formation)
		{
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0001F9E0 File Offset: 0x0001DBE0
		protected override void CalculateCurrentOrder()
		{
			bool flag = false;
			bool flag2 = false;
			foreach (Formation formation in base.Formation.Team.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					flag = true;
					if (formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Retreat)
					{
						flag2 = false;
						break;
					}
					flag2 = true;
				}
			}
			if (!flag)
			{
				base.CurrentOrder = MovementOrder.MovementOrderCharge;
				return;
			}
			if (flag2)
			{
				base.CurrentOrder = MovementOrder.MovementOrderRetreat;
				return;
			}
			bool flag3 = false;
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team) && team.HasAnyFormationsIncludingSpecialThatIsNotEmpty())
				{
					flag3 = true;
					break;
				}
			}
			WorldPosition worldPosition;
			if (flag3 && base.Formation.Team.HasAnyFormationsIncludingSpecialThatIsNotEmpty())
			{
				float num = ((base.Formation.PhysicalClass.IsMounted() && base.Formation.Team.QuerySystem.CavalryRatio + base.Formation.Team.QuerySystem.RangedCavalryRatio >= 33.3f) ? 40f : 3f);
				if (this._mainFormation != null && this._mainFormation.CountOfUnits > 0)
				{
					float num2 = this._mainFormation.Depth + num;
					worldPosition = this._mainFormation.CachedMedianPosition;
					worldPosition.SetVec2(worldPosition.AsVec2 - (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2).Normalized() * num2);
				}
				else
				{
					worldPosition = base.Formation.QuerySystem.Team.MedianPosition;
					worldPosition.SetVec2(base.Formation.QuerySystem.Team.AveragePosition - (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - base.Formation.QuerySystem.Team.AveragePosition).Normalized() * num);
				}
			}
			else
			{
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0001FC90 File Offset: 0x0001DE90
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0001FCAC File Offset: 0x0001DEAC
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0001FD14 File Offset: 0x0001DF14
		protected override float GetAiWeight()
		{
			if (this._mainFormation == null || !this._mainFormation.AI.IsMainFormation)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			return 1.2f;
		}

		// Token: 0x04000362 RID: 866
		private Formation _mainFormation;
	}
}
