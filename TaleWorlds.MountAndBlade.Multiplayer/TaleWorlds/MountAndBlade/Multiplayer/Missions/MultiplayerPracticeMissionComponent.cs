using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.Missions
{
	// Token: 0x02000066 RID: 102
	public class MultiplayerPracticeMissionComponent : MissionLogic
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000E695 File Offset: 0x0000C895
		public override void AfterStart()
		{
			base.AfterStart();
			this._lobbyClient = NetworkMain.GameClient;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this._lastMessagePrintPassedTime += dt;
			if (this._shutDownMissionTriggered)
			{
				this._shutDownMissionTimer += dt;
				if (this._shutDownMissionTimer >= 1f)
				{
					this._shutDownMissionTimer -= 1f;
					this._shutDownMissionCount++;
					if (this._shutDownMissionCount >= 3)
					{
						base.Mission.EndMission();
						return;
					}
					this.InformMissionDuration();
					return;
				}
			}
			else if (this._lobbyClient.CurrentState == LobbyClient.State.SearchingBattle)
			{
				if (this._lastMessagePrintPassedTime > 5f)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=MrEhLbht}Still searching for a battle...", null).ToString()));
					this._lastMessagePrintPassedTime = 0f;
					return;
				}
			}
			else if (this._lobbyClient.CurrentState == LobbyClient.State.AtBattle && !this._shutDownMissionTriggered)
			{
				this._shutDownMissionTriggered = true;
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=BN1Pmhho}Found a battle by matchmaker!", null).ToString()));
				this.InformMissionDuration();
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		private void InformMissionDuration()
		{
			int num = 3 - this._shutDownMissionCount;
			TextObject textObject = new TextObject("{=aNMmlya4}Shutting down mission in {REMAINING_SECONDS_TO_SHUT_DOWN_MISSION} seconds!", null);
			textObject.SetTextVariable("REMAINING_SECONDS_TO_SHUT_DOWN_MISSION", num.ToString());
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
		}

		// Token: 0x040000F5 RID: 245
		private LobbyClient _lobbyClient;

		// Token: 0x040000F6 RID: 246
		private float _lastMessagePrintPassedTime;

		// Token: 0x040000F7 RID: 247
		private bool _shutDownMissionTriggered;

		// Token: 0x040000F8 RID: 248
		private float _shutDownMissionTimer;

		// Token: 0x040000F9 RID: 249
		private int _shutDownMissionCount;

		// Token: 0x040000FA RID: 250
		private const int ShutDownDurationInSeconds = 3;
	}
}
