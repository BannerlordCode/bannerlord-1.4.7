using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A7 RID: 423
	public abstract class AllianceModel : MBGameModel<AllianceModel>
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001CD1 RID: 7377
		public abstract CampaignTime MaxDurationOfAlliance { get; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001CD2 RID: 7378
		public abstract CampaignTime MaxDurationOfWarParticipation { get; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001CD3 RID: 7379
		public abstract int MaxNumberOfAlliances { get; }

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001CD4 RID: 7380
		public abstract CampaignTime DurationForOffers { get; }

		// Token: 0x06001CD5 RID: 7381
		public abstract int GetCallToWarCost(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x06001CD6 RID: 7382
		public abstract ExplainedNumber GetScoreOfStartingAlliance(Kingdom kingdomDeclaresAlliance, Kingdom kingdomDeclaredAlliance, out TextObject explanation, bool includeDescription = false);

		// Token: 0x06001CD7 RID: 7383
		public abstract float GetSupportScoreOfStartingAllianceForClan(Kingdom kingdomDeclaresAlliance, Kingdom kingdomDeclaredAlliance, Clan evaluatingClan, out TextObject explanation, bool includeDescription = false);

		// Token: 0x06001CD8 RID: 7384
		public abstract float GetScoreOfCallingToWar(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, IFaction evaluatingFaction, out TextObject reason);

		// Token: 0x06001CD9 RID: 7385
		public abstract float GetScoreOfJoiningWar(Kingdom offeringKingdom, Kingdom kingdomToOfferToJoinWarWith, Kingdom kingdomToOfferToJoinWarAgainst, IFaction evaluatingFaction, out TextObject reason);

		// Token: 0x06001CDA RID: 7386
		public abstract int GetInfluenceCostOfProposingStartingAlliance(Clan proposingClan);

		// Token: 0x06001CDB RID: 7387
		public abstract int GetInfluenceCostOfCallingToWar(Clan proposingClan);

		// Token: 0x06001CDC RID: 7388
		public abstract bool CanMakeAlliance(Kingdom kingdom, Kingdom targetKingdom, IFaction evaluatingFaction, out TextObject reason, bool includeReason = false);

		// Token: 0x06001CDD RID: 7389
		public abstract float GetAllianceFactorForDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar);

		// Token: 0x06001CDE RID: 7390
		public abstract float GetAllianceFactorForDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace);

		// Token: 0x06001CDF RID: 7391
		public abstract Clan GetProposerClanForAllianceDecision(Kingdom proposerKingdom, Kingdom proposedKingdom);
	}
}
