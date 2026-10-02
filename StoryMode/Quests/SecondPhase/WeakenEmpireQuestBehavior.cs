using System;
using System.Collections.Generic;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000029 RID: 41
	public class WeakenEmpireQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000B9D9 File Offset: 0x00009BD9
		public override void RegisterEvents()
		{
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.OnMainStoryLineSideChosen));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000B9F2 File Offset: 0x00009BF2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000B9F4 File Offset: 0x00009BF4
		private void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			if (side == MainStoryLineSide.CreateAntiImperialKingdom || side == MainStoryLineSide.SupportAntiImperialKingdom)
			{
				new WeakenEmpireQuestBehavior.WeakenEmpireQuest(StoryModeHeroes.AntiImperialMentor).StartQuest();
			}
		}

		// Token: 0x0200006D RID: 109
		public class WeakenEmpireQuestBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060005F2 RID: 1522 RVA: 0x000210C3 File Offset: 0x0001F2C3
			public WeakenEmpireQuestBehaviorTypeDefiner()
				: base(1005000)
			{
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x000210D0 File Offset: 0x0001F2D0
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(WeakenEmpireQuestBehavior.WeakenEmpireQuest), 1, null);
			}
		}

		// Token: 0x0200006E RID: 110
		public class WeakenEmpireQuest : StoryModeQuestBase
		{
			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x060005F4 RID: 1524 RVA: 0x000210E4 File Offset: 0x0001F2E4
			private TextObject _startQuestLog
			{
				get
				{
					TextObject textObject = new TextObject("{=0wQlpbtL}In order for the Empire to go into its final decline, there should be fewer than {NUMBER} imperial-owned settlements. If this happens, another kingdom can become the dominant power in Calradia.", null);
					textObject.SetTextVariable("NUMBER", 4);
					return textObject;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000210FE File Offset: 0x0001F2FE
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=iR4QCTxv}Weaken Empire", null);
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0002110B File Offset: 0x0001F30B
			private TextObject _questCanceledLogText
			{
				get
				{
					return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
				}
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x00021118 File Offset: 0x0001F318
			public WeakenEmpireQuest(Hero questGiver)
				: base("weaken_empire_quest", questGiver, CampaignTime.Never)
			{
				this._weakenedEmpire = false;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
				base.AddLog(this._startQuestLog, false);
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x0002114C File Offset: 0x0001F34C
			protected override void SetDialogs()
			{
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=VeY3PQFL}You chose to defeat the Empire.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.CloseDialog();
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x0002118A File Offset: 0x0001F38A
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x060005FA RID: 1530 RVA: 0x00021192 File Offset: 0x0001F392
			protected override void RegisterEvents()
			{
				StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			}

			// Token: 0x060005FB RID: 1531 RVA: 0x000211C2 File Offset: 0x0001F3C2
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
				{
					base.CompleteQuestWithCancel(this._questCanceledLogText);
					StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
				}
			}

			// Token: 0x060005FC RID: 1532 RVA: 0x000211F9 File Offset: 0x0001F3F9
			protected override void HourlyTick()
			{
				if (this.QuestConditionsHold())
				{
					this.SuccessComplete();
				}
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x00021209 File Offset: 0x0001F409
			private void OnConspiracyActivated()
			{
				if (!this._weakenedEmpire)
				{
					base.CompleteQuestWithFail(new TextObject("{=JVkPkbdg}You could not weaken the Empire.", null));
				}
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x00021224 File Offset: 0x0001F424
			private bool QuestConditionsHold()
			{
				return StoryModeData.NorthernEmpireKingdom.Towns.Count + StoryModeData.WesternEmpireKingdom.Towns.Count + StoryModeData.SouthernEmpireKingdom.Towns.Count < 4;
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x00021258 File Offset: 0x0001F458
			private void SuccessComplete()
			{
				base.AddLog(new TextObject("{=wO19nK2y}You have weakened the Empire.", null), false);
				base.CompleteQuestWithSuccess();
				this._weakenedEmpire = true;
				SecondPhase.Instance.ActivateConspiracy();
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x00021284 File Offset: 0x0001F484
			internal static void AutoGeneratedStaticCollectObjectsWeakenEmpireQuest(object o, List<object> collectedObjects)
			{
				((WeakenEmpireQuestBehavior.WeakenEmpireQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x00021292 File Offset: 0x0001F492
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x04000226 RID: 550
			private const int EmpireDefeatSettlementCount = 4;

			// Token: 0x04000227 RID: 551
			private bool _weakenedEmpire;
		}
	}
}
