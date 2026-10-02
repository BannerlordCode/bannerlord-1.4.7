using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000200 RID: 512
	public abstract class IncidentModel : MBGameModel<IncidentModel>
	{
		// Token: 0x06001FA6 RID: 8102
		public abstract CampaignTime GetMinGlobalCooldownTime();

		// Token: 0x06001FA7 RID: 8103
		public abstract CampaignTime GetMaxGlobalCooldownTime();

		// Token: 0x06001FA8 RID: 8104
		public abstract float GetIncidentTriggerGlobalProbability();

		// Token: 0x06001FA9 RID: 8105
		public abstract float GetIncidentTriggerProbabilityDuringSiege();

		// Token: 0x06001FAA RID: 8106
		public abstract float GetIncidentTriggerProbabilityDuringWait();
	}
}
