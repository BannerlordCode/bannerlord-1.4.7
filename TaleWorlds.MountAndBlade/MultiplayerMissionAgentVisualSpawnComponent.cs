using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BA RID: 698
	public class MultiplayerMissionAgentVisualSpawnComponent : MissionNetwork
	{
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060027F8 RID: 10232 RVA: 0x000971F4 File Offset: 0x000953F4
		// (remove) Token: 0x060027F9 RID: 10233 RVA: 0x0009722C File Offset: 0x0009542C
		public event Action OnMyAgentVisualSpawned;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x060027FA RID: 10234 RVA: 0x00097264 File Offset: 0x00095464
		// (remove) Token: 0x060027FB RID: 10235 RVA: 0x0009729C File Offset: 0x0009549C
		public event Action OnMyAgentSpawnedFromVisual;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x060027FC RID: 10236 RVA: 0x000972D4 File Offset: 0x000954D4
		// (remove) Token: 0x060027FD RID: 10237 RVA: 0x0009730C File Offset: 0x0009550C
		public event Action OnMyAgentVisualRemoved;

		// Token: 0x060027FE RID: 10238 RVA: 0x00097344 File Offset: 0x00095544
		public void SpawnAgentVisualsForPeer(MissionPeer missionPeer, AgentBuildData buildData, int selectedEquipmentSetIndex = -1, bool isBot = false, int totalTroopCount = 0)
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer != null)
			{
				myPeer.GetComponent<MissionPeer>();
			}
			if (buildData.AgentVisualsIndex == 0)
			{
				missionPeer.ClearAllVisuals(false);
			}
			missionPeer.ClearVisuals(buildData.AgentVisualsIndex);
			Equipment equipment = new Equipment(buildData.AgentOverridenSpawnEquipment);
			ItemObject item = equipment[10].Item;
			MatrixFrame spawnPointFrameForPlayer = this._spawnFrameSelectionHelper.GetSpawnPointFrameForPlayer(missionPeer.Peer, missionPeer.Team.Side, buildData.AgentVisualsIndex, totalTroopCount, item != null);
			ActionIndexCache actionIndexCache = ((item == null) ? ActionIndexCache.act_walk_idle_unarmed : ActionIndexCache.act_horse_stand_1);
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(buildData.AgentCharacter);
			MBReadOnlyList<MPPerkObject> selectedPerks = missionPeer.SelectedPerks;
			float num = 0.1f + MBRandom.RandomFloat * 0.8f;
			IAgentVisual agentVisual = null;
			if (item != null)
			{
				Monster monster = item.HorseComponent.Monster;
				AgentVisualsData agentVisualsData = new AgentVisualsData().Equipment(equipment).Scale(item.ScaleFactor).Frame(MatrixFrame.Identity)
					.ActionSet(MBGlobals.GetActionSet(monster.ActionSetCode))
					.Scene(Mission.Current.Scene)
					.Monster(monster)
					.PrepareImmediately(false)
					.MountCreationKey(MountCreationKey.GetRandomMountKeyString(item, MBRandom.RandomInt()));
				agentVisual = Mission.Current.AgentVisualCreator.Create(agentVisualsData, "Agent " + buildData.AgentCharacter.StringId + " mount", true, false);
				MatrixFrame matrixFrame = spawnPointFrameForPlayer;
				matrixFrame.rotation.ApplyScaleLocal(agentVisualsData.ScaleData);
				ActionIndexCache actionIndexCache2 = ActionIndexCache.act_none;
				foreach (MPPerkObject mpperkObject in selectedPerks)
				{
					if (!isBot && mpperkObject.HeroMountIdleAnimOverride != null)
					{
						actionIndexCache2 = ActionIndexCache.Create(mpperkObject.HeroMountIdleAnimOverride);
						break;
					}
					if (isBot && mpperkObject.TroopMountIdleAnimOverride != null)
					{
						actionIndexCache2 = ActionIndexCache.Create(mpperkObject.TroopMountIdleAnimOverride);
						break;
					}
				}
				if (actionIndexCache2 == ActionIndexCache.act_none)
				{
					if (item.StringId == "mp_aserai_camel")
					{
						Debug.Print("Client is spawning a camel for without mountCustomAction from the perk.", 0, Debug.DebugColor.White, 17179869184UL);
						actionIndexCache2 = ((!isBot) ? ActionIndexCache.act_hero_mount_idle_camel : ActionIndexCache.act_camel_idle_1);
					}
					else
					{
						if (!isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.HeroMountIdleAnim))
						{
							actionIndexCache2 = ActionIndexCache.Create(mpheroClassForCharacter.HeroMountIdleAnim);
						}
						if (isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.TroopMountIdleAnim))
						{
							actionIndexCache2 = ActionIndexCache.Create(mpheroClassForCharacter.TroopMountIdleAnim);
						}
					}
				}
				if (actionIndexCache2 != ActionIndexCache.act_none)
				{
					agentVisual.SetAction(in actionIndexCache2, 0f, true);
					agentVisual.GetVisuals().GetSkeleton().SetAnimationParameterAtChannel(0, num);
					agentVisual.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.1f, matrixFrame, true);
				}
				agentVisual.GetVisuals().GetEntity().SetFrame(ref matrixFrame, true);
			}
			ActionIndexCache actionIndexCache3 = actionIndexCache;
			if (agentVisual != null)
			{
				actionIndexCache3 = agentVisual.GetVisuals().GetSkeleton().GetActionAtChannel(0);
			}
			else
			{
				foreach (MPPerkObject mpperkObject2 in selectedPerks)
				{
					if (!isBot && mpperkObject2.HeroIdleAnimOverride != null)
					{
						actionIndexCache3 = ActionIndexCache.Create(mpperkObject2.HeroIdleAnimOverride);
						break;
					}
					if (isBot && mpperkObject2.TroopIdleAnimOverride != null)
					{
						actionIndexCache3 = ActionIndexCache.Create(mpperkObject2.TroopIdleAnimOverride);
						break;
					}
				}
				if (actionIndexCache3 == actionIndexCache)
				{
					if (!isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.HeroIdleAnim))
					{
						actionIndexCache3 = ActionIndexCache.Create(mpheroClassForCharacter.HeroIdleAnim);
					}
					if (isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.TroopIdleAnim))
					{
						actionIndexCache3 = ActionIndexCache.Create(mpheroClassForCharacter.TroopIdleAnim);
					}
				}
			}
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(buildData.AgentCharacter.Race);
			IAgentVisual agentVisual2 = Mission.Current.AgentVisualCreator.Create(new AgentVisualsData().Equipment(equipment).BodyProperties(buildData.AgentBodyProperties).Frame(spawnPointFrameForPlayer)
				.ActionSet(MBActionSet.GetActionSet(baseMonsterFromRace.ActionSetCode))
				.Scene(Mission.Current.Scene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(false)
				.UseMorphAnims(true)
				.SkeletonType(buildData.AgentIsFemale ? SkeletonType.Female : SkeletonType.Male)
				.ClothColor1(buildData.AgentClothingColor1)
				.ClothColor2(buildData.AgentClothingColor2)
				.AddColorRandomness(buildData.AgentVisualsIndex != 0)
				.ActionCode(in actionIndexCache3), "Mission::SpawnAgentVisuals", true, false);
			agentVisual2.SetAction(in actionIndexCache3, 0f, true);
			agentVisual2.GetVisuals().GetSkeleton().SetAnimationParameterAtChannel(0, num);
			agentVisual2.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.1f, spawnPointFrameForPlayer, true);
			agentVisual2.GetVisuals().SetFrame(ref spawnPointFrameForPlayer);
			agentVisual2.SetCharacterObjectID(buildData.AgentCharacter.StringId);
			EquipmentIndex equipmentIndex;
			EquipmentIndex equipmentIndex2;
			bool flag;
			equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
			if (flag)
			{
				equipmentIndex2 = EquipmentIndex.None;
			}
			agentVisual2.GetVisuals().SetWieldedWeaponIndices((int)equipmentIndex, (int)equipmentIndex2);
			PeerVisualsHolder peerVisualsHolder = new PeerVisualsHolder(missionPeer, buildData.AgentVisualsIndex, agentVisual2, agentVisual);
			missionPeer.OnVisualsSpawned(peerVisualsHolder, peerVisualsHolder.VisualsIndex);
			if (missionPeer.IsMine && buildData.AgentVisualsIndex == 0)
			{
				Action onMyAgentVisualSpawned = this.OnMyAgentVisualSpawned;
				if (onMyAgentVisualSpawned == null)
				{
					return;
				}
				onMyAgentVisualSpawned();
			}
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00097870 File Offset: 0x00095A70
		public void RemoveAgentVisuals(MissionPeer missionPeer, bool sync = false)
		{
			missionPeer.ClearAllVisuals(false);
			if (!GameNetwork.IsDedicatedServer && !missionPeer.Peer.IsMine)
			{
				this._spawnFrameSelectionHelper.FreeSpawnPointFromPlayer(missionPeer.Peer);
			}
			if (this.OnMyAgentVisualRemoved != null && missionPeer.IsMine)
			{
				this.OnMyAgentVisualRemoved();
			}
			Debug.Print("Removed visuals for " + missionPeer.Name + ".", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000978EA File Offset: 0x00095AEA
		public void OnMyAgentSpawned()
		{
			Action onMyAgentSpawnedFromVisual = this.OnMyAgentSpawnedFromVisual;
			if (onMyAgentSpawnedFromVisual == null)
			{
				return;
			}
			onMyAgentSpawnedFromVisual();
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000978FC File Offset: 0x00095AFC
		public override void OnPreMissionTick(float dt)
		{
			if (!GameNetwork.IsDedicatedServer && this._spawnFrameSelectionHelper == null && Mission.Current != null && GameNetwork.MyPeer != null)
			{
				this._spawnFrameSelectionHelper = new MultiplayerMissionAgentVisualSpawnComponent.VisualSpawnFrameSelectionHelper();
			}
		}

		// Token: 0x04000F49 RID: 3913
		private MultiplayerMissionAgentVisualSpawnComponent.VisualSpawnFrameSelectionHelper _spawnFrameSelectionHelper;

		// Token: 0x020005A1 RID: 1441
		private class VisualSpawnFrameSelectionHelper
		{
			// Token: 0x06003DCC RID: 15820 RVA: 0x000F4084 File Offset: 0x000F2284
			public VisualSpawnFrameSelectionHelper()
			{
				this._visualSpawnPoints = new GameEntity[6];
				this._visualAttackerSpawnPoints = new GameEntity[6];
				this._visualDefenderSpawnPoints = new GameEntity[6];
				this._visualSpawnPointUsers = new VirtualPlayer[6];
				for (int i = 0; i < 6; i++)
				{
					GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("sp_visual_" + i);
					if (gameEntity != null)
					{
						this._visualSpawnPoints[i] = gameEntity;
					}
					gameEntity = Mission.Current.Scene.FindEntityWithTag("sp_visual_attacker_" + i);
					if (gameEntity != null)
					{
						this._visualAttackerSpawnPoints[i] = gameEntity;
					}
					gameEntity = Mission.Current.Scene.FindEntityWithTag("sp_visual_defender_" + i);
					if (gameEntity != null)
					{
						this._visualDefenderSpawnPoints[i] = gameEntity;
					}
				}
				this._visualSpawnPointUsers[0] = GameNetwork.MyPeer.VirtualPlayer;
			}

			// Token: 0x06003DCD RID: 15821 RVA: 0x000F4184 File Offset: 0x000F2384
			public MatrixFrame GetSpawnPointFrameForPlayer(VirtualPlayer player, BattleSideEnum side, int agentVisualIndex, int totalTroopCount, bool isMounted = false)
			{
				if (agentVisualIndex == 0)
				{
					int num = -1;
					int num2 = -1;
					for (int i = 0; i < this._visualSpawnPointUsers.Length; i++)
					{
						if (this._visualSpawnPointUsers[i] == player)
						{
							num = i;
							break;
						}
						if (num2 < 0 && this._visualSpawnPointUsers[i] == null)
						{
							num2 = i;
						}
					}
					int num3 = ((num >= 0) ? num : num2);
					if (num3 >= 0)
					{
						this._visualSpawnPointUsers[num3] = player;
						GameEntity gameEntity = null;
						if (side == BattleSideEnum.Attacker)
						{
							gameEntity = this._visualAttackerSpawnPoints[num3];
						}
						else if (side == BattleSideEnum.Defender)
						{
							gameEntity = this._visualDefenderSpawnPoints[num3];
						}
						MatrixFrame matrixFrame = ((gameEntity != null) ? gameEntity.GetGlobalFrame() : this._visualSpawnPoints[num3].GetGlobalFrame());
						matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
						return matrixFrame;
					}
					Debug.FailedAssert("Couldn't find a valid spawn point for player.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerMissionAgentVisualSpawnComponent.cs", "GetSpawnPointFrameForPlayer", 139);
					return MatrixFrame.Identity;
				}
				else
				{
					Vec3 origin = this._visualSpawnPoints[3].GetGlobalFrame().origin;
					Vec3 origin2 = this._visualSpawnPoints[1].GetGlobalFrame().origin;
					Vec3 origin3 = this._visualSpawnPoints[5].GetGlobalFrame().origin;
					Mat3 rotation = this._visualSpawnPoints[0].GetGlobalFrame().rotation;
					rotation.MakeUnit();
					List<WorldFrame> formationFramesForBeforeFormationCreation = Formation.GetFormationFramesForBeforeFormationCreation(origin2.Distance(origin3), totalTroopCount, isMounted, new WorldPosition(Mission.Current.Scene, origin), rotation);
					if (formationFramesForBeforeFormationCreation.Count < agentVisualIndex)
					{
						return new MatrixFrame(in rotation, in origin);
					}
					return formationFramesForBeforeFormationCreation[agentVisualIndex - 1].ToGroundMatrixFrame();
				}
			}

			// Token: 0x06003DCE RID: 15822 RVA: 0x000F42F8 File Offset: 0x000F24F8
			public void FreeSpawnPointFromPlayer(VirtualPlayer player)
			{
				for (int i = 0; i < this._visualSpawnPointUsers.Length; i++)
				{
					if (this._visualSpawnPointUsers[i] == player)
					{
						this._visualSpawnPointUsers[i] = null;
						return;
					}
				}
			}

			// Token: 0x04001EB6 RID: 7862
			private const string SpawnPointTagPrefix = "sp_visual_";

			// Token: 0x04001EB7 RID: 7863
			private const string AttackerSpawnPointTagPrefix = "sp_visual_attacker_";

			// Token: 0x04001EB8 RID: 7864
			private const string DefenderSpawnPointTagPrefix = "sp_visual_defender_";

			// Token: 0x04001EB9 RID: 7865
			private const int NumberOfSpawnPoints = 6;

			// Token: 0x04001EBA RID: 7866
			private const int PlayerSpawnPointIndex = 0;

			// Token: 0x04001EBB RID: 7867
			private GameEntity[] _visualSpawnPoints;

			// Token: 0x04001EBC RID: 7868
			private GameEntity[] _visualAttackerSpawnPoints;

			// Token: 0x04001EBD RID: 7869
			private GameEntity[] _visualDefenderSpawnPoints;

			// Token: 0x04001EBE RID: 7870
			private VirtualPlayer[] _visualSpawnPointUsers;
		}
	}
}
