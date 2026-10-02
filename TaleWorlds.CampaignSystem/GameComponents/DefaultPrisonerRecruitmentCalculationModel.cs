using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000144 RID: 324
	public class DefaultPrisonerRecruitmentCalculationModel : PrisonerRecruitmentCalculationModel
	{
		// Token: 0x060019B3 RID: 6579 RVA: 0x000813C4 File Offset: 0x0007F5C4
		public override int GetConformityNeededToRecruitPrisoner(CharacterObject character)
		{
			return (character.Level + 6) * (character.Level + 6) - 10;
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000813DC File Offset: 0x0007F5DC
		public override ExplainedNumber GetConformityChangePerHour(PartyBase party, CharacterObject troopToBoost)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(10f, false, null);
			if (party.LeaderHero != null)
			{
				explainedNumber.Add((float)party.LeaderHero.GetSkillValue(DefaultSkills.Leadership) * 0.05f, null, null);
			}
			if (troopToBoost.Tier <= 3 && party.MobileParty != null && !party.MobileParty.IsCurrentlyAtSea)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.FerventAttacker, party.MobileParty, false, ref explainedNumber, false);
			}
			if (troopToBoost.Tier >= 4 && !party.MobileParty.IsCurrentlyAtSea && party.MobileParty.HasPerk(DefaultPerks.Leadership.StoutDefender, true))
			{
				explainedNumber.AddFactor(DefaultPerks.Leadership.StoutDefender.SecondaryBonus, null);
			}
			if (troopToBoost.Occupation != Occupation.Bandit && !party.MobileParty.IsCurrentlyAtSea && party.MobileParty.HasPerk(DefaultPerks.Leadership.LoyaltyAndHonor, true))
			{
				explainedNumber.AddFactor(DefaultPerks.Leadership.LoyaltyAndHonor.SecondaryBonus, null);
			}
			if (troopToBoost.IsInfantry)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.LeadByExample, party.MobileParty, true, ref explainedNumber, party.MobileParty.IsCurrentlyAtSea);
			}
			if (troopToBoost.IsRanged)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.TrustedCommander, party.MobileParty, true, ref explainedNumber, party.MobileParty.IsCurrentlyAtSea);
			}
			if (troopToBoost.Occupation == Occupation.Bandit && !party.MobileParty.IsCurrentlyAtSea && party.MobileParty.HasPerk(DefaultPerks.Roguery.Promises, true))
			{
				explainedNumber.AddFactor(DefaultPerks.Roguery.Promises.SecondaryBonus, null);
			}
			return explainedNumber;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00081554 File Offset: 0x0007F754
		public override int GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num)
		{
			CultureObject culture = character.Culture;
			Hero leaderHero = party.LeaderHero;
			if (culture == ((leaderHero != null) ? leaderHero.Culture : null))
			{
				MobileParty mobileParty = party.MobileParty;
				if (mobileParty != null && mobileParty.HasPerk(DefaultPerks.Leadership.Presence, true))
				{
					return 0;
				}
			}
			if (character.Occupation == Occupation.Bandit)
			{
				MobileParty mobileParty2 = party.MobileParty;
				if (mobileParty2 != null && mobileParty2.HasPerk(DefaultPerks.Roguery.TwoFaced, true))
				{
					return 0;
				}
			}
			int num2;
			if (character.Occupation == Occupation.Bandit)
			{
				num2 = -2;
			}
			else
			{
				num2 = -1;
			}
			return num2 * num;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000815D0 File Offset: 0x0007F7D0
		public override bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded)
		{
			if (!character.IsRegular || character.Tier > Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier || character.Tier < 2 || character.Culture.IsBandit)
			{
				conformityNeeded = 0;
				return false;
			}
			int elementXp = party.MobileParty.PrisonRoster.GetElementXp(character);
			conformityNeeded = this.GetConformityNeededToRecruitPrisoner(character);
			return elementXp >= conformityNeeded;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00081640 File Offset: 0x0007F840
		public override bool ShouldPartyRecruitPrisoners(PartyBase party)
		{
			return party.IsMobile && party.PartySizeLimit > party.MobileParty.MemberRoster.TotalManCount && !party.MobileParty.IsWageLimitExceeded() && !party.MobileParty.IsPatrolParty && (party.MobileParty.Morale > 30f || party.MobileParty.HasPerk(DefaultPerks.Leadership.Presence, true));
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x000816B0 File Offset: 0x0007F8B0
		public override int CalculateRecruitableNumber(PartyBase party, CharacterObject character)
		{
			if (character.IsHero || party.PrisonRoster.Count == 0 || party.PrisonRoster.TotalRegulars <= 0)
			{
				return 0;
			}
			int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(character);
			int elementXp = party.PrisonRoster.GetElementXp(character);
			int elementNumber = party.PrisonRoster.GetElementNumber(character);
			return MathF.Min(elementXp / conformityNeededToRecruitPrisoner, elementNumber);
		}

		// Token: 0x0400088B RID: 2187
		private const int AILordMinTierRequirementForRecruitPrisoners = 2;
	}
}
