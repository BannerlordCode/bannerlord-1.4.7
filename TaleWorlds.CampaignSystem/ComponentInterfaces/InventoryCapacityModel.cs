using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BE RID: 446
	public abstract class InventoryCapacityModel : MBGameModel<InventoryCapacityModel>
	{
		// Token: 0x06001DD8 RID: 7640
		public abstract ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false, int additionalManOnFoot = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false);

		// Token: 0x06001DD9 RID: 7641
		public abstract int GetItemAverageWeight();

		// Token: 0x06001DDA RID: 7642
		public abstract float GetItemEffectiveWeight(EquipmentElement equipmentElement, MobileParty mobileParty, bool isCurrentlyAtSea, out TextObject description);

		// Token: 0x06001DDB RID: 7643
		public abstract ExplainedNumber CalculateTotalWeightCarried(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false);
	}
}
