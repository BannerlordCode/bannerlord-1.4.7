using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SandBox.AI
{
	// Token: 0x0200010B RID: 267
	public class AgentBehaviorManager : IAgentBehaviorManager
	{
		// Token: 0x06000D52 RID: 3410 RVA: 0x000610F4 File Offset: 0x0005F2F4
		public void AddQuestCharacterBehaviors(IAgent agent)
		{
			BehaviorSets.AddQuestCharacterBehaviors(agent);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000610FC File Offset: 0x0005F2FC
		void IAgentBehaviorManager.AddWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddWandererBehaviors(agent);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00061104 File Offset: 0x0005F304
		void IAgentBehaviorManager.AddOutdoorWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddOutdoorWandererBehaviors(agent);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0006110C File Offset: 0x0005F30C
		void IAgentBehaviorManager.AddIndoorWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddIndoorWandererBehaviors(agent);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00061114 File Offset: 0x0005F314
		void IAgentBehaviorManager.AddFixedCharacterBehaviors(IAgent agent)
		{
			BehaviorSets.AddFixedCharacterBehaviors(agent);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0006111C File Offset: 0x0005F31C
		void IAgentBehaviorManager.AddPatrollingThugBehaviors(IAgent agent)
		{
			BehaviorSets.AddPatrollingThugBehaviors(agent);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00061124 File Offset: 0x0005F324
		void IAgentBehaviorManager.AddStandGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddStandGuardBehaviors(agent);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0006112C File Offset: 0x0005F32C
		void IAgentBehaviorManager.AddFixedGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddFixedGuardBehaviors(agent);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00061134 File Offset: 0x0005F334
		void IAgentBehaviorManager.AddStealthAgentBehaviors(IAgent agent)
		{
			BehaviorSets.StealthAgentBehaviors(agent);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0006113C File Offset: 0x0005F33C
		void IAgentBehaviorManager.AddPatrollingGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddPatrollingGuardBehaviors(agent);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00061144 File Offset: 0x0005F344
		void IAgentBehaviorManager.AddCompanionBehaviors(IAgent agent)
		{
			BehaviorSets.AddCompanionBehaviors(agent);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0006114C File Offset: 0x0005F34C
		void IAgentBehaviorManager.AddBodyguardBehaviors(IAgent agent)
		{
			BehaviorSets.AddBodyguardBehaviors(agent);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00061154 File Offset: 0x0005F354
		public void AddFirstCompanionBehavior(IAgent agent)
		{
			BehaviorSets.AddFirstCompanionBehavior(agent);
		}
	}
}
