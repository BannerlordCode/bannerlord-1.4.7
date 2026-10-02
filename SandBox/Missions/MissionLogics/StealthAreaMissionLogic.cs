using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.AreaMarkers;
using SandBox.Objects.Usables;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000087 RID: 135
	public class StealthAreaMissionLogic : MissionLogic
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x000230B4 File Offset: 0x000212B4
		public MBReadOnlyList<Agent> AllyTroops
		{
			get
			{
				return this._allyTroops;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x000230BC File Offset: 0x000212BC
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x000230C4 File Offset: 0x000212C4
		public bool AllReinforcementsCalled { get; private set; }

		// Token: 0x06000541 RID: 1345 RVA: 0x000230F8 File Offset: 0x000212F8
		public bool IsSentry(Agent agent)
		{
			foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
			{
				foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
				{
					if (keyValuePair.Value.Contains(agent))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00023194 File Offset: 0x00021394
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			foreach (StealthAreaUsePoint stealthAreaUsePoint in base.Mission.MissionObjects.FindAllWithType<StealthAreaUsePoint>())
			{
				this._stealthAreaData.Add(new StealthAreaMissionLogic.StealthAreaData(stealthAreaUsePoint));
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000231FC File Offset: 0x000213FC
		private MBList<Agent> SpawnReinforcementAllyGroupTroops(StealthAreaMissionLogic.StealthAreaData triggeredStealthAreaData, StealthAreaMarker stealthAreaMarker)
		{
			StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate spawnReinforcementAllyTroopsEvent = this.SpawnReinforcementAllyTroopsEvent;
			return ((spawnReinforcementAllyTroopsEvent != null) ? spawnReinforcementAllyTroopsEvent(triggeredStealthAreaData, stealthAreaMarker) : null) ?? new MBList<Agent>();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0002321B File Offset: 0x0002141B
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			base.OnAgentBuild(agent, banner);
			this.CheckStealthAreaMarkerForAgent(agent);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0002322C File Offset: 0x0002142C
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			base.OnAgentTeamChanged(prevTeam, newTeam, agent);
			this.CheckStealthAreaMarkerForAgent(agent);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00023240 File Offset: 0x00021440
		private void CheckStealthAreaMarkerForAgent(Agent agent)
		{
			if (agent.IsHuman && agent.Team == Mission.Current.PlayerEnemyTeam)
			{
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						if (keyValuePair.Key.IsPositionInRange(agent.Position))
						{
							stealthAreaData.AddAgentToStealthAreaMarker(keyValuePair.Key, agent);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0002330C File Offset: 0x0002150C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectorAgent != null && affectorAgent.IsMainAgent)
			{
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						if (keyValuePair.Value.Contains(affectedAgent))
						{
							stealthAreaData.RemoveAgentFromStealthAreaMarker(keyValuePair.Key, affectedAgent);
						}
					}
				}
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000233C0 File Offset: 0x000215C0
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			if (usedObject is StealthAreaUsePoint)
			{
				if (this.IsInCombat())
				{
					return;
				}
				StealthAreaMissionLogic.StealthAreaData stealthAreaData = null;
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData2 in this._stealthAreaData)
				{
					if (stealthAreaData2.StealthAreaUsePoint == usedObject)
					{
						stealthAreaData = stealthAreaData2;
						break;
					}
				}
				if (stealthAreaData != null)
				{
					stealthAreaData.IsReinforcementCalled = true;
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						MBList<Agent> mblist = this.SpawnReinforcementAllyGroupTroops(stealthAreaData, keyValuePair.Key);
						this._allyTroops.AddRange(mblist);
					}
				}
			}
			this.AllReinforcementsCalled = this._stealthAreaData.All<StealthAreaMissionLogic.StealthAreaData>((StealthAreaMissionLogic.StealthAreaData x) => x.IsReinforcementCalled);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000234C4 File Offset: 0x000216C4
		private bool IsInCombat()
		{
			bool flag = false;
			foreach (Agent agent in Mission.Current.AllAgents)
			{
				if (agent.IsActive())
				{
					Agent.AIStateFlag aistateFlag = Agent.AIStateFlag.Alarmed;
					if ((agent.AIStateFlags & aistateFlag) == aistateFlag)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00023530 File Offset: 0x00021730
		public bool CheckIfAllStealthAreasAreTriggered()
		{
			return this._stealthAreaData.All<StealthAreaMissionLogic.StealthAreaData>((StealthAreaMissionLogic.StealthAreaData x) => x.IsStealthAreaTriggered);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0002355C File Offset: 0x0002175C
		public bool CheckIfAllStealthAreasReinforcementsAreCalled()
		{
			return this.AllReinforcementsCalled;
		}

		// Token: 0x040002C7 RID: 711
		private readonly MBList<StealthAreaMissionLogic.StealthAreaData> _stealthAreaData = new MBList<StealthAreaMissionLogic.StealthAreaData>();

		// Token: 0x040002C8 RID: 712
		private readonly Dictionary<string, Dictionary<string, int>> _agentSpawnTypes = new Dictionary<string, Dictionary<string, int>>();

		// Token: 0x040002C9 RID: 713
		private readonly MBList<Agent> _allyTroops = new MBList<Agent>();

		// Token: 0x040002CA RID: 714
		public StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate SpawnReinforcementAllyTroopsEvent;

		// Token: 0x02000187 RID: 391
		// (Invoke) Token: 0x06000ECB RID: 3787
		public delegate MBList<Agent> SpawnReinforcementAllyTroopsDelegate(StealthAreaMissionLogic.StealthAreaData triggeredStealthAreaData, StealthAreaMarker stealthAreaMarker);

		// Token: 0x02000188 RID: 392
		public class StealthAreaData
		{
			// Token: 0x06000ECE RID: 3790 RVA: 0x000668D4 File Offset: 0x00064AD4
			internal StealthAreaData(StealthAreaUsePoint stealthAreaUsePoint)
			{
				this.StealthAreaUsePoint = stealthAreaUsePoint;
				this.StealthAreaMarkers = new Dictionary<StealthAreaMarker, List<Agent>>();
				foreach (WeakGameEntity weakGameEntity in stealthAreaUsePoint.GameEntity.GetChildren())
				{
					if (weakGameEntity.HasScriptOfType<StealthAreaMarker>())
					{
						this.StealthAreaMarkers.Add(weakGameEntity.GetFirstScriptOfType<StealthAreaMarker>(), new List<Agent>());
					}
				}
			}

			// Token: 0x06000ECF RID: 3791 RVA: 0x0006695C File Offset: 0x00064B5C
			internal void AddAgentToStealthAreaMarker(StealthAreaMarker stealthAreaMarker, Agent agent)
			{
				this.StealthAreaMarkers[stealthAreaMarker].Add(agent);
			}

			// Token: 0x06000ED0 RID: 3792 RVA: 0x00066970 File Offset: 0x00064B70
			internal void RemoveAgentFromStealthAreaMarker(StealthAreaMarker stealthAreaMarker, Agent agent)
			{
				this.StealthAreaMarkers[stealthAreaMarker].Remove(agent);
				if (this.StealthAreaMarkers.All<KeyValuePair<StealthAreaMarker, List<Agent>>>((KeyValuePair<StealthAreaMarker, List<Agent>> x) => x.Value.IsEmpty<Agent>()))
				{
					this.StealthAreaUsePoint.EnableStealthAreaUsePoint();
					this.IsStealthAreaTriggered = true;
				}
			}

			// Token: 0x04000764 RID: 1892
			internal bool IsStealthAreaTriggered;

			// Token: 0x04000765 RID: 1893
			internal bool IsReinforcementCalled;

			// Token: 0x04000766 RID: 1894
			internal readonly StealthAreaUsePoint StealthAreaUsePoint;

			// Token: 0x04000767 RID: 1895
			internal readonly Dictionary<StealthAreaMarker, List<Agent>> StealthAreaMarkers;
		}
	}
}
