using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A5 RID: 421
	public abstract class MinorFactionsModel : MBGameModel<MinorFactionsModel>
	{
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001CC9 RID: 7369
		public abstract float DailyMinorFactionHeroSpawnChance { get; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001CCA RID: 7370
		public abstract int MinorFactionHeroLimit { get; }

		// Token: 0x06001CCB RID: 7371
		public abstract int GetMercenaryAwardFactorToJoinKingdom(Clan mercenaryClan, Kingdom kingdom, bool neededAmountForClanToJoinCalculation = false);
	}
}
