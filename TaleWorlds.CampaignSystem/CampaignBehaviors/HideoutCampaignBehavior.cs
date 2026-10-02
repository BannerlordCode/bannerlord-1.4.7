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
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F8 RID: 1016
	public class HideoutCampaignBehavior : CampaignBehaviorBase, IHideoutCampaignBehavior
	{
		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x001234D8 File Offset: 0x001216D8
		private static float IncreaseRelationWithVillageNotableMaximumDistanceAsDays
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x001234DF File Offset: 0x001216DF
		private int CanAttackHideoutStart
		{
			get
			{
				return Campaign.Current.Models.HideoutModel.CanAttackHideoutStartTime;
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x001234F5 File Offset: 0x001216F5
		private int CanAttackHideoutEnd
		{
			get
			{
				return Campaign.Current.Models.HideoutModel.CanAttackHideoutEndTime;
			}
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0012350C File Offset: 0x0012170C
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnHideoutSpottedEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnHideoutSpotted));
			CampaignEvents.OnHideoutBattleCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState>(this.OnHideoutBattleCompleted));
			CampaignEvents.OnCollectLootsItemsEvent.AddNonSerializedListener(this, new Action<PartyBase, ItemRoster>(this.OnCollectLootItems));
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x001235BA File Offset: 0x001217BA
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x001235C3 File Offset: 0x001217C3
		public void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x001235CC File Offset: 0x001217CC
		public void HourlyTickSettlement(Settlement settlement)
		{
			if (settlement.IsHideout && settlement.Hideout.IsInfested && !settlement.Hideout.IsSpotted)
			{
				float hideoutSpottingDistance = Campaign.Current.Models.MapVisibilityModel.GetHideoutSpottingDistance();
				float num = MobileParty.MainParty.Position.DistanceSquared(settlement.Position);
				float num2 = 1f - num / (hideoutSpottingDistance * hideoutSpottingDistance);
				if (num2 > 0f && settlement.Parties.Count > 0 && MBRandom.RandomFloat < num2 && !settlement.Hideout.IsSpotted)
				{
					settlement.Hideout.IsSpotted = true;
					settlement.IsVisible = true;
					CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, settlement.Party);
				}
			}
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x00123696 File Offset: 0x00121896
		private void OnHideoutSpotted(PartyBase party, PartyBase hideout)
		{
			SkillLevelingManager.OnHideoutSpotted(party.MobileParty, hideout);
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x001236A4 File Offset: 0x001218A4
		private int GetItemValueForHideoutLoot(ItemObject itemToLoot)
		{
			return Campaign.Current.Models.TradeItemPriceFactorModel.GetTheoreticalMaxItemMarketValue(itemToLoot) + 1;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x001236BD File Offset: 0x001218BD
		private void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
			if (battleEndState != HideoutEventComponent.HideoutBattleEndState.Victory && battleEndState != HideoutEventComponent.HideoutBattleEndState.SendTroops)
			{
				this._initialHideoutPopulation = 0;
			}
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x001236D0 File Offset: 0x001218D0
		private void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
			if (winnerParty == PartyBase.MainParty && !PlayerEncounter.Current.ForceHideoutSendTroops)
			{
				MapEvent mapEvent = MobileParty.MainParty.MapEvent;
				if (mapEvent.IsHideoutBattle && mapEvent.MapEventSettlement == Settlement.CurrentSettlement && this.IsItNighttimeNow())
				{
					int num = 0;
					foreach (MapEventParty mapEventParty in mapEvent.GetMapEventSide(mapEvent.PlayerSide).Parties)
					{
						if (mapEventParty.Party == PartyBase.MainParty)
						{
							num = mapEventParty.PlunderedGold;
							break;
						}
					}
					int totalLootedValue = 0;
					float targetValue = (float)(num * (this._initialHideoutPopulation * 135));
					targetValue = MathF.Clamp(targetValue, (float)this._minimumHideoutLootTargetValue, 4750f);
					if ((float)totalLootedValue < targetValue)
					{
						int num2 = 0;
						ItemObject itemObject;
						while (num2 < this._potentialLootItems.Count && gainedLoots.Count < 5 && (float)totalLootedValue < targetValue)
						{
							itemObject = this._potentialLootItems[num2];
							int itemValueForHideoutLoot = this.GetItemValueForHideoutLoot(itemObject);
							if ((float)itemValueForHideoutLoot <= targetValue - (float)totalLootedValue)
							{
								gainedLoots.AddToCounts(itemObject, 1);
								totalLootedValue += itemValueForHideoutLoot;
							}
							num2++;
						}
						Func<ItemRosterElement, bool> <>9__0;
						do
						{
							Func<ItemRosterElement, bool> func;
							if ((func = <>9__0) == null)
							{
								func = (<>9__0 = (ItemRosterElement x) => !x.EquipmentElement.Item.NotMerchandise && !x.EquipmentElement.IsQuestItem && !x.EquipmentElement.Item.IsBannerItem && (float)this.GetItemValueForHideoutLoot(x.EquipmentElement.Item) <= targetValue - (float)totalLootedValue);
							}
							itemObject = gainedLoots.GetRandomElementWithPredicate<ItemRosterElement>(func).EquipmentElement.Item;
							if (itemObject != null)
							{
								gainedLoots.AddToCounts(itemObject, 1);
								totalLootedValue += this.GetItemValueForHideoutLoot(itemObject);
							}
						}
						while (itemObject != null);
					}
				}
			}
			this._initialHideoutPopulation = 0;
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x001238CC File Offset: 0x00121ACC
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<float>("_hideoutWaitProgressHours", ref this._hideoutWaitProgressHours);
			dataStore.SyncData<float>("_hideoutWaitTargetHours", ref this._hideoutWaitTargetHours);
			dataStore.SyncData<float>("_hideoutSendTroopsWaitProgressHour", ref this._hideoutSendTroopsWaitProgressHour);
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x00123904 File Offset: 0x00121B04
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			foreach (ItemObject itemObject in Campaign.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.IsTradeGood)
				{
					int itemValueForHideoutLoot = this.GetItemValueForHideoutLoot(itemObject);
					if (itemValueForHideoutLoot >= this._minimumHideoutLootTargetValue && itemValueForHideoutLoot <= 4750)
					{
						this._potentialLootItems.Add(itemObject);
					}
				}
			}
			this._potentialLootItems = this._potentialLootItems.OrderByDescending<ItemObject, int>((ItemObject x) => x.Value).ToList<ItemObject>();
			if (this._potentialLootItems.Count > 0)
			{
				this._minimumHideoutLootTargetValue = this.GetItemValueForHideoutLoot(this._potentialLootItems[this._potentialLootItems.Count - 1]);
			}
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x001239F0 File Offset: 0x00121BF0
		protected void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("hideout_place", "{=!}{HIDEOUT_TEXT}", new OnInitDelegate(this.game_menu_hideout_place_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "attack", "{=p5GkeK8F}Sneak in now", new GameMenuOption.OnConditionDelegate(this.game_menu_hideout_sneak_in_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_sneak_in_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "assault", "{=b4YsZY5H}Assault hideout", new GameMenuOption.OnConditionDelegate(this.game_menu_assault_hideout_parties_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_assault_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "wait", "{=!}{WAIT_OPTION}", new GameMenuOption.OnConditionDelegate(this.game_menu_wait_until_nightfall_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_wait_until_nightfall_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "send_troops", "{=RepcYuoJ}Send troops to clear ({SUCCESS_CHANCE}%)", new GameMenuOption.OnConditionDelegate(this.game_menu_send_troops_hideout_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_send_troops_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddWaitGameMenu("hideout_wait", "{=!}{WAIT_TEXT}", new OnInitDelegate(this.hideout_wait_menu_on_init), new OnConditionDelegate(this.hideout_wait_menu_on_condition), new OnConsequenceDelegate(this.hideout_wait_menu_on_consequence), new OnTickDelegate(this.hideout_wait_menu_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameMenu.MenuOverlayType.None, this._hideoutWaitTargetHours, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_after_wait_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_wait", "{=!}{HIDEOUT_TEXT}", new OnInitDelegate(this.hideout_after_wait_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "attack", "{=Abcgrf4j}Sneak in", new GameMenuOption.OnConditionDelegate(this.game_menu_hideout_sneak_in_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_sneak_in_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "assault", "{=b4YsZY5H}Assault hideout", new GameMenuOption.OnConditionDelegate(this.game_menu_assault_hideout_parties_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_assault_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "send_troops", "{=RepcYuoJ}Send troops to clear ({SUCCESS_CHANCE}%)", new GameMenuOption.OnConditionDelegate(this.game_menu_send_troops_hideout_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_send_troops_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_after_wait_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_defeated_and_saved", "{=1zLZf5rw}The rest of your men rushed to your help, dragging you out to safety and driving the bandits back into hiding.", new OnInitDelegate(this.game_menu_hideout_after_defeated_and_saved_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_defeated_and_saved", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_found_by_sentries", "{=n0ynsBPx}Sentries detected you and alerted the rest of the bandits. Bandits moved back into hiding before you could round up your troops.", new OnInitDelegate(this.game_menu_hideout_after_defeated_and_saved_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_found_by_sentries", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddWaitGameMenu("hideout_send_troops_wait", "{=QOT7PSUp}Your troops are clearing the hideout.", new OnInitDelegate(this.hideout_send_troops_wait_menu_on_init), null, null, new OnTickDelegate(this.hideout_send_troops_wait_menu_tick), GameMenu.MenuAndOptionType.WaitMenuShowProgressAndHoursOption, GameMenu.MenuOverlayType.None, 6f, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_send_troops_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.hideout_send_troops_wait_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_send_troops_result_success", "{=nfgQU4Uk}Your men surprised the bandits, and cut down several before the rest fled in disarray. You don't think they'll be back.", new OnInitDelegate(this.hideout_send_troops_result_success_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_send_troops_result_success", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.hideout_send_troops_result_success_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_send_troops_result_failure", "{=b4HypxQ5}Your men failed their approach... The bandits' sentries spotted them before they attacked, and the bandits withdrew in good order, probably to some nearby hiding places. Until they are dealt with properly, their presence will continue to threaten the region. You expect it won't be long before they return to their lair.\r\n", new OnInitDelegate(this.hideout_send_troops_result_failure_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_send_troops_result_failure", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.hideout_send_troops_result_failure_consequence), true, -1, false, null);
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x00123E14 File Offset: 0x00122014
		private void hideout_send_troops_result_success_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PlayerEncounter.Finish(true);
			this.SetCleanHideoutRelations(currentSettlement);
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x00123E34 File Offset: 0x00122034
		private void hideout_send_troops_result_success_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Battle.WinningSide == BattleSideEnum.None)
			{
				PlayerEncounter.Battle.SetOverrideWinner(PlayerEncounter.Battle.PlayerSide);
				CampaignEventDispatcher.Instance.OnHideoutBattleCompleted(PlayerEncounter.Battle.PlayerSide, (HideoutEventComponent)PlayerEncounter.Battle.Component, HideoutEventComponent.HideoutBattleEndState.SendTroops);
				PlayerEncounter.Update();
				Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
				if (encounterSettlement == null)
				{
					return;
				}
				encounterSettlement.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x00123E9F File Offset: 0x0012209F
		private void hideout_send_troops_result_failure_on_init(MenuCallbackArgs args)
		{
			Settlement.CurrentSettlement.Hideout.SetNextPossibleAttackTime(Campaign.Current.Models.HideoutModel.HideoutHiddenDuration);
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00123EC4 File Offset: 0x001220C4
		private void hideout_send_troops_result_failure_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x00123ECC File Offset: 0x001220CC
		public int GetInitialHideoutPopulation()
		{
			return this._initialHideoutPopulation;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x00123ED4 File Offset: 0x001220D4
		private void hideout_wait_menu_on_init(MenuCallbackArgs args)
		{
			this.UpdateHideoutWaitProgress(args);
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x00123EE0 File Offset: 0x001220E0
		private bool IsItNighttimeNow()
		{
			float currentHourInDay = CampaignTime.Now.CurrentHourInDay;
			return (this.CanAttackHideoutStart > this.CanAttackHideoutEnd && (currentHourInDay >= (float)this.CanAttackHideoutStart || currentHourInDay <= (float)this.CanAttackHideoutEnd)) || (this.CanAttackHideoutStart < this.CanAttackHideoutEnd && currentHourInDay >= (float)this.CanAttackHideoutStart && currentHourInDay <= (float)this.CanAttackHideoutEnd);
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x00123F48 File Offset: 0x00122148
		public bool hideout_wait_menu_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x00123F4B File Offset: 0x0012214B
		public void hideout_wait_menu_on_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._hideoutWaitProgressHours += (float)campaignTime.ToHours;
			this.UpdateHideoutWaitProgress(args);
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x00123F6C File Offset: 0x0012216C
		private void UpdateHideoutWaitProgress(MenuCallbackArgs args)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (!this.IsItNighttimeNow())
			{
				textObject = new TextObject("{=VLLAOXve}Waiting until nightfall to sneak in.", null);
			}
			else
			{
				textObject = new TextObject("{=GrrWYNIZ}Waiting until morning to begin the assault.", null);
			}
			MBTextManager.SetTextVariable("WAIT_TEXT", textObject, false);
			if (this._hideoutWaitTargetHours.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this.CalculateHideoutAttackTime();
			}
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._hideoutWaitProgressHours / this._hideoutWaitTargetHours);
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x00123FE7 File Offset: 0x001221E7
		public void hideout_wait_menu_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_after_wait");
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00123FF3 File Offset: 0x001221F3
		private bool leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x00124000 File Offset: 0x00122200
		[GameMenuInitializationHandler("hideout_wait")]
		[GameMenuInitializationHandler("hideout_after_wait")]
		[GameMenuInitializationHandler("hideout_after_defeated_and_saved")]
		private static void game_menu_hideout_ui_place_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Hideout.WaitMeshName);
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0012402C File Offset: 0x0012222C
		[GameMenuInitializationHandler("hideout_place")]
		private static void game_menu_hideout_sound_place_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_hideout");
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Hideout.WaitMeshName);
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x00124065 File Offset: 0x00122265
		private void game_menu_hideout_after_defeated_and_saved_on_init(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.IsHideout)
			{
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x00124088 File Offset: 0x00122288
		private void game_menu_hideout_place_on_init(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.IsHideout)
			{
				return;
			}
			this._hideoutWaitProgressHours = 0f;
			this._hideoutSendTroopsWaitProgressHour = 0f;
			if (!this.IsItNighttimeNow())
			{
				this.CalculateHideoutAttackTime();
			}
			else
			{
				this._hideoutWaitTargetHours = 0f;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			int num = 0;
			foreach (MobileParty mobileParty in currentSettlement.Parties)
			{
				num += mobileParty.MemberRoster.TotalManCount - mobileParty.MemberRoster.TotalWounded;
			}
			GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=DOmb81Mu}(Undefined hideout type)");
			if (currentSettlement.Culture.StringId.Equals("forest_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=cu2cLT5r}You spy though the trees what seems to be a clearing in the forest with what appears to be the outlines of a camp.");
			}
			if (currentSettlement.Culture.StringId.Equals("sea_raiders"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=bJ6ygV3P}As you travel along the coast, you see a sheltered cove with what appears to the outlines of a camp.");
			}
			if (currentSettlement.Culture.StringId.Equals("mountain_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=iyWUDSm8}Passing by the slopes of the mountains, you see an outcrop crowned with the ruins of an ancient fortress.");
			}
			if (currentSettlement.Culture.StringId.Equals("desert_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=b3iBOVXN}Passing by a wadi, you see what looks like a camouflaged well to tap the groundwater left behind by rare rainfalls.");
			}
			if (currentSettlement.Culture.StringId.Equals("steppe_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=5JaGVr0U}While traveling by a low range of hills, you see what appears to be the remains of a campsite in a stream gully.");
			}
			bool flag = !currentSettlement.Hideout.NextPossibleAttackTime.IsPast;
			if (flag)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=KLWn6yZQ}{HIDEOUT_DESCRIPTION} The remains of a fire suggest that it's been recently occupied, but its residents - whoever they are - are well-hidden for now. Until they are dealt with properly, their presence will continue to threaten the region.");
			}
			else if (num > 0)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=prcBBqMR}{HIDEOUT_DESCRIPTION} You see armed men moving about. As you listen quietly, you hear scraps of conversation about raids, ransoms, and the best places to waylay travellers.");
			}
			else
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=gywyEgZa}{HIDEOUT_DESCRIPTION} There seems to be no one inside.");
			}
			if (!flag && num > 0 && Hero.MainHero.IsWounded)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=fMekM2UH}{HIDEOUT_DESCRIPTION} You can not attack since your wounds do not allow you.");
			}
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			bool isInfested = Settlement.CurrentSettlement.Hideout.IsInfested;
			Settlement settlement = (Settlement.CurrentSettlement.IsHideout ? Settlement.CurrentSettlement : null);
			if (PlayerEncounter.Battle != null)
			{
				bool flag2 = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Current.PlayerSide;
				PlayerEncounter.Update();
				if (flag2 && PlayerEncounter.Battle == null && settlement != null)
				{
					this.SetCleanHideoutRelations(settlement);
				}
			}
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x001242E4 File Offset: 0x001224E4
		private void CalculateHideoutAttackTime()
		{
			float currentHourInDay = CampaignTime.Now.CurrentHourInDay;
			if (this.IsItNighttimeNow())
			{
				this._hideoutWaitTargetHours = (((float)this.CanAttackHideoutEnd > currentHourInDay) ? ((float)this.CanAttackHideoutEnd - currentHourInDay) : ((float)CampaignTime.HoursInDay - currentHourInDay + (float)this.CanAttackHideoutEnd));
				return;
			}
			this._hideoutWaitTargetHours = (((float)this.CanAttackHideoutStart > currentHourInDay) ? ((float)this.CanAttackHideoutStart - currentHourInDay) : ((float)CampaignTime.HoursInDay - currentHourInDay + (float)this.CanAttackHideoutStart));
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x00124360 File Offset: 0x00122560
		private void SetCleanHideoutRelations(Settlement hideout)
		{
			List<Settlement> list = new List<Settlement>();
			float num = HideoutCampaignBehavior.IncreaseRelationWithVillageNotableMaximumDistanceAsDays * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			foreach (Village village in Campaign.Current.AllVillages)
			{
				if (village.Settlement.Position.DistanceSquared(hideout.Position) <= num * num)
				{
					list.Add(village.Settlement);
				}
			}
			foreach (Settlement settlement in list)
			{
				if (settlement.Notables.Count > 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(settlement.Notables.GetRandomElement<Hero>(), 2, true, false);
				}
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
			{
				Town town = SettlementHelper.FindNearestTownToSettlement(hideout, MobileParty.NavigationType.All, null);
				Hero leader = town.OwnerClan.Leader;
				if (leader == Hero.MainHero)
				{
					town.Loyalty += 1f;
				}
				else
				{
					ChangeRelationAction.ApplyPlayerRelation(leader, (int)DefaultPerks.Charm.EffortForThePeople.PrimaryBonus, true, true);
				}
			}
			MBTextManager.SetTextVariable("RELATION_VALUE", (int)DefaultPerks.Charm.EffortForThePeople.PrimaryBonus);
			MBInformationManager.AddQuickInformation(new TextObject("{=o0qwDa0q}Your relation increased by {RELATION_VALUE} with nearby notables.", null), 0, null, null, "");
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x001244DC File Offset: 0x001226DC
		private void hideout_after_wait_menu_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (this.IsItNighttimeNow())
			{
				textObject = new TextObject("{=VbU8Ue0O}After waiting for a while you find a good opportunity to close in undetected beneath the shroud of the night.", null);
			}
			else
			{
				textObject = new TextObject("{=7ovEa1gB}After waiting for a while you find a good opportunity to assault the camp.", null);
			}
			MBTextManager.SetTextVariable("HIDEOUT_TEXT", textObject, false);
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x00124520 File Offset: 0x00122720
		private bool game_menu_hideout_sneak_in_on_condition(MenuCallbackArgs args)
		{
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			object obj;
			if (Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit))
				{
					obj = hideout.NextPossibleAttackTime.IsPast;
					goto IL_0062;
				}
			}
			obj = 0;
			IL_0062:
			object obj2 = obj;
			if (obj2 != null)
			{
				if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=pM9GOxrV}You are wounded, you can't sneak in!", null);
				}
				else
				{
					int minimumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, false);
					if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumTroopCountForHideoutMission)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=XasRXCod}You should have more than {AMOUNT} healthy troops in your party to attack!", null);
						args.Tooltip.SetTextVariable("AMOUNT", minimumTroopCountForHideoutMission);
					}
					else if (!this.IsItNighttimeNow())
					{
						args.Tooltip = new TextObject("{=beDxk5YB}Bandits are awake and too aware to be ambushed.", null);
						args.IsEnabled = false;
					}
				}
			}
			args.optionLeaveType = GameMenuOption.LeaveType.SneakIn;
			return obj2 != null;
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x0012463C File Offset: 0x0012283C
		private bool game_menu_assault_hideout_parties_on_condition(MenuCallbackArgs args)
		{
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			object obj;
			if (Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit))
				{
					obj = hideout.NextPossibleAttackTime.IsPast;
					goto IL_0062;
				}
			}
			obj = 0;
			IL_0062:
			object obj2 = obj;
			if (obj2 != null)
			{
				if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=ZCKKVRjT}You are wounded, you can't assault the hideout!", null);
				}
				else
				{
					int minimumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, true);
					if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumTroopCountForHideoutMission)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=XasRXCod}You should have more than {AMOUNT} healthy troops in your party to attack!", null);
						args.Tooltip.SetTextVariable("AMOUNT", minimumTroopCountForHideoutMission);
					}
					else if (this.IsItNighttimeNow())
					{
						args.Tooltip = new TextObject("{=7IXfD2qq}A frontal assault against the hideout is too dangerous.", null);
						args.IsEnabled = false;
					}
				}
				args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			}
			return obj2 != null;
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x00124758 File Offset: 0x00122958
		private void game_menu_encounter_sneak_in_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args, delegate(TroopRoster x)
			{
				this.OnTroopRosterManageDone(x, false);
			}, false);
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0012476E File Offset: 0x0012296E
		private void game_menu_encounter_assault_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args, delegate(TroopRoster x)
			{
				this.OnTroopRosterManageDone(x, true);
			}, true);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00124784 File Offset: 0x00122984
		private void game_menu_encounter_attack_on_consequence(MenuCallbackArgs args, Action<TroopRoster> onDone, bool isDirectAssault)
		{
			BanditDensityModel banditDensityModel = Campaign.Current.Models.BanditDensityModel;
			int maximumTroopCountForHideoutMission = banditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty.MainParty, isDirectAssault);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster strongestAndPriorTroops = MobilePartyHelper.GetStrongestAndPriorTroops(MobileParty.MainParty, maximumTroopCountForHideoutMission, true);
			troopRoster.Add(strongestAndPriorTroops);
			int maximumTroopCountForHideoutMission2 = banditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty.MainParty, isDirectAssault);
			args.MenuContext.OpenTroopSelection(MobileParty.MainParty.MemberRoster, troopRoster, new Func<CharacterObject, bool>(this.CanChangeStatusOfTroop), onDone, maximumTroopCountForHideoutMission2, banditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, isDirectAssault));
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00124808 File Offset: 0x00122A08
		private bool game_menu_send_troops_hideout_on_condition(MenuCallbackArgs args)
		{
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			args.Tooltip = new TextObject("{=Xum0Iddf}You will gain no loot or experience.", null);
			object obj;
			if (Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit))
				{
					obj = hideout.NextPossibleAttackTime.IsPast;
					goto IL_0073;
				}
			}
			obj = 0;
			IL_0073:
			object obj2 = obj;
			if (obj2 != null)
			{
				int minimumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, false);
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumTroopCountForHideoutMission)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=yUbdUFSC}You should have more than {AMOUNT} healthy troops in your party to send your troops!", null);
					args.Tooltip.SetTextVariable("AMOUNT", minimumTroopCountForHideoutMission);
				}
				args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
				float sendTroopsSuccessChance = Campaign.Current.Models.HideoutModel.GetSendTroopsSuccessChance(hideout);
				MBTextManager.SetTextVariable("SUCCESS_CHANCE", MathF.Round(sendTroopsSuccessChance * 100f));
			}
			return obj2 != null;
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00124919 File Offset: 0x00122B19
		private void game_menu_encounter_send_troops_on_consequence(MenuCallbackArgs args)
		{
			this.UpdateInitialHideoutPopulation();
			PlayerEncounter.Current.ForceHideoutSendTroops = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00124938 File Offset: 0x00122B38
		private void ArrangeHideoutTroopCountsForMission()
		{
			int numberOfMinimumBanditTroopsInHideoutMission = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditTroopsInHideoutMission;
			int num = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForFirstFightInHideout + Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForBossFightInHideout;
			MBList<MobileParty> mblist = Settlement.CurrentSettlement.Parties.Where<MobileParty>((MobileParty x) => x.IsBandit || x.IsBanditBossParty).ToMBList<MobileParty>();
			int num2 = mblist.Sum<MobileParty>((MobileParty x) => x.MemberRoster.TotalHealthyCount);
			if (num2 > num)
			{
				int i = num2 - num;
				mblist.RemoveAll((MobileParty x) => x.IsBanditBossParty || x.MemberRoster.TotalHealthyCount == 1);
				while (i > 0)
				{
					if (mblist.Count <= 0)
					{
						return;
					}
					MobileParty randomElement = mblist.GetRandomElement<MobileParty>();
					List<TroopRosterElement> troopRoster = randomElement.MemberRoster.GetTroopRoster();
					List<ValueTuple<TroopRosterElement, float>> list = new List<ValueTuple<TroopRosterElement, float>>();
					foreach (TroopRosterElement troopRosterElement in troopRoster)
					{
						list.Add(new ValueTuple<TroopRosterElement, float>(troopRosterElement, (float)(troopRosterElement.Number - troopRosterElement.WoundedNumber)));
					}
					TroopRosterElement troopRosterElement2 = MBRandom.ChooseWeighted<TroopRosterElement>(list);
					randomElement.MemberRoster.AddToCounts(troopRosterElement2.Character, -1, false, 0, 0, true, -1);
					i--;
					if (randomElement.MemberRoster.TotalHealthyCount == 1)
					{
						mblist.Remove(randomElement);
					}
				}
			}
			else if (num2 < numberOfMinimumBanditTroopsInHideoutMission)
			{
				int num3 = numberOfMinimumBanditTroopsInHideoutMission - num2;
				mblist.RemoveAll((MobileParty x) => x.MemberRoster.GetTroopRoster().All<TroopRosterElement>((TroopRosterElement y) => y.Number == 0 || y.Character.Culture.BanditBoss == y.Character || y.Character.IsHero));
				while (num3 > 0 && mblist.Count > 0)
				{
					MobileParty randomElement2 = mblist.GetRandomElement<MobileParty>();
					List<TroopRosterElement> troopRoster2 = randomElement2.MemberRoster.GetTroopRoster();
					List<ValueTuple<TroopRosterElement, float>> list2 = new List<ValueTuple<TroopRosterElement, float>>();
					foreach (TroopRosterElement troopRosterElement3 in troopRoster2)
					{
						list2.Add(new ValueTuple<TroopRosterElement, float>(troopRosterElement3, (float)(troopRosterElement3.Number * ((troopRosterElement3.Character.Culture.BanditBoss == troopRosterElement3.Character || troopRosterElement3.Character.IsHero) ? 0 : 1))));
					}
					TroopRosterElement troopRosterElement4 = MBRandom.ChooseWeighted<TroopRosterElement>(list2);
					randomElement2.MemberRoster.AddToCounts(troopRosterElement4.Character, 1, false, 0, 0, true, -1);
					num3--;
				}
			}
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00124BE8 File Offset: 0x00122DE8
		private void OnTroopRosterManageDone(TroopRoster hideoutTroops, bool isDirectAssault)
		{
			this.ArrangeHideoutTroopCountsForMission();
			GameMenu.SwitchToMenu("hideout_place");
			Settlement.CurrentSettlement.Hideout.SetNextPossibleAttackTime(Campaign.Current.Models.HideoutModel.HideoutHiddenDuration);
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				PlayerEncounter.Start();
				PlayerEncounter.Current.SetupFields(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			}
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
				PlayerEncounter.Update();
			}
			if (isDirectAssault)
			{
				this.AdjustTroopCountForHideoutAssault();
			}
			this.UpdateInitialHideoutPopulation();
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("hideout_center");
			if (isDirectAssault)
			{
				CampaignMission.OpenHideoutBattleMission(locationWithId.GetSceneName(0), (hideoutTroops != null) ? hideoutTroops.ToFlattenedRoster() : null, false);
				return;
			}
			CampaignMission.OpenHideoutAmbushMission(locationWithId.GetSceneName(0), (hideoutTroops != null) ? hideoutTroops.ToFlattenedRoster() : null, locationWithId);
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x00124CC4 File Offset: 0x00122EC4
		private void AdjustTroopCountForHideoutAssault()
		{
			int num = 0;
			MapEventParty mapEventParty = null;
			foreach (MapEventParty mapEventParty2 in MapEvent.PlayerMapEvent.PartiesOnSide(BattleSideEnum.Defender))
			{
				if (mapEventParty2.Party.IsMobile)
				{
					if (mapEventParty == null)
					{
						mapEventParty = mapEventParty2;
					}
					num += mapEventParty2.Party.MemberRoster.TotalHealthyCount;
				}
			}
			if (mapEventParty != null && num < 25)
			{
				int num2 = 25 - num;
				CharacterObject banditBandit = mapEventParty.Party.Culture.BanditBandit;
				mapEventParty.Party.MemberRoster.AddToCounts(banditBandit, num2, false, 0, 0, true, -1);
			}
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00124D78 File Offset: 0x00122F78
		private void UpdateInitialHideoutPopulation()
		{
			this._initialHideoutPopulation = 0;
			foreach (MobileParty mobileParty in Settlement.CurrentSettlement.Parties)
			{
				if (mobileParty.IsBandit)
				{
					foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster())
					{
						int num = troopRosterElement.Number - troopRosterElement.WoundedNumber;
						this._initialHideoutPopulation += num;
					}
				}
			}
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x00124E38 File Offset: 0x00123038
		private bool CanChangeStatusOfTroop(CharacterObject character)
		{
			return !character.IsPlayerCharacter && !character.IsNotTransferableInHideouts;
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x00124E50 File Offset: 0x00123050
		private bool game_menu_talk_to_leader_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			PartyBase party = Settlement.CurrentSettlement.Parties[0].Party;
			return party != null && party.LeaderHero != null && party.LeaderHero != Hero.MainHero;
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x00124E98 File Offset: 0x00123098
		private void game_menu_talk_to_leader_on_consequence(MenuCallbackArgs args)
		{
			PartyBase party = Settlement.CurrentSettlement.Parties[0].Party;
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(party), party, false, false, false, false, false, false);
			CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x00124EF8 File Offset: 0x001230F8
		private bool game_menu_wait_until_nightfall_on_condition(MenuCallbackArgs args)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (this.IsItNighttimeNow())
			{
				textObject = new TextObject("{=qr1V1Drj}Wait until morning to begin the assault", null);
			}
			else
			{
				textObject = new TextObject("{=JYH6FF35}Wait until nightfall to sneak in", null);
			}
			MBTextManager.SetTextVariable("WAIT_OPTION", textObject, false);
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty t) => t != MobileParty.MainParty) && Settlement.CurrentSettlement.Hideout.NextPossibleAttackTime.IsPast;
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x00124F8A File Offset: 0x0012318A
		private void game_menu_wait_until_nightfall_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_wait");
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00124F96 File Offset: 0x00123196
		private void game_menu_hideout_leave_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement != null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00124FB9 File Offset: 0x001231B9
		private void game_menu_hideout_after_wait_leave_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_place");
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00124FC5 File Offset: 0x001231C5
		private void hideout_send_troops_wait_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Hideout.WaitMeshName);
			this.UpdateSendTroopsToClearProgress(args);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00124FE8 File Offset: 0x001231E8
		private void hideout_send_troops_wait_menu_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._hideoutSendTroopsWaitProgressHour += (float)campaignTime.ToHours;
			this.UpdateSendTroopsToClearProgress(args);
			if (args.MenuContext.GameMenu.Progress >= 1f)
			{
				this.ApplySendTroopsResults();
			}
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00125023 File Offset: 0x00123223
		private void ApplySendTroopsResults()
		{
			if (this.GetHideoutCanBeSuccessfullyClearedWithSendTroops(Settlement.CurrentSettlement.Hideout))
			{
				GameMenu.SwitchToMenu("hideout_send_troops_result_success");
				return;
			}
			GameMenu.SwitchToMenu("hideout_send_troops_result_failure");
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x0012504C File Offset: 0x0012324C
		private bool GetHideoutCanBeSuccessfullyClearedWithSendTroops(Hideout hideout)
		{
			return hideout.Settlement.RandomFloatWithSeed((uint)CampaignTime.Now.ToHours) < Campaign.Current.Models.HideoutModel.GetSendTroopsSuccessChance(hideout);
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00125089 File Offset: 0x00123289
		private void UpdateSendTroopsToClearProgress(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._hideoutSendTroopsWaitProgressHour / 6f);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x001250A7 File Offset: 0x001232A7
		private void hideout_send_troops_wait_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x040012FD RID: 4861
		private const int HideoutClearRelationEffect = 2;

		// Token: 0x040012FE RID: 4862
		private const int HideoutLootTargetValueMultiplier = 135;

		// Token: 0x040012FF RID: 4863
		private int _minimumHideoutLootTargetValue = 350;

		// Token: 0x04001300 RID: 4864
		private const int MaximumHideoutLootTargetValue = 4750;

		// Token: 0x04001301 RID: 4865
		private const int MaximumHideoutExtraLootTypeCount = 5;

		// Token: 0x04001302 RID: 4866
		private const float HideoutSendTroopsWaitTargetHour = 6f;

		// Token: 0x04001303 RID: 4867
		private float _hideoutWaitProgressHours;

		// Token: 0x04001304 RID: 4868
		private float _hideoutWaitTargetHours;

		// Token: 0x04001305 RID: 4869
		private float _hideoutSendTroopsWaitProgressHour;

		// Token: 0x04001306 RID: 4870
		private int _initialHideoutPopulation;

		// Token: 0x04001307 RID: 4871
		private List<ItemObject> _potentialLootItems = new List<ItemObject>();
	}
}
