using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200011E RID: 286
	public class BehaviorProtectGeneral : BehaviorComponent
	{
		// Token: 0x06000E43 RID: 3651 RVA: 0x0002118C File Offset: 0x0001F38C
		public BehaviorProtectGeneral(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderFollow((formation.Team.GeneralsFormation != null && formation.Team.GeneralsFormation.CountOfUnits > 0) ? formation.Team.GeneralsFormation.GetFirstUnit() : Mission.Current.MainAgent);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x000211E7 File Offset: 0x0001F3E7
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x000211FA File Offset: 0x0001F3FA
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00021204 File Offset: 0x0001F404
		protected override float GetAiWeight()
		{
			if ((base.Formation.Team.GeneralsFormation != null && base.Formation.Team.GeneralsFormation.CountOfUnits > 0) || (base.Formation.Team.IsPlayerTeam && base.Formation.Team.IsPlayerGeneral && Mission.Current.MainAgent != null))
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00021278 File Offset: 0x0001F478
		public override void OnAgentRemoved(Agent agent)
		{
			if (base.CurrentOrder._targetAgent == agent)
			{
				base.CurrentOrder = MovementOrder.MovementOrderNull;
			}
		}
	}
}
