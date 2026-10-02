using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AF RID: 431
	public abstract class VolunteerModel : MBGameModel<VolunteerModel>
	{
		// Token: 0x06001D45 RID: 7493
		public abstract int MaximumIndexHeroCanRecruitFromHero(Hero buyerHero, Hero sellerHero, int useValueAsRelation = -101);

		// Token: 0x06001D46 RID: 7494
		public abstract int MaximumIndexGarrisonCanRecruitFromHero(Settlement settlement, Hero sellerHero);

		// Token: 0x06001D47 RID: 7495
		public abstract float GetDailyVolunteerProductionProbability(Hero hero, int index, Settlement settlement);

		// Token: 0x06001D48 RID: 7496
		public abstract CharacterObject GetBasicVolunteer(Hero hero);

		// Token: 0x06001D49 RID: 7497
		public abstract bool CanHaveRecruits(Hero hero);

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001D4A RID: 7498
		public abstract int MaxVolunteerTier { get; }
	}
}
