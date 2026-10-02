using System;
using System.Collections.Generic;
using System.Linq;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.TutorialPhase
{
	// Token: 0x02000023 RID: 35
	public class TravelToVillageTutorialQuest : StoryModeQuestBase
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000909F File Offset: 0x0000729F
		private TextObject _startQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=bNqLQKQS}You are out of food. There is a village called {VILLAGE_NAME} north of here where you can buy provisions and find some help.", null);
				textObject.SetTextVariable("VILLAGE_NAME", this._questVillage.Name);
				return textObject;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000199 RID: 409 RVA: 0x000090C3 File Offset: 0x000072C3
		private TextObject _endQuestLog
		{
			get
			{
				return new TextObject("{=7VFLb3Qj}You have arrived at the village.", null);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000090D0 File Offset: 0x000072D0
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=oa4XFhve}Travel to Village {VILLAGE_NAME}", null);
				textObject.SetTextVariable("VILLAGE_NAME", this._questVillage.Name);
				return textObject;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000090F4 File Offset: 0x000072F4
		public TravelToVillageTutorialQuest()
			: base("travel_to_village_tutorial_quest", null, CampaignTime.Never)
		{
			this._questVillage = Settlement.Find("village_ES3_2");
			base.AddTrackedObject(this._questVillage);
			Hero hero = this._questVillage.Notables.First<Hero>((Hero x) => x.IsHeadman);
			base.AddTrackedObject(hero);
			this._refugeeParties = new MobileParty[4];
			TextObject textObject = new TextObject("{=3YHL3wpM}{BROTHER.NAME}:", null);
			textObject.SetCharacterProperties("BROTHER", StoryModeHeroes.ElderBrother.CharacterObject, false);
			InformationManager.ShowInquiry(new InquiryData(textObject.ToString(), new TextObject("{=dE2ufxte}Before we do anything else... We're low on food. There's a village north of here where we can buy provisions and find some help. You're a better rider than I am so I'll let you lead the way...", null).ToString(), true, false, new TextObject("{=JOJ09cLW}Let's go.", null).ToString(), null, delegate
			{
				StoryModeEvents.Instance.OnTravelToVillageTutorialQuestStarted();
			}, null, "", 0f, null, null, null), false, false);
			this.SetDialogs();
			base.InitializeQuestOnCreation();
			base.AddLog(this._startQuestLog, false);
			TutorialPhase.Instance.SetTutorialFocusSettlement(this._questVillage);
			this.CreateRefugeeParties();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009223 File Offset: 0x00007423
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000922B File Offset: 0x0000742B
		protected override void HourlyTick()
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009230 File Offset: 0x00007430
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=MDtTC5j5}Don't hurt us![ib:nervous][if:convo_nervous]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.news_about_raiders_condition))
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.news_about_raiders_consequence))
				.PlayerLine(new TextObject("{=pX5cx3b4}I mean you no harm. We're hunting a group of raiders who took our brother and sister.", null), null, null, null)
				.NpcLine(new TextObject("{=ajBBFq1D}Aii... Those devils. They raided our village. Took whoever they could catch. Slavers, I'll bet.[if:convo_nervous][ib:nervous2]", null), null, null, null, null)
				.NpcLine(new TextObject("{=AhthUkMu}People say they're still about. We're sleeping in the woods, not going back until they're gone. You hunt them down and kill every one, you hear! Heaven protect you! Heaven guide your swords![if:convo_nervous2][ib:nervous]", null), null, null, null, null)
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000020).NpcLine(new TextObject("{=pa9LrHln}We're here, I guess. So... We need food, and after that, maybe some men to come with us.[if:convo_thinking]", null), null, null, null, null).Condition(() => Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == this._questVillage && Hero.OneToOneConversationHero == StoryModeHeroes.ElderBrother)
				.NpcLine(new TextObject("{=p0fmZY5r}The headman here can probably help us. Let's try to find him...[if:convo_pondering]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.talk_with_brother_consequence))
				.CloseDialog(), this);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00009340 File Offset: 0x00007540
		private bool news_about_raiders_condition()
		{
			return Settlement.CurrentSettlement == null && MobileParty.ConversationParty != null && this._refugeeParties.Contains(MobileParty.ConversationParty);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00009362 File Offset: 0x00007562
		private void news_about_raiders_consequence()
		{
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000936A File Offset: 0x0000756A
		private void talk_with_brother_consequence()
		{
			Campaign.Current.ConversationManager.ConversationEndOneShot += base.CompleteQuestWithSuccess;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00009388 File Offset: 0x00007588
		protected override void RegisterEvents()
		{
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
			StoryModeEvents.OnTravelToVillageTutorialQuestStartedEvent.AddNonSerializedListener(this, new Action(this.OnTravelToVillageTutorialQuestStarted));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000093DC File Offset: 0x000075DC
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (!TutorialPhase.Instance.IsCompleted && Settlement.CurrentSettlement == null && PlayerEncounter.EncounteredParty != null && args.MenuContext.GameMenu.StringId != "encounter_meeting" && args.MenuContext.GameMenu.StringId != "encounter" && this._refugeeParties.Contains(PlayerEncounter.EncounteredMobileParty))
			{
				GameMenu.SwitchToMenu("encounter_meeting");
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009458 File Offset: 0x00007658
		private void OnBeforeMissionOpened()
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == Settlement.Find("village_ES3_2"))
			{
				int hitPoints = StoryModeHeroes.ElderBrother.HitPoints;
				int num = 50;
				if (hitPoints < num)
				{
					int num2 = num - hitPoints;
					StoryModeHeroes.ElderBrother.Heal(num2, false);
				}
				LocationCharacter locationCharacterOfHero = LocationComplex.Current.GetLocationCharacterOfHero(StoryModeHeroes.ElderBrother);
				PlayerEncounter.LocationEncounter.AddAccompanyingCharacter(locationCharacterOfHero, true);
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000094BC File Offset: 0x000076BC
		protected override void DailyTick()
		{
			for (int i = 0; i < this._refugeeParties.Length; i++)
			{
				if (this._refugeeParties[i].Party.IsStarving)
				{
					this._refugeeParties[i].Party.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009510 File Offset: 0x00007710
		private void OnTravelToVillageTutorialQuestStarted()
		{
			MapState mapState;
			if ((mapState = GameStateManager.Current.ActiveState as MapState) != null)
			{
				mapState.Handler.StartCameraAnimation(this._questVillage.GatePosition, 1f);
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000954C File Offset: 0x0000774C
		private void CreateRefugeeParties()
		{
			for (int i = 0; i < 4; i++)
			{
				MobileParty mobileParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._questVillage.GatePosition, MobileParty.MainParty.SeeingRange, this._questVillage, new TextObject("{=7FWF01bW}Refugees", null), null, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), this._questVillage.OwnerClan.Leader, "", "", 0f, true);
				mobileParty.InitializePartyTrade(200);
				SetPartyAiAction.GetActionForPatrollingAroundSettlement(mobileParty, this._questVillage, MobileParty.NavigationType.Default, false, false);
				mobileParty.Ai.SetDoNotMakeNewDecisions(true);
				mobileParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
				mobileParty.SetPartyUsedByQuest(true);
				mobileParty.Party.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("storymode_quest_refugee_female");
				CharacterObject object2 = MBObjectManager.Instance.GetObject<CharacterObject>("storymode_quest_refugee_male");
				int num = MBRandom.RandomInt(6, 12);
				for (int j = 0; j < num; j++)
				{
					mobileParty.MemberRoster.AddToCounts((MBRandom.RandomFloat < 0.5f) ? @object : object2, 1, false, 0, 0, true, -1);
				}
				this._refugeeParties[i] = mobileParty;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009678 File Offset: 0x00007878
		protected override void OnCompleteWithSuccess()
		{
			foreach (MobileParty mobileParty in this._refugeeParties.ToList<MobileParty>())
			{
				DestroyPartyAction.Apply(null, mobileParty);
			}
			base.AddLog(this._endQuestLog, false);
			TutorialPhase.Instance.RemoveTutorialFocusSettlement();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000096E8 File Offset: 0x000078E8
		internal static void AutoGeneratedStaticCollectObjectsTravelToVillageTutorialQuest(object o, List<object> collectedObjects)
		{
			((TravelToVillageTutorialQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000096F6 File Offset: 0x000078F6
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._questVillage);
			collectedObjects.Add(this._refugeeParties);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009717 File Offset: 0x00007917
		internal static object AutoGeneratedGetMemberValue_questVillage(object o)
		{
			return ((TravelToVillageTutorialQuest)o)._questVillage;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009724 File Offset: 0x00007924
		internal static object AutoGeneratedGetMemberValue_refugeeParties(object o)
		{
			return ((TravelToVillageTutorialQuest)o)._refugeeParties;
		}

		// Token: 0x0400009F RID: 159
		private const int RefugePartyCount = 4;

		// Token: 0x040000A0 RID: 160
		[SaveableField(1)]
		private Settlement _questVillage;

		// Token: 0x040000A1 RID: 161
		[SaveableField(2)]
		private readonly MobileParty[] _refugeeParties;
	}
}
