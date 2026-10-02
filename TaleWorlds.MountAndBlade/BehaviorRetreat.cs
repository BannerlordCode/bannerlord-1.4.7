using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000123 RID: 291
	public class BehaviorRetreat : BehaviorComponent
	{
		// Token: 0x06000E68 RID: 3688 RVA: 0x00022512 File Offset: 0x00020712
		public BehaviorRetreat(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderRetreat;
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00022531 File Offset: 0x00020731
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00022544 File Offset: 0x00020744
		protected override void OnBehaviorActivatedAux()
		{
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x00022546 File Offset: 0x00020746
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00022550 File Offset: 0x00020750
		protected override float GetAiWeight()
		{
			float casualtyPowerLossOfFormation = Mission.Current.GetMissionBehavior<CasualtyHandler>().GetCasualtyPowerLossOfFormation(base.Formation);
			float num = MathF.Sqrt(casualtyPowerLossOfFormation / (base.Formation.QuerySystem.FormationPower + casualtyPowerLossOfFormation));
			return MBMath.ClampFloat(base.Formation.Team.QuerySystem.TotalPowerRatio, 0.1f, 3f) / MBMath.ClampFloat(base.Formation.Team.QuerySystem.RemainingPowerRatio, 0.1f, 3f) * (0.05f + num);
		}
	}
}
