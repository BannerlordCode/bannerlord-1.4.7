using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000027 RID: 39
	public class ConspiracyProgressQuest : StoryModeQuestBase
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B32F File Offset: 0x0000952F
		private bool _isImperialSide
		{
			get
			{
				return StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000B340 File Offset: 0x00009540
		private TextObject _startQuestLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=oX2aoilb}{MENTOR.NAME} knows of the rise of your {KINGDOM_NAME}. Rumors say {MENTOR.NAME} is planning to undo your progress. Be ready!", null);
				StringHelpers.SetCharacterProperties("MENTOR", this._isImperialSide ? StoryModeHeroes.AntiImperialMentor.CharacterObject : StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("KINGDOM_NAME", (Clan.PlayerClan.Kingdom != null) ? Clan.PlayerClan.Kingdom.Name : Clan.PlayerClan.Name);
				return textObject;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000B3B8 File Offset: 0x000095B8
		private TextObject _questCanceledLogText
		{
			get
			{
				return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000B3C8 File Offset: 0x000095C8
		public override TextObject Title
		{
			get
			{
				TextObject textObject;
				if (this._isImperialSide)
				{
					textObject = new TextObject("{=PJ5C3Dim}{ANTIIMPERIAL_MENTOR.NAME}'s Conspiracy", null);
					StringHelpers.SetCharacterProperties("ANTIIMPERIAL_MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				}
				else
				{
					textObject = new TextObject("{=i3SSc0I4}{IMPERIAL_MENTOR.NAME}'s Plan", null);
					StringHelpers.SetCharacterProperties("IMPERIAL_MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				}
				return textObject;
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000B426 File Offset: 0x00009626
		public ConspiracyProgressQuest()
			: base("conspiracy_quest_campaign_behavior", null, CampaignTime.Never)
		{
			SecondPhase.Instance.TriggerConspiracy();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000B443 File Offset: 0x00009643
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000B44B File Offset: 0x0000964B
		protected override void HourlyTick()
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B450 File Offset: 0x00009650
		protected override void RegisterEvents()
		{
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000B4A2 File Offset: 0x000096A2
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
			{
				base.CompleteQuestWithCancel(this._questCanceledLogText);
				StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B4D9 File Offset: 0x000096D9
		protected override void OnStartQuest()
		{
			this._startQuestLog = base.AddDiscreteLog(this._startQuestLogText, new TextObject("{=1LrHV647}Conspiracy Strength", null), (int)SecondPhase.Instance.ConspiracyStrength, 2000, null, false);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B50A File Offset: 0x0000970A
		protected override void SetDialogs()
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B50C File Offset: 0x0000970C
		protected override void OnFinalize()
		{
			base.OnFinalize();
			foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests.ToList<QuestBase>())
			{
				if (typeof(ConspiracyQuestBase) == questBase.GetType().BaseType && questBase.IsOngoing)
				{
					questBase.CompleteQuestWithCancel(new TextObject("{=YJxCbbpd}Conspiracy is activated!", null));
				}
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B5A4 File Offset: 0x000097A4
		protected override void DailyTick()
		{
			StoryModeManager.Current.MainStoryLine.SecondPhase.IncreaseConspiracyStrength();
			this._startQuestLog.UpdateCurrentProgress((int)StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyStrength);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B5DC File Offset: 0x000097DC
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (detail == QuestBase.QuestCompleteDetails.Success && typeof(ConspiracyQuestBase) == quest.GetType().BaseType)
			{
				this._startQuestLog.UpdateCurrentProgress((int)StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyStrength);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B629 File Offset: 0x00009829
		private void OnConspiracyActivated()
		{
			base.CompleteQuestWithTimeOut(null);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000B632 File Offset: 0x00009832
		internal static void AutoGeneratedStaticCollectObjectsConspiracyProgressQuest(object o, List<object> collectedObjects)
		{
			((ConspiracyProgressQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B640 File Offset: 0x00009840
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._startQuestLog);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000B655 File Offset: 0x00009855
		internal static object AutoGeneratedGetMemberValue_startQuestLog(object o)
		{
			return ((ConspiracyProgressQuest)o)._startQuestLog;
		}

		// Token: 0x040000AF RID: 175
		[SaveableField(2)]
		private JournalLog _startQuestLog;
	}
}
