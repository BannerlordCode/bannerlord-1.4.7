using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CD RID: 461
	public abstract class SettlementGarrisonModel : MBGameModel<SettlementGarrisonModel>
	{
		// Token: 0x06001E4C RID: 7756
		public abstract int GetMaximumDailyAutoRecruitmentCount(Town town);

		// Token: 0x06001E4D RID: 7757
		public abstract ExplainedNumber CalculateBaseGarrisonChange(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001E4E RID: 7758
		public abstract int FindNumberOfTroopsToTakeFromGarrison(MobileParty mobileParty, Settlement settlement, float idealGarrisonStrengthPerWalledCenter = 0f);

		// Token: 0x06001E4F RID: 7759
		public abstract int FindNumberOfTroopsToLeaveToGarrison(MobileParty mobileParty, Settlement settlement);

		// Token: 0x06001E50 RID: 7760
		public abstract float GetMaximumDailyRepairAmount(Settlement settlement);
	}
}
