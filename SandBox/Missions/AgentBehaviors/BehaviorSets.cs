using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A4 RID: 164
	public class BehaviorSets
	{
		// Token: 0x060006E1 RID: 1761 RVA: 0x0002E948 File Offset: 0x0002CB48
		private static void AddBehaviorGroups(IAgent agent)
		{
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.AddBehaviorGroup<DailyBehaviorGroup>();
			agentNavigator.AddBehaviorGroup<InterruptingBehaviorGroup>();
			agentNavigator.AddBehaviorGroup<AlarmedBehaviorGroup>();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002E96E File Offset: 0x0002CB6E
		public static void AddQuestCharacterBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<WalkingBehavior>();
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FleeBehavior>();
			behaviorGroup.AddBehavior<FightBehavior>();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002E9A4 File Offset: 0x0002CBA4
		public static void AddWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<WalkingBehavior>();
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FleeBehavior>();
			behaviorGroup.AddBehavior<FightBehavior>();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002E9DC File Offset: 0x0002CBDC
		public static void AddOutdoorWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			DailyBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			behaviorGroup.AddBehavior<WalkingBehavior>().SetIndoorWandering(false);
			behaviorGroup.AddBehavior<ChangeLocationBehavior>();
			AlarmedBehaviorGroup behaviorGroup2 = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup2.AddBehavior<FleeBehavior>();
			behaviorGroup2.AddBehavior<FightBehavior>();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002EA29 File Offset: 0x0002CC29
		public static void AddIndoorWandererBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<WalkingBehavior>().SetOutdoorWandering(false);
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FleeBehavior>();
			behaviorGroup.AddBehavior<FightBehavior>();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002EA64 File Offset: 0x0002CC64
		public static void AddFixedCharacterBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			WalkingBehavior walkingBehavior = agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<WalkingBehavior>();
			walkingBehavior.SetIndoorWandering(false);
			walkingBehavior.SetOutdoorWandering(false);
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FleeBehavior>();
			behaviorGroup.AddBehavior<FightBehavior>();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002EAB1 File Offset: 0x0002CCB1
		public static void AddPatrollingThugBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<PatrolAgentBehavior>();
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FleeBehavior>();
			behaviorGroup.AddBehavior<FightBehavior>();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002EAE7 File Offset: 0x0002CCE7
		public static void AddStandGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<StandGuardBehavior>();
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FightBehavior>();
			behaviorGroup.DisableCalmDown = true;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002EB1D File Offset: 0x0002CD1D
		public static void AddFixedGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().AddBehavior<FightBehavior>();
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002EB40 File Offset: 0x0002CD40
		public static void StealthAgentBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.AddBehaviorGroup<DailyBehaviorGroup>();
			agentNavigator.AddBehaviorGroup<AlarmedBehaviorGroup>();
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<CautiousBehavior>();
			behaviorGroup.AddBehavior<FightBehavior>();
			if (agent.Character.StringId == "disguise_officer_character")
			{
				behaviorGroup.SetCanMoveWhenCautious(false);
			}
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<PatrolAgentBehavior>();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002EBAF File Offset: 0x0002CDAF
		public static void AddPatrollingGuardBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<PatrollingGuardBehavior>();
			AlarmedBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
			behaviorGroup.AddBehavior<FightBehavior>();
			behaviorGroup.DisableCalmDown = true;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002EBE5 File Offset: 0x0002CDE5
		public static void AddCompanionBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().AddBehavior<WalkingBehavior>().SetIndoorWandering(false);
			agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().AddBehavior<FightBehavior>();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002EC19 File Offset: 0x0002CE19
		public static void AddBodyguardBehaviors(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			DailyBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			behaviorGroup.AddBehavior<WalkingBehavior>();
			behaviorGroup.AddBehavior<FollowAgentBehavior>().SetTargetAgent(Agent.Main);
			agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().AddBehavior<FightBehavior>();
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002EC58 File Offset: 0x0002CE58
		public static void AddFirstCompanionBehavior(IAgent agent)
		{
			BehaviorSets.AddBehaviorGroups(agent);
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().AddBehavior<FightBehavior>();
		}
	}
}
