using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.MissionRepresentatives;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001C RID: 28
	public class DuelSpawningBehavior : SpawningBehaviorBase
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00006FA2 File Offset: 0x000051A2
		public override void Initialize(SpawnComponent spawnComponent)
		{
			base.Initialize(spawnComponent);
			base.OnPeerSpawnedFromVisuals += this.OnPeerSpawned;
			if (this.GameMode.WarmupComponent == null)
			{
				this.RequestStartSpawnSession();
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006FD0 File Offset: 0x000051D0
		public override void Clear()
		{
			base.Clear();
			base.OnPeerSpawnedFromVisuals -= this.OnPeerSpawned;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006FEA File Offset: 0x000051EA
		public override void OnTick(float dt)
		{
			if (this.IsSpawningEnabled && this.SpawnCheckTimer.Check(Mission.Current.CurrentTime))
			{
				this.SpawnAgents();
			}
			base.OnTick(dt);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007018 File Offset: 0x00005218
		protected override void SpawnAgents()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.IsSynchronized)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component.Representative is DuelMissionRepresentative && networkCommunicator.IsSynchronized && component.ControlledAgent == null && !component.HasSpawnedAgentVisuals && component.Team != null && component.Team != base.Mission.SpectatorTeam && component.TeamInitialPerkInfoReady && component.Culture != null && component.SpawnTimer.Check(Mission.Current.CurrentTime))
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component, false);
						if (mpheroClassForPeer == null)
						{
							if (component.SelectedTroopIndex != 0)
							{
								component.SelectedTroopIndex = 0;
								GameNetwork.BeginBroadcastModuleEvent();
								GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(networkCommunicator, 0));
								GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkCommunicator);
							}
						}
						else
						{
							BasicCharacterObject heroCharacter = mpheroClassForPeer.HeroCharacter;
							Equipment equipment = heroCharacter.Equipment.Clone(false);
							MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(component);
							IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
							if (enumerable != null)
							{
								foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
								{
									equipment[valueTuple.Item1] = valueTuple.Item2;
								}
							}
							AgentBuildData agentBuildData = new AgentBuildData(heroCharacter).MissionPeer(component).Equipment(equipment).Team(component.Team)
								.TroopOrigin(new BasicBattleAgentOrigin(heroCharacter))
								.IsFemale(component.Peer.IsFemale)
								.BodyProperties(base.GetBodyProperties(component, component.Culture))
								.VisualsIndex(0)
								.ClothingColor1(component.Culture.Color)
								.ClothingColor2(component.Culture.Color2);
							if (this.GameMode.ShouldSpawnVisualsForServer(networkCommunicator))
							{
								base.AgentVisualSpawnComponent.SpawnAgentVisualsForPeer(component, agentBuildData, component.SelectedTroopIndex, false, 0);
								if (agentBuildData.AgentVisualsIndex == 0)
								{
									component.HasSpawnedAgentVisuals = true;
									component.EquipmentUpdatingExpired = false;
								}
							}
							this.GameMode.HandleAgentVisualSpawning(networkCommunicator, agentBuildData, 0, true);
						}
					}
				}
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000728C File Offset: 0x0000548C
		public override bool AllowEarlyAgentVisualsDespawning(MissionPeer missionPeer)
		{
			return true;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000728F File Offset: 0x0000548F
		protected override bool IsRoundInProgress()
		{
			return Mission.Current.CurrentState == Mission.State.Continuing;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000729E File Offset: 0x0000549E
		private void OnPeerSpawned(MissionPeer peer)
		{
			MissionRepresentativeBase representative = peer.Representative;
		}
	}
}
