using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.CampaignBehaviors;
using SandBox.Objects;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace SandBox.Missions
{
	// Token: 0x0200005A RID: 90
	public class CheckpointMissionLogic : MissionLogic
	{
		// Token: 0x06000383 RID: 899 RVA: 0x000144EF File Offset: 0x000126EF
		public CheckpointMissionLogic()
		{
			this._allSpawnedSaveableAgents = new Dictionary<Agent, AgentSaveData>();
			this._checkpointCampaignBehavior = Campaign.Current.GetCampaignBehavior<CheckpointCampaignBehavior>();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00014512 File Offset: 0x00012712
		public override void EarlyStart()
		{
			this.DisablePatrolAreasAccordingToTheLastUsedCheckpoint();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001451A File Offset: 0x0001271A
		public override void OnRenderingStarted()
		{
			this._isRenderingStarted = true;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00014524 File Offset: 0x00012724
		public override void OnEarlyAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.Team == Mission.Current.PlayerEnemyTeam && agentState == AgentState.Killed)
			{
				foreach (KeyValuePair<Agent, AgentSaveData> keyValuePair in this._allSpawnedSaveableAgents)
				{
					if (keyValuePair.Key == affectedAgent)
					{
						AgentSaveData value = keyValuePair.Value;
						Mat3 lookRotation = keyValuePair.Key.LookRotation;
						Vec3 position = keyValuePair.Key.Position;
						value.UpdateSpawnFrame(new MatrixFrame(in lookRotation, in position));
						break;
					}
				}
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000145C8 File Offset: 0x000127C8
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this._isInitialized && Agent.Main != null && this._isRenderingStarted)
			{
				this._isInitialized = true;
				if (this._checkpointCampaignBehavior.LastUsedMissionCheckpointId >= 0)
				{
					List<GameEntity> list = new List<GameEntity>();
					Mission.Current.Scene.GetAllEntitiesWithScriptComponent<CheckpointArea>(ref list);
					CheckpointArea checkpointArea = null;
					foreach (GameEntity gameEntity in list)
					{
						CheckpointArea firstScriptOfType = gameEntity.GetFirstScriptOfType<CheckpointArea>();
						if (firstScriptOfType.UniqueId == this._checkpointCampaignBehavior.LastUsedMissionCheckpointId)
						{
							checkpointArea = firstScriptOfType;
							Vec3 globalPosition = checkpointArea.SpawnPoint.GlobalPosition;
							Agent.Main.TeleportToPosition(globalPosition);
							break;
						}
					}
					if (checkpointArea == null)
					{
						List<GameEntity> list2 = new List<GameEntity>();
						Mission.Current.Scene.GetAllEntitiesWithScriptComponent<CheckpointUsePoint>(ref list2);
						foreach (GameEntity gameEntity2 in list2)
						{
							CheckpointUsePoint firstScriptOfType2 = gameEntity2.GetFirstScriptOfType<CheckpointUsePoint>();
							if (firstScriptOfType2.UniqueId == this._checkpointCampaignBehavior.LastUsedMissionCheckpointId)
							{
								Vec3 globalPosition2 = firstScriptOfType2.SpawnPoint.GlobalPosition;
								Agent.Main.TeleportToPosition(globalPosition2);
								break;
							}
						}
					}
					Game.Current.EventManager.TriggerEvent<CheckpointLoadedMissionEvent>(new CheckpointLoadedMissionEvent(this._checkpointCampaignBehavior.LastUsedMissionCheckpointId));
				}
				this.SpawnCorpses();
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001474C File Offset: 0x0001294C
		private bool CanUseCheckpoint()
		{
			bool flag = true;
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.Team == Mission.Current.PlayerEnemyTeam && (agent.IsCautious() || agent.IsPatrollingCautious() || agent.IsAlarmed()))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000147D0 File Offset: 0x000129D0
		public void OnCheckpointUsed(int checkpointUniqueId)
		{
			if (this.CanUseCheckpoint())
			{
				this._checkpointCampaignBehavior.LastUsedMissionCheckpointId = checkpointUniqueId;
				this._checkpointCampaignBehavior.CorpseList.Clear();
				foreach (KeyValuePair<Agent, AgentSaveData> keyValuePair in this._allSpawnedSaveableAgents)
				{
					if (keyValuePair.Key.State == AgentState.Killed || keyValuePair.Key.State == AgentState.Unconscious)
					{
						this._checkpointCampaignBehavior.CorpseList.Add(keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00014878 File Offset: 0x00012A78
		private void DisablePatrolAreasAccordingToTheLastUsedCheckpoint()
		{
			if (!this._checkpointCampaignBehavior.CorpseList.IsEmpty<AgentSaveData>())
			{
				List<GameEntity> list = new List<GameEntity>();
				Mission.Current.Scene.GetAllEntitiesWithScriptComponent<DynamicPatrolAreaParent>(ref list);
				foreach (AgentSaveData agentSaveData in this._checkpointCampaignBehavior.CorpseList)
				{
					foreach (GameEntity gameEntity in list)
					{
						foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
						{
							if (gameEntity2.GetChild(0).Tags.SequenceEqual<string>(agentSaveData.AgentSpawnPointTags))
							{
								gameEntity2.GetChild(0).GetFirstScriptOfType<PatrolPoint>().SetDisabled(true);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00014998 File Offset: 0x00012B98
		private void SpawnCorpses()
		{
			foreach (AgentSaveData agentSaveData in this._checkpointCampaignBehavior.CorpseList)
			{
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>(agentSaveData.CharacterStringId);
				AgentBuildData agentBuildData = new AgentBuildData(@object).TroopOrigin(new SimpleAgentOrigin(@object, -1, null, default(UniqueTroopDescriptor))).EquipmentSeed(agentSaveData.AgentSeed).InitialPosition(in agentSaveData.SpawnFrame.origin);
				Vec3 f = agentSaveData.SpawnFrame.rotation.f;
				Vec2 asVec = f.NormalizedCopy().AsVec2;
				AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec);
				Agent agent = Mission.Current.SpawnAgent(agentBuildData2, false);
				agent.MakeDead(true, ActionIndexCache.act_none, -1);
				GameEntity entity = agent.AgentVisuals.GetEntity();
				foreach (string text in agentSaveData.AgentSpawnPointTags)
				{
					entity.AddTag(text);
				}
				this.RegisterAgent(agent);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00014AC4 File Offset: 0x00012CC4
		public void RegisterAgent(Agent agent)
		{
			Dictionary<Agent, AgentSaveData> allSpawnedSaveableAgents = this._allSpawnedSaveableAgents;
			string stringId = agent.Character.StringId;
			Mat3 lookRotation = agent.LookRotation;
			Vec3 position = agent.Position;
			allSpawnedSaveableAgents.Add(agent, new AgentSaveData(stringId, new MatrixFrame(in lookRotation, in position), agent.AgentVisuals.GetEntity().Tags, agent.Origin.Seed));
		}

		// Token: 0x040001CD RID: 461
		private readonly Dictionary<Agent, AgentSaveData> _allSpawnedSaveableAgents;

		// Token: 0x040001CE RID: 462
		private readonly CheckpointCampaignBehavior _checkpointCampaignBehavior;

		// Token: 0x040001CF RID: 463
		private bool _isInitialized;

		// Token: 0x040001D0 RID: 464
		private bool _isRenderingStarted;
	}
}
