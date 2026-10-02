using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CE RID: 462
	public abstract class SettlementTaxModel : MBGameModel<SettlementTaxModel>
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001E52 RID: 7762
		public abstract float SettlementCommissionRateTown { get; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001E53 RID: 7763
		public abstract float SettlementCommissionRateVillage { get; }

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001E54 RID: 7764
		public abstract int SettlementCommissionDecreaseSecurityThreshold { get; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001E55 RID: 7765
		public abstract int MaximumDecreaseBasedOnSecuritySecurity { get; }

		// Token: 0x06001E56 RID: 7766
		public abstract float GetTownTaxRatio(Town town);

		// Token: 0x06001E57 RID: 7767
		public abstract float GetVillageTaxRatio(Village village);

		// Token: 0x06001E58 RID: 7768
		public abstract float GetTownCommissionChangeBasedOnSecurity(Town town, float commission);

		// Token: 0x06001E59 RID: 7769
		public abstract ExplainedNumber CalculateTownTax(Town town, bool includeDescriptions = false);

		// Token: 0x06001E5A RID: 7770
		public abstract int CalculateVillageTaxFromIncome(Village village, int marketIncome);
	}
}
