using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D3 RID: 467
	public abstract class ClanPoliticsModel : MBGameModel<ClanPoliticsModel>
	{
		// Token: 0x06001E76 RID: 7798
		public abstract ExplainedNumber CalculateInfluenceChange(Clan clan, bool includeDescriptions = false);

		// Token: 0x06001E77 RID: 7799
		public abstract float CalculateSupportForPolicyInClan(Clan clan, PolicyObject policy);

		// Token: 0x06001E78 RID: 7800
		public abstract float CalculateRelationshipChangeWithSponsor(Clan clan, Clan sponsorClan);

		// Token: 0x06001E79 RID: 7801
		public abstract int GetInfluenceRequiredToOverrideKingdomDecision(DecisionOutcome popularOption, DecisionOutcome overridingOption, KingdomDecision decision);

		// Token: 0x06001E7A RID: 7802
		public abstract bool CanHeroBeGovernor(Hero hero);
	}
}
