using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DB RID: 475
	public abstract class MarriageModel : MBGameModel<MarriageModel>
	{
		// Token: 0x06001E9D RID: 7837
		public abstract bool IsCoupleSuitableForMarriage(Hero firstHero, Hero secondHero);

		// Token: 0x06001E9E RID: 7838
		public abstract int GetEffectiveRelationIncrease(Hero firstHero, Hero secondHero);

		// Token: 0x06001E9F RID: 7839
		public abstract Clan GetClanAfterMarriage(Hero firstHero, Hero secondHero);

		// Token: 0x06001EA0 RID: 7840
		public abstract bool IsSuitableForMarriage(Hero hero);

		// Token: 0x06001EA1 RID: 7841
		public abstract bool IsClanSuitableForMarriage(Clan clan);

		// Token: 0x06001EA2 RID: 7842
		public abstract float NpcCoupleMarriageChance(Hero firstHero, Hero secondHero);

		// Token: 0x06001EA3 RID: 7843
		public abstract bool ShouldNpcMarriageBetweenClansBeAllowed(Clan consideringClan, Clan targetClan);

		// Token: 0x06001EA4 RID: 7844
		public abstract List<Hero> GetAdultChildrenSuitableForMarriage(Hero hero);

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001EA5 RID: 7845
		public abstract int MinimumMarriageAgeMale { get; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001EA6 RID: 7846
		public abstract int MinimumMarriageAgeFemale { get; }
	}
}
