using System;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x0200001C RID: 28
	public static class MultiplayerViewCreator
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002C6C File Offset: 0x00000E6C
		public static MissionView CreateMissionMultiplayerPreloadView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerPreloadView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C7D File Offset: 0x00000E7D
		public static MissionView CreateMissionScoreBoardUIHandler(Mission mission, bool isSingleTeam)
		{
			return ViewCreatorManager.CreateMissionView<MissionScoreboardUIHandler>(mission != null, mission, new object[] { isSingleTeam });
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C98 File Offset: 0x00000E98
		public static MissionView CreateMultiplayerEndOfRoundUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerEndOfRoundUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CA6 File Offset: 0x00000EA6
		public static MissionView CreateMultiplayerTeamSelectUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerTeamSelectUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public static MissionView CreateMultiplayerCultureSelectUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerCultureSelectUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002CC2 File Offset: 0x00000EC2
		public static MissionView CreateLobbyEquipmentUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionLobbyEquipmentUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public static MissionView CreatePollProgressUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerPollProgressUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CDE File Offset: 0x00000EDE
		public static MissionView CreateMissionMultiplayerEscapeMenu(string gameType)
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerEscapeMenu>(false, null, new object[] { gameType });
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002CF1 File Offset: 0x00000EF1
		public static MissionView CreateMissionMultiplayerPracticeEscapeMenu()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerPracticeEscapeMenu>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002CFF File Offset: 0x00000EFF
		public static MissionView CreateMissionKillNotificationUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerKillNotificationUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D0D File Offset: 0x00000F0D
		public static MissionView CreateMissionServerStatusUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerServerStatusUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D1B File Offset: 0x00000F1B
		public static MissionView CreateMultiplayerAdminPanelUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerAdminPanelUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D29 File Offset: 0x00000F29
		public static MissionView CreateMultiplayerFactionBanVoteUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerFactionBanVoteUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002D37 File Offset: 0x00000F37
		public static MissionView CreateMultiplayerMissionHUDExtensionUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerHUDExtensionUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D45 File Offset: 0x00000F45
		public static MissionView CreateMultiplayerMissionVoiceChatUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerVoiceChatUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002D53 File Offset: 0x00000F53
		public static MissionView CreateMultiplayerMissionOrderUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerMissionOrderUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002D64 File Offset: 0x00000F64
		public static MissionView CreateMultiplayerMissionDeathCardUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerDeathCardUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002D75 File Offset: 0x00000F75
		public static MissionView CreateMissionMultiplayerDuelUI()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerDuelUI>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002D83 File Offset: 0x00000F83
		public static MissionView CreateMultiplayerEndOfBattleUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MultiplayerEndOfBattleUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002D91 File Offset: 0x00000F91
		public static MissionView CreateMissionFlagMarkerUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionMultiplayerMarkerUIHandler>(false, null, Array.Empty<object>());
		}
	}
}
