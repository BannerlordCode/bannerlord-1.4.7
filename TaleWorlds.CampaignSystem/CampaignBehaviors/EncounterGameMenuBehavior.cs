using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003EE RID: 1006
	public class EncounterGameMenuBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E65 RID: 15973 RVA: 0x00116E24 File Offset: 0x00115024
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<TroopRoster>("_breakInOutCasualties", ref this._breakInOutCasualties);
			dataStore.SyncData<int>("_breakInOutArmyCasualties", ref this._breakInOutArmyCasualties);
			dataStore.SyncData<bool>("_playerIsAlreadyInCastle", ref this._playerIsAlreadyInCastle);
			dataStore.SyncData<bool>("_isBreakingOutFromPort", ref this._isBreakingOutFromPort);
			dataStore.SyncData<List<Settlement>>("_alreadySneakedSettlements", ref this._alreadySneakedSettlements);
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00116E8C File Offset: 0x0011508C
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00116EF5 File Offset: 0x001150F5
		public void AddCurrentSettlementAsAlreadySneakedIn()
		{
			this._alreadySneakedSettlements.Add(Settlement.CurrentSettlement);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00116F08 File Offset: 0x00115108
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			IFaction mapFaction = siegeEvent.BesiegerCamp.MapFaction;
			if (siegeEvent.IsPlayerSiegeEvent && mapFaction != null && mapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				mapFaction.NotAttackableByPlayerUntilTime = CampaignTime.Zero;
			}
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00116F48 File Offset: 0x00115148
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (mapEvent.IsPlayerMapEvent && attackerParty.MapFaction != null && attackerParty.MapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				attackerParty.MapFaction.NotAttackableByPlayerUntilTime = CampaignTime.Zero;
			}
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00116F8A File Offset: 0x0011518A
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party == MobileParty.MainParty)
			{
				this._playerIsAlreadyInCastle = false;
			}
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x00116F9B File Offset: 0x0011519B
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeAccessDetails();
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00116FAC File Offset: 0x001151AC
		private void InitializeAccessDetails()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement != null && (currentSettlement.IsFortification || currentSettlement.IsVillage))
			{
				Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(Settlement.CurrentSettlement, out this._accessDetails);
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00116FF4 File Offset: 0x001151F4
		private void AddGameMenus(CampaignGameStarter gameSystemInitializer)
		{
			gameSystemInitializer.AddGameMenu("taken_prisoner", "{=ezClQMBj}Your enemies take you as a prisoner.", new OnInitDelegate(this.game_menu_taken_prisoner_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("taken_prisoner", "taken_prisoner_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_taken_prisoner_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_taken_prisoner_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("defeated_and_taken_prisoner", "{=ezClQMBj}Your enemies take you as a prisoner.", new OnInitDelegate(this.game_menu_taken_prisoner_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("defeated_and_taken_prisoner", "taken_prisoner_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_taken_prisoner_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_taken_prisoner_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_meeting", "{=!}.", new OnInitDelegate(this.game_menu_encounter_meeting_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("join_encounter", "{=jKWJpIES}{JOIN_ENCOUNTER_TEXT}. You decide to...", new OnInitDelegate(this.game_menu_join_encounter_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_help_attackers", "{=h3yEHb4U}Help {ATTACKER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_attackers_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_attackers_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_help_defenders", "{=FwIgakj8}Help {DEFENDER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_defenders_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_defenders_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_abandon", "{=Nr49hlfC}Abandon army.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_abandon_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_abandon_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_leave", "{=!}{LEAVE_TEXT}", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_leave_no_army_on_condition), delegate(MenuCallbackArgs args)
			{
				if (MobileParty.MainParty.SiegeEvent != null && MobileParty.MainParty.SiegeEvent.BesiegerCamp != null && MobileParty.MainParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(PartyBase.MainParty, MapEvent.BattleTypes.Siege))
				{
					MobileParty.MainParty.BesiegerCamp = null;
				}
				PlayerEncounter.Finish(true);
				MobileParty.MainParty.SetMoveModeHold();
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("join_sally_out", "{=CcNVobQU}Garrison of the settlement you are in decided to sally out. You decide to...", new OnInitDelegate(this.game_menu_join_sally_out_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("join_sally_out", "join_siege_event", "{=fyNNCOFK}Join the sally out", new GameMenuOption.OnConditionDelegate(this.game_menu_join_sally_out_event_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_sally_out_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_sally_out", "join_siege_event_break_in", "{=z1RHDsOG}Stay in settlement", new GameMenuOption.OnConditionDelegate(this.game_menu_stay_in_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_stay_in_settlement_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("naval_town_outside", "{=!}{PORT_OUTSIDE_TEXT}", new OnInitDelegate(this.naval_town_outside_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("naval_town_outside", "attack_the_blockade", "{=90OXjYk8}Attack the blockade to help the defenders", new GameMenuOption.OnConditionDelegate(this.attack_blockade_besieger_side_on_condition), new GameMenuOption.OnConsequenceDelegate(this.attack_blockade_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("naval_town_outside", "join_siege_defender", "{=X8KWb3PK}Break in through the blockade", new GameMenuOption.OnConditionDelegate(this.attack_blockade_besieger_side_break_in_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_siege_event_on_defender_side_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("naval_town_outside", "join_encounter_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_naval_outside_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("player_blockade_got_attacked", "{=4T34aAMv}Your blockade is under attack!", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("player_blockade_got_attacked", "defend_the_blockade", "{=zRyM1hYm}Defend the blockade.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.SetSail;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(this.defend_blockade_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("player_blockade_got_attacked", "lift_the_blockade", "{=tixbTdlH}Lift the blockade.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(this.lift_players_blockade), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("besiegers_lift_the_blockade", "{=tcmSIJKj}The besiegers lifted the blockade.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("besiegers_lift_the_blockade", "continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_debrief_continue_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_break_out_from_gate", "{=dFcgXnQq}Break out from gate", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_defender_siege_break_out_from_gate_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_defender_siege_break_out_from_gate_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_break_out_from_port", "{=g2b93XVr}Break out from port", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_defender_siege_break_out_from_port_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_defender_siege_break_out_from_port_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_sally_out_from_gate", "{=!}{SALLY_OUT_BUTTON_TEXT}", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_sally_out_from_gate_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_sally_out_land_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_sally_out_from_port", "{=!}{SALLY_OUT_BUTTON_TEXT}", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_sally_out_from_port_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_sally_out_naval_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("join_siege_event", "{=xNyKVMHx}{JOIN_SIEGE_TEXT} You decide to...", new OnInitDelegate(this.game_menu_join_siege_event_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "join_siege_event", "{=ZVsJf5Ff}Join the continuing siege.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_siege_event_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_siege_event_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "attack_besiegers", "{=CVg3P07C}Assault the siege camp.", new GameMenuOption.OnConditionDelegate(this.attack_besieger_side_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_defenders_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "join_siege_event_break_in", "{=XAvwP3Ce}Break in to help the defenders", new GameMenuOption.OnConditionDelegate(this.break_in_to_help_defender_side_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_siege_event_on_defender_side_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "join_encounter_leave", "{=ebUwP3Q3}Don't get involved.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_leave_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("siege_attacker_left", "{=LR6Y57Rq}Attackers abandoned the siege.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_left", "siege_attacker_left_return_to_settlement", "{=j7bZRFxc}Return to {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_left", "siege_attacker_left_leave", "{=mfAP8Wlq}Leave settlement.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_left_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Finish(true);
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("siege_attacker_defeated", "{=njbpMLdJ}Attackers have been defeated.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_defeated", "siege_attacker_defeated_return_to_settlement", "{=j7bZRFxc}Return to {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_defeated", "siege_attacker_defeated_leave", "{=mfAP8Wlq}Leave settlement.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_defeated_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Finish(true);
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter", "{=!}{ENCOUNTER_TEXT}", new OnInitDelegate(this.game_menu_encounter_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "continue_preparations", "{=FOoMM4AU}Continue siege preparations.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_besiege_continue_siege_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_besiege_continue_siege_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "village_raid_action", "{=lvttCRi8}Plunder the village, then raze it.", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_raid_no_resist_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "village_force_volunteer_action", "{=9YHjPkb8}Force notables to give you recruits.", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_force_volunteers_no_resist_loot_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "village_force_supplies_action", "{=JMzyh6Gl}Force people to give you supplies.", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_force_supplies_no_resist_loot_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "attack", "{=o1pZHZOF}{ATTACK_TEXT}!", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "capture_the_enemy", "{=27yneDGL}Capture the enemy.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_capture_the_enemy_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_capture_the_enemy_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "str_order_attack", "{=!}{SEND_TROOPS_TEXT}", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_order_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_order_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "leave_soldiers_behind", "{=qNgGoqmI}Try to get away.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_leave_your_soldiers_behind_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("try_to_get_away");
			}, false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "surrender", "{=3nT5wWzb}Surrender.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_surrender_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "abandon_army", "{=Nr49hlfC}Abandon army.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_abandon_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_abandon_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "go_back_to_settlement", "{=j7bZRFxc}Return to {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_sally_out_go_back_to_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_sally_out_go_back_to_settlement_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("army_encounter", "{=!}{ARMY_ENCOUNTER_TEXT}", new OnInitDelegate(this.game_menu_army_encounter_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_talk_to_leader", "{=tYVW8iQN}Talk to army leader", new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_leader_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_leader_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_talk_to_other_members", "{=b7APCGY2}Talk to other members", new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_other_members_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_other_members_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_join_army", "{=N4Qa0WsT}Join army", new GameMenuOption.OnConditionDelegate(this.game_menu_army_join_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_join_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_attack_army", "{=0URijoc0}Attack army", new GameMenuOption.OnConditionDelegate(this.game_menu_army_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_army_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.army_encounter_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("game_menu_army_talk_to_other_members", "{=yYTotiqW}Talk to...", new OnInitDelegate(this.game_menu_army_talk_to_other_members_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("game_menu_army_talk_to_other_members", "game_menu_army_talk_to_other_members_item", "{=!}{CHAR_NAME}", new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_other_members_item_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_other_members_item_on_consequence), false, -1, true, null);
			gameSystemInitializer.AddGameMenuOption("game_menu_army_talk_to_other_members", "game_menu_army_talk_to_other_members_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_other_members_back_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_other_members_back_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("try_to_get_away", "{=!}{TRY_TO_GET_AWAY_TEXT}", new OnInitDelegate(this.game_menu_leave_soldiers_behind_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("try_to_get_away", "try_to_get_away_accept", "{=DbOv36TA}Go ahead with that.", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_accept_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_leave_your_soldiers_behind_accept_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("try_to_get_away", "try_to_get_away_reject", "{=f1etg9oL}Think of something else.", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_reject_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("encounter");
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("try_to_get_away_debrief", "{=!}{TRY_TAKE_AWAY_FINISHED}", new OnInitDelegate(this.try_to_get_away_debrief_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("try_to_get_away_debrief", "try_to_get_away_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_try_to_get_away_end), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("assault_town", "", new OnInitDelegate(this.game_menu_town_assault_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("assault_town_order_attack", "", new OnInitDelegate(this.game_menu_town_assault_order_attack_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("town_outside", "{=!}{TOWN_TEXT}", new OnInitDelegate(this.game_menu_town_outside_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "approach_gates", "{=XlbDnuJx}Approach the gates and hail the guard.", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_approach_gates_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_outside_approach_gates_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_disguise_yourself", "{=VCREeAF1}Disguise yourself and sneak through the gate.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_disguise_yourself_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_initial_disguise_yourself_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_besiege", "{=WdIGdHuL}Besiege the town.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_town_besiege_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_town_besiege_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_enter_cheat", "{=!}Enter town (Cheat).", new GameMenuOption.OnConditionDelegate(this.game_menu_town_outside_cheat_enter_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_outside_enter_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_outside_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_outside_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("disguise_blocked_night_time", "{=KZ27sSXS}With increased security at night guards check the identity of every entry. You can't sneak in during the night.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("disguise_blocked_night_time", "back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_outside");
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("disguise_first_time", "{=6q7UsTtn}You have no contact in this town, you need to set one up.", new OnInitDelegate(this.first_time_disguise_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("disguise_first_time", "continue", "{=WjwHVQzx}Set up contact", new GameMenuOption.OnConditionDelegate(this.launch_mission_on_condition), new GameMenuOption.OnConsequenceDelegate(this.launch_disguise_mission), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("disguise_first_time", "back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_outside");
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("settlement_player_unconscious_when_disguise_contact_set", "{=S5OEsjwg}You slip into unconsciousness. After a little while some of the friendlier locals manage to bring you around. A little confused but without any serious injuries, you resolve to be more careful next time.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("settlement_player_unconscious_when_disguise_contact_set", "continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.continue_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("disguise_not_first_time");
			}, false, -1, false, null);
			gameSystemInitializer.AddGameMenu("settlement_player_unconscious_when_disguise_contact_not_set", "{=KqrkAOY9}You slip into unconsciousness guards find you and throw you in jail.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("settlement_player_unconscious_when_disguise_contact_not_set", "continue", "{=3nT5wWzb}Surrender", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.mno_sneak_caught_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_menu_captivity_castle_taken_prisoner_cont_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("disguise_not_first_time", "{=jqb0q3Gp}You have a contact in this town, you can go about your business disguised.", new OnInitDelegate(this.disguise_not_first_time_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("disguise_not_first_time", "quick_sneak", "{=hPmawJUs}Sneak in as quickly as you can ({SNEAK_CHANCE}%)", new GameMenuOption.OnConditionDelegate(this.game_menu_town_disguise_yourself_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_disguise_yourself_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("disguise_not_first_time", "take_a_walk", "{=iHLBzWSI}Take a walk around the town disguised", new GameMenuOption.OnConditionDelegate(this.launch_mission_on_condition), new GameMenuOption.OnConsequenceDelegate(this.launch_disguise_mission), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("disguise_not_first_time", "back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_outside");
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("settlement_player_run_away_when_disguise", "{=WJyTrMf4}You manage to escape the town before getting caught somehow.", new OnInitDelegate(this.disguise_not_first_time_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("settlement_player_run_away_when_disguise", "continue_back", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_sneak_into_town_succeeded_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.escape_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_sneak_into_town_succeeded", "{=pSSDfAjR}Disguised in the garments of a poor pilgrim, you fool the guards and make your way into the town.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_sneak_into_town_succeeded", "str_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_sneak_into_town_succeeded_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.menu_sneak_into_town_succeeded_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_sneak_into_town_caught", "{=u7yLV7Vr}As you try to sneak in, one of the guards recognizes you and raises the alarm! Another quickly slams the gate shut behind you, and you have no choice but to give up.", new OnInitDelegate(EncounterGameMenuBehavior.game_menu_sneak_into_town_caught_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_sneak_into_town_caught", "mno_sneak_caught_surrender", "{=3nT5wWzb}Surrender.", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.mno_sneak_caught_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.mno_sneak_caught_surrender_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_captivity_castle_taken_prisoner", "{=AFJ3BvTH}You are quickly surrounded by guards who take away your weapons. With curses and insults, they throw you into the dungeon where you must while away the miserable days of your captivity.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_captivity_castle_taken_prisoner", "mno_sneak_caught_surrender", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_menu_captivity_castle_taken_prisoner_cont_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_menu_captivity_castle_taken_prisoner_cont_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_captivity_castle_taken_prisoner", "cheat_continue", "{=!}Cheat : Leave.", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_menu_captivity_taken_prisoner_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_menu_captivity_taken_prisoner_cheat_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("fortification_crime_rating", "{=!}{FORTIFICATION_CRIME_RATING_TEXT}", new OnInitDelegate(this.game_menu_fortification_high_crime_rating_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("fortification_crime_rating", "fortification_crime_rating_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_fortification_high_crime_rating_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_fortification_high_crime_rating_continue_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("army_left_settlement_due_to_war_declaration", "{=!}{ARMY_LEFT_SETTLEMENT_DUE_TO_WAR_TEXT}", new OnInitDelegate(this.game_menu_army_left_settlement_due_to_war_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("army_left_settlement_due_to_war_declaration", "army_left_settlement_due_to_war_declaration_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_army_left_settlement_due_to_war_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_left_settlement_due_to_war_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("castle_outside", "{=!}{TOWN_TEXT}", new OnInitDelegate(this.game_menu_castle_outside_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("castle_outside", "approach_gates", "{=XlbDnuJx}Approach the gates and hail the guard.", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_approach_gates_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_outside_approach_gates_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_outside", "town_besiege", "{=UzMYZgoE}Besiege the castle.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_town_besiege_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_town_besiege_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_outside", "town_outside_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_outside_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("town_guard", "{=SxkaQbSa}You approach the gate. The men on the walls watch you closely.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("town_guard", "request_meeting_commander", "{=RSQbOjub}Request a meeting with someone.", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_someone_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_someone_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_guard", "guard_discuss_criminal_surrender", "{=ACvQdkG8}Discuss the terms of your surrender", new GameMenuOption.OnConditionDelegate(this.outside_menu_criminal_on_condition), new GameMenuOption.OnConsequenceDelegate(this.outside_menu_criminal_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_guard", "guard_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_guard_back_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("castle_guard", "{=SxkaQbSa}You approach the gate. The men on the walls watch you closely.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("castle_guard", "request_shelter", "{=mG9jW8Fp}Request entry to the castle.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_guard_request_shelter_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_entry_to_castle_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_guard", "request_meeting_commander", "{=RSQbOjub}Request a meeting with someone.", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_someone_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_someone_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_guard", "guard_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_guard_back_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("castle_enter_bribe", "{=yyz111nn}The guards say that they can't just let anyone in.", null, GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("castle_enter_bribe", "castle_bribe_pay", "{=3lxq5fvI}Pay a {AMOUNT}{GOLD_ICON} bribe to enter the castle.", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_menu_castle_enter_bribe_pay_bribe_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_enter_bribe_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_enter_bribe", "castle_bribe_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("castle_guard");
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_castle_entry_granted", "{=Mg1PotzO}After a brief wait, the guards open the gates for you and allow your party inside.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_castle_entry_granted", "str_continue", "{=bLNocKd1}Continue..", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_request_entry_to_castle_approved_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_request_entry_to_castle_approved_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_castle_entry_denied", "{=QpQQJjD6}The lord of this castle has forbidden you from coming inside these walls, and the guard sergeant informs you that his men will fire if you attempt to come any closer.", new OnInitDelegate(EncounterGameMenuBehavior.menu_castle_entry_denied_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_castle_entry_denied", "str_continue", "{=veWOovVv}Continue...", null, new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_request_entry_to_castle_rejected_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("request_meeting", "{=pBAx7jTM}With whom do you want to meet?", new OnInitDelegate(this.game_menu_town_menu_request_meeting_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting", "request_meeting_with", "{=!}{HERO_TO_MEET.LINK}", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_with_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_with_on_consequence), false, -1, true, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting", "meeting_town_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_town_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting", "meeting_castle_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_castle_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_castle_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("request_meeting_with_besiegers", "{=pBAx7jTM}With whom do you want to meet?", new OnInitDelegate(this.game_menu_town_menu_request_meeting_with_besiegers_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting_with_besiegers", "request_meeting_with", "{=!}{PARTY_LEADER.LINK}", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_with_besiegers_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_with_besiegers_on_consequence), false, -1, true, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting_with_besiegers", "request_meeting_town_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_town_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting_with_besiegers", "request_meeting_castle_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_castle_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_castle_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("village_outside", "{=!}.", new OnInitDelegate(this.VillageOutsideOnInit), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("village_loot_complete", "{=qt5bkw8l}On your orders your troops sack the village, pillaging everything of any value, and then put the buildings to the torch. From the coins and valuables that are found, you get your share.", new OnInitDelegate(this.game_menu_village_loot_complete_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("village_loot_complete", "continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_village_loot_complete_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_loot_complete_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("raid_interrupted", "{=KW7amS8c}While your troops are pillaging the countryside, you receive news that the enemy is approaching. You quickly gather up your soldiers and prepare for battle.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("raid_interrupted", "continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_raid_interrupted_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_raid_interrupted_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_interrupted", "{=lKWflUid}While you are waiting in {DEFENDER}, {ATTACKER} started an attack on it.", new OnInitDelegate(this.game_menu_encounter_interrupted_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted", "encounter_interrupted_help_attackers", "{=h3yEHb4U}Help {ATTACKER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_attackers_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_attackers_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted", "encounter_interrupted_help_defenders", "{=FwIgakj8}Help {DEFENDER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_defenders_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_defenders_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted", "leave", "{=UgfmaQgx}Leave {DEFENDER}", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_interrupted_siege_preparations", "{=ABeCWcLi}While you are resting, you hear news that a force led by {ATTACKER} has arrived outside the walls of {DEFENDER} and is beginning preparations for a siege.", new OnInitDelegate(this.game_menu_encounter_interrupted_siege_preparations_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_siege_preparations", "encounter_interrupted_siege_preparations_join_defend", "{=Lxx97yNh}Join the defense of {SETTLEMENT}", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_siege_preparations_join_defend_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_siege_preparations_join_defend_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_siege_preparations", "encounter_interrupted_siege_preparations_break_out_of_town", "{=ybzBF59f}Break out of {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_siege_preparations_break_out_of_town_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_break_out_of_town_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_siege_preparations", "encounter_interrupted_siege_preparations_leave_town", "{=FILG5eZD}Leave {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_siege_preparations_leave_town_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_interrupted_raid_started", "{=7o4AfEhN}While you are resting, you hear news that a force led by {ATTACKER} has arrived outside of {DEFENDER} to raid it.", new OnInitDelegate(this.game_menu_encounter_interrupted_by_raid_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_raid_started", "encounter_interrupted_raid_started_leave", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_by_raid_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("continue_siege_after_attack", "{=CVp0j9al}You have defeated the enemies outside the walls. Now you decide to...", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("continue_siege_after_attack", "continue_siege", "{=zeKvSEpN}Continue the siege", new GameMenuOption.OnConditionDelegate(this.continue_siege_after_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.continue_siege_after_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("continue_siege_after_attack", "leave_siege", "{=b7UHp4J9}Leave the siege", new GameMenuOption.OnConditionDelegate(this.leave_siege_after_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_siege_after_attack_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("continue_siege_after_attack", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(this.leave_army_after_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_army_after_attack_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("town_caught_by_guards", "{=gVuF84RZ}Guards caught you", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("town_caught_by_guards", "town_caught_by_guards_criminal_outside_menu_give_yourself_up", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.outside_menu_criminal_on_condition), new GameMenuOption.OnConsequenceDelegate(this.caught_outside_menu_criminal_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_caught_by_guards", "town_caught_by_guards_enemy_outside_menu_give_yourself_up", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.caught_outside_menu_enemy_on_condition), new GameMenuOption.OnConsequenceDelegate(this.caught_outside_menu_enemy_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_in_menu", "{=!}{BREAK_IN_OUT_MENU}", new OnInitDelegate(this.break_in_menu_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_in_menu", "break_in_menu_accept", "{=DbOv36TA}Go ahead with that.", new GameMenuOption.OnConditionDelegate(this.break_in_menu_accept_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_menu_accept_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("break_in_menu", "break_in_menu_reject", "{=f1etg9oL}Think of something else.", new GameMenuOption.OnConditionDelegate(this.break_in_menu_reject_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_menu_reject_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_in_debrief_menu", "{=!}{BREAK_IN_DEBRIEF}", new OnInitDelegate(this.break_in_out_debrief_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_in_debrief_menu", "break_in_debrief_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_debrief_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_out_menu", "{=!}{BREAK_IN_OUT_MENU}", new OnInitDelegate(this.break_out_menu_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_out_menu", "break_out_menu_accept", "{=DbOv36TA}Go ahead with that.", new GameMenuOption.OnConditionDelegate(this.break_out_menu_accept_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_out_menu_accept_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("break_out_menu", "break_out_menu_reject", "{=f1etg9oL}Think of something else.", new GameMenuOption.OnConditionDelegate(this.break_out_menu_reject_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_out_menu_reject_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_out_debrief_menu", "{=!}{BREAK_IN_DEBRIEF}", new OnInitDelegate(this.break_in_out_debrief_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_out_debrief_menu", "break_out_debrief_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_out_debrief_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("naval_encounter_disengaged", "{=yvLLaNp2}Both sides have disengaged to regroup and consider their next move.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("naval_encounter_disengaged", "naval_encounter_disengaged_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.naval_encounter_disengage_condition), new GameMenuOption.OnConsequenceDelegate(this.naval_encounter_disengaged_continue_on_consequence), false, -1, false, null);
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x00118B3A File Offset: 0x00116D3A
		private bool naval_encounter_disengage_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00118B45 File Offset: 0x00116D45
		private void naval_encounter_disengaged_continue_on_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			GameMenu.ExitToLast();
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00118B54 File Offset: 0x00116D54
		private void escape_continue_on_consequence(MenuCallbackArgs args)
		{
			ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, 10f, true);
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x00118B78 File Offset: 0x00116D78
		private void disguise_not_first_time_init(MenuCallbackArgs args)
		{
			if (Campaign.Current.GameMenuManager.NextLocation != null)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
			}
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00118BE1 File Offset: 0x00116DE1
		private static bool continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00118BEC File Offset: 0x00116DEC
		private bool launch_mission_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00118BF8 File Offset: 0x00116DF8
		private void first_time_disguise_on_init(MenuCallbackArgs args)
		{
			if (this._alreadySneakedSettlements.Contains(Settlement.CurrentSettlement))
			{
				GameMenu.SwitchToMenu("disguise_not_first_time");
				return;
			}
			if (Campaign.Current.GameMenuManager.NextLocation != null)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
			}
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00118C80 File Offset: 0x00116E80
		private void launch_disguise_mission(MenuCallbackArgs args)
		{
			Campaign.Current.IsMainHeroDisguised = true;
			int wallLevel = Settlement.CurrentSettlement.Town.GetWallLevel();
			string sceneName = LocationComplex.Current.GetLocationWithId("center").GetSceneName(wallLevel);
			string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(wallLevel);
			bool flag = !this._alreadySneakedSettlements.Contains(Settlement.CurrentSettlement);
			CampaignMission.OpenDisguiseMission(sceneName, flag, civilianUpgradeLevelTag, null);
			Campaign.Current.GameMenuManager.NextLocation = null;
			Campaign.Current.GameMenuManager.PreviousLocation = null;
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00118D10 File Offset: 0x00116F10
		private static bool menu_sally_out_from_port_on_condition(MenuCallbackArgs args)
		{
			if (EncounterGameMenuBehavior.menu_sally_out_from_gate_on_condition(args) && Settlement.CurrentSettlement.HasPort)
			{
				if (args.Tooltip == null)
				{
					if (!Settlement.CurrentSettlement.SiegeEvent.IsBlockadeActive)
					{
						args.Tooltip = new TextObject("{=eVgOW7bm}There is no active blockade!", null);
						args.IsEnabled = false;
					}
					else if (!MobileParty.MainParty.Ships.Any<Ship>())
					{
						args.Tooltip = new TextObject("{=Yu10hbHI}You don't own any ships!", null);
						args.IsEnabled = false;
					}
					else if (!MobileParty.MainParty.Anchor.IsAtSettlement(Settlement.CurrentSettlement))
					{
						args.Tooltip = new TextObject("{=8VEugUMj}Your fleet is not here!", null);
						args.IsEnabled = false;
					}
					else if (Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && !Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsBlockade)
					{
						args.Tooltip = new TextObject("{=ZEj4Xrbo}You cannot sally out from port during an ongoing assault.", null);
						args.IsEnabled = false;
					}
				}
				args.Text.SetTextVariable("SALLY_OUT_BUTTON_TEXT", new TextObject("{=OnOJMVJO}Sally out from port", null));
				return true;
			}
			return false;
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00118E48 File Offset: 0x00117048
		private static bool menu_sally_out_from_gate_on_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapFaction))
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=UqaNs3ck}You are not at war with the besiegers.", null);
			}
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(PlayerSiege.PlayerSiegeEvent, PlayerSiege.PlayerSide) != Hero.MainHero && (PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent == null || !PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSallyOut))
			{
				args.IsEnabled = false;
				TextObject textObject = new TextObject("{=OmGHXuZB}You are not in command of the defenders.", null);
				args.Tooltip = textObject;
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSallyOut)
			{
				args.Text.SetTextVariable("SALLY_OUT_BUTTON_TEXT", new TextObject("{=fyNNCOFK}Join the sally out", null));
			}
			else
			{
				args.Text.SetTextVariable("SALLY_OUT_BUTTON_TEXT", new TextObject("{=AXxUEFas}Sally out from gate", null));
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00118F93 File Offset: 0x00117193
		private void menu_sally_out_naval_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.SetIsBlockadeSallyOutAttack(true);
			this.sally_out_consequence();
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00118FA6 File Offset: 0x001171A6
		private void menu_sally_out_land_on_consequence(MenuCallbackArgs args)
		{
			this.sally_out_consequence();
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00118FB0 File Offset: 0x001171B0
		private void sally_out_consequence()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty leaderParty = currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
			if (leaderParty.Party.MapEvent != null)
			{
				leaderParty.Party.MapEvent.FinalizeEvent();
			}
			if (currentSettlement.SiegeEvent != null)
			{
				EncounterManager.StartPartyEncounter(MobileParty.MainParty.Party, leaderParty.Party);
				return;
			}
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.SwitchToMenu("siege_attacker_left");
				return;
			}
			GameMenu.ActivateGameMenu("siege_attacker_left");
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00119030 File Offset: 0x00117230
		private static bool menu_defender_siege_break_out_from_port_on_condition(MenuCallbackArgs args)
		{
			if (EncounterGameMenuBehavior.menu_defender_siege_break_out_from_gate_on_condition(args) && Settlement.CurrentSettlement.HasPort)
			{
				if (!MobileParty.MainParty.Ships.Any<Ship>())
				{
					args.Tooltip = new TextObject("{=Yu10hbHI}You don't own any ships!", null);
					args.IsEnabled = false;
				}
				else if (!MobileParty.MainParty.Anchor.IsAtSettlement(Settlement.CurrentSettlement))
				{
					args.Tooltip = new TextObject("{=8VEugUMj}Your fleet is not here!", null);
					args.IsEnabled = false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x001190B0 File Offset: 0x001172B0
		private static bool menu_defender_siege_break_out_from_gate_on_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				args.IsEnabled = true;
				TextObject textObject = new TextObject("{=VUFWXRtP}If you break out from the siege, you will also leave the army. This is a dishonorable act and you will lose relations with all army member lords.{newline}• Army Leader: {ARMY_LEADER_RELATION_PENALTY}{newline}• Army Members: {ARMY_MEMBER_RELATION_PENALTY}", null);
				textObject.SetTextVariable("ARMY_LEADER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyLeaderRelationPenalty);
				textObject.SetTextVariable("ARMY_MEMBER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyMemberRelationPenalty);
				args.Tooltip = textObject;
			}
			if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.MapFaction))
			{
				return false;
			}
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int roundedResultNumber = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent, false).RoundedResultNumber;
			int num = ((mainParty.Army != null && mainParty.Army.LeaderParty == mainParty) ? mainParty.Army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars);
			if (roundedResultNumber > num)
			{
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				args.IsEnabled = false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			return Hero.MainHero.MapFaction != siegeEvent.BesiegerCamp.MapFaction;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00119216 File Offset: 0x00117416
		private void menu_defender_siege_break_out_from_gate_on_consequence(MenuCallbackArgs args)
		{
			this._isBreakingOutFromPort = false;
			GameMenu.SwitchToMenu("break_out_menu");
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00119229 File Offset: 0x00117429
		private void menu_defender_siege_break_out_from_port_on_consequence(MenuCallbackArgs args)
		{
			this._isBreakingOutFromPort = true;
			GameMenu.SwitchToMenu("break_out_menu");
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x0011923C File Offset: 0x0011743C
		private void break_in_leave_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
			if (Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Army != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty != MobileParty.MainParty)
			{
				Hero.MainHero.PartyBelongedTo.Army = null;
				MobileParty.MainParty.SetMoveModeHold();
			}
			if (MobileParty.MainParty.SiegeEvent != null)
			{
				if (MobileParty.MainParty.MapEventSide != null)
				{
					MobileParty.MainParty.MapEventSide = null;
				}
				MobileParty.MainParty.BesiegerCamp = null;
			}
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x001192DC File Offset: 0x001174DC
		private bool game_menu_encounter_army_lead_inf_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any<MapEventParty>((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any<TroopRosterElement>((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.Infantry));
			}
			return false;
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x0011937C File Offset: 0x0011757C
		private void game_menu_encounter_army_lead_inf_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x00119388 File Offset: 0x00117588
		private bool game_menu_encounter_army_lead_arc_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any<MapEventParty>((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any<TroopRosterElement>((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.Ranged));
			}
			return false;
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x00119428 File Offset: 0x00117628
		private void game_menu_encounter_army_lead_arc_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00119434 File Offset: 0x00117634
		private bool game_menu_encounter_army_lead_cav_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any<MapEventParty>((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any<TroopRosterElement>((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.Cavalry));
			}
			return false;
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x001194D4 File Offset: 0x001176D4
		private void game_menu_encounter_army_lead_cav_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x001194DD File Offset: 0x001176DD
		public static void game_menu_captivity_taken_prisoner_cheat_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x001194E8 File Offset: 0x001176E8
		private bool game_menu_encounter_army_lead_har_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any<MapEventParty>((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any<TroopRosterElement>((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.HorseArcher));
			}
			return false;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00119588 File Offset: 0x00117788
		private void game_menu_encounter_army_lead_har_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00119594 File Offset: 0x00117794
		private void game_menu_join_encounter_on_init(MenuCallbackArgs args)
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 0) && PlayerEncounter.Current == null)
			{
				GameMenu.ExitToLast();
				return;
			}
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			PartyBase leaderParty = encounteredBattle.GetLeaderParty(BattleSideEnum.Attacker);
			PartyBase leaderParty2 = encounteredBattle.GetLeaderParty(BattleSideEnum.Defender);
			if (leaderParty.IsMobile && leaderParty.MobileParty.Army != null)
			{
				MBTextManager.SetTextVariable("ATTACKER", leaderParty.MobileParty.ArmyName, false);
			}
			else
			{
				MBTextManager.SetTextVariable("ATTACKER", leaderParty.Name, false);
			}
			if (leaderParty2.IsMobile && leaderParty2.MobileParty.Army != null)
			{
				MBTextManager.SetTextVariable("DEFENDER", leaderParty2.MobileParty.ArmyName, false);
			}
			else
			{
				MBTextManager.SetTextVariable("DEFENDER", leaderParty2.Name, false);
			}
			if (encounteredBattle.IsSallyOut)
			{
				MBTextManager.SetTextVariable("JOIN_ENCOUNTER_TEXT", GameTexts.FindText("str_defenders_make_sally_out", null), false);
				StringHelpers.SetCharacterProperties("BESIEGER_LEADER", Campaign.Current.Models.EncounterModel.GetLeaderOfMapEvent(encounteredBattle, BattleSideEnum.Defender).CharacterObject, null, false);
				return;
			}
			if (leaderParty2.IsSettlement)
			{
				TextObject textObject = new TextObject("{=kDiN9iYw}{ATTACKER} is besieging the walls of {DEFENDER}", null);
				if (encounteredBattle.IsSiegeAssault)
				{
					Settlement.SiegeState currentSiegeState = leaderParty2.Settlement.CurrentSiegeState;
					if (currentSiegeState != Settlement.SiegeState.OnTheWalls && currentSiegeState == Settlement.SiegeState.InTheLordsHall)
					{
						textObject = new TextObject("{=oXY2wnic}{ATTACKER} is fighting inside the lord's hall of {DEFENDER}", null);
					}
				}
				else if (encounteredBattle.IsRaid)
				{
					if (encounteredBattle.DefenderSide.TroopCount > 0)
					{
						textObject = new TextObject("{=kvNQLcCb}{ATTACKER} is fighting in {DEFENDER}", null);
					}
					else
					{
						textObject = new TextObject("{=BExNNwm0}{ATTACKER} is raiding {DEFENDER}", null);
					}
				}
				MBTextManager.SetTextVariable("JOIN_ENCOUNTER_TEXT", textObject, false);
				return;
			}
			MBTextManager.SetTextVariable("JOIN_ENCOUNTER_TEXT", GameTexts.FindText("str_come_across_battle", null), false);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x00119738 File Offset: 0x00117938
		private bool game_menu_join_encounter_help_attackers_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			IFaction mapFaction = encounteredBattle.GetLeaderParty(BattleSideEnum.Defender).MapFaction;
			this.CheckFactionAttackableHonorably(args, mapFaction);
			if (encounteredBattle.IsNavalMapEvent != MobileParty.MainParty.IsCurrentlyAtSea && !encounteredBattle.IsRaid)
			{
				args.IsEnabled = false;
				if (encounteredBattle.IsBlockade)
				{
					args.Tooltip = new TextObject("{=Lg3U6trj}You cannot join the siege since there is an ongoing naval battle outside the harbor. You should join that battle by sea.", null);
				}
				else
				{
					args.Tooltip = new TextObject("{=aBHvjGLh}You cannot join the sea battle since there is an ongoing assault on the walls. You can join the assault by land.", null);
				}
			}
			else if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MapEvent encounteredBattle2 = PlayerEncounter.EncounteredBattle;
				if (encounteredBattle2 != null && encounteredBattle2.IsRaid)
				{
					MapEvent encounteredBattle3 = PlayerEncounter.EncounteredBattle;
					bool flag;
					if (encounteredBattle3 == null)
					{
						flag = false;
					}
					else
					{
						Settlement mapEventSettlement = encounteredBattle3.MapEventSettlement;
						bool? flag2 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (flag)
					{
						int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
						if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumNumberOfMenForAttackingVillageViaScene)
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=7b4WZVyU}You should at least have {NUMBER} healthy men in your party to take a hostile action.", null);
							args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
						}
						else if (!ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().AnyQ<Ship>())
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=hd626n2z}You don't have any shallow draft ship.", null);
						}
						else if (Math.Min(MobileParty.MainParty.MemberRoster.TotalHealthyCount, ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().SumQ<Ship>((Ship x) => x.MainDeckCrewCapacity)) < minimumNumberOfMenForAttackingVillageViaScene)
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=*}Your shallow ship's crew capacity is too low for a hostile action. A minimum of {NUMBER} crew is required. Use a larger or additional vessel.", null);
							args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
						}
					}
				}
			}
			return encounteredBattle.CanPartyJoinBattle(PartyBase.MainParty, BattleSideEnum.Attacker);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x00119920 File Offset: 0x00117B20
		private void game_menu_join_encounter_help_attackers_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.InsideSettlement && PlayerEncounter.EncounterSettlement.IsUnderSiege)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PartyBase partyBase;
			if (!PlayerEncounter.EncounteredBattle.DefenderSide.LeaderParty.IsMobile)
			{
				MapEventParty mapEventParty = PlayerEncounter.EncounteredBattle.DefenderSide.Parties.FirstOrDefault<MapEventParty>((MapEventParty x) => x.Party.IsMobile);
				partyBase = ((mapEventParty != null) ? mapEventParty.Party : null);
			}
			else
			{
				partyBase = PlayerEncounter.EncounteredBattle.DefenderSide.LeaderParty;
			}
			PartyBase partyBase2 = partyBase;
			if (PlayerEncounter.EncounteredBattle.IsRaid && PlayerEncounter.EncounteredBattle.WasEverInLootingPhase && partyBase2 != null)
			{
				List<PartyBase> list = new List<PartyBase>();
				foreach (MapEventParty mapEventParty2 in PlayerEncounter.EncounteredBattle.AttackerSide.Parties)
				{
					if (mapEventParty2.Party != PartyBase.MainParty && mapEventParty2.Party.IsMobile)
					{
						list.Add(mapEventParty2.Party);
					}
				}
				List<PartyBase> list2 = new List<PartyBase>();
				foreach (MapEventParty mapEventParty3 in PlayerEncounter.EncounteredBattle.DefenderSide.Parties)
				{
					if (mapEventParty3.Party != partyBase2 && mapEventParty3.Party.IsMobile)
					{
						list2.Add(mapEventParty3.Party);
					}
				}
				bool wasEverInLootingPhase = PlayerEncounter.EncounteredBattle.WasEverInLootingPhase;
				Settlement mapEventSettlement = PlayerEncounter.EncounteredBattle.MapEventSettlement;
				PlayerEncounter.RestartPlayerEncounter(PartyBase.MainParty, partyBase2, true, true);
				GameMenu.ActivateGameMenu("encounter");
				MapEvent.PlayerMapEvent.WasEverInLootingPhase = wasEverInLootingPhase;
				MapEvent.PlayerMapEvent.OverrideMapEventSettlementForRaidToFieldBattleSwitch(mapEventSettlement);
				foreach (PartyBase partyBase3 in list)
				{
					partyBase3.MapEventSide = PartyBase.MainParty.MapEventSide;
				}
				foreach (PartyBase partyBase4 in list2)
				{
					partyBase4.MapEventSide = partyBase2.MapEventSide;
				}
				return;
			}
			PlayerEncounter.JoinBattle(BattleSideEnum.Attacker);
			if (PlayerEncounter.Battle.DefenderSide.TroopCount > 0)
			{
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			if (MobileParty.MainParty.Army != null)
			{
				if (MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
				{
					if (!MobileParty.MainParty.Army.LeaderParty.AttachedParties.Contains(MobileParty.MainParty))
					{
						MobileParty.MainParty.Army.AddPartyToMergedParties(MobileParty.MainParty);
						Campaign.Current.CameraFollowParty = MobileParty.MainParty.Army.LeaderParty.Party;
						CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
					}
					if (PlayerEncounter.Battle.IsRaid)
					{
						GameMenu.SwitchToMenu("raiding_village");
						return;
					}
					GameMenu.SwitchToMenu("army_wait");
					return;
				}
				else
				{
					if (PlayerEncounter.Battle.IsRaid)
					{
						GameMenu.SwitchToMenu("raiding_village");
						MobileParty.MainParty.SetMoveModeHold();
						return;
					}
					GameMenu.SwitchToMenu("encounter");
					return;
				}
			}
			else
			{
				if (PlayerEncounter.Battle.IsRaid)
				{
					GameMenu.SwitchToMenu("raiding_village");
					MobileParty.MainParty.SetMoveModeHold();
					return;
				}
				GameMenu.SwitchToMenu("menu_siege_strategies");
				MobileParty.MainParty.SetMoveModeHold();
				return;
			}
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00119CB0 File Offset: 0x00117EB0
		private bool game_menu_join_encounter_abandon_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00119CE4 File Offset: 0x00117EE4
		private bool game_menu_join_encounter_help_defenders_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			IFaction mapFaction = encounteredBattle.GetLeaderParty(BattleSideEnum.Attacker).MapFaction;
			this.CheckFactionAttackableHonorably(args, mapFaction);
			if (MobileParty.MainParty.MemberRoster.TotalHealthyCount == 0)
			{
				args.Tooltip = new TextObject("{=Z6kgb8go}You have no healthy members of your party who can fight", null);
				args.IsEnabled = false;
			}
			if (encounteredBattle.IsNavalMapEvent != MobileParty.MainParty.IsCurrentlyAtSea && !encounteredBattle.IsRaid)
			{
				args.IsEnabled = false;
				if (encounteredBattle.IsBlockade)
				{
					args.Tooltip = new TextObject("{=4VwBa182}You cannot join the battle as the blockade is under attack. You can join that battle by sea.", null);
				}
				else
				{
					args.Tooltip = new TextObject("{=RvLaJbkQ}You cannot join the battle as the walls are being assaulted. You can join the assault on the walls by land.", null);
				}
			}
			else if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MapEvent encounteredBattle2 = PlayerEncounter.EncounteredBattle;
				bool flag;
				if (encounteredBattle2 == null)
				{
					flag = false;
				}
				else
				{
					Settlement mapEventSettlement = encounteredBattle2.MapEventSettlement;
					bool? flag2 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
				}
				if (flag)
				{
					MapEvent encounteredBattle3 = PlayerEncounter.EncounteredBattle;
					if (encounteredBattle3 != null && encounteredBattle3.IsRaid)
					{
						int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
						if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumNumberOfMenForAttackingVillageViaScene)
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=7b4WZVyU}You should at least have {NUMBER} healthy men in your party to take a hostile action.", null);
							args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
						}
						else if (!ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().AnyQ<Ship>())
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=hd626n2z}You don't have any shallow draft ship.", null);
						}
						else if (Math.Min(MobileParty.MainParty.MemberRoster.TotalHealthyCount, ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().SumQ<Ship>((Ship x) => x.MainDeckCrewCapacity)) < minimumNumberOfMenForAttackingVillageViaScene)
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=*}Your shallow ship's crew capacity is too low for a hostile action. A minimum of {NUMBER} crew is required. Use a larger or additional vessel.", null);
							args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
						}
					}
				}
			}
			return encounteredBattle.CanPartyJoinBattle(PartyBase.MainParty, BattleSideEnum.Defender);
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x00119EF3 File Offset: 0x001180F3
		public static bool game_menu_captivity_castle_taken_prisoner_cont_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x00119F00 File Offset: 0x00118100
		private void game_menu_join_encounter_help_defenders_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.EncounteredParty != null)
			{
				if (PlayerEncounter.EncounteredParty.MapEvent == null)
				{
					if (!PlayerEncounter.EncounteredParty.IsSettlement)
					{
						goto IL_0209;
					}
					SiegeEvent siegeEvent = PlayerEncounter.EncounteredParty.SiegeEvent;
					if (((siegeEvent != null) ? siegeEvent.BesiegerCamp.LeaderParty.MapEvent : null) == null)
					{
						goto IL_0209;
					}
				}
				if (PlayerEncounter.EncounteredBattle.IsRaid)
				{
					bool interruptedWhileWaiting = PlayerEncounter.Current.InterruptedWhileWaiting;
					Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
					PartyBase leaderParty = PlayerEncounter.EncounteredBattle.AttackerSide.LeaderParty;
					List<PartyBase> list = new List<PartyBase>();
					bool wasEverInLootingPhase = PlayerEncounter.EncounteredBattle.WasEverInLootingPhase;
					foreach (MapEventParty mapEventParty in PlayerEncounter.EncounteredBattle.AttackerSide.Parties)
					{
						if (mapEventParty.Party != leaderParty)
						{
							list.Add(mapEventParty.Party);
						}
					}
					List<PartyBase> list2 = new List<PartyBase>();
					foreach (MapEventParty mapEventParty2 in PlayerEncounter.EncounteredBattle.DefenderSide.Parties)
					{
						if (mapEventParty2.Party.IsMobile)
						{
							list2.Add(mapEventParty2.Party);
						}
					}
					PlayerEncounter.EncounteredBattle.FinalizeEvent();
					if (interruptedWhileWaiting || !wasEverInLootingPhase)
					{
						PlayerEncounter.RestartPlayerEncounter(PartyBase.MainParty, leaderParty, true, false);
					}
					else
					{
						PlayerEncounter.RestartPlayerEncounter(leaderParty, PartyBase.MainParty, true, true);
					}
					GameMenu.ActivateGameMenu("encounter");
					foreach (PartyBase partyBase in list)
					{
						partyBase.MapEventSide = leaderParty.MapEventSide;
					}
					foreach (PartyBase partyBase2 in list2)
					{
						partyBase2.MapEventSide = PartyBase.MainParty.MapEventSide;
					}
					MapEvent.PlayerMapEvent.OverrideMapEventSettlementForRaidToFieldBattleSwitch(encounterSettlement);
					MapEvent.PlayerMapEvent.WasEverInLootingPhase = wasEverInLootingPhase;
					return;
				}
				PlayerEncounter.JoinBattle(BattleSideEnum.Defender);
				GameMenu.ActivateGameMenu("encounter");
				return;
			}
			IL_0209:
			if (PlayerEncounter.Current != null)
			{
				if (PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.SiegeEvent != null && !PlayerEncounter.EncounterSettlement.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction))
				{
					PlayerEncounter.RestartPlayerEncounter(PlayerEncounter.EncounterSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party, PartyBase.MainParty, false, false);
				}
				GameMenu.ActivateGameMenu("encounter");
			}
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x0011A1B0 File Offset: 0x001183B0
		private void naval_town_outside_on_init(MenuCallbackArgs args)
		{
			this.InitializeAccessDetails();
			if (PlayerEncounter.EncounterSettlement.IsUnderSiege && PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				Debug.FailedAssert("naval_town_outside_on_init", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\EncounterGameMenuBehavior.cs", "naval_town_outside_on_init", 1146);
				PlayerEncounter.Finish(true);
			}
			TextObject textObject = null;
			if (PlayerEncounter.EncounterSettlement.IsUnderSiege)
			{
				if (PlayerEncounter.EncounterSettlement.MapFaction.IsAtWarWith(Clan.PlayerClan.MapFaction))
				{
					textObject = new TextObject("{=n5A1tp2j}The settlement is under siege, and is also hostile to you. You may not enter.", null);
				}
				else if (!PlayerEncounter.EncounterSettlement.SiegeEvent.IsBlockadeActive)
				{
					this.game_menu_naval_town_outside_enter_on_consequence();
				}
				else if (!PlayerEncounter.EncounterSettlement.SiegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Defender) && !PlayerEncounter.EncounterSettlement.SiegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Attacker))
				{
					textObject = new TextObject("{=dZNCEaGB}The settlement is under siege and naval blockade. You are not allowed to get involved.", null);
				}
				else
				{
					textObject = new TextObject("{=ccttrcaX}The settlement is under siege and naval blockade. You may attempt to run the blockade.", null);
				}
			}
			else if (PlayerEncounter.EncounterSettlement.MapFaction.IsAtWarWith(Clan.PlayerClan.MapFaction))
			{
				textObject = new TextObject("{=eGizNNNC}The settlement is hostile to you, and you will not be allowed to dock at the port.", null);
			}
			else if (this.game_menu_town_disguise_yourself_on_condition(args))
			{
				textObject = new TextObject("{=X3TL6QZ8}You are wanted in the settlement for criminal acts, and you will not be allowed to dock at the port.", null);
			}
			else
			{
				GameMenu.SwitchToMenu("port_menu");
			}
			if (!TextObject.IsNullOrEmpty(textObject))
			{
				MBTextManager.SetTextVariable("PORT_OUTSIDE_TEXT", textObject, false);
			}
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x0011A311 File Offset: 0x00118511
		private void game_menu_join_siege_event_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				PlayerEncounter.Finish(true);
			}
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0011A331 File Offset: 0x00118531
		private void game_menu_join_sally_out_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x0011A348 File Offset: 0x00118548
		private bool game_menu_join_siege_event_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			TextObject textObject;
			if (DiplomacyHelper.DidMainHeroSwornNotToAttackFaction(Settlement.CurrentSettlement.MapFaction, out textObject))
			{
				args.IsEnabled = false;
				args.Tooltip = textObject;
			}
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			return siegeEvent != null && siegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Attacker);
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0011A3A0 File Offset: 0x001185A0
		private void game_menu_join_siege_event_on_consequence(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.IsMainParty && !Settlement.CurrentSettlement.SiegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Attacker))
			{
				Debug.FailedAssert("Player should not be able to join this siege.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\EncounterGameMenuBehavior.cs", "game_menu_join_siege_event_on_consequence", 1255);
				return;
			}
			if (Settlement.CurrentSettlement.Party.MapEvent != null)
			{
				PlayerEncounter.JoinBattle(Settlement.CurrentSettlement.Party.MapEvent.IsSallyOut ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (Hero.MainHero.CurrentSettlement != null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.BesiegerCamp = currentSettlement.SiegeEvent.BesiegerCamp;
			PlayerSiege.StartPlayerSiege(BattleSideEnum.Attacker, false, currentSettlement);
			PlayerSiege.StartSiegePreparation();
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x0011A486 File Offset: 0x00118686
		private bool game_menu_join_sally_out_event_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return true;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x0011A491 File Offset: 0x00118691
		private bool game_menu_stay_in_settlement_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x0011A49C File Offset: 0x0011869C
		private void game_menu_join_sally_out_on_consequence(MenuCallbackArgs args)
		{
			PartyBase sallyOutDefenderLeader = MapEventHelper.GetSallyOutDefenderLeader();
			EncounterManager.StartPartyEncounter(MobileParty.MainParty.Party, sallyOutDefenderLeader);
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0011A4BF File Offset: 0x001186BF
		private void game_menu_stay_in_settlement_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("menu_siege_strategies");
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0011A4DD File Offset: 0x001186DD
		private bool break_in_to_help_defender_side_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			return this.common_join_siege_event_button_condition(args);
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0011A4F0 File Offset: 0x001186F0
		private bool common_join_siege_event_button_condition(MenuCallbackArgs args)
		{
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			if (siegeEvent != null)
			{
				MobileParty mainParty = MobileParty.MainParty;
				int roundedResultNumber = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent).RoundedResultNumber;
				Army army = mainParty.Army;
				int num = ((army != null) ? army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars);
				TextObject textObject;
				if (DiplomacyHelper.DidMainHeroSwornNotToAttackFaction(siegeEvent.BesiegerCamp.MapFaction, out textObject))
				{
					args.IsEnabled = false;
					args.Tooltip = textObject;
				}
				else if (roundedResultNumber > num)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				}
				return siegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Defender);
			}
			return false;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x0011A5A5 File Offset: 0x001187A5
		private bool attack_besieger_side_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return !MobileParty.MainParty.IsCurrentlyAtSea && this.common_join_siege_event_button_condition(args);
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0011A5C4 File Offset: 0x001187C4
		private bool attack_blockade_besieger_side_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return this.attack_blockade_besieger_side_common_condition(args);
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0011A5D5 File Offset: 0x001187D5
		private bool attack_blockade_besieger_side_break_in_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.OrderShipsToAttack;
			return this.attack_blockade_besieger_side_common_condition(args);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0011A5E6 File Offset: 0x001187E6
		private bool attack_blockade_besieger_side_common_condition(MenuCallbackArgs args)
		{
			return MobileParty.MainParty.IsCurrentlyAtSea && PlayerEncounter.EncounterSettlement.SiegeEvent.IsBlockadeActive && this.common_join_siege_event_button_condition(args);
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0011A60E File Offset: 0x0011880E
		private void game_menu_join_siege_event_on_defender_side_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("break_in_menu");
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0011A61C File Offset: 0x0011881C
		private bool game_menu_join_encounter_leave_no_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			MBTextManager.SetTextVariable("LEAVE_TEXT", "{=ebUwP3Q3}Don't get involved.", false);
			if (MobileParty.MainParty.Army != null)
			{
				Army army = MobileParty.MainParty.Army;
				return ((army != null) ? army.LeaderParty : null) == MobileParty.MainParty;
			}
			return true;
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0011A66C File Offset: 0x0011886C
		private bool game_menu_join_encounter_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			MobileParty mobileParty = ((Settlement.CurrentSettlement.SiegeEvent != null) ? Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty : null);
			return mobileParty == null || !mobileParty.IsMainParty;
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0011A6B3 File Offset: 0x001188B3
		private bool game_menu_siege_attacker_left_return_to_settlement_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			GameTexts.SetVariable("SETTLEMENT", MobileParty.MainParty.LastVisitedSettlement.Name);
			return true;
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0011A6D8 File Offset: 0x001188D8
		private void game_menu_siege_attacker_left_return_to_settlement_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(false);
			}
			if (MobileParty.MainParty.AttachedTo == null)
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, MobileParty.MainParty.LastVisitedSettlement);
			}
			else
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty.AttachedTo, MobileParty.MainParty.LastVisitedSettlement);
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.LocationEncounter == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (string.IsNullOrEmpty(genericStateMenu))
			{
				GameMenu.ExitToLast();
				return;
			}
			GameMenu.SwitchToMenu(genericStateMenu);
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0011A769 File Offset: 0x00118969
		private bool game_menu_siege_attacker_left_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x0011A774 File Offset: 0x00118974
		private bool game_menu_siege_attacker_defeated_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x0011A77F File Offset: 0x0011897F
		private bool game_menu_encounter_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.CheatMode;
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0011A78C File Offset: 0x0011898C
		private void game_menu_encounter_interrupted_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PartyBase leaderParty = PlayerEncounter.EncounteredBattle.GetLeaderParty(BattleSideEnum.Attacker);
			MBTextManager.SetTextVariable("ATTACKER", leaderParty.Name, false);
			MBTextManager.SetTextVariable("DEFENDER", currentSettlement.Name, false);
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0011A7D0 File Offset: 0x001189D0
		private void game_menu_encounter_interrupted_siege_preparations_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			TextObject name = Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Name;
			TextObject text = args.MenuContext.GameMenu.GetText();
			text.SetTextVariable("ATTACKER", name);
			text.SetTextVariable("DEFENDER", currentSettlement.Name);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.InterruptedWhileWaiting = PlayerEncounter.Current.IsPlayerWaiting;
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x0011A854 File Offset: 0x00118A54
		private bool game_menu_encounter_interrupted_siege_preparations_leave_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			return !FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.MapFaction);
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x0011A8B4 File Offset: 0x00118AB4
		private void game_menu_encounter_interrupted_by_raid_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			TextObject name = currentSettlement.Party.MapEvent.GetLeaderParty(currentSettlement.Party.OpponentSide).Name;
			TextObject text = args.MenuContext.GameMenu.GetText();
			text.SetTextVariable("ATTACKER", name);
			text.SetTextVariable("DEFENDER", currentSettlement.Name);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.InterruptedWhileWaiting = PlayerEncounter.Current.IsPlayerWaiting;
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x0011A93C File Offset: 0x00118B3C
		private bool game_menu_encounter_interrupted_by_raid_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x0011A948 File Offset: 0x00118B48
		private void game_menu_settlement_hide_and_wait_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			SiegeEvent siegeEvent = currentSettlement.SiegeEvent;
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp.LeaderParty : null) != null)
			{
				GameMenu.SwitchToMenu("encounter_interrupted_siege_preparations");
				return;
			}
			if (currentSettlement.IsTown)
			{
				GameMenu.SwitchToMenu("town");
				return;
			}
			if (currentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
			}
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x0011A9A4 File Offset: 0x00118BA4
		private bool game_menu_settlement_hide_and_wait_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x0011A9AF File Offset: 0x00118BAF
		private bool wait_menu_settlement_hide_and_wait_on_condition(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.StoppableFastForward;
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return true;
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x0011A9EC File Offset: 0x00118BEC
		private bool game_menu_encounter_interrupted_siege_preparations_break_out_of_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int roundedResultNumber = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent, false).RoundedResultNumber;
			int num = ((mainParty.Army != null && mainParty.Army.LeaderParty == mainParty) ? mainParty.Army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars);
			if (mainParty.Army != null && mainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				args.IsEnabled = true;
				TextObject textObject = new TextObject("{=VUFWXRtP}If you break out from the siege, you will also leave the army. This is a dishonorable act and you will lose relations with all army member lords.{newline}• Army Leader: {ARMY_LEADER_RELATION_PENALTY}{newline}• Army Members: {ARMY_MEMBER_RELATION_PENALTY}", null);
				textObject.SetTextVariable("ARMY_LEADER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyLeaderRelationPenalty);
				textObject.SetTextVariable("ARMY_MEMBER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyMemberRelationPenalty);
				args.Tooltip = textObject;
			}
			if (roundedResultNumber > num)
			{
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				args.IsEnabled = false;
			}
			return FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, siegeEvent.BesiegerCamp.MapFaction);
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x0011AB34 File Offset: 0x00118D34
		private bool game_menu_encounter_interrupted_siege_preparations_hide_in_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			IFaction mapFaction = Hero.MainHero.MapFaction;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			IFaction mapFaction2 = Settlement.CurrentSettlement.MapFaction;
			return mapFaction != siegeEvent.BesiegerCamp.MapFaction && (FactionManager.IsAtWarAgainstFaction(mapFaction2, mapFaction) || FactionManager.IsNeutralWithFaction(mapFaction2, mapFaction));
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x0011AB8C File Offset: 0x00118D8C
		private void game_menu_encounter_interrupted_break_out_of_town_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("break_out_menu");
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x0011AB98 File Offset: 0x00118D98
		private void game_menu_encounter_interrupted_siege_preparations_join_defend_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PlayerSiege.StartPlayerSiege(BattleSideEnum.Defender, false, null);
			MobileParty.MainParty.SetMoveDefendSettlement(currentSettlement, false, MobileParty.NavigationType.Default);
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0011ABC5 File Offset: 0x00118DC5
		private bool game_menu_encounter_interrupted_siege_preparations_join_defend_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			GameTexts.SetVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			return Settlement.CurrentSettlement.SiegeEvent.CanPartyJoinSide(PartyBase.MainParty, BattleSideEnum.Defender);
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0011ABF8 File Offset: 0x00118DF8
		private void game_menu_encounter_interrupted_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0011AC00 File Offset: 0x00118E00
		public static void menu_sneak_into_town_succeeded_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0011AC0C File Offset: 0x00118E0C
		public static bool menu_sneak_into_town_succeeded_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0011AC17 File Offset: 0x00118E17
		public static void game_menu_sneak_into_town_caught_on_init(MenuCallbackArgs args)
		{
			ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, 10f, true);
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x0011AC2E File Offset: 0x00118E2E
		public static void mno_sneak_caught_surrender_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("menu_captivity_castle_taken_prisoner");
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0011AC3A File Offset: 0x00118E3A
		public static bool mno_sneak_caught_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return true;
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0011AC45 File Offset: 0x00118E45
		private void game_menu_encounter_interrupted_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("join_encounter");
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x0011AC51 File Offset: 0x00118E51
		private bool game_menu_encounter_interrupted_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x0011AC5C File Offset: 0x00118E5C
		private void game_menu_town_assault_on_init(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0011AC6F File Offset: 0x00118E6F
		private void game_menu_town_assault_order_attack_on_init(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
			this.game_menu_encounter_order_attack_on_consequence(args);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0011AC84 File Offset: 0x00118E84
		private void game_menu_army_encounter_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.LeaveEncounter)
			{
				PlayerEncounter.Finish(true);
				MobileParty.MainParty.SetMoveModeHold();
				return;
			}
			if ((PlayerEncounter.Battle != null && PlayerEncounter.Battle.AttackerSide.LeaderParty != PartyBase.MainParty && PlayerEncounter.Battle.DefenderSide.LeaderParty != PartyBase.MainParty) || PlayerEncounter.MeetingDone)
			{
				if (PlayerEncounter.Battle == null)
				{
					PlayerEncounter.StartBattle();
				}
				if (PlayerEncounter.BattleChallenge)
				{
					GameMenu.SwitchToMenu("duel_starter_menu");
					return;
				}
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			else
			{
				if (PlayerEncounter.EncounteredMobileParty.SiegeEvent != null && Settlement.CurrentSettlement != null)
				{
					GameMenu.SwitchToMenu("join_siege_event");
					return;
				}
				if (PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.Army != null)
				{
					MBTextManager.SetTextVariable("ARMY", PlayerEncounter.EncounteredMobileParty.Army.Name, false);
					MBTextManager.SetTextVariable("ARMY_ENCOUNTER_TEXT", GameTexts.FindText("str_you_have_encountered_ARMY", null), true);
				}
				return;
			}
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0011AD70 File Offset: 0x00118F70
		private void game_menu_encounter_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/battle/slide_in");
			if (PlayerEncounter.Battle == null)
			{
				if (MobileParty.MainParty.MapEvent != null)
				{
					PlayerEncounter.Init();
				}
				else
				{
					PlayerEncounter.StartBattle();
				}
			}
			PlayerEncounter.Update();
			this.UpdateVillageHostileActionEncounter(args);
			EncounterGameMenuBehavior.UpdateHideoutHostileActionEncounter();
			if (PlayerEncounter.Current == null)
			{
				Campaign.Current.SaveHandler.SignalAutoSave();
			}
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x0011ADD4 File Offset: 0x00118FD4
		private static void UpdateHideoutHostileActionEncounter()
		{
			MapEvent battle = PlayerEncounter.Battle;
			HideoutEventComponent hideoutEventComponent;
			if (Game.Current.GameStateManager.ActiveState is MapState && ((battle != null) ? battle.MapEventSettlement : null) != null && battle.MapEventSettlement.IsHideout && battle.IsHideoutBattle && battle.DefenderSide.LeaderParty.IsSettlement && battle.AttackerSide == battle.GetMapEventSide(battle.PlayerSide) && (hideoutEventComponent = battle.Component as HideoutEventComponent) != null && hideoutEventComponent.IsSendTroops)
			{
				GameMenu.SwitchToMenu("hideout_send_troops_wait");
			}
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0011AE68 File Offset: 0x00119068
		private void UpdateVillageHostileActionEncounter(MenuCallbackArgs args)
		{
			MapEvent battle = PlayerEncounter.Battle;
			MapState mapState;
			if ((mapState = Game.Current.GameStateManager.ActiveState as MapState) != null && !mapState.MapConversationActive && ((battle != null) ? battle.MapEventSettlement : null) != null && battle.MapEventSettlement.IsVillage && battle.DefenderSide.LeaderParty.IsSettlement && battle.AttackerSide == battle.GetMapEventSide(battle.PlayerSide))
			{
				bool flag = battle.DefenderSide.Parties.All<MapEventParty>((MapEventParty x) => x.Party.MemberRoster.TotalHealthyCount == 0);
				bool flag2 = this.ConsiderVillageSurrenderPossibility();
				if (flag || flag2)
				{
					if (!flag)
					{
						for (int i = battle.DefenderSide.Parties.Count - 1; i >= 0; i--)
						{
							if (battle.DefenderSide.Parties[i].Party.IsMobile)
							{
								battle.DefenderSide.Parties[i].Party.MapEventSide = null;
							}
						}
						if (!battle.IsRaid)
						{
							battle.SetOverrideWinner(BattleSideEnum.Attacker);
						}
					}
					if (battle.IsRaid)
					{
						this.game_menu_village_raid_no_resist_on_consequence(args);
						return;
					}
					if (battle.IsForcingSupplies)
					{
						this.game_menu_village_force_supplies_no_resist_loot_on_consequence(args);
						return;
					}
					if (battle.IsForcingVolunteers)
					{
						this.game_menu_village_force_volunteers_no_resist_loot_on_consequence(args);
						return;
					}
				}
				else if (!battle.AttackerSide.MapFaction.IsAtWarWith(battle.DefenderSide.MapFaction))
				{
					Debug.FailedAssert("This case should not be happening anymore, check this case and make sure this is intended", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\EncounterGameMenuBehavior.cs", "UpdateVillageHostileActionEncounter", 1776);
				}
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0011B001 File Offset: 0x00119201
		public static bool game_menu_captivity_taken_prisoner_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0011B010 File Offset: 0x00119210
		private bool ConsiderVillageSurrenderPossibility()
		{
			bool flag = false;
			MapEvent battle = PlayerEncounter.Battle;
			if ((battle.IsRaid || battle.IsForcingSupplies || battle.IsForcingVolunteers) && battle.MapEventSettlement.IsVillage)
			{
				Settlement mapEventSettlement = battle.MapEventSettlement;
				float num = 0f;
				bool flag2 = false;
				foreach (MapEventParty mapEventParty in battle.DefenderSide.Parties)
				{
					num += mapEventParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.Village);
					if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsLordParty)
					{
						flag2 = true;
					}
				}
				float num2 = 0f;
				foreach (MapEventParty mapEventParty2 in battle.AttackerSide.Parties)
				{
					if (!mapEventParty2.Party.IsMobile || mapEventParty2.Party.MobileParty.Army == null)
					{
						num2 += mapEventParty2.Party.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.Village);
					}
					else if (mapEventParty2.Party.IsMobile && mapEventParty2.Party.MobileParty.Army != null && mapEventParty2.Party.MobileParty.Army.LeaderParty == mapEventParty2.Party.MobileParty)
					{
						foreach (MobileParty mobileParty in mapEventParty2.Party.MobileParty.Army.LeaderParty.AttachedParties)
						{
							num2 += mobileParty.Party.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.Village);
						}
					}
				}
				Clan ownerClan = mapEventSettlement.OwnerClan;
				bool flag3;
				if (ownerClan == null)
				{
					flag3 = null != null;
				}
				else
				{
					Hero leader = ownerClan.Leader;
					flag3 = ((leader != null) ? leader.PartyBelongedTo : null) != null;
				}
				float num3 = (flag3 ? mapEventSettlement.OwnerClan.Leader.PartyBelongedTo.Party.RandomFloatWithSeed(1U, 0.05f, 0.15f) : 0.1f);
				flag = !flag2 && num2 * num3 > num;
			}
			return flag;
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0011B270 File Offset: 0x00119470
		private bool game_menu_encounter_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MapEventHelper.CanMainPartyLeaveBattleCommonCondition() && this.LeaveOptionVisibilityCheckForNavalRaid() && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && ((!MobileParty.MainParty.MapEvent.IsSallyOut && !MobileParty.MainParty.MapEvent.IsBlockadeSallyOut) || MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Defender || MobileParty.MainParty.CurrentSettlement == null);
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0011B2FC File Offset: 0x001194FC
		private bool LeaveOptionVisibilityCheckForNavalRaid()
		{
			if (!PlayerEncounter.Current.IsPlayerEncounterRestartedForRaid)
			{
				MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
				if (encounteredBattle != null && encounteredBattle.IsRaid)
				{
					MapEvent encounteredBattle2 = PlayerEncounter.EncounteredBattle;
					bool flag;
					if (encounteredBattle2 == null)
					{
						flag = false;
					}
					else
					{
						Settlement mapEventSettlement = encounteredBattle2.MapEventSettlement;
						bool? flag2 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (flag)
					{
						goto IL_0063;
					}
				}
				return true;
			}
			IL_0063:
			if (MapEventHelper.IsNavalRaid(PlayerEncounter.EncounteredBattle))
			{
				int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
				return Math.Min(MobileParty.MainParty.MemberRoster.TotalHealthyCount, ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().SumQ<Ship>((Ship x) => x.MainDeckCrewCapacity)) < minimumNumberOfMenForAttackingVillageViaScene;
			}
			return false;
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0011B3D4 File Offset: 0x001195D4
		private bool game_menu_sally_out_go_back_to_settlement_on_condition(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.MapEvent != null && (MobileParty.MainParty.MapEvent.IsSallyOut || MobileParty.MainParty.MapEvent.IsBlockadeSallyOut) && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.CurrentSettlement != null)
			{
				bool flag = Campaign.Current.Models.EncounterModel.GetLeaderOfMapEvent(MobileParty.MainParty.MapEvent, MobileParty.MainParty.MapEvent.PlayerSide) == Hero.MainHero;
				if ((MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty || flag) && (MobileParty.MainParty.SiegeEvent == null || !MobileParty.MainParty.SiegeEvent.BesiegerCamp.IsBesiegerSideParty(MobileParty.MainParty)))
				{
					args.optionLeaveType = GameMenuOption.LeaveType.Leave;
					GameTexts.SetVariable("SETTLEMENT", MobileParty.MainParty.LastVisitedSettlement.Name);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x0011B4E4 File Offset: 0x001196E4
		private void game_menu_sally_out_go_back_to_settlement_consequence(MenuCallbackArgs args)
		{
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			playerMapEvent.BeginWait();
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfMapEvent(playerMapEvent, playerMapEvent.PlayerSide) == Hero.MainHero)
			{
				PlayerEncounter.Current.FinalizeBattle();
				PlayerEncounter.Current.SetupFields(Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party, PartyBase.MainParty);
			}
			else
			{
				PlayerEncounter.LeaveBattle();
			}
			GameMenu.SwitchToMenu("menu_siege_strategies");
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0011B564 File Offset: 0x00119764
		private bool game_menu_encounter_abandon_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && !MobileParty.MainParty.MapEvent.IsSallyOut && MapEventHelper.CanMainPartyLeaveBattleCommonCondition();
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x0011B5B4 File Offset: 0x001197B4
		private void game_menu_army_talk_to_leader_on_consequence(MenuCallbackArgs args)
		{
			Campaign.Current.CurrentConversationContext = ConversationContext.PartyEncounter;
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(PlayerEncounter.EncounteredParty), PlayerEncounter.EncounteredParty, false, false, false, false, false, false);
			PlayerEncounter.SetMeetingDone();
			if (PartyBase.MainParty.MobileParty.IsCurrentlyAtSea)
			{
				CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
				return;
			}
			CampaignMapConversation.OpenConversation(conversationCharacterData, conversationCharacterData2);
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x0011B638 File Offset: 0x00119838
		private bool game_menu_army_talk_to_leader_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (((encounteredParty != null) ? encounteredParty.LeaderHero : null) != null)
			{
				MenuHelper.SetIssueAndQuestDataForHero(args, PlayerEncounter.EncounteredParty.LeaderHero);
			}
			return true;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0011B666 File Offset: 0x00119866
		public static void game_menu_captivity_castle_taken_prisoner_cont_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
			PartyBase.MainParty.AddElementToMemberRoster(CharacterObject.PlayerCharacter, -1, true);
			TakePrisonerAction.Apply(Settlement.CurrentSettlement.Party, Hero.MainHero);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x0011B694 File Offset: 0x00119894
		private bool game_menu_army_talk_to_other_members_on_condition(MenuCallbackArgs args)
		{
			foreach (MobileParty mobileParty in PlayerEncounter.EncounteredMobileParty.Army.LeaderParty.AttachedParties)
			{
				Hero leaderHero = mobileParty.LeaderHero;
				if (leaderHero != null)
				{
					MenuHelper.SetIssueAndQuestDataForHero(args, leaderHero);
				}
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return !FactionManager.IsAtWarAgainstFaction(MobileParty.MainParty.MapFaction, PlayerEncounter.EncounteredMobileParty.MapFaction) && PlayerEncounter.EncounteredMobileParty.Army.LeaderParty.AttachedParties.Count > 0;
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0011B740 File Offset: 0x00119940
		private void game_menu_army_talk_to_other_members_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("game_menu_army_talk_to_other_members");
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0011B74C File Offset: 0x0011994C
		private void game_menu_army_talk_to_other_members_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.LeaveEncounter)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			args.MenuContext.SetRepeatObjectList(PlayerEncounter.EncounteredMobileParty.Army.LeaderParty.AttachedParties.ToList<MobileParty>());
			if (PlayerEncounter.EncounteredMobileParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction))
			{
				GameMenu.SwitchToMenu("encounter");
			}
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0011B7B0 File Offset: 0x001199B0
		private bool game_menu_army_talk_to_other_members_item_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			MobileParty mobileParty = args.MenuContext.GetCurrentRepeatableObject() as MobileParty;
			MBTextManager.SetTextVariable("CHAR_NAME", (mobileParty != null) ? mobileParty.LeaderHero.Name : null, false);
			if (mobileParty != null && mobileParty.LeaderHero != null)
			{
				MenuHelper.SetIssueAndQuestDataForHero(args, mobileParty.LeaderHero);
			}
			return true;
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0011B80C File Offset: 0x00119A0C
		private void game_menu_army_talk_to_other_members_item_on_consequence(MenuCallbackArgs args)
		{
			MobileParty mobileParty = args.MenuContext.GetSelectedObject() as MobileParty;
			Campaign.Current.CurrentConversationContext = ConversationContext.PartyEncounter;
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(mobileParty.Party), mobileParty.Party, false, false, false, false, false, false);
			if (PartyBase.MainParty.MobileParty.IsCurrentlyAtSea)
			{
				CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
				return;
			}
			CampaignMapConversation.OpenConversation(conversationCharacterData, conversationCharacterData2);
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0011B896 File Offset: 0x00119A96
		private bool game_menu_army_talk_to_other_members_back_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0011B8A1 File Offset: 0x00119AA1
		private void game_menu_army_talk_to_other_members_back_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("army_encounter");
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0011B8AD File Offset: 0x00119AAD
		private bool game_menu_army_attack_on_condition(MenuCallbackArgs args)
		{
			MenuHelper.CheckEnemyAttackableHonorably(args);
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredMobileParty.MapFaction);
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0011B8D8 File Offset: 0x00119AD8
		private void CheckFactionAttackableHonorably(MenuCallbackArgs args, IFaction faction)
		{
			if (faction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = EncounterGameMenuBehavior.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0011B908 File Offset: 0x00119B08
		private void CheckFortificationAttackableHonorably(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				return;
			}
			IFaction mapFaction = PlayerEncounter.EncounterSettlement.MapFaction;
			if (mapFaction != null && mapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = EncounterGameMenuBehavior.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x0011B96A File Offset: 0x00119B6A
		private bool game_menu_army_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0011B975 File Offset: 0x00119B75
		private void game_menu_army_attack_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0011B984 File Offset: 0x00119B84
		private bool game_menu_army_join_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			if (PlayerEncounter.EncounteredMobileParty.MapFaction != MobileParty.MainParty.MapFaction)
			{
				return false;
			}
			if (PlayerEncounter.EncounteredMobileParty.Army == MobileParty.MainParty.Army)
			{
				return false;
			}
			if (PlayerEncounter.EncounteredMobileParty.MapFaction != null)
			{
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.IsAtWarWith(Clan.PlayerClan.MapFaction) && kingdom.NotAttackableByPlayerUntilTime.IsFuture)
					{
						args.IsEnabled = false;
						args.Tooltip = GameTexts.FindText("str_cant_join_army_safe_passage", null);
					}
				}
			}
			return MobileParty.MainParty.Army == null && PlayerEncounter.EncounteredMobileParty.MapFaction == MobileParty.MainParty.MapFaction;
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0011BA74 File Offset: 0x00119C74
		private void game_menu_army_join_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.Army = PlayerEncounter.EncounteredMobileParty.Army;
			MobileParty.MainParty.Army.AddPartyToMergedParties(MobileParty.MainParty);
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0011BAA4 File Offset: 0x00119CA4
		private void army_encounter_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0011BAB8 File Offset: 0x00119CB8
		private void game_menu_encounter_leave_on_consequence(MenuCallbackArgs args)
		{
			Settlement besiegedSettlement = MobileParty.MainParty.BesiegedSettlement;
			if (besiegedSettlement != null && besiegedSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall)
			{
				TextObject textObject = new TextObject("{=h3YuHSRb}Are you sure you want to abandon the siege?", null);
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision", null).ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					MenuHelper.EncounterLeaveConsequence();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MapEvent battle = PlayerEncounter.Battle;
				if (battle != null && battle.MapEventSettlement != null && battle.MapEventSettlement.IsVillage && battle.MapEventSettlement.LastAttackerParty != null)
				{
					PartyBase leaderParty = battle.DefenderSide.LeaderParty;
					if (battle.MapEventSettlement.LastAttackerParty == leaderParty.MobileParty && !leaderParty.MobileParty.IsCurrentlyAtSea)
					{
						leaderParty.MobileParty.Ai.ForceDefaultBehaviorUpdate();
					}
				}
			}
			MenuHelper.EncounterLeaveConsequence();
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0011BBD8 File Offset: 0x00119DD8
		private void game_menu_encounter_abandon_on_consequence(MenuCallbackArgs args)
		{
			((PlayerEncounter.Battle != null) ? PlayerEncounter.Battle : PlayerEncounter.EncounteredBattle).BeginWait();
			MobileParty.MainParty.SetMoveModeHold();
			Hero.MainHero.PartyBelongedTo.Army = null;
			PlayerEncounter.Finish(true);
			if (MobileParty.MainParty.BesiegerCamp != null)
			{
				MobileParty.MainParty.BesiegerCamp = null;
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0011BC34 File Offset: 0x00119E34
		private bool game_menu_encounter_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return MobileParty.MainParty.IsInRaftState || (MobileParty.MainParty.MapEvent != null && !MapEventHelper.CanMainPartyLeaveBattleCommonCondition() && PartyBase.MainParty.Side == BattleSideEnum.Defender && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == MobileParty.MainParty.Party.NumberOfHealthyMembers) || (Hero.MainHero.IsWounded && !MobilePartyHelper.CanPartyAttackWithCurrentMorale(MobileParty.MainParty));
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0011BCB6 File Offset: 0x00119EB6
		private void game_menu_encounter_surrender_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.PlayerSurrender = true;
			PlayerEncounter.Update();
			if (!Hero.MainHero.CanBecomePrisoner())
			{
				GameMenu.ActivateGameMenu("menu_captivity_end_no_more_enemies");
			}
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0011BCD9 File Offset: 0x00119ED9
		private bool game_menu_encounter_attack_on_condition(MenuCallbackArgs args)
		{
			MenuHelper.CheckEnemyAttackableHonorably(args);
			return MenuHelper.EncounterAttackCondition(args);
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0011BCE7 File Offset: 0x00119EE7
		private bool game_menu_encounter_capture_the_enemy_on_condition(MenuCallbackArgs args)
		{
			return MenuHelper.EncounterCaptureEnemyCondition(args);
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0011BCEF File Offset: 0x00119EEF
		private void game_menu_encounter_attack_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterAttackConsequence(args);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0011BCF7 File Offset: 0x00119EF7
		private void game_menu_capture_the_enemy_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterCaptureTheEnemyOnConsequence(args);
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0011BCFF File Offset: 0x00119EFF
		private bool game_menu_encounter_order_attack_on_condition(MenuCallbackArgs args)
		{
			return MenuHelper.EncounterOrderAttackCondition(args);
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0011BD07 File Offset: 0x00119F07
		private void game_menu_encounter_order_attack_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterOrderAttackConsequence(args);
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0011BD10 File Offset: 0x00119F10
		private bool game_menu_encounter_leave_your_soldiers_behind_on_condition(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.IsInRaftState)
			{
				return false;
			}
			if (PartyBase.MainParty.Side == BattleSideEnum.Defender && PlayerEncounter.Battle.DefenderSide.LeaderParty == PartyBase.MainParty && !MobileParty.MainParty.MapEvent.HasWinner)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
				TextObject textObject;
				if (!Campaign.Current.Models.TroopSacrificeModel.CanPlayerGetAwayFromEncounter(out textObject))
				{
					args.Tooltip = textObject;
					args.IsEnabled = false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0011BD90 File Offset: 0x00119F90
		private void game_menu_leave_soldiers_behind_on_init(MenuCallbackArgs args)
		{
			Hero heroWithHighestSkill = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Tactics);
			int num = ((heroWithHighestSkill != null) ? heroWithHighestSkill.GetSkillValue(DefaultSkills.Tactics) : 0);
			MBTextManager.SetTextVariable("HIGHEST_TACTICS_SKILL", num);
			MBTextManager.SetTextVariable("HIGHEST_TACTICS_SKILLED_MEMBER", (heroWithHighestSkill != null && heroWithHighestSkill != Hero.MainHero) ? heroWithHighestSkill.Name : GameTexts.FindText("str_you", null), false);
			int numberOfTroopsSacrificedForTryingToGetAway = Campaign.Current.Models.TroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle);
			MBTextManager.SetTextVariable("NEEEDED_MEN_COUNT", numberOfTroopsSacrificedForTryingToGetAway);
			TextObject textObject = new TextObject("{=loPnK14T}As the highest tactics skilled member {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devise a plan to disperse into the wilderness to break away from your enemies. You and most men may escape with your lives, but as many as {NEEEDED_MEN_COUNT} {?NEEEDED_MEN_COUNT<=1}soldier{?}soldiers{\\?} may be lost and part of your baggage could be captured.", null);
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				textObject = new TextObject("{=VTQ2kwmg}As the party member with the highest skill in tactics, {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devise a plan to send a ship with a skeleton crew to delay the enemy while the rest of you row hard for safety. Most of you will escape, but as many as {NEEEDED_MEN_COUNT} {?NEEEDED_MEN_COUNT<=1}troop{?}troops{\\?} and part of your baggage will be captured. Your fleet will also be suffering losses, with {CAPTURED_SHIPS_NUMBER} {?CAPTURED_SHIPS_NUMBER==1}ship{?}ships{\\?} captured and others taking damage.", null);
				MBList<Ship> mblist;
				Ship ship;
				float num2;
				Campaign.Current.Models.TroopSacrificeModel.GetShipsToSacrificeForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle, out mblist, out ship, out num2);
				textObject.SetTextVariable("CAPTURED_SHIPS_NUMBER", mblist.AnyQ<Ship>() ? mblist.Count : 1);
			}
			MBTextManager.SetTextVariable("TRY_TO_GET_AWAY_TEXT", textObject, false);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0011BE9A File Offset: 0x0011A09A
		public static void game_request_entry_to_castle_approved_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle");
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0011BEA6 File Offset: 0x0011A0A6
		public static bool game_request_entry_to_castle_approved_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x0011BEB1 File Offset: 0x0011A0B1
		public static void game_request_entry_to_castle_rejected_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle_outside");
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x0011BEBD File Offset: 0x0011A0BD
		public static void menu_castle_entry_denied_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x0011BEC0 File Offset: 0x0011A0C0
		private void game_menu_encounter_leave_your_soldiers_behind_accept_on_consequence(MenuCallbackArgs args)
		{
			int numberOfTroopsSacrificedForTryingToGetAway = Campaign.Current.Models.TroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle);
			this.RemoveTroopsForTryToGetAway(numberOfTroopsSacrificedForTryingToGetAway);
			this.CalculateAndRemoveItemsForTryToGetAway();
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MBList<Ship> mblist;
				Ship ship;
				float num;
				Campaign.Current.Models.TroopSacrificeModel.GetShipsToSacrificeForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle, out mblist, out ship, out num);
				if (mblist.Any<Ship>())
				{
					this.CaptureShipsForTryToGetAway(mblist);
				}
				if (ship != null)
				{
					this.DamageLastShipToTakeTryToGetAway(ship, num);
				}
			}
			CampaignEventDispatcher.Instance.OnPlayerDesertedBattle(numberOfTroopsSacrificedForTryingToGetAway);
			if (MobileParty.MainParty.BesiegerCamp != null)
			{
				MobileParty.MainParty.BesiegerCamp = null;
			}
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.SwitchToMenu("try_to_get_away_debrief");
				return;
			}
			GameMenu.ActivateGameMenu("try_to_get_away_debrief");
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0011BF90 File Offset: 0x0011A190
		private void RemoveTroopsForTryToGetAway(int numberOfTroopsToRemove)
		{
			int num = MobileParty.MainParty.Party.NumberOfRegularMembers;
			if (MobileParty.MainParty.Army != null)
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					num += mobileParty.Party.NumberOfRegularMembers;
				}
			}
			float num2 = (float)numberOfTroopsToRemove / (float)num;
			this.SacrificeTroopsWithRatio(MobileParty.MainParty, num2);
			if (MobileParty.MainParty.Army != null)
			{
				foreach (MobileParty mobileParty2 in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					this.SacrificeTroopsWithRatio(mobileParty2, num2);
				}
			}
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0011C088 File Offset: 0x0011A288
		private void SacrificeTroopsWithRatio(MobileParty mobileParty, float sacrificeRatio)
		{
			int num = MathF.Floor((float)mobileParty.Party.NumberOfRegularMembers * sacrificeRatio);
			for (int i = 0; i < num; i++)
			{
				float num2 = 100f;
				TroopRosterElement troopRosterElement = mobileParty.Party.MemberRoster.GetTroopRoster().FirstOrDefault<TroopRosterElement>();
				foreach (TroopRosterElement troopRosterElement2 in mobileParty.Party.MemberRoster.GetTroopRoster())
				{
					float num3 = (float)troopRosterElement2.Character.Level - ((troopRosterElement2.WoundedNumber > 0) ? 0.5f : 0f) - MBRandom.RandomFloat * 0.5f;
					if (!troopRosterElement2.Character.IsHero && num3 < num2 && troopRosterElement2.Number > 0)
					{
						num2 = num3;
						troopRosterElement = troopRosterElement2;
					}
				}
				mobileParty.MemberRoster.AddToCounts(troopRosterElement.Character, -1, false, (troopRosterElement.WoundedNumber > 0) ? (-1) : 0, 0, true, -1);
			}
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0011C19C File Offset: 0x0011A39C
		private void CalculateAndRemoveItemsForTryToGetAway()
		{
			foreach (ItemRosterElement itemRosterElement in new ItemRoster(PartyBase.MainParty.ItemRoster))
			{
				if (!itemRosterElement.EquipmentElement.Item.NotMerchandise && !itemRosterElement.EquipmentElement.Item.IsBannerItem)
				{
					int num = MathF.Floor((float)itemRosterElement.Amount * 0.15f);
					PartyBase.MainParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num);
				}
			}
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x0011C244 File Offset: 0x0011A444
		private void CaptureShipsForTryToGetAway(MBList<Ship> shipsToCapture)
		{
			MBReadOnlyList<MapEventParty> mbreadOnlyList = PlayerEncounter.Battle.PartiesOnSide(PlayerEncounter.Current.PlayerSide.GetOppositeSide());
			foreach (KeyValuePair<Ship, MapEventParty> keyValuePair in Campaign.Current.Models.BattleRewardModel.DistributeDefeatedPartyShipsAmongWinners(PlayerEncounter.Battle, shipsToCapture, mbreadOnlyList))
			{
				if (keyValuePair.Value != null)
				{
					ChangeShipOwnerAction.ApplyByLooting(keyValuePair.Value.Party, keyValuePair.Key);
				}
				else
				{
					DestroyShipAction.Apply(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x0011C2F0 File Offset: 0x0011A4F0
		private void DamageLastShipToTakeTryToGetAway(Ship ship, float damageToApply)
		{
			float num;
			ship.OnShipDamaged(damageToApply, null, out num);
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0011C308 File Offset: 0x0011A508
		private void try_to_get_away_debrief_init(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			if (!MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MBTextManager.SetTextVariable("TRY_TAKE_AWAY_FINISHED", new TextObject("{=ruU70rFl}You disperse into the shrubs and bushes. The enemies halt and seem to hesitate for a while before resuming their pursuit.", null), false);
				return;
			}
			MBTextManager.SetTextVariable("TRY_TAKE_AWAY_FINISHED", new TextObject("{=AdiAmDvI}You have escaped, but as you row away you look back at the men you left behind, clinging to wreckage in the water in the bitter aftermath of your defeat.", null), false);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0011C356 File Offset: 0x0011A556
		private bool game_menu_try_to_get_away_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0011C361 File Offset: 0x0011A561
		private bool game_menu_try_to_get_away_reject_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0011C36C File Offset: 0x0011A56C
		private bool game_menu_try_to_get_away_accept_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0011C378 File Offset: 0x0011A578
		private void game_menu_try_to_get_away_end(MenuCallbackArgs args)
		{
			foreach (MapEventParty mapEventParty in PlayerEncounter.Battle.PartiesOnSide(BattleSideEnum.Defender))
			{
				if (mapEventParty.Party.MobileParty != null)
				{
					if (mapEventParty.Party.MobileParty.BesiegerCamp != null)
					{
						mapEventParty.Party.MobileParty.BesiegerCamp = null;
					}
					if (mapEventParty.Party.MobileParty.CurrentSettlement != null && mapEventParty.Party == PartyBase.MainParty)
					{
						LeaveSettlementAction.ApplyForParty(mapEventParty.Party.MobileParty);
					}
				}
			}
			PlayerEncounter.Battle.DiplomaticallyFinished = true;
			PlayerEncounter.ProtectPlayerSide(1f);
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x0011C444 File Offset: 0x0011A644
		private bool game_menu_town_besiege_continue_siege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (encounteredParty == null)
			{
				return false;
			}
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			return encounteredBattle != null && encounteredBattle.GetLeaderParty(PartyBase.MainParty.Side) == PartyBase.MainParty && encounteredParty.IsSettlement && encounteredParty.Settlement.IsFortification && FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, encounteredParty.MapFaction) && encounteredParty.Settlement.IsUnderSiege && encounteredParty.Settlement.CurrentSiegeState == Settlement.SiegeState.OnTheWalls;
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x0011C4CD File Offset: 0x0011A6CD
		private void game_menu_town_besiege_continue_siege_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Battle != null)
			{
				PlayerEncounter.Finish(true);
			}
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0011C4E4 File Offset: 0x0011A6E4
		private bool game_menu_village_hostile_action_on_condition(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement == null || !Settlement.CurrentSettlement.IsVillage)
			{
				return false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Raid;
			MapEvent battle = PlayerEncounter.Battle;
			if (PartyBase.MainParty.Side == BattleSideEnum.Attacker)
			{
				return !battle.PartiesOnSide(BattleSideEnum.Defender).Any<MapEventParty>((MapEventParty party) => party.Party.NumberOfHealthyMembers > 0);
			}
			return false;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0011C554 File Offset: 0x0011A754
		private void game_menu_village_raid_no_resist_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			Settlement.CurrentSettlement.Militia = 0f;
			if (PlayerEncounter.Current != null)
			{
				if (PlayerEncounter.InsideSettlement)
				{
					PlayerEncounter.LeaveSettlement();
				}
				GameMenu.ActivateGameMenu("raiding_village");
			}
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0011C5A1 File Offset: 0x0011A7A1
		private void game_menu_village_force_supplies_no_resist_loot_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyMinorCoercionHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			GameMenu.ActivateGameMenu("force_supplies_village");
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x0011C5C1 File Offset: 0x0011A7C1
		private void game_menu_village_force_volunteers_no_resist_loot_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyMajorCoercionHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			GameMenu.ActivateGameMenu("force_volunteers_village");
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x0011C5E1 File Offset: 0x0011A7E1
		private void game_menu_taken_prisoner_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0011C5E3 File Offset: 0x0011A7E3
		private bool game_menu_taken_prisoner_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x0011C5EE File Offset: 0x0011A7EE
		private void game_menu_taken_prisoner_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0011C5F8 File Offset: 0x0011A7F8
		private void game_menu_encounter_meeting_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current == null || ((PlayerEncounter.Battle == null || PlayerEncounter.Battle.AttackerSide.LeaderParty == PartyBase.MainParty || PlayerEncounter.Battle.DefenderSide.LeaderParty == PartyBase.MainParty) && !PlayerEncounter.MeetingDone))
			{
				PlayerEncounter.DoMeeting();
				return;
			}
			if (PlayerEncounter.LeaveEncounter)
			{
				PlayerEncounter.Finish(true);
				MobileParty.MainParty.SetMoveModeHold();
				return;
			}
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
			}
			if (PlayerEncounter.BattleChallenge)
			{
				GameMenu.SwitchToMenu("duel_starter_menu");
				return;
			}
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0011C68D File Offset: 0x0011A88D
		private void VillageOutsideOnInit(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0011C69C File Offset: 0x0011A89C
		private void game_menu_town_outside_on_init(MenuCallbackArgs args)
		{
			Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
			args.MenuTitle = encounterSettlement.Name;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(encounterSettlement, out this._accessDetails);
			SettlementAccessModel.AccessLevel accessLevel = this._accessDetails.AccessLevel;
			int num = (int)accessLevel;
			TextObject textObject;
			if (num != 0)
			{
				if (num == 1)
				{
					if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
					{
						textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
						textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
						goto IL_0140;
					}
				}
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.HostileFaction)
			{
				if (encounterSettlement.InRebelliousState)
				{
					textObject = GameTexts.FindText("str_gate_down_enemy_text_castle_low_loyalty", null);
					textObject.SetTextVariable("FACTION_INFORMAL_NAME", encounterSettlement.MapFaction.InformalName);
					goto IL_0140;
				}
				textObject = GameTexts.FindText("str_gate_down_enemy_text_castle", null);
				goto IL_0140;
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
			{
				textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
				textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
				goto IL_0140;
			}
			if (encounterSettlement.InRebelliousState)
			{
				textObject = GameTexts.FindText("str_settlement_not_allowed_text_low_loyalty", null);
				textObject.SetTextVariable("FACTION_INFORMAL_NAME", encounterSettlement.MapFaction.InformalName);
			}
			else
			{
				textObject = GameTexts.FindText("str_settlement_not_allowed_text", null);
			}
			IL_0140:
			textObject.SetTextVariable("SETTLEMENT_NAME", encounterSettlement.EncyclopediaLinkWithName);
			textObject.SetTextVariable("FACTION_TERM", encounterSettlement.MapFaction.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("TOWN_TEXT", textObject, false);
			if (this._accessDetails.PreliminaryActionObligation == SettlementAccessModel.PreliminaryActionObligation.Optional && this._accessDetails.PreliminaryActionType == SettlementAccessModel.PreliminaryActionType.FaceCharges)
			{
				GameMenu.SwitchToMenu("town_inside_criminal");
				return;
			}
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess && this._accessDetails.AccessMethod == SettlementAccessModel.AccessMethod.Direct)
			{
				GameMenu.SwitchToMenu("town");
			}
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x0011C86C File Offset: 0x0011AA6C
		private void game_menu_fortification_high_crime_rating_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=DdeIg5hz}As you move through the streets, you hear whispers of an upcoming war between your faction and {SETTLEMENT_FACTION}. Upon hearing this, you slink away without attracting any suspicion.", null);
			textObject.SetTextVariable("SETTLEMENT_FACTION", Settlement.CurrentSettlement.MapFaction.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("FORTIFICATION_CRIME_RATING_TEXT", textObject, false);
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x0011C8AC File Offset: 0x0011AAAC
		private bool game_menu_fortification_high_crime_rating_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0011C8B8 File Offset: 0x0011AAB8
		private void game_menu_army_left_settlement_due_to_war_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=Nsb6SD4y}After receiving word of an upcoming war against {ENEMY_FACTION}, {ARMY_NAME} decided to leave {SETTLEMENT_NAME}.", null);
			textObject.SetTextVariable("ENEMY_FACTION", Settlement.CurrentSettlement.MapFaction.EncyclopediaLinkWithName);
			textObject.SetTextVariable("ARMY_NAME", MobileParty.MainParty.Army.Name);
			textObject.SetTextVariable("SETTLEMENT_NAME", Settlement.CurrentSettlement.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("ARMY_LEFT_SETTLEMENT_DUE_TO_WAR_TEXT", textObject, false);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x0011C929 File Offset: 0x0011AB29
		private bool game_menu_army_left_settlement_due_to_war_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x0011C934 File Offset: 0x0011AB34
		private void game_menu_castle_outside_on_init(MenuCallbackArgs args)
		{
			Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
			args.MenuTitle = encounterSettlement.Name;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(encounterSettlement, out this._accessDetails);
			TextObject textObject = TextObject.GetEmpty();
			SettlementAccessModel.AccessLevel accessLevel = this._accessDetails.AccessLevel;
			int num = (int)accessLevel;
			if (num != 0)
			{
				if (num == 1)
				{
					if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
					{
						textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
						textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
						goto IL_0114;
					}
				}
				if (encounterSettlement.OwnerClan == Hero.MainHero.Clan)
				{
					textObject = GameTexts.FindText("str_castle_text_yours", null);
				}
				else
				{
					textObject = GameTexts.FindText("str_castle_text_1", null);
				}
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.HostileFaction)
			{
				textObject = GameTexts.FindText("str_gate_down_enemy_text_castle", null);
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
			{
				textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
				textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_settlement_not_allowed_text", null);
			}
			IL_0114:
			encounterSettlement.OwnerClan.Leader.SetPropertiesToTextObject(textObject, "LORD");
			textObject.SetTextVariable("FACTION_TERM", encounterSettlement.MapFaction.EncyclopediaLinkWithName);
			textObject.SetTextVariable("SETTLEMENT_NAME", encounterSettlement.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("TOWN_TEXT", textObject, false);
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess && (this._accessDetails.AccessMethod == SettlementAccessModel.AccessMethod.Direct || (this._playerIsAlreadyInCastle && this._accessDetails.AccessMethod == SettlementAccessModel.AccessMethod.ByRequest)))
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			this._playerIsAlreadyInCastle = false;
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0011CAE4 File Offset: 0x0011ACE4
		private void game_menu_army_left_settlement_due_to_war_on_consequence(MenuCallbackArgs args)
		{
			LeaveSettlementAction.ApplyForParty(MobileParty.MainParty.Army.LeaderParty);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0011CAFA File Offset: 0x0011ACFA
		private void game_menu_town_outside_approach_gates_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town_guard");
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0011CB06 File Offset: 0x0011AD06
		private bool game_menu_castle_outside_approach_gates_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0011CB10 File Offset: 0x0011AD10
		private void game_menu_castle_outside_approach_gates_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle_guard");
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0011CB1C File Offset: 0x0011AD1C
		private void game_menu_fortification_high_crime_rating_continue_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0011CB24 File Offset: 0x0011AD24
		private bool outside_menu_criminal_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x0011CB4B File Offset: 0x0011AD4B
		private void outside_menu_criminal_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town_discuss_criminal_surrender");
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0011CB57 File Offset: 0x0011AD57
		private void caught_outside_menu_criminal_on_consequence(MenuCallbackArgs args)
		{
			ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, 10f, true);
			GameMenu.SwitchToMenu("town_inside_criminal");
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0011CB78 File Offset: 0x0011AD78
		private bool caught_outside_menu_enemy_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return Hero.MainHero.MapFaction.IsAtWarWith(Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x0011CB9B File Offset: 0x0011AD9B
		private void caught_outside_menu_enemy_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("taken_prisoner");
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0011CBA8 File Offset: 0x0011ADA8
		private bool game_menu_town_disguise_yourself_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.SneakIn;
			MBTextManager.SetTextVariable("SNEAK_CHANCE", MathF.Round(Campaign.Current.Models.DisguiseDetectionModel.CalculateDisguiseDetectionProbability(Settlement.CurrentSettlement) * 100f));
			return this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && this._accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise;
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x0011CC0C File Offset: 0x0011AE0C
		private void game_menu_town_initial_disguise_yourself_on_consequence(MenuCallbackArgs args)
		{
			if (CampaignTime.Now.IsNightTime)
			{
				GameMenu.SwitchToMenu("disguise_blocked_night_time");
				return;
			}
			GameMenu.SwitchToMenu(this._alreadySneakedSettlements.Contains(Settlement.CurrentSettlement) ? "disguise_not_first_time" : "disguise_first_time");
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x0011CC58 File Offset: 0x0011AE58
		private void game_menu_town_disguise_yourself_on_consequence(MenuCallbackArgs args)
		{
			bool flag = Campaign.Current.Models.DisguiseDetectionModel.CalculateDisguiseDetectionProbability(Settlement.CurrentSettlement) > MBRandom.RandomFloat;
			SkillLevelingManager.OnMainHeroDisguised(flag);
			Campaign.Current.IsMainHeroDisguised = true;
			if (flag)
			{
				GameMenu.SwitchToMenu("menu_sneak_into_town_succeeded");
				return;
			}
			GameMenu.SwitchToMenu("menu_sneak_into_town_caught");
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x0011CCB0 File Offset: 0x0011AEB0
		private bool game_menu_town_town_besiege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.BesiegeTown;
			this.CheckFortificationAttackableHonorably(args);
			return FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction) && PartyBase.MainParty.NumberOfHealthyMembers > 0 && !Settlement.CurrentSettlement.IsUnderSiege;
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0011CD03 File Offset: 0x0011AF03
		private void leave_siege_after_attack_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.BesiegerCamp = null;
			GameMenu.ExitToLast();
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x0011CD15 File Offset: 0x0011AF15
		private bool leave_siege_after_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty;
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x0011CD43 File Offset: 0x0011AF43
		private bool leave_army_after_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0011CD74 File Offset: 0x0011AF74
		private void leave_army_after_attack_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			else
			{
				GameMenu.ExitToLast();
			}
			if (Settlement.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
				PartyBase.MainParty.SetVisualAsDirty();
			}
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0011CDB0 File Offset: 0x0011AFB0
		private void game_menu_town_town_besiege_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			Campaign.Current.SiegeEventManager.StartSiegeEvent(currentSettlement, MobileParty.MainParty);
			PlayerSiege.StartPlayerSiege(BattleSideEnum.Attacker, false, null);
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0011CDF3 File Offset: 0x0011AFF3
		private void continue_siege_after_attack_on_consequence(MenuCallbackArgs args)
		{
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0011CDFA File Offset: 0x0011AFFA
		private bool continue_siege_after_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0011CE05 File Offset: 0x0011B005
		private bool game_menu_town_outside_cheat_enter_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0011CE11 File Offset: 0x0011B011
		private void game_menu_town_outside_enter_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town");
			PlayerEncounter.LocationEncounter.IsInsideOfASettlement = true;
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0011CE28 File Offset: 0x0011B028
		private void game_menu_naval_town_outside_enter_on_consequence()
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.LocationEncounter == null && !PlayerEncounter.EncounterSettlement.IsUnderSiege)
			{
				PlayerEncounter.EnterSettlement();
			}
			if (Settlement.CurrentSettlement.SiegeEvent != null)
			{
				GameMenu.SwitchToMenu("join_siege_event");
			}
			else
			{
				GameMenu.SwitchToMenu("port_menu");
			}
			if (PlayerEncounter.LocationEncounter != null)
			{
				PlayerEncounter.LocationEncounter.IsInsideOfASettlement = true;
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0011CE88 File Offset: 0x0011B088
		private bool game_menu_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				if (!PlayerEncounter.Current.IsPlayerEncounterRestartedForRaid)
				{
					MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
					if (encounteredBattle == null || !encounteredBattle.IsRaid)
					{
						return true;
					}
					MapEvent encounteredBattle2 = PlayerEncounter.EncounteredBattle;
					bool flag;
					if (encounteredBattle2 == null)
					{
						flag = false;
					}
					else
					{
						Settlement mapEventSettlement = encounteredBattle2.MapEventSettlement;
						bool? flag2 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (!flag)
					{
						return true;
					}
				}
				int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount >= minimumNumberOfMenForAttackingVillageViaScene && ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().AnyQ<Ship>())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0011CF42 File Offset: 0x0011B142
		private void game_menu_castle_outside_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0011CF54 File Offset: 0x0011B154
		private void game_menu_town_naval_outside_leave_on_consequence(MenuCallbackArgs args)
		{
			if (!MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MobileParty.MainParty.SetSailAtPosition(PlayerEncounter.EncounterSettlement.PortPosition);
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0011CF7C File Offset: 0x0011B17C
		private bool game_menu_town_guard_request_shelter_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess && this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
			{
				args.Tooltip = new TextObject("{=03DZpTYi}You are a wanted criminal.", null);
				args.IsEnabled = false;
			}
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall" || x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0011D000 File Offset: 0x0011B200
		private void game_menu_request_entry_to_castle_on_consequence(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess)
			{
				this._playerIsAlreadyInCastle = true;
				GameMenu.SwitchToMenu("menu_castle_entry_granted");
				return;
			}
			if (accessDetails.AccessLevel != SettlementAccessModel.AccessLevel.LimitedAccess || accessDetails.LimitedAccessSolution != SettlementAccessModel.LimitedAccessSolution.Bribe)
			{
				GameMenu.SwitchToMenu("menu_castle_entry_denied");
				return;
			}
			if (Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement) > 0)
			{
				GameMenu.SwitchToMenu("castle_enter_bribe");
				return;
			}
			this._playerIsAlreadyInCastle = true;
			GameMenu.SwitchToMenu("menu_castle_entry_granted");
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0011D098 File Offset: 0x0011B298
		private bool game_menu_request_meeting_someone_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.IsRequestMeetingOptionAvailable(Settlement.CurrentSettlement, out flag2, out textObject);
			args.Tooltip = textObject;
			args.IsEnabled = !flag2;
			return flag;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0011D116 File Offset: 0x0011B316
		private void game_menu_request_meeting_someone_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("request_meeting");
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0011D122 File Offset: 0x0011B322
		private void game_menu_town_guard_back_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x0011D14C File Offset: 0x0011B34C
		private static bool game_menu_castle_enter_bribe_pay_bribe_on_condition(MenuCallbackArgs args)
		{
			int bribeToEnterLordsHall = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement);
			MBTextManager.SetTextVariable("AMOUNT", bribeToEnterLordsHall);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			if (Hero.MainHero.Gold < bribeToEnterLordsHall)
			{
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
				args.IsEnabled = false;
			}
			return bribeToEnterLordsHall > 0;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0011D1EC File Offset: 0x0011B3EC
		private void game_menu_castle_enter_bribe_on_consequence(MenuCallbackArgs args)
		{
			int bribeToEnterLordsHall = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement);
			BribeGuardsAction.Apply(Settlement.CurrentSettlement, bribeToEnterLordsHall);
			this._playerIsAlreadyInCastle = true;
			GameMenu.SwitchToMenu("menu_castle_entry_granted");
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0011D230 File Offset: 0x0011B430
		private void game_menu_town_menu_request_meeting_on_init(MenuCallbackArgs args)
		{
			List<Hero> heroesToMeetInTown = TownHelpers.GetHeroesToMeetInTown(Settlement.CurrentSettlement);
			args.MenuContext.SetRepeatObjectList(heroesToMeetInTown);
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0011D254 File Offset: 0x0011B454
		private bool game_menu_request_meeting_with_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			Hero hero = args.MenuContext.GetCurrentRepeatableObject() as Hero;
			if (hero != null)
			{
				StringHelpers.SetCharacterProperties("HERO_TO_MEET", hero.CharacterObject, null, false);
				MenuHelper.SetIssueAndQuestDataForHero(args, hero);
				return true;
			}
			return false;
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0011D29C File Offset: 0x0011B49C
		private void game_menu_town_menu_request_meeting_with_besiegers_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement.SiegeEvent == null)
			{
				if (MobileParty.MainParty.BesiegerCamp == null)
				{
					PlayerSiege.FinalizePlayerSiege();
				}
				if (currentSettlement.IsTown)
				{
					GameMenu.SwitchToMenu("town");
					return;
				}
				if (currentSettlement.IsCastle)
				{
					GameMenu.SwitchToMenu("castle");
					return;
				}
				Debug.FailedAssert("non-fortification under siege", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\EncounterGameMenuBehavior.cs", "game_menu_town_menu_request_meeting_with_besiegers_on_init", 3228);
			}
			List<MobileParty> list = new List<MobileParty>();
			if (currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army != null)
			{
				list.Add(currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army.LeaderParty);
			}
			else
			{
				list.Add(currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty);
			}
			args.MenuContext.SetRepeatObjectList(list.AsReadOnly());
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0011D370 File Offset: 0x0011B570
		private bool game_menu_request_meeting_with_besiegers_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement.SiegeEvent != null)
			{
				MobileParty mobileParty = ((currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army != null) ? currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army.LeaderParty : currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty);
				StringHelpers.SetCharacterProperties("PARTY_LEADER", mobileParty.LeaderHero.CharacterObject, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x0011D3F8 File Offset: 0x0011B5F8
		private string GetMeetingScene(out string sceneLevel)
		{
			string text = GameSceneDataManager.Instance.MeetingScenes.GetRandomElementWithPredicate<MeetingSceneData>((MeetingSceneData x) => x.Culture == Settlement.CurrentSettlement.Culture).SceneID;
			if (string.IsNullOrEmpty(text))
			{
				text = GameSceneDataManager.Instance.MeetingScenes.GetRandomElement<MeetingSceneData>().SceneID;
			}
			sceneLevel = "";
			if (Settlement.CurrentSettlement.IsFortification)
			{
				sceneLevel = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(Settlement.CurrentSettlement.Town.GetWallLevel());
			}
			return text;
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0011D498 File Offset: 0x0011B698
		private void game_menu_request_meeting_with_besiegers_on_consequence(MenuCallbackArgs args)
		{
			string text;
			string meetingScene = this.GetMeetingScene(out text);
			MobileParty mobileParty = (MobileParty)args.MenuContext.GetSelectedObject();
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, PartyBase.MainParty, true, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(mobileParty.Party), mobileParty.Party, false, false, false, false, false, false);
			CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, meetingScene, text, false);
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0011D504 File Offset: 0x0011B704
		private void game_menu_request_meeting_with_on_consequence(MenuCallbackArgs args)
		{
			string text;
			string meetingScene = this.GetMeetingScene(out text);
			Hero hero = (Hero)args.MenuContext.GetSelectedObject();
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, PartyBase.MainParty, false, false, false, false, false, false);
			CharacterObject characterObject = hero.CharacterObject;
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(characterObject, (partyBelongedTo != null) ? partyBelongedTo.Party : null, true, false, false, false, false, false);
			CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, meetingScene, text, false);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0011D574 File Offset: 0x0011B774
		private bool game_meeting_town_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return Settlement.CurrentSettlement.IsTown;
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x0011D588 File Offset: 0x0011B788
		private bool game_meeting_castle_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return Settlement.CurrentSettlement.IsCastle;
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0011D59C File Offset: 0x0011B79C
		private void game_menu_request_meeting_town_leave_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSiegeEvent.BesiegedSettlement != Settlement.CurrentSettlement)
			{
				GameMenu.SwitchToMenu("town_guard");
				return;
			}
			GameMenu.ExitToLast();
			PlayerEncounter.LeaveEncounter = false;
			if (Hero.MainHero.CurrentSettlement != null && PlayerSiege.PlayerSiegeEvent == null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.SiegeEvent != null)
			{
				PlayerSiege.StartSiegePreparation();
				return;
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0011D60C File Offset: 0x0011B80C
		private void game_menu_request_meeting_castle_leave_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSiegeEvent.BesiegedSettlement != Settlement.CurrentSettlement)
			{
				GameMenu.SwitchToMenu("castle_guard");
				return;
			}
			GameMenu.ExitToLast();
			PlayerEncounter.LeaveEncounter = false;
			if (Hero.MainHero.CurrentSettlement != null && PlayerSiege.PlayerSiegeEvent == null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.SiegeEvent != null)
			{
				PlayerSiege.StartSiegePreparation();
				return;
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x0011D67C File Offset: 0x0011B87C
		private void game_menu_village_loot_complete_on_init(MenuCallbackArgs args)
		{
			PlayerEncounter.Update();
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0011D683 File Offset: 0x0011B883
		private void game_menu_village_loot_complete_continue_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0011D68B File Offset: 0x0011B88B
		private bool game_menu_village_loot_complete_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x0011D696 File Offset: 0x0011B896
		private void game_menu_raid_interrupted_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0011D6A2 File Offset: 0x0011B8A2
		private bool game_menu_raid_interrupted_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0011D6AD File Offset: 0x0011B8AD
		private void break_out_menu_accept_on_consequence(MenuCallbackArgs args)
		{
			BreakInOutBesiegedSettlementAction.ApplyBreakOut(out this._breakInOutCasualties, out this._breakInOutArmyCasualties, this._isBreakingOutFromPort);
			GameMenu.SwitchToMenu("break_out_debrief_menu");
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0011D6D0 File Offset: 0x0011B8D0
		private bool break_out_menu_accept_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int roundedResultNumber = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent, this._isBreakingOutFromPort).RoundedResultNumber;
			int num = ((mainParty.Army != null && mainParty.Army.LeaderParty == mainParty) ? mainParty.Army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars);
			if (roundedResultNumber > num)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
			}
			return true;
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0011D768 File Offset: 0x0011B968
		private void lift_players_blockade(MenuCallbackArgs args)
		{
			PlayerSiege.PlayerSiegeEvent.DeactivateBlockade();
			List<MapEventParty> list = MobileParty.MainParty.MapEvent.AttackerSide.Parties.ToList<MapEventParty>();
			if (PlayerEncounter.Current == null)
			{
				PlayerEncounter.Init();
			}
			PlayerEncounter.Current.FinalizeBattle();
			PlayerEncounter.Current.SetupFields(MobileParty.MainParty.Party, PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.Party);
			foreach (MapEventParty mapEventParty in list)
			{
				if (mapEventParty.Party != PartyBase.MainParty && mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.CurrentSettlement == null && mapEventParty.Party.MobileParty.AttachedTo == null)
				{
					mapEventParty.Party.MobileParty.SetMoveGoToSettlement(PlayerSiege.PlayerSiegeEvent.BesiegedSettlement, MobileParty.NavigationType.Naval, true);
				}
			}
			GameMenu.SwitchToMenu("menu_siege_strategies");
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0011D870 File Offset: 0x0011BA70
		private void defend_blockade_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current == null)
			{
				PlayerEncounter.Start();
			}
			PlayerEncounter.Current.SetIsBlockadeAttack(true);
			PlayerEncounter.Current.SetupFields(MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty, PartyBase.MainParty);
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0011D8C4 File Offset: 0x0011BAC4
		private void attack_blockade_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.SetIsBlockadeAttack(true);
			PlayerEncounter.Current.SetupFields(PartyBase.MainParty, PlayerEncounter.Current.EncounterSettlementAux.SiegeEvent.BesiegerCamp.LeaderParty.Party);
			PlayerEncounter.StartBattle();
			if (PlayerEncounter.Battle != null && !PlayerEncounter.Battle.IsFinalized)
			{
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			GameMenu.SwitchToMenu("besiegers_lift_the_blockade");
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0011D937 File Offset: 0x0011BB37
		private void break_in_menu_accept_on_consequence(MenuCallbackArgs args)
		{
			BreakInOutBesiegedSettlementAction.ApplyBreakIn(out this._breakInOutCasualties, out this._breakInOutArmyCasualties, MobileParty.MainParty.IsCurrentlyAtSea);
			GameMenu.SwitchToMenu("break_in_debrief_menu");
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0011D960 File Offset: 0x0011BB60
		private bool break_in_menu_accept_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int num = ((siegeEvent != null) ? Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent).RoundedResultNumber : 0);
			Army army = mainParty.Army;
			int num2 = ((army != null) ? army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars);
			if (num > num2)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
			}
			return true;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0011D9E3 File Offset: 0x0011BBE3
		private void break_out_menu_reject_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				GameMenu.SwitchToMenu("menu_siege_strategies");
				return;
			}
			GameMenu.SwitchToMenu("encounter_interrupted_siege_preparations");
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x0011DA01 File Offset: 0x0011BC01
		private bool break_out_menu_reject_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x0011DA0B File Offset: 0x0011BC0B
		private void break_in_menu_reject_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				GameMenu.SwitchToMenu("naval_town_outside");
				return;
			}
			GameMenu.SwitchToMenu("join_siege_event");
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x0011DA2E File Offset: 0x0011BC2E
		private bool break_in_menu_reject_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x0011DA38 File Offset: 0x0011BC38
		private void break_in_menu_on_init(MenuCallbackArgs args)
		{
			this.break_in_out_menu_on_init(args, true);
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x0011DA42 File Offset: 0x0011BC42
		private void break_out_menu_on_init(MenuCallbackArgs args)
		{
			this.break_in_out_menu_on_init(args, false);
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0011DA4C File Offset: 0x0011BC4C
		private void break_in_out_menu_on_init(MenuCallbackArgs args, bool isBreakIn)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			Hero heroWithHighestSkill = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Tactics);
			int num = ((heroWithHighestSkill != null) ? heroWithHighestSkill.GetSkillValue(DefaultSkills.Tactics) : 0);
			MBTextManager.SetTextVariable("HIGHEST_TACTICS_SKILL", num);
			MBTextManager.SetTextVariable("HIGHEST_TACTICS_SKILLED_MEMBER", (heroWithHighestSkill != null && heroWithHighestSkill != Hero.MainHero) ? heroWithHighestSkill.Name : GameTexts.FindText("str_you", null), false);
			int num2 = (isBreakIn ? Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent).RoundedResultNumber : Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent, this._isBreakingOutFromPort).RoundedResultNumber);
			TextObject text = args.MenuContext.GameMenu.GetText();
			if (num2 == 0 && isBreakIn)
			{
				this.break_in_debrief_continue_on_consequence(args);
				return;
			}
			TextObject textObject = TextObject.GetEmpty();
			if (isBreakIn)
			{
				if (MobileParty.MainParty.IsCurrentlyAtSea)
				{
					if (num2 == 0)
					{
						textObject = new TextObject("{=rQGNGtDi}No troops will be lost.", null);
					}
					else
					{
						textObject = new TextObject("{=NUSwsQA9}As the party member with the highest skill in tactics, {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devised a plan to send a ship with a skeleton crew to distract the enemy while the rest of you row hard towards the port through the blockade. Most of you will get through, but as many as {POSSIBLE_CASUALTIES} {?POSSIBLE_CASUALTIES > 1}troops{?}troop{\\?} will be lost.", null);
					}
				}
				else
				{
					textObject = new TextObject("{=Yc3DbHoN}As the party member with the highest skill in tactics, {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devised a plan to distract the besiegers so you can rush the fortress gates, expecting the defenders to let you in. You and most of your men may get through, but as many as {POSSIBLE_CASUALTIES} {?PLURAL}troops{?}troop{\\?} may be lost.", null);
				}
			}
			else if (this._isBreakingOutFromPort)
			{
				if (num2 == 0)
				{
					textObject = new TextObject("{=rQGNGtDi}No troops will be lost.", null);
				}
				else
				{
					textObject = new TextObject("{=zw4XTpAl}As the party member with the highest skill in tactics, {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devised a plan to send a ship with a skeleton crew to delay the enemy while the rest of you row hard for safety. Most of you will escape, but as many as {POSSIBLE_CASUALTIES} {?POSSIBLE_CASUALTIES > 1}troops{?}troop{\\?} will be lost.", null);
				}
			}
			else
			{
				textObject = new TextObject("{=tr9KzBa3}As the party member with the highest skill in tactics, {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devised a plan to fight your way through the attackers to escape from the fortress. You and most of your men may survive, but as many as {POSSIBLE_CASUALTIES} {?PLURAL}troops{?}troop{\\?} may be lost.", null);
			}
			textObject.SetTextVariable("POSSIBLE_CASUALTIES", num2);
			text.SetTextVariable("BREAK_IN_OUT_MENU", textObject);
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x0011DBEC File Offset: 0x0011BDEC
		private void break_in_out_debrief_menu_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			TextObject textObject = new TextObject("{=PHe0oco1}You fought your way through the attackers to reach the gates. The defenders open them quickly to let you through. When the gates are safely closed behind you, you take a quick tally only to see you have lost the following: {CASUALTIES}.{OTHER_CASUALTIES}", null);
			if (this._isBreakingOutFromPort || MobileParty.MainParty.IsCurrentlyAtSea)
			{
				if (Settlement.CurrentSettlement.SiegeEvent.IsBlockadeActive)
				{
					textObject = new TextObject("{=ziPgpjIG}You fought your way through the blockade to reach the port. You have lost the following: {CASUALTIES}.{OTHER_CASUALTIES}", null);
				}
				else
				{
					textObject = new TextObject("{=LJyeexzV}You did not lose any troops since there was no blockade", null);
				}
			}
			if (this._breakInOutCasualties != null)
			{
				textObject.SetTextVariable("CASUALTIES", PartyBaseHelper.PrintRegularTroopCategories(this._breakInOutCasualties));
				if (this._breakInOutArmyCasualties > 0)
				{
					TextObject textObject2 = new TextObject("{=hxnCr8bm} Other parties of your army lost {NUMBER} {?PLURAL}troops{?}troop{\\?}.", null);
					textObject2.SetTextVariable("NUMBER", this._breakInOutArmyCasualties);
					textObject2.SetTextVariable("PLURAL", (this._breakInOutArmyCasualties > 1) ? 1 : 0);
					textObject.SetTextVariable("OTHER_CASUALTIES", textObject2);
				}
				else
				{
					textObject.SetTextVariable("OTHER_CASUALTIES", TextObject.GetEmpty());
				}
			}
			args.MenuContext.GameMenu.GetText().SetTextVariable("BREAK_IN_DEBRIEF", textObject);
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x0011DD00 File Offset: 0x0011BF00
		private void break_out_debrief_continue_on_consequence(MenuCallbackArgs args)
		{
			Settlement besiegedSettlement = PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
			PlayerEncounter.Finish(true);
			besiegedSettlement.Party.SetVisualAsDirty();
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				PlayerSiege.FinalizePlayerSiege();
			}
			PlayerEncounter.ProtectPlayerSide(1f);
			if (this._isBreakingOutFromPort)
			{
				MobileParty.MainParty.SetSailAtPosition(besiegedSettlement.PortPosition);
			}
			this._isBreakingOutFromPort = false;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0011DD60 File Offset: 0x0011BF60
		private void break_in_debrief_continue_on_consequence(MenuCallbackArgs args)
		{
			if (Hero.MainHero.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			if (PlayerSiege.PlayerSiegeEvent == null)
			{
				PlayerSiege.StartPlayerSiege(BattleSideEnum.Defender, false, null);
			}
			if (Hero.MainHero.CurrentSettlement.Party.MapEvent != null)
			{
				GameMenu.SwitchToMenu("join_encounter");
			}
			else
			{
				PlayerEncounter.RestartPlayerEncounter(PartyBase.MainParty, PlayerEncounter.EncounterSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party, false, false);
				PlayerSiege.StartSiegePreparation();
			}
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MobileParty.MainParty.DisembarkToPosition(Settlement.CurrentSettlement.GatePosition);
			}
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0011DDF9 File Offset: 0x0011BFF9
		[GameMenuInitializationHandler("besiegers_lift_the_blockade")]
		[GameMenuInitializationHandler("player_blockade_got_attacked")]
		[GameMenuInitializationHandler("player_blockade_got_attacked")]
		private static void besiegers_lift_the_blockade_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("town_blockade");
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x0011DE0C File Offset: 0x0011C00C
		[GameMenuInitializationHandler("army_encounter")]
		[GameMenuInitializationHandler("game_menu_army_talk_to_other_members")]
		private static void army_encounter_background_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.Army != null)
			{
				args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounteredMobileParty.Army.Kingdom.Culture.EncounterBackgroundMesh);
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_fallback");
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0011DE64 File Offset: 0x0011C064
		[GameMenuInitializationHandler("castle_outside")]
		[GameMenuInitializationHandler("castle_guard")]
		[GameMenuInitializationHandler("town_outside")]
		[GameMenuInitializationHandler("fortification_crime_rating")]
		[GameMenuInitializationHandler("village_outside")]
		[GameMenuInitializationHandler("menu_sneak_into_town_succeeded")]
		[GameMenuInitializationHandler("disguise_first_time")]
		[GameMenuInitializationHandler("disguise_not_first_time")]
		private static void encounter_menu_ui_castle_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x0011DE8D File Offset: 0x0011C08D
		[GameMenuInitializationHandler("menu_castle_taken")]
		[GameMenuInitializationHandler("menu_settlement_taken")]
		[GameMenuInitializationHandler("siege_ended_by_last_conspiracy_kingdom_defeat")]
		private static void encounter_menu_settlement_taken_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x0011DEA0 File Offset: 0x0011C0A0
		[GameMenuInitializationHandler("encounter_meeting")]
		private static void game_menu_encounter_meeting_background_on_init(MenuCallbackArgs args)
		{
			string encounterCultureBackgroundMesh = MenuHelper.GetEncounterCultureBackgroundMesh(PlayerEncounter.EncounteredParty.MapFaction.Culture);
			args.MenuContext.SetBackgroundMeshName(encounterCultureBackgroundMesh);
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0011DECE File Offset: 0x0011C0CE
		[GameMenuInitializationHandler("menu_castle_entry_denied")]
		private static void game_menu_castle_guard_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_guards");
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0011DEE0 File Offset: 0x0011C0E0
		[GameMenuInitializationHandler("break_in_menu")]
		[GameMenuInitializationHandler("break_in_debrief_menu")]
		[GameMenuInitializationHandler("break_out_menu")]
		[GameMenuInitializationHandler("break_out_debrief_menu")]
		[GameMenuInitializationHandler("continue_siege_after_attack")]
		[GameMenuInitializationHandler("siege_attacker_defeated")]
		[GameMenuInitializationHandler("siege_attacker_left")]
		private static void game_menu_siege_background_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_besieging");
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x0011DEF2 File Offset: 0x0011C0F2
		[GameMenuInitializationHandler("castle_enter_bribe")]
		public static void game_menu_castle_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/keep");
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0011DF1E File Offset: 0x0011C11E
		[GameMenuInitializationHandler("naval_encounter_disengaged")]
		public static void game_menu_encounter_naval_disengaged_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_naval");
		}

		// Token: 0x040012E4 RID: 4836
		private static readonly TextObject EnemyNotAttackableTooltip = GameTexts.FindText("str_enemy_not_attackable_tooltip", null);

		// Token: 0x040012E5 RID: 4837
		private TroopRoster _breakInOutCasualties;

		// Token: 0x040012E6 RID: 4838
		private int _breakInOutArmyCasualties;

		// Token: 0x040012E7 RID: 4839
		private SettlementAccessModel.AccessDetails _accessDetails;

		// Token: 0x040012E8 RID: 4840
		private bool _playerIsAlreadyInCastle;

		// Token: 0x040012E9 RID: 4841
		private const float SmugglingCrimeRate = 10f;

		// Token: 0x040012EA RID: 4842
		private bool _isBreakingOutFromPort;

		// Token: 0x040012EB RID: 4843
		private const float RatioOfItemsToRemoveOnTryToGetAway = 0.15f;

		// Token: 0x040012EC RID: 4844
		private List<Settlement> _alreadySneakedSettlements = new List<Settlement>();
	}
}
