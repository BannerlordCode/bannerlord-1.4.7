using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FA RID: 250
	public class DefaultBuildingEffectModel : BuildingEffectModel
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x00068D9C File Offset: 0x00066F9C
		public override ExplainedNumber GetBuildingEffect(Building building, BuildingEffectEnum effect)
		{
			float baseBuildingEffectAmount = building.BuildingType.GetBaseBuildingEffectAmount(effect, building.CurrentLevel);
			ExplainedNumber explainedNumber = new ExplainedNumber(baseBuildingEffectAmount, false, null);
			if (effect == BuildingEffectEnum.DenarByBoundVillageHeartPerDay)
			{
				float num = 0f;
				foreach (Village village in building.Town.Villages)
				{
					num += village.Hearth;
				}
				explainedNumber = new ExplainedNumber(num * baseBuildingEffectAmount, false, null);
			}
			if (effect == BuildingEffectEnum.FoodStock && (building.BuildingType == DefaultBuildingTypes.CastleGranary || building.BuildingType == DefaultBuildingTypes.SettlementWarehouse))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.Battlements, building.Town, ref explainedNumber);
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.Contractors, building.Town, ref explainedNumber);
			if (building.BuildingType.IsDailyProject)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.MasterOfPlanning, building.Town, ref explainedNumber);
			}
			if (building.BuildingType == DefaultBuildingTypes.SettlementMarketplace || building.BuildingType == DefaultBuildingTypes.SettlementDailyFestivalAndGames)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Charm.PublicSpeaker, building.Town, ref explainedNumber);
			}
			return explainedNumber;
		}
	}
}
