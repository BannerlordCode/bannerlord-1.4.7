using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x020002EE RID: 750
	public class RetirementEncounter : LocationEncounter
	{
		// Token: 0x060029E9 RID: 10729 RVA: 0x000AF0A6 File Offset: 0x000AD2A6
		public RetirementEncounter(Settlement settlement)
			: base(settlement)
		{
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000AF0B0 File Offset: 0x000AD2B0
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			IMission mission = null;
			if (Settlement.CurrentSettlement.SettlementComponent is RetirementSettlementComponent)
			{
				int num = (Settlement.CurrentSettlement.IsTown ? Settlement.CurrentSettlement.Town.GetWallLevel() : 1);
				mission = CampaignMission.OpenRetirementMission(nextLocation.GetSceneName(num), nextLocation, null, null, "retirement_after_player_knockedout");
			}
			return mission;
		}

		// Token: 0x04000C2E RID: 3118
		private const string UnconsciousGameMenuID = "retirement_after_player_knockedout";
	}
}
