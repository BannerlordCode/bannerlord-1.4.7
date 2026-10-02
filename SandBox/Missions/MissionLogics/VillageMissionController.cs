using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200008A RID: 138
	public class VillageMissionController : MissionLogic
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x00023C5B File Offset: 0x00021E5B
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00023C70 File Offset: 0x00021E70
		public override void AfterStart()
		{
			base.AfterStart();
			bool isNight = Campaign.Current.IsNight;
			base.Mission.IsInventoryAccessible = true;
			base.Mission.IsQuestScreenAccessible = true;
			MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, false, false, false, "");
			missionBehavior.SpawnLocationCharacters(null);
			SandBoxHelpers.MissionHelper.SpawnHorses();
			if (!isNight)
			{
				SandBoxHelpers.MissionHelper.SpawnSheeps();
				SandBoxHelpers.MissionHelper.SpawnCows();
				SandBoxHelpers.MissionHelper.SpawnHogs();
				SandBoxHelpers.MissionHelper.SpawnGeese();
				SandBoxHelpers.MissionHelper.SpawnChicken();
			}
		}
	}
}
