using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Missions.MissionLogics.Arena;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace SandBox.Tournaments
{
	// Token: 0x0200002B RID: 43
	[MissionManager]
	public static class TournamentMissionStarter
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00008278 File Offset: 0x00006478
		[MissionMethod]
		public static Mission OpenTournamentArcheryMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return MissionState.OpenNew("TournamentArchery", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false, DecalAtlasGroup.Town), delegate(Mission missionController)
			{
				TournamentArcheryMissionController tournamentArcheryMissionController = new TournamentArcheryMissionController(culture);
				return new MissionBehavior[]
				{
					new CampaignMissionComponent(),
					new EquipmentControllerLeaveLogic(),
					tournamentArcheryMissionController,
					new TournamentBehavior(tournamentGame, settlement, tournamentArcheryMissionController, isPlayerParticipating),
					new AgentVictoryLogic(),
					new MissionAgentPanicHandler(),
					new AgentHumanAILogic(),
					new ArenaAgentStateDeciderLogic(),
					new BasicLeaveMissionLogic(true),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionOptionsComponent()
				};
			}, true, true);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000082D0 File Offset: 0x000064D0
		[MissionMethod]
		public static Mission OpenTournamentFightMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return MissionState.OpenNew("TournamentFight", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false, DecalAtlasGroup.Town), delegate(Mission missionController)
			{
				TournamentFightMissionController tournamentFightMissionController = new TournamentFightMissionController(culture);
				return new MissionBehavior[]
				{
					new CampaignMissionComponent(),
					new EquipmentControllerLeaveLogic(),
					tournamentFightMissionController,
					new TournamentBehavior(tournamentGame, settlement, tournamentFightMissionController, isPlayerParticipating),
					new AgentVictoryLogic(),
					new MissionAgentPanicHandler(),
					new AgentHumanAILogic(),
					new ArenaAgentStateDeciderLogic(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionOptionsComponent(),
					new HighlightsController(),
					new SandboxHighlightsController()
				};
			}, true, true);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008328 File Offset: 0x00006528
		[MissionMethod]
		public static Mission OpenTournamentHorseRaceMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return MissionState.OpenNew("TournamentHorseRace", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false, DecalAtlasGroup.Town), delegate(Mission missionController)
			{
				TownHorseRaceMissionController townHorseRaceMissionController = new TownHorseRaceMissionController(culture);
				return new MissionBehavior[]
				{
					new CampaignMissionComponent(),
					new EquipmentControllerLeaveLogic(),
					townHorseRaceMissionController,
					new TournamentBehavior(tournamentGame, settlement, townHorseRaceMissionController, isPlayerParticipating),
					new AgentVictoryLogic(),
					new MissionAgentPanicHandler(),
					new AgentHumanAILogic(),
					new ArenaAgentStateDeciderLogic(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionOptionsComponent()
				};
			}, true, true);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008380 File Offset: 0x00006580
		[MissionMethod]
		public static Mission OpenTournamentJoustingMission(string scene, TournamentGame tournamentGame, Settlement settlement, CultureObject culture, bool isPlayerParticipating)
		{
			return MissionState.OpenNew("TournamentJousting", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false, DecalAtlasGroup.Town), delegate(Mission missionController)
			{
				TournamentJoustingMissionController tournamentJoustingMissionController = new TournamentJoustingMissionController(culture);
				return new MissionBehavior[]
				{
					new CampaignMissionComponent(),
					new EquipmentControllerLeaveLogic(),
					tournamentJoustingMissionController,
					new TournamentBehavior(tournamentGame, settlement, tournamentJoustingMissionController, isPlayerParticipating),
					new AgentVictoryLogic(),
					new MissionAgentPanicHandler(),
					new AgentHumanAILogic(),
					new ArenaAgentStateDeciderLogic(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MissionOptionsComponent()
				};
			}, true, true);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000083D5 File Offset: 0x000065D5
		[MissionMethod]
		public static Mission OpenBattleChallengeMission(string scene, IList<Hero> priorityCharsAttacker, IList<Hero> priorityCharsDefender)
		{
			return null;
		}
	}
}
