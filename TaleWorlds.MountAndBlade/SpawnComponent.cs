using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C4 RID: 708
	public class SpawnComponent : MissionLogic
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x0009A51A File Offset: 0x0009871A
		// (set) Token: 0x060028AE RID: 10414 RVA: 0x0009A522 File Offset: 0x00098722
		public SpawnFrameBehaviorBase SpawnFrameBehavior { get; private set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x0009A52B File Offset: 0x0009872B
		// (set) Token: 0x060028B0 RID: 10416 RVA: 0x0009A533 File Offset: 0x00098733
		public SpawningBehaviorBase SpawningBehavior { get; private set; }

		// Token: 0x060028B1 RID: 10417 RVA: 0x0009A53C File Offset: 0x0009873C
		public SpawnComponent(SpawnFrameBehaviorBase spawnFrameBehavior, SpawningBehaviorBase spawningBehavior)
		{
			this.SpawnFrameBehavior = spawnFrameBehavior;
			this.SpawningBehavior = spawningBehavior;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x0009A552 File Offset: 0x00098752
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionMultiplayerGameModeBase = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0009A56B File Offset: 0x0009876B
		public bool AreAgentsSpawning()
		{
			return this.SpawningBehavior.AreAgentsSpawning();
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x0009A578 File Offset: 0x00098778
		public void SetNewSpawnFrameBehavior(SpawnFrameBehaviorBase spawnFrameBehavior)
		{
			this.SpawnFrameBehavior = spawnFrameBehavior;
			if (this.SpawnFrameBehavior != null)
			{
				this.SpawnFrameBehavior.Initialize();
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x0009A594 File Offset: 0x00098794
		public void SetNewSpawningBehavior(SpawningBehaviorBase spawningBehavior)
		{
			this.SpawningBehavior = spawningBehavior;
			if (this.SpawningBehavior != null)
			{
				this.SpawningBehavior.Initialize(this);
			}
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x0009A5B1 File Offset: 0x000987B1
		protected override void OnEndMission()
		{
			base.OnEndMission();
			this.SpawningBehavior.Clear();
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x0009A5C4 File Offset: 0x000987C4
		public static void SetSiegeSpawningBehavior()
		{
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawnFrameBehavior(new SiegeSpawnFrameBehavior());
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawningBehavior(new SiegeSpawningBehavior());
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x0009A5EE File Offset: 0x000987EE
		public static void SetFlagDominationSpawningBehavior()
		{
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawnFrameBehavior(new FlagDominationSpawnFrameBehavior());
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawningBehavior(new FlagDominationSpawningBehavior());
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x0009A618 File Offset: 0x00098818
		public static void SetWarmupSpawningBehavior()
		{
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawnFrameBehavior(new FFASpawnFrameBehavior());
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetNewSpawningBehavior(new WarmupSpawningBehavior());
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x0009A642 File Offset: 0x00098842
		public static void SetSpawningBehaviorForCurrentGameType(MultiplayerGameType currentGameType)
		{
			if (currentGameType == MultiplayerGameType.Siege)
			{
				SpawnComponent.SetSiegeSpawningBehavior();
				return;
			}
			if (currentGameType - MultiplayerGameType.Battle > 2)
			{
				return;
			}
			SpawnComponent.SetFlagDominationSpawningBehavior();
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x0009A65A File Offset: 0x0009885A
		public override void AfterStart()
		{
			base.AfterStart();
			this.SetNewSpawnFrameBehavior(this.SpawnFrameBehavior);
			this.SetNewSpawningBehavior(this.SpawningBehavior);
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x0009A67A File Offset: 0x0009887A
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this.SpawningBehavior.OnTick(dt);
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x0009A68F File Offset: 0x0009888F
		protected void StartSpawnSession()
		{
			this.SpawningBehavior.RequestStartSpawnSession();
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x0009A69C File Offset: 0x0009889C
		public MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn = false)
		{
			SpawnFrameBehaviorBase spawnFrameBehavior = this.SpawnFrameBehavior;
			if (spawnFrameBehavior == null)
			{
				return MatrixFrame.Identity;
			}
			return spawnFrameBehavior.GetSpawnFrame(team, hasMount, isInitialSpawn);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x0009A6B6 File Offset: 0x000988B6
		protected void SpawnEquipmentUpdated(MissionPeer lobbyPeer, Equipment equipment)
		{
			if (GameNetwork.IsServer && lobbyPeer != null && this.SpawningBehavior.CanUpdateSpawnEquipment(lobbyPeer) && lobbyPeer.HasSpawnedAgentVisuals)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new EquipEquipmentToPeer(lobbyPeer.GetNetworkPeer(), equipment));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x0009A6F5 File Offset: 0x000988F5
		public void SetEarlyAgentVisualsDespawning(MissionPeer missionPeer, bool canDespawnEarly = true)
		{
			if (missionPeer != null && this.AllowEarlyAgentVisualsDespawning(missionPeer))
			{
				missionPeer.EquipmentUpdatingExpired = canDespawnEarly;
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x0009A70A File Offset: 0x0009890A
		public void ToggleUpdatingSpawnEquipment(bool canUpdate)
		{
			this.SpawningBehavior.ToggleUpdatingSpawnEquipment(canUpdate);
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x0009A718 File Offset: 0x00098918
		public bool AllowEarlyAgentVisualsDespawning(MissionPeer lobbyPeer)
		{
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(lobbyPeer, false);
			return this._missionMultiplayerGameModeBase.IsClassAvailable(mpheroClassForPeer) && this.SpawningBehavior.AllowEarlyAgentVisualsDespawning(lobbyPeer);
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x0009A749 File Offset: 0x00098949
		public int GetMaximumReSpawnPeriodForPeer(MissionPeer lobbyPeer)
		{
			return this.SpawningBehavior.GetMaximumReSpawnPeriodForPeer(lobbyPeer);
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x0009A757 File Offset: 0x00098957
		public override void OnClearScene()
		{
			base.OnClearScene();
			this.SpawningBehavior.OnClearScene();
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x0009A76A File Offset: 0x0009896A
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			this.SpawningBehavior.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			this.SpawnFrameBehavior.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
		}

		// Token: 0x04000F9D RID: 3997
		private MissionMultiplayerGameModeBase _missionMultiplayerGameModeBase;
	}
}
