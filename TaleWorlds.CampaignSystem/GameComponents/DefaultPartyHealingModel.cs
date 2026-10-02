using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000133 RID: 307
	public class DefaultPartyHealingModel : PartyHealingModel
	{
		// Token: 0x0600191E RID: 6430 RVA: 0x0007C48C File Offset: 0x0007A68C
		public override float GetSurgeryChance(PartyBase party)
		{
			MobileParty mobileParty = party.MobileParty;
			int? num;
			if (mobileParty == null)
			{
				num = null;
			}
			else
			{
				Hero effectiveSurgeon = mobileParty.EffectiveSurgeon;
				num = ((effectiveSurgeon != null) ? new int?(effectiveSurgeon.GetSkillValue(DefaultSkills.Medicine)) : null);
			}
			int num2 = num ?? 0;
			return 0.0015f * (float)num2;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0007C4F0 File Offset: 0x0007A6F0
		public override float GetSiegeBombardmentHitSurgeryChance(PartyBase party)
		{
			float num = 0f;
			if (party != null && party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Medicine.SiegeMedic, false))
			{
				num += DefaultPerks.Medicine.SiegeMedic.PrimaryBonus;
			}
			return num;
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0007C530 File Offset: 0x0007A730
		public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty = null)
		{
			if ((damageType == DamageTypes.Blunt && !canDamageKillEvenIfBlunt) || (character.IsHero && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.VeryEasy) || (character.IsPlayerCharacter && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.Easy))
			{
				return 1f;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			float num;
			if (((party != null) ? party.MobileParty : null) != null)
			{
				MobileParty mobileParty = party.MobileParty;
				this.AddSurgeonSurvivalBonus(mobileParty, ref explainedNumber);
				if (((enemyParty != null) ? enemyParty.MobileParty : null) != null && enemyParty.MobileParty.HasPerk(DefaultPerks.Medicine.DoctorsOath, false))
				{
					DefaultPartyHealingModel.AddDoctorsOathSkillBonusForParty(enemyParty.MobileParty, ref explainedNumber);
					SkillLevelingManager.OnSurgeryApplied(enemyParty.MobileParty, false, character.Tier);
				}
				explainedNumber.Add((float)character.Level * 0.02f, null, null);
				if (!character.IsHero && party.MapEvent != null && character.Tier < 3)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PhysicianOfPeople, party.MobileParty, false, ref explainedNumber, party.MobileParty.IsCurrentlyAtSea);
				}
				if (character.IsHero)
				{
					explainedNumber.Add(character.GetTotalArmorSum(Equipment.EquipmentType.Battle) * 0.01f, null, null);
					explainedNumber.Add(character.Age * -0.01f, null, null);
					explainedNumber.AddFactor(50f, null);
				}
				ExplainedNumber explainedNumber2 = new ExplainedNumber(1f / explainedNumber.ResultNumber, false, null);
				if (character.IsHero)
				{
					if (party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Medicine.CheatDeath, true))
					{
						explainedNumber2.AddFactor(DefaultPerks.Medicine.CheatDeath.SecondaryBonus, DefaultPerks.Medicine.CheatDeath.Name);
					}
					if (character.HeroObject.Clan == Clan.PlayerClan)
					{
						float clanMemberDeathChanceMultiplier = Campaign.Current.Models.DifficultyModel.GetClanMemberDeathChanceMultiplier();
						if (!clanMemberDeathChanceMultiplier.ApproximatelyEqualsTo(0f, 1E-05f))
						{
							explainedNumber2.AddFactor(clanMemberDeathChanceMultiplier, GameTexts.FindText("str_game_difficulty", null));
						}
					}
				}
				num = 1f - MBMath.ClampFloat(explainedNumber2.ResultNumber, 0f, 1f);
			}
			else if (character.IsHero && character.HeroObject.IsPrisoner)
			{
				num = 1f - character.Age * 0.0035f;
			}
			else if (explainedNumber.ResultNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				num = 0f;
			}
			else
			{
				num = 1f - 1f / explainedNumber.ResultNumber;
			}
			return num;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0007C78F File Offset: 0x0007A98F
		public override int GetSkillXpFromHealingTroop(PartyBase party)
		{
			return 5;
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0007C794 File Offset: 0x0007A994
		public override ExplainedNumber GetDailyHealingForRegulars(PartyBase party, bool isPrisoners, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (isPrisoners)
			{
				explainedNumber.Add(1f, null, null);
			}
			else if (party != null && party.IsMobile)
			{
				MobileParty mobileParty = party.MobileParty;
				if (party.IsStarving || (mobileParty.IsGarrison && mobileParty.CurrentSettlement.IsStarving))
				{
					if (mobileParty.IsGarrison)
					{
						if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
						{
							int num = MBRandom.RoundRandomized((float)party.MemberRoster.TotalRegulars * 0.1f);
							explainedNumber.Add((float)(-(float)num), DefaultPartyHealingModel._starvingText, null);
						}
					}
					else
					{
						int totalRegulars = party.MemberRoster.TotalRegulars;
						explainedNumber.Add((float)(-(float)totalRegulars) * 0.25f, DefaultPartyHealingModel._starvingText, null);
					}
				}
				else
				{
					explainedNumber.Add(5f, null, null);
					if (mobileParty.IsGarrison)
					{
						if (mobileParty.CurrentSettlement.IsTown)
						{
							SkillHelper.AddSkillBonusForTown(DefaultSkillEffects.GovernorHealingRateBonus, mobileParty.CurrentSettlement.Town, ref explainedNumber);
						}
					}
					else
					{
						SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.HealingRateBonusForRegulars, mobileParty, ref explainedNumber);
					}
					if (!mobileParty.IsGarrison && !mobileParty.IsMilitia)
					{
						if (!mobileParty.IsMoving)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.TriageTent, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
						}
						else if (!mobileParty.IsCurrentlyAtSea)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.WalkItOff, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WalkItOff, mobileParty, true, ref explainedNumber, false);
						}
					}
					if (mobileParty.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BestMedicine, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
					}
					if (mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsHideout)
					{
						if (mobileParty.CurrentSettlement.IsFortification)
						{
							explainedNumber.Add(10f, DefaultPartyHealingModel._settlementText, null);
						}
						if (party.SiegeEvent == null && !mobileParty.CurrentSettlement.IsUnderSiege && !mobileParty.CurrentSettlement.IsRaided && !mobileParty.CurrentSettlement.IsUnderRaid)
						{
							if (mobileParty.CurrentSettlement.IsTown)
							{
								PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PristineStreets, mobileParty, false, ref explainedNumber, false);
							}
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.AGoodDaysRest, mobileParty, true, ref explainedNumber, false);
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.GoodLogdings, mobileParty, true, ref explainedNumber, false);
						}
					}
					else if (!mobileParty.IsMoving && mobileParty.LastVisitedSettlement != null && mobileParty.LastVisitedSettlement.IsVillage && mobileParty.LastVisitedSettlement.Position.DistanceSquared(party.Position) < 2f && !mobileParty.LastVisitedSettlement.IsUnderRaid && !mobileParty.LastVisitedSettlement.IsRaided)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BushDoctor, mobileParty, false, ref explainedNumber, false);
					}
					if (mobileParty.Army != null)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Rearguard, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
					}
					if (party.ItemRoster.FoodVariety > 0 && mobileParty.HasPerk(DefaultPerks.Medicine.PerfectHealth, false))
					{
						float num2 = DefaultPerks.Medicine.PerfectHealth.PrimaryBonus;
						if (party.IsMobile && party.MobileParty.IsCurrentlyAtSea)
						{
							num2 *= 0.5f;
						}
						explainedNumber.AddFactor((float)mobileParty.ItemRoster.FoodVariety * num2, DefaultPerks.Medicine.PerfectHealth.Name);
					}
					if (mobileParty.HasPerk(DefaultPerks.Medicine.HelpingHands, false))
					{
						float num3 = (float)MathF.Floor((float)party.MemberRoster.TotalManCount / 10f);
						float num4 = DefaultPerks.Medicine.HelpingHands.PrimaryBonus;
						if (mobileParty.IsCurrentlyAtSea)
						{
							num4 *= 0.5f;
						}
						float num5 = num3 * num4;
						explainedNumber.AddFactor(num5, DefaultPerks.Medicine.HelpingHands.Name);
					}
				}
				if (mobileParty.IsInRaftState)
				{
					int totalRegulars2 = party.MemberRoster.TotalRegulars;
					explainedNumber.Add((float)(-(float)totalRegulars2) * 0.25f, DefaultPartyHealingModel._raftStateText, null);
				}
			}
			return explainedNumber;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0007CB5C File Offset: 0x0007AD5C
		public override ExplainedNumber GetDailyHealingHpForHeroes(PartyBase party, bool isPrisoners, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (isPrisoners)
			{
				explainedNumber.Add(20f, null, null);
			}
			else if (party == null)
			{
				explainedNumber.Add(11f, null, null);
			}
			else if (party.IsMobile)
			{
				MobileParty mobileParty = party.MobileParty;
				if (party.IsStarving && mobileParty.CurrentSettlement == null)
				{
					return new ExplainedNumber(-19f, includeDescriptions, DefaultPartyHealingModel._starvingText);
				}
				explainedNumber.Add(11f, null, null);
				if (!mobileParty.IsGarrison && !mobileParty.IsMilitia)
				{
					if (!mobileParty.IsMoving)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.TriageTent, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
					}
					else if (!mobileParty.IsCurrentlyAtSea)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.WalkItOff, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WalkItOff, mobileParty, true, ref explainedNumber, false);
					}
				}
				if (mobileParty.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BestMedicine, mobileParty, true, ref explainedNumber, mobileParty.IsCurrentlyAtSea);
				}
				if (mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsHideout)
				{
					if (mobileParty.CurrentSettlement.IsFortification)
					{
						explainedNumber.Add(8f, DefaultPartyHealingModel._settlementText, null);
					}
					if (mobileParty.CurrentSettlement.IsTown)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PristineStreets, mobileParty, false, ref explainedNumber, false);
					}
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.AGoodDaysRest, mobileParty, true, ref explainedNumber, false);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.GoodLogdings, mobileParty, true, ref explainedNumber, false);
				}
				else if (!mobileParty.IsMoving && mobileParty.LastVisitedSettlement != null && mobileParty.LastVisitedSettlement.IsVillage && mobileParty.LastVisitedSettlement.Position.DistanceSquared(party.Position) < 2f && !mobileParty.LastVisitedSettlement.IsUnderRaid && !mobileParty.LastVisitedSettlement.IsRaided)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BushDoctor, mobileParty, false, ref explainedNumber, false);
				}
				SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.HealingRateBonusForHeroes, mobileParty, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0007CD54 File Offset: 0x0007AF54
		public override int GetHeroesEffectedHealingAmount(Hero hero, float healingRate)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(healingRate, false, null);
			bool flag = (hero.PartyBelongedTo != null && hero.PartyBelongedTo.IsCurrentlyAtSea) || (hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.IsCurrentlyAtSea);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.SelfMedication, hero.CharacterObject, true, ref explainedNumber, flag);
			float resultNumber = explainedNumber.ResultNumber;
			if (resultNumber - (float)((int)resultNumber) > MBRandom.RandomFloat)
			{
				return (int)resultNumber + 1;
			}
			return (int)resultNumber;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0007CDDC File Offset: 0x0007AFDC
		public override ExplainedNumber GetBattleEndHealingAmount(PartyBase party, Hero hero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (hero.GetPerkValue(DefaultPerks.Medicine.PreventiveMedicine))
			{
				explainedNumber.Add(DefaultPerks.Medicine.PreventiveMedicine.SecondaryBonus * (float)(hero.MaxHitPoints - hero.HitPoints), DefaultPerks.Medicine.PreventiveMedicine.Name, null);
			}
			if (party.MapEventSide == party.MapEvent.AttackerSide && hero.GetPerkValue(DefaultPerks.Medicine.WalkItOff))
			{
				explainedNumber.Add(DefaultPerks.Medicine.WalkItOff.SecondaryBonus, DefaultPerks.Medicine.WalkItOff.Name, null);
			}
			return explainedNumber;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0007CE6C File Offset: 0x0007B06C
		private static void AddDoctorsOathSkillBonusForParty(MobileParty enemyParty, ref ExplainedNumber explainedNumber)
		{
			Hero effectiveRoleHolder = enemyParty.GetEffectiveRoleHolder(PartyRole.Surgeon);
			CharacterObject characterObject = ((effectiveRoleHolder != null) ? effectiveRoleHolder.CharacterObject : null) ?? SkillHelper.GetEffectivePartyLeaderForSkill(enemyParty.Party);
			if (characterObject != null)
			{
				MapEvent mapEvent = enemyParty.MapEvent;
				bool flag = mapEvent != null && mapEvent.IsPlayerMapEvent;
				int skillValue = characterObject.GetSkillValue(DefaultSkillEffects.SurgeonSurvivalBonus.EffectedSkill);
				float skillEffectValue = DefaultSkillEffects.SurgeonSurvivalBonus.GetSkillEffectValue(skillValue);
				explainedNumber.Add(skillEffectValue * (flag ? 1f : 0.1f), explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", PartyRole.Surgeon.ToString()) : null, null);
			}
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0007CF0C File Offset: 0x0007B10C
		private void AddSurgeonSurvivalBonus(MobileParty mobileParty, ref ExplainedNumber survivalDenominator)
		{
			Hero effectiveRoleHolder = mobileParty.GetEffectiveRoleHolder(PartyRole.Surgeon);
			CharacterObject characterObject = ((effectiveRoleHolder != null) ? effectiveRoleHolder.CharacterObject : null) ?? SkillHelper.GetEffectivePartyLeaderForSkill(mobileParty.Party);
			if (characterObject != null)
			{
				MapEvent mapEvent = mobileParty.MapEvent;
				bool flag = mapEvent != null && mapEvent.IsPlayerMapEvent;
				int skillValue = characterObject.GetSkillValue(DefaultSkillEffects.SurgeonSurvivalBonus.EffectedSkill);
				float skillEffectValue = DefaultSkillEffects.SurgeonSurvivalBonus.GetSkillEffectValue(skillValue);
				survivalDenominator.Add(skillEffectValue * (flag ? 1f : 0.25f), survivalDenominator.IncludeDescriptions ? GameTexts.FindText("role", PartyRole.Surgeon.ToString()) : null, null);
			}
		}

		// Token: 0x0400082B RID: 2091
		private const int StarvingEffectHeroes = -19;

		// Token: 0x0400082C RID: 2092
		private const int FortificationEffectForHeroes = 8;

		// Token: 0x0400082D RID: 2093
		private const int FortificationEffectForRegulars = 10;

		// Token: 0x0400082E RID: 2094
		private const int BaseDailyHealingForHeroes = 11;

		// Token: 0x0400082F RID: 2095
		private const int DailyHealingForPrisonerHeroes = 20;

		// Token: 0x04000830 RID: 2096
		private const int DailyHealingForPrisonerRegulars = 1;

		// Token: 0x04000831 RID: 2097
		private const int BaseDailyHealingForTroops = 5;

		// Token: 0x04000832 RID: 2098
		private const int SkillEXPFromHealingTroops = 5;

		// Token: 0x04000833 RID: 2099
		private const float StarvingWoundedEffectRatio = 0.25f;

		// Token: 0x04000834 RID: 2100
		private const float StarvingWoundedEffectRatioForGarrison = 0.1f;

		// Token: 0x04000835 RID: 2101
		private const float DriftingWoundedEffectRatio = 0.25f;

		// Token: 0x04000836 RID: 2102
		private const float AISurgeonSurvivalMultiplier = 0.25f;

		// Token: 0x04000837 RID: 2103
		private const float DoctorsOathMultiplier = 0.1f;

		// Token: 0x04000838 RID: 2104
		private static readonly TextObject _starvingText = new TextObject("{=jZYUdkXF}Starving", null);

		// Token: 0x04000839 RID: 2105
		private static readonly TextObject _settlementText = new TextObject("{=M0Gpl0dH}In Settlement", null);

		// Token: 0x0400083A RID: 2106
		private static readonly TextObject _raftStateText = new TextObject("{=dNJLG7O5}Stranded at sea", null);
	}
}
