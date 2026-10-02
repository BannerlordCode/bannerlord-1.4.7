using System;
using System.Collections.Generic;
using SandBox.View.Missions.Sound.Components;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;

namespace SandBox.View.Missions.Tournaments
{
	// Token: 0x0200002B RID: 43
	[ViewCreatorModule]
	public class TournamentMissionViews
	{
		// Token: 0x06000124 RID: 292 RVA: 0x0000D068 File Offset: 0x0000B268
		[ViewMethod("TournamentArchery")]
		public static MissionView[] OpenTournamentArcheryMission(Mission mission)
		{
			return new List<MissionView>
			{
				new MissionCampaignView(),
				new MissionConversationCameraView(),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				ViewCreator.CreateMissionSpectatorControlView(mission),
				SandBoxViewCreator.CreateMissionTournamentView(),
				new MissionAudienceHandler(0.4f + MBRandom.RandomFloat * 0.6f),
				new MissionSingleplayerViewHandler(),
				new MissionMessageUIHandler(),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				new MissionItemContourControllerView(),
				ViewCreator.CreatePhotoModeView(),
				new ArenaPreloadView()
			}.ToArray();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000D170 File Offset: 0x0000B370
		[ViewMethod("TournamentFight")]
		public static MissionView[] OpenTournamentFightMission(Mission mission)
		{
			return new List<MissionView>
			{
				new MissionCampaignView(),
				new MissionConversationCameraView(),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				SandBoxViewCreator.CreateMissionTournamentView(),
				new MissionAudienceHandler(0.4f + MBRandom.RandomFloat * 0.6f),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				ViewCreator.CreateMissionSpectatorControlView(mission),
				new MusicTournamentMissionView(),
				new MissionSingleplayerViewHandler(),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				new MissionItemContourControllerView(),
				new MissionCampaignBattleSpectatorView(),
				ViewCreator.CreatePhotoModeView(),
				new ArenaPreloadView()
			}.ToArray();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000D280 File Offset: 0x0000B480
		[ViewMethod("TournamentHorseRace")]
		public static MissionView[] OpenTournamentHorseRaceMission(Mission mission)
		{
			return new List<MissionView>
			{
				new MissionCampaignView(),
				new MissionConversationCameraView(),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				ViewCreator.CreateMissionSpectatorControlView(mission),
				new MissionTournamentView(),
				new MissionSingleplayerViewHandler(),
				new MissionAudienceHandler(0.4f + MBRandom.RandomFloat * 0.6f),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				new MissionItemContourControllerView(),
				new MissionCampaignBattleSpectatorView(),
				ViewCreator.CreatePhotoModeView(),
				new ArenaPreloadView()
			}.ToArray();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000D388 File Offset: 0x0000B588
		[ViewMethod("TournamentJousting")]
		public static MissionView[] OpenTournamentJoustingMission(Mission mission)
		{
			return new List<MissionView>
			{
				new MissionCampaignView(),
				new MissionConversationCameraView(),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				ViewCreator.CreateMissionSpectatorControlView(mission),
				SandBoxViewCreator.CreateMissionTournamentView(),
				new MissionAudienceHandler(0.4f + MBRandom.RandomFloat * 0.6f),
				new MissionSingleplayerViewHandler(),
				new MissionMessageUIHandler(),
				new MissionScoreUIHandler(),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler(),
				new MissionTournamentJoustingView(),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				new MissionItemContourControllerView(),
				new MissionCampaignBattleSpectatorView(),
				ViewCreator.CreatePhotoModeView(),
				new ArenaPreloadView()
			}.ToArray();
		}
	}
}
