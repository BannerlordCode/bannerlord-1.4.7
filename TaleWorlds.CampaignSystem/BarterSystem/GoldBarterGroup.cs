using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x02000477 RID: 1143
	public class GoldBarterGroup : BarterGroup
	{
		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x060048DB RID: 18651 RVA: 0x00172B78 File Offset: 0x00170D78
		public override float AIDecisionWeight
		{
			get
			{
				return 0.6f;
			}
		}
	}
}
