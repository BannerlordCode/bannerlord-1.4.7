using System;
using System.Collections.Generic;
using StoryMode.Quests.TutorialPhase;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000054 RID: 84
	public class StoryModeTutorialBoxCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001D9D5 File Offset: 0x0001BBD5
		public MBReadOnlyList<CampaignTutorial> AvailableTutorials
		{
			get
			{
				return this._availableTutorials;
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001D9DD File Offset: 0x0001BBDD
		public StoryModeTutorialBoxCampaignBehavior()
		{
			this._shownTutorials = new List<string>();
			this._availableTutorials = new MBList<CampaignTutorial>();
			this._tutorialBackup = new Dictionary<string, int>();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001DA08 File Offset: 0x0001BC08
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnTutorialCompletedEvent.AddNonSerializedListener(this, new Action<string>(this.OnTutorialCompleted));
			CampaignEvents.CollectAvailableTutorialsEvent.AddNonSerializedListener(this, new Action<List<CampaignTutorial>>(this.OnTutorialListRequested));
			CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(this.OnQuestStarted));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			StoryModeEvents.OnTravelToVillageTutorialQuestStartedEvent.AddNonSerializedListener(this, new Action(this.OnTravelToVillageTutorialQuestStarted));
			Game.Current.EventManager.RegisterEvent<ResetAllTutorialsEvent>(new Action<ResetAllTutorialsEvent>(this.OnResetAllTutorials));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001DABA File Offset: 0x0001BCBA
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<string>>("_shownTutorials", ref this._shownTutorials);
			dataStore.SyncData<Dictionary<string, int>>("_tutorialBackup", ref this._tutorialBackup);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.BackupTutorial("MovementInMissionTutorial", 5);
			int num = 100;
			this.BackupTutorial("EncyclopediaHomeTutorial", num++);
			this.BackupTutorial("EncyclopediaSettlementsTutorial", num++);
			this.BackupTutorial("EncyclopediaTroopsTutorial", num++);
			this.BackupTutorial("EncyclopediaKingdomsTutorial", num++);
			this.BackupTutorial("EncyclopediaClansTutorial", num++);
			this.BackupTutorial("EncyclopediaConceptsTutorial", num++);
			this.BackupTutorial("EncyclopediaTrackTutorial", num++);
			this.BackupTutorial("EncyclopediaSearchTutorial", num++);
			this.BackupTutorial("EncyclopediaFiltersTutorial", num++);
			this.BackupTutorial("EncyclopediaSortTutorial", num++);
			this.BackupTutorial("EncyclopediaFogOfWarTutorial", num++);
			this.BackupTutorial("RaidVillageStep1", num++);
			this.BackupTutorial("UpgradingTroopsStep1", num++);
			this.BackupTutorial("UpgradingTroopsStep2", num++);
			this.BackupTutorial("UpgradingTroopsStep3", num++);
			this.BackupTutorial("ChoosingPerkUpgradesStep1", num++);
			this.BackupTutorial("ChoosingPerkUpgradesStep2", num++);
			this.BackupTutorial("ChoosingPerkUpgradesStep3", num++);
			this.BackupTutorial("ChoosingSkillFocusStep1", num++);
			this.BackupTutorial("ChoosingSkillFocusStep2", num++);
			this.BackupTutorial("GettingCompanionsStep1", num++);
			this.BackupTutorial("GettingCompanionsStep2", num++);
			this.BackupTutorial("GettingCompanionsStep3", num++);
			this.BackupTutorial("RansomingPrisonersStep1", num++);
			this.BackupTutorial("RansomingPrisonersStep2", num++);
			this.BackupTutorial("EquipmentSets", num++);
			this.BackupTutorial("PartySpeed", num++);
			this.BackupTutorial("ArmyCohesionStep1", num++);
			this.BackupTutorial("ArmyCohesionStep2", num++);
			this.BackupTutorial("CreateArmyStep2", num++);
			this.BackupTutorial("CreateArmyStep3", num++);
			this.BackupTutorial("OrderOfBattleTutorialStep1", num++);
			this.BackupTutorial("OrderOfBattleTutorialStep2", num++);
			this.BackupTutorial("OrderOfBattleTutorialStep3", num++);
			this.BackupTutorial("CraftingStep1Tutorial", num++);
			this.BackupTutorial("CraftingOrdersTutorial", num++);
			this.BackupTutorial("InventoryBannerItemTutorial", num++);
			this.BackupTutorial("CrimeTutorial", num++);
			this.BackupTutorial("AssignRolesTutorial", num++);
			this.BackupTutorial("BombardmentStep1", num++);
			this.BackupTutorial("KingdomDecisionVotingTutorial", num++);
			foreach (KeyValuePair<string, int> keyValuePair in this._tutorialBackup)
			{
				this.AddTutorial(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001DDE0 File Offset: 0x0001BFE0
		private void OnTravelToVillageTutorialQuestStarted()
		{
			this.AddTutorial("SeeMarkersInMissionTutorial", 1);
			this.AddTutorial("NavigateOnMapTutorialStep1", 2);
			this.AddTutorial("NavigateOnMapTutorialStep2", 3);
			this.AddTutorial("EnterVillageTutorial", 4);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001DE14 File Offset: 0x0001C014
		private void OnQuestStarted(QuestBase quest)
		{
			if (quest is PurchaseGrainTutorialQuest)
			{
				this.AddTutorial("PressLeaveToReturnFromMissionType1", 10);
				this.AddTutorial("GetSuppliesTutorialStep1", 20);
				this.AddTutorial("GetSuppliesTutorialStep3", 22);
			}
			else if (quest is RecruitTroopsTutorialQuest)
			{
				this.AddTutorial("RecruitmentTutorialStep1", 11);
				this.AddTutorial("RecruitmentTutorialStep2", 12);
			}
			else if (quest is LocateAndRescueTravellerTutorialQuest)
			{
				this.AddTutorial("PressLeaveToReturnFromMissionType2", 30);
				this.AddTutorial("OrderTutorial1TutorialStep2", 33);
				this.AddTutorial("TakeAndRescuePrisonerTutorial", 34);
				this.AddTutorial("OrderTutorial2Tutorial", 35);
			}
			else if (quest is VillagersInNeed)
			{
				this.AddTutorial("StealthCrouchTutorial", 36);
				this.AddTutorial("StealthWalkSlowTutorial", 37);
				this.AddTutorial("StealthHideInBushesTutorial", 38);
				this.AddTutorial("StealthDistractionTutorial", 39);
				this.AddTutorial("StealthDarkZoneTutorial", 40);
				this.AddTutorial("StealthStealthKillTutorial", 41);
				this.AddTutorial("StealthHideCorpseTutorial", 42);
			}
			this._availableTutorials.Sort((CampaignTutorial x, CampaignTutorial y) => x.Priority.CompareTo(y.Priority));
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001DF48 File Offset: 0x0001C148
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (TutorialPhase.Instance.TutorialQuestPhase == TutorialQuestPhase.RecruitAndPurchaseStarted && ((quest is RecruitTroopsTutorialQuest && !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(PurchaseGrainTutorialQuest))) || (quest is PurchaseGrainTutorialQuest && !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RecruitTroopsTutorialQuest)))))
			{
				this.AddTutorial("TalkToNotableTutorialStep1", 40);
				this.AddTutorial("TalkToNotableTutorialStep2", 41);
			}
			this._availableTutorials.Sort((CampaignTutorial x, CampaignTutorial y) => x.Priority.CompareTo(y.Priority));
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001DFEC File Offset: 0x0001C1EC
		private void OnTutorialCompleted(string completedTutorialType)
		{
			CampaignTutorial campaignTutorial = this._availableTutorials.Find((CampaignTutorial t) => t.TutorialTypeId == completedTutorialType);
			if (campaignTutorial != null)
			{
				this._availableTutorials.Remove(campaignTutorial);
				this._shownTutorials.Add(completedTutorialType);
				this._tutorialBackup.Remove(completedTutorialType);
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001E054 File Offset: 0x0001C254
		private void OnTutorialListRequested(List<CampaignTutorial> campaignTutorials)
		{
			if (!BannerlordConfig.EnableTutorialHints)
			{
				return;
			}
			MBTextManager.SetTextVariable("TUTORIAL_SETTLEMENT_NAME", MBObjectManager.Instance.GetObject<Settlement>("village_ES3_2").Name, false);
			foreach (CampaignTutorial campaignTutorial in this.AvailableTutorials)
			{
				campaignTutorials.Add(campaignTutorial);
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001E0D0 File Offset: 0x0001C2D0
		private void BackupTutorial(string tutorialTypeId, int priority)
		{
			if (!this._shownTutorials.Contains(tutorialTypeId) && !this._tutorialBackup.ContainsKey(tutorialTypeId))
			{
				this._tutorialBackup.Add(tutorialTypeId, priority);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001E0FC File Offset: 0x0001C2FC
		private void AddTutorial(string tutorialTypeId, int priority)
		{
			if (!this._shownTutorials.Contains(tutorialTypeId))
			{
				CampaignTutorial campaignTutorial = new CampaignTutorial(tutorialTypeId, priority);
				this._availableTutorials.Add(campaignTutorial);
				if (!this._tutorialBackup.ContainsKey(tutorialTypeId))
				{
					this._tutorialBackup.Add(tutorialTypeId, priority);
				}
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001E146 File Offset: 0x0001C346
		public void OnResetAllTutorials(ResetAllTutorialsEvent obj)
		{
			this._shownTutorials.Clear();
		}

		// Token: 0x040001D8 RID: 472
		private List<string> _shownTutorials;

		// Token: 0x040001D9 RID: 473
		private readonly MBList<CampaignTutorial> _availableTutorials;

		// Token: 0x040001DA RID: 474
		private Dictionary<string, int> _tutorialBackup;
	}
}
