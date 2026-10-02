using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042F RID: 1071
	public class PlayerArmyWaitBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004453 RID: 17491 RVA: 0x0014CF04 File Offset: 0x0014B104
		public PlayerArmyWaitBehavior()
		{
			this._leadingArmyDescriptionText = GameTexts.FindText("str_you_are_leading_army", null);
			this._armyDescriptionText = GameTexts.FindText("str_army_of_HERO", null);
			this._disbandingArmyDescriptionText = new TextObject("{=Yan3ZG1w}Disbanding Army!", null);
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0014CF40 File Offset: 0x0014B140
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.TickEvent.AddNonSerializedListener(this, new Action<float>(PlayerArmyWaitBehavior.OnTick));
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0014CF92 File Offset: 0x0014B192
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Army.ArmyDispersionReason>("_playerArmyDispersionReason", ref this._playerArmyDispersionReason);
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0014CFA6 File Offset: 0x0014B1A6
		private void OnSessionLaunched(CampaignGameStarter starter)
		{
			this.AddMenus(starter);
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0014CFB0 File Offset: 0x0014B1B0
		private void AddMenus(CampaignGameStarter starter)
		{
			starter.AddWaitGameMenu("army_wait", "{=0gwQGnm4}{ARMY_OWNER_TEXT} {ARMY_BEHAVIOR}", new OnInitDelegate(this.wait_menu_army_wait_on_init), new OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_wait_on_condition), null, new OnTickDelegate(this.ArmyWaitMenuTick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("army_wait", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_consequence), true, -1, false, null);
			starter.AddGameMenuOption("army_wait", "abandon_army", "{=0vnegjxf}Abandon Army", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_abandon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_abandon_on_consequence), true, -1, false, null);
			starter.AddWaitGameMenu("army_wait_at_settlement", "{=0gwQGnm4}{ARMY_OWNER_TEXT} {ARMY_BEHAVIOR}", new OnInitDelegate(this.wait_menu_army_wait_at_settlement_on_init), new OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_wait_on_condition), null, new OnTickDelegate(PlayerArmyWaitBehavior.wait_menu_army_wait_at_settlement_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("army_wait_at_settlement", "enter_settlement", "{=!}{ENTER_SETTLEMENT}", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_enter_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_enter_settlement_on_consequence), false, -1, false, null);
			starter.AddGameMenuOption("army_wait_at_settlement", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_consequence), true, -1, false, null);
			starter.AddGameMenu("army_dispersed", "{=!}{ARMY_DISPERSE_REASON}", new OnInitDelegate(this.army_dispersed_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("army_dispersed", "army_dispersed_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.army_dispersed_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.army_dispersed_continue_on_consequence), true, -1, false, null);
			starter.AddGameMenu("menu_player_kicked_out_from_army_navigation_incapability", "{=ayktBG98}Your party does not have seaworthy ships. Army leader kicked you out from the army.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("menu_player_kicked_out_from_army_navigation_incapability", "menu_player_kicked_out_from_army_navigation_incapability_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.army_dispersed_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.player_kicked_out_from_army_consequence), false, -1, false, null);
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0014D192 File Offset: 0x0014B392
		private void army_dispersed_menu_on_init(MenuCallbackArgs args)
		{
			MBTextManager.SetTextVariable("ARMY_DISPERSE_REASON", PlayerArmyWaitBehavior.GetArmyDispersionReason(this._playerArmyDispersionReason), false);
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0014D1AA File Offset: 0x0014B3AA
		private static void player_kicked_out_from_army_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.Army = null;
			PlayerArmyWaitBehavior.army_dispersed_continue_on_consequence(args);
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0014D1C0 File Offset: 0x0014B3C0
		private void ArmyWaitMenuTick(MenuCallbackArgs args, CampaignTime dt)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (genericStateMenu != "army_wait")
			{
				args.MenuContext.GameMenu.EndWait();
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.SwitchToMenu(genericStateMenu);
				}
				else
				{
					GameMenu.ExitToLast();
				}
			}
			else
			{
				this.RefreshArmyTexts(args);
			}
			if (MobileParty.MainParty.Army.LeaderParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName(Hero.MainHero.MapFaction.Culture.EncounterBackgroundMesh);
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0014D264 File Offset: 0x0014B464
		private static TextObject GetArmyDispersionReason(Army.ArmyDispersionReason reason)
		{
			Army army = MobileParty.MainParty.Army;
			bool flag = army == null || army.LeaderParty == MobileParty.MainParty;
			bool flag2 = true;
			TextObject textObject;
			if (reason == Army.ArmyDispersionReason.NoActiveWar)
			{
				if (flag)
				{
					textObject = new TextObject("{=hrhDNRa0}Your army has disbanded. The kingdom is now at peace.", null);
				}
				else
				{
					textObject = new TextObject("{=tvAdOGzc}{ARMY_LEADER}'s army has disbanded. The kingdom is now at peace.", null);
				}
			}
			else if (reason == Army.ArmyDispersionReason.CohesionDepleted)
			{
				if (flag)
				{
					textObject = new TextObject("{=rJBgDaxe}Your army has disbanded due to lack of cohesion.", null);
				}
				else
				{
					textObject = new TextObject("{=5wwO7ozf}{ARMY_LEADER}'s army has disbanded due to a lack of cohesion.", null);
				}
			}
			else if (reason == Army.ArmyDispersionReason.FoodProblem)
			{
				if (flag)
				{
					textObject = new TextObject("{=jlU2MmaO}Your army has disbanded due to a lack of food.", null);
				}
				else
				{
					textObject = new TextObject("{=eVdUaG3x}{ARMY_LEADER}'s army has disbanded due to a lack of food.", null);
				}
			}
			else if (reason == Army.ArmyDispersionReason.NoShipToUse)
			{
				if (flag)
				{
					textObject = new TextObject("{=9ryGDgOX}Your fleet has disbanded, as you no longer have a flagship with which to lead it. ", null);
				}
				else
				{
					textObject = new TextObject("{=!}{ARMY_LEADER}'s fleet has disbanded, as {she/he} no longer has a flagship with which to lead it.", null);
				}
			}
			else
			{
				textObject = new TextObject("{=FXPvGTEa}Army you are in is dispersed.", null);
				flag2 = false;
			}
			if (!flag && flag2)
			{
				textObject.SetTextVariable("ARMY_LEADER", army.LeaderParty.LeaderHero.Name);
			}
			return textObject;
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0014D358 File Offset: 0x0014B558
		private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			if (isPlayersArmy)
			{
				Debug.Print(string.Format("Player army is dispersed due to:  {0}", reason), 0, Debug.DebugColor.White, 17592186044416UL);
				this._playerArmyDispersionReason = reason;
				if (Campaign.Current.CurrentMenuContext != null)
				{
					Campaign.Current.CurrentMenuContext.GameMenu.EndWait();
					GameMenu.SwitchToMenu("army_dispersed");
					return;
				}
				GameMenu.ActivateGameMenu("army_dispersed");
			}
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0014D3C8 File Offset: 0x0014B5C8
		private void wait_menu_army_wait_on_init(MenuCallbackArgs args)
		{
			Army army = MobileParty.MainParty.Army;
			bool flag;
			if (army == null)
			{
				flag = null != null;
			}
			else
			{
				MobileParty leaderParty = army.LeaderParty;
				flag = ((leaderParty != null) ? leaderParty.LeaderHero : null) != null;
			}
			if (flag)
			{
				this._armyDescriptionText.SetTextVariable("HERO", army.LeaderParty.LeaderHero.Name);
				args.MenuTitle = this._armyDescriptionText;
			}
			else
			{
				args.MenuTitle = this._disbandingArmyDescriptionText;
			}
			this.RefreshArmyTexts(args);
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0014D43C File Offset: 0x0014B63C
		private void wait_menu_army_wait_at_settlement_on_init(MenuCallbackArgs args)
		{
			if (!PlayerEncounter.InsideSettlement && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				PlayerEncounter.EnterSettlement();
			}
			this._armyDescriptionText.SetTextVariable("HERO", MobileParty.MainParty.Army.LeaderParty.LeaderHero.Name);
			args.MenuTitle = this._armyDescriptionText;
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = true;
			}
			this.RefreshArmyTexts(args);
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0014D4BC File Offset: 0x0014B6BC
		private static void wait_menu_army_wait_at_settlement_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (genericStateMenu != "army_wait_at_settlement")
			{
				args.MenuContext.GameMenu.EndWait();
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.SwitchToMenu(genericStateMenu);
					return;
				}
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0014D510 File Offset: 0x0014B710
		private void RefreshArmyTexts(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null)
			{
				TextObject text = args.MenuContext.GameMenu.GetText();
				if (MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
				{
					TextObject textObject = GameTexts.FindText("str_you_are_following_army", null);
					textObject.SetTextVariable("ARMY_LEADER", MobileParty.MainParty.Army.LeaderParty.LeaderHero.Name);
					text.SetTextVariable("ARMY_OWNER_TEXT", textObject);
					text.SetTextVariable("ARMY_BEHAVIOR", MobileParty.MainParty.Army.GetLongTermBehaviorText(false));
					return;
				}
				text.SetTextVariable("ARMY_OWNER_TEXT", this._leadingArmyDescriptionText);
				text.SetTextVariable("ARMY_BEHAVIOR", "");
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0014D5D1 File Offset: 0x0014B7D1
		private static bool wait_menu_army_wait_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0014D5D4 File Offset: 0x0014B7D4
		private static bool wait_menu_army_abandon_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			if (MobileParty.MainParty.Army == null || (MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.BesiegedSettlement == null))
			{
				return false;
			}
			args.Tooltip = GameTexts.FindText("str_abandon_army", null);
			args.Tooltip.SetTextVariable("INFLUENCE_COST", Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy());
			return true;
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0014D650 File Offset: 0x0014B850
		private static bool wait_menu_army_enter_settlement_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.BesiegedSettlement == null)
			{
				Settlement settlement = null;
				if (MobileParty.MainParty.CurrentSettlement != null)
				{
					settlement = MobileParty.MainParty.CurrentSettlement;
				}
				else if (MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement != null && MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement.Position.Distance(MobileParty.MainParty.Army.LeaderParty.Position) < 1f)
				{
					settlement = MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement;
				}
				if (settlement != null)
				{
					if (settlement.IsTown)
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=bkoJ57h3}Enter the Town", false);
					}
					else if (settlement.IsCastle)
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=aa3kbW8j}Enter the Castle", false);
					}
					else if (settlement.IsVillage)
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=8UzRj1YW}Enter the Village", false);
					}
					else
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=eabR87ne}Enter the Settlement", false);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0014D790 File Offset: 0x0014B990
		private static void wait_menu_army_enter_settlement_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && (MobileParty.MainParty.CurrentSettlement == null || PlayerEncounter.Current == null))
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement);
				return;
			}
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			if (currentSettlement.IsTown)
			{
				GameMenu.ActivateGameMenu("town");
				return;
			}
			if (currentSettlement.IsCastle)
			{
				GameMenu.ActivateGameMenu("castle");
				return;
			}
			GameMenu.ActivateGameMenu("village");
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0014D82C File Offset: 0x0014BA2C
		private static bool wait_menu_army_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.BesiegedSettlement == null;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0014D85D File Offset: 0x0014BA5D
		private static void wait_menu_army_leave_on_consequence(MenuCallbackArgs args)
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

		// Token: 0x06004467 RID: 17511 RVA: 0x0014D89C File Offset: 0x0014BA9C
		private static void wait_menu_army_abandon_on_consequence(MenuCallbackArgs args)
		{
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy()));
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			else
			{
				GameMenu.ExitToLast();
			}
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0014D8E8 File Offset: 0x0014BAE8
		private static void OnTick(float dt)
		{
			if (MobileParty.MainParty.AttachedTo != null)
			{
				MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
				string text;
				if (currentMenuContext == null)
				{
					text = null;
				}
				else
				{
					GameMenu gameMenu = currentMenuContext.GameMenu;
					text = ((gameMenu != null) ? gameMenu.StringId : null);
				}
				Settlement settlement;
				if (text == "army_wait" && (settlement = MobileParty.MainParty.AttachedTo.Army.AiBehaviorObject as Settlement) != null && settlement.SiegeEvent != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty.BesiegedSettlement == settlement)
				{
					PlayerSiege.StartPlayerSiege(BattleSideEnum.Attacker, false, settlement);
					PlayerSiege.StartSiegePreparation();
				}
			}
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0014D980 File Offset: 0x0014BB80
		private static void army_dispersed_continue_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				GameMenu.ExitToLast();
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement.IsVillage)
			{
				GameMenu.SwitchToMenu("village");
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement.IsTown)
			{
				GameMenu.SwitchToMenu((MobileParty.MainParty.CurrentSettlement.SiegeEvent != null) ? "menu_siege_strategies" : "town");
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu((MobileParty.MainParty.CurrentSettlement.SiegeEvent != null) ? "menu_siege_strategies" : "castle");
				return;
			}
			LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0014DA30 File Offset: 0x0014BC30
		private static bool army_dispersed_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0014DA3C File Offset: 0x0014BC3C
		[GameMenuInitializationHandler("army_wait")]
		private static void game_menu_army_wait_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army.LeaderParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName(Hero.MainHero.MapFaction.Culture.EncounterBackgroundMesh);
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0014DA90 File Offset: 0x0014BC90
		[GameMenuInitializationHandler("army_wait_at_settlement")]
		private static void game_menu_army_wait_at_settlement_on_init(MenuCallbackArgs args)
		{
			Settlement settlement = ((Settlement.CurrentSettlement != null) ? Settlement.CurrentSettlement : ((MobileParty.MainParty.LastVisitedSettlement != null) ? MobileParty.MainParty.LastVisitedSettlement : MobileParty.MainParty.AttachedTo.LastVisitedSettlement));
			args.MenuContext.SetBackgroundMeshName(settlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0014DAE9 File Offset: 0x0014BCE9
		[GameMenuInitializationHandler("army_dispersed")]
		private static void game_menu_army_dispersed_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_fallback");
		}

		// Token: 0x0400136B RID: 4971
		private readonly TextObject _leadingArmyDescriptionText;

		// Token: 0x0400136C RID: 4972
		private readonly TextObject _armyDescriptionText;

		// Token: 0x0400136D RID: 4973
		private readonly TextObject _disbandingArmyDescriptionText;

		// Token: 0x0400136E RID: 4974
		private Army.ArmyDispersionReason _playerArmyDispersionReason;
	}
}
