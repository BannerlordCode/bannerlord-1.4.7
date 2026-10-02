using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EF RID: 239
	public class DefaultAlleyModel : AlleyModel
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00063E7E File Offset: 0x0006207E
		private CharacterObject _thug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_1");
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x00063E8F File Offset: 0x0006208F
		private CharacterObject _expertThug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_2");
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x00063EA0 File Offset: 0x000620A0
		private CharacterObject _masterThug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_3");
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x00063EB1 File Offset: 0x000620B1
		public override CampaignTime DestroyAlleyAfterDaysWhenLeaderIsDeath
		{
			get
			{
				return CampaignTime.Days(4f);
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x00063EBD File Offset: 0x000620BD
		public override int MinimumTroopCountInPlayerOwnedAlley
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x00063EC0 File Offset: 0x000620C0
		public override int MaximumTroopCountInPlayerOwnedAlley
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001603 RID: 5635 RVA: 0x00063EC4 File Offset: 0x000620C4
		public override float GetDailyCrimeRatingOfAlley
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00063ECB File Offset: 0x000620CB
		public override float GetDailyXpGainForAssignedClanMember(Hero assignedHero)
		{
			return 200f;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00063ED2 File Offset: 0x000620D2
		public override float GetDailyXpGainForMainHero()
		{
			return 40f;
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00063ED9 File Offset: 0x000620D9
		public override float GetInitialXpGainForMainHero()
		{
			return 1500f;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00063EE0 File Offset: 0x000620E0
		public override float GetXpGainAfterSuccessfulAlleyDefenseForMainHero()
		{
			return 6000f;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00063EE7 File Offset: 0x000620E7
		public override TroopRoster GetTroopsOfAIOwnedAlley(Alley alley)
		{
			return this.GetTroopsOfAlleyInternal(alley);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00063EF0 File Offset: 0x000620F0
		public override TroopRoster GetTroopsOfAlleyForBattleMission(Alley alley)
		{
			TroopRoster troopsOfAlleyInternal = this.GetTroopsOfAlleyInternal(alley);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopRosterElement troopRosterElement in troopsOfAlleyInternal.GetTroopRoster())
			{
				troopRoster.AddToCounts(troopRosterElement.Character, troopRosterElement.Number * 2, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00063F64 File Offset: 0x00062164
		private TroopRoster GetTroopsOfAlleyInternal(Alley alley)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			Hero owner = alley.Owner;
			if (owner.Power <= 100f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 1, false, 0, 0, true, -1);
				}
			}
			else if (owner.Power <= 200f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
			}
			else if (owner.Power <= 300f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 3, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 3, false, 0, 0, true, -1);
				}
			}
			else if ((float)owner.RandomValue > 0.5f)
			{
				troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._expertThug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._masterThug, 3, false, 0, 0, true, -1);
			}
			else
			{
				troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._expertThug, 4, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._masterThug, 4, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00064194 File Offset: 0x00062394
		public override List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(Alley alley)
		{
			List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> list = new List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>>();
			foreach (Hero hero in Clan.PlayerClan.AliveLords)
			{
				if (hero != Hero.MainHero)
				{
					list.Add(new ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>(hero, this.GetAvailability(alley, hero)));
				}
			}
			foreach (Hero hero2 in Clan.PlayerClan.Companions)
			{
				if (hero2 != Hero.MainHero && !hero2.IsDead)
				{
					list.Add(new ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>(hero2, this.GetAvailability(alley, hero2)));
				}
			}
			return list;
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0006426C File Offset: 0x0006246C
		public override TroopRoster GetTroopsToRecruitFromAlleyDependingOnAlleyRandom(Alley alley, float random)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			if (random >= 0.5f)
			{
				return troopRoster;
			}
			Clan relatedBanditClanDependingOnAlleySettlementFaction = this.GetRelatedBanditClanDependingOnAlleySettlementFaction(alley);
			if (random > 0.3f)
			{
				troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 1, false, 0, 0, true, -1);
			}
			else if (random > 0.15f)
			{
				troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 1, false, 0, 0, true, -1);
			}
			else if (random > 0.05f)
			{
				troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 1, false, 0, 0, true, -1);
			}
			else
			{
				troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 3, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0006439C File Offset: 0x0006259C
		public override TextObject GetDisabledReasonTextForHero(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail)
		{
			switch (detail)
			{
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available:
				return TextObject.GetEmpty();
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay:
			{
				TextObject textObject = new TextObject("{=dgUF5awO}It will take {HOURS} {?HOURS > 1}hours{?}hour{\\?} for this clan member to arrive.", null);
				textObject.SetTextVariable("HOURS", (int)Math.Ceiling((double)Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).ResultNumber));
				return textObject;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughRoguerySkill:
			{
				TextObject textObject2 = GameTexts.FindText("str_character_role_disabled_tooltip", null);
				textObject2.SetTextVariable("SKILL_NAME", DefaultSkills.Roguery.Name.ToString());
				textObject2.SetTextVariable("MIN_SKILL_AMOUNT", 30);
				return textObject2;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughMercyTrait:
			{
				TextObject textObject3 = GameTexts.FindText("str_hero_needs_trait_tooltip", null);
				textObject3.SetTextVariable("TRAIT_NAME", DefaultTraits.Mercy.Name.ToString());
				textObject3.SetTextVariable("MAX_TRAIT_AMOUNT", 0);
				return textObject3;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.CanNotLeadParty:
				return new TextObject("{=qClVr2ka}This hero cannot lead a party.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlreadyAlleyLeader:
				return GameTexts.FindText("str_hero_is_already_alley_leader", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Prisoner:
				return new TextObject("{=qhRC8XWU}This hero is currently prisoner.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.SolvingIssue:
				return new TextObject("{=nT6EQGf9}This hero is currently solving an issue.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Traveling:
				return new TextObject("{=WECWpVSw}This hero is currently traveling.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy:
				return new TextObject("{=c9iu5lcc}This hero is currently busy.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Fugutive:
				return new TextObject("{=eZYtkDff}This hero is currently fugutive.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Governor:
				return new TextObject("{=8NI4wrqU}This hero is currently assigned as a governor.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlleyUnderAttack:
				return new TextObject("{=pdqi2qz1}You can not do this action while your alley is under attack.", null);
			default:
				return TextObject.GetEmpty();
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00064510 File Offset: 0x00062710
		public override float GetAlleyAttackResponseTimeInDays(TroopRoster troopRoster)
		{
			float num = 0f;
			foreach (TroopRosterElement troopRosterElement in troopRoster.GetTroopRoster())
			{
				num += (((float)troopRosterElement.Character.Tier > 4f) ? 4f : ((float)troopRosterElement.Character.Tier)) * (float)troopRosterElement.Number;
			}
			return (float)Math.Min(12, 8 + (int)(num / 8f));
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x000645A8 File Offset: 0x000627A8
		private Clan GetRelatedBanditClanDependingOnAlleySettlementFaction(Alley alley)
		{
			string stringId = alley.Settlement.Culture.StringId;
			Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
			if (stringId == "khuzait")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "steppe_bandits");
			}
			else if (stringId == "vlandia" || stringId.Contains("empire"))
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
			}
			else if (stringId == "aserai")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "desert_bandits");
			}
			else if (stringId == "battania")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "forest_bandits");
			}
			else if (stringId == "sturgia" || stringId == "nord")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "sea_raiders");
			}
			return clan;
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0006472C File Offset: 0x0006292C
		private DefaultAlleyModel.AlleyMemberAvailabilityDetail GetAvailability(Alley alley, Hero hero)
		{
			IAlleyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>();
			if (alley.Owner == Hero.MainHero && campaignBehavior != null && campaignBehavior.GetIsPlayerAlleyUnderAttack(alley))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlleyUnderAttack;
			}
			if (hero.GetSkillValue(DefaultSkills.Roguery) < 30)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughRoguerySkill;
			}
			if (hero.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughMercyTrait;
			}
			if (campaignBehavior != null && campaignBehavior.GetAllAssignedClanMembersForOwnedAlleys().Contains(hero))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlreadyAlleyLeader;
			}
			if (hero.GovernorOf != null)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Governor;
			}
			if (!hero.CanLeadParty())
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.CanNotLeadParty;
			}
			if (Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.SolvingIssue;
			}
			if (hero.IsFugitive)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Fugutive;
			}
			if (hero.IsTraveling)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Traveling;
			}
			if (hero.IsPrisoner)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Prisoner;
			}
			if (!hero.IsActive)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy;
			}
			if (hero.IsPartyLeader)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy;
			}
			if (Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).BaseNumber > 0f)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay;
			}
			return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available;
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00064829 File Offset: 0x00062A29
		public override int GetDailyIncomeOfAlley(Alley alley)
		{
			return (int)(alley.Settlement.Town.Prosperity / 50f);
		}

		// Token: 0x0400074D RID: 1869
		private const int BaseResponseTimeInDays = 8;

		// Token: 0x0400074E RID: 1870
		private const int MaxResponseTimeInDays = 12;

		// Token: 0x0400074F RID: 1871
		public const int MinimumRoguerySkillNeededForLeadingAnAlley = 30;

		// Token: 0x04000750 RID: 1872
		public const int MaximumMercyTraitNeededForLeadingAnAlley = 0;

		// Token: 0x0200056C RID: 1388
		public enum AlleyMemberAvailabilityDetail
		{
			// Token: 0x04001744 RID: 5956
			Available,
			// Token: 0x04001745 RID: 5957
			AvailableWithDelay,
			// Token: 0x04001746 RID: 5958
			NotEnoughRoguerySkill,
			// Token: 0x04001747 RID: 5959
			NotEnoughMercyTrait,
			// Token: 0x04001748 RID: 5960
			CanNotLeadParty,
			// Token: 0x04001749 RID: 5961
			AlreadyAlleyLeader,
			// Token: 0x0400174A RID: 5962
			Prisoner,
			// Token: 0x0400174B RID: 5963
			SolvingIssue,
			// Token: 0x0400174C RID: 5964
			Traveling,
			// Token: 0x0400174D RID: 5965
			Busy,
			// Token: 0x0400174E RID: 5966
			Fugutive,
			// Token: 0x0400174F RID: 5967
			Governor,
			// Token: 0x04001750 RID: 5968
			AlleyUnderAttack
		}
	}
}
