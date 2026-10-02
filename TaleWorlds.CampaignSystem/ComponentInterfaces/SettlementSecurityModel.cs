using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CB RID: 459
	public abstract class SettlementSecurityModel : MBGameModel<SettlementSecurityModel>
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001E33 RID: 7731
		public abstract int MaximumSecurityInSettlement { get; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001E34 RID: 7732
		public abstract int SecurityDriftMedium { get; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001E35 RID: 7733
		public abstract float MapEventSecurityEffectRadius { get; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001E36 RID: 7734
		public abstract float HideoutClearedSecurityEffectRadius { get; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001E37 RID: 7735
		public abstract int HideoutClearedSecurityGain { get; }

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001E38 RID: 7736
		public abstract int ThresholdForTaxCorruption { get; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001E39 RID: 7737
		public abstract int ThresholdForHigherTaxCorruption { get; }

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001E3A RID: 7738
		public abstract int ThresholdForTaxBoost { get; }

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001E3B RID: 7739
		public abstract int SettlementTaxBoostPercentage { get; }

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001E3C RID: 7740
		public abstract int SettlementTaxPenaltyPercentage { get; }

		// Token: 0x06001E3D RID: 7741
		public abstract float GetLootedNearbyPartySecurityEffect(Town town, float sumOfAttackedPartyStrengths);

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001E3E RID: 7742
		public abstract int ThresholdForNotableRelationBonus { get; }

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001E3F RID: 7743
		public abstract int ThresholdForNotableRelationPenalty { get; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001E40 RID: 7744
		public abstract int DailyNotableRelationBonus { get; }

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001E41 RID: 7745
		public abstract int DailyNotableRelationPenalty { get; }

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001E42 RID: 7746
		public abstract int DailyNotablePowerBonus { get; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001E43 RID: 7747
		public abstract int DailyNotablePowerPenalty { get; }

		// Token: 0x06001E44 RID: 7748
		public abstract ExplainedNumber CalculateSecurityChange(Town town, bool includeDescriptions = false);

		// Token: 0x06001E45 RID: 7749
		public abstract float GetNearbyBanditPartyDefeatedSecurityEffect(Town town, float sumOfAttackedPartyStrengths);

		// Token: 0x06001E46 RID: 7750
		public abstract void CalculateGoldGainDueToHighSecurity(Town town, ref ExplainedNumber explainedNumber);

		// Token: 0x06001E47 RID: 7751
		public abstract void CalculateGoldCutDueToLowSecurity(Town town, ref ExplainedNumber explainedNumber);
	}
}
