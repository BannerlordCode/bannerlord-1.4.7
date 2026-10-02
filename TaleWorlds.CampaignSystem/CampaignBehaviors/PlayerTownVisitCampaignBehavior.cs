using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000431 RID: 1073
	public class PlayerTownVisitCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600447D RID: 17533 RVA: 0x0014E954 File Offset: 0x0014CB54
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenus));
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0014E9A6 File Offset: 0x0014CBA6
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_lastTimeRelationGivenPathfinder", ref this._lastTimeRelationGivenPathfinder);
			dataStore.SyncData<CampaignTime>("_lastTimeRelationGivenWaterDiviner", ref this._lastTimeRelationGivenWaterDiviner);
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0014E9CC File Offset: 0x0014CBCC
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenu("town", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_keep", "{=!}{GO_TO_KEEP_TEXT}", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_go_to_keep_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_go_to_keep_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_arena", "{=CfDlOdTH}Go to the arena", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_go_to_arena_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_arena");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_backstreet", "{=l9sFJawW}Go to the tavern district", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_go_to_tavern_district_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_backstreet");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "manage_production", "{=dgf6q4qB}Manage town", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_manage_town_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "manage_production_cheat", "{=zZ3GqbzC}Manage town (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_manage_town_cheat_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "recruit_volunteers", "{=E31IJyqs}Recruit troops", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_recruit_troops_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_recruit_volunteers_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "trade", "{=GmcgoiGy}Trade", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_trade_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_market_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_smithy", "{=McHsHbH8}Enter smithy", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_craft_item_on_condition), delegate(MenuCallbackArgs x)
			{
				CraftingHelper.OpenCrafting(CraftingTemplate.All[0], null);
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_streets", "{=R5ObSaUN}Take a walk around the town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_streets_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_streets_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_wait_here_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_wait_menus");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_return_to_army", "{=SK43eB6y}Return to Army", new GameMenuOption.OnConditionDelegate(this.game_menu_return_to_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_return_to_army_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_keep", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_lords_hall_go_to_dungeon", "{=etjMHPjQ}Go to dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_go_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_go_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "leave_troops_to_garrison", "{=7J9KNFTz}Donate troops to garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_consequece), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "manage_garrison", "{=QazTA60M}Manage garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "open_stash", "{=xl4K9ecB}Open stash", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_lords_hall", "{=dv2ZNazN}Go to the lord's hall", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_go_to_lords_hall_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_lordshall_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_lords_hall_cheat", "{=!}Go to the lord's hall (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_lords_hall_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_lordshall_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_castle_back", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_keep_dungeon", "{=!}{PRISONER_INTRODUCTION}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_dungeon_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_leave_prisoners", "{=kmsNUfbA}Donate prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_manage_prisoners", "{=VXkL5Ysd}Manage prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison", "{=UnQFawna}Enter the dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_enter_the_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_cheat", "{=KBxajw4c}Enter the dungeon (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_dungeon_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_dungeon_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_keep_dungeon_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_keep");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_keep_bribe", "{=yyz111nn}The guards say that they can't just let anyone in.", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_bribe_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_bribe", "town_keep_bribe_pay", "{=fxEka7Bm}Pay a {AMOUNT}{GOLD_ICON} bribe to enter the keep", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_bribe_pay_bribe_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_bribe_pay_bribe_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_bribe", "town_keep_bribe_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_enemy_town_keep", "{=!}{SCOUT_KEEP_TEXT}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_enemy_keep_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_enemy_town_keep", "settlement_go_back_to_center", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_backstreet", "{=Zwy8JybD}You are in the backstreets. The local tavern seems to be attracting its usual crowd.", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_backstreet_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "town_tavern", "{=qcl3YTPh}Visit the tavern", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.visit_the_tavern_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_tavern_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "sell_all_prisoners", "{=xZIBKK0v}Ransom your prisoners ({RANSOM_AMOUNT}{GOLD_ICON})", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.SellPrisonersCondition), delegate(MenuCallbackArgs x)
			{
				PlayerTownVisitCampaignBehavior.SellAllTransferablePrisoners();
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "sell_some_prisoners", "{=Q8VN9UCq}Choose the prisoners to be ransomed", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.SellPrisonerOneStackCondition), delegate(MenuCallbackArgs x)
			{
				PlayerTownVisitCampaignBehavior.ChooseRansomPrisoners();
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "town_backstreet_back", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_arena", "{=5id9mGrc}You are near the arena. {ADDITIONAL_STRING}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_arena_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "town_enter_arena", "{=YQ3vm6er}Enter the arena", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_enter_the_arena_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_arena_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "town_arena_back", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("settlement_player_unconscious", "{=S5OEsjwg}You slip into unconsciousness. After a little while some of the friendlier locals manage to bring you around. A little confused but without any serious injuries, you resolve to be more careful next time.", null, GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("settlement_player_unconscious", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.settlement_player_unconscious_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("settlement_wait", "{=8AbHxCM8}{CAPTIVITY_TEXT}{newline}Waiting in captivity...", new OnInitDelegate(PlayerTownVisitCampaignBehavior.settlement_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_condition), null, new OnTickDelegate(PlayerTownVisitCampaignBehavior.wait_menu_settlement_wait_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddWaitGameMenu("town_wait_menus", "{=ydbVysqv}You are waiting in {CURRENT_SETTLEMENT}.", new OnInitDelegate(this.game_menu_settlement_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_wait_on_condition), null, delegate(MenuCallbackArgs args, CampaignTime dt)
			{
				this.SwitchToMenuIfThereIsAnInterrupt(args.MenuContext.GameMenu.StringId);
			}, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_wait_menus", "wait_leave", "{=UqDNAZqM}Stop waiting", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
				this.SwitchToMenuIfThereIsAnInterrupt(args.MenuContext.GameMenu.StringId);
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("castle", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_prison", "{=esSm5V6t}Go to the dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_the_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_keep_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_prison_cheat", "{=pa7oiQb1}Go to the dungeon (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_dungeon_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_dungeon_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "manage_garrison", "{=QazTA60M}Manage garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "manage_production", "{=Ll1EJHXF}Manage castle", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_castle_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "open_stash", "{=xl4K9ecB}Open stash", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "leave_troops_to_garrison", "{=7J9KNFTz}Donate troops to garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_consequece), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "take_a_walk_around_the_castle", "{=R92XzKXE}Take a walk around the castle", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_take_a_walk_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_take_a_walk_around_the_castle_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_lords_hall", "{=dv2ZNazN}Go to the lord's hall", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_lords_hall_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_lordshall_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_lords_hall_cheat", "{=dl6YxNTT}Go to the lord's hall (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_lords_hall_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_lordshall_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "town_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_wait_here_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_wait_menus");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_return_to_army", "{=SK43eB6y}Return to Army", new GameMenuOption.OnConditionDelegate(this.game_menu_return_to_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_return_to_army_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("castle_dungeon", "{=!}{PRISONER_INTRODUCTION}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_dungeon_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison_leave_prisoners", "{=kmsNUfbA}Donate prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison_manage_prisoners", "{=VXkL5Ysd}Manage prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison", "{=UnQFawna}Enter the dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_enter_the_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison_cheat", "{=KBxajw4c}Enter the dungeon (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_dungeon_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_dungeon_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_keep_dungeon_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("castle");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "recruit_volunteers", "{=E31IJyqs}Recruit troops", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_recruit_volunteers_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_recruit_volunteers_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "trade", "{=VN4ctHIU}Buy products", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_buy_good_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "village_center", "{=U4azeSib}Take a walk through the lands", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_village_center_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_village_center_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "village_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_wait_here_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_wait_village_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "village_return_to_army", "{=SK43eB6y}Return to Army", new GameMenuOption.OnConditionDelegate(this.game_menu_return_to_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_return_to_army_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "disembark", "{=kVqzLm5o}Disembark", new GameMenuOption.OnConditionDelegate(this.game_menu_village_disembark_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_disembark_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "leave_set_sail", "{=fbCbFqyj}Set Sail", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_set_sail_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_set_sail_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "leave_at_sea", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_leave_at_sea_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_set_sail_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_looted", "disembark", "{=kVqzLm5o}Disembark", new GameMenuOption.OnConditionDelegate(this.game_menu_village_disembark_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_disembark_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_looted", "leave_set_sail", "{=fbCbFqyj}Set Sail", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_set_sail_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_set_sail_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("village_wait_menus", "{=lsBuV9W7}You are waiting in the village.", new OnInitDelegate(this.game_menu_settlement_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_wait_on_condition), null, delegate(MenuCallbackArgs args, CampaignTime dt)
			{
				this.SwitchToMenuIfThereIsAnInterrupt(args.MenuContext.GameMenu.StringId);
			}, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_wait_menus", "wait_leave", "{=UqDNAZqM}Stop waiting", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_stop_waiting_at_village_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("prisoner_wait", "{=!}{CAPTIVITY_TEXT}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_condition), null, new OnTickDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0014F93C File Offset: 0x0014DB3C
		private bool game_menu_village_disembark_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			if (Settlement.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) && Settlement.CurrentSettlement.Village.VillageState != Village.VillageStates.Looted)
			{
				args.Tooltip = new TextObject("{=FRbbKXXY}You cannot disembark to a hostile village.", null);
				args.IsEnabled = false;
			}
			return MobileParty.MainParty.IsCurrentlyAtSea;
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0014F9A0 File Offset: 0x0014DBA0
		private void game_menu_settlement_wait_on_init(MenuCallbackArgs args)
		{
			string text = (PlayerEncounter.EncounterSettlement.IsVillage ? "village" : (PlayerEncounter.EncounterSettlement.IsTown ? "town" : (PlayerEncounter.EncounterSettlement.IsCastle ? "castle" : null)));
			if (text != null)
			{
				PlayerTownVisitCampaignBehavior.UpdateMenuLocations(text);
			}
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = true;
			}
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0014FA10 File Offset: 0x0014DC10
		private static void OpenMissionWithSettingPreviousLocation(string previousLocationId, string missionLocationId)
		{
			Campaign.Current.GameMenuManager.NextLocation = LocationComplex.Current.GetLocationWithId(missionLocationId);
			Campaign.Current.GameMenuManager.PreviousLocation = LocationComplex.Current.GetLocationWithId(previousLocationId);
			PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, null, null, null);
			Campaign.Current.GameMenuManager.NextLocation = null;
			Campaign.Current.GameMenuManager.PreviousLocation = null;
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0014FA8E File Offset: 0x0014DC8E
		private void game_menu_stop_waiting_at_village_on_consequence(MenuCallbackArgs args)
		{
			EnterSettlementAction.ApplyForParty(MobileParty.MainParty, MobileParty.MainParty.LastVisitedSettlement);
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0014FAAE File Offset: 0x0014DCAE
		private static bool continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0014FABC File Offset: 0x0014DCBC
		private static bool game_menu_castle_go_to_the_dungeon_on_condition(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterDungeon(Settlement.CurrentSettlement, out accessDetails);
			if (Settlement.CurrentSettlement.BribePaid < Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterDungeon(Settlement.CurrentSettlement) && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess)
			{
				args.Tooltip = new TextObject("{=aPoS8wOW}You have limited access to Dungeon.", null);
				args.IsEnabled = false;
			}
			if (FactionManager.IsAtWarAgainstFaction(Settlement.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
			{
				args.Tooltip = new TextObject("{=h9i9VXLd}You cannot enter an enemy lord's hall.", null);
				args.IsEnabled = false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0014FBA8 File Offset: 0x0014DDA8
		private static bool game_menu_castle_enter_the_dungeon_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0014FBF8 File Offset: 0x0014DDF8
		private static bool game_menu_castle_go_to_dungeon_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0014FC04 File Offset: 0x0014DE04
		private static bool game_menu_castle_leave_prisoners_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DonatePrisoners;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement.IsFortification)
			{
				if (currentSettlement.Party != null && currentSettlement.Party.PrisonerSizeLimit <= currentSettlement.Party.NumberOfPrisoners)
				{
					args.IsEnabled = false;
					args.Tooltip = GameTexts.FindText("str_dungeon_size_limit_exceeded", null);
					args.Tooltip.SetTextVariable("TROOP_NUMBER", currentSettlement.Party.NumberOfPrisoners);
				}
				return currentSettlement.OwnerClan != Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction;
			}
			return false;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0014FCA0 File Offset: 0x0014DEA0
		private static bool game_menu_castle_manage_prisoners_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ManagePrisoners;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return currentSettlement.OwnerClan == Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction && currentSettlement.IsFortification;
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0014FCE2 File Offset: 0x0014DEE2
		private static void game_menu_castle_leave_prisoners_on_consequence(MenuCallbackArgs args)
		{
			PartyScreenHelper.OpenScreenAsDonatePrisoners();
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0014FCE9 File Offset: 0x0014DEE9
		private static void game_menu_castle_manage_prisoners_on_consequence(MenuCallbackArgs args)
		{
			PartyScreenHelper.OpenScreenAsManagePrisoners();
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0014FCF0 File Offset: 0x0014DEF0
		private static bool game_menu_town_go_to_keep_on_condition(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=XZFQ1Jf6}Go to the keep", null);
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
			{
				args.IsEnabled = false;
				PlayerTownVisitCampaignBehavior.SetLordsHallAccessLimitationReasonText(args, accessDetails);
			}
			else if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise)
			{
				textObject = new TextObject("{=1GPa9aTQ}Scout the keep", null);
				args.Tooltip = new TextObject("{=ubOtRU3u}You have limited access to keep while in disguise.", null);
			}
			MBTextManager.SetTextVariable("GO_TO_KEEP_TEXT", textObject, false);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall" || x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0014FDBC File Offset: 0x0014DFBC
		private static bool game_menu_go_to_tavern_district_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroAccessLocation(Settlement.CurrentSettlement, "tavern", out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "tavern").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0014FE38 File Offset: 0x0014E038
		private static bool game_menu_trade_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Trade, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0014FE78 File Offset: 0x0014E078
		private static bool game_menu_town_recruit_troops_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.RecruitTroops, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0014FEB8 File Offset: 0x0014E0B8
		private static bool game_menu_wait_here_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.WaitInSettlement, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0014FEF5 File Offset: 0x0014E0F5
		private void game_menu_wait_village_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_wait_menus");
			LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0014FF0B File Offset: 0x0014E10B
		private bool game_menu_return_to_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0014FF3C File Offset: 0x0014E13C
		private void game_menu_return_to_army_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("army_wait_at_settlement");
			if (MobileParty.MainParty.CurrentSettlement.IsVillage)
			{
				PlayerEncounter.LeaveSettlement();
				PlayerEncounter.Finish(true);
			}
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0014FF64 File Offset: 0x0014E164
		private static bool game_menu_castle_take_a_walk_on_condition(MenuCallbackArgs args)
		{
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "center").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0014FFB4 File Offset: 0x0014E1B4
		private static void game_menu_town_go_to_keep_on_consequence(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			SettlementAccessModel.AccessLevel accessLevel = accessDetails.AccessLevel;
			int num = (int)accessLevel;
			if (num != 1)
			{
				if (num == 2)
				{
					GameMenu.SwitchToMenu("town_keep");
					return;
				}
			}
			else
			{
				if (accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe)
				{
					GameMenu.SwitchToMenu("town_keep_bribe");
					return;
				}
				if (accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise)
				{
					GameMenu.SwitchToMenu("town_enemy_town_keep");
					return;
				}
			}
			Debug.FailedAssert("invalid LimitedAccessSolution or AccessLevel for town keep", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "game_menu_town_go_to_keep_on_consequence", 483);
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0015003C File Offset: 0x0014E23C
		private static bool game_menu_go_dungeon_on_condition(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterDungeon(Settlement.CurrentSettlement, out accessDetails);
			if (Settlement.CurrentSettlement.BribePaid < Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterDungeon(Settlement.CurrentSettlement) && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess)
			{
				args.Tooltip = new TextObject("{=aPoS8wOW}You have limited access to Dungeon.", null);
				args.IsEnabled = false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x001500F2 File Offset: 0x0014E2F2
		private static void game_menu_go_dungeon_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town_keep_dungeon");
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x001500FE File Offset: 0x0014E2FE
		private static bool back_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x0015010C File Offset: 0x0014E30C
		private static bool visit_the_tavern_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "tavern").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0015015C File Offset: 0x0014E35C
		private static bool game_menu_town_go_to_arena_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroAccessLocation(Settlement.CurrentSettlement, "arena", out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "arena").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x001501D8 File Offset: 0x0014E3D8
		private static bool game_menu_town_enter_the_arena_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.WalkAroundTheArena, out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "arena").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x00150250 File Offset: 0x0014E450
		private static bool game_menu_craft_item_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Craft, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Craft;
			ICraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			CraftingCampaignBehavior.CraftingOrderSlots craftingOrderSlots;
			if (Settlement.CurrentSettlement.IsTown && campaignBehavior != null && campaignBehavior.CraftingOrders != null && campaignBehavior.CraftingOrders.TryGetValue(Settlement.CurrentSettlement.Town, out craftingOrderSlots) && craftingOrderSlots.CustomOrders.Count > 0)
			{
				args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveIssue;
			}
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x001502E4 File Offset: 0x0014E4E4
		public static void wait_menu_prisoner_wait_on_init(MenuCallbackArgs args)
		{
			TextObject text = args.MenuContext.GameMenu.GetText();
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			TextObject textObject;
			if (captiveTimeInDays == 0)
			{
				textObject = GameTexts.FindText("str_prisoner_of_party_menu_text", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_prisoner_of_party_for_days_menu_text", null);
				textObject.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
				textObject.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			}
			textObject.SetTextVariable("PARTY_NAME", (Hero.MainHero.PartyBelongedToAsPrisoner != null) ? Hero.MainHero.PartyBelongedToAsPrisoner.Name : TextObject.GetEmpty());
			text.SetTextVariable("CAPTIVITY_TEXT", textObject);
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0015037F File Offset: 0x0014E57F
		[GameMenuInitializationHandler("settlement_wait")]
		public static void wait_menu_prisoner_settlement_wait_ui_on_init(MenuCallbackArgs args)
		{
			if (Hero.MainHero.IsFemale)
			{
				args.MenuContext.SetBackgroundMeshName("wait_prisoner_female");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_prisoner_male");
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x001503AE File Offset: 0x0014E5AE
		public static bool wait_menu_prisoner_wait_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x001503B4 File Offset: 0x0014E5B4
		public static void wait_menu_prisoner_wait_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			if (captiveTimeInDays == 0)
			{
				return;
			}
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject = GameTexts.FindText("str_prisoner_of_party_for_days_menu_text", null);
			textObject.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
			textObject.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			textObject.SetTextVariable("PARTY_NAME", PlayerCaptivity.CaptorParty.Name);
			text.SetTextVariable("CAPTIVITY_TEXT", textObject);
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0015042C File Offset: 0x0014E62C
		public static void wait_menu_settlement_wait_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			if (captiveTimeInDays == 0)
			{
				return;
			}
			TextObject textObject = (Hero.MainHero.IsPrisoner ? Hero.MainHero.PartyBelongedToAsPrisoner.Settlement.Name : Settlement.CurrentSettlement.Name);
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject2 = GameTexts.FindText("str_prisoner_of_settlement_for_days_menu_text", null);
			textObject2.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
			textObject2.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			textObject2.SetTextVariable("SETTLEMENT_NAME", textObject);
			text.SetTextVariable("CAPTIVITY_TEXT", textObject2);
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x001504C8 File Offset: 0x0014E6C8
		private static bool SellPrisonersCondition(MenuCallbackArgs args)
		{
			if (PartyBase.MainParty.PrisonRoster.Count > 0)
			{
				int ransomValueOfAllTransferablePrisoners = PlayerTownVisitCampaignBehavior.GetRansomValueOfAllTransferablePrisoners();
				if (ransomValueOfAllTransferablePrisoners > 0)
				{
					MBTextManager.SetTextVariable("RANSOM_AMOUNT", ransomValueOfAllTransferablePrisoners);
					args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x00150507 File Offset: 0x0014E707
		private static bool SellPrisonerOneStackCondition(MenuCallbackArgs args)
		{
			if (PartyBase.MainParty.PrisonRoster.Count > 0)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
				return true;
			}
			return false;
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x00150528 File Offset: 0x0014E728
		private static int GetRansomValueOfAllTransferablePrisoners()
		{
			int num = 0;
			List<string> list = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyPrisonerLocks().ToList<string>();
			foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.PrisonRoster.GetTroopRoster())
			{
				if (!list.Contains(troopRosterElement.Character.StringId))
				{
					num += Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(troopRosterElement.Character, Hero.MainHero) * troopRosterElement.Number;
				}
			}
			return num;
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x001505D4 File Offset: 0x0014E7D4
		private static void ChooseRansomPrisoners()
		{
			GameMenu.SwitchToMenu("town_backstreet");
			PartyScreenHelper.OpenScreenAsRansom();
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x001505E5 File Offset: 0x0014E7E5
		private static void SellAllTransferablePrisoners()
		{
			SellPrisonersAction.ApplyForSelectedPrisoners(PartyBase.MainParty, null, MobilePartyHelper.GetPlayerPrisonersPlayerCanSell());
			GameMenu.SwitchToMenu("town_backstreet");
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x00150604 File Offset: 0x0014E804
		private static bool game_menu_castle_go_to_lords_hall_on_condition(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
			{
				args.IsEnabled = false;
				PlayerTownVisitCampaignBehavior.SetLordsHallAccessLimitationReasonText(args, accessDetails);
			}
			else if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe && Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement) > Hero.MainHero.Gold)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
			}
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x001506D8 File Offset: 0x0014E8D8
		private static void SetLordsHallAccessLimitationReasonText(MenuCallbackArgs args, SettlementAccessModel.AccessDetails accessDetails)
		{
			SettlementAccessModel.AccessLimitationReason accessLimitationReason = accessDetails.AccessLimitationReason;
			if (accessLimitationReason == SettlementAccessModel.AccessLimitationReason.HostileFaction)
			{
				args.Tooltip = new TextObject("{=h9i9VXLd}You cannot enter an enemy lord's hall.", null);
				return;
			}
			if (accessLimitationReason != SettlementAccessModel.AccessLimitationReason.LocationEmpty)
			{
				Debug.FailedAssert(string.Format("{0} is not a valid no access reason for lord's hall", accessDetails.AccessLimitationReason), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "SetLordsHallAccessLimitationReasonText", 732);
				return;
			}
			args.Tooltip = new TextObject("{=cojKmfSk}There is no one inside.", null);
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x00150744 File Offset: 0x0014E944
		private static bool game_menu_town_keep_go_to_lords_hall_on_condition(MenuCallbackArgs args)
		{
			if (FactionManager.IsAtWarAgainstFaction(Settlement.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
			{
				args.Tooltip = new TextObject("{=h9i9VXLd}You cannot enter an enemy lord's hall.", null);
				args.IsEnabled = false;
			}
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x001507C8 File Offset: 0x0014E9C8
		private static bool game_menu_town_keep_bribe_pay_bribe_on_condition(MenuCallbackArgs args)
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

		// Token: 0x060044AB RID: 17579 RVA: 0x00150865 File Offset: 0x0014EA65
		private static bool game_menu_castle_go_to_lords_hall_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x00150871 File Offset: 0x0014EA71
		private static void game_menu_castle_take_a_walk_around_the_castle_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("center"), null, null, null);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x00150898 File Offset: 0x0014EA98
		private static void game_menu_town_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, false);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			MobileParty garrisonParty = Settlement.CurrentSettlement.Town.GarrisonParty;
			if (garrisonParty != null && garrisonParty.MemberRoster.Count <= 0)
			{
				MobileParty garrisonParty2 = Settlement.CurrentSettlement.Town.GarrisonParty;
				if (garrisonParty2 != null && garrisonParty2.PrisonRoster.Count <= 0)
				{
					DestroyPartyAction.Apply(null, Settlement.CurrentSettlement.Town.GarrisonParty);
				}
			}
			args.MenuTitle = new TextObject("{=mVKcvY2U}Town Center", null);
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0015094C File Offset: 0x0014EB4C
		private static void UpdateMenuLocations(string menuID)
		{
			Campaign.Current.GameMenuManager.MenuLocations.Clear();
			Settlement settlement = ((Settlement.CurrentSettlement == null) ? MobileParty.MainParty.CurrentSettlement : Settlement.CurrentSettlement);
			uint num = <PrivateImplementationDetails>.ComputeStringHash(menuID);
			if (num <= 1579208614U)
			{
				if (num <= 1192893027U)
				{
					if (num != 864577349U)
					{
						if (num != 983285761U)
						{
							if (num != 1192893027U)
							{
								goto IL_03D2;
							}
							if (!(menuID == "village"))
							{
								goto IL_03D2;
							}
							Campaign.Current.GameMenuManager.MenuLocations.AddRange(settlement.LocationComplex.GetListOfLocations());
							return;
						}
						else
						{
							if (!(menuID == "castle_enter_bribe"))
							{
								goto IL_03D2;
							}
							return;
						}
					}
					else
					{
						if (!(menuID == "town"))
						{
							goto IL_03D2;
						}
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("center"));
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("arena"));
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("house_1"));
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("house_2"));
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("house_3"));
						return;
					}
				}
				else if (num != 1470125011U)
				{
					if (num != 1556184416U)
					{
						if (num != 1579208614U)
						{
							goto IL_03D2;
						}
						if (!(menuID == "town_backstreet"))
						{
							goto IL_03D2;
						}
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("tavern"));
						return;
					}
					else if (!(menuID == "castle_dungeon"))
					{
						goto IL_03D2;
					}
				}
				else
				{
					if (!(menuID == "town_keep"))
					{
						goto IL_03D2;
					}
					Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("lordshall"));
					return;
				}
			}
			else if (num <= 2781132822U)
			{
				if (num != 1924483413U)
				{
					if (num != 2596447321U)
					{
						if (num != 2781132822U)
						{
							goto IL_03D2;
						}
						if (!(menuID == "town_keep_dungeon"))
						{
							goto IL_03D2;
						}
					}
					else
					{
						if (!(menuID == "castle"))
						{
							goto IL_03D2;
						}
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("center"));
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("lordshall"));
						return;
					}
				}
				else if (!(menuID == "town_enemy_town_keep"))
				{
					goto IL_03D2;
				}
			}
			else if (num != 4029725827U)
			{
				if (num != 4147432362U)
				{
					if (num != 4246693001U)
					{
						goto IL_03D2;
					}
					if (!(menuID == "town_arena"))
					{
						goto IL_03D2;
					}
					Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("arena"));
					return;
				}
				else
				{
					if (!(menuID == "town_keep_bribe"))
					{
						goto IL_03D2;
					}
					return;
				}
			}
			else
			{
				if (!(menuID == "town_center"))
				{
					goto IL_03D2;
				}
				Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("center"));
				Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("arena"));
				return;
			}
			Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("prison"));
			return;
			IL_03D2:
			Debug.FailedAssert("Could not get the associated locations for Game Menu: " + menuID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "UpdateMenuLocations", 871);
			Campaign.Current.GameMenuManager.MenuLocations.AddRange(settlement.LocationComplex.GetListOfLocations());
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x00150D69 File Offset: 0x0014EF69
		private static void town_keep_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, true);
			args.MenuTitle = new TextObject("{=723ig40Q}Keep", null);
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x00150DA8 File Offset: 0x0014EFA8
		private static void town_enemy_keep_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, true);
			TextObject text = args.MenuContext.GameMenu.GetText();
			MobileParty garrisonParty = Settlement.CurrentSettlement.Town.GarrisonParty;
			bool flag = (garrisonParty != null && garrisonParty.PrisonRoster.TotalHeroes > 0) || Settlement.CurrentSettlement.Party.PrisonRoster.TotalHeroes > 0;
			text.SetTextVariable("SCOUT_KEEP_TEXT", flag ? "{=Tfb9LNAr}You have observed the comings and goings of the guards at the keep. You think you've identified a guard who might be approached and offered a bribe." : "{=qGUrhBpI}After spending time observing the keep and eavesdropping on the guards, you conclude that there are no prisoners here who you can liberate.");
			args.MenuTitle = new TextObject("{=723ig40Q}Keep", null);
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x00150E5C File Offset: 0x0014F05C
		private static void town_keep_dungeon_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			args.MenuTitle = new TextObject("{=x04UGQDn}Dungeon", null);
			TextObject textObject;
			if (Settlement.CurrentSettlement.SettlementComponent.GetPrisonerHeroes().Count == 0)
			{
				textObject = new TextObject("{=O4flV28Q}There are no prisoners here.", null);
			}
			else
			{
				int count = Settlement.CurrentSettlement.SettlementComponent.GetPrisonerHeroes().Count;
				textObject = new TextObject("{=gAc8SWDt}There {?(PRISONER_COUNT > 1)}are {PRISONER_COUNT} prisoners{?}is 1 prisoner{\\?} here.", null);
				textObject.SetTextVariable("PRISONER_COUNT", count);
			}
			MBTextManager.SetTextVariable("PRISONER_INTRODUCTION", textObject, false);
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x00150EF8 File Offset: 0x0014F0F8
		private static void town_keep_bribe_on_init(MenuCallbackArgs args)
		{
			args.MenuTitle = new TextObject("{=723ig40Q}Keep", null);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement) == 0)
			{
				GameMenu.ActivateGameMenu("town_keep");
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x00150F50 File Offset: 0x0014F150
		private static void town_backstreet_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			string text = Settlement.CurrentSettlement.Culture.StringId + "_tavern";
			args.MenuContext.SetBackgroundMeshName(text);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			args.MenuTitle = new TextObject("{=a0MVffcN}Backstreet", null);
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x00150FB4 File Offset: 0x0014F1B4
		private static void town_arena_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown && Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town) != null && Campaign.Current.IsDay)
			{
				TextObject textObject = GameTexts.FindText("str_town_new_tournament_text", null);
				MBTextManager.SetTextVariable("ADDITIONAL_STRING", textObject, false);
			}
			else
			{
				TextObject textObject2 = GameTexts.FindText("str_town_empty_arena_text", null);
				MBTextManager.SetTextVariable("ADDITIONAL_STRING", textObject2, false);
			}
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			args.MenuTitle = new TextObject("{=mMU3H6HZ}Arena", null);
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0015105C File Offset: 0x0014F25C
		public static bool game_menu_town_manage_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			bool flag;
			TextObject textObject;
			return MenuHelper.SetOptionProperties(args, Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(currentSettlement, SettlementAccessModel.SettlementAction.ManageTown, out flag, out textObject), flag, textObject);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x00151099 File Offset: 0x0014F299
		public static bool game_menu_town_manage_town_cheat_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			return GameManagerBase.Current.IsDevelopmentMode && Settlement.CurrentSettlement.IsTown && Settlement.CurrentSettlement.OwnerClan.Leader != Hero.MainHero;
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x001510D8 File Offset: 0x0014F2D8
		private static bool game_menu_town_keep_open_stash_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.OpenStash;
			return Settlement.CurrentSettlement.OwnerClan == Clan.PlayerClan && !Settlement.CurrentSettlement.Town.IsOwnerUnassigned;
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x00151107 File Offset: 0x0014F307
		private static void game_menu_town_keep_open_stash_on_consequence(MenuCallbackArgs args)
		{
			InventoryScreenHelper.OpenScreenAsStash(Settlement.CurrentSettlement.Stash);
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x00151118 File Offset: 0x0014F318
		private static bool game_menu_manage_garrison_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ManageGarrison;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return currentSettlement.OwnerClan == Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction && currentSettlement.IsFortification;
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0015115A File Offset: 0x0014F35A
		private static bool game_menu_manage_castle_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			return Settlement.CurrentSettlement.OwnerClan == Clan.PlayerClan && Settlement.CurrentSettlement.IsCastle;
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x00151184 File Offset: 0x0014F384
		private static void game_menu_manage_garrison_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
			if (currentSettlement.Town.GarrisonParty == null)
			{
				currentSettlement.AddGarrisonParty();
			}
			PartyScreenHelper.OpenScreenAsManageTroops(currentSettlement.Town.GarrisonParty);
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x001511C0 File Offset: 0x0014F3C0
		private static bool game_menu_leave_troops_garrison_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DonateTroops;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return currentSettlement.OwnerClan != Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction && currentSettlement.IsFortification && (currentSettlement.Town.GarrisonParty == null || currentSettlement.Town.GarrisonParty.Party.PartySizeLimit > currentSettlement.Town.GarrisonParty.Party.NumberOfAllMembers);
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0015123F File Offset: 0x0014F43F
		private static void game_menu_leave_troops_garrison_on_consequece(MenuCallbackArgs args)
		{
			PartyScreenHelper.OpenScreenAsDonateGarrisonWithCurrentSettlement();
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x00151248 File Offset: 0x0014F448
		private static bool game_menu_town_town_streets_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroAccessLocation(Settlement.CurrentSettlement, "center", out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "center").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x001512C3 File Offset: 0x0014F4C3
		private static void game_menu_town_town_streets_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("center"), null, null, null);
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x001512E8 File Offset: 0x0014F4E8
		private static void game_menu_town_lordshall_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "lordshall");
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x001512FF File Offset: 0x0014F4FF
		private static void game_menu_castle_lordshall_on_consequence(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "lordshall");
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x00151310 File Offset: 0x0014F510
		private static void game_menu_town_keep_bribe_pay_bribe_on_consequence(MenuCallbackArgs args)
		{
			int bribeToEnterLordsHall = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement);
			BribeGuardsAction.Apply(Settlement.CurrentSettlement, bribeToEnterLordsHall);
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			GameMenu.ActivateGameMenu("town_keep");
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x00151352 File Offset: 0x0014F552
		private static void game_menu_lordshall_cheat_on_consequence(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "lordshall");
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x00151363 File Offset: 0x0014F563
		private static void game_menu_dungeon_cheat_on_consequence(MenuCallbackArgs ARGS)
		{
			GameMenu.SwitchToMenu("castle_dungeon");
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0015136F File Offset: 0x0014F56F
		private static void game_menu_town_dungeon_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "prison");
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x00151386 File Offset: 0x0014F586
		private static void game_menu_castle_dungeon_on_consequence(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "prison");
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x00151397 File Offset: 0x0014F597
		private static void game_menu_keep_dungeon_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle_dungeon");
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x001513A3 File Offset: 0x0014F5A3
		private static void game_menu_town_town_tavern_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "tavern");
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x001513BA File Offset: 0x0014F5BA
		private static void game_menu_town_town_arena_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "arena");
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x001513D1 File Offset: 0x0014F5D1
		private static void game_menu_town_town_market_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Town, InventoryScreenHelper.InventoryCategoryType.None, null);
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x001513F4 File Offset: 0x0014F5F4
		private static bool game_menu_town_town_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return !MobileParty.MainParty.IsCurrentlyAtSea && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty);
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x00151430 File Offset: 0x0014F630
		private static bool game_menu_village_leave_at_sea_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.IsCurrentlyAtSea && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty);
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x0015146C File Offset: 0x0014F66C
		private static bool game_menu_village_set_sail_leave_on_condition(MenuCallbackArgs args)
		{
			if (!MobileParty.MainParty.IsCurrentlyAtSea)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.SetSail;
				if (Settlement.CurrentSettlement.HasPort)
				{
					if (!MobileParty.MainParty.HasNavalNavigationCapability)
					{
						args.Tooltip = new TextObject("{=HUUd7Ohd}You don't own any ships! Go to the town center to leave.", null);
						args.IsEnabled = false;
					}
					else if (Settlement.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) && Settlement.CurrentSettlement.Village.VillageState != Village.VillageStates.Looted)
					{
						args.Tooltip = new TextObject("{=Y9EaZjJm}You cannot set sail from a hostile village.", null);
						args.IsEnabled = false;
					}
					else if (!MobileParty.MainParty.Anchor.IsAtSettlement(Settlement.CurrentSettlement))
					{
						args.Tooltip = new TextObject("{=LmTqrE8x}Your fleet is not docked at this settlement.", null);
						args.IsEnabled = false;
					}
					return MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty;
				}
			}
			return false;
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x00151560 File Offset: 0x0014F760
		private static void game_menu_settlement_leave_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.Position = MobileParty.MainParty.CurrentSettlement.GatePosition;
			if (MobileParty.MainParty.Army != null)
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.AttachedParties)
				{
					mobileParty.Position = MobileParty.MainParty.Position;
				}
			}
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
			Campaign.Current.SaveHandler.SignalAutoSave();
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x00151608 File Offset: 0x0014F808
		private void game_menu_village_set_sail_leave_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.SetSailAtPosition(Settlement.CurrentSettlement.PortPosition);
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0015162E File Offset: 0x0014F82E
		private void game_menu_village_disembark_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.DisembarkToPosition(Settlement.CurrentSettlement.GatePosition);
			MobileParty.MainParty.Anchor.SetSettlement(Settlement.CurrentSettlement);
			PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence(args);
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x00151660 File Offset: 0x0014F860
		private static void settlement_wait_on_init(MenuCallbackArgs args)
		{
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject = (Hero.MainHero.IsPrisoner ? Hero.MainHero.PartyBelongedToAsPrisoner.Settlement.Name : Settlement.CurrentSettlement.Name);
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			TextObject textObject2;
			if (captiveTimeInDays == 0)
			{
				textObject2 = GameTexts.FindText("str_prisoner_of_settlement_menu_text", null);
			}
			else
			{
				textObject2 = GameTexts.FindText("str_prisoner_of_settlement_for_days_menu_text", null);
				textObject2.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
				textObject2.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			}
			textObject2.SetTextVariable("SETTLEMENT_NAME", textObject);
			text.SetTextVariable("CAPTIVITY_TEXT", textObject2);
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x00151708 File Offset: 0x0014F908
		private static void game_menu_village_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, false);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			SettlementAccessModel settlementAccessModel = Campaign.Current.Models.SettlementAccessModel;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			SettlementAccessModel.AccessDetails accessDetails;
			settlementAccessModel.CanMainHeroEnterSettlement(currentSettlement, out accessDetails);
			if (currentSettlement != null && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess && accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.VillageIsLooted)
			{
				GameMenu.SwitchToMenu("village_looted");
			}
			args.MenuTitle = new TextObject("{=Ua6CNLBZ}Village", null);
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x00151784 File Offset: 0x0014F984
		private static void game_menu_village_drop_off_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, false);
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess && accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.VillageIsLooted)
			{
				GameMenu.SwitchToMenu("village_looted");
			}
			args.MenuTitle = new TextObject("{=Ua6CNLBZ}Village", null);
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x001517E4 File Offset: 0x0014F9E4
		private static void game_menu_castle_on_init(MenuCallbackArgs args)
		{
			MobileParty garrisonParty = Settlement.CurrentSettlement.Town.GarrisonParty;
			if (garrisonParty != null && garrisonParty.MemberRoster.Count <= 0)
			{
				DestroyPartyAction.Apply(null, Settlement.CurrentSettlement.Town.GarrisonParty);
			}
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, true);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (Campaign.Current.GameMenuManager.NextLocation != null)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
			}
			args.MenuTitle = new TextObject("{=sVXa3zFx}Castle", null);
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x001518BC File Offset: 0x0014FABC
		private static bool game_menu_village_village_center_on_condition(MenuCallbackArgs args)
		{
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.GetListOfLocations().ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return Settlement.CurrentSettlement.Village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x001518FE File Offset: 0x0014FAFE
		private static void game_menu_village_village_center_on_consequence(MenuCallbackArgs args)
		{
			(PlayerEncounter.LocationEncounter as VillageEncounter).CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("village_center"), null, null, null);
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x00151924 File Offset: 0x0014FB24
		private static bool game_menu_village_buy_good_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Trade, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x00151961 File Offset: 0x0014FB61
		private static void game_menu_recruit_volunteers_on_consequence(MenuCallbackArgs args)
		{
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00151964 File Offset: 0x0014FB64
		private static bool game_menu_recruit_volunteers_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.RecruitTroops, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x001519A1 File Offset: 0x0014FBA1
		private static bool game_menu_village_wait_on_condition(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x001519BD File Offset: 0x0014FBBD
		private static bool game_menu_town_wait_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			MBTextManager.SetTextVariable("CURRENT_SETTLEMENT", Settlement.CurrentSettlement.EncyclopediaLinkWithName, false);
			return true;
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x001519E0 File Offset: 0x0014FBE0
		public static void settlement_player_unconscious_continue_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			GameMenu.SwitchToMenu(currentSettlement.IsVillage ? "village" : (currentSettlement.IsCastle ? "castle" : "town"));
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x00151A20 File Offset: 0x0014FC20
		private static void SetIntroductionText(Settlement settlement, bool fromKeep)
		{
			TextObject textObject;
			if (settlement.IsTown && !fromKeep)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=kXVHwjoV}You have arrived at your fief of {SETTLEMENT_LINK}. {PROSPERITY_INFO} {MORALE_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=UWzQsHA2}{SETTLEMENT_LINK} is governed by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {PROSPERITY_INFO} {MORALE_INFO}", null);
				}
			}
			else if (settlement.IsTown && fromKeep)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=u0Dc5g4Z}You are in your town of {SETTLEMENT_LINK}. {KEEP_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=q3wD0rbq}{SETTLEMENT_LINK} is governed by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {KEEP_INFO}", null);
				}
			}
			else if (settlement.IsCastle)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=dA8RGoQ1}You have arrived at {SETTLEMENT_LINK}. {KEEP_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=4pmvrnmN}The castle of {SETTLEMENT_LINK} is owned by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {KEEP_INFO}", null);
				}
			}
			else if (settlement.IsVillage)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=M5iR1e5h}You have arrived at your fief of {SETTLEMENT_LINK}. {PROSPERITY_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=RVDojUOM}The lands around {SETTLEMENT_LINK} are owned mostly by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {PROSPERITY_INFO}", null);
				}
			}
			else
			{
				Debug.FailedAssert("Couldn't set settlementIntro!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "SetIntroductionText", 1464);
				textObject = TextObject.GetEmpty();
			}
			settlement.OwnerClan.Leader.SetPropertiesToTextObject(textObject, "LORD");
			string text = settlement.OwnerClan.Leader.MapFaction.Culture.StringId;
			if (settlement.OwnerClan.Leader.IsFemale)
			{
				text += "_f";
			}
			if (settlement.OwnerClan.Leader == Hero.MainHero && !Hero.MainHero.MapFaction.IsKingdomFaction)
			{
				textObject.SetTextVariable("FACTION_TERM", Hero.MainHero.Clan.EncyclopediaLinkWithName);
				textObject.SetTextVariable("FACTION_OFFICIAL", new TextObject("{=hb30yQPN}leader", null));
			}
			else
			{
				textObject.SetTextVariable("FACTION_TERM", settlement.MapFaction.EncyclopediaLinkWithName);
				if (settlement.OwnerClan.MapFaction.IsKingdomFaction && settlement.OwnerClan.Leader == settlement.OwnerClan.Leader.MapFaction.Leader)
				{
					textObject.SetTextVariable("FACTION_OFFICIAL", GameTexts.FindText("str_faction_ruler", text));
				}
				else
				{
					textObject.SetTextVariable("FACTION_OFFICIAL", GameTexts.FindText("str_faction_official", text));
				}
			}
			textObject.SetTextVariable("SETTLEMENT_LINK", settlement.EncyclopediaLinkWithName);
			settlement.SetPropertiesToTextObject(textObject, "SETTLEMENT_OBJECT");
			string text2 = settlement.SettlementComponent.GetProsperityLevel().ToString();
			if ((settlement.IsTown && settlement.Town.InRebelliousState) || (settlement.IsVillage && settlement.Village.Bound.Town.InRebelliousState))
			{
				textObject.SetTextVariable("PROSPERITY_INFO", GameTexts.FindText("str_settlement_rebellion", null));
				textObject.SetTextVariable("MORALE_INFO", "");
			}
			else if (settlement.IsTown)
			{
				textObject.SetTextVariable("PROSPERITY_INFO", GameTexts.FindText("str_town_long_prosperity_1", text2));
				textObject.SetTextVariable("MORALE_INFO", PlayerTownVisitCampaignBehavior.SetTownMoraleText(settlement));
			}
			else if (settlement.IsVillage)
			{
				textObject.SetTextVariable("PROSPERITY_INFO", GameTexts.FindText("str_village_long_prosperity", text2));
			}
			textObject.SetTextVariable("KEEP_INFO", "");
			if (fromKeep && LocationComplex.Current != null)
			{
				if (!LocationComplex.Current.GetLocationWithId("lordshall").GetCharacterList().Any<LocationCharacter>((LocationCharacter x) => x.Character.IsHero))
				{
					textObject.SetTextVariable("KEEP_INFO", "{=OgkSLkFi}There is nobody in the lord's hall.");
				}
			}
			MBTextManager.SetTextVariable("SETTLEMENT_INFO", textObject, false);
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x00151D9C File Offset: 0x0014FF9C
		private static TextObject SetTownMoraleText(Settlement settlement)
		{
			SettlementComponent.ProsperityLevel prosperityLevel = settlement.SettlementComponent.GetProsperityLevel();
			string text;
			if (settlement.Town.Loyalty < 25f)
			{
				if (prosperityLevel <= SettlementComponent.ProsperityLevel.Low)
				{
					text = "str_settlement_morale_rebellious_adversity";
				}
				else if (prosperityLevel <= SettlementComponent.ProsperityLevel.Mid)
				{
					text = "str_settlement_morale_rebellious_average";
				}
				else
				{
					text = "str_settlement_morale_rebellious_prosperity";
				}
			}
			else if (settlement.Town.Loyalty < 65f)
			{
				if (prosperityLevel <= SettlementComponent.ProsperityLevel.Low)
				{
				}
				if (prosperityLevel <= SettlementComponent.ProsperityLevel.Mid)
				{
					text = "str_settlement_morale_medium_average";
				}
				else
				{
					text = "str_settlement_morale_medium_prosperity";
				}
			}
			else if (prosperityLevel <= SettlementComponent.ProsperityLevel.Low)
			{
				text = "str_settlement_morale_high_adversity";
			}
			else if (prosperityLevel <= SettlementComponent.ProsperityLevel.Mid)
			{
				text = "str_settlement_morale_high_average";
			}
			else
			{
				text = "str_settlement_morale_high_prosperity";
			}
			return GameTexts.FindText(text, null);
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x00151E42 File Offset: 0x00150042
		[GameMenuInitializationHandler("town_guard")]
		[GameMenuInitializationHandler("menu_tournament_withdraw_verify")]
		[GameMenuInitializationHandler("menu_tournament_bet_confirm")]
		[GameMenuInitializationHandler("town_castle_not_enough_bribe")]
		[GameMenuInitializationHandler("settlement_player_unconscious")]
		[GameMenuInitializationHandler("castle")]
		[GameMenuInitializationHandler("town_castle_nobody_inside")]
		[GameMenuInitializationHandler("encounter_interrupted")]
		[GameMenuInitializationHandler("encounter_interrupted_siege_preparations")]
		[GameMenuInitializationHandler("castle_dungeon")]
		[GameMenuInitializationHandler("encounter_interrupted_raid_started")]
		[GameMenuInitializationHandler("settlement_player_unconscious_when_disguise")]
		public static void game_menu_town_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00151E60 File Offset: 0x00150060
		[GameMenuInitializationHandler("town_arena")]
		public static void game_menu_town_menu_arena_on_init(MenuCallbackArgs args)
		{
			string text = Settlement.CurrentSettlement.Culture.StringId + "_arena";
			args.MenuContext.SetBackgroundMeshName(text);
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_arena");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/arena");
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00151EB4 File Offset: 0x001500B4
		[GameMenuInitializationHandler("village_hostile_action")]
		[GameMenuInitializationHandler("force_volunteers_village")]
		[GameMenuInitializationHandler("force_supplies_village")]
		[GameMenuInitializationHandler("raid_village_no_resist_warn_player")]
		[GameMenuInitializationHandler("raid_village_resisted")]
		[GameMenuInitializationHandler("village_loot_no_resist")]
		[GameMenuInitializationHandler("village_take_food_confirm")]
		[GameMenuInitializationHandler("village_press_into_service_confirm")]
		[GameMenuInitializationHandler("menu_press_into_service_success")]
		[GameMenuInitializationHandler("menu_village_take_food_success")]
		public static void game_menu_village_menu_on_init(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00151EE0 File Offset: 0x001500E0
		[GameMenuInitializationHandler("town_keep")]
		public static void game_menu_town_menu_keep_on_init(MenuCallbackArgs args)
		{
			string text = Settlement.CurrentSettlement.Culture.StringId + "_keep";
			args.MenuContext.SetBackgroundMeshName(text);
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_keep");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/keep");
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x00151F33 File Offset: 0x00150133
		[GameMenuEventHandler("town", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_ui_town_manage_town_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x00151F40 File Offset: 0x00150140
		[GameMenuEventHandler("town_keep", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_ui_town_castle_manage_town_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00151F4D File Offset: 0x0015014D
		[GameMenuEventHandler("village", "trade", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_ui_village_buy_good_on_consequence(MenuCallbackArgs args)
		{
			InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Village, InventoryScreenHelper.InventoryCategoryType.None, null);
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00151F6A File Offset: 0x0015016A
		[GameMenuEventHandler("village", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_ui_village_manage_village_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x00151F77 File Offset: 0x00150177
		[GameMenuEventHandler("village", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town_backstreet", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_ui_recruit_volunteers_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenRecruitVolunteers();
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_recruit");
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x00151F94 File Offset: 0x00150194
		[GameMenuInitializationHandler("prisoner_wait")]
		private static void wait_menu_ui_prisoner_wait_on_init(MenuCallbackArgs args)
		{
			PartyBase partyBelongedToAsPrisoner = Hero.MainHero.PartyBelongedToAsPrisoner;
			bool flag;
			if (partyBelongedToAsPrisoner == null)
			{
				flag = false;
			}
			else
			{
				MobileParty mobileParty = partyBelongedToAsPrisoner.MobileParty;
				bool? flag2 = ((mobileParty != null) ? new bool?(mobileParty.IsCurrentlyAtSea) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				if (Hero.MainHero.IsFemale)
				{
					args.MenuContext.SetBackgroundMeshName("wait_captive_at_sea_female");
					return;
				}
				args.MenuContext.SetBackgroundMeshName("wait_captive_at_sea_male");
				return;
			}
			else
			{
				if (Hero.MainHero.IsFemale)
				{
					args.MenuContext.SetBackgroundMeshName("wait_captive_female");
					return;
				}
				args.MenuContext.SetBackgroundMeshName("wait_captive_male");
				return;
			}
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x00152042 File Offset: 0x00150242
		[GameMenuInitializationHandler("town_backstreet")]
		public static void game_menu_town_menu_backstreet_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/tavern");
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_tavern");
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x00152064 File Offset: 0x00150264
		[GameMenuInitializationHandler("town_enemy_town_keep")]
		[GameMenuInitializationHandler("town_keep_dungeon")]
		[GameMenuInitializationHandler("town_keep_bribe")]
		public static void game_menu_town_menu_keep_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/keep");
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x00152090 File Offset: 0x00150290
		[GameMenuInitializationHandler("town_wait_menus")]
		[GameMenuInitializationHandler("town_wait")]
		public static void game_menu_town_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/city");
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_wait");
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x001520CC File Offset: 0x001502CC
		[GameMenuInitializationHandler("town")]
		public static void game_menu_town_menu_enter_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_city");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/city");
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x00152108 File Offset: 0x00150308
		[GameMenuInitializationHandler("village_wait_menus")]
		public static void game_menu_village_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/village");
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00152144 File Offset: 0x00150344
		[GameMenuInitializationHandler("village")]
		[GameMenuInitializationHandler("village_raid_diplomatically_ended")]
		public static void game_menu_village__enter_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_village");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/village");
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0015218D File Offset: 0x0015038D
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party != null && party.IsMainParty)
			{
				settlement.HasVisited = true;
				if (MBRandom.RandomFloat > 0.5f && (settlement.IsVillage || settlement.IsTown))
				{
					this.CheckPerkAndGiveRelation(party, settlement);
				}
			}
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x001521C8 File Offset: 0x001503C8
		private void CheckPerkAndGiveRelation(MobileParty party, Settlement settlement)
		{
			bool isVillage = settlement.IsVillage;
			bool flag = (isVillage ? party.HasPerk(DefaultPerks.Scouting.WaterDiviner, true) : party.HasPerk(DefaultPerks.Scouting.Pathfinder, true));
			bool flag2 = (isVillage ? (this._lastTimeRelationGivenWaterDiviner.Equals(CampaignTime.Zero) || this._lastTimeRelationGivenWaterDiviner.ElapsedDaysUntilNow >= 1f) : (this._lastTimeRelationGivenPathfinder.Equals(CampaignTime.Zero) || this._lastTimeRelationGivenPathfinder.ElapsedDaysUntilNow >= 1f));
			if (flag && flag2)
			{
				Hero randomElement = settlement.Notables.GetRandomElement<Hero>();
				if (randomElement != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(party.ActualClan.Leader, randomElement, 1, true);
				}
				if (isVillage)
				{
					this._lastTimeRelationGivenWaterDiviner = CampaignTime.Now;
					return;
				}
				this._lastTimeRelationGivenPathfinder = CampaignTime.Now;
			}
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x00152295 File Offset: 0x00150495
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party != null && party.IsMainParty)
			{
				Campaign.Current.IsMainHeroDisguised = false;
			}
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x001522B0 File Offset: 0x001504B0
		private void SwitchToMenuIfThereIsAnInterrupt(string currentMenuId)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (genericStateMenu != currentMenuId)
			{
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.SwitchToMenu(genericStateMenu);
					return;
				}
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x04001374 RID: 4980
		private CampaignTime _lastTimeRelationGivenPathfinder;

		// Token: 0x04001375 RID: 4981
		private CampaignTime _lastTimeRelationGivenWaterDiviner;
	}
}
