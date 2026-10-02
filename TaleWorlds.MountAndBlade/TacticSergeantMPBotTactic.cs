using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000175 RID: 373
	public class TacticSergeantMPBotTactic : TacticComponent
	{
		// Token: 0x0600137D RID: 4989 RVA: 0x00048797 File Offset: 0x00046997
		public TacticSergeantMPBotTactic(Team team)
			: base(team)
		{
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000487A0 File Offset: 0x000469A0
		public override void TickOccasionally()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
					formation.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPInfantry>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPRanged>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPMounted>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPMountedRanged>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPLastFlagLastStand>(1f);
				}
			}
		}
	}
}
