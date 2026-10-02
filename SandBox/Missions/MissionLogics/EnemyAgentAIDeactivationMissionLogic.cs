using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006A RID: 106
	public class EnemyAgentAIDeactivationMissionLogic : MissionLogic
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0001A7BD File Offset: 0x000189BD
		public EnemyAgentAIDeactivationMissionLogic()
		{
			Game.Current.EventManager.RegisterEvent<LocationCharacterAgentSpawnedMissionEvent>(new Action<LocationCharacterAgentSpawnedMissionEvent>(this.OnLocationCharacterAgentSpawned));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001A7E0 File Offset: 0x000189E0
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<LocationCharacterAgentSpawnedMissionEvent>(new Action<LocationCharacterAgentSpawnedMissionEvent>(this.OnLocationCharacterAgentSpawned));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001A800 File Offset: 0x00018A00
		private void OnLocationCharacterAgentSpawned(LocationCharacterAgentSpawnedMissionEvent locationCharacterAgentSpawnedEvent)
		{
			Agent agent = locationCharacterAgentSpawnedEvent.Agent;
			if (agent.Team == Mission.Current.PlayerEnemyTeam)
			{
				DailyBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
				if (!behaviorGroup.HasBehavior<IdleAgentBehavior>())
				{
					behaviorGroup.AddBehavior<IdleAgentBehavior>();
				}
				behaviorGroup.SetScriptedBehavior<IdleAgentBehavior>();
			}
		}
	}
}
