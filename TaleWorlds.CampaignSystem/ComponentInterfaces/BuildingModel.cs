using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FD RID: 509
	public abstract class BuildingModel : MBGameModel<BuildingModel>
	{
		// Token: 0x06001F9D RID: 8093
		public abstract bool CanAddBuildingTypeToTown(BuildingType buildingType, Town town);
	}
}
