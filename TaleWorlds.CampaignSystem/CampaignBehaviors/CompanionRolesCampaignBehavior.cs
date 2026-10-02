using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E1 RID: 993
	public class CompanionRolesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06003D10 RID: 15632 RVA: 0x00107CB1 File Offset: 0x00105EB1
		private CompanionRolesCampaignBehavior CurrentBehavior
		{
			get
			{
				return Campaign.Current.GetCampaignBehavior<CompanionRolesCampaignBehavior>();
			}
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x00107CC0 File Offset: 0x00105EC0
		public override void RegisterEvents()
		{
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x00107D12 File Offset: 0x00105F12
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<int>>("_alreadyUsedIconIdsForNewClans", ref this._alreadyUsedIconIdsForNewClans);
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x00107D28 File Offset: 0x00105F28
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (((effectiveHero == Hero.MainHero && effectiveHeroGainedRelationWith.IsPlayerCompanion) || (effectiveHero.IsPlayerCompanion && effectiveHeroGainedRelationWith == Hero.MainHero)) && relationChange < 0 && effectiveHero.GetRelation(effectiveHeroGainedRelationWith) < -10)
			{
				KillCharacterAction.ApplyByRemove(effectiveHero.IsPlayerCompanion ? effectiveHero : effectiveHeroGainedRelationWith, false, true);
			}
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x00107D77 File Offset: 0x00105F77
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x00107D80 File Offset: 0x00105F80
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddPlayerLine("companion_rejoin_after_emprisonment_role", "hero_main_options", "companion_rejoin", "{=!}{COMPANION_REJOIN_LINE}", new ConversationSentence.OnConditionDelegate(this.companion_rejoin_after_emprisonment_role_on_condition), delegate
			{
				Campaign.Current.ConversationManager.ConversationEnd += this.companion_rejoin_after_emprisonment_role_on_consequence;
			}, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_rejoin", "companion_rejoin", "close_window", "{=ppi6eVos}As you wish.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_start_role", "hero_main_options", "companion_role_pretalk", "{=d4t6oUCn}About your position in the clan...", new ConversationSentence.OnConditionDelegate(this.companion_role_discuss_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_pretalk", "companion_role_pretalk", "companion_role", "{=!}{COMPANION_ROLE}", new ConversationSentence.OnConditionDelegate(this.companion_has_role_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire", "companion_role", "companion_fire", "{=pRsCnGoo}I no longer have need of your services.", new ConversationSentence.OnConditionDelegate(this.companion_fire_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire_2", "companion_role", "companion_assign_new_role", "{=2g18dlwo}I would like to assign you a new role.", new ConversationSentence.OnConditionDelegate(this.companion_assign_role_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire_3", "companion_role", "too_many_roles", "{=2g18dlwo}I would like to assign you a new role.", new ConversationSentence.OnConditionDelegate(this.companion_assign_but_too_many_role_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_assign_new_role", "companion_assign_new_role", "companion_roles", "{=5ajobQiL}What role do you have in mind?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire_3", "companion_role", "companion_okay", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer", "companion_roles", "companion_okay", "{=E91oU7oi}I no longer need you as Engineer.", new ConversationSentence.OnConditionDelegate(this.companion_fire_engineer_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_engineer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_surgeon", "companion_roles", "companion_okay", "{=Dga7sQOu}I no longer need you as Surgeon.", new ConversationSentence.OnConditionDelegate(this.companion_fire_surgeon_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_surgeon_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_quartermaster", "companion_roles", "companion_okay", "{=GjpJN2xE}I no longer need you as Quartermaster.", new ConversationSentence.OnConditionDelegate(this.companion_fire_quartermaster_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_quartermaster_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_scout", "companion_roles", "companion_okay", "{=EUQnsZFb}I no longer need you as Scout.", new ConversationSentence.OnConditionDelegate(this.companion_fire_scout_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_scout_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("companion_role_response", "companion_okay", "hero_main_options", "{=dzXaXKaC}Very well.", null, null, 1, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer_2", "companion_roles", "give_companion_roles", "{=UuFPafDj}Engineer {CURRENTLY_HELD_ENGINEER}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_engineer_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_engineer_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_engineer_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("companion_becomes_surgeon_2", "companion_roles", "give_companion_roles", "{=6xZ8U3Yz}Surgeon {CURRENTLY_HELD_SURGEON}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_surgeon_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_surgeon_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_surgeon_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("companion_becomes_quartermaster_2", "companion_roles", "give_companion_roles", "{=B0VLXHHz}Quartermaster {CURRENTLY_HELD_QUARTERMASTER}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_quartermaster_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_quartermaster_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_quartermaster_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("companion_becomes_scout_2", "companion_roles", "give_companion_roles", "{=3aziL3Gs}Scout {CURRENTLY_HELD_SCOUT}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_scout_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_scout_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_scout_clickable_condition), null);
			campaignGameStarter.AddDialogLine("companion_role_response_2", "give_companion_roles", "hero_main_options", "{=5hhxQBTj}I would be honored.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("companion_have_too_many_roles", "too_many_roles", "too_many_roles_responses", "{=m3AsvplJ}I already have quite a few duties. Perhaps you could relieve me of one of them, so that I can take on this new responsibility?", null, null, 1, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=E91oU7oi}I no longer need you as Engineer.", new ConversationSentence.OnConditionDelegate(this.companion_fire_engineer_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_engineer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_surgeon_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=Dga7sQOu}I no longer need you as Surgeon.", new ConversationSentence.OnConditionDelegate(this.companion_fire_surgeon_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_surgeon_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_quartermaster_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=GjpJN2xE}I no longer need you as Quartermaster.", new ConversationSentence.OnConditionDelegate(this.companion_fire_quartermaster_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_quartermaster_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_scout_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=EUQnsZFb}I no longer need you as Scout.", new ConversationSentence.OnConditionDelegate(this.companion_fire_scout_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_scout_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer", "too_many_roles_responses", "hero_main_options", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_role_unassign_response", "companion_okay_to_role_selection", "companion_assign_new_role", "{=dzXaXKaC}Very well.", null, null, 1, null);
			campaignGameStarter.AddPlayerLine("companion_talk_return", "companion_roles", "companion_okay", "{=D33fIGQe}Never mind.", null, null, 1, null, null);
			campaignGameStarter.AddDialogLine("companion_start_mission", "hero_main_options", "companion_mission_pretalk", "{=4ry48jbg}I have a mission for you...", () => HeroHelper.IsCompanionInPlayerParty(Hero.OneToOneConversationHero), null, 100, null);
			campaignGameStarter.AddDialogLine("companion_pretalk_2", "companion_mission_pretalk", "companion_mission", "{=7EoBCTX0}What do you want me to do?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_mission_gather_troops", "companion_mission", "companion_recruit_troops", "{=MDik3Kfn}I want you to recruit some troops.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_mission_forage", "companion_mission", "companion_forage", "{=kAbebv72}I want you to go forage some food.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_mission_patrol", "companion_mission", "companion_patrol", "{=OMaM6ihN}I want you to patrol the area.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_mission_cancel", "companion_mission", "hero_main_options", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_forage_1", "companion_forage", "companion_forage_2", "{=o2g6Wi9K}As you wish. Will I take some troops with me?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_forage_2", "companion_forage_2", "companion_forage_troops", "{=lVbQCibL}Yes. Take these troops with you.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_forage_3", "companion_forage_2", "companion_forage_3", "{=3bOcF1Cw}I can't spare anyone now. You will need to go alone.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_fire", "companion_fire", "companion_fire2", "{=bUzU50P8}What? Why? Did I do something wrong?[ib:closed]", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_fire_age", "companion_fire2", "companion_fire3", "{=ywtuRAmP}Time has taken its toll on us all, friend. It's time that you retire.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_no_fit", "companion_fire2", "companion_fire3", "{=1s3bHupn}You're not getting along with the rest of the company. It's better you go.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_no_fit_2", "companion_fire2", "companion_fire3", "{=Q0xPr6CP}I cannot be sure of your loyalty any longer.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_underperforming", "companion_fire2", "companion_fire3", "{=aCwCaWGC}Your skills are not what I need.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_cancel", "companion_fire2", "companion_fire_cancel", "{=8VlqJteC}I was just jesting. I need you more than ever. Now go back to your job.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_talk_done_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("companion_fire_cancel2", "companion_fire_cancel", "close_window", "{=vctta154}Well {PLAYER.NAME}, it is certainly good to see you still retain your sense of humor.[if:convo_nervous][ib:normal2]", null, null, 100, null);
			campaignGameStarter.AddDialogLine("companion_fire_farewell", "companion_fire3", "close_window", "{=!}{AGREE_TO_LEAVE}[ib:nervous2]", new ConversationSentence.OnConditionDelegate(this.companion_agrees_to_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_start", "hero_main_options", "turn_companion_to_lord_talk_answer", "{=B9uT9wa6}I wish to reward you for your services.", new ConversationSentence.OnConditionDelegate(this.turn_companion_to_lord_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_start_answer_2", "turn_companion_to_lord_talk_answer", "companion_leading_caravan", "{=IkH0pVhC}I would be honored, my {?PLAYER.GENDER}lady{?}lord{\\?}. But I can't take on any new responsibilities while leading this caravan. If you wish to relieve me of my duties, we can discuss this further.", new ConversationSentence.OnConditionDelegate(this.companion_is_leading_caravan_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_start_answer_player", "companion_leading_caravan", "lord_pretalk", "{=i7k0AXsO}I see. We will speak again when you are relieved from your duty.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_start_answer", "turn_companion_to_lord_talk_answer", "turn_companion_to_lord_talk", "{=TXO1ihiZ}Thank you, my {?PLAYER.GENDER}lady{?}lord{\\?}. I have often thought about that. If I had a fief, with revenues, and perhaps a title to go with it, I could marry well and pass my wealth down to my heirs, and of course raise troops to help defend the realm.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_has_fief", "turn_companion_to_lord_talk", "check_player_has_fief_to_grant", "{=KqazzTWV}Indeed. You have shed your blood for me, and you deserve a fief of your own..", null, new ConversationSentence.OnConsequenceDelegate(this.fief_grant_answer_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_has_no_fief", "check_player_has_fief_to_grant", "player_has_no_fief_to_grant", "{=Wx5ysDp1}My {?PLAYER.GENDER}lady{?}lord{\\?}, as much as I appreciate the gesture, I am not sure that you have a suitable estate to grant me.", new ConversationSentence.OnConditionDelegate(this.turn_companion_to_lord_no_fief_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_has_no_fief_player_answer", "player_has_no_fief_to_grant", "player_has_no_fief_to_grant_answer", "{=6uUzWz46}I see. Maybe we will speak again when I have one.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_has_no_fief_companion_answer", "player_has_no_fief_to_grant_answer", "hero_main_options", "{=PP3LzCKk}As you wish, my {?PLAYER.GENDER}lady{?}lord{\\?}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_has_fief_answer", "check_player_has_fief_to_grant", "player_has_fief_list", "{=ArNB7aaL}Where exactly did you have in mind?[if:convo_happy]", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("turn_companion_to_lord_has_fief_list", "player_has_fief_list", "player_selected_fief_to_grant", "{=3rHeoq6r}{SETTLEMENT_NAME}.", "{=sxc2D6NJ}I am thinking of a different location.", "check_player_has_fief_to_grant", new ConversationSentence.OnConditionDelegate(this.list_player_fief_on_condition), new ConversationSentence.OnConsequenceDelegate(this.list_player_fief_selected_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.list_player_fief_clickable_condition));
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_has_fief_list_cancel", "player_has_fief_list", "turn_companion_to_lord_fief_conclude", "{=UEbesbKZ}Actually, I have changed my mind.", null, new ConversationSentence.OnConsequenceDelegate(this.list_player_fief_cancel_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_fief_selected", "player_selected_fief_to_grant", "turn_companion_to_lord_fief_selected_answer", "{=Mt9abZzi}{SETTLEMENT_NAME}? This is a great honor, my {?PLAYER.GENDER}lady{?}lord{\\?}. I will protect it until the last drop of my blood.[ib:hip][if:convo_happy]", new ConversationSentence.OnConditionDelegate(this.fief_selected_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_fief_selected_confirm", "turn_companion_to_lord_fief_selected_answer", "turn_companion_to_lord_fief_selected_confirm_box", "{=TtlwXnVc}I am pleased to grant you the title of {CULTURE_SPECIFIC_TITLE} and the fiefdom of {SETTLEMENT_NAME}.. You richly deserve it.", null, null, 100, new ConversationSentence.OnClickableConditionDelegate(this.fief_selected_confirm_clickable_on_condition), null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_fief_selected_reject", "turn_companion_to_lord_fief_selected_answer", "turn_companion_to_lord_fief_conclude", "{=LDGMSQJJ}Very well. Let me think on this a bit longer", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_fief_selected_confirm_box", "turn_companion_to_lord_fief_selected_confirm_box", "turn_companion_to_lord_fief_conclude", "{=LOiZfCEy}My {?PLAYER.GENDER}lady{?}lord{\\?}, it would be an honor if you were to choose the name of my noble house.", null, new ConversationSentence.OnConsequenceDelegate(this.turn_companion_to_lord_consequence), 100, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_done_answer_thanks", "turn_companion_to_lord_fief_conclude", "close_window", "{=dpYhBgAC}Thank you my {?PLAYER.GENDER}lady{?}lord{\\?}. I will always remember this grand gesture.[ib:hip][if:convo_happy]", new ConversationSentence.OnConditionDelegate(this.companion_thanks_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_talk_done_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_done_answer_rejected", "turn_companion_to_lord_fief_conclude", "hero_main_options", "{=SVEptNxR}It's only normal that you have second thoughts. I will be right by your side if you change your mind, my {?PLAYER.GENDER}lady{?}lord{\\?}.[ib:hip][if:convo_nervous]", null, new ConversationSentence.OnConsequenceDelegate(this.companion_talk_done_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_start", "start", "rescue_companion_option_acknowledgement", "{=FVOfzPot}{SALUTATION}... Thank you for freeing me.", new ConversationSentence.OnConditionDelegate(this.companion_rescue_start_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_acknowledgement", "rescue_companion_option_acknowledgement", "rescue_companion_preoptions", "{=YyNywO6Z}Think nothing of it. I'm glad you're safe.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("rescue_companion_preoptions", "rescue_companion_preoptions", "rescue_companion_options", "{=kaVMFgBs}What now?", new ConversationSentence.OnConditionDelegate(this.companion_rescue_start_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_1", "rescue_companion_options", "rescue_companion_join_party", "{=drIfaTa7}Rejoin the others and let's be off.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_rescue_answer_options_join_party_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_2", "rescue_companion_options", "rescue_companion_lead_party", "{=Y6Z8qNW9}I'll need you to lead a party.", null, null, 100, new ConversationSentence.OnClickableConditionDelegate(this.lead_a_party_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_3", "rescue_companion_options", "rescue_companion_do_nothing", "{=dRKk0E1V}Unfortunately, I can't take you back right now.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_answer", "rescue_companion_lead_party", "close_window", "{=Q9Ltufg5}Tell me who to command.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_rescue_answer_options_lead_party_consequence), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_join_party_answer", "rescue_companion_join_party", "close_window", "{=92mngWSd}All right. It's good to be back.", null, new ConversationSentence.OnConsequenceDelegate(this.end_rescue_companion), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_do_nothing_answer", "rescue_companion_do_nothing", "close_window", "{=gT2O4YXc}I will go off on my own, then. I can stay busy. But I'll remember - I owe you one!", null, new ConversationSentence.OnConsequenceDelegate(this.end_rescue_companion), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_create_party_continue_0", "start", "party_screen_rescue_continue", "{=ppi6eVos}As you wish.", new ConversationSentence.OnConditionDelegate(this.party_screen_continue_conversation_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_create_party_continue_1", "party_screen_rescue_continue", "rescue_companion_options", "{=ttWBYlxS}So, what shall I do?", new ConversationSentence.OnConditionDelegate(this.party_screen_opened_but_party_is_not_created_after_rescue_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_create_party_continue_2", "party_screen_rescue_continue", "close_window", "{=DiEKuVGF}We'll make ready to set out at once.", new ConversationSentence.OnConditionDelegate(this.party_screen_opened_and_party_is_created_after_rescue_condition), new ConversationSentence.OnConsequenceDelegate(this.end_rescue_companion), 100, null);
			campaignGameStarter.AddDialogLine("default_conversation_for_wrongly_created_heroes", "start", "close_window", "{=BaeqKlQ6}I am not allowed to talk with you.", null, null, 0, null);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x001089CF File Offset: 0x00106BCF
		private bool companion_fire_condition()
		{
			return Hero.OneToOneConversationHero.IsPlayerCompanion && Settlement.CurrentSettlement == null && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsInRaftState);
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00108A06 File Offset: 0x00106C06
		private bool turn_companion_to_lord_no_fief_on_condition()
		{
			return !Hero.MainHero.Clan.Settlements.Any<Settlement>((Settlement x) => x.IsTown || x.IsCastle);
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00108A40 File Offset: 0x00106C40
		private bool turn_companion_to_lord_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			if (oneToOneConversationHero != null && oneToOneConversationHero.IsPlayerCompanion && Hero.MainHero.IsKingdomLeader)
			{
				MobileParty partyBelongedTo = oneToOneConversationHero.PartyBelongedTo;
				if (partyBelongedTo == null || !partyBelongedTo.IsCurrentlyAtSea)
				{
					this.CurrentBehavior._playerConfirmedTheAction = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x00108A90 File Offset: 0x00106C90
		private bool companion_is_leading_caravan_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.IsPlayerCompanion && oneToOneConversationHero.PartyBelongedTo != null && oneToOneConversationHero.PartyBelongedTo.IsCaravan;
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x00108AC3 File Offset: 0x00106CC3
		private void fief_grant_answer_consequence()
		{
			ConversationSentence.SetObjectsToRepeatOver(Hero.MainHero.Clan.Settlements.Where<Settlement>((Settlement x) => x.IsTown || x.IsCastle).ToList<Settlement>(), 5);
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00108B04 File Offset: 0x00106D04
		private bool list_player_fief_clickable_condition(out TextObject explanation)
		{
			Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
			Settlement fief = ConversationSentence.CurrentProcessedRepeatObject as Settlement;
			if (fief.SiegeEvent != null)
			{
				explanation = new TextObject("{=arCGUuR5}The settlement is under siege.", null);
				return false;
			}
			if (fief.Town.IsOwnerUnassigned || kingdom.UnresolvedDecisions.Any<KingdomDecision>(delegate(KingdomDecision x)
			{
				SettlementClaimantDecision settlementClaimantDecision;
				SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
				return ((settlementClaimantDecision = x as SettlementClaimantDecision) != null && settlementClaimantDecision.Settlement == fief) || ((settlementClaimantPreliminaryDecision = x as SettlementClaimantPreliminaryDecision) != null && settlementClaimantPreliminaryDecision.Settlement == fief);
			}))
			{
				explanation = new TextObject("{=OiPqa3L8}This settlement's ownership will be decided through voting.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00108B94 File Offset: 0x00106D94
		private bool list_player_fief_on_condition()
		{
			Settlement settlement = ConversationSentence.CurrentProcessedRepeatObject as Settlement;
			if (settlement != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("SETTLEMENT_NAME", settlement.Name);
			}
			return true;
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00108BC6 File Offset: 0x00106DC6
		private void list_player_fief_selected_on_consequence()
		{
			this._selectedFief = ConversationSentence.SelectedRepeatObject as Settlement;
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x00108BD8 File Offset: 0x00106DD8
		private void turn_companion_to_lord_consequence()
		{
			TextObject textObject = new TextObject("{=ntDH7J3H}This action costs {NEEDED_GOLD_TO_GRANT_FIEF}{GOLD_ICON} and {NEEDED_INFLUENCE_TO_GRANT_FIEF}{INFLUENCE_ICON}. You will also be granting {SETTLEMENT} to {COMPANION.NAME}.", null);
			textObject.SetTextVariable("NEEDED_GOLD_TO_GRANT_FIEF", 20000);
			textObject.SetTextVariable("NEEDED_INFLUENCE_TO_GRANT_FIEF", 500);
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			textObject.SetCharacterProperties("COMPANION", Hero.OneToOneConversationHero.CharacterObject, false);
			textObject.SetTextVariable("SETTLEMENT", this.CurrentBehavior._selectedFief.Name);
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=awjomtnJ}Are you sure?", null).ToString(), textObject.ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.ConfirmTurningCompanionToLordConsequence), new Action(this.RejectTurningCompanionToLordConsequence), "", 0f, null, null, null), false, false);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x00108CD0 File Offset: 0x00106ED0
		private void ConfirmTurningCompanionToLordConsequence()
		{
			this.CurrentBehavior._playerConfirmedTheAction = true;
			object obj = new TextObject("{=4eStbG4S}Select {COMPANION.NAME}{.o} clan name: ", null);
			StringHelpers.SetCharacterProperties("COMPANION", Hero.OneToOneConversationHero.CharacterObject, null, false);
			InformationManager.ShowTextInquiry(new TextInquiryData(obj.ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(this.ClanNameSelectionIsDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", ""), false, false);
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x00108D58 File Offset: 0x00106F58
		private void RejectTurningCompanionToLordConsequence()
		{
			this.CurrentBehavior._playerConfirmedTheAction = false;
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00108D78 File Offset: 0x00106F78
		private void ClanNameSelectionIsDone(string clanName)
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			RemoveCompanionAction.ApplyByByTurningToLord(Hero.MainHero.Clan, oneToOneConversationHero);
			oneToOneConversationHero.SetNewOccupation(Occupation.Lord);
			TextObject textObject = GameTexts.FindText("str_generic_clan_name", null);
			textObject.SetTextVariable("CLAN_NAME", new TextObject(clanName, null));
			int randomBannerIdForNewClan = this.GetRandomBannerIdForNewClan();
			Clan clan = Clan.CreateCompanionToLordClan(oneToOneConversationHero, this.CurrentBehavior._selectedFief, textObject, randomBannerIdForNewClan);
			if (oneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty)
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(oneToOneConversationHero.CharacterObject, -1, false, 0, 0, true, -1);
			}
			MobileParty partyBelongedTo = oneToOneConversationHero.PartyBelongedTo;
			if (partyBelongedTo == null)
			{
				MobileParty mobileParty = LordPartyComponent.CreateLordParty(oneToOneConversationHero.CharacterObject.StringId, oneToOneConversationHero, MobileParty.MainParty.Position, 3f, this.CurrentBehavior._selectedFief, oneToOneConversationHero);
				mobileParty.MemberRoster.AddToCounts(clan.Culture.BasicTroop, MBRandom.RandomInt(12, 15), false, 0, 0, true, -1);
				mobileParty.MemberRoster.AddToCounts(clan.Culture.EliteBasicTroop, MBRandom.RandomInt(10, 15), false, 0, 0, true, -1);
			}
			else
			{
				partyBelongedTo.ActualClan = clan;
				partyBelongedTo.Party.SetVisualAsDirty();
			}
			this.AdjustCompanionsEquipment(oneToOneConversationHero);
			this.SpawnNewHeroesForNewCompanionClan(oneToOneConversationHero, clan, this.CurrentBehavior._selectedFief);
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, oneToOneConversationHero, 20000, false);
			GainKingdomInfluenceAction.ApplyForDefault(Hero.MainHero, -500f);
			ChangeRelationAction.ApplyPlayerRelation(oneToOneConversationHero, 50, true, true);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x00108EF0 File Offset: 0x001070F0
		private void AdjustCompanionsEquipment(Hero companionHero)
		{
			Equipment equipmentForCompanionWhenTurningToLord = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForCompanionWhenTurningToLord(companionHero, Equipment.EquipmentType.Civilian);
			Equipment equipmentForCompanionWhenTurningToLord2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForCompanionWhenTurningToLord(companionHero, Equipment.EquipmentType.Battle);
			Equipment equipment = new Equipment(Equipment.EquipmentType.Civilian);
			Equipment equipment2 = new Equipment(Equipment.EquipmentType.Battle);
			for (int i = 0; i < 12; i++)
			{
				if (equipmentForCompanionWhenTurningToLord2[i].Item != null && (companionHero.BattleEquipment[i].Item == null || companionHero.BattleEquipment[i].Item.Tier < equipmentForCompanionWhenTurningToLord2[i].Item.Tier))
				{
					equipment2[i] = equipmentForCompanionWhenTurningToLord2[i];
				}
				else
				{
					equipment2[i] = companionHero.BattleEquipment[i];
				}
				if (equipmentForCompanionWhenTurningToLord[i].Item != null && (companionHero.CivilianEquipment[i].Item == null || companionHero.CivilianEquipment[i].Item.Tier < equipmentForCompanionWhenTurningToLord[i].Item.Tier))
				{
					equipment[i] = equipmentForCompanionWhenTurningToLord[i];
				}
				else
				{
					equipment[i] = companionHero.CivilianEquipment[i];
				}
			}
			EquipmentHelper.AssignHeroEquipmentFromEquipment(companionHero, equipment);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(companionHero, equipment2);
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00109068 File Offset: 0x00107268
		private int GetRandomBannerIdForNewClan()
		{
			MBReadOnlyList<int> possibleClanBannerIconsIDs = Hero.MainHero.MapFaction.Culture.PossibleClanBannerIconsIDs;
			int num = possibleClanBannerIconsIDs.GetRandomElement<int>();
			if (this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(num))
			{
				int num2 = 0;
				do
				{
					num = possibleClanBannerIconsIDs.GetRandomElement<int>();
					num2++;
				}
				while (this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(num) && num2 < 20);
				bool flag = num2 != 20;
				if (!flag)
				{
					for (int i = 0; i < possibleClanBannerIconsIDs.Count; i++)
					{
						if (!this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(possibleClanBannerIconsIDs[i]))
						{
							num = possibleClanBannerIconsIDs[i];
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					num = possibleClanBannerIconsIDs.GetRandomElement<int>();
				}
			}
			if (!this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(num))
			{
				this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Add(num);
			}
			return num;
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00109144 File Offset: 0x00107344
		private void SpawnNewHeroesForNewCompanionClan(Hero companionHero, Clan clan, Settlement settlement)
		{
			MBReadOnlyList<CharacterObject> lordTemplates = companionHero.Culture.LordTemplates;
			List<Hero> list = new List<Hero>();
			list.Add(this.CreateNewHeroForNewCompanionClan(lordTemplates.GetRandomElement<CharacterObject>(), settlement, new Dictionary<SkillObject, int>
			{
				{
					DefaultSkills.Steward,
					MBRandom.RandomInt(100, 175)
				},
				{
					DefaultSkills.Leadership,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.OneHanded,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.Medicine,
					MBRandom.RandomInt(125, 175)
				}
			}));
			list.Add(this.CreateNewHeroForNewCompanionClan(lordTemplates.GetRandomElement<CharacterObject>(), settlement, new Dictionary<SkillObject, int>
			{
				{
					DefaultSkills.OneHanded,
					MBRandom.RandomInt(100, 175)
				},
				{
					DefaultSkills.Leadership,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.Tactics,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.Engineering,
					MBRandom.RandomInt(125, 175)
				}
			}));
			list.Add(companionHero);
			foreach (Hero hero in list)
			{
				hero.Clan = clan;
				hero.ChangeState(Hero.CharacterStates.Active);
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, Hero.MainHero, MBRandom.RandomInt(5, 10), false);
				if (hero != companionHero)
				{
					EnterSettlementAction.ApplyForCharacterOnly(hero, settlement);
				}
				foreach (Hero hero2 in list)
				{
					if (hero != hero2)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, hero2, MBRandom.RandomInt(5, 10), false);
					}
				}
			}
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00109308 File Offset: 0x00107508
		private Hero CreateNewHeroForNewCompanionClan(CharacterObject templateCharacter, Settlement settlement, Dictionary<SkillObject, int> startingSkills)
		{
			Hero hero = HeroCreator.CreateSpecialHero(templateCharacter, settlement, null, null, MBRandom.RandomInt(Campaign.Current.Models.AgeModel.HeroComesOfAge, 50));
			foreach (KeyValuePair<SkillObject, int> keyValuePair in startingSkills)
			{
				hero.HeroDeveloper.SetInitialSkillLevel(keyValuePair.Key, keyValuePair.Value);
			}
			return hero;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00109390 File Offset: 0x00107590
		private void list_player_fief_cancel_on_consequence()
		{
			this.CurrentBehavior._playerConfirmedTheAction = false;
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x0010939E File Offset: 0x0010759E
		private bool fief_selected_on_condition()
		{
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", this.CurrentBehavior._selectedFief.Name, false);
			return true;
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x001093BC File Offset: 0x001075BC
		private bool companion_thanks_on_condition()
		{
			return this.CurrentBehavior._playerConfirmedTheAction;
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x001093CC File Offset: 0x001075CC
		private bool fief_selected_confirm_clickable_on_condition(out TextObject explanation)
		{
			MBTextManager.SetTextVariable("CULTURE_SPECIFIC_TITLE", HeroHelper.GetTitleInIndefiniteCase(Hero.OneToOneConversationHero), false);
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", this.CurrentBehavior._selectedFief.Name, false);
			bool flag = Hero.MainHero.Gold >= 20000;
			bool flag2 = Hero.MainHero.Clan.Influence >= 500f;
			MBTextManager.SetTextVariable("NEEDED_GOLD_TO_GRANT_FIEF", 20000);
			MBTextManager.SetTextVariable("NEEDED_INFLUENCE_TO_GRANT_FIEF", 500);
			MBTextManager.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">", false);
			MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
			if (flag && flag2)
			{
				explanation = new TextObject("{=PxQEwCha}You will pay {NEEDED_GOLD_TO_GRANT_FIEF}{GOLD_ICON}, {NEEDED_INFLUENCE_TO_GRANT_FIEF}{INFLUENCE_ICON}.", null);
				return true;
			}
			explanation = new TextObject("{=!}{GOLD_REQUIREMENT}{INFLUENCE_REQUIREMENT}", null);
			if (!flag)
			{
				TextObject textObject = new TextObject("{=yo2NvkQQ}You need {NEEDED_GOLD_TO_GRANT_FIEF}{GOLD_ICON}. ", null);
				explanation.SetTextVariable("GOLD_REQUIREMENT", textObject);
			}
			if (!flag2)
			{
				TextObject textObject2 = new TextObject("{=pDeFXZJd}You need {NEEDED_INFLUENCE_TO_GRANT_FIEF}{INFLUENCE_ICON}.", null);
				explanation.SetTextVariable("INFLUENCE_REQUIREMENT", textObject2);
			}
			return false;
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x001094D2 File Offset: 0x001076D2
		private void companion_talk_done_on_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x001094E4 File Offset: 0x001076E4
		private void companion_fire_on_consequence()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			RemoveCompanionAction.ApplyByFire(oneToOneConversationHero.CompanionOf, oneToOneConversationHero);
			KillCharacterAction.ApplyByRemove(oneToOneConversationHero, false, true);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00109518 File Offset: 0x00107718
		private bool companion_rejoin_after_emprisonment_role_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && !Hero.OneToOneConversationHero.IsPartyLeader && (Hero.OneToOneConversationHero.IsPlayerCompanion || Hero.OneToOneConversationHero.Clan == Clan.PlayerClan) && Hero.OneToOneConversationHero.PartyBelongedTo != MobileParty.MainParty && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsCaravan))
			{
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown && Hero.OneToOneConversationHero.GovernorOf == Settlement.CurrentSettlement.Town)
				{
					MBTextManager.SetTextVariable("COMPANION_REJOIN_LINE", "{=Z5zAok5G}I need to recall you to my party, and to stop governing this town.", false);
				}
				else
				{
					MBTextManager.SetTextVariable("COMPANION_REJOIN_LINE", "{=gR0ksbaQ}Get your things. I'd like you to rejoin the party.", false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x001095D7 File Offset: 0x001077D7
		private void companion_rejoin_after_emprisonment_role_on_consequence()
		{
			AddHeroToPartyAction.Apply(Hero.OneToOneConversationHero, MobileParty.MainParty, true);
			Campaign.Current.ConversationManager.ConversationEnd -= this.companion_rejoin_after_emprisonment_role_on_consequence;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00109604 File Offset: 0x00107804
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (LocationComplex.Current != null)
			{
				LocationComplex.Current.RemoveCharacterIfExists(companion);
			}
			if (PlayerEncounter.LocationEncounter != null)
			{
				PlayerEncounter.LocationEncounter.RemoveAccompanyingCharacter(companion);
			}
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x0010962A File Offset: 0x0010782A
		private bool companion_agrees_to_leave_on_condition()
		{
			MBTextManager.SetTextVariable("AGREE_TO_LEAVE", new TextObject("{=0geP718k}Well... I don't know what to say. Goodbye, then.", null), false);
			return true;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00109644 File Offset: 0x00107844
		private bool companion_has_role_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			List<PartyRole> heroPartyRoles = MobileParty.MainParty.GetHeroPartyRoles(oneToOneConversationHero);
			if (heroPartyRoles.Count == 0)
			{
				MBTextManager.SetTextVariable("COMPANION_ROLE", new TextObject("{=k7ebznzr}Yes?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("COMPANION_ROLE", new TextObject("{=n3bvfe8t}I am currently working as {COMPANION_JOB}.", null), false);
				if (heroPartyRoles.Count == 1)
				{
					MBTextManager.SetTextVariable("COMPANION_JOB", GameTexts.FindText("role", heroPartyRoles.First<PartyRole>().ToString()), false);
				}
				else
				{
					List<TextObject> list = new List<TextObject>();
					foreach (PartyRole partyRole in heroPartyRoles)
					{
						list.Add(GameTexts.FindText("role", partyRole.ToString()));
					}
					TextObject textObject = GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, true);
					MBTextManager.SetTextVariable("COMPANION_JOB", textObject, false);
				}
			}
			return true;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00109748 File Offset: 0x00107948
		private bool companion_role_discuss_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.Clan == Clan.PlayerClan)
			{
				MobileParty partyBelongedTo = Hero.OneToOneConversationHero.PartyBelongedTo;
				return partyBelongedTo != null && !partyBelongedTo.IsInRaftState;
			}
			return false;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x0010977C File Offset: 0x0010797C
		private bool companion_assign_role_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.Clan == Clan.PlayerClan && Hero.OneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty && MobileParty.MainParty.GetHeroPartyRoles(Hero.OneToOneConversationHero).Count < Campaign.Current.Models.ClanMemberPartyRoleModel.MaximumPartyRoleAssignmentCount;
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x001097E0 File Offset: 0x001079E0
		private bool companion_assign_but_too_many_role_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.Clan == Clan.PlayerClan && Hero.OneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty && MobileParty.MainParty.GetHeroPartyRoles(Hero.OneToOneConversationHero).Count >= Campaign.Current.Models.ClanMemberPartyRoleModel.MaximumPartyRoleAssignmentCount;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00109845 File Offset: 0x00107A45
		private bool party_role_assignment_clickable_condition(PartyRole role, out TextObject explanation)
		{
			bool flag = Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(role, Hero.OneToOneConversationHero, Hero.OneToOneConversationHero.PartyBelongedTo);
			if (!flag)
			{
				explanation = new TextObject("{=zcTOL3gI}Not eligible for the role.", null);
				return flag;
			}
			explanation = TextObject.GetEmpty();
			return flag;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00109883 File Offset: 0x00107A83
		private bool companion_becomes_engineer_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Engineer, out explanation);
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00109890 File Offset: 0x00107A90
		private bool companion_becomes_engineer_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Engineer);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_ENGINEER", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_ENGINEER", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			return roleHolder != oneToOneConversationHero;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x001098F7 File Offset: 0x00107AF7
		private void companion_becomes_engineer_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartyEngineer(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x0010990D File Offset: 0x00107B0D
		private bool companion_becomes_surgeon_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Surgeon, out explanation);
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00109918 File Offset: 0x00107B18
		private bool companion_becomes_surgeon_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Surgeon);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SURGEON", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SURGEON", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			return roleHolder != oneToOneConversationHero && Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(PartyRole.Surgeon, oneToOneConversationHero, oneToOneConversationHero.PartyBelongedTo);
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x0010999A File Offset: 0x00107B9A
		private void companion_becomes_surgeon_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartySurgeon(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x001099B0 File Offset: 0x00107BB0
		private bool companion_becomes_quartermaster_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Quartermaster, out explanation);
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x001099BC File Offset: 0x00107BBC
		private bool companion_becomes_quartermaster_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Quartermaster);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_QUARTERMASTER", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_QUARTERMASTER", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			Hero oneToOneConversationHero2 = Hero.OneToOneConversationHero;
			return roleHolder != oneToOneConversationHero && Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(PartyRole.Quartermaster, oneToOneConversationHero, oneToOneConversationHero.PartyBelongedTo);
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00109A46 File Offset: 0x00107C46
		private void companion_becomes_quartermaster_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartyQuartermaster(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00109A5C File Offset: 0x00107C5C
		private bool companion_becomes_scout_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Scout, out explanation);
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00109A68 File Offset: 0x00107C68
		private bool companion_becomes_scout_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Scout);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SCOUT", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SCOUT", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			return roleHolder != oneToOneConversationHero && Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(PartyRole.Scout, oneToOneConversationHero, oneToOneConversationHero.PartyBelongedTo);
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00109AEC File Offset: 0x00107CEC
		private void companion_becomes_scout_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartyScout(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00109B02 File Offset: 0x00107D02
		private bool CanFireHeroFromRole(PartyRole role, Hero hero)
		{
			return hero.PartyBelongedTo.GetRoleHolder(role) == hero && hero != hero.PartyBelongedTo.LeaderHero;
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00109B26 File Offset: 0x00107D26
		private bool companion_fire_engineer_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Engineer, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00109B34 File Offset: 0x00107D34
		private bool companion_fire_surgeon_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Surgeon, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00109B42 File Offset: 0x00107D42
		private bool companion_fire_quartermaster_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Quartermaster, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00109B51 File Offset: 0x00107D51
		private bool companion_fire_scout_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Scout, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00109B60 File Offset: 0x00107D60
		private void companion_fire_engineer_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Engineer);
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00109B77 File Offset: 0x00107D77
		private void companion_fire_surgeon_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Surgeon);
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00109B8E File Offset: 0x00107D8E
		private void companion_fire_quartermaster_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Quartermaster);
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00109BA6 File Offset: 0x00107DA6
		private void companion_fire_scout_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Scout);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00109BC0 File Offset: 0x00107DC0
		private bool companion_rescue_start_condition()
		{
			if (Campaign.Current.CurrentConversationContext == ConversationContext.FreeOrCapturePrisonerHero)
			{
				Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
				if (((oneToOneConversationHero != null) ? oneToOneConversationHero.CompanionOf : null) == Clan.PlayerClan && CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Wanderer)
				{
					MBTextManager.SetTextVariable("SALUTATION", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_salutation", CharacterObject.OneToOneConversationCharacter), false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00109C27 File Offset: 0x00107E27
		private void companion_rescue_answer_options_join_party_consequence()
		{
			EndCaptivityAction.ApplyByReleasedAfterBattle(Hero.OneToOneConversationHero);
			Hero.OneToOneConversationHero.ChangeState(Hero.CharacterStates.Active);
			MobileParty.MainParty.AddElementToMemberRoster(CharacterObject.OneToOneConversationCharacter, 1, false);
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00109C50 File Offset: 0x00107E50
		private bool lead_a_party_clickable_condition(out TextObject reason)
		{
			bool flag = Clan.PlayerClan.WarPartyLimit > Clan.PlayerClan.WarPartyComponents.Count;
			int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
			bool flag2 = Hero.MainHero.Gold > partyGoldLowerThreshold - Hero.OneToOneConversationHero.Gold;
			TextObject textObject = new TextObject("{=QH3pgsia}Creating the party will cost you {PARTY_COST}{GOLD_ICON}.", null).SetTextVariable("PARTY_COST", partyGoldLowerThreshold - Hero.OneToOneConversationHero.Gold).SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			reason = textObject;
			if (!flag)
			{
				reason = GameTexts.FindText("str_clan_doesnt_have_empty_party_slots", null);
			}
			else if (!flag2)
			{
				reason = new TextObject("{=xpCdwmlX}You don't have enough gold to make {HERO.NAME} a party leader.", null);
				reason.SetCharacterProperties("HERO", Hero.OneToOneConversationHero.CharacterObject, false);
			}
			return flag && flag2;
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00109D15 File Offset: 0x00107F15
		private void companion_rescue_answer_options_lead_party_consequence()
		{
			this.OpenPartyScreenForRescue();
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x00109D1D File Offset: 0x00107F1D
		private void OpenPartyScreenForRescue()
		{
			PartyScreenHelper.OpenScreenAsCreateClanPartyForHero(Hero.OneToOneConversationHero, new PartyScreenClosedDelegate(this.PartyScreenClosed), new IsTroopTransferableDelegate(this.TroopTransferableDelegate));
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x00109D44 File Offset: 0x00107F44
		private void PartyScreenClosed(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			if (!fromCancel)
			{
				CharacterObject character = leftMemberRoster.GetTroopRoster().FirstOrDefault<TroopRosterElement>(delegate(TroopRosterElement x)
				{
					Hero heroObject = x.Character.HeroObject;
					return heroObject != null && heroObject.IsPlayerCompanion;
				}).Character;
				EndCaptivityAction.ApplyByReleasedAfterBattle(character.HeroObject);
				character.HeroObject.ChangeState(Hero.CharacterStates.Active);
				MobileParty.MainParty.AddElementToMemberRoster(character, 1, false);
				this._partyCreatedAfterRescueForCompanion = true;
				int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
				if (character.HeroObject.Gold < partyGoldLowerThreshold)
				{
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, character.HeroObject, partyGoldLowerThreshold - character.HeroObject.Gold, false);
				}
				MobileParty mobileParty = MobilePartyHelper.CreateNewClanMobileParty(character.HeroObject, Clan.PlayerClan);
				foreach (TroopRosterElement troopRosterElement in leftMemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character != character)
					{
						mobileParty.MemberRoster.Add(troopRosterElement);
						rightOwnerParty.MemberRoster.AddToCounts(troopRosterElement.Character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, -troopRosterElement.Xp, true, -1);
					}
				}
				foreach (TroopRosterElement troopRosterElement2 in leftPrisonRoster.GetTroopRoster())
				{
					mobileParty.MemberRoster.Add(troopRosterElement2);
					rightOwnerParty.PrisonRoster.AddToCounts(troopRosterElement2.Character, -troopRosterElement2.Number, false, -troopRosterElement2.WoundedNumber, -troopRosterElement2.Xp, true, -1);
				}
			}
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x00109F04 File Offset: 0x00108104
		private bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00109F0F File Offset: 0x0010810F
		private bool party_screen_continue_conversation_condition()
		{
			if (Campaign.Current.CurrentConversationContext == ConversationContext.FreeOrCapturePrisonerHero)
			{
				Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
				if (((oneToOneConversationHero != null) ? oneToOneConversationHero.CompanionOf : null) == Clan.PlayerClan)
				{
					return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Wanderer;
				}
			}
			return false;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00109F46 File Offset: 0x00108146
		private bool party_screen_opened_but_party_is_not_created_after_rescue_condition()
		{
			return !this._partyCreatedAfterRescueForCompanion;
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x00109F51 File Offset: 0x00108151
		private bool party_screen_opened_and_party_is_created_after_rescue_condition()
		{
			return this._partyCreatedAfterRescueForCompanion;
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00109F59 File Offset: 0x00108159
		private void end_rescue_companion()
		{
			this._partyCreatedAfterRescueForCompanion = false;
			if (Hero.OneToOneConversationHero.IsPrisoner)
			{
				EndCaptivityAction.ApplyByReleasedAfterBattle(Hero.OneToOneConversationHero);
			}
		}

		// Token: 0x0400128E RID: 4750
		private const int CompanionRelationLimit = -10;

		// Token: 0x0400128F RID: 4751
		private const int NeededGoldToGrantFief = 20000;

		// Token: 0x04001290 RID: 4752
		private const int NeededInfluenceToGrantFief = 500;

		// Token: 0x04001291 RID: 4753
		private const int RelationGainWhenCompanionToLordAction = 50;

		// Token: 0x04001292 RID: 4754
		private const int NewCreatedHeroForCompanionClanMaxAge = 50;

		// Token: 0x04001293 RID: 4755
		private const int NewHeroSkillUpperLimit = 175;

		// Token: 0x04001294 RID: 4756
		private const int NewHeroSkillLowerLimit = 125;

		// Token: 0x04001295 RID: 4757
		private Settlement _selectedFief;

		// Token: 0x04001296 RID: 4758
		private bool _playerConfirmedTheAction;

		// Token: 0x04001297 RID: 4759
		private List<int> _alreadyUsedIconIdsForNewClans = new List<int>();

		// Token: 0x04001298 RID: 4760
		private bool _partyCreatedAfterRescueForCompanion;
	}
}
