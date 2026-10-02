using System;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000198 RID: 408
	public abstract class BarterModel : MBGameModel<BarterModel>
	{
		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001C6F RID: 7279
		public abstract int BarterCooldownWithHeroInDays { get; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001C70 RID: 7280
		public abstract float MaximumPercentageOfNpcGoldToSpendAtBarter { get; }

		// Token: 0x06001C71 RID: 7281
		public abstract int CalculateOverpayRelationIncreaseCosts(Hero hero, float overpayAmount);

		// Token: 0x06001C72 RID: 7282
		public abstract ExplainedNumber GetBarterPenalty(IFaction faction, ItemBarterable itemBarterable, Hero otherHero, PartyBase otherParty);
	}
}
