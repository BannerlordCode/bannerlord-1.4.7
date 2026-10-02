using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C0 RID: 448
	public abstract class PartyWageModel : MBGameModel<PartyWageModel>
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001DE1 RID: 7649
		public abstract int MaxWagePaymentLimit { get; }

		// Token: 0x06001DE2 RID: 7650
		public abstract int GetCharacterWage(CharacterObject character);

		// Token: 0x06001DE3 RID: 7651
		public abstract ExplainedNumber GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions = false);

		// Token: 0x06001DE4 RID: 7652
		public abstract ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false);
	}
}
