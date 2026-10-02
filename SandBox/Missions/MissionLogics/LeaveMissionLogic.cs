using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000070 RID: 112
	public class LeaveMissionLogic : MissionLogic
	{
		// Token: 0x0600047E RID: 1150 RVA: 0x0001B0C9 File Offset: 0x000192C9
		public LeaveMissionLogic(string leaveMenuId = "settlement_player_unconscious")
		{
			this._menuId = leaveMenuId;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001B0D8 File Offset: 0x000192D8
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return base.Mission.MainAgent != null && !base.Mission.MainAgent.IsActive();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001B0FC File Offset: 0x000192FC
		public override void OnMissionTick(float dt)
		{
			if (Agent.Main == null || !Agent.Main.IsActive())
			{
				if (this._isAgentDeadTimer == null)
				{
					this._isAgentDeadTimer = new Timer(Mission.Current.CurrentTime, 5f, true);
				}
				if (this._isAgentDeadTimer.Check(Mission.Current.CurrentTime))
				{
					Mission.Current.NextCheckTimeEndMission = 0f;
					Mission.Current.EndMission();
					Campaign.Current.GameMenuManager.SetNextMenu(this._menuId);
					return;
				}
			}
			else if (this._isAgentDeadTimer != null)
			{
				this._isAgentDeadTimer = null;
			}
		}

		// Token: 0x04000268 RID: 616
		private string _menuId;

		// Token: 0x04000269 RID: 617
		private Timer _isAgentDeadTimer;
	}
}
