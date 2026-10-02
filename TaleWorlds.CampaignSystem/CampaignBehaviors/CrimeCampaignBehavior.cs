using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E4 RID: 996
	public class CrimeCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DAB RID: 15787 RVA: 0x0010D2A0 File Offset: 0x0010B4A0
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterGameCreated));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroDeath));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x0010D320 File Offset: 0x0010B520
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x0010D324 File Offset: 0x0010B524
		private void OnDailyTick()
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				float dailyCrimeRatingChange = clan.DailyCrimeRatingChange;
				if (!clan.IsEliminated && !dailyCrimeRatingChange.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					ChangeCrimeRatingAction.Apply(clan, dailyCrimeRatingChange, false);
				}
			}
			foreach (Kingdom kingdom in Kingdom.All)
			{
				float dailyCrimeRatingChange2 = kingdom.DailyCrimeRatingChange;
				if (!kingdom.IsEliminated && !dailyCrimeRatingChange2.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					ChangeCrimeRatingAction.Apply(kingdom, dailyCrimeRatingChange2, false);
				}
			}
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x0010D3FC File Offset: 0x0010B5FC
		private void OnAfterGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x0010D408 File Offset: 0x0010B608
		private void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenu("town_inside_criminal", "{=XgA2JgVR}You are brought to the town square to face judgment.", new OnInitDelegate(CrimeCampaignBehavior.town_inside_criminal_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_punishment", "{=8iDpmu0L}Accept corporal punishment", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_give_punishment_and_money", "{=Xi1wpR2L}Accept corporal punishment and pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_give_your_life", "{=bVi0JKSx}You will be executed. You must face it as bravely as you can", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_influence", "{=1cMS6415}Pay {FINE}{INFLUENCE_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_money", "{=870ZCp1J}Pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_ignore_charges", "{=UQhRKJb9}Ignore the charges", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_ignore_charges_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_ignore_charges_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_discuss_criminal_surrender", "{=lwVwe4qU}You are discussing the terms of your surrender.", new OnInitDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_punishment", "{=8iDpmu0L}Accept corporal punishment", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_give_punishment_and_money", "{=Xi1wpR2L}Accept corporal punishment and pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_give_your_life", "{=VSzwMDJ2}You will be put to death", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_influence", "{=1cMS6415}Pay {FINE}{INFLUENCE_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_money", "{=870ZCp1J}Pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_back_on_consequence), true, -1, false, null);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x0010D6AC File Offset: 0x0010B8AC
		private void OnHeroDeath(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim == Hero.MainHero)
			{
				foreach (Clan clan in Clan.NonBanditFactions)
				{
					if (!clan.IsEliminated)
					{
						ChangeCrimeRatingAction.Apply(clan, -clan.MainHeroCrimeRating, true);
					}
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (!kingdom.IsEliminated)
					{
						ChangeCrimeRatingAction.Apply(kingdom, -kingdom.MainHeroCrimeRating, true);
					}
				}
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x0010D764 File Offset: 0x0010B964
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if (side1Faction == Hero.MainHero.MapFaction || side2Faction == Hero.MainHero.MapFaction)
			{
				IFaction faction = ((side1Faction == Hero.MainHero.MapFaction) ? side2Faction : side1Faction);
				float num = Campaign.Current.Models.CrimeModel.DeclareWarCrimeRatingThreshold * 0.5f;
				if (faction.MainHeroCrimeRating > num)
				{
					ChangeCrimeRatingAction.Apply(faction, num - faction.MainHeroCrimeRating, true);
				}
			}
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x0010D7D0 File Offset: 0x0010B9D0
		private static bool CanPayCriminalRatingValueWith(IFaction faction, CrimeModel.PaymentMethod paymentMethod)
		{
			if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(Settlement.CurrentSettlement.MapFaction))
			{
				if (paymentMethod == CrimeModel.PaymentMethod.Gold)
				{
					return true;
				}
				if (CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(faction))
				{
					if (paymentMethod == CrimeModel.PaymentMethod.Influence)
					{
						return true;
					}
				}
				else if (paymentMethod == CrimeModel.PaymentMethod.Punishment)
				{
					return true;
				}
			}
			else if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(Settlement.CurrentSettlement.MapFaction))
			{
				if (CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(faction))
				{
					if (paymentMethod == CrimeModel.PaymentMethod.Gold)
					{
						return true;
					}
					if (paymentMethod == CrimeModel.PaymentMethod.Influence)
					{
						return true;
					}
				}
				else
				{
					if (paymentMethod.HasAnyFlag(CrimeModel.PaymentMethod.Execution))
					{
						return Hero.MainHero.Gold < (int)PayForCrimeAction.GetClearCrimeCost(faction, CrimeModel.PaymentMethod.Gold);
					}
					if (paymentMethod.HasAllFlags(CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x0010D878 File Offset: 0x0010BA78
		private static bool IsCriminalPlayerInSameKingdomOf(IFaction faction)
		{
			Clan clan = faction as Clan;
			return Hero.MainHero.Clan == faction || Hero.MainHero.Clan.Kingdom == faction || (clan != null && Hero.MainHero.Clan.Kingdom == clan.Kingdom);
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x0010D8C9 File Offset: 0x0010BAC9
		[GameMenuInitializationHandler("town_discuss_criminal_surrender")]
		[GameMenuInitializationHandler("town_inside_criminal")]
		public static void game_menu_town_criminal_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Town.WaitMeshName);
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x0010D8E5 File Offset: 0x0010BAE5
		public static void town_inside_criminal_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x0010D8F8 File Offset: 0x0010BAF8
		public static void town_discuss_criminal_surrender_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x0010D8FA File Offset: 0x0010BAFA
		public static bool criminal_inside_menu_pay_by_punishment_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			args.Tooltip = new TextObject("{=yGvsbUWc}Beware that you may die from punishment!", null);
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Punishment);
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x0010D928 File Offset: 0x0010BB28
		public static void criminal_inside_menu_pay_by_punishment_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Punishment);
			if (Hero.MainHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (Settlement.CurrentSettlement.IsCastle)
					{
						GameMenu.SwitchToMenu("castle_outside");
						return;
					}
					GameMenu.SwitchToMenu("town_outside");
					return;
				}
				else
				{
					PlayerEncounter.Finish(true);
				}
			}
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x0010D988 File Offset: 0x0010BB88
		public static bool criminal_inside_menu_give_money_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			int num = (int)PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			args.Text.SetTextVariable("FINE", num);
			args.Text.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			if (Hero.MainHero.Gold < num)
			{
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x0010DA0C File Offset: 0x0010BC0C
		public static void criminal_inside_menu_give_money_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x0010DA40 File Offset: 0x0010BC40
		public static bool criminal_inside_menu_give_influence_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Bribe;
			float clearCrimeCost = PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
			args.Text.SetTextVariable("FINE", clearCrimeCost.ToString("F1"));
			args.Text.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			if (Clan.PlayerClan.Influence < clearCrimeCost)
			{
				args.Tooltip = new TextObject("{=rMagXCrI}You don't have enough influence to get the charges dropped.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x0010DACD File Offset: 0x0010BCCD
		public static void criminal_inside_menu_give_influence_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x0010DB00 File Offset: 0x0010BD00
		public static bool criminal_inside_menu_give_punishment_and_money_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			int num = (int)PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			args.Text.SetTextVariable("FINE", num);
			args.Text.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			if (Hero.MainHero.Gold < num)
			{
				args.Tooltip = new TextObject("{=ETKyjOkJ}You don't have enough denars to pay the fine.", null);
				args.IsEnabled = false;
			}
			else
			{
				args.Tooltip = new TextObject("{=yGvsbUWc}Beware that you may die from punishment!", null);
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment);
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x0010DB98 File Offset: 0x0010BD98
		public static void criminal_inside_menu_give_punishment_and_money_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment);
			if (Hero.MainHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (Settlement.CurrentSettlement.IsCastle)
					{
						GameMenu.SwitchToMenu("castle_outside");
						return;
					}
					GameMenu.SwitchToMenu("town_outside");
					return;
				}
				else
				{
					PlayerEncounter.Finish(true);
				}
			}
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0010DBF6 File Offset: 0x0010BDF6
		public static bool criminal_inside_menu_give_your_life_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Execution);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x0010DC10 File Offset: 0x0010BE10
		public static void criminal_inside_menu_give_your_life_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Execution);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0010DC22 File Offset: 0x0010BE22
		public static bool criminal_inside_menu_ignore_charges_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x0010DC3B File Offset: 0x0010BE3B
		public static void criminal_inside_menu_ignore_charges_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x0010DC5E File Offset: 0x0010BE5E
		public static void town_discuss_criminal_surrender_back_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_guard");
				return;
			}
			GameMenu.SwitchToMenu("town_guard");
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0010DC81 File Offset: 0x0010BE81
		public static bool town_discuss_criminal_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}
	}
}
