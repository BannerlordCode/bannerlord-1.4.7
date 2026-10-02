using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A5 RID: 677
	public class MissionBattleSchedulerClientComponent : MissionLobbyComponent
	{
		// Token: 0x06002562 RID: 9570 RVA: 0x00087383 File Offset: 0x00085583
		public override void QuitMission()
		{
			base.QuitMission();
			if (base.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && NetworkMain.GameClient.LoggedIn && NetworkMain.GameClient.CurrentState == LobbyClient.State.AtBattle)
			{
				NetworkMain.GameClient.QuitFromMatchmakerGame();
			}
		}
	}
}
