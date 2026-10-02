using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003DA RID: 986
	public class CaravanConversationsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B19 RID: 15129 RVA: 0x000F5A58 File Offset: 0x000F3C58
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x000F5A71 File Offset: 0x000F3C71
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x000F5A73 File Offset: 0x000F3C73
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x000F5A7C File Offset: 0x000F3C7C
		protected void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddPlayerLine("caravan_create_conversation_1", "hero_main_options", "magistrate_form_a_caravan_cost", "{=!}{CARAVAN_BUY_INTENT_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_build_on_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(this.conversation_caravan_build_clickable_condition), null);
			starter.AddDialogLine("caravan_create_conversation_2", "magistrate_form_a_caravan_cost", "magistrate_form_a_caravan_player_answer", "{=!}{CARAVAN_FORMING_INFO_1}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_cost_on_condition), null, 100, null);
			starter.AddPlayerLine("caravan_create_conversation_3", "magistrate_form_a_caravan_player_answer", "lord_pretalk", "{=otVPaR6T}Actually I do not have a free companion right now.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_caravan_companion_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_4", "magistrate_form_a_caravan_player_answer", "lord_pretalk", "{=w6WFuDn0}I am sorry, I don't have that much money.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_caravan_gold_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_5", "magistrate_form_a_caravan_player_answer", "magistrate_form_a_caravan_accepted", "{=!}{FORM_CARAVAN_ACCEPT}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_small_caravan_accept_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_magistrate_form_a_small_caravan_accept_on_consequence), 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_6", "magistrate_form_a_caravan_player_answer", "magistrate_form_a_caravan_big", "{=!}{LARGE_CARAVAN_OFFER}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_large_caravan_accept_on_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_10", "magistrate_form_a_caravan_player_answer", "lord_pretalk", "{=2mJjDTAZ}That sounds expensive.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_reject_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_create_conversation_7", "magistrate_form_a_caravan_big", "magistrate_form_a_caravan_big_player_answer", "{=DaBzJkIz}I can increase quality of troops, but cost will proportionally increase, too. It will cost {AMOUNT}{GOLD_ICON}.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_offer_condition), null, 100, null);
			starter.AddPlayerLine("caravan_create_conversation_8", "magistrate_form_a_caravan_big_player_answer", "magistrate_form_a_caravan_accepted", "{=!}{CREATE_LARGE_CARAVAN_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_accept_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_magistrate_form_a_big_caravan_accept_on_consequence), 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_9", "magistrate_form_a_caravan_big_player_answer", "lord_pretalk", "{=w6WFuDn0}I am sorry, I don't have that much money.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_gold_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_10_2", "magistrate_form_a_caravan_big_player_answer", "lord_pretalk", "{=2mJjDTAZ}That sounds expensive.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_reject_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_create_conversation_11", "magistrate_form_a_caravan_accepted", "magistrate_form_a_caravan_accepted_choose_leader", "{=!}{CARAVAN_LEADER_CHOOSE_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_accepted_choose_leader_on_condition), null, 100, null);
			starter.AddRepeatablePlayerLine("caravan_create_conversation_12", "magistrate_form_a_caravan_accepted_choose_leader", "magistrate_form_a_caravan_accepted_leader_is_chosen", "{=!}{HERO.NAME}", "{=UNFE1BeG}I am thinking of a different person", "magistrate_form_a_caravan_accepted", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_accepted_leader_is_chosen_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_magistrate_form_a_caravan_accept_on_consequence), 100, null);
			starter.AddPlayerLine("caravan_create_conversation_13", "magistrate_form_a_caravan_accepted_choose_leader", "lord_pretalk", "{=PznWhAdU}Actually, never mind.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_create_conversation_14", "magistrate_form_a_caravan_accepted_leader_is_chosen", "close_window", "{=!}{CARAVAN_NOTABLE_FINAL_TALK}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_final_conversation_on_condition), null, 100, null);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x000F5D44 File Offset: 0x000F3F44
		private bool conversation_caravan_build_on_condition()
		{
			bool flag = Hero.OneToOneConversationHero != null && (Hero.OneToOneConversationHero.IsMerchant || Hero.OneToOneConversationHero.IsArtisan);
			if (flag)
			{
				if (this.ShouldCreateConvoy())
				{
					MBTextManager.SetTextVariable("CARAVAN_BUY_INTENT_TEXT", "{=7l4I06Hi}I wish to form a trade convoy in this town.", false);
					return flag;
				}
				MBTextManager.SetTextVariable("CARAVAN_BUY_INTENT_TEXT", "{=tuz8ZNT6}I wish to form a caravan in this town.", false);
			}
			return flag;
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x000F5DA0 File Offset: 0x000F3FA0
		private bool conversation_caravan_build_clickable_condition(out TextObject explanation)
		{
			if (Campaign.Current.IsMainHeroDisguised)
			{
				explanation = new TextObject("{=jcEoUPCB}You are in disguise.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x000F5DC1 File Offset: 0x000F3FC1
		private bool conversation_magistrate_form_a_caravan_cost_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("CARAVAN_FORMING_INFO_1", "{=OvtzH0b3}Well.. There are many goods around the town that can bring good money if you trade them. A trade convoy you formed will do this for you. You need to pay at least {AMOUNT}{GOLD_ICON} to hire guards to form a trade convoy and you need one companion to lead the convoy guards.", false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_FORMING_INFO_1", "{=cZptYTYd}Well.. There are many goods around the town that can bring good money if you trade them. A caravan you formed will do this for you. You need to pay at least {AMOUNT}{GOLD_ICON} to hire caravan guards to form a caravan and you need one companion to lead the caravan guards.", false);
			}
			MBTextManager.SetTextVariable("AMOUNT", this.GetSmallCaravanGoldCost());
			return true;
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x000F5DFE File Offset: 0x000F3FFE
		private bool conversation_magistrate_form_caravan_companion_condition()
		{
			return this.FindSuitableCompanionsToLeadCaravan().Count == 0;
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000F5E0E File Offset: 0x000F400E
		private bool conversation_magistrate_form_caravan_gold_condition()
		{
			return this.FindSuitableCompanionsToLeadCaravan().Count > 0 && Hero.MainHero.Gold < this.GetSmallCaravanGoldCost();
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x000F5E34 File Offset: 0x000F4034
		private bool conversation_magistrate_form_a_small_caravan_accept_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("FORM_CARAVAN_ACCEPT", new TextObject("{=JNZwJaJ9}I accept these conditions and I am ready to pay {AMOUNT}{GOLD_ICON} to create a trade convoy.", null), false);
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=V8bxlnSl}Is there a way to form a trade convoy that includes better troops?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("FORM_CARAVAN_ACCEPT", new TextObject("{=zOp48Fsg}I accept these conditions and I am ready to pay {AMOUNT}{GOLD_ICON} to create a caravan.", null), false);
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=4mhOs9Fb}Is there a way to form a caravan that includes better troops?", null), false);
			}
			MBTextManager.SetTextVariable("AMOUNT", this.GetSmallCaravanGoldCost());
			return this.FindSuitableCompanionsToLeadCaravan().Count > 0 && Hero.MainHero.Gold >= this.GetSmallCaravanGoldCost();
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x000F5ED8 File Offset: 0x000F40D8
		private bool conversation_magistrate_form_a_large_caravan_accept_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=V8bxlnSl}Is there a way to form a trade convoy that includes better troops?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=4mhOs9Fb}Is there a way to form a caravan that includes better troops?", null), false);
			}
			return this.conversation_magistrate_form_a_small_caravan_accept_on_condition();
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x000F5F16 File Offset: 0x000F4116
		private void conversation_magistrate_form_a_small_caravan_accept_on_consequence()
		{
			this._selectedCaravanType = 0;
			this.conversation_magistrate_form_a_caravan_accepted_on_consequence();
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000F5F25 File Offset: 0x000F4125
		private void conversation_magistrate_form_a_caravan_accepted_on_consequence()
		{
			ConversationSentence.SetObjectsToRepeatOver(this.FindSuitableCompanionsToLeadCaravan(), 5);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x000F5F33 File Offset: 0x000F4133
		private bool conversation_magistrate_form_a_caravan_reject_on_condition()
		{
			return Hero.MainHero.Gold >= this.GetSmallCaravanGoldCost();
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x000F5F4A File Offset: 0x000F414A
		private bool conversation_magistrate_form_a_big_caravan_offer_condition()
		{
			MBTextManager.SetTextVariable("AMOUNT", this.GetLargeCaravanGoldCost());
			return true;
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x000F5F60 File Offset: 0x000F4160
		private bool conversation_magistrate_form_a_big_caravan_accept_on_condition()
		{
			TextObject textObject = TextObject.GetEmpty();
			if (this.ShouldCreateConvoy())
			{
				textObject = new TextObject("{=XxQzR39f}Okay then lets go with better troops, I am ready to pay {AMOUNT}{GOLD_ICON} to create a trade convoy.", null);
			}
			else
			{
				textObject = new TextObject("{=AuMLELpp}Okay then lets go with better troops, I am ready to pay {AMOUNT}{GOLD_ICON} to create a caravan.", null);
			}
			MBTextManager.SetTextVariable("AMOUNT", this.GetLargeCaravanGoldCost());
			MBTextManager.SetTextVariable("CREATE_LARGE_CARAVAN_TEXT", textObject, false);
			return this.FindSuitableCompanionsToLeadCaravan().Count > 0 && Hero.MainHero.Gold >= this.GetLargeCaravanGoldCost();
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x000F5FD6 File Offset: 0x000F41D6
		private bool conversation_magistrate_form_a_big_caravan_gold_condition()
		{
			return Hero.MainHero.Gold < this.GetLargeCaravanGoldCost();
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x000F5FEA File Offset: 0x000F41EA
		private void conversation_magistrate_form_a_big_caravan_accept_on_consequence()
		{
			this._selectedCaravanType = 1;
			this.conversation_magistrate_form_a_caravan_accepted_on_consequence();
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000F5FF9 File Offset: 0x000F41F9
		private bool conversation_magistrate_form_a_big_caravan_reject_on_condition()
		{
			return Hero.MainHero.Gold >= this.GetLargeCaravanGoldCost();
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x000F6010 File Offset: 0x000F4210
		private bool conversation_magistrate_form_a_caravan_accepted_choose_leader_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("CARAVAN_LEADER_CHOOSE_TEXT", new TextObject("{=Ww7vJSb9}Whom do you want to lead the convoy?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_LEADER_CHOOSE_TEXT", new TextObject("{=aeCYFe1g}Whom do you want to lead the caravan?", null), false);
			}
			return true;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000F6049 File Offset: 0x000F4249
		private bool conversation_magistrate_form_a_caravan_final_conversation_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("CARAVAN_NOTABLE_FINAL_TALK", new TextObject("{=2WFPZrFf}Ok then. I will call my men to help you form a trade convoy. I hope it brings you a good profit.", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_NOTABLE_FINAL_TALK", new TextObject("{=Z2Lq2QLq}Ok then. I will call my men to help you form a caravan. I hope it brings you a good profit.", null), false);
			}
			return true;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x000F6084 File Offset: 0x000F4284
		private bool conversation_magistrate_form_a_caravan_accepted_leader_is_chosen_on_condition()
		{
			CharacterObject characterObject = ConversationSentence.CurrentProcessedRepeatObject as CharacterObject;
			if (characterObject != null)
			{
				StringHelpers.SetRepeatableCharacterProperties("HERO", characterObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000F60B0 File Offset: 0x000F42B0
		private void conversation_magistrate_form_a_caravan_accept_on_consequence()
		{
			CharacterObject characterObject = ConversationSentence.SelectedRepeatObject as CharacterObject;
			this.FadeOutSelectedCaravanCompanionInMission(characterObject);
			LeaveSettlementAction.ApplyForCharacterOnly(characterObject.HeroObject);
			bool flag = this._selectedCaravanType == 1;
			PartyTemplateObject randomCaravanTemplate = CaravanHelper.GetRandomCaravanTemplate(Settlement.CurrentSettlement.Culture, flag, !this.ShouldCreateConvoy());
			CaravanPartyComponent.CreateCaravanParty(Hero.MainHero, Settlement.CurrentSettlement, randomCaravanTemplate, false, characterObject.HeroObject, null, flag);
			GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, Settlement.CurrentSettlement, (!flag) ? this.GetSmallCaravanGoldCost() : this.GetLargeCaravanGoldCost(), false);
			TextObject textObject;
			if (this.ShouldCreateConvoy())
			{
				textObject = new TextObject("{=c7VOPmSb}A new trade convoy is created for {HERO.NAME}.", null);
			}
			else
			{
				textObject = new TextObject("{=RmtTsqcx}A new caravan is created for {HERO.NAME}.", null);
			}
			StringHelpers.SetCharacterProperties("HERO", Hero.MainHero.CharacterObject, textObject, false);
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000F6182 File Offset: 0x000F4382
		private void FadeOutSelectedCaravanCompanionInMission(CharacterObject caravanLeader)
		{
			ICampaignMission campaignMission = CampaignMission.Current;
			if (campaignMission == null)
			{
				return;
			}
			campaignMission.FadeOutCharacter(caravanLeader);
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000F6194 File Offset: 0x000F4394
		private List<CharacterObject> FindSuitableCompanionsToLeadCaravan()
		{
			List<CharacterObject> list = new List<CharacterObject>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				Hero heroObject = troopRosterElement.Character.HeroObject;
				if (heroObject != null && heroObject != Hero.MainHero && heroObject.Clan == Clan.PlayerClan && heroObject.GovernorOf == null && heroObject.CanLeadParty())
				{
					list.Add(troopRosterElement.Character);
				}
			}
			return list;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000F6230 File Offset: 0x000F4430
		private bool ShouldCreateConvoy()
		{
			if (Settlement.CurrentSettlement == null)
			{
				Debug.FailedAssert("Current settlement is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CaravanConversationsCampaignBehavior.cs", "ShouldCreateConvoy", 302);
				return false;
			}
			return Settlement.CurrentSettlement.HasPort;
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000F625E File Offset: 0x000F445E
		private int GetLargeCaravanGoldCost()
		{
			if (!this.ShouldCreateConvoy())
			{
				return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(true, false);
			}
			return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(true, true);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000F6295 File Offset: 0x000F4495
		private int GetSmallCaravanGoldCost()
		{
			if (!this.ShouldCreateConvoy())
			{
				return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(false, false);
			}
			return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(false, true);
		}

		// Token: 0x0400124D RID: 4685
		private int _selectedCaravanType;
	}
}
