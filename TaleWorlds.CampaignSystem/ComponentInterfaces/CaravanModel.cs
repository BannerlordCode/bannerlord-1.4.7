using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000196 RID: 406
	public abstract class CaravanModel : MBGameModel<CaravanModel>
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001C5E RID: 7262
		public abstract int MaxNumberOfItemsToBuyFromSingleCategory { get; }

		// Token: 0x06001C5F RID: 7263
		public abstract int GetMaxGoldToSpendOnOneItemCategory(MobileParty caravan, ItemCategory itemCategory);

		// Token: 0x06001C60 RID: 7264
		public abstract int GetInitialTradeGold(Hero owner, bool isNavalCaravan, bool eliteCaravan);

		// Token: 0x06001C61 RID: 7265
		public abstract int GetCaravanFormingCost(bool eliteCaravan, bool navalCaravan);

		// Token: 0x06001C62 RID: 7266
		public abstract int GetPowerChangeAfterCaravanCreation(Hero hero, MobileParty caravanParty);

		// Token: 0x06001C63 RID: 7267
		public abstract bool CanHeroCreateCaravan(Hero hero);

		// Token: 0x06001C64 RID: 7268
		public abstract float GetEliteCaravanSpawnChance(Hero hero);
	}
}
