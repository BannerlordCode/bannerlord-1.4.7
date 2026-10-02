using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C2 RID: 450
	public abstract class TradeItemPriceFactorModel : MBGameModel<TradeItemPriceFactorModel>
	{
		// Token: 0x06001DED RID: 7661
		public abstract float GetTradePenalty(ItemObject item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStore, float supply, float demand);

		// Token: 0x06001DEE RID: 7662
		public abstract float GetBasePriceFactor(ItemCategory itemCategory, float inStoreValue, float supply, float demand, bool isSelling, int transferValue);

		// Token: 0x06001DEF RID: 7663
		public abstract int GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand);

		// Token: 0x06001DF0 RID: 7664
		public abstract int GetTheoreticalMaxItemMarketValue(ItemObject item);
	}
}
