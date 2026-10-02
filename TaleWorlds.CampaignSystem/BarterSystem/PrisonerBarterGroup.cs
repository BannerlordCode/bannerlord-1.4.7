using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x0200047A RID: 1146
	public class PrisonerBarterGroup : BarterGroup
	{
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x060048E1 RID: 18657 RVA: 0x00172BA5 File Offset: 0x00170DA5
		public override float AIDecisionWeight
		{
			get
			{
				return 0.7f;
			}
		}
	}
}
