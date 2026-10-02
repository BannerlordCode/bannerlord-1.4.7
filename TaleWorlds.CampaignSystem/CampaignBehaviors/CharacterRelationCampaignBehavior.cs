using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003DE RID: 990
	public class CharacterRelationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003CD9 RID: 15577 RVA: 0x00104120 File Offset: 0x00102320
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.UpdateFriendshipAndEnemies));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.OnPrisonerDonatedToSettlementEvent.AddNonSerializedListener(this, new Action<MobileParty, FlattenedTroopRoster, Settlement>(this.OnPrisonerDonatedToSettlement));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(CharacterRelationCampaignBehavior.OnHeroesMarried));
			CampaignEvents.OnHeroUnregisteredEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroUnregistered));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00104244 File Offset: 0x00102444
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if ((detail == KillCharacterAction.KillCharacterActionDetail.Executed || detail == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent) && killer == Hero.MainHero && victim.Clan != null)
			{
				int num = 0;
				foreach (Clan clan in Clan.All)
				{
					if (!clan.IsEliminated && !clan.IsBanditFaction && clan != Clan.PlayerClan)
					{
						bool flag;
						int relationChangeForExecutingHero = Campaign.Current.Models.ExecutionRelationModel.GetRelationChangeForExecutingHero(victim, clan.Leader, out flag);
						if (relationChangeForExecutingHero != 0)
						{
							Hero leader = clan.Leader;
							ChangeRelationAction.ApplyPlayerRelation(leader, relationChangeForExecutingHero, true, false);
							if (flag)
							{
								num++;
								TextObject textObject = GameTexts.FindText("str_your_relation_decreased_with_clan", null);
								textObject.SetTextVariable("CLAN_LEADER", clan.Name);
								textObject.SetTextVariable("VALUE", leader.GetRelation(killer));
								textObject.SetTextVariable("MAGNITUDE", MathF.Abs(relationChangeForExecutingHero));
								InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
							}
						}
					}
				}
				if (num > 0)
				{
					TextObject textObject2 = new TextObject("{=oqO9kjeW}The execution has hurt your relations with {COUNT} {?IS_PLURAL}clans{?}clan{\\?}.", null);
					MBTextManager.SetTextVariable("IS_PLURAL", (num > 1) ? 1 : 0);
					textObject2.SetTextVariable("COUNT", num);
					MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
				}
			}
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x001043A4 File Offset: 0x001025A4
		private void OnHeroUnregistered(Hero hero)
		{
			Campaign.Current.CharacterRelationManager.RemoveHero(hero);
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x001043B6 File Offset: 0x001025B6
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (relationChange > 0)
			{
				SkillLevelingManager.OnGainRelation(originalHero, effectiveHeroGainedRelationWith, (float)relationChange, detail);
			}
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x001043C8 File Offset: 0x001025C8
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.HasWinner)
			{
				MapEventSide winnerSide = mapEvent.Winner;
				MapEventSide otherSide = winnerSide.OtherSide;
				if (mapEvent.EventType == MapEvent.BattleTypes.FieldBattle || mapEvent.EventType == MapEvent.BattleTypes.Siege || mapEvent.EventType == MapEvent.BattleTypes.SiegeOutside)
				{
					bool flag = false;
					foreach (MapEventParty mapEventParty in otherSide.Parties)
					{
						if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsLordParty)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						Hero leaderHero = winnerSide.LeaderParty.LeaderHero;
						if (leaderHero != null && leaderHero.GetPerkValue(DefaultPerks.Charm.Oratory))
						{
							Hero randomElementWithPredicate = Hero.AllAliveHeroes.GetRandomElementWithPredicate<Hero>(delegate(Hero x)
							{
								if (x.IsActive && x.IsNotable)
								{
									Settlement currentSettlement = x.CurrentSettlement;
									return ((currentSettlement != null) ? currentSettlement.MapFaction : null) == winnerSide.LeaderParty.MapFaction;
								}
								return false;
							});
							if (randomElementWithPredicate != null)
							{
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(winnerSide.LeaderParty.LeaderHero, randomElementWithPredicate, (int)DefaultPerks.Charm.Oratory.SecondaryBonus, true);
							}
						}
						Hero leaderHero2 = winnerSide.LeaderParty.LeaderHero;
						if (leaderHero2 != null && leaderHero2.GetPerkValue(DefaultPerks.Charm.Warlord))
						{
							Hero randomElementWithPredicate2 = winnerSide.LeaderParty.MapFaction.AliveLords.GetRandomElementWithPredicate<Hero>((Hero x) => x != winnerSide.LeaderParty.LeaderHero);
							if (randomElementWithPredicate2 != null)
							{
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(winnerSide.LeaderParty.LeaderHero, randomElementWithPredicate2, (int)DefaultPerks.Charm.Warlord.SecondaryBonus, true);
							}
						}
					}
				}
				int num = winnerSide.CalculateTotalContribution();
				if (num > 0)
				{
					MBReadOnlyList<MapEventParty> parties = winnerSide.Parties;
					List<MobileParty> list = new List<MobileParty>();
					List<MobileParty> list2 = new List<MobileParty>();
					foreach (MapEventParty mapEventParty2 in parties)
					{
						PartyBase party = mapEventParty2.Party;
						if (party.IsMobile)
						{
							if (party.MobileParty.IsVillager)
							{
								list.Add(party.MobileParty);
							}
							else if (party.MobileParty.IsCaravan)
							{
								list2.Add(party.MobileParty);
							}
						}
					}
					foreach (MapEventParty mapEventParty3 in parties)
					{
						PartyBase party2 = mapEventParty3.Party;
						if (party2.LeaderHero != null)
						{
							float num2 = (float)mapEventParty3.ContributionToBattle / (float)num;
							if (num2 > 0f)
							{
								if (mapEvent.EventType == MapEvent.BattleTypes.Raid && winnerSide.MissionSide == BattleSideEnum.Defender && mapEvent.MapEventSettlement.Notables.Count > 0)
								{
									ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mapEvent.MapEventSettlement.Notables.GetRandomElement<Hero>(), party2.LeaderHero, 5, true);
								}
								foreach (MobileParty mobileParty in list)
								{
									if (mobileParty.HomeSettlement.OwnerClan != party2.LeaderHero.Clan && !mobileParty.HomeSettlement.OwnerClan.IsEliminated && !party2.LeaderHero.Clan.IsEliminated)
									{
										int num3 = MBRandom.RoundRandomized(4f * num2);
										if (num3 > 0)
										{
											ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.HomeSettlement.OwnerClan.Leader, party2.LeaderHero.Clan.Leader, num3, true);
										}
										int num4 = MBRandom.RoundRandomized(2f * num2);
										if (num4 > 0)
										{
											foreach (Hero hero in mobileParty.HomeSettlement.Notables)
											{
												ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, party2.LeaderHero.Clan.Leader, num4, true);
											}
										}
									}
								}
								foreach (MobileParty mobileParty2 in list2)
								{
									if (mobileParty2.HomeSettlement != null && mobileParty2.HomeSettlement.OwnerClan != null && party2.LeaderHero != null && mobileParty2.HomeSettlement.OwnerClan.Leader.Clan != party2.LeaderHero.Clan && mobileParty2.Party.Owner != null && mobileParty2.Party.Owner != Hero.MainHero && mobileParty2.Party.Owner.IsAlive && party2.LeaderHero.Clan.Leader != null && party2.LeaderHero.Clan.Leader.IsAlive && !mobileParty2.IsCurrentlyUsedByAQuest)
									{
										int num5 = MBRandom.RoundRandomized(6f * num2);
										ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty2.Party.Owner, party2.LeaderHero.Clan.Leader, num5, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x0010496C File Offset: 0x00102B6C
		private void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
			if (donatingParty.IsMainParty)
			{
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in donatedPrisoners)
				{
					if (flattenedTroopRosterElement.Troop.IsHero)
					{
						float num = Campaign.Current.Models.PrisonerDonationModel.CalculateRelationGainAfterHeroPrisonerDonate(donatingParty.Party, flattenedTroopRosterElement.Troop.HeroObject, donatedSettlement);
						if (num != 0f)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, donatedSettlement.OwnerClan.Leader, (int)num, true);
						}
					}
				}
			}
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00104A0C File Offset: 0x00102C0C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00104A10 File Offset: 0x00102C10
		private void UpdateFriendshipAndEnemies(CampaignGameStarter campaignGameStarter)
		{
			List<Hero> list = new List<Hero>(Hero.AllAliveHeroes.Count + Hero.DeadOrDisabledHeroes.Count);
			foreach (Hero hero in Campaign.Current.AliveHeroes)
			{
				if (hero.IsLord && hero != Hero.MainHero && hero.MapFaction != null)
				{
					IFaction mapFaction = hero.MapFaction;
					if (((mapFaction != null) ? mapFaction.Leader : null) != Hero.MainHero)
					{
						list.Add(hero);
					}
				}
			}
			foreach (Hero hero2 in Campaign.Current.DeadOrDisabledHeroes)
			{
				if (hero2.IsLord && hero2 != Hero.MainHero && hero2.MapFaction != null)
				{
					IFaction mapFaction2 = hero2.MapFaction;
					if (((mapFaction2 != null) ? mapFaction2.Leader : null) != Hero.MainHero)
					{
						list.Add(hero2);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				Hero hero3 = list[i];
				for (int j = i + 1; j < list.Count; j++)
				{
					Hero hero4 = list[j];
					if ((!hero4.IsDead || !(hero4.DeathDay < hero3.BirthDay)) && (!hero3.IsDead || !(hero3.DeathDay < hero4.BirthDay)))
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(hero3.MapFaction.FactionMidSettlement, hero4.MapFaction.FactionMidSettlement, false, false, MobileParty.NavigationType.All);
						float num = 1f / (2f + 5f * (distance / Campaign.Current.Models.MapDistanceModel.GetMaximumDistanceBetweenTwoConnectedSettlements(MobileParty.NavigationType.All)));
						if (hero3 == hero3.MapFaction.Leader || hero4 == hero4.MapFaction.Leader)
						{
							num = MathF.Sqrt(num);
						}
						if ((hero3.Clan != null && hero3.Clan.Tier >= 5 && hero4.Clan != null && hero4.Clan.Tier >= 5) || (hero3.IsKingdomLeader && hero4.IsKingdomLeader))
						{
							num = 1f;
						}
						if (MBRandom.RandomFloat < num)
						{
							float num2 = 0f;
							int num3 = HeroHelper.NPCPersonalityClashWithNPC(hero3, hero4);
							if (hero3.IsKingdomLeader && hero4.IsKingdomLeader)
							{
								if (hero3.Culture == hero4.Culture)
								{
									hero3.SetPersonalRelation(hero4, MathF.Round(MBRandom.RandomFloatRanged(35f, 100f)) * -1);
									goto IL_03A3;
								}
								if (num3 == 0)
								{
									num2 = (float)(((double)MBRandom.RandomFloat > 0.5) ? MathF.Round(MBRandom.RandomFloatRanged(-100f, -35f)) : MathF.Round(MBRandom.RandomFloatRanged(35f, 100f)));
								}
								else
								{
									for (int k = 0; k < 4; k++)
									{
										num2 += MBRandom.RandomFloat * 2f - 1f;
									}
									num2 = MBMath.ClampFloat(num2 * 30f, -100f, 100f);
								}
							}
							else
							{
								for (int l = 0; l < 4; l++)
								{
									num2 += MBRandom.RandomFloat * 2f - 1f;
								}
								num2 = MBMath.ClampFloat(num2 * 30f, -100f, 100f);
							}
							if (num3 == 0)
							{
								hero3.SetPersonalRelation(hero4, MathF.Round(num2));
							}
							else if (num3 < 0)
							{
								hero3.SetPersonalRelation(hero4, MathF.Abs(MathF.Round(num2)) * -1);
							}
							else
							{
								hero3.SetPersonalRelation(hero4, MathF.Abs(MathF.Round(num2)));
							}
						}
					}
					IL_03A3:;
				}
			}
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00104E04 File Offset: 0x00103004
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.LeaderHero != null)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (currentSettlement != null && currentSettlement.IsTown && mobileParty.CurrentSettlement.SiegeEvent == null)
				{
					if (mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Medicine.BestMedicine))
					{
						Hero randomElementWithPredicate = mobileParty.CurrentSettlement.Notables.GetRandomElementWithPredicate<Hero>((Hero x) => x.Age >= 40f && x.IsAlive);
						if (randomElementWithPredicate != null)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.LeaderHero, randomElementWithPredicate, (int)DefaultPerks.Medicine.BestMedicine.SecondaryBonus, true);
						}
					}
					if (mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Medicine.GoodLogdings))
					{
						Hero randomElement = TownHelpers.GetHeroesInSettlement(mobileParty.CurrentSettlement, (Hero x) => x.Age >= 40f && x != mobileParty.LeaderHero && x.IsLord).GetRandomElement<Hero>();
						if (randomElement != null)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.LeaderHero, randomElement, (int)DefaultPerks.Medicine.GoodLogdings.SecondaryBonus, true);
						}
					}
				}
				if (mobileParty.Army != null && MBRandom.RandomFloat < DefaultPerks.Charm.Parade.SecondaryBonus && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Charm.Parade))
				{
					Func<TroopRosterElement, bool> <>9__3;
					MobileParty randomElementWithPredicate2 = mobileParty.Army.Parties.GetRandomElementWithPredicate<MobileParty>(delegate(MobileParty x)
					{
						List<TroopRosterElement> troopRoster = x.MemberRoster.GetTroopRoster();
						Func<TroopRosterElement, bool> func;
						if ((func = <>9__3) == null)
						{
							func = (<>9__3 = (TroopRosterElement y) => y.Character.IsHero && y.Character.Occupation == Occupation.Lord && y.Character.HeroObject != mobileParty.LeaderHero);
						}
						return troopRoster.AnyQ<TroopRosterElement>(func);
					});
					if (randomElementWithPredicate2 != null)
					{
						CharacterObject character = randomElementWithPredicate2.MemberRoster.GetTroopRoster().GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement x) => x.Character.IsHero && x.Character.Occupation == Occupation.Lord && x.Character.HeroObject != mobileParty.LeaderHero).Character;
						Hero hero = ((character != null) ? character.HeroObject : null);
						if (hero != null)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.LeaderHero, hero, 1, true);
						}
					}
				}
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00104FCC File Offset: 0x001031CC
		private void DailyTick()
		{
			if (Settlement.CurrentSettlement != null && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.ForgivableGrievances) && MBRandom.RandomFloat < DefaultPerks.Charm.ForgivableGrievances.SecondaryBonus)
			{
				MBList<Hero> mblist = new MBList<Hero>();
				foreach (Hero hero in SettlementHelper.GetAllHeroesOfSettlement(Settlement.CurrentSettlement, true))
				{
					if (!hero.IsHumanPlayerCharacter && hero.GetRelationWithPlayer() < 0f)
					{
						mblist.Add(hero);
					}
				}
				if (mblist.Count > 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(mblist.GetRandomElement<Hero>(), 1, true, true);
				}
			}
			SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
			SettlementSecurityModel settlementSecurityModel = Campaign.Current.Models.SettlementSecurityModel;
			bool flag = false;
			bool flag2 = false;
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					if (settlement.Town.Security >= (float)settlementSecurityModel.ThresholdForNotableRelationBonus)
					{
						using (List<Hero>.Enumerator enumerator3 = settlement.Notables.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Hero hero2 = enumerator3.Current;
								if ((hero2.IsArtisan || hero2.IsMerchant) && MBRandom.RandomFloat < 0.05f)
								{
									ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero2, settlementSecurityModel.DailyNotableRelationBonus, false);
									flag2 = flag2 || settlement.OwnerClan.Leader.IsHumanPlayerCharacter;
								}
							}
							continue;
						}
					}
					if (settlement.Town.Security >= (float)settlementSecurityModel.ThresholdForNotableRelationPenalty)
					{
						continue;
					}
					foreach (Hero hero3 in settlement.Notables)
					{
						if ((hero3.IsArtisan || hero3.IsMerchant) && MBRandom.RandomFloat < 0.05f)
						{
							hero3.AddPower((float)settlementSecurityModel.DailyNotablePowerPenalty);
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero3, settlementSecurityModel.DailyNotableRelationPenalty, false);
						}
					}
					using (List<Hero>.Enumerator enumerator3 = settlement.Notables.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Hero hero4 = enumerator3.Current;
							if (hero4.IsGangLeader && MBRandom.RandomFloat < 0.05f)
							{
								hero4.AddPower((float)settlementSecurityModel.DailyNotablePowerBonus);
							}
						}
						continue;
					}
				}
				if (settlement.IsVillage && settlement.Village.Bound.Town.Loyalty >= settlementLoyaltyModel.ThresholdForNotableRelationBonus)
				{
					foreach (Hero hero5 in settlement.Notables)
					{
						if ((hero5.IsHeadman || hero5.IsRuralNotable) && MBRandom.RandomFloat < 0.05f)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero5, settlementLoyaltyModel.DailyNotableRelationBonus, false);
							flag = flag || settlement.OwnerClan.Leader.IsHumanPlayerCharacter;
						}
					}
				}
			}
			if (flag2)
			{
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=ME5hmllb}Your relation with notables in some of your settlements increased due to high security", null).ToString()));
			}
			if (flag)
			{
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=0h5BrVdA}Your relation with notables in some of your settlements increased due to high loyalty", null).ToString()));
			}
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x001053E4 File Offset: 0x001035E4
		public void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if ((detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByBarter) && oldOwner != null && oldOwner.MapFaction != null && oldOwner.MapFaction.Leader != oldOwner && oldOwner.IsAlive && oldOwner.MapFaction.Leader != Hero.MainHero)
			{
				float value = settlement.GetValue(null, true);
				int num = (int)((1f + MathF.Max(1f, MathF.Sqrt(value / 100000f))) * ((newOwner.MapFaction != oldOwner.MapFaction) ? 1f : 0.5f));
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(oldOwner, oldOwner.MapFaction.Leader, -num, false);
				if (capturerHero != null && capturerHero.Clan != capturerHero.MapFaction.Leader.Clan)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(capturerHero, capturerHero.MapFaction.Leader, num / 2, false);
				}
				if (oldOwner.Clan != null && settlement != null)
				{
					ChangeClanInfluenceAction.Apply(oldOwner.Clan, (float)(settlement.IsTown ? (-50) : (-25)));
				}
			}
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x001054FC File Offset: 0x001036FC
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			MapEvent mapEvent = raidEvent.MapEvent;
			PartyBase leaderParty = mapEvent.AttackerSide.LeaderParty;
			Hero hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
			PartyBase leaderParty2 = mapEvent.DefenderSide.LeaderParty;
			if (leaderParty == null || leaderParty.MapFaction == mapEvent.MapEventSettlement.MapFaction)
			{
				return;
			}
			if (winnerSide == BattleSideEnum.Attacker && hero != null && leaderParty2 != null && leaderParty2.IsSettlement && leaderParty2.Settlement.IsVillage && leaderParty2.Settlement.OwnerClan != Clan.PlayerClan)
			{
				int num = -MathF.Ceiling(6f * raidEvent.RaidDamage);
				int num2 = -MathF.Ceiling(6f * raidEvent.RaidDamage * 0.5f);
				if (num < 0)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, leaderParty2.Settlement.OwnerClan.Leader, num, true);
				}
				if (num2 < 0)
				{
					foreach (Hero hero2 in leaderParty2.Settlement.Notables)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, hero2, num2, true);
					}
				}
			}
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00105634 File Offset: 0x00103834
		private static void OnHeroesMarried(Hero firstHero, Hero secondHero, bool showNotification)
		{
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(firstHero, secondHero, 30, false);
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00105640 File Offset: 0x00103840
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom)
			{
				int num = ((detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion) ? (-40) : (-20));
				Hero leader = clan.Leader;
				foreach (Clan clan2 in oldKingdom.Clans)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leader, clan2.Leader, num, true);
				}
			}
		}

		// Token: 0x04001282 RID: 4738
		private const int RelationPenaltyFactor = 6;

		// Token: 0x04001283 RID: 4739
		private const int RelationIncreaseBetweenHeroesAfterMarriage = 30;

		// Token: 0x04001284 RID: 4740
		private const int RelationChangeForLeavingWithRebellion = -40;

		// Token: 0x04001285 RID: 4741
		private const int RelationChangeForLeaveKingdom = -20;

		// Token: 0x04001286 RID: 4742
		private const int RaidDefenseRelationGainWithVillageNotable = 5;

		// Token: 0x04001287 RID: 4743
		private const float ChanceForRelationChange = 0.05f;
	}
}
