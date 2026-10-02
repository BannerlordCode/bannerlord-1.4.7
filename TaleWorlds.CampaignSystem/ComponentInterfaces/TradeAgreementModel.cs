using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C1 RID: 449
	public abstract class TradeAgreementModel : MBGameModel<TradeAgreementModel>
	{
		// Token: 0x06001DE6 RID: 7654
		public abstract int GetProfitPerCaravanVisit(MobileParty mobileParty);

		// Token: 0x06001DE7 RID: 7655
		public abstract CampaignTime GetTradeAgreementDurationInYears(Kingdom iniatatingKingdom, Kingdom otherKingdom);

		// Token: 0x06001DE8 RID: 7656
		public abstract int GetMaximumTradeAgreementCount(Kingdom kingdom);

		// Token: 0x06001DE9 RID: 7657
		public abstract int GetInfluenceCostOfProposingTradeAgreement(Clan clan);

		// Token: 0x06001DEA RID: 7658
		public abstract float GetScoreOfStartingTradeAgreement(Kingdom kingdom, Kingdom targetKingdom, Clan clan, out TextObject explanation, bool includeExplanation = false);

		// Token: 0x06001DEB RID: 7659
		public abstract bool CanMakeTradeAgreement(Kingdom kingdom, Kingdom other, bool checkOtherSideTradeSupport, out TextObject reason, bool includeReason = false);
	}
}
