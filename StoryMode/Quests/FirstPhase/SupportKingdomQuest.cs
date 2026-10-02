using System;
using System.Collections.Generic;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.FirstPhase
{
	// Token: 0x02000038 RID: 56
	public class SupportKingdomQuest : StoryModeQuestBase
	{
		// Token: 0x06000388 RID: 904 RVA: 0x00012E7C File Offset: 0x0001107C
		public SupportKingdomQuest(Hero questGiver)
			: base("main_storyline_support_kingdom_quest_" + ((StoryModeHeroes.ImperialMentor == questGiver) ? "1" : "0"), questGiver, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._isImperial = StoryModeHeroes.ImperialMentor == questGiver;
			this.SetDialogs();
			if (this._isImperial)
			{
				base.AddLog(this._onQuestStartedImperialLogText, false);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetImperialKingDialogueFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetImperialMentorDialogueFlow(), this);
			}
			else
			{
				base.AddLog(this._onQuestStartedAntiImperialLogText, false);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetAntiImperialKingDialogueFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetAntiImperialMentorDialogueFlow(), this);
			}
			base.InitializeQuestOnCreation();
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00012F5A File Offset: 0x0001115A
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=XtC0hXhr}Support {?IS_IMPERIAL}an Imperial Faction{?}a Non-Imperial Kingdom{\\?}", null);
				textObject.SetTextVariable("IS_IMPERIAL", this._isImperial ? 1 : 0);
				return textObject;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00012F80 File Offset: 0x00011180
		private TextObject _onQuestStartedImperialLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=TZZX9kWf}{MENTOR.LINK} suggested that you should support an imperial faction by offering them the Dragon Banner.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00012FB4 File Offset: 0x000111B4
		private TextObject _onQuestStartedAntiImperialLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=4d5SP6B6}{MENTOR.LINK} suggested that you should support an anti-imperial kingdom by offering them the Dragon Banner.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00012FE8 File Offset: 0x000111E8
		private TextObject _onImperialKingdomSupportedLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=atUTLABh}You have chosen to support the {KINGDOM} by presenting them the Dragon Banner, taking the advice of {MENTOR.LINK}.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("KINGDOM", Clan.PlayerClan.Kingdom.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00013034 File Offset: 0x00011234
		private TextObject _onAntiImperialKingdomSupportedLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=atUTLABh}You have chosen to support the {KINGDOM} by presenting them the Dragon Banner, taking the advice of {MENTOR.LINK}.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("KINGDOM", Clan.PlayerClan.Kingdom.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00013080 File Offset: 0x00011280
		private TextObject _onPlayerRuledKingdomSupportedLogText
		{
			get
			{
				return new TextObject("{=kqj1Wp0f}You have decided to keep the Dragon Banner within the kingdom you are ruling.", null);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0001308D File Offset: 0x0001128D
		private TextObject _questFailedLogText
		{
			get
			{
				return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0001309A File Offset: 0x0001129A
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001309D File Offset: 0x0001129D
		protected override void SetDialogs()
		{
			this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=9tpTkKdY}Tell me which path you choose when you've made progress.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
				.CloseDialog();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000130DC File Offset: 0x000112DC
		private DialogFlow GetImperialKingDialogueFlow()
		{
			return DialogFlow.CreateDialogFlow("hero_main_options", 300).BeginPlayerOptions(null, false).PlayerSpecialOption(new TextObject("{=Ke7f4XSC}I present you with the Dragon Banner of Calradios.", null), null, null, null)
				.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CheckConditionToSupportKingdom))
				.Condition(() => Hero.OneToOneConversationHero.Clan != null && Hero.OneToOneConversationHero.Clan.Kingdom != null && Hero.OneToOneConversationHero.Clan.Kingdom.Leader == Hero.OneToOneConversationHero && StoryModeData.IsKingdomImperial(Hero.OneToOneConversationHero.Clan.Kingdom))
				.NpcLine("{=PQgzfHLk}Well now. I had heard rumors that you had obtained this great artifact.[if:convo_nonchalant]", null, null, null, null)
				.NpcLine("{=ULn7iWlz}It will be a powerful tool in our hands. People will believe that the Heavens intend us to restore the Empire of Calradia.[if:convo_pondering]", null, null, null, null)
				.NpcLine("{=S1yCTPrL}This is one of the most valuable services anyone has ever done for me. I am very grateful.[if:convo_grateful]", null, null, null, null)
				.Consequence(delegate
				{
					this.OnKingdomSupported(Hero.OneToOneConversationHero.Clan.Kingdom, true);
					if (PlayerEncounter.Current != null)
					{
						PlayerEncounter.LeaveEncounter = true;
					}
					TextObject textObject = new TextObject("{=IL4FcHXv}You've pledged your allegiance to the {KINGDOM_NAME}!", null);
					textObject.SetTextVariable("KINGDOM_NAME", Hero.OneToOneConversationHero.Clan.Kingdom.Name);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				})
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00013194 File Offset: 0x00011394
		private DialogFlow GetAntiImperialKingDialogueFlow()
		{
			return DialogFlow.CreateDialogFlow("hero_main_options", 300).BeginPlayerOptions(null, false).PlayerSpecialOption(new TextObject("{=Ke7f4XSC}I present you with the Dragon Banner of Calradios.", null), null, null, null)
				.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CheckConditionToSupportKingdom))
				.Condition(() => Hero.OneToOneConversationHero.Clan != null && Hero.OneToOneConversationHero.Clan.Kingdom != null && Hero.OneToOneConversationHero.Clan.Kingdom.Leader == Hero.OneToOneConversationHero && !StoryModeData.IsKingdomImperial(Hero.OneToOneConversationHero.Clan.Kingdom))
				.NpcLine("{=PQgzfHLk}Well now. I had heard rumors that you had obtained this great artifact.[if:convo_nonchalant]", null, null, null, null)
				.NpcLine("{=4olAbDTq}It will be a powerful tool in our hands. People will believe that the Heavens have transferred dominion over Calradia from the Empire to us.[if:convo_pondering]", null, null, null, null)
				.NpcLine("{=S1yCTPrL}This is one of the most valuable services anyone has ever done for me. I am very grateful.[if:convo_grateful]", null, null, null, null)
				.Consequence(delegate
				{
					this.OnKingdomSupported(Hero.OneToOneConversationHero.Clan.Kingdom, false);
					if (PlayerEncounter.Current != null)
					{
						PlayerEncounter.LeaveEncounter = true;
					}
					TextObject textObject = new TextObject("{=IL4FcHXv}You've pledged your allegiance to the {KINGDOM_NAME}!", null);
					textObject.SetTextVariable("KINGDOM_NAME", Hero.OneToOneConversationHero.Clan.Kingdom.Name);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				})
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001324C File Offset: 0x0001144C
		private DialogFlow GetImperialMentorDialogueFlow()
		{
			return DialogFlow.CreateDialogFlow("hero_main_options", 300).BeginPlayerOptions(null, false).PlayerSpecialOption(new TextObject("{=O2BAcMNO}As the legitimate {?PLAYER.GENDER}Empress{?}Emperor{\\?} of Calradia, I am ready to declare my ownership of the Dragon Banner.", null), null, null, null)
				.Condition(() => base.IsOngoing && Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor)
				.NpcLine("{=ATduKfHu}This will make a great impression. It will attract allies, but also probably make you new enemies. Are you sure you're ready?[if:convo_undecided_closed]", null, null, null, null)
				.BeginPlayerOptions(null, false)
				.PlayerOption("{=n8pmVHNn}Yes, I am ready.", null, null, null)
				.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CheckPlayerCanDeclareBannerOwnershipClickableCondition))
				.NpcLine("{=gL241Hoz}Very nice. Superstitious twaddle, of course, but people will believe you. Very well, oh heir to Calradios, go forth![if:convo_nonchalant]", null, null, null, null)
				.Consequence(delegate
				{
					this.OnKingdomSupported(Clan.PlayerClan.Kingdom, true);
				})
				.CloseDialog()
				.PlayerOption("{=fRMIoPUK}Give me more time.", null, null, null)
				.NpcLine("{=KH07mJ5k}Very well, come back when you are ready.", null, null, null, null)
				.EndPlayerOptions()
				.CloseDialog()
				.PlayerOption("{=eYXLYgsC}I still am not sure what I will do with it.", null, null, null)
				.Condition(() => base.IsOngoing && Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor)
				.NpcLine("{=UCoOMWaj}As I said before, there's a case for all of the claimants. When this war began, I thought Rhagaea understood best how to rule, Garios was the strongest warrior, and Lucon had the firmest grasp of our traditions.[if:convo_empathic_voice]", null, null, null, null)
				.NpcLine("{=uFsMzAuR}Speak to whichever one you choose, or come back to me if you wish to claim the banner for yourself.[if:convo_normal]", null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.NpcLine("{=Z54ZrDG9}Until next time, then.", null, null, null, null)
				.CloseDialog();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00013368 File Offset: 0x00011568
		private DialogFlow GetAntiImperialMentorDialogueFlow()
		{
			return DialogFlow.CreateDialogFlow("hero_main_options", 300).BeginPlayerOptions(null, false).PlayerSpecialOption(new TextObject("{=N5jJtZyr}As the Empire's nemesis, I am ready to declare my ownership of the Dragon Banner.", null), null, null, null)
				.Condition(() => base.IsOngoing && Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor)
				.NpcLine("{=BXMKgTXl}This will make a great impression. It will attract allies, but also probably make you new enemies. Are you sure you're ready?[if:convo_astonished]", null, null, null, null)
				.BeginPlayerOptions(null, false)
				.PlayerOption("{=ALWqXMiP}Yes, I am sure.", null, null, null)
				.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CheckPlayerCanDeclareBannerOwnershipClickableCondition))
				.NpcLine("{=exoZygYL}Very well. The Dragon Banner in your hands proclaims you the avenger of the Empire's crimes and its successor. Now go forth and claim your destiny![if:convo_calm_friendly]", null, null, null, null)
				.Consequence(delegate
				{
					this.OnKingdomSupported(Clan.PlayerClan.Kingdom, false);
				})
				.CloseDialog()
				.PlayerOption("{=fRMIoPUK}Give me more time.", null, null, null)
				.NpcLine("{=YgoxFJSz}Very well, come back when you are ready.[if:convo_nonchalant]", null, null, null, null)
				.EndPlayerOptions()
				.CloseDialog()
				.PlayerOption("{=tzsZTcWd}I wonder which kingdom should I support..", null, null, null)
				.Condition(() => base.IsOngoing && Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor)
				.NpcLine("{=1v6aYpDx}You must choose, but choose wisely. Or you can claim it yourself. I have no preference.", null, null, null, null)
				.GotoDialogState("hero_main_options")
				.EndPlayerOptions()
				.NpcLine("{=Z54ZrDG9}Until next time, then.", null, null, null, null)
				.CloseDialog();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001347C File Offset: 0x0001167C
		private bool IsPlayerTheRulerOfAKingdom()
		{
			bool flag = Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Leader == Hero.MainHero && StoryModeData.IsKingdomImperial(Clan.PlayerClan.Kingdom) == this._isImperial;
			if (flag)
			{
				MBTextManager.SetTextVariable("FACTION", Clan.PlayerClan.Kingdom.Name, false);
			}
			return flag;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000134E2 File Offset: 0x000116E2
		private bool CheckPlayerCanDeclareBannerOwnershipClickableCondition(out TextObject explanation)
		{
			if (this.IsPlayerTheRulerOfAKingdom())
			{
				explanation = null;
				return true;
			}
			explanation = (this._isImperial ? new TextObject("{=mziMNKm2}You should be ruling a kingdom of the imperial culture.", null) : new TextObject("{=HCA9xOOo}You should be ruling a kingdom of non-imperial culture.", null));
			return false;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00013514 File Offset: 0x00011714
		private bool CheckConditionToSupportKingdom(out TextObject explanation)
		{
			explanation = new TextObject("{=qNR8WKcX}You should join a kingdom before supporting it with the Dragon Banner.", null);
			return Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom == Hero.OneToOneConversationHero.Clan.Kingdom;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00013550 File Offset: 0x00011750
		private void OnKingdomSupported(Kingdom kingdom, bool isImperial)
		{
			if (isImperial)
			{
				if (kingdom.RulingClan == Clan.PlayerClan)
				{
					base.AddLog(this._onPlayerRuledKingdomSupportedLogText, false);
					StoryModeManager.Current.MainStoryLine.SetStoryLineSide(MainStoryLineSide.CreateImperialKingdom);
					MBInformationManager.ShowSceneNotification(new DeclareDragonBannerSceneNotificationItem(true));
				}
				else
				{
					base.AddLog(this._onImperialKingdomSupportedLogText, false);
					StoryModeManager.Current.MainStoryLine.SetStoryLineSide(MainStoryLineSide.SupportImperialKingdom);
					MBInformationManager.ShowSceneNotification(new PledgeAllegianceSceneNotificationItem(Hero.MainHero, true));
				}
			}
			else if (kingdom.RulingClan == Clan.PlayerClan)
			{
				base.AddLog(this._onPlayerRuledKingdomSupportedLogText, false);
				StoryModeManager.Current.MainStoryLine.SetStoryLineSide(MainStoryLineSide.CreateAntiImperialKingdom);
				MBInformationManager.ShowSceneNotification(new DeclareDragonBannerSceneNotificationItem(false));
			}
			else
			{
				base.AddLog(this._onAntiImperialKingdomSupportedLogText, false);
				StoryModeManager.Current.MainStoryLine.SetStoryLineSide(MainStoryLineSide.SupportAntiImperialKingdom);
				MBInformationManager.ShowSceneNotification(new PledgeAllegianceSceneNotificationItem(Hero.MainHero, false));
			}
			base.CompleteQuestWithSuccess();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00013637 File Offset: 0x00011837
		private void MainStoryLineChosen(MainStoryLineSide chosenSide)
		{
			if ((this._isImperial && chosenSide != MainStoryLineSide.SupportImperialKingdom && chosenSide != MainStoryLineSide.CreateImperialKingdom) || (!this._isImperial && chosenSide != MainStoryLineSide.SupportAntiImperialKingdom && chosenSide != MainStoryLineSide.CreateAntiImperialKingdom))
			{
				base.CompleteQuestWithCancel(this._questFailedLogText);
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00013665 File Offset: 0x00011865
		protected override void HourlyTick()
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00013667 File Offset: 0x00011867
		protected override void RegisterEvents()
		{
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.MainStoryLineChosen));
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00013680 File Offset: 0x00011880
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
			if (this._isImperial)
			{
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetImperialKingDialogueFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetImperialMentorDialogueFlow(), this);
				return;
			}
			Campaign.Current.ConversationManager.AddDialogFlow(this.GetAntiImperialKingDialogueFlow(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(this.GetAntiImperialMentorDialogueFlow(), this);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000136F4 File Offset: 0x000118F4
		internal static void AutoGeneratedStaticCollectObjectsSupportKingdomQuest(object o, List<object> collectedObjects)
		{
			((SupportKingdomQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00013702 File Offset: 0x00011902
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001370B File Offset: 0x0001190B
		internal static object AutoGeneratedGetMemberValue_isImperial(object o)
		{
			return ((SupportKingdomQuest)o)._isImperial;
		}

		// Token: 0x04000117 RID: 279
		[SaveableField(1)]
		private bool _isImperial;
	}
}
