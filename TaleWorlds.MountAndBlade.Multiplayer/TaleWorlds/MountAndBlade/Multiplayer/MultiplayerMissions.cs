using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Multiplayer.Missions;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000061 RID: 97
	[MissionManager]
	public static class MultiplayerMissions
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x0000D53D File Offset: 0x0000B73D
		[MissionMethod]
		public static void OpenTeamDeathmatchMission(string scene)
		{
			MissionState.OpenNew("MultiplayerTeamDeathmatch", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerTeamDeathmatch(),
						new MissionMultiplayerTeamDeathmatchClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new TeamDeathmatchSpawnFrameBehavior(), new TeamDeathmatchSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new TDMScoreboardData()),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MissionMultiplayerTeamDeathmatchClient(),
					new MultiplayerAchievementComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new TDMScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000D571 File Offset: 0x0000B771
		[MissionMethod]
		public static void OpenDuelMission(string scene)
		{
			MissionState.OpenNew("MultiplayerDuel", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerDuel(),
						new MissionMultiplayerGameModeDuelClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new DuelSpawnFrameBehavior(), new DuelSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new DuelScoreboardData()),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MissionMultiplayerGameModeDuelClient(),
					new MultiplayerAchievementComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new DuelScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000D5A8 File Offset: 0x0000B7A8
		[MissionMethod]
		public static void OpenSiegeMission(string scene)
		{
			MissionState.OpenNew("MultiplayerSiege", new MissionInitializerRecord(scene)
			{
				SceneUpgradeLevel = 3,
				SceneLevels = ""
			}, delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerSiege(),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerSiegeClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new SiegeSpawnFrameBehavior(), new SiegeSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new SiegeScoreboardData()),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerSiegeClient(),
					new MultiplayerAchievementComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new SiegeScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000D5FE File Offset: 0x0000B7FE
		[MissionMethod]
		public static void OpenBattleMission(string scene)
		{
			MissionState.OpenNew("MultiplayerBattle", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MultiplayerRoundController(),
						new MissionMultiplayerFlagDomination(MultiplayerGameType.Battle),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerGameModeFlagDominationClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new AgentVictoryLogic(),
						new AgentHumanAILogic(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new BattleScoreboardData()),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerRoundComponent(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerGameModeFlagDominationClient(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new BattleScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000D632 File Offset: 0x0000B832
		[MissionMethod]
		public static void OpenCaptainMission(string scene)
		{
			MissionState.OpenNew("MultiplayerCaptain", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerFlagDomination(MultiplayerGameType.Captain),
						new MultiplayerRoundController(),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerGameModeFlagDominationClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new AgentVictoryLogic(),
						new AgentHumanAILogic(),
						new MissionAgentPanicHandler(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new CaptainScoreboardData()),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerAchievementComponent(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerGameModeFlagDominationClient(),
					new MultiplayerRoundComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new CaptainScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000D666 File Offset: 0x0000B866
		[MissionMethod]
		public static void OpenSkirmishMission(string scene)
		{
			MissionState.OpenNew("MultiplayerSkirmish", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerFlagDomination(MultiplayerGameType.Skirmish),
						new MultiplayerRoundController(),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerGameModeFlagDominationClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new AgentVictoryLogic(),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new SkirmishScoreboardData()),
						new EquipmentControllerLeaveLogic(),
						new VoiceChatHandler(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerAchievementComponent(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerGameModeFlagDominationClient(),
					new MultiplayerRoundComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new SkirmishScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new VoiceChatHandler(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}
	}
}
