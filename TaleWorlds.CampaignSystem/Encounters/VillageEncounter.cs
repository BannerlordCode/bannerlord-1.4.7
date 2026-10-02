using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x020002F0 RID: 752
	public class VillageEncounter : LocationEncounter
	{
		// Token: 0x060029ED RID: 10733 RVA: 0x000AF1C6 File Offset: 0x000AD3C6
		public VillageEncounter(Settlement settlement)
			: base(settlement)
		{
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000AF1D0 File Offset: 0x000AD3D0
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			IMission mission = null;
			if (nextLocation.StringId == "village_center")
			{
				mission = CampaignMission.OpenVillageMission(nextLocation.GetSceneName(1), nextLocation, talkToChar);
			}
			return mission;
		}
	}
}
