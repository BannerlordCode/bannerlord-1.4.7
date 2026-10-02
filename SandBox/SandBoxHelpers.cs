using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.Source.Objects;
using TaleWorlds.ObjectSystem;

namespace SandBox
{
	// Token: 0x02000024 RID: 36
	public static class SandBoxHelpers
	{
		// Token: 0x02000130 RID: 304
		public static class MissionHelper
		{
			// Token: 0x06000DCB RID: 3531 RVA: 0x00063AD4 File Offset: 0x00061CD4
			public static void FollowAgent(Agent agent, Agent target)
			{
				if (agent != null && target != null && agent.IsActive() && target.IsActive())
				{
					AgentBehaviorGroup activeBehaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetActiveBehaviorGroup();
					if (activeBehaviorGroup != null)
					{
						FollowAgentBehavior followAgentBehavior = activeBehaviorGroup.GetBehavior<FollowAgentBehavior>();
						if (followAgentBehavior == null)
						{
							followAgentBehavior = activeBehaviorGroup.AddBehavior<FollowAgentBehavior>();
						}
						activeBehaviorGroup.SetScriptedBehavior<FollowAgentBehavior>();
						followAgentBehavior.SetTargetAgent(target);
						return;
					}
				}
				else
				{
					Debug.FailedAssert("Cant follow agent", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\SandboxHelpers.cs", "FollowAgent", 45);
				}
			}

			// Token: 0x06000DCC RID: 3532 RVA: 0x00063B40 File Offset: 0x00061D40
			public static void UnfollowAgent(Agent agent)
			{
				if (agent != null && agent.IsActive())
				{
					AgentBehaviorGroup activeBehaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetActiveBehaviorGroup();
					if (activeBehaviorGroup != null && activeBehaviorGroup.GetBehavior<FollowAgentBehavior>() != null)
					{
						activeBehaviorGroup.RemoveBehavior<FollowAgentBehavior>();
						return;
					}
				}
				else
				{
					Debug.FailedAssert("Cant unfollow agent", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\SandboxHelpers.cs", "UnfollowAgent", 66);
				}
			}

			// Token: 0x06000DCD RID: 3533 RVA: 0x00063B94 File Offset: 0x00061D94
			public static void FadeOutAgents(IEnumerable<Agent> agents, bool hideInstantly, bool hideMount)
			{
				if (agents != null)
				{
					Agent[] array = agents.ToArray<Agent>();
					foreach (Agent agent in array)
					{
						if (!agent.IsMount)
						{
							agent.FadeOut(hideInstantly, hideMount);
						}
					}
					foreach (Agent agent2 in array)
					{
						if (agent2.State != AgentState.Routed)
						{
							agent2.FadeOut(hideInstantly, hideMount);
						}
					}
				}
			}

			// Token: 0x06000DCE RID: 3534 RVA: 0x00063BF8 File Offset: 0x00061DF8
			public static void DisableGenericMissionEventScript(string triggeringObjectTag, GenericMissionEvent missionEvent)
			{
				using (IEnumerator<ScriptComponentBehavior> enumerator = Mission.Current.Scene.FindEntityWithTag(triggeringObjectTag).GetScriptComponents().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GenericMissionEventScript genericMissionEventScript;
						if ((genericMissionEventScript = enumerator.Current as GenericMissionEventScript) != null && genericMissionEventScript.EventId.Equals(missionEvent.EventId) && genericMissionEventScript.Parameter.Equals(missionEvent.Parameter))
						{
							genericMissionEventScript.IsDisabled = true;
						}
					}
				}
			}

			// Token: 0x06000DCF RID: 3535 RVA: 0x00063C84 File Offset: 0x00061E84
			public static void SpawnPlayer(bool civilianEquipment = false, bool noHorses = false, bool noWeapon = false, bool wieldInitialWeapons = false, string spawnTag = "")
			{
				GameEntity gameEntity;
				if (!string.IsNullOrEmpty(spawnTag))
				{
					gameEntity = Mission.Current.Scene.FindEntityWithTag(spawnTag);
				}
				else
				{
					gameEntity = Mission.Current.Scene.FindEntityWithTag("spawnpoint_player");
				}
				SandBoxHelpers.MissionHelper.SpawnPlayer(gameEntity, civilianEquipment, noHorses, noWeapon, wieldInitialWeapons);
			}

			// Token: 0x06000DD0 RID: 3536 RVA: 0x00063CD0 File Offset: 0x00061ED0
			public static void SpawnPlayer(GameEntity spawnPosition, bool civilianEquipment = false, bool noHorses = false, bool noWeapon = false, bool wieldInitialWeapons = false)
			{
				if (Campaign.Current.GameMode != CampaignGameMode.Campaign)
				{
					civilianEquipment = false;
				}
				MatrixFrame matrixFrame = MatrixFrame.Identity;
				if (spawnPosition != null)
				{
					matrixFrame = spawnPosition.GetGlobalFrame();
					matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				}
				CampaignEventDispatcher.Instance.OnBeforePlayerAgentSpawn(ref matrixFrame);
				CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
				AgentBuildData agentBuildData = new AgentBuildData(playerCharacter).Team(Mission.Current.PlayerTeam).InitialPosition(in matrixFrame.origin);
				Vec2 vec = matrixFrame.rotation.f.AsVec2;
				vec = vec.Normalized();
				AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).CivilianEquipment(civilianEquipment).NoHorses(noHorses)
					.NoWeapons(noWeapon)
					.ClothingColor1(Mission.Current.PlayerTeam.Color)
					.ClothingColor2(Mission.Current.PlayerTeam.Color2)
					.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, playerCharacter, -1, default(UniqueTroopDescriptor), false, false))
					.MountKey(MountCreationKey.GetRandomMountKeyString(playerCharacter.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, playerCharacter.GetMountKeySeed()))
					.Controller(AgentControllerType.Player);
				Debug.Print(string.Format("Spawn position: {0}", matrixFrame.origin), 0, Debug.DebugColor.White, 17592186044416UL);
				Hero heroObject = playerCharacter.HeroObject;
				if (((heroObject != null) ? heroObject.ClanBanner : null) != null)
				{
					agentBuildData2.Banner(playerCharacter.HeroObject.ClanBanner);
				}
				if (Campaign.Current.GameMode != CampaignGameMode.Campaign)
				{
					agentBuildData2.TroopOrigin(new SimpleAgentOrigin(CharacterObject.PlayerCharacter, -1, null, default(UniqueTroopDescriptor)));
				}
				if (Campaign.Current.IsMainHeroDisguised)
				{
					MBEquipmentRoster @object = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("npc_disguised_hero_equipment_template");
					agentBuildData2.Equipment(@object.DefaultEquipment);
				}
				Agent agent = Mission.Current.SpawnAgent(agentBuildData2, false);
				if (wieldInitialWeapons)
				{
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				}
				CampaignEventDispatcher.Instance.OnPlayerAgentSpawned();
				if (spawnPosition != null)
				{
					foreach (string text in spawnPosition.Tags)
					{
						agent.AgentVisuals.GetEntity().AddTag(text);
					}
				}
				for (int j = 0; j < 3; j++)
				{
					Agent.Main.AgentVisuals.GetSkeleton().TickAnimations(0.1f, Agent.Main.AgentVisuals.GetGlobalFrame(), true);
				}
			}

			// Token: 0x06000DD1 RID: 3537 RVA: 0x00063F24 File Offset: 0x00062124
			public static List<Agent> SpawnHorses()
			{
				List<Agent> list = new List<Agent>();
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_horse"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					string text = gameEntity.Tags[1];
					ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(text);
					ItemRosterElement itemRosterElement = new ItemRosterElement(@object, 1, null);
					ItemRosterElement itemRosterElement2 = default(ItemRosterElement);
					if (@object.HasHorseComponent)
					{
						globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
						Mission mission = Mission.Current;
						ItemRosterElement itemRosterElement3 = itemRosterElement;
						ItemRosterElement itemRosterElement4 = itemRosterElement2;
						Vec2 asVec = globalFrame.rotation.f.AsVec2;
						Agent agent = mission.SpawnMonster(itemRosterElement3, itemRosterElement4, in globalFrame.origin, in asVec, -1);
						AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
						SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
						list.Add(agent);
					}
				}
				return list;
			}

			// Token: 0x06000DD2 RID: 3538 RVA: 0x00064010 File Offset: 0x00062210
			public static void SpawnSheeps()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_sheep"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("sheep"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000DD3 RID: 3539 RVA: 0x00064120 File Offset: 0x00062320
			public static void SpawnCows()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_cow"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("cow"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000DD4 RID: 3540 RVA: 0x00064230 File Offset: 0x00062430
			public static void SpawnGeese()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_goose"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("goose"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000DD5 RID: 3541 RVA: 0x00064340 File Offset: 0x00062540
			public static void SpawnChicken()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_chicken"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("chicken"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000DD6 RID: 3542 RVA: 0x00064450 File Offset: 0x00062650
			public static void SpawnHogs()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_hog"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("hog"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000DD7 RID: 3543 RVA: 0x00064560 File Offset: 0x00062760
			private static void SimulateAnimalAnimations(Agent agent)
			{
				int num = 10 + MBRandom.RandomInt(90);
				for (int i = 0; i < num; i++)
				{
					agent.TickActionChannels(0.1f);
					Vec3 vec = agent.ComputeAnimationDisplacement(0.1f);
					if (vec.LengthSquared > 0f)
					{
						agent.TeleportToPosition(agent.Position + vec);
					}
					agent.AgentVisuals.GetSkeleton().TickAnimations(0.1f, agent.AgentVisuals.GetGlobalFrame(), true);
				}
			}
		}

		// Token: 0x02000131 RID: 305
		public static class MapSceneHelper
		{
			// Token: 0x06000DD8 RID: 3544 RVA: 0x000645DC File Offset: 0x000627DC
			public static bool[] GetRegionMapping(PartyNavigationModel model)
			{
				TerrainType[] array = (TerrainType[])Enum.GetValues(typeof(TerrainType));
				bool[] array2 = new bool[array.Max<TerrainType>((TerrainType v) => (int)v) + 1];
				foreach (TerrainType terrainType in array)
				{
					array2[(int)terrainType] = model.IsTerrainTypeValidForNavigationType(terrainType, MobileParty.NavigationType.Default);
				}
				return array2;
			}
		}
	}
}
