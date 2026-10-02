using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000114 RID: 276
	public class DefaultEncounterModel : EncounterModel
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0007238C File Offset: 0x0007058C
		public override float NeededMaximumLandDistanceForEncounteringMobileParty
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x00072393 File Offset: 0x00070593
		public override float NeededMaximumNavalDistanceForEncounteringMobileParty
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0007239A File Offset: 0x0007059A
		public override float MaximumAllowedLandDistanceForEncounteringMobilePartyInArmy
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x000723A1 File Offset: 0x000705A1
		public override float MaximumAllowedNavalDistanceForEncounteringMobilePartyInArmy
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x000723A8 File Offset: 0x000705A8
		public override float NeededMaximumDistanceForEncounteringTown
		{
			get
			{
				return 0.05f;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x000723AF File Offset: 0x000705AF
		public override float NeededMaximumDistanceForEncounteringBlockade
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060017DD RID: 6109 RVA: 0x000723B6 File Offset: 0x000705B6
		public override float NeededMaximumDistanceForEncounteringVillage
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x000723BD File Offset: 0x000705BD
		public override float GetEncounterJoiningRadius
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x000723C4 File Offset: 0x000705C4
		public override float PlayerParleyDistance
		{
			get
			{
				return MobileParty.MainParty.SeeingRange;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x000723D0 File Offset: 0x000705D0
		public override float GetSettlementBeingNearFieldBattleRadius
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x000723D7 File Offset: 0x000705D7
		public override int MinimumNumberOfMenForAttackingVillageViaScene
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000723DA File Offset: 0x000705DA
		public override bool IsEncounterExemptFromHostileActions(PartyBase side1, PartyBase side2)
		{
			return side1 == null || side2 == null || (side1.IsMobile && side1.MobileParty.AvoidHostileActions) || (side2.IsMobile && side2.MobileParty.AvoidHostileActions);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00072410 File Offset: 0x00070610
		public override Hero GetLeaderOfSiegeEvent(SiegeEvent siegeEvent, BattleSideEnum side)
		{
			IEnumerable<PartyBase> involvedPartiesForEventType = siegeEvent.GetSiegeEventSide(side).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege);
			if (involvedPartiesForEventType.Count<PartyBase>() == 1)
			{
				return involvedPartiesForEventType.ElementAt<PartyBase>(0).LeaderHero;
			}
			IFaction faction = ((side == BattleSideEnum.Attacker) ? siegeEvent.BesiegerCamp.MapFaction : siegeEvent.BesiegedSettlement.MapFaction);
			return this.GetLeaderOfEventInternal(involvedPartiesForEventType, faction);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00072468 File Offset: 0x00070668
		public override bool CanMainHeroDoParleyWithParty(PartyBase partyBase, out TextObject explanation)
		{
			bool flag = true;
			explanation = null;
			if (MapEvent.PlayerMapEvent == null && Settlement.CurrentSettlement == null && MobileParty.MainParty.IsActive && !Hero.MainHero.IsPrisoner && partyBase.MapFaction != null && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && partyBase.MapFaction.IsAtWarWith(Clan.PlayerClan.MapFaction))
			{
				if (partyBase.MapFaction.IsRebelClan)
				{
					explanation = new TextObject("{=6LG4BDZZ}You can't start parley with Rebels.", null);
					flag = false;
				}
				else
				{
					if (partyBase.IsMobile)
					{
						return false;
					}
					if (partyBase.IsSettlement && partyBase.Settlement.IsFortification && !partyBase.Settlement.IsUnderSiege && partyBase.Settlement.IsInspected)
					{
						Settlement settlement = partyBase.Settlement;
						float num;
						bool flag2 = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, settlement, MobileParty.MainParty.IsCurrentlyAtSea && settlement.HasPort, MobileParty.MainParty.NavigationCapability, out num) < Campaign.Current.Models.EncounterModel.PlayerParleyDistance;
						bool flag3;
						if (!Campaign.Current.Models.SettlementAccessModel.IsRequestMeetingOptionAvailable(settlement, out flag3, out explanation) || flag3)
						{
							flag = false;
						}
						else if (!flag2)
						{
							explanation = new TextObject("{=Y8JPgz1c}You are too far away from {SETTLEMENT} to start parley.", null);
							explanation.SetTextVariable("SETTLEMENT", partyBase.Settlement.Name);
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0007260C File Offset: 0x0007080C
		public override Hero GetLeaderOfMapEvent(MapEvent mapEvent, BattleSideEnum side)
		{
			IFaction faction = ((side == BattleSideEnum.Attacker) ? mapEvent.AttackerSide.LeaderParty.MapFaction : mapEvent.DefenderSide.LeaderParty.MapFaction);
			return this.GetLeaderOfEventInternal(mapEvent.GetMapEventSide(side).Parties.Select<MapEventParty, PartyBase>((MapEventParty x) => x.Party), faction);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00072677 File Offset: 0x00070877
		private bool IsArmyLeader(Hero hero)
		{
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			return ((partyBelongedTo != null) ? partyBelongedTo.Army : null) != null && hero.PartyBelongedTo.Army.LeaderParty == hero.PartyBelongedTo;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000726A7 File Offset: 0x000708A7
		private int GetLeadingScore(Hero hero)
		{
			if (!hero.IsKingdomLeader && !this.IsArmyLeader(hero))
			{
				return this.GetCharacterSergeantScore(hero);
			}
			return (int)hero.PartyBelongedTo.GetTotalLandStrengthWithFollowers(true);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x000726D0 File Offset: 0x000708D0
		private Hero GetLeaderOfEventInternal(IEnumerable<PartyBase> allPartiesThatBelongToASide, IFaction eventFaction)
		{
			Hero hero = null;
			int num = 0;
			foreach (PartyBase partyBase in allPartiesThatBelongToASide)
			{
				Hero leaderHero = partyBase.LeaderHero;
				if (leaderHero != null)
				{
					int leadingScore = this.GetLeadingScore(leaderHero);
					if (hero == null)
					{
						hero = leaderHero;
						num = leadingScore;
					}
					bool flag = leaderHero.MapFaction == eventFaction;
					bool isKingdomLeader = leaderHero.IsKingdomLeader;
					bool flag2 = this.IsArmyLeader(leaderHero);
					bool flag3 = hero.MapFaction == eventFaction;
					bool isKingdomLeader2 = hero.IsKingdomLeader;
					bool flag4 = this.IsArmyLeader(hero);
					if (!flag3 && flag)
					{
						hero = leaderHero;
						num = leadingScore;
					}
					else if (flag == flag3)
					{
						if (isKingdomLeader)
						{
							if (!isKingdomLeader2 || leadingScore > num)
							{
								hero = leaderHero;
								num = leadingScore;
							}
						}
						else if (flag2)
						{
							if ((!isKingdomLeader2 && !flag4) || (flag4 && !isKingdomLeader2 && leadingScore > num))
							{
								hero = leaderHero;
								num = leadingScore;
							}
						}
						else if (!isKingdomLeader2 && !flag4 && leadingScore > num)
						{
							hero = leaderHero;
							num = leadingScore;
						}
					}
				}
			}
			return hero;
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x000727D0 File Offset: 0x000709D0
		public override int GetCharacterSergeantScore(Hero hero)
		{
			int num = 0;
			Clan clan = hero.Clan;
			if (clan != null)
			{
				num += clan.Tier * ((hero == clan.Leader) ? 100 : 20);
				if (clan.Kingdom != null && clan.Kingdom.Leader == hero)
				{
					num += 2000;
				}
			}
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			if (partyBelongedTo != null)
			{
				if (partyBelongedTo.Army != null && partyBelongedTo.Army.LeaderParty == partyBelongedTo)
				{
					num += partyBelongedTo.Army.Parties.Count * 200;
				}
				num += partyBelongedTo.MemberRoster.TotalManCount - partyBelongedTo.MemberRoster.TotalWounded;
			}
			return num;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00072874 File Offset: 0x00070A74
		public override IEnumerable<PartyBase> GetDefenderPartiesOfSettlement(Settlement settlement, MapEvent.BattleTypes mapEventType)
		{
			if (settlement.IsFortification)
			{
				return settlement.Town.GetDefenderParties(mapEventType);
			}
			if (settlement.IsVillage)
			{
				return settlement.Village.GetDefenderParties(mapEventType);
			}
			if (settlement.IsHideout)
			{
				return settlement.Hideout.GetDefenderParties(mapEventType);
			}
			return null;
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x000728C4 File Offset: 0x00070AC4
		public override PartyBase GetNextDefenderPartyOfSettlement(Settlement settlement, ref int partyIndex, MapEvent.BattleTypes mapEventType)
		{
			if (settlement.IsFortification)
			{
				return settlement.Town.GetNextDefenderParty(ref partyIndex, mapEventType);
			}
			if (settlement.IsVillage)
			{
				return settlement.Village.GetNextDefenderParty(ref partyIndex, mapEventType);
			}
			if (settlement.IsHideout)
			{
				return settlement.Hideout.GetNextDefenderParty(ref partyIndex, mapEventType);
			}
			return null;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00072914 File Offset: 0x00070B14
		public override MapEventComponent CreateMapEventComponentForEncounter(PartyBase attackerParty, PartyBase defenderParty, MapEvent.BattleTypes battleType)
		{
			MapEventComponent mapEventComponent = null;
			switch (battleType)
			{
			case MapEvent.BattleTypes.FieldBattle:
				mapEventComponent = FieldBattleEventComponent.CreateFieldBattleEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.Raid:
				mapEventComponent = RaidEventComponent.CreateRaidEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.Siege:
				Campaign.Current.MapEventManager.StartSiegeMapEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.Hideout:
				mapEventComponent = HideoutEventComponent.CreateHideoutEvent(attackerParty, defenderParty, false);
				break;
			case MapEvent.BattleTypes.SallyOut:
				Campaign.Current.MapEventManager.StartSallyOutMapEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.SiegeOutside:
				Campaign.Current.MapEventManager.StartSiegeOutsideMapEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.BlockadeBattle:
				mapEventComponent = BlockadeBattleMapEvent.CreateBlockadeBattleMapEvent(attackerParty, defenderParty, false);
				break;
			case MapEvent.BattleTypes.BlockadeSallyOutBattle:
				mapEventComponent = BlockadeBattleMapEvent.CreateBlockadeBattleMapEvent(attackerParty, defenderParty, true);
				break;
			}
			return mapEventComponent;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x000729C8 File Offset: 0x00070BC8
		public override float GetSurrenderChance(MobileParty defenderParty, MobileParty attackerParty)
		{
			float num = defenderParty.Party.CalculateCurrentStrength();
			float num2 = attackerParty.Party.CalculateCurrentStrength();
			if (num.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return 1f;
			}
			if (num2.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return 0f;
			}
			if (num >= num2)
			{
				return 0f;
			}
			float num3 = 0f;
			float num4 = 0f;
			if (defenderParty.IsVillager)
			{
				num3 = 0.23f;
				num4 = -13f;
			}
			else if (defenderParty.IsCaravan)
			{
				num3 = 0.3f;
				num4 = -10f;
			}
			else if (defenderParty.IsBandit)
			{
				if (defenderParty.IsCurrentlyAtSea)
				{
					num3 = 0.2f;
				}
				else if (defenderParty.ActualClan.StringId == "deserters")
				{
					num3 = 0.005f;
				}
				else
				{
					num3 = 0.1f;
				}
				num4 = -15f;
			}
			else
			{
				Debug.FailedAssert("Unable to calculate threshold and exponentialScalingFactor!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultEncounterModel.cs", "GetSurrenderChance", 351);
			}
			float num5 = num / num2;
			float num6 = num4 * (num5 - num3);
			float num7 = 1f - 1f / (1f + (float)Math.Exp((double)num6));
			if (!MobileParty.MainParty.IsCurrentlyAtSea && Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num7 = MathF.Min(1f, num7 * (1f + DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return num7;
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00072B28 File Offset: 0x00070D28
		public override ExplainedNumber GetBribeChance(MobileParty defenderParty, MobileParty attackerParty)
		{
			float num = defenderParty.Party.CalculateCurrentStrength();
			float num2 = attackerParty.Party.CalculateCurrentStrength();
			if (num.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return new ExplainedNumber(1f, false, null);
			}
			if (num2.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return new ExplainedNumber(0f, false, null);
			}
			if (num >= num2)
			{
				return new ExplainedNumber(0f, false, null);
			}
			float num3 = 0f;
			float num4 = 0f;
			if (defenderParty.IsVillager)
			{
				num3 = 0.3f;
				num4 = -10f;
			}
			else if (defenderParty.IsCaravan)
			{
				num3 = 0.52f;
				num4 = -10f;
			}
			else if (defenderParty.IsBandit)
			{
				num3 = 0.2f;
				num4 = -15f;
			}
			else
			{
				Debug.FailedAssert("Unable to calculate threshold and exponentialScalingFactor!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultEncounterModel.cs", "GetBribeChance", 406);
			}
			float num5 = num / num2;
			float num6 = num4 * (num5 - num3);
			ExplainedNumber explainedNumber = new ExplainedNumber(1f - 1f / (1f + (float)Math.Exp((double)num6)), false, null);
			explainedNumber.LimitMax(1f, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.Scarface, Hero.MainHero.CharacterObject, true, ref explainedNumber, false);
			return explainedNumber;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00072C5C File Offset: 0x00070E5C
		public override float GetMapEventSideRunAwayChance(MapEventSide mapEventSide)
		{
			float num = 0f;
			if (mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.Siege && mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.SallyOut && mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.SiegeOutside && mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.Raid && mapEventSide != MobileParty.MainParty.MapEventSide)
			{
				num = this.GetRunAwayChanceInternal(mapEventSide);
			}
			return num;
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00072CC0 File Offset: 0x00070EC0
		private float GetRunAwayChanceInternal(MapEventSide mapEventSide)
		{
			MapEvent mapEvent = mapEventSide.MapEvent;
			float num = 0f;
			if (mapEvent.UpdateCount >= 8 && mapEventSide.LeaderParty.IsMobile && mapEventSide.GetSideMorale() <= 20f)
			{
				for (int i = 0; i < 4; i++)
				{
					BattleSideEnum battleSideEnum = mapEvent.WonRounds[mapEvent.WonRounds.Count - 1 - i];
					if (battleSideEnum == mapEventSide.MissionSide || battleSideEnum == BattleSideEnum.None)
					{
						return 0f;
					}
				}
				num = 0.2f;
				Hero leaderHero = mapEventSide.LeaderParty.LeaderHero;
				int num2 = ((leaderHero != null) ? leaderHero.GetTraitLevel(DefaultTraits.Valor) : 0);
				num -= (float)num2 * 0.05f;
			}
			return num;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00072D6C File Offset: 0x00070F6C
		public override void FindNonAttachedNpcPartiesWhoWillJoinPlayerEncounter(List<MobileParty> partiesToJoinPlayerSide, List<MobileParty> partiesToJoinEnemySide)
		{
			CampaignVec2 campaignVec = MobileParty.MainParty.Position;
			float num = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				num = Campaign.Current.Models.MobilePartyAIModel.SettlementDefendingWaitingPositionRadius * 1.25f;
			}
			if (PlayerEncounter.Battle != null)
			{
				campaignVec = PlayerEncounter.Battle.Position;
				if (PlayerEncounter.Battle.IsSallyOut)
				{
					campaignVec = ((PlayerSiege.PlayerSiegeEvent != null) ? PlayerSiege.PlayerSiegeEvent : PlayerEncounter.EncounterSettlement.SiegeEvent).BesiegerCamp.LeaderParty.Position;
				}
				else if (PlayerEncounter.Battle.IsBlockade || PlayerEncounter.Battle.IsBlockadeSallyOut)
				{
					Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
					campaignVec = ((besiegedSettlement != null) ? besiegedSettlement.PortPosition : PlayerEncounter.Battle.MapEventSettlement.PortPosition);
					num = Campaign.Current.Models.EncounterModel.NeededMaximumDistanceForEncounteringBlockade * 3f;
				}
			}
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(campaignVec.ToVec2(), num);
			MobileParty nearbyParty = MobileParty.FindNextLocatable(ref locatableSearchData);
			List<MobileParty> list = new List<MobileParty>();
			List<MobileParty> list2 = new List<MobileParty>();
			Func<MobileParty, bool> <>9__4;
			Func<MobileParty, bool> <>9__5;
			while (nearbyParty != null)
			{
				bool flag = (PlayerEncounter.Battle != null && (PlayerEncounter.Battle.IsBlockade || PlayerEncounter.Battle.IsBlockadeSallyOut)) || MobileParty.MainParty.IsCurrentlyAtSea;
				if (nearbyParty != MobileParty.MainParty && nearbyParty.MapEvent == null && !nearbyParty.IsInRaftState && nearbyParty.SiegeEvent == null && nearbyParty.CurrentSettlement == null && nearbyParty.AttachedTo == null)
				{
					if (nearbyParty.IsCurrentlyAtSea != flag)
					{
						MapEvent battle = PlayerEncounter.Battle;
						bool flag2;
						if (battle == null)
						{
							flag2 = false;
						}
						else
						{
							Settlement mapEventSettlement = battle.MapEventSettlement;
							bool? flag3 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
							bool flag4 = true;
							flag2 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
						}
						if (!flag2)
						{
							goto IL_0388;
						}
					}
					if (nearbyParty.IsLordParty || nearbyParty.IsBandit || nearbyParty.IsPatrolParty || nearbyParty.ShouldJoinPlayerBattles)
					{
						if (PlayerEncounter.Battle != null)
						{
							bool flag5 = PlayerEncounter.Battle.CanPartyJoinBattle(nearbyParty.Party, PlayerEncounter.Battle.PlayerSide);
							bool flag6 = PlayerEncounter.Battle.CanPartyJoinBattle(nearbyParty.Party, PlayerEncounter.Battle.PlayerSide.GetOppositeSide());
							if (flag5)
							{
								list.Add(nearbyParty);
							}
							if (flag6)
							{
								list2.Add(nearbyParty);
							}
						}
						else
						{
							if (!nearbyParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction) && nearbyParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredParty.MapFaction))
							{
								IEnumerable<MobileParty> enumerable = list2;
								Func<MobileParty, bool> func;
								if ((func = <>9__4) == null)
								{
									func = (<>9__4 = (MobileParty x) => x.MapFaction.IsAtWarWith(nearbyParty.MapFaction));
								}
								if (enumerable.All<MobileParty>(func))
								{
									list.Add(nearbyParty);
								}
							}
							if (nearbyParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction) && !nearbyParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredParty.MapFaction))
							{
								IEnumerable<MobileParty> enumerable2 = list;
								Func<MobileParty, bool> func2;
								if ((func2 = <>9__5) == null)
								{
									func2 = (<>9__5 = (MobileParty x) => x.MapFaction.IsAtWarWith(nearbyParty.MapFaction));
								}
								if (enumerable2.All<MobileParty>(func2))
								{
									list2.Add(nearbyParty);
								}
							}
						}
					}
				}
				IL_0388:
				nearbyParty = MobileParty.FindNextLocatable(ref locatableSearchData);
			}
			if (!list2.AnyQ<MobileParty>((MobileParty t) => t.ShouldBeIgnored))
			{
				if (!partiesToJoinEnemySide.AnyQ<MobileParty>((MobileParty t) => t.ShouldBeIgnored))
				{
					goto IL_040C;
				}
			}
			Debug.Print("Ally parties wont join player encounter since there is an ignored party in enemy side", 0, Debug.DebugColor.White, 17592186044416UL);
			list.Clear();
			IL_040C:
			if (!list.AnyQ<MobileParty>((MobileParty t) => t.ShouldBeIgnored))
			{
				if (!partiesToJoinPlayerSide.AnyQ<MobileParty>((MobileParty t) => t != MobileParty.MainParty && t.ShouldBeIgnored))
				{
					goto IL_0478;
				}
			}
			Debug.Print("Enemy parties wont join player encounter since there is an ignored party in ally side", 0, Debug.DebugColor.White, 17592186044416UL);
			list2.Clear();
			IL_0478:
			partiesToJoinPlayerSide.AddRange(list.Except<MobileParty>(partiesToJoinPlayerSide));
			partiesToJoinEnemySide.AddRange(list2.Except<MobileParty>(partiesToJoinEnemySide));
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00073210 File Offset: 0x00071410
		public override bool CanPlayerForceBanditsToJoin(out TextObject explanation)
		{
			bool perkValue = Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.PartnersInCrime);
			explanation = (perkValue ? null : new TextObject("{=MaetSSa1}You need '{PERK}' perk to make this party join you.", null));
			TextObject textObject = explanation;
			if (textObject != null)
			{
				textObject.SetTextVariable("PERK", DefaultPerks.Roguery.PartnersInCrime.Name);
			}
			return perkValue;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00073260 File Offset: 0x00071460
		public override bool IsPartyUnderPlayerCommand(PartyBase party)
		{
			if (party == PartyBase.MainParty)
			{
				return true;
			}
			if (party.Side != PartyBase.MainParty.Side)
			{
				return false;
			}
			bool flag = party.Owner == Hero.MainHero;
			IFaction mapFaction = party.MapFaction;
			bool flag2 = ((mapFaction != null) ? mapFaction.Leader : null) == Hero.MainHero;
			bool flag3 = party.MobileParty != null && party.MobileParty.DefaultBehavior == AiBehavior.EscortParty && party.MobileParty.TargetParty == MobileParty.MainParty;
			bool flag4 = party.MobileParty != null && party.MobileParty.Army != null && party.MobileParty.Army.LeaderParty == MobileParty.MainParty;
			Settlement mapEventSettlement = party.MapEvent.MapEventSettlement;
			bool flag5 = mapEventSettlement != null && mapEventSettlement.OwnerClan.Leader == Hero.MainHero;
			return flag || flag2 || flag3 || flag4 || flag5;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00073340 File Offset: 0x00071540
		public override MBReadOnlyList<MobileParty> GetPartiesToTeleportOnMapEventFinalize(MapEvent mapEvent)
		{
			MBReadOnlyList<MapEventParty> mbreadOnlyList;
			if (mapEvent.IsPlayerMapEvent)
			{
				mbreadOnlyList = mapEvent.GetMapEventSide(mapEvent.PlayerSide.GetOppositeSide()).Parties;
			}
			else
			{
				mbreadOnlyList = mapEvent.GetMapEventSide(mapEvent.DefeatedSide).Parties;
			}
			MBList<MobileParty> mblist = new MBList<MobileParty>();
			foreach (MapEventParty mapEventParty in mbreadOnlyList)
			{
				if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsActive && mapEventParty.Party.NumberOfHealthyMembers > 0 && !mapEventParty.Party.MobileParty.IsGarrison && (mapEventParty.Party.MobileParty.Army == null || mapEventParty.Party.MobileParty.Army.LeaderParty == mapEventParty.Party.MobileParty || mapEventParty.Party.MobileParty.AttachedTo == null))
				{
					mblist.Add(mapEventParty.Party.MobileParty);
				}
			}
			return mblist;
		}
	}
}
