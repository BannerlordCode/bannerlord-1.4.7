using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000422 RID: 1058
	public class ParleyCampaignBehavior : CampaignBehaviorBase, IParleyCampaignBehavior
	{
		// Token: 0x06004386 RID: 17286 RVA: 0x00147CCD File Offset: 0x00145ECD
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x00147CE6 File Offset: 0x00145EE6
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<PartyBase>("_parleyedParty", ref this._parleyedParty);
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x00147CFA File Offset: 0x00145EFA
		public void StartParley(PartyBase partyBase)
		{
			if (partyBase.IsSettlement)
			{
				this._parleyedParty = partyBase;
				GameMenu.ActivateGameMenu("request_meeting_parley");
				return;
			}
			Debug.FailedAssert("MobileParty parley not implemented yet!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\ParleyCampaignBehavior.cs", "StartParley", 35);
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x00147D2C File Offset: 0x00145F2C
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddMenus(campaignGameStarter);
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x00147D38 File Offset: 0x00145F38
		private void AddMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("request_meeting_parley", "{=pBAx7jTM}With whom do you want to meet?", new OnInitDelegate(this.game_menu_town_menu_request_meeting_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("request_meeting_parley", "request_meeting_with", "{=!}{HERO_TO_MEET.LINK}", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_with_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_with_on_consequence), false, -1, true, null);
			campaignGameStarter.AddGameMenuOption("request_meeting_parley", "meeting_town_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_meeting_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_town_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenuOption("request_meeting_parley", "meeting_castle_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_meeting_castle_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_castle_leave_on_consequence), true, -1, false, null);
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x00147DF8 File Offset: 0x00145FF8
		private void game_menu_town_menu_request_meeting_on_init(MenuCallbackArgs args)
		{
			List<Hero> heroesToMeetInTown = TownHelpers.GetHeroesToMeetInTown(this._parleyedParty.Settlement);
			args.MenuContext.SetRepeatObjectList(heroesToMeetInTown);
			args.MenuContext.SetBackgroundMeshName(this._parleyedParty.Settlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x00147E44 File Offset: 0x00146044
		private bool game_menu_request_meeting_with_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			Hero hero = args.MenuContext.GetCurrentRepeatableObject() as Hero;
			if (this._parleyedParty != null && hero != null)
			{
				StringHelpers.SetCharacterProperties("HERO_TO_MEET", hero.CharacterObject, null, false);
				MenuHelper.SetIssueAndQuestDataForHero(args, hero);
				return true;
			}
			return false;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x00147E92 File Offset: 0x00146092
		private void game_menu_request_meeting_town_leave_on_consequence(MenuCallbackArgs args)
		{
			this.SettlementMenuLeaveConsequenceCommon();
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x00147E9A File Offset: 0x0014609A
		private void game_menu_request_meeting_castle_leave_on_consequence(MenuCallbackArgs args)
		{
			this.SettlementMenuLeaveConsequenceCommon();
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x00147EA2 File Offset: 0x001460A2
		private void SettlementMenuLeaveConsequenceCommon()
		{
			GameMenu.ExitToLast();
			this._parleyedParty = null;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x00147EB0 File Offset: 0x001460B0
		private void game_menu_request_meeting_with_on_consequence(MenuCallbackArgs args)
		{
			string text;
			string meetingScene = this.GetMeetingScene(out text);
			Hero hero = (Hero)args.MenuContext.GetSelectedObject();
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, PartyBase.MainParty, false, false, false, false, false, false);
			CharacterObject characterObject = hero.CharacterObject;
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(characterObject, (partyBelongedTo != null) ? partyBelongedTo.Party : null, true, false, false, false, false, false);
			CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, meetingScene, text, false);
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x00147F20 File Offset: 0x00146120
		private string GetMeetingScene(out string sceneLevel)
		{
			string text = GameSceneDataManager.Instance.MeetingScenes.GetRandomElementWithPredicate<MeetingSceneData>((MeetingSceneData x) => x.Culture == this._parleyedParty.Settlement.Culture).SceneID;
			if (string.IsNullOrEmpty(text))
			{
				text = GameSceneDataManager.Instance.MeetingScenes.GetRandomElement<MeetingSceneData>().SceneID;
			}
			sceneLevel = "";
			if (this._parleyedParty.Settlement.IsFortification)
			{
				sceneLevel = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(this._parleyedParty.Settlement.Town.GetWallLevel());
			}
			return text;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00147FB6 File Offset: 0x001461B6
		private bool game_meeting_town_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return this._parleyedParty.Settlement.IsTown;
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x00147FD0 File Offset: 0x001461D0
		private bool game_meeting_castle_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return this._parleyedParty.Settlement.IsCastle;
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00147FEA File Offset: 0x001461EA
		public PartyBase GetParleyedParty()
		{
			return this._parleyedParty;
		}

		// Token: 0x0400134D RID: 4941
		private PartyBase _parleyedParty;
	}
}
