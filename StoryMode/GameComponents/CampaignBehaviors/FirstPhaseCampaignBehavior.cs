using System;
using System.Linq;
using Helpers;
using StoryMode.Quests.FirstPhase;
using StoryMode.Quests.PlayerClanQuests;
using StoryMode.Quests.TutorialPhase;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x0200004E RID: 78
	public class FirstPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x0001AD7C File Offset: 0x00018F7C
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			StoryModeEvents.OnBannerPieceCollectedEvent.AddNonSerializedListener(this, new Action(this.OnBannerPieceCollected));
			StoryModeEvents.OnStoryModeTutorialEndedEvent.AddNonSerializedListener(this, new Action(this.OnStoryModeTutorialEnded));
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.OnMainStoryLineSideChosen));
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001AE58 File Offset: 0x00019058
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Location>("_imperialMentorHouse", ref this._imperialMentorHouse);
			dataStore.SyncData<Location>("_antiImperialMentorHouse", ref this._antiImperialMentorHouse);
			dataStore.SyncData<bool>("_popUpShowed", ref this._popUpShowed);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001AE90 File Offset: 0x00019090
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.SpawnMentorsIfNeeded();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001AE98 File Offset: 0x00019098
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter campaignGameStarter)
		{
			Settlement settlement = SettlementHelper.FindRandomSettlement((Settlement s) => s.IsTown && !s.IsUnderSiege && s.Culture.StringId == "empire");
			this._imperialMentorHouse = this.ReserveHouseForMentor(StoryModeHeroes.ImperialMentor, settlement);
			Settlement settlement2 = SettlementHelper.FindRandomSettlement((Settlement s) => s.IsTown && !s.IsUnderSiege && s.Culture.StringId == "battania");
			this._antiImperialMentorHouse = this.ReserveHouseForMentor(StoryModeHeroes.AntiImperialMentor, settlement2);
			StoryModeManager.Current.MainStoryLine.SetMentorSettlements(settlement, settlement2);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001AF24 File Offset: 0x00019124
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (detail == QuestBase.QuestCompleteDetails.Success)
			{
				if (quest is BannerInvestigationQuest)
				{
					new MeetWithIstianaQuest(StoryModeManager.Current.MainStoryLine.ImperialMentorSettlement).StartQuest();
					new MeetWithArzagosQuest(StoryModeManager.Current.MainStoryLine.AntiImperialMentorSettlement).StartQuest();
					return;
				}
				if (quest is MeetWithIstianaQuest)
				{
					Hero imperialMentor = StoryModeHeroes.ImperialMentor;
					new IstianasBannerPieceQuest(imperialMentor, this.FindSuitableHideout(imperialMentor)).StartQuest();
					return;
				}
				if (quest is MeetWithArzagosQuest)
				{
					Hero antiImperialMentor = StoryModeHeroes.AntiImperialMentor;
					new ArzagosBannerPieceQuest(antiImperialMentor, this.FindSuitableHideout(antiImperialMentor)).StartQuest();
				}
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001AFB1 File Offset: 0x000191B1
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			this.SpawnMentorsIfNeeded();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001AFB9 File Offset: 0x000191B9
		private void OnBeforeMissionOpened()
		{
			this.SpawnMentorsIfNeeded();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001AFC4 File Offset: 0x000191C4
		private void SpawnMentorsIfNeeded()
		{
			if (this._imperialMentorHouse != null && this._antiImperialMentorHouse != null && Settlement.CurrentSettlement != null && (StoryModeHeroes.ImperialMentor.CurrentSettlement == Settlement.CurrentSettlement || StoryModeHeroes.AntiImperialMentor.CurrentSettlement == Settlement.CurrentSettlement))
			{
				this.SpawnMentorInHouse(Settlement.CurrentSettlement);
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001B018 File Offset: 0x00019218
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement.StringId == "tutorial_training_field" && party == MobileParty.MainParty && TutorialPhase.Instance.TutorialQuestPhase == TutorialQuestPhase.Finalized && !this._popUpShowed && TutorialPhase.Instance.IsSkipped)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=EWD4Op6d}Notification", null).ToString(), GameTexts.FindText("main_storyline_skip_tutorial_notification_text", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, delegate
				{
					this._popUpShowed = true;
					CampaignEventDispatcher.Instance.RemoveListeners(Campaign.Current.GetCampaignBehavior<TutorialPhaseCampaignBehavior>());
					MBInformationManager.ShowSceneNotification(new FindingFirstBannerPieceSceneNotificationItem(Hero.MainHero, new Action(this.OnPieceFoundAction)));
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001B0BC File Offset: 0x000192BC
		private void ShowStealthTutorialInquiry()
		{
			object obj = new TextObject("{=DhMge68x}Stealth Tutorial", null);
			TextObject textObject = new TextObject("{=bU88a6lW}You and your brother part ways. As he rides over the crest of a hill, he lifts his arm in salute, then disappears from view. A few days ago you were a family of six. Now, you are alone, and you realize that despite your courage and determination you and your brother may never see each other again.{newline}However, you are not left long in your solitude. As you make the final preparations to set out, a young boy staggers into your camp. Once he regains his breath, he tells you that a small group of bandits raided his village and seized the headman as a hostage. The villagers saw you riding through the countryside, and thought you might be able to help them.", null);
			InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, GameTexts.FindText("str_continue", null).ToString(), string.Empty, new Action(this.StartStealthTutorial), null, "", 0f, null, null, null), true, false);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001B128 File Offset: 0x00019328
		private void StartStealthTutorial()
		{
			new VillagersInNeed().StartQuest();
			StoryModeEvents.Instance.OnStealthTutorialActivated();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001B13E File Offset: 0x0001933E
		private void OnPieceFoundAction()
		{
			this.SelectClanName();
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001B146 File Offset: 0x00019346
		private void OnStoryModeTutorialEnded()
		{
			new RebuildPlayerClanQuest().StartQuest();
			new BannerInvestigationQuest().StartQuest();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001B15C File Offset: 0x0001935C
		private void OnBannerPieceCollected()
		{
			TextObject textObject = new TextObject("{=Pus87ZW2}You've found the {BANNER_PIECE_COUNT} banner piece!", null);
			if (FirstPhase.Instance == null || FirstPhase.Instance.CollectedBannerPieceCount == 1)
			{
				textObject.SetTextVariable("BANNER_PIECE_COUNT", new TextObject("{=oAoTaAWg}first", null));
			}
			else if (FirstPhase.Instance.CollectedBannerPieceCount == 2)
			{
				textObject.SetTextVariable("BANNER_PIECE_COUNT", new TextObject("{=9ZyXl25X}second", null));
			}
			else if (FirstPhase.Instance.CollectedBannerPieceCount == 3)
			{
				textObject.SetTextVariable("BANNER_PIECE_COUNT", new TextObject("{=4cw169Kb}third and the final", null));
			}
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001B1FA File Offset: 0x000193FA
		private void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			this._imperialMentorHouse.RemoveReservation();
			this._imperialMentorHouse = null;
			this._antiImperialMentorHouse.RemoveReservation();
			this._antiImperialMentorHouse = null;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001B220 File Offset: 0x00019420
		private void SelectClanName()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=JJiKk4ow}Select your family name: ", null).ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(this.OnChangeClanNameDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", Clan.PlayerClan.Name.ToString()), false, false);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001B290 File Offset: 0x00019490
		private void OnChangeClanNameDone(string newClanName)
		{
			TextObject textObject = GameTexts.FindText("str_generic_clan_name", null);
			textObject.SetTextVariable("CLAN_NAME", new TextObject(newClanName, null));
			Clan.PlayerClan.ChangeClanName(textObject, textObject);
			this.OpenBannerSelectionScreen(new Action(this.ShowStealthTutorialInquiry));
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001B2DA File Offset: 0x000194DA
		private void OpenBannerSelectionScreen(Action endAction)
		{
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>(new object[] { endAction }), 0);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001B308 File Offset: 0x00019508
		private Settlement FindSuitableHideout(Hero questGiver)
		{
			Settlement settlement = null;
			float num = float.MaxValue;
			foreach (Hideout hideout in Hideout.All)
			{
				if (!hideout.Settlement.IsSettlementBusy(this))
				{
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(hideout.Settlement, questGiver.CurrentSettlement, false, false, MobileParty.NavigationType.Default);
					if (distance < num)
					{
						num = distance;
						settlement = hideout.Settlement;
					}
				}
			}
			return settlement;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001B3A0 File Offset: 0x000195A0
		private void SpawnMentorInHouse(Settlement settlement)
		{
			Hero hero = ((StoryModeHeroes.ImperialMentor.CurrentSettlement == settlement) ? StoryModeHeroes.ImperialMentor : StoryModeHeroes.AntiImperialMentor);
			Location location = ((StoryModeHeroes.ImperialMentor.CurrentSettlement == settlement) ? this._imperialMentorHouse : this._antiImperialMentorHouse);
			CharacterObject characterObject = hero.CharacterObject;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(characterObject.Race, "_settlement");
			LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(characterObject, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true, null, false);
			location.AddCharacter(locationCharacter);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001B448 File Offset: 0x00019648
		private Location ReserveHouseForMentor(Hero mentor, Settlement settlement)
		{
			if (settlement == null)
			{
				Debug.Print("There is null settlement in ReserveHouseForMentor", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			MBList<Location> mblist = new MBList<Location>();
			mblist.Add(settlement.LocationComplex.GetLocationWithId("house_1"));
			mblist.Add(settlement.LocationComplex.GetLocationWithId("house_2"));
			mblist.Add(settlement.LocationComplex.GetLocationWithId("house_3"));
			object obj = mblist.First<Location>((Location h) => !h.IsReserved) ?? mblist.GetRandomElement<Location>();
			TextObject textObject = new TextObject("{=EZ19JOGj}{MENTOR.NAME}'s House", null);
			StringHelpers.SetCharacterProperties("MENTOR", mentor.CharacterObject, textObject, false);
			object obj2 = obj;
			obj2.ReserveLocation(textObject, textObject);
			return obj2;
		}

		// Token: 0x040001CE RID: 462
		private Location _imperialMentorHouse;

		// Token: 0x040001CF RID: 463
		private Location _antiImperialMentorHouse;

		// Token: 0x040001D0 RID: 464
		private bool _popUpShowed;
	}
}
