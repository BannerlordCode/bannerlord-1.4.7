using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003A9 RID: 937
	public class DefaultSkillLevelingManager : ISkillLevelingManager
	{
		// Token: 0x06003629 RID: 13865 RVA: 0x000E2F38 File Offset: 0x000E1138
		public void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge, bool isSneakAttack)
		{
			if (isTeamKill)
			{
				return;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (affectorCharacter.IsHero)
			{
				Hero heroObject = affectorCharacter.HeroObject;
				CombatXpModel combatXpModel = Campaign.Current.Models.CombatXpModel;
				CharacterObject characterObject = heroObject.CharacterObject;
				MobileParty partyBelongedTo = heroObject.PartyBelongedTo;
				explainedNumber = new ExplainedNumber(combatXpModel.GetXpFromHit(characterObject, captain, affectedCharacter, (partyBelongedTo != null) ? partyBelongedTo.Party : null, (int)damageAmount, isFatal, missionType).ResultNumber, false, null);
				SkillObject skillObject;
				if (affectorWeapon != null)
				{
					skillObject = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(affectorWeapon, isSiegeEngineHit);
					float num = ((skillObject == DefaultSkills.Bow) ? 0.5f : 1f);
					if (shotDifficulty > 0f)
					{
						explainedNumber.AddFactor(num * Campaign.Current.Models.CombatXpModel.GetXpMultiplierFromShotDifficulty(shotDifficulty), null);
					}
				}
				else
				{
					skillObject = (isHorseCharge ? DefaultSkills.Riding : DefaultSkills.Athletics);
				}
				heroObject.AddSkillXp(skillObject, (float)MBRandom.RoundRandomized((float)explainedNumber.RoundedResultNumber));
				if (!isSiegeEngineHit && !isHorseCharge)
				{
					float num2 = shotDifficulty * 0.15f;
					if (isAffectorMounted)
					{
						float num3 = 0.5f;
						if (num2 > 0f)
						{
							num3 += num2;
						}
						if (speedBonusFromMovement > 0f)
						{
							num3 *= 1f + speedBonusFromMovement;
						}
						if (num3 > 0f)
						{
							DefaultSkillLevelingManager.OnGainingRidingExperience(heroObject, (float)MBRandom.RoundRandomized(num3 * (float)explainedNumber.RoundedResultNumber), heroObject.CharacterObject.Equipment.Horse.Item);
						}
					}
					else
					{
						float num4 = 1f;
						if (num2 > 0f)
						{
							num4 += num2;
						}
						if (speedBonusFromMovement > 0f)
						{
							num4 += 1.5f * speedBonusFromMovement;
						}
						if (num4 > 0f)
						{
							heroObject.AddSkillXp(DefaultSkills.Athletics, (float)MBRandom.RoundRandomized(num4 * explainedNumber.ResultNumber));
						}
					}
				}
				if (isSneakAttack)
				{
					heroObject.AddSkillXp(DefaultSkills.Roguery, 78f);
				}
			}
			if (commander != null && commander != affectorCharacter.HeroObject && commander.PartyBelongedTo != null)
			{
				this.OnTacticsUsed(commander.PartyBelongedTo, (float)MathF.Ceiling(0.02f * (float)explainedNumber.RoundedResultNumber));
			}
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000E315C File Offset: 0x000E135C
		public void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine)
		{
			if (((party != null) ? party.EffectiveEngineer : null) != null)
			{
				float num = (float)destroyedSiegeEngine.ManDayCost * 20f;
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Engineering, num, PartyRole.Engineer);
			}
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000E3194 File Offset: 0x000E1394
		public void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty)
		{
			int xpReward = Campaign.Current.Models.PartyTrainingModel.GetXpReward(affectedCharacter);
			if (affectorCharacter.IsHero)
			{
				ItemObject defaultWeapon = CharacterHelper.GetDefaultWeapon(affectorCharacter);
				Hero heroObject = affectorCharacter.HeroObject;
				if (defaultWeapon != null)
				{
					SkillObject skillForWeapon = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(defaultWeapon.GetWeaponWithUsageIndex(0), false);
					heroObject.AddSkillXp(skillForWeapon, (float)xpReward);
				}
				if (affectorCharacter.IsMounted)
				{
					float num = (float)xpReward * 0.3f;
					DefaultSkillLevelingManager.OnGainingRidingExperience(heroObject, (float)MBRandom.RoundRandomized(num), heroObject.CharacterObject.Equipment.Horse.Item);
				}
				else
				{
					float num2 = (float)xpReward * 0.3f;
					heroObject.AddSkillXp(DefaultSkills.Athletics, (float)MBRandom.RoundRandomized(num2));
				}
			}
			if (commanderParty != null && commanderParty.IsMobile && !commanderParty.MapEvent.IsNavalMapEvent && commanderParty.LeaderHero != null && commanderParty.LeaderHero != affectedCharacter.HeroObject)
			{
				this.OnTacticsUsed(commanderParty.MobileParty, (float)MathF.Ceiling(0.02f * (float)xpReward));
			}
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000E32A0 File Offset: 0x000E14A0
		public void OnTradeProfitMade(PartyBase party, int tradeProfit)
		{
			if (tradeProfit > 0)
			{
				float num = (float)tradeProfit * 0.5f;
				DefaultSkillLevelingManager.OnPartySkillExercised(party.MobileParty, DefaultSkills.Trade, num, PartyRole.PartyLeader);
			}
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000E32CC File Offset: 0x000E14CC
		public void OnTradeProfitMade(Hero hero, int tradeProfit)
		{
			if (tradeProfit > 0)
			{
				float num = (float)tradeProfit * 0.5f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Trade, num, hero == Hero.MainHero);
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000E32FA File Offset: 0x000E14FA
		public void OnSettlementProjectFinished(Settlement settlement)
		{
			DefaultSkillLevelingManager.OnSettlementSkillExercised(settlement, DefaultSkills.Steward, 1000f);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000E330C File Offset: 0x000E150C
		public void OnSettlementGoverned(Hero governor, Settlement settlement)
		{
			float prosperityChange = settlement.Town.ProsperityChange;
			if (prosperityChange > 0f)
			{
				float num = prosperityChange * 30f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(governor, DefaultSkills.Steward, num, true);
			}
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000E3344 File Offset: 0x000E1544
		public void OnInfluenceSpent(Hero hero, float amountSpent)
		{
			if (hero.PartyBelongedTo != null)
			{
				float num = 10f * amountSpent;
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Steward, num, PartyRole.PartyLeader);
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000E3374 File Offset: 0x000E1574
		public void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default)
		{
			if ((hero.PartyBelongedTo == null && detail != ChangeRelationAction.ChangeRelationDetail.Emissary) || relationChange <= 0f)
			{
				return;
			}
			int charmExperienceFromRelationGain = Campaign.Current.Models.DiplomacyModel.GetCharmExperienceFromRelationGain(gainedRelationWith, relationChange, detail);
			if (hero.PartyBelongedTo != null)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Charm, (float)charmExperienceFromRelationGain, PartyRole.PartyLeader);
				return;
			}
			DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Charm, (float)charmExperienceFromRelationGain, true);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000E33DC File Offset: 0x000E15DC
		public void OnTroopRecruited(Hero hero, int amount, int tier)
		{
			if (amount > 0)
			{
				int num = amount * tier * 2;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Leadership, (float)num, true);
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000E3404 File Offset: 0x000E1604
		public void OnBribeGiven(int amount)
		{
			if (amount > 0)
			{
				float num = (float)amount * 0.1f;
				DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Roguery, num, PartyRole.PartyLeader);
			}
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000E342F File Offset: 0x000E162F
		public void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count)
		{
			if (count > 0)
			{
				DefaultSkillLevelingManager.OnPersonalSkillExercised(mobileParty.LeaderHero, DefaultSkills.Roguery, (float)(count * 2 * bandit.Tier), true);
			}
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000E3454 File Offset: 0x000E1654
		public void OnMainHeroReleasedFromCaptivity(float captivityTime)
		{
			float num = captivityTime * 0.5f;
			DefaultSkillLevelingManager.OnPersonalSkillExercised(Hero.MainHero, DefaultSkills.Roguery, num, true);
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000E347C File Offset: 0x000E167C
		public void OnMainHeroTortured()
		{
			float num = MBRandom.RandomFloatRanged(50f, 100f);
			DefaultSkillLevelingManager.OnPersonalSkillExercised(Hero.MainHero, DefaultSkills.Roguery, num, true);
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000E34AC File Offset: 0x000E16AC
		public void OnMainHeroDisguised(bool isNotCaught)
		{
			float num = (isNotCaught ? MBRandom.RandomFloatRanged(10f, 25f) : MBRandom.RandomFloatRanged(1f, 10f));
			DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Roguery, num, PartyRole.PartyLeader);
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000E34F0 File Offset: 0x000E16F0
		public void OnRaid(MobileParty attackerParty, ItemRoster lootedItems)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = (float)lootedItems.TradeGoodsTotalValue * 0.5f + (float)(lootedItems.NumberOfMounts * 100) + (float)(lootedItems.NumberOfLivestockAnimals * 25) + (float)(lootedItems.NumberOfPackAnimals * 25);
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, num, true);
			}
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000E3548 File Offset: 0x000E1748
		public void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = 0f;
				if (forcedParty.IsVillager)
				{
					num = (attacked ? 0.75f : 0.5f);
				}
				else if (forcedParty.IsCaravan)
				{
					num = (attacked ? 0.15f : 0.1f);
				}
				float num2 = (float)(lootedItems.TradeGoodsTotalValue + lootedItems.NumberOfMounts * 200 + lootedItems.NumberOfLivestockAnimals * 50 + lootedItems.NumberOfPackAnimals * 50) * num;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, num2, true);
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000E35D4 File Offset: 0x000E17D4
		public void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			int num = 0;
			for (int i = 0; i < prisonerRoster.Count; i++)
			{
				num += prisonerRoster.data[i].Character.Tier * prisonerRoster.data[i].Number;
			}
			int num2 = num * 2;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Roguery, (float)num2, PartyRole.PartyLeader);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000E3634 File Offset: 0x000E1834
		public void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier)
		{
			float num = (float)(surgerySuccess ? (10 * troopTier) : (5 * troopTier));
			DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Medicine, num, PartyRole.Surgeon);
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000E365C File Offset: 0x000E185C
		public void OnTacticsUsed(MobileParty party, float xp)
		{
			if (xp > 0f)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Tactics, xp, PartyRole.PartyLeader);
			}
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000E3673 File Offset: 0x000E1873
		public void OnHideoutSpotted(MobileParty party, PartyBase spottedParty)
		{
			DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Scouting, 100f, PartyRole.Scout);
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000E3688 File Offset: 0x000E1888
		public void OnTrackDetected(Track track)
		{
			float skillFromTrackDetected = Campaign.Current.Models.MapTrackModel.GetSkillFromTrackDetected(track);
			DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Scouting, skillFromTrackDetected, PartyRole.Scout);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000E36BD File Offset: 0x000E18BD
		public void OnTravelOnFoot(Hero hero, float speed)
		{
			hero.AddSkillXp(DefaultSkills.Athletics, (float)(MBRandom.RoundRandomized(0.2f * speed) + 1));
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000E36DC File Offset: 0x000E18DC
		public void OnTravelOnHorse(Hero hero, float speed)
		{
			ItemObject item = hero.CharacterObject.Equipment.Horse.Item;
			DefaultSkillLevelingManager.OnGainingRidingExperience(hero, (float)MBRandom.RoundRandomized(0.3f * speed), item);
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000E3718 File Offset: 0x000E1918
		public void OnHeroHealedWhileWaiting(Hero hero, int healingAmount)
		{
			if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.EffectiveSurgeon != null)
			{
				float num = (float)Campaign.Current.Models.PartyHealingModel.GetSkillXpFromHealingTroop(hero.PartyBelongedTo.Party);
				float num2 = ((hero.PartyBelongedTo.CurrentSettlement != null && !hero.PartyBelongedTo.CurrentSettlement.IsCastle) ? 0.2f : 0.1f);
				num *= (float)healingAmount * num2 * (1f + (float)hero.PartyBelongedTo.EffectiveSurgeon.Level * 0.1f);
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Medicine, num, PartyRole.Surgeon);
			}
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000E37C4 File Offset: 0x000E19C4
		public void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier)
		{
			float num = (float)(Campaign.Current.Models.PartyHealingModel.GetSkillXpFromHealingTroop(mobileParty.Party) * healedTroopCount) * averageTier;
			float num2 = ((mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsCastle) ? 2f : 1f);
			num *= num2;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Medicine, num, PartyRole.Surgeon);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000E3824 File Offset: 0x000E1A24
		public void OnLeadingArmy(MobileParty mobileParty)
		{
			Army army = mobileParty.Army;
			float num = ((army != null) ? army.EstimatedStrength : mobileParty.Party.EstimatedStrength) * 0.0004f * mobileParty.Army.Morale;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Leadership, num, PartyRole.PartyLeader);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000E3870 File Offset: 0x000E1A70
		public void OnSieging(MobileParty mobileParty)
		{
			int num = mobileParty.MemberRoster.TotalManCount;
			if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
			{
				foreach (MobileParty mobileParty2 in mobileParty.Army.Parties)
				{
					if (mobileParty2 != mobileParty)
					{
						num += mobileParty2.MemberRoster.TotalManCount;
					}
				}
			}
			float num2 = 0.25f * MathF.Sqrt((float)num);
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Engineering, num2, PartyRole.Engineer);
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000E3910 File Offset: 0x000E1B10
		public void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine)
		{
			float num = 30f + 2f * (float)siegeEngine.Difficulty;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Engineering, num, PartyRole.Engineer);
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000E3940 File Offset: 0x000E1B40
		public void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops)
		{
			Hero hero = party.LeaderHero ?? party.Owner;
			if (hero != null)
			{
				SkillObject skillObject = DefaultSkills.Leadership;
				float num = 0.025f;
				if (troop.Occupation == Occupation.Bandit)
				{
					skillObject = DefaultSkills.Roguery;
					num = 0.05f;
				}
				float num2 = (float)Campaign.Current.Models.PartyTroopUpgradeModel.GetXpCostForUpgrade(party, troop, upgrade) * num * (float)numberOfTroops;
				hero.AddSkillXp(skillObject, num2);
			}
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000E39AC File Offset: 0x000E1BAC
		public void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			float num = (float)Campaign.Current.Models.PersuasionModel.GetSkillXpFromPersuasion(difficulty, argumentDifficultyBonusCoefficient);
			if (num > 0f)
			{
				targetHero.AddSkillXp(skill, num);
			}
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000E39E4 File Offset: 0x000E1BE4
		public void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded)
		{
			float rogueryRewardOnPrisonBreak = Campaign.Current.Models.PrisonBreakModel.GetRogueryRewardOnPrisonBreak(prisonerHero, isSucceeded);
			if (rogueryRewardOnPrisonBreak > 0f)
			{
				Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryRewardOnPrisonBreak);
			}
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000E3A20 File Offset: 0x000E1C20
		public void OnWallBreached(MobileParty party)
		{
			if (((party != null) ? party.EffectiveEngineer : null) != null)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Engineering, 250f, PartyRole.Engineer);
			}
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000E3A44 File Offset: 0x000E1C44
		public void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty)
		{
			if (attackerParty.LeaderHero != null)
			{
				int num = MathF.Ceiling(forcedParty.Settlement.Village.Hearth / 10f);
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, (float)num, true);
			}
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000E3A88 File Offset: 0x000E1C88
		public void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = (attacked ? 0.75f : 0.5f);
				float num2 = (float)(lootedItems.TradeGoodsTotalValue + lootedItems.NumberOfMounts * 200 + lootedItems.NumberOfLivestockAnimals * 50 + lootedItems.NumberOfPackAnimals * 50) * num;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, num2, true);
			}
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000E3AEC File Offset: 0x000E1CEC
		public void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType)
		{
			int num = ((currentTerrainType == TerrainType.Forest) ? MBRandom.RoundRandomized(5f) : MBRandom.RoundRandomized(3f));
			hero.AddSkillXp(DefaultSkills.Scouting, isCaravanParty ? ((float)num / 2f) : ((float)num));
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000E3B30 File Offset: 0x000E1D30
		public void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType)
		{
			float num = 0f;
			float lastCalculatedSpeed = mobileParty._lastCalculatedSpeed;
			if (lastCalculatedSpeed > 1f)
			{
				bool flag = currentTerrainType == TerrainType.Desert || currentTerrainType == TerrainType.Dune || currentTerrainType == TerrainType.Forest || currentTerrainType == TerrainType.Snow;
				num = lastCalculatedSpeed * (1f + MathF.Pow((float)mobileParty.MemberRoster.TotalManCount, 0.66f)) * (flag ? 0.25f : 0.15f);
			}
			if (mobileParty.IsCaravan)
			{
				num *= 0.5f;
			}
			if (num >= 5f)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Scouting, num, PartyRole.Scout);
			}
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000E3BBC File Offset: 0x000E1DBC
		public void OnBattleEnded(PartyBase party, CharacterObject troop, int excessXp)
		{
			Hero hero = party.LeaderHero ?? party.Owner;
			float num = 0.025f;
			SkillObject skillObject = DefaultSkills.Leadership;
			if (troop.Occupation == Occupation.Bandit)
			{
				num = 0.05f;
				skillObject = DefaultSkills.Roguery;
			}
			float num2 = (float)excessXp * num;
			hero.AddSkillXp(skillObject, num2);
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000E3C08 File Offset: 0x000E1E08
		public void OnFoodConsumed(MobileParty mobileParty, bool wasStarving)
		{
			if (!wasStarving && mobileParty.ItemRoster.FoodVariety > 3 && mobileParty.EffectiveQuartermaster != null)
			{
				float num = (float)MathF.Round(-mobileParty.BaseFoodChange * 100f) * ((float)mobileParty.ItemRoster.FoodVariety - 2f) / 3f;
				DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Steward, num, PartyRole.Quartermaster);
			}
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000E3C69 File Offset: 0x000E1E69
		public void OnAlleyCleared(Alley alley)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetInitialXpGainForMainHero());
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000E3C90 File Offset: 0x000E1E90
		public void OnDailyAlleyTick(Alley alley, Hero alleyLeader)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetDailyXpGainForMainHero());
			if (alleyLeader != null && !alleyLeader.IsDead)
			{
				alleyLeader.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetDailyXpGainForAssignedClanMember(alleyLeader));
			}
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000E3CEC File Offset: 0x000E1EEC
		public void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain)
		{
			switch (difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20f);
				break;
			case BoardGameHelper.AIDifficulty.Normal:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50f);
				break;
			case BoardGameHelper.AIDifficulty.Hard:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 100f);
				break;
			}
			if (extraXpGain)
			{
				lord.AddSkillXp(DefaultSkills.Steward, 100f);
			}
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000E3D60 File Offset: 0x000E1F60
		public void OnHideoutClearedAsGhost()
		{
			TextObject textObject = new TextObject("{=Obuhsttm}Ghost bonus: {XP} Roguery exp! (Base: {BASE_XP})", null);
			float rogueryXpGainAsGhost = Campaign.Current.Models.HideoutModel.GetRogueryXpGainAsGhost();
			float skillXp = Hero.MainHero.HeroDeveloper.GetSkillXp(DefaultSkills.Roguery);
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryXpGainAsGhost);
			float num = Hero.MainHero.HeroDeveloper.GetSkillXp(DefaultSkills.Roguery) - skillXp;
			textObject.SetTextVariable("BASE_XP", MathF.Floor(rogueryXpGainAsGhost));
			textObject.SetTextVariable("XP", MathF.Floor(num));
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), new Color(0f, 0f, 1f, 1f)));
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000E3E18 File Offset: 0x000E2018
		public void OnHideoutMissionEnd(bool isSucceeded)
		{
			float rogueryXpGainOnHideoutMissionEnd = Campaign.Current.Models.HideoutModel.GetRogueryXpGainOnHideoutMissionEnd(isSucceeded);
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryXpGainOnHideoutMissionEnd);
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000E3E4B File Offset: 0x000E204B
		public void OnWarehouseProduction(EquipmentElement production)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Trade, Campaign.Current.Models.WorkshopModel.GetTradeXpPerWarehouseProduction(production));
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000E3E74 File Offset: 0x000E2074
		public void OnAIPartyLootCasualties(int goldAmount, Hero winnerPartyLeader, PartyBase defeatedParty)
		{
			if (defeatedParty.IsMobile)
			{
				float num = -1f;
				MobileParty mobileParty = defeatedParty.MobileParty;
				if (mobileParty.IsVillager)
				{
					num = 0.75f;
				}
				else if (mobileParty.IsCaravan)
				{
					num = 0.15f;
				}
				if (num > 0f)
				{
					float num2 = (float)goldAmount * num;
					winnerPartyLeader.HeroDeveloper.AddSkillXp(DefaultSkills.Roguery, num2, true, false);
				}
			}
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000E3ED4 File Offset: 0x000E20D4
		public void OnShipDamaged(Ship ship, float rawDamage, float finalDamage)
		{
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000E3ED6 File Offset: 0x000E20D6
		public void OnShipRepaired(Ship ship, float repairedHitPoints)
		{
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000E3ED8 File Offset: 0x000E20D8
		public void OnTravelOnWater(MobileParty party, float speed)
		{
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000E3EDA File Offset: 0x000E20DA
		private static void OnPersonalSkillExercised(Hero hero, SkillObject skill, float skillXp, bool shouldNotify = true)
		{
			if (hero != null)
			{
				hero.HeroDeveloper.AddSkillXp(skill, skillXp, true, shouldNotify);
			}
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000E3EF0 File Offset: 0x000E20F0
		private static void OnSettlementSkillExercised(Settlement settlement, SkillObject skill, float skillXp)
		{
			Town town = settlement.Town;
			Hero hero = ((town != null) ? town.Governor : null) ?? ((settlement.OwnerClan.Leader.CurrentSettlement == settlement) ? settlement.OwnerClan.Leader : null);
			if (hero == null)
			{
				return;
			}
			hero.AddSkillXp(skill, skillXp);
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000E3F40 File Offset: 0x000E2140
		private static void OnGainingRidingExperience(Hero hero, float baseXpAmount, ItemObject horse)
		{
			if (horse != null)
			{
				float num = 1f + (float)horse.Difficulty * 0.02f;
				hero.AddSkillXp(DefaultSkills.Riding, baseXpAmount * num);
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000E3F72 File Offset: 0x000E2172
		private static void OnPartySkillExercised(MobileParty party, SkillObject skill, float skillXp, PartyRole partyRole = PartyRole.PartyLeader)
		{
			Hero effectiveRoleHolder = party.GetEffectiveRoleHolder(partyRole);
			if (effectiveRoleHolder == null)
			{
				return;
			}
			effectiveRoleHolder.AddSkillXp(skill, skillXp);
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000E3F8F File Offset: 0x000E218F
		void ISkillLevelingManager.OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			this.OnPrisonerSell(mobileParty, in prisonerRoster);
		}

		// Token: 0x040010F0 RID: 4336
		private const float TacticsXpCoefficient = 0.02f;

		// Token: 0x040010F1 RID: 4337
		private const int RogueryXpGainOnSneakAttack = 78;
	}
}
