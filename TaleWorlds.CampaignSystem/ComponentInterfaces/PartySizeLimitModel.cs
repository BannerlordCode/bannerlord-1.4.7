using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BD RID: 445
	public abstract class PartySizeLimitModel : MBGameModel<PartySizeLimitModel>
	{
		// Token: 0x06001DCD RID: 7629
		public abstract ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false);

		// Token: 0x06001DCE RID: 7630
		public abstract ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false);

		// Token: 0x06001DCF RID: 7631
		public abstract ExplainedNumber CalculateGarrisonPartySizeLimit(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001DD0 RID: 7632
		public abstract int GetClanTierPartySizeEffectForHero(Hero hero);

		// Token: 0x06001DD1 RID: 7633
		public abstract int GetNextClanTierPartySizeEffectChangeForHero(Hero hero);

		// Token: 0x06001DD2 RID: 7634
		public abstract int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan);

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001DD3 RID: 7635
		public abstract int MinimumNumberOfVillagersAtVillagerParty { get; }

		// Token: 0x06001DD4 RID: 7636
		public abstract int GetIdealVillagerPartySize(Village village);

		// Token: 0x06001DD5 RID: 7637
		public abstract TroopRoster FindAppropriateInitialRosterForMobileParty(MobileParty party, PartyTemplateObject partyTemplate);

		// Token: 0x06001DD6 RID: 7638
		public abstract List<Ship> FindAppropriateInitialShipsForMobileParty(MobileParty party, PartyTemplateObject partyTemplate);
	}
}
