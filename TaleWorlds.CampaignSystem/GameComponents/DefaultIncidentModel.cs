using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011F RID: 287
	public class DefaultIncidentModel : IncidentModel
	{
		// Token: 0x0600183F RID: 6207 RVA: 0x0007557F File Offset: 0x0007377F
		public override CampaignTime GetMinGlobalCooldownTime()
		{
			return CampaignTime.Days(8f);
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0007558B File Offset: 0x0007378B
		public override CampaignTime GetMaxGlobalCooldownTime()
		{
			return CampaignTime.Days(15f);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00075597 File Offset: 0x00073797
		public override float GetIncidentTriggerGlobalProbability()
		{
			return 0.5f;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0007559E File Offset: 0x0007379E
		public override float GetIncidentTriggerProbabilityDuringSiege()
		{
			return 0.143f;
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x000755A5 File Offset: 0x000737A5
		public override float GetIncidentTriggerProbabilityDuringWait()
		{
			return 0.143f;
		}
	}
}
