using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A8 RID: 424
	public abstract class DiplomacyModel : MBGameModel<DiplomacyModel>
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001CE1 RID: 7393
		public abstract int MaxRelationLimit { get; }

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001CE2 RID: 7394
		public abstract int MinRelationLimit { get; }

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001CE3 RID: 7395
		public abstract int MaxNeutralRelationLimit { get; }

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001CE4 RID: 7396
		public abstract int MinNeutralRelationLimit { get; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001CE5 RID: 7397
		public abstract int MinimumRelationWithConversationCharacterToJoinKingdom { get; }

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001CE6 RID: 7398
		public abstract int GiftingTownRelationshipBonus { get; }

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001CE7 RID: 7399
		public abstract int GiftingCastleRelationshipBonus { get; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001CE8 RID: 7400
		public abstract float WarDeclarationScorePenaltyAgainstTradePartners { get; }

		// Token: 0x06001CE9 RID: 7401
		public abstract float GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(Kingdom kingdomToJoin);

		// Token: 0x06001CEA RID: 7402
		public abstract float GetRelationIncreaseFactor(Hero hero1, Hero hero2, float relationValue);

		// Token: 0x06001CEB RID: 7403
		public abstract int GetInfluenceAwardForSettlementCapturer(Settlement settlement);

		// Token: 0x06001CEC RID: 7404
		public abstract float GetHourlyInfluenceAwardForRaidingEnemyVillage(MobileParty mobileParty);

		// Token: 0x06001CED RID: 7405
		public abstract float GetHourlyInfluenceAwardForBesiegingEnemyFortification(MobileParty mobileParty);

		// Token: 0x06001CEE RID: 7406
		public abstract float GetHourlyInfluenceAwardForBeingArmyMember(MobileParty mobileParty);

		// Token: 0x06001CEF RID: 7407
		public abstract float GetScoreOfClanToJoinKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001CF0 RID: 7408
		public abstract float GetScoreOfClanToLeaveKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001CF1 RID: 7409
		public abstract float GetScoreOfKingdomToGetClan(Kingdom kingdom, Clan clan);

		// Token: 0x06001CF2 RID: 7410
		public abstract float GetScoreOfKingdomToSackClan(Kingdom kingdom, Clan clan);

		// Token: 0x06001CF3 RID: 7411
		public abstract float GetScoreOfMercenaryToJoinKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001CF4 RID: 7412
		public abstract float GetScoreOfMercenaryToLeaveKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001CF5 RID: 7413
		public abstract float GetScoreOfKingdomToHireMercenary(Kingdom kingdom, Clan mercenaryClan);

		// Token: 0x06001CF6 RID: 7414
		public abstract float GetScoreOfKingdomToSackMercenary(Kingdom kingdom, Clan mercenaryClan);

		// Token: 0x06001CF7 RID: 7415
		public abstract float GetScoreOfDeclaringPeaceForClan(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, Clan evaluatingClan, out TextObject reason, bool includeReason = false);

		// Token: 0x06001CF8 RID: 7416
		public abstract float GetScoreOfDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace);

		// Token: 0x06001CF9 RID: 7417
		public abstract bool IsPeaceSuitable(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace);

		// Token: 0x06001CFA RID: 7418
		public abstract float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, Clan evaluatingClan, out TextObject reason, bool includeReason = false);

		// Token: 0x06001CFB RID: 7419
		public abstract ExplainedNumber GetWarProgressScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar, bool includeDescriptions = false);

		// Token: 0x06001CFC RID: 7420
		public abstract float GetScoreOfLettingPartyGo(MobileParty party, MobileParty partyToLetGo);

		// Token: 0x06001CFD RID: 7421
		public abstract float GetValueOfHeroForFaction(Hero examinedHero, IFaction targetFaction, bool forMarriage = false);

		// Token: 0x06001CFE RID: 7422
		public abstract int GetRelationCostOfExpellingClanFromKingdom();

		// Token: 0x06001CFF RID: 7423
		public abstract int GetInfluenceCostOfSupportingClan();

		// Token: 0x06001D00 RID: 7424
		public abstract int GetInfluenceCostOfExpellingClan(Clan proposingClan);

		// Token: 0x06001D01 RID: 7425
		public abstract int GetInfluenceCostOfProposingPeace(Clan proposingClan);

		// Token: 0x06001D02 RID: 7426
		public abstract int GetInfluenceCostOfProposingWar(Clan proposingClan);

		// Token: 0x06001D03 RID: 7427
		public abstract int GetInfluenceValueOfSupportingClan();

		// Token: 0x06001D04 RID: 7428
		public abstract int GetRelationValueOfSupportingClan();

		// Token: 0x06001D05 RID: 7429
		public abstract int GetInfluenceCostOfAnnexation(Clan proposingClan);

		// Token: 0x06001D06 RID: 7430
		public abstract int GetInfluenceCostOfChangingLeaderOfArmy();

		// Token: 0x06001D07 RID: 7431
		public abstract int GetInfluenceCostOfDisbandingArmy();

		// Token: 0x06001D08 RID: 7432
		public abstract int GetRelationCostOfDisbandingArmy(bool isLeaderParty);

		// Token: 0x06001D09 RID: 7433
		public abstract int GetInfluenceCostOfPolicyProposalAndDisavowal(Clan proposingClan);

		// Token: 0x06001D0A RID: 7434
		public abstract int GetInfluenceCostOfAbandoningArmy();

		// Token: 0x06001D0B RID: 7435
		public abstract int GetEffectiveRelation(Hero hero, Hero hero1);

		// Token: 0x06001D0C RID: 7436
		public abstract int GetBaseRelation(Hero hero, Hero hero1);

		// Token: 0x06001D0D RID: 7437
		public abstract void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2);

		// Token: 0x06001D0E RID: 7438
		public abstract int GetRelationChangeAfterClanLeaderIsDead(Hero deadLeader, Hero relationHero);

		// Token: 0x06001D0F RID: 7439
		public abstract int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner);

		// Token: 0x06001D10 RID: 7440
		public abstract float GetClanStrength(Clan clan);

		// Token: 0x06001D11 RID: 7441
		public abstract float GetHeroCommandingStrengthForClan(Hero hero);

		// Token: 0x06001D12 RID: 7442
		public abstract float GetHeroGoverningStrengthForClan(Hero hero);

		// Token: 0x06001D13 RID: 7443
		public abstract uint GetNotificationColor(ChatNotificationType notificationType);

		// Token: 0x06001D14 RID: 7444
		public abstract int GetDailyTributeToPay(Clan factionToPay, Clan factionToReceive, out int tributeDurationInDays);

		// Token: 0x06001D15 RID: 7445
		public abstract float GetDecisionMakingThreshold(IFaction consideringFaction);

		// Token: 0x06001D16 RID: 7446
		public abstract float GetValueOfSettlementsForFaction(IFaction faction);

		// Token: 0x06001D17 RID: 7447
		public abstract bool CanSettlementBeGifted(Settlement settlement);

		// Token: 0x06001D18 RID: 7448
		public abstract bool IsClanEligibleToBecomeRuler(Clan clan);

		// Token: 0x06001D19 RID: 7449
		public abstract IEnumerable<BarterGroup> GetBarterGroups();

		// Token: 0x06001D1A RID: 7450
		public abstract int GetCharmExperienceFromRelationGain(Hero hero, float relationChange, ChangeRelationAction.ChangeRelationDetail detail);

		// Token: 0x06001D1B RID: 7451
		public abstract float DenarsToInfluence();

		// Token: 0x06001D1C RID: 7452
		public abstract DiplomacyModel.DiplomacyStance? GetShallowDiplomaticStance(IFaction faction1, IFaction faction2);

		// Token: 0x06001D1D RID: 7453
		public abstract DiplomacyModel.DiplomacyStance GetDefaultDiplomaticStance(IFaction faction1, IFaction faction2);

		// Token: 0x06001D1E RID: 7454
		public abstract bool IsAtConstantWar(IFaction faction1, IFaction faction2);

		// Token: 0x02000600 RID: 1536
		public enum DiplomacyStance
		{
			// Token: 0x04001903 RID: 6403
			Neutral,
			// Token: 0x04001904 RID: 6404
			War
		}
	}
}
