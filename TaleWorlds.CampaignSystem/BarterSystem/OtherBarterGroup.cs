using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x0200047B RID: 1147
	public class OtherBarterGroup : BarterGroup
	{
		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x060048E3 RID: 18659 RVA: 0x00172BB4 File Offset: 0x00170DB4
		public override float AIDecisionWeight
		{
			get
			{
				return 0.25f;
			}
		}
	}
}
