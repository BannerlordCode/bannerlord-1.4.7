using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E9 RID: 489
	public abstract class BuildingScoreCalculationModel : MBGameModel<BuildingScoreCalculationModel>
	{
		// Token: 0x06001F09 RID: 7945
		public abstract Building GetNextBuilding(Town town);

		// Token: 0x06001F0A RID: 7946
		public abstract Building GetNextDailyBuilding(Town town);
	}
}
