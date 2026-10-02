using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C9 RID: 457
	public abstract class SettlementValueModel : MBGameModel<SettlementValueModel>
	{
		// Token: 0x06001E13 RID: 7699
		public abstract Settlement FindMostSuitableHomeSettlement(Clan clan);

		// Token: 0x06001E14 RID: 7700
		public abstract float CalculateSettlementValueForFaction(Settlement settlement, IFaction faction);

		// Token: 0x06001E15 RID: 7701
		public abstract float CalculateSettlementBaseValue(Settlement settlement);

		// Token: 0x06001E16 RID: 7702
		public abstract float CalculateSettlementValueForEnemyHero(Settlement settlement, Hero hero);
	}
}
