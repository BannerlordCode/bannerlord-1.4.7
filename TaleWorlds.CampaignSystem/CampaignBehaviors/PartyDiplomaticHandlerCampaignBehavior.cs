using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000427 RID: 1063
	public class PartyDiplomaticHandlerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060043B1 RID: 17329 RVA: 0x00148D9C File Offset: 0x00146F9C
		public override void RegisterEvents()
		{
			CampaignEvents.OnMapEventContinuityNeedsUpdateEvent.AddNonSerializedListener(this, new Action<IFaction>(this.OnMapEventContinuityNeedsUpdate));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x00148E08 File Offset: 0x00147008
		private void OnSessionLaunched(CampaignGameStarter gameSystemInitializer)
		{
			gameSystemInitializer.AddGameMenu("hostile_action_end_by_peace", "{=1rbg3Hz2}The {FACTION_1} and {FACTION_2} are no longer enemies.", new OnInitDelegate(this.game_menu_hostile_action_end_by_peace_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("hostile_action_end_by_peace", "hostile_action_en_by_peace_end", "{=WVkc4UgX}Continue.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(PartyDiplomaticHandlerCampaignBehavior.player_encounter_finished_with_diplomatic_change_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("village_raid_diplomatically_ended", "{=CnNUOM9Q}The owner of this fief and your kingdom are no longer enemies.", new OnInitDelegate(this.game_menu_hostile_action_end_by_peace_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("village_raid_diplomatically_ended", "leave", "{=3sRdGQou}Leave", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty;
			}, new GameMenuOption.OnConsequenceDelegate(PartyDiplomaticHandlerCampaignBehavior.player_encounter_finished_with_diplomatic_change_consequence), true, -1, false, null);
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x00148EDB File Offset: 0x001470DB
		private static void player_encounter_finished_with_diplomatic_change_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x00148EF0 File Offset: 0x001470F0
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool arg5)
		{
			if (newKingdom != null)
			{
				CampaignEventDispatcher.Instance.OnMapEventContinuityNeedsUpdate(newKingdom);
				return;
			}
			CampaignEventDispatcher.Instance.OnMapEventContinuityNeedsUpdate(clan);
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x00148F0C File Offset: 0x0014710C
		private void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
			this.CheckMapEvents(faction);
			this.CheckSiegeEvents(faction);
			this.CheckFactionPartiesAndSettlements(faction);
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x00148F24 File Offset: 0x00147124
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (detail != ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege && settlement.SiegeEvent != null)
			{
				this.CheckSiegeEventContinuity(settlement.SiegeEvent);
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.EncounterSettlementAux != null)
			{
				foreach (Village village in settlement.BoundVillages)
				{
					if (village.Settlement == PlayerEncounter.Current.EncounterSettlementAux && village.Settlement.IsUnderRaid && !Clan.PlayerClan.IsAtWarWith(newOwner.MapFaction))
					{
						this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent = oldOwner.MapFaction;
						GameMenu.ActivateGameMenu("village_raid_diplomatically_ended");
						break;
					}
				}
			}
			this.CheckSettlementSuitabilityForParties(settlement.Parties);
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x00148FF8 File Offset: 0x001471F8
		private void CheckFactionPartiesAndSettlements(IFaction faction)
		{
			this.CheckSettlementSuitabilityForParties(faction.WarPartyComponents.Select<WarPartyComponent, MobileParty>((WarPartyComponent x) => x.MobileParty));
			foreach (Settlement settlement in faction.Settlements)
			{
				this.CheckSettlementSuitabilityForParties(settlement.Parties);
			}
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x00149080 File Offset: 0x00147280
		private void CheckMapEvents(IFaction faction)
		{
			MapEventManager mapEventManager = Campaign.Current.MapEventManager;
			List<MapEvent> list = ((mapEventManager != null) ? mapEventManager.MapEvents : null);
			List<MapEvent> list2 = new List<MapEvent>();
			Func<PartyBase, bool> <>9__0;
			foreach (MapEvent mapEvent in list)
			{
				if (!mapEvent.IsFinalized)
				{
					IEnumerable<PartyBase> involvedParties = mapEvent.InvolvedParties;
					Func<PartyBase, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (PartyBase x) => x.MapFaction == faction);
					}
					if (involvedParties.Any<PartyBase>(func))
					{
						list2.Add(mapEvent);
					}
				}
			}
			foreach (MapEvent mapEvent2 in list2)
			{
				List<MapEventParty> list3 = mapEvent2.AttackerSide.Parties.ToList<MapEventParty>();
				MapEventState state = mapEvent2.State;
				bool flag = false;
				foreach (MapEventParty mapEventParty in list3)
				{
					if (!mapEvent2.CanPartyJoinBattle(mapEventParty.Party, BattleSideEnum.Attacker))
					{
						flag = flag || mapEvent2.IsPlayerMapEvent;
						if (mapEventParty.Party != PartyBase.MainParty)
						{
							mapEventParty.Party.MapEventSide = null;
						}
					}
				}
				if (state != MapEventState.WaitingRemoval && mapEvent2.State == MapEventState.WaitingRemoval)
				{
					mapEvent2.DiplomaticallyFinished = true;
				}
				if (flag)
				{
					if (PlayerEncounter.Current != null)
					{
						PlayerEncounter.Finish(true);
					}
					else
					{
						GameMenu.ExitToLast();
					}
				}
			}
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x00149228 File Offset: 0x00147428
		private void CheckSiegeEvents(IFaction faction)
		{
			SiegeEventManager siegeEventManager = Campaign.Current.SiegeEventManager;
			List<SiegeEvent> list = ((siegeEventManager != null) ? siegeEventManager.SiegeEvents : null);
			List<SiegeEvent> list2 = new List<SiegeEvent>();
			Func<PartyBase, bool> <>9__0;
			foreach (SiegeEvent siegeEvent in list)
			{
				if (!siegeEvent.ReadyToBeRemoved)
				{
					IEnumerable<PartyBase> involvedPartiesForEventType = siegeEvent.GetInvolvedPartiesForEventType(siegeEvent.GetCurrentBattleType());
					Func<PartyBase, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (PartyBase x) => x.MapFaction == faction);
					}
					if (involvedPartiesForEventType.Any<PartyBase>(func))
					{
						list2.Add(siegeEvent);
					}
				}
			}
			foreach (SiegeEvent siegeEvent2 in list2)
			{
				this.CheckSiegeEventContinuity(siegeEvent2);
			}
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0014931C File Offset: 0x0014751C
		private void CheckSiegeEventContinuity(SiegeEvent siegeEvent)
		{
			bool flag = siegeEvent == PlayerSiege.PlayerSiegeEvent;
			List<PartyBase> list = siegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(siegeEvent.GetCurrentBattleType()).ToList<PartyBase>();
			bool flag2 = false;
			for (int i = 0; i < list.Count; i++)
			{
				PartyBase partyBase = list[i];
				if (!siegeEvent.CanPartyJoinSide(partyBase, BattleSideEnum.Attacker))
				{
					if (flag && !flag2 && partyBase == PartyBase.MainParty)
					{
						flag2 = true;
						this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent = siegeEvent.BesiegedSettlement.MapFaction;
					}
					partyBase.MobileParty.BesiegerCamp = null;
				}
			}
			if (!siegeEvent.ReadyToBeRemoved)
			{
				if (siegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(siegeEvent.GetCurrentBattleType()).Any<PartyBase>((PartyBase x) => x != PartyBase.MainParty))
				{
					List<MobileParty> list2 = siegeEvent.BesiegedSettlement.Parties.ToList<MobileParty>();
					for (int j = 0; j < list2.Count; j++)
					{
						PartyBase party = list2[j].Party;
						if (siegeEvent.BesiegedSettlement.MapFaction != party.MapFaction && !siegeEvent.CanPartyJoinSide(party, BattleSideEnum.Defender))
						{
							if (flag && !flag2 && party == PartyBase.MainParty)
							{
								flag2 = true;
								this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent = siegeEvent.BesiegerCamp.LeaderParty.MapFaction;
							}
							else if (party.MobileParty.Army == null || party.MobileParty.Army.LeaderParty == party.MobileParty)
							{
								LeaveSettlementAction.ApplyForParty(party.MobileParty);
							}
						}
					}
				}
			}
			if (flag2)
			{
				GameMenu.ActivateGameMenu("hostile_action_end_by_peace");
			}
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x001494AC File Offset: 0x001476AC
		private void CheckSettlementSuitabilityForParties(IEnumerable<MobileParty> parties)
		{
			foreach (MobileParty mobileParty in parties.ToList<MobileParty>())
			{
				if (mobileParty.CurrentSettlement != null && mobileParty.MapFaction.IsAtWarWith(mobileParty.CurrentSettlement.MapFaction))
				{
					if (mobileParty != MobileParty.MainParty)
					{
						if (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty)
						{
							if (mobileParty.Army != null && mobileParty.Army.Parties.Contains(MobileParty.MainParty))
							{
								GameMenu.SwitchToMenu("army_left_settlement_due_to_war_declaration");
							}
							else
							{
								if (mobileParty.IsTransitionInProgress)
								{
									mobileParty.CancelNavigationTransition();
								}
								LeaveSettlementAction.ApplyForParty(mobileParty);
							}
						}
					}
					else if (mobileParty.CurrentSettlement.IsFortification)
					{
						GameMenu.SwitchToMenu("fortification_crime_rating");
					}
				}
			}
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x00149598 File Offset: 0x00147798
		[GameMenuInitializationHandler("hostile_action_end_by_peace")]
		public static void hostile_action_end_by_peace_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.BesiegedSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(MobileParty.MainParty.BesiegedSettlement.SettlementComponent.WaitMeshName);
				return;
			}
			if (MobileParty.MainParty.LastVisitedSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(MobileParty.MainParty.LastVisitedSettlement.SettlementComponent.WaitMeshName);
				return;
			}
			if (PlayerEncounter.EncounterSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounterSettlement.SettlementComponent.WaitMeshName);
				return;
			}
			Debug.FailedAssert("No menu background to initialize!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PartyDiplomaticHandlerCampaignBehavior.cs", "hostile_action_end_by_peace_on_init", 297);
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x00149638 File Offset: 0x00147838
		private void game_menu_hostile_action_end_by_peace_on_init(MenuCallbackArgs args)
		{
			if (this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent == null)
			{
				IEnumerable<IFaction> enumerable = Kingdom.All.Union<IFaction>(Clan.All);
				IFaction mapFaction = MobileParty.MainParty.MapFaction;
				StanceLink stanceLink = (from x in enumerable
					where x != mapFaction
					select x.GetStanceWith(mapFaction) into x
					where !x.IsAtWar
					orderby x.PeaceDeclarationDate.ElapsedHoursUntilNow
					select x).FirstOrDefault<StanceLink>();
				this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent = ((stanceLink.Faction1 != Clan.PlayerClan.MapFaction) ? stanceLink.Faction1 : stanceLink.Faction2);
			}
			GameTexts.SetVariable("FACTION_1", Clan.PlayerClan.MapFaction.EncyclopediaLinkWithName);
			GameTexts.SetVariable("FACTION_2", this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent.EncyclopediaLinkWithName);
			if (PlayerEncounter.Battle != null)
			{
				PlayerEncounter.Battle.DiplomaticallyFinished = true;
			}
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0014974B File Offset: 0x0014794B
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<IFaction>("_lastFactionMadePeaceWithCausedPlayerToLeaveEvent", ref this._lastFactionMadePeaceWithCausedPlayerToLeaveEvent);
		}

		// Token: 0x0400134F RID: 4943
		private IFaction _lastFactionMadePeaceWithCausedPlayerToLeaveEvent;
	}
}
