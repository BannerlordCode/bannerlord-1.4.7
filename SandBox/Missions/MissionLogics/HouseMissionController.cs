using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006C RID: 108
	public class HouseMissionController : MissionLogic
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x0001A8FE File Offset: 0x00018AFE
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001A917 File Offset: 0x00018B17
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = true;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001A92B File Offset: 0x00018B2B
		public override void EarlyStart()
		{
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001A930 File Offset: 0x00018B30
		public override void AfterStart()
		{
			base.AfterStart();
			base.Mission.SetMissionMode(MissionMode.StartUp, true);
			base.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
			base.Mission.IsQuestScreenAccessible = true;
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, true, true, false, "");
			this._missionAgentHandler.SpawnLocationCharacters(null);
		}

		// Token: 0x0400025E RID: 606
		private MissionAgentHandler _missionAgentHandler;
	}
}
