using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000135 RID: 309
	public class DefaultPartyMoraleModel : PartyMoraleModel
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x0007D173 File Offset: 0x0007B373
		public override float HighMoraleValue
		{
			get
			{
				return 70f;
			}
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0007D17A File Offset: 0x0007B37A
		public override int GetDailyStarvationMoralePenalty(PartyBase party)
		{
			return -5;
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0007D17E File Offset: 0x0007B37E
		public override int GetDailyNoWageMoralePenalty(MobileParty party)
		{
			return -3;
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0007D182 File Offset: 0x0007B382
		private int GetStarvationMoralePenalty(MobileParty party)
		{
			return -30;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0007D186 File Offset: 0x0007B386
		private int GetNoWageMoralePenalty(MobileParty party)
		{
			return -20;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0007D18A File Offset: 0x0007B38A
		public override float GetStandardBaseMorale(PartyBase party)
		{
			return 50f;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0007D191 File Offset: 0x0007B391
		public override float GetVictoryMoraleChange(PartyBase party)
		{
			return 20f;
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0007D198 File Offset: 0x0007B398
		public override float GetDefeatMoraleChange(PartyBase party)
		{
			return -20f;
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0007D1A0 File Offset: 0x0007B3A0
		private void CalculateFoodVarietyMoraleBonus(MobileParty party, ref ExplainedNumber result)
		{
			if (!party.Party.IsStarving)
			{
				float num;
				switch (party.ItemRoster.FoodVariety)
				{
				case 0:
				case 1:
					num = -2f;
					break;
				case 2:
					num = -1f;
					break;
				case 3:
					num = 0f;
					break;
				case 4:
					num = 1f;
					break;
				case 5:
					num = 2f;
					break;
				case 6:
					num = 3f;
					break;
				case 7:
					num = 5f;
					break;
				case 8:
					num = 6f;
					break;
				case 9:
					num = 7f;
					break;
				case 10:
					num = 8f;
					break;
				case 11:
					num = 9f;
					break;
				case 12:
					num = 10f;
					break;
				default:
					num = 10f;
					break;
				}
				if (num < 0f && party.LeaderHero != null && !party.IsCurrentlyAtSea && party.LeaderHero.GetPerkValue(DefaultPerks.Steward.WarriorsDiet))
				{
					num = 0f;
				}
				if (num != 0f)
				{
					result.Add(num, this._foodBonusMoraleText, null);
					if (num > 0f && party.HasPerk(DefaultPerks.Steward.Gourmet, false))
					{
						if (party.IsCurrentlyAtSea)
						{
							num *= 0.5f;
						}
						result.Add(num, DefaultPerks.Steward.Gourmet.Name, null);
					}
				}
			}
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0007D2EC File Offset: 0x0007B4EC
		private void GetPartySizeMoraleEffect(MobileParty mobileParty, ref ExplainedNumber result)
		{
			if (!mobileParty.IsMilitia && !mobileParty.IsVillager)
			{
				int num = mobileParty.Party.NumberOfAllMembers - mobileParty.Party.PartySizeLimit;
				if (num > 0)
				{
					result.Add(-1f * MathF.Sqrt((float)num), this._partySizeMoraleText, null);
				}
			}
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0007D340 File Offset: 0x0007B540
		private static void CheckPerkEffectOnPartyMorale(MobileParty party, PerkObject perk, bool isInfoNeeded, TextObject newInfo, int perkEffect, out TextObject outNewInfo, out int outPerkEffect)
		{
			outNewInfo = newInfo;
			outPerkEffect = perkEffect;
			if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(perk))
			{
				if (isInfoNeeded)
				{
					MBTextManager.SetTextVariable("EFFECT_NAME", perk.Name, false);
					MBTextManager.SetTextVariable("NUM", 10);
					MBTextManager.SetTextVariable("STR1", newInfo, false);
					MBTextManager.SetTextVariable("STR2", GameTexts.FindText("str_party_effect", null), false);
					outNewInfo = GameTexts.FindText("str_new_item_line", null);
				}
				outPerkEffect += 10;
			}
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0007D3C8 File Offset: 0x0007B5C8
		private void GetMoraleEffectsFromPerks(MobileParty party, ref ExplainedNumber bonus)
		{
			if (party.HasPerk(DefaultPerks.Crossbow.PeasantLeader, false))
			{
				float num = this.CalculateTroopTierRatio(party);
				bonus.AddFactor(DefaultPerks.Crossbow.PeasantLeader.PrimaryBonus * num, DefaultPerks.Crossbow.PeasantLeader.Name);
			}
			Settlement currentSettlement = party.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null && party.HasPerk(DefaultPerks.Charm.SelfPromoter, true))
			{
				bonus.Add(DefaultPerks.Charm.SelfPromoter.SecondaryBonus, DefaultPerks.Charm.SelfPromoter.Name, null);
			}
			if (!party.IsCurrentlyAtSea && party.HasPerk(DefaultPerks.Steward.Logistician, false))
			{
				int num2 = 0;
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsMounted)
					{
						num2 += elementCopyAtIndex.Number;
					}
				}
				if (party.Party.NumberOfMounts > party.MemberRoster.TotalManCount - num2)
				{
					bonus.Add(DefaultPerks.Steward.Logistician.PrimaryBonus, DefaultPerks.Steward.Logistician.Name, null);
				}
			}
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0007D4CC File Offset: 0x0007B6CC
		private float CalculateTroopTierRatio(MobileParty party)
		{
			int totalManCount = party.MemberRoster.TotalManCount;
			float num = 0f;
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.Tier <= 3)
				{
					num += (float)troopRosterElement.Number;
				}
			}
			return num / (float)totalManCount;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0007D54C File Offset: 0x0007B74C
		private void GetMoraleEffectsFromSkill(MobileParty party, ref ExplainedNumber bonus)
		{
			CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
			if (effectivePartyLeaderForSkill != null && effectivePartyLeaderForSkill.GetSkillValue(DefaultSkills.Leadership) > 0)
			{
				SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.LeadershipMoraleBonus, effectivePartyLeaderForSkill, ref bonus);
			}
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0007D584 File Offset: 0x0007B784
		public override ExplainedNumber GetEffectivePartyMorale(MobileParty mobileParty, bool includeDescription = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(50f, includeDescription, null);
			explainedNumber.Add(mobileParty.RecentEventsMorale, this._recentEventsText, null);
			this.GetMoraleEffectsFromSkill(mobileParty, ref explainedNumber);
			if (mobileParty.IsMilitia || mobileParty.IsGarrison)
			{
				if (mobileParty.IsMilitia)
				{
					if (mobileParty.HomeSettlement.IsStarving)
					{
						explainedNumber.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
					}
				}
				else if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
				{
					explainedNumber.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
				}
			}
			else if (mobileParty.Party.IsStarving)
			{
				explainedNumber.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
			}
			if (mobileParty.HasUnpaidWages > 0f)
			{
				explainedNumber.Add(mobileParty.HasUnpaidWages * (float)this.GetNoWageMoralePenalty(mobileParty), this._noWageMoraleText, null);
			}
			this.GetMoraleEffectsFromPerks(mobileParty, ref explainedNumber);
			this.CalculateFoodVarietyMoraleBonus(mobileParty, ref explainedNumber);
			this.GetPartySizeMoraleEffect(mobileParty, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x0400083D RID: 2109
		private const float BaseMoraleValue = 50f;

		// Token: 0x0400083E RID: 2110
		private readonly TextObject _recentEventsText = GameTexts.FindText("str_recent_events", null);

		// Token: 0x0400083F RID: 2111
		private readonly TextObject _starvationMoraleText = GameTexts.FindText("str_starvation_morale", null);

		// Token: 0x04000840 RID: 2112
		private readonly TextObject _noWageMoraleText = GameTexts.FindText("str_no_wage_morale", null);

		// Token: 0x04000841 RID: 2113
		private readonly TextObject _foodBonusMoraleText = GameTexts.FindText("str_food_bonus_morale", null);

		// Token: 0x04000842 RID: 2114
		private readonly TextObject _partySizeMoraleText = GameTexts.FindText("str_party_size_morale", null);
	}
}
