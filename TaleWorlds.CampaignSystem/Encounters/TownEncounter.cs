using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x020002EF RID: 751
	public class TownEncounter : LocationEncounter
	{
		// Token: 0x060029EB RID: 10731 RVA: 0x000AF105 File Offset: 0x000AD305
		public TownEncounter(Settlement settlement)
			: base(settlement)
		{
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000AF110 File Offset: 0x000AD310
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			int num = base.Settlement.Town.GetWallLevel();
			string sceneName = nextLocation.GetSceneName(num);
			IMission mission;
			if (nextLocation.StringId == "center")
			{
				if (Campaign.Current.IsMainHeroDisguised)
				{
					string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(num);
					mission = CampaignMission.OpenDisguiseMission(sceneName, false, civilianUpgradeLevelTag, previousLocation);
				}
				else
				{
					mission = CampaignMission.OpenTownCenterMission(sceneName, nextLocation, talkToChar, num, playerSpecialSpawnTag);
				}
			}
			else if (nextLocation.StringId == "arena")
			{
				mission = CampaignMission.OpenArenaStartMission(sceneName, nextLocation, talkToChar);
			}
			else
			{
				num = Campaign.Current.Models.LocationModel.GetSettlementUpgradeLevel(PlayerEncounter.LocationEncounter);
				mission = CampaignMission.OpenIndoorMission(sceneName, num, nextLocation, talkToChar);
			}
			return mission;
		}
	}
}
