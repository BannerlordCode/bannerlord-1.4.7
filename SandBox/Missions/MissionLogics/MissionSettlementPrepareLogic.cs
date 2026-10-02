using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200007C RID: 124
	public class MissionSettlementPrepareLogic : MissionLogic
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x00022732 File Offset: 0x00020932
		public override void AfterStart()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsTown || Settlement.CurrentSettlement.IsCastle))
			{
				this.OpenGates();
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00022768 File Offset: 0x00020968
		private void OpenGates()
		{
			foreach (CastleGate castleGate in Mission.Current.ActiveMissionObjects.FindAllWithType<CastleGate>().ToList<CastleGate>())
			{
				castleGate.OpenDoorAndDisableGateForCivilianMission();
			}
		}
	}
}
