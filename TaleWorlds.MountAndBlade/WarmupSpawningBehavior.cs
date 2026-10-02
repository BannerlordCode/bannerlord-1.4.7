using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D4 RID: 724
	public class WarmupSpawningBehavior : SpawningBehaviorBase
	{
		// Token: 0x060029C1 RID: 10689 RVA: 0x0009ED7C File Offset: 0x0009CF7C
		public WarmupSpawningBehavior()
		{
			this.IsSpawningEnabled = true;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x0009ED8B File Offset: 0x0009CF8B
		public override void OnTick(float dt)
		{
			if (this.IsSpawningEnabled && this.SpawnCheckTimer.Check(base.Mission.CurrentTime))
			{
				this.SpawnAgents();
			}
			base.OnTick(dt);
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x0009EDBC File Offset: 0x0009CFBC
		protected override void SpawnAgents()
		{
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.IsSynchronized)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.ControlledAgent == null && !component.HasSpawnedAgentVisuals && component.Team != null && component.Team != base.Mission.SpectatorTeam && component.TeamInitialPerkInfoReady && component.SpawnTimer.Check(base.Mission.CurrentTime))
					{
						IAgentVisual agentVisualForPeer = component.GetAgentVisualForPeer(0);
						BasicCultureObject basicCultureObject = ((component.Culture == @object) ? @object : object2);
						int num = component.SelectedTroopIndex;
						IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses(basicCultureObject);
						MultiplayerClassDivisions.MPHeroClass mpheroClass = ((num < 0) ? null : mpheroClasses.ElementAt<MultiplayerClassDivisions.MPHeroClass>(num));
						if (mpheroClass == null && num < 0)
						{
							mpheroClass = mpheroClasses.First<MultiplayerClassDivisions.MPHeroClass>();
							num = 0;
						}
						BasicCharacterObject heroCharacter = mpheroClass.HeroCharacter;
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
						MatrixFrame matrixFrame;
						if (agentVisualForPeer == null)
						{
							matrixFrame = this.SpawnComponent.GetSpawnFrame(component.Team, heroCharacter.Equipment.Horse.Item != null, false);
						}
						else
						{
							matrixFrame = agentVisualForPeer.GetFrame();
							matrixFrame.rotation.MakeUnit();
						}
						MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = multiplayerBattleColors.GetPeerColors(component);
						AgentBuildData agentBuildData = new AgentBuildData(heroCharacter).MissionPeer(component).Equipment(equipment).Team(component.Team)
							.TroopOrigin(new BasicBattleAgentOrigin(heroCharacter))
							.InitialPosition(in matrixFrame.origin);
						Vec2 vec = matrixFrame.rotation.f.AsVec2;
						vec = vec.Normalized();
						AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).IsFemale(component.Peer.IsFemale).BodyProperties(base.GetBodyProperties(component, basicCultureObject))
							.VisualsIndex(0)
							.ClothingColor1(peerColors.ClothingColor1Uint)
							.ClothingColor2(peerColors.ClothingColor2Uint);
						if (this.GameMode.ShouldSpawnVisualsForServer(networkCommunicator))
						{
							base.AgentVisualSpawnComponent.SpawnAgentVisualsForPeer(component, agentBuildData2, num, false, 0);
							if (agentBuildData2.AgentVisualsIndex == 0)
							{
								component.HasSpawnedAgentVisuals = true;
								component.EquipmentUpdatingExpired = false;
							}
						}
						this.GameMode.HandleAgentVisualSpawning(networkCommunicator, agentBuildData2, 0, true);
					}
				}
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x0009F0E4 File Offset: 0x0009D2E4
		public override bool AllowEarlyAgentVisualsDespawning(MissionPeer lobbyPeer)
		{
			return true;
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x0009F0E7 File Offset: 0x0009D2E7
		public override int GetMaximumReSpawnPeriodForPeer(MissionPeer peer)
		{
			return 3;
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x0009F0EA File Offset: 0x0009D2EA
		protected override bool IsRoundInProgress()
		{
			return Mission.Current.CurrentState == Mission.State.Continuing;
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x0009F0F9 File Offset: 0x0009D2F9
		public override void Clear()
		{
			base.Clear();
			base.RequestStopSpawnSession();
		}
	}
}
