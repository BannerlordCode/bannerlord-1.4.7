using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013E RID: 318
	public class DefaultPartyWageModel : PartyWageModel
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x0007FE99 File Offset: 0x0007E099
		public override int MaxWagePaymentLimit
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0007FEA0 File Offset: 0x0007E0A0
		public override int GetCharacterWage(CharacterObject character)
		{
			int num;
			switch (character.Tier)
			{
			case 0:
				num = 1;
				break;
			case 1:
				num = 2;
				break;
			case 2:
				num = 3;
				break;
			case 3:
				num = 5;
				break;
			case 4:
				num = 8;
				break;
			case 5:
				num = 12;
				break;
			case 6:
				num = 17;
				break;
			default:
				num = 23;
				break;
			}
			if (character.Occupation == Occupation.Mercenary)
			{
				num = (int)((float)num * 1.5f);
			}
			return num;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0007FF10 File Offset: 0x0007E110
		public override ExplainedNumber GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions = false)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			bool flag = !mobileParty.HasPerk(DefaultPerks.Steward.AidCorps, false);
			int num7 = 0;
			int num8 = 0;
			for (int i = 0; i < troopRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(i);
				CharacterObject character = elementCopyAtIndex.Character;
				if (!flag)
				{
					int number = elementCopyAtIndex.Number;
					int woundedNumber = elementCopyAtIndex.WoundedNumber;
				}
				else
				{
					int number2 = elementCopyAtIndex.Number;
				}
				if (character.IsHero)
				{
					bool flag2 = mobileParty.IsMainParty && character.HeroObject.Clan == Clan.PlayerClan && character.HeroObject.Occupation == Occupation.Lord;
					Hero heroObject = elementCopyAtIndex.Character.HeroObject;
					Clan clan = character.HeroObject.Clan;
					if (heroObject != ((clan != null) ? clan.Leader : null) && !flag2)
					{
						if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Steward.PaidInPromise))
						{
							num += MathF.Round((float)character.TroopWage * (1f + DefaultPerks.Steward.PaidInPromise.PrimaryBonus));
						}
						else
						{
							num += character.TroopWage;
						}
					}
				}
				else
				{
					int num9 = character.TroopWage * elementCopyAtIndex.Number;
					num += num9;
					if (character.Culture.IsBandit)
					{
						num6 += num9;
					}
					if (character.IsInfantry)
					{
						num2 += num9;
					}
					if (character.IsMounted)
					{
						num3 += num9;
					}
					if (character.Occupation == Occupation.CaravanGuard)
					{
						num7 += num9;
					}
					if (character.Occupation == Occupation.Mercenary)
					{
						num8 += num9;
					}
					if (character.IsRanged)
					{
						num4 += num9;
						if (character.Tier >= 4)
						{
							num5 += num9;
						}
					}
				}
			}
			if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Roguery.DeepPockets))
			{
				num -= num6;
				ExplainedNumber explainedNumber = new ExplainedNumber((float)num6, false, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DeepPockets, mobileParty.LeaderHero.CharacterObject, false, ref explainedNumber, false);
				num += (int)explainedNumber.ResultNumber;
			}
			if (num5 > 0)
			{
				num -= num5;
				ExplainedNumber explainedNumber2 = new ExplainedNumber((float)num5, false, null);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.PickedShots, mobileParty, true, ref explainedNumber2, mobileParty.IsCurrentlyAtSea);
				num += (int)explainedNumber2.ResultNumber;
			}
			ExplainedNumber explainedNumber3 = new ExplainedNumber((float)num, includeDescriptions, null);
			explainedNumber3.LimitMin(0f);
			ExplainedNumber explainedNumber4 = new ExplainedNumber(1f, false, null);
			if (mobileParty.IsGarrison)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) != null)
				{
					if (mobileParty.CurrentSettlement.IsFortification)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.MilitaryTradition, mobileParty.CurrentSettlement.Town, ref explainedNumber3);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Berserker, mobileParty.CurrentSettlement.Town, ref explainedNumber3);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.DrillSergant, mobileParty.CurrentSettlement.Town, ref explainedNumber3);
						float num10 = (float)num2 / explainedNumber3.BaseNumber;
						this.CalculatePartialGarrisonWageReduction(num10, mobileParty, DefaultPerks.Polearm.StandardBearer, ref explainedNumber3, true);
						float num11 = (float)num4 / explainedNumber3.BaseNumber;
						this.CalculatePartialGarrisonWageReduction(num11, mobileParty, DefaultPerks.Crossbow.PeasantLeader, ref explainedNumber3, true);
						float num12 = (float)num3 / explainedNumber3.BaseNumber;
						this.CalculatePartialGarrisonWageReduction(num12, mobileParty, DefaultPerks.Riding.CavalryTactics, ref explainedNumber3, true);
					}
					if (mobileParty.CurrentSettlement.IsCastle)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.HunterClan, mobileParty.CurrentSettlement.Town, ref explainedNumber3);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.StiffUpperLip, mobileParty.CurrentSettlement.Town, ref explainedNumber3);
					}
					if (mobileParty.CurrentSettlement.Owner.Culture.HasFeat(DefaultCulturalFeats.EmpireGarrisonWageFeat))
					{
						explainedNumber3.AddFactor(DefaultCulturalFeats.EmpireGarrisonWageFeat.EffectBonus, this._cultureText);
					}
					mobileParty.CurrentSettlement.Town.AddEffectOfBuildings(BuildingEffectEnum.GarrisonWageReduction, ref explainedNumber4);
				}
			}
			float num13 = ((mobileParty.LeaderHero != null && mobileParty.LeaderHero.Clan.Kingdom != null && !mobileParty.LeaderHero.Clan.IsUnderMercenaryService && mobileParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.MilitaryCoronae)) ? 0.1f : 0f);
			if (mobileParty.HasPerk(DefaultPerks.Trade.SwordForBarter, true))
			{
				float num14 = (float)num7 / explainedNumber3.BaseNumber;
				if (num14 > 0f)
				{
					float num15 = DefaultPerks.Trade.SwordForBarter.SecondaryBonus * num14;
					explainedNumber3.AddFactor(num15, DefaultPerks.Trade.SwordForBarter.Name);
				}
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.Contractors, false))
			{
				float num16 = (float)num8 / explainedNumber3.BaseNumber;
				if (num16 > 0f)
				{
					float num17 = DefaultPerks.Steward.Contractors.PrimaryBonus * num16;
					explainedNumber3.AddFactor(num17, DefaultPerks.Steward.Contractors.Name);
				}
			}
			if (mobileParty.HasPerk(DefaultPerks.Trade.MercenaryConnections, true))
			{
				float num18 = (float)num8 / explainedNumber3.BaseNumber;
				if (num18 > 0f)
				{
					float num19 = DefaultPerks.Trade.MercenaryConnections.SecondaryBonus * num18;
					explainedNumber3.AddFactor(num19, DefaultPerks.Trade.MercenaryConnections.Name);
				}
			}
			explainedNumber3.AddFactor(num13, DefaultPolicies.MilitaryCoronae.Name);
			explainedNumber3.AddFactor(explainedNumber4.ResultNumber - 1f, this._buildingEffects);
			if (PartyBaseHelper.HasFeat(mobileParty.Party, DefaultCulturalFeats.AseraiIncreasedWageFeat))
			{
				explainedNumber3.AddFactor(DefaultCulturalFeats.AseraiIncreasedWageFeat.EffectBonus, this._cultureText);
			}
			if (!mobileParty.IsCurrentlyAtSea && mobileParty.HasPerk(DefaultPerks.Steward.Frugal, false))
			{
				explainedNumber3.AddFactor(DefaultPerks.Steward.Frugal.PrimaryBonus, DefaultPerks.Steward.Frugal.Name);
			}
			if (mobileParty.Army != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.EfficientCampaigner, mobileParty, false, ref explainedNumber3, mobileParty.IsCurrentlyAtSea);
			}
			if (mobileParty.SiegeEvent != null && mobileParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(mobileParty.Party, MapEvent.BattleTypes.Siege) && mobileParty.HasPerk(DefaultPerks.Steward.MasterOfWarcraft, false))
			{
				explainedNumber3.AddFactor(DefaultPerks.Steward.MasterOfWarcraft.PrimaryBonus, DefaultPerks.Steward.MasterOfWarcraft.Name);
			}
			if (mobileParty.EffectiveQuartermaster != null)
			{
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, mobileParty.EffectiveQuartermaster.CharacterObject, DefaultSkills.Steward, true, ref explainedNumber3, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
			}
			if (mobileParty.CurrentSettlement != null && mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Trade.ContentTrades))
			{
				explainedNumber3.AddFactor(DefaultPerks.Trade.ContentTrades.SecondaryBonus, DefaultPerks.Trade.ContentTrades.Name);
			}
			return explainedNumber3;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00080560 File Offset: 0x0007E760
		private void CalculatePartialGarrisonWageReduction(float troopRatio, MobileParty mobileParty, PerkObject perk, ref ExplainedNumber garrisonWageReductionMultiplier, bool isSecondaryEffect)
		{
			if (troopRatio > 0f && mobileParty.CurrentSettlement.Town.Governor != null && PerkHelper.GetPerkValueForTown(perk, mobileParty.CurrentSettlement.Town))
			{
				garrisonWageReductionMultiplier.AddFactor(isSecondaryEffect ? (perk.SecondaryBonus * troopRatio) : (perk.PrimaryBonus * troopRatio), perk.Name);
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000805C0 File Offset: 0x0007E7C0
		public override ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
		{
			ExplainedNumber explainedNumber;
			if (troop.Level <= 1)
			{
				explainedNumber = new ExplainedNumber(10f, false, null);
			}
			else if (troop.Level <= 6)
			{
				explainedNumber = new ExplainedNumber(20f, false, null);
			}
			else if (troop.Level <= 11)
			{
				explainedNumber = new ExplainedNumber(50f, false, null);
			}
			else if (troop.Level <= 16)
			{
				explainedNumber = new ExplainedNumber(100f, false, null);
			}
			else if (troop.Level <= 21)
			{
				explainedNumber = new ExplainedNumber(200f, false, null);
			}
			else if (troop.Level <= 26)
			{
				explainedNumber = new ExplainedNumber(400f, false, null);
			}
			else if (troop.Level <= 31)
			{
				explainedNumber = new ExplainedNumber(600f, false, null);
			}
			else if (troop.Level <= 36)
			{
				explainedNumber = new ExplainedNumber(1000f, false, null);
			}
			else
			{
				explainedNumber = new ExplainedNumber(1500f, false, null);
			}
			if (troop.Equipment.Horse.Item != null && !withoutItemCost)
			{
				if (troop.Level < 26)
				{
					explainedNumber.Add(150f, null, null);
				}
				else
				{
					explainedNumber.Add(500f, null, null);
				}
			}
			bool flag = troop.Occupation == Occupation.Mercenary || troop.Occupation == Occupation.Gangster || troop.Occupation == Occupation.CaravanGuard;
			if (flag)
			{
				explainedNumber.Add(explainedNumber.BaseNumber * 2f, null, null);
			}
			if (buyerHero != null)
			{
				if (troop.Tier >= 2 && buyerHero.GetPerkValue(DefaultPerks.Throwing.HeadHunter))
				{
					explainedNumber.AddFactor(DefaultPerks.Throwing.HeadHunter.SecondaryBonus, null);
				}
				if (troop.IsInfantry)
				{
					if (buyerHero.GetPerkValue(DefaultPerks.OneHanded.ChinkInTheArmor))
					{
						explainedNumber.AddFactor(DefaultPerks.OneHanded.ChinkInTheArmor.SecondaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.TwoHanded.ShowOfStrength))
					{
						explainedNumber.AddFactor(DefaultPerks.TwoHanded.ShowOfStrength.SecondaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.Polearm.HardyFrontline))
					{
						explainedNumber.AddFactor(DefaultPerks.Polearm.HardyFrontline.SecondaryBonus, null);
					}
				}
				else if (troop.IsRanged)
				{
					if (buyerHero.GetPerkValue(DefaultPerks.Bow.RenownedArcher))
					{
						explainedNumber.AddFactor(DefaultPerks.Bow.RenownedArcher.SecondaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.Crossbow.Piercer))
					{
						explainedNumber.AddFactor(DefaultPerks.Crossbow.Piercer.SecondaryBonus, null);
					}
				}
				if (troop.IsMounted && buyerHero.Culture.HasFeat(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat))
				{
					explainedNumber.AddFactor(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat.EffectBonus, this._cultureText);
				}
				if (buyerHero.IsPartyLeader && buyerHero.GetPerkValue(DefaultPerks.Steward.Frugal))
				{
					explainedNumber.AddFactor(DefaultPerks.Steward.Frugal.SecondaryBonus, null);
				}
				if (flag)
				{
					if (buyerHero.GetPerkValue(DefaultPerks.Trade.SwordForBarter))
					{
						explainedNumber.AddFactor(DefaultPerks.Trade.SwordForBarter.PrimaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						explainedNumber.AddFactor(DefaultPerks.Charm.SlickNegotiator.PrimaryBonus, null);
					}
				}
				explainedNumber.LimitMin(1f);
			}
			return explainedNumber;
		}

		// Token: 0x04000885 RID: 2181
		private readonly TextObject _cultureText = GameTexts.FindText("str_culture", null);

		// Token: 0x04000886 RID: 2182
		private readonly TextObject _buildingEffects = GameTexts.FindText("str_building_effects", null);

		// Token: 0x04000887 RID: 2183
		private const float MercenaryWageFactor = 1.5f;
	}
}
