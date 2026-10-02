using System;
using System.Linq;
using StoryMode.Quests.SecondPhase;
using StoryMode.Quests.SecondPhase.ConspiracyQuests;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000051 RID: 81
	public class SecondPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004F5 RID: 1269 RVA: 0x0001BE20 File Offset: 0x0001A020
		public SecondPhaseCampaignBehavior()
		{
			this._conspiracyQuestTriggerDayCounter = 0;
			this._isConspiracySetUpStarted = false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001BE38 File Offset: 0x0001A038
		public override void RegisterEvents()
		{
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(this.OnQuestStarted));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001BECF File Offset: 0x0001A0CF
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("_conspiracyQuestTriggerDayCounter", ref this._conspiracyQuestTriggerDayCounter);
			dataStore.SyncData<bool>("_isConspiracySetUpStarted", ref this._isConspiracySetUpStarted);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
		private void WeeklyTick()
		{
			int num = 14;
			SecondPhase instance = SecondPhase.Instance;
			int num2 = num + MBRandom.RandomIntWithSeed((uint)((instance != null) ? instance.LastConspiracyQuestCreationTime.ToMilliseconds : 53.0), 2000U) % 8;
			if (this._isConspiracySetUpStarted && StoryModeManager.Current.MainStoryLine.ThirdPhase == null && SecondPhase.Instance.ConspiracyStrength < 2000f && SecondPhase.Instance.LastConspiracyQuestCreationTime.ElapsedDaysUntilNow >= (float)num2 && !this.IsThereActiveConspiracyQuest())
			{
				SecondPhase.Instance.CreateNextConspiracyQuest();
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001BF89 File Offset: 0x0001A189
		private void OnQuestStarted(QuestBase quest)
		{
			if (quest is AssembleEmpireQuestBehavior.AssembleEmpireQuest || quest is WeakenEmpireQuestBehavior.WeakenEmpireQuest)
			{
				StoryModeManager.Current.MainStoryLine.CompleteFirstPhase();
				this._isConspiracySetUpStarted = true;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001BFB1 File Offset: 0x0001A1B1
		private void DailyTick()
		{
			if (this._isConspiracySetUpStarted && this._conspiracyQuestTriggerDayCounter < 10)
			{
				this._conspiracyQuestTriggerDayCounter++;
				if (this._conspiracyQuestTriggerDayCounter >= 10)
				{
					new ConspiracyProgressQuest().StartQuest();
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001BFE7 File Offset: 0x0001A1E7
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			SecondPhase instance = SecondPhase.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnSessionLaunched();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001BFF8 File Offset: 0x0001A1F8
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			foreach (MobileParty mobileParty in Campaign.Current.CustomParties.ToList<MobileParty>())
			{
				if (mobileParty.Name.HasSameValue(new TextObject("{=eVzg5Mtl}Conspiracy Caravan", null)))
				{
					bool flag = true;
					foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests)
					{
						if (questBase.GetType() == typeof(DisruptSupplyLinesConspiracyQuest) && ((DisruptSupplyLinesConspiracyQuest)questBase).ConspiracyCaravan == mobileParty)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						DestroyPartyAction.Apply(null, mobileParty);
					}
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
		private void OnConspiracyActivated()
		{
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
		private bool IsThereActiveConspiracyQuest()
		{
			foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests)
			{
				if (questBase.IsOngoing && typeof(ConspiracyQuestBase) == questBase.GetType().BaseType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040001D1 RID: 465
		private int _conspiracyQuestTriggerDayCounter;

		// Token: 0x040001D2 RID: 466
		private bool _isConspiracySetUpStarted;
	}
}
