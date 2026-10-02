using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FC RID: 252
	public class DefaultBuildingScoreCalculationModel : BuildingScoreCalculationModel
	{
		// Token: 0x060016A4 RID: 5796 RVA: 0x00068FEE File Offset: 0x000671EE
		public override Building GetNextDailyBuilding(Town town)
		{
			return town.Buildings.GetRandomElementWithPredicate<Building>((Building b) => b.BuildingType.IsDailyProject);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0006901C File Offset: 0x0006721C
		public override Building GetNextBuilding(Town town)
		{
			return town.Buildings.WhereQ<Building>((Building x) => !x.BuildingType.IsDailyProject && x.CurrentLevel < 3 && !town.BuildingsInProgress.Contains(x)).GetRandomElementInefficiently<Building>();
		}
	}
}
