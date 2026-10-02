using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000090 RID: 144
	public interface IAgentBehaviorManager
	{
		// Token: 0x06001254 RID: 4692
		void AddQuestCharacterBehaviors(IAgent agent);

		// Token: 0x06001255 RID: 4693
		void AddWandererBehaviors(IAgent agent);

		// Token: 0x06001256 RID: 4694
		void AddOutdoorWandererBehaviors(IAgent agent);

		// Token: 0x06001257 RID: 4695
		void AddIndoorWandererBehaviors(IAgent agent);

		// Token: 0x06001258 RID: 4696
		void AddFixedCharacterBehaviors(IAgent agent);

		// Token: 0x06001259 RID: 4697
		void AddPatrollingThugBehaviors(IAgent agent);

		// Token: 0x0600125A RID: 4698
		void AddStandGuardBehaviors(IAgent agent);

		// Token: 0x0600125B RID: 4699
		void AddFixedGuardBehaviors(IAgent agent);

		// Token: 0x0600125C RID: 4700
		void AddStealthAgentBehaviors(IAgent agent);

		// Token: 0x0600125D RID: 4701
		void AddPatrollingGuardBehaviors(IAgent agent);

		// Token: 0x0600125E RID: 4702
		void AddCompanionBehaviors(IAgent agent);

		// Token: 0x0600125F RID: 4703
		void AddBodyguardBehaviors(IAgent agent);

		// Token: 0x06001260 RID: 4704
		void AddFirstCompanionBehavior(IAgent agent);
	}
}
