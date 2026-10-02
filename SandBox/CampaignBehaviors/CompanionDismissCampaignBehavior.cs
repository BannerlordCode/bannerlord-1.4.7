using System;
using SandBox.Conversation;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D5 RID: 213
	internal class CompanionDismissCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060009A5 RID: 2469 RVA: 0x0004830F File Offset: 0x0004650F
		public override void RegisterEvents()
		{
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00048328 File Offset: 0x00046528
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (LocationComplex.Current != null)
			{
				LocationComplex.Current.RemoveCharacterIfExists(companion);
			}
			if (PlayerEncounter.LocationEncounter != null)
			{
				PlayerEncounter.LocationEncounter.RemoveAccompanyingCharacter(companion);
			}
			if (detail == RemoveCompanionAction.RemoveCompanionDetail.Fire && Hero.MainHero.CurrentSettlement != null)
			{
				AgentNavigator agentNavigator = ConversationMission.OneToOneConversationAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
				if (((agentNavigator != null) ? agentNavigator.GetActiveBehavior() : null) is FollowAgentBehavior)
				{
					agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().RemoveBehavior<FollowAgentBehavior>();
				}
			}
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00048396 File Offset: 0x00046596
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
