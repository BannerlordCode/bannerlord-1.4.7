using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D4 RID: 468
	public abstract class ClanFinanceModel : MBGameModel<ClanFinanceModel>
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001E7C RID: 7804
		public abstract int PartyGoldLowerThreshold { get; }

		// Token: 0x06001E7D RID: 7805
		public abstract ExplainedNumber CalculateClanGoldChange(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001E7E RID: 7806
		public abstract ExplainedNumber CalculateClanIncome(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001E7F RID: 7807
		public abstract ExplainedNumber CalculateClanExpenses(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001E80 RID: 7808
		public abstract ExplainedNumber CalculateTownIncomeFromTariffs(Clan clan, Town town, bool applyWithdrawals = false);

		// Token: 0x06001E81 RID: 7809
		public abstract int CalculateTownIncomeFromProjects(Town town);

		// Token: 0x06001E82 RID: 7810
		public abstract int CalculateNotableDailyGoldChange(Hero hero, bool applyWithdrawals);

		// Token: 0x06001E83 RID: 7811
		public abstract int CalculateVillageIncome(Clan clan, Village village, bool applyWithdrawals = false);

		// Token: 0x06001E84 RID: 7812
		public abstract int CalculateOwnerIncomeFromCaravan(MobileParty caravan);

		// Token: 0x06001E85 RID: 7813
		public abstract int CalculateOwnerIncomeFromWorkshop(Workshop workshop);

		// Token: 0x06001E86 RID: 7814
		public abstract float RevenueSmoothenFraction();
	}
}
