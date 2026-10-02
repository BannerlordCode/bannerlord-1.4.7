using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CA RID: 458
	public abstract class SettlementLoyaltyModel : MBGameModel<SettlementLoyaltyModel>
	{
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06001E18 RID: 7704
		public abstract int SettlementLoyaltyChangeDueToSecurityThreshold { get; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001E19 RID: 7705
		public abstract int MaximumLoyaltyInSettlement { get; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001E1A RID: 7706
		public abstract int LoyaltyDriftMedium { get; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001E1B RID: 7707
		public abstract float HighLoyaltyProsperityEffect { get; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001E1C RID: 7708
		public abstract int LowLoyaltyProsperityEffect { get; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001E1D RID: 7709
		public abstract int MilitiaBoostPercentage { get; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001E1E RID: 7710
		public abstract float HighSecurityLoyaltyEffect { get; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001E1F RID: 7711
		public abstract float LowSecurityLoyaltyEffect { get; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001E20 RID: 7712
		public abstract float GovernorSameCultureLoyaltyEffect { get; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001E21 RID: 7713
		public abstract float GovernorDifferentCultureLoyaltyEffect { get; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001E22 RID: 7714
		public abstract float SettlementOwnerDifferentCultureLoyaltyEffect { get; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001E23 RID: 7715
		public abstract int ThresholdForTaxBoost { get; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001E24 RID: 7716
		public abstract int RebellionStartLoyaltyThreshold { get; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001E25 RID: 7717
		public abstract int ThresholdForTaxCorruption { get; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001E26 RID: 7718
		public abstract int ThresholdForHigherTaxCorruption { get; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001E27 RID: 7719
		public abstract int ThresholdForProsperityBoost { get; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001E28 RID: 7720
		public abstract int ThresholdForProsperityPenalty { get; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001E29 RID: 7721
		public abstract int AdditionalStarvationPenaltyStartDay { get; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001E2A RID: 7722
		public abstract int AdditionalStarvationLoyaltyEffect { get; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001E2B RID: 7723
		public abstract int RebelliousStateStartLoyaltyThreshold { get; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001E2C RID: 7724
		public abstract int LoyaltyBoostAfterRebellionStartValue { get; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001E2D RID: 7725
		public abstract float ThresholdForNotableRelationBonus { get; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001E2E RID: 7726
		public abstract int DailyNotableRelationBonus { get; }

		// Token: 0x06001E2F RID: 7727
		public abstract ExplainedNumber CalculateLoyaltyChange(Town town, bool includeDescriptions = false);

		// Token: 0x06001E30 RID: 7728
		public abstract void CalculateGoldGainDueToHighLoyalty(Town town, ref ExplainedNumber explainedNumber);

		// Token: 0x06001E31 RID: 7729
		public abstract void CalculateGoldCutDueToLowLoyalty(Town town, ref ExplainedNumber explainedNumber);
	}
}
