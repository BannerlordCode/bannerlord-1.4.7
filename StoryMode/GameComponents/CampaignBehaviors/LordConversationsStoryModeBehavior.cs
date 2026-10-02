using System;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x0200004F RID: 79
	public class LordConversationsStoryModeBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x0001B54C File Offset: 0x0001974C
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001B565 File Offset: 0x00019765
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001B567 File Offset: 0x00019767
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001B570 File Offset: 0x00019770
		private void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddDialogLine("anti_imperial_mentor_introduction", "lord_introduction", "lord_start", "{=TB20aFsf}You probably are aware that I am {CONVERSATION_HERO.FIRSTNAME}. I am not sure why you have sought me out, but know that my old life, as imperial lap-dog, is over.", new ConversationSentence.OnConditionDelegate(this.conversation_anti_imperial_mentor_introduction_on_condition), null, 150, null);
			starter.AddDialogLine("imperial_mentor_introduction", "lord_introduction", "lord_start", "{=6aDiS9eP}I am {CONVERSATION_HERO.FIRSTNAME}. You probably already know that, though. Once I wielded great power, but now... Anyway, I am most curious what you might want with me.", new ConversationSentence.OnConditionDelegate(this.conversation_imperial_mentor_introduction_on_condition), null, 150, null);
			starter.AddDialogLine("start_default_for_mentors", "start", "lord_start", "{=!}{PLAYER.NAME}...", new ConversationSentence.OnConditionDelegate(this.start_default_for_mentors_on_condition), null, 150, null);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001B607 File Offset: 0x00019807
		private bool conversation_imperial_mentor_introduction_on_condition()
		{
			if (Campaign.Current.ConversationManager.CurrentConversationIsFirst && Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor)
			{
				StringHelpers.SetCharacterProperties("CONVERSATION_HERO", CharacterObject.OneToOneConversationCharacter, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001B63B File Offset: 0x0001983B
		private bool conversation_anti_imperial_mentor_introduction_on_condition()
		{
			if (Campaign.Current.ConversationManager.CurrentConversationIsFirst && Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor)
			{
				StringHelpers.SetCharacterProperties("CONVERSATION_HERO", CharacterObject.OneToOneConversationCharacter, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001B66F File Offset: 0x0001986F
		private bool start_default_for_mentors_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.HasMet && (Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor || Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor);
		}
	}
}
