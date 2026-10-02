using System;
using System.Collections.Generic;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000026 RID: 38
	public class AssembleEmpireQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x0000B2F3 File Offset: 0x000094F3
		public override void RegisterEvents()
		{
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.OnMainStoryLineSideChosen));
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000B30C File Offset: 0x0000950C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B30E File Offset: 0x0000950E
		private void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			if (side == MainStoryLineSide.CreateImperialKingdom || side == MainStoryLineSide.SupportImperialKingdom)
			{
				new AssembleEmpireQuestBehavior.AssembleEmpireQuest(StoryModeHeroes.ImperialMentor).StartQuest();
			}
		}

		// Token: 0x02000068 RID: 104
		public class AssembleEmpireQuestBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060005D6 RID: 1494 RVA: 0x00020C24 File Offset: 0x0001EE24
			public AssembleEmpireQuestBehaviorTypeDefiner()
				: base(1002000)
			{
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x00020C31 File Offset: 0x0001EE31
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(AssembleEmpireQuestBehavior.AssembleEmpireQuest), 1, null);
			}
		}

		// Token: 0x02000069 RID: 105
		public class AssembleEmpireQuest : StoryModeQuestBase
		{
			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00020C45 File Offset: 0x0001EE45
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=ya8eMCpj}Unify the Empire", null);
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00020C52 File Offset: 0x0001EE52
			private TextObject _questCanceledLogText
			{
				get
				{
					return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
				}
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x00020C60 File Offset: 0x0001EE60
			public AssembleEmpireQuest(Hero questGiver)
				: base("assemble_empire_quest", questGiver, CampaignTime.Never)
			{
				this._assembledEmpire = false;
				this.CacheSettlementCounts();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
				this._numberOfCapturedSettlementsLog = base.AddDiscreteLog(new TextObject("{=3deb2lMd}To restore the Empire you should capture two thirds of settlements with imperial culture.", null), new TextObject("{=Dp6newHS}Conquered Settlements", null), this._ownedByPlayerImperialTowns, MathF.Ceiling((float)this._imperialCultureTowns * 0.66f), null, false);
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x00020CD3 File Offset: 0x0001EED3
			protected override void SetDialogs()
			{
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=mxKhvbn7}You have decided to unify the Empire.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.CloseDialog();
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x00020D14 File Offset: 0x0001EF14
			protected override void InitializeQuestOnGameLoad()
			{
				this.CacheSettlementCounts();
				this.SetDialogs();
				if (this._numberOfCapturedSettlementsLog == null)
				{
					this._numberOfCapturedSettlementsLog = base.AddDiscreteLog(new TextObject("{=3deb2lMd}To restore the Empire you should capture two thirds of settlements with imperial culture.", null), new TextObject("{=Dp6newHS}Conquered Settlements", null), this._ownedByPlayerImperialTowns, MathF.Ceiling((float)this._imperialCultureTowns * 0.66f), null, false);
				}
				this._numberOfCapturedSettlementsLog.UpdateCurrentProgress((int)MathF.Clamp((float)this._ownedByPlayerImperialTowns, 0f, (float)this._imperialCultureTowns));
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x00020D98 File Offset: 0x0001EF98
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
				StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x00020DEA File Offset: 0x0001EFEA
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
				{
					base.CompleteQuestWithCancel(this._questCanceledLogText);
					StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
				}
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x00020E24 File Offset: 0x0001F024
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement.IsTown && settlement.Culture.StringId == "empire")
				{
					if (settlement.OwnerClan.Kingdom == Clan.PlayerClan.Kingdom && oldOwner.Clan.Kingdom != Clan.PlayerClan.Kingdom)
					{
						this._ownedByPlayerImperialTowns++;
					}
					if (oldOwner.Clan.Kingdom == Clan.PlayerClan.Kingdom && newOwner.Clan.Kingdom != Clan.PlayerClan.Kingdom)
					{
						this._ownedByPlayerImperialTowns--;
					}
					this._numberOfCapturedSettlementsLog.UpdateCurrentProgress((int)MathF.Clamp((float)this._ownedByPlayerImperialTowns, 0f, (float)this._imperialCultureTowns));
				}
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x00020EF4 File Offset: 0x0001F0F4
			protected override void HourlyTick()
			{
				if (this.QuestConditionsHold())
				{
					this.SuccessQuest();
				}
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x00020F04 File Offset: 0x0001F104
			private void OnConspiracyActivated()
			{
				if (!this._assembledEmpire)
				{
					base.CompleteQuestWithFail(new TextObject("{=80NOk1Ee}You could not unify the Empire.", null));
				}
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x00020F20 File Offset: 0x0001F120
			private void CacheSettlementCounts()
			{
				this._imperialCultureTowns = 0;
				this._ownedByPlayerImperialTowns = 0;
				foreach (Settlement settlement in Settlement.All)
				{
					if (settlement.IsTown && settlement.Culture.StringId == "empire")
					{
						this._imperialCultureTowns++;
						if (settlement.OwnerClan.Kingdom == Clan.PlayerClan.Kingdom)
						{
							this._ownedByPlayerImperialTowns++;
						}
					}
				}
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x00020FCC File Offset: 0x0001F1CC
			private bool QuestConditionsHold()
			{
				return this._ownedByPlayerImperialTowns >= MathF.Ceiling((float)this._imperialCultureTowns * 0.66f);
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x00020FEB File Offset: 0x0001F1EB
			private void SuccessQuest()
			{
				base.AddLog(new TextObject("{=sJeYHMGG}You have unified the Empire.", null), false);
				base.CompleteQuestWithSuccess();
				this._assembledEmpire = true;
				SecondPhase.Instance.ActivateConspiracy();
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x00021017 File Offset: 0x0001F217
			internal static void AutoGeneratedStaticCollectObjectsAssembleEmpireQuest(object o, List<object> collectedObjects)
			{
				((AssembleEmpireQuestBehavior.AssembleEmpireQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x00021025 File Offset: 0x0001F225
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._numberOfCapturedSettlementsLog);
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x0002103A File Offset: 0x0001F23A
			internal static object AutoGeneratedGetMemberValue_numberOfCapturedSettlementsLog(object o)
			{
				return ((AssembleEmpireQuestBehavior.AssembleEmpireQuest)o)._numberOfCapturedSettlementsLog;
			}

			// Token: 0x0400021C RID: 540
			private int _imperialCultureTowns;

			// Token: 0x0400021D RID: 541
			private int _ownedByPlayerImperialTowns;

			// Token: 0x0400021E RID: 542
			private bool _assembledEmpire;

			// Token: 0x0400021F RID: 543
			private const float _ratioOfSettlementToTake = 0.66f;

			// Token: 0x04000220 RID: 544
			[SaveableField(1)]
			private JournalLog _numberOfCapturedSettlementsLog;
		}
	}
}
