using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000B8 RID: 184
	public class RivalGangMovingInIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x00033C20 File Offset: 0x00031E20
		private static RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest Instance
		{
			get
			{
				RivalGangMovingInIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<RivalGangMovingInIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest rivalGangMovingInIssueQuest;
						if ((rivalGangMovingInIssueQuest = enumerator.Current as RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest) != null)
						{
							campaignBehavior._cachedQuest = rivalGangMovingInIssueQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00033CB8 File Offset: 0x00031EB8
		private void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssue), IssueBase.IssueFrequency.Common, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00033D1C File Offset: 0x00031F1C
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			Hero rivalGangLeader = this.GetRivalGangLeader(issueOwner);
			return new RivalGangMovingInIssueBehavior.RivalGangMovingInIssue(issueOwner, rivalGangLeader);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00033D38 File Offset: 0x00031F38
		private static void rival_gang_wait_duration_is_over_menu_on_init(MenuCallbackArgs args)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			TextObject textObject = new TextObject("{=9Kr9pjGs}{QUEST_GIVER.LINK} has prepared {?QUEST_GIVER.GENDER}her{?}his{\\?} men and is waiting for you.", null);
			StringHelpers.SetCharacterProperties("QUEST_GIVER", RivalGangMovingInIssueBehavior.Instance.QuestGiver.CharacterObject, null, false);
			MBTextManager.SetTextVariable("MENU_TEXT", textObject, false);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00033D84 File Offset: 0x00031F84
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsGangLeader && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.IsTown && issueGiver.CurrentSettlement.Town.Security <= 60f && this.GetRivalGangLeader(issueGiver) != null;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00033DD4 File Offset: 0x00031FD4
		private void rival_gang_quest_wait_duration_is_over_yes_consequence(MenuCallbackArgs args)
		{
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(RivalGangMovingInIssueBehavior.Instance.QuestGiver.CharacterObject, null, true, true, false, false, false, false));
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00033E14 File Offset: 0x00032014
		private Hero GetRivalGangLeader(Hero issueOwner)
		{
			Hero hero = null;
			foreach (Hero hero2 in issueOwner.CurrentSettlement.Notables)
			{
				if (hero2 != issueOwner && hero2.IsGangLeader && hero2.CanHaveCampaignIssues())
				{
					hero = hero2;
					break;
				}
			}
			return hero;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00033E80 File Offset: 0x00032080
		private bool rival_gang_quest_wait_duration_is_over_yes_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00033E8B File Offset: 0x0003208B
		private bool rival_gang_quest_wait_duration_is_over_no_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00033E96 File Offset: 0x00032096
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00033EC8 File Offset: 0x000320C8
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("rival_gang_quest_before_fight", "", new OnInitDelegate(RivalGangMovingInIssueBehavior.rival_gang_quest_before_fight_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenu("rival_gang_quest_after_fight", "", new OnInitDelegate(RivalGangMovingInIssueBehavior.rival_gang_quest_after_fight_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenu("rival_gang_quest_wait_duration_is_over", "{MENU_TEXT}", new OnInitDelegate(RivalGangMovingInIssueBehavior.rival_gang_wait_duration_is_over_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("rival_gang_quest_wait_duration_is_over", "rival_gang_quest_wait_duration_is_over_yes", "{=aka03VdU}Meet {?QUEST_GIVER.GENDER}her{?}him{\\?} now", new GameMenuOption.OnConditionDelegate(this.rival_gang_quest_wait_duration_is_over_yes_condition), new GameMenuOption.OnConsequenceDelegate(this.rival_gang_quest_wait_duration_is_over_yes_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("rival_gang_quest_wait_duration_is_over", "rival_gang_quest_wait_duration_is_over_no", "{=NIzQb6nT}Leave and meet {?QUEST_GIVER.GENDER}her{?}him{\\?} later", new GameMenuOption.OnConditionDelegate(this.rival_gang_quest_wait_duration_is_over_no_condition), new GameMenuOption.OnConsequenceDelegate(this.rival_gang_quest_wait_duration_is_over_no_consequence), true, -1, false, null);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00033F94 File Offset: 0x00032194
		private void rival_gang_quest_wait_duration_is_over_no_consequence(MenuCallbackArgs args)
		{
			Campaign.Current.CurrentMenuContext.SwitchToMenu("town_wait_menus");
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00033FAA File Offset: 0x000321AA
		private static void rival_gang_quest_before_fight_init(MenuCallbackArgs args)
		{
			if (RivalGangMovingInIssueBehavior.Instance != null && RivalGangMovingInIssueBehavior.Instance._isFinalStage)
			{
				RivalGangMovingInIssueBehavior.Instance.StartAlleyBattle();
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00033FCC File Offset: 0x000321CC
		private static void rival_gang_quest_after_fight_init(MenuCallbackArgs args)
		{
			if (RivalGangMovingInIssueBehavior.Instance != null && RivalGangMovingInIssueBehavior.Instance._isReadyToBeFinalized)
			{
				bool flag = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide;
				PlayerEncounter.Current.FinalizeBattle();
				RivalGangMovingInIssueBehavior.Instance.HandlePlayerEncounterResult(flag);
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00034018 File Offset: 0x00032218
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0003401C File Offset: 0x0003221C
		[GameMenuInitializationHandler("rival_gang_quest_after_fight")]
		[GameMenuInitializationHandler("rival_gang_quest_wait_duration_is_over")]
		private static void game_menu_rival_gang_quest_end_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(currentSettlement.SettlementComponent.WaitMeshName);
			}
		}

		// Token: 0x04000419 RID: 1049
		private const IssueBase.IssueFrequency RivalGangLeaderIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x0400041A RID: 1050
		private RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest _cachedQuest;

		// Token: 0x020001CC RID: 460
		public class RivalGangMovingInIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06001164 RID: 4452 RVA: 0x00070872 File Offset: 0x0006EA72
			public RivalGangMovingInIssueTypeDefiner()
				: base(310000)
			{
			}

			// Token: 0x06001165 RID: 4453 RVA: 0x0007087F File Offset: 0x0006EA7F
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssue), 1, null);
				base.AddClassDefinition(typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest), 2, null);
			}
		}

		// Token: 0x020001CD RID: 461
		public class RivalGangMovingInIssue : IssueBase
		{
			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06001166 RID: 4454 RVA: 0x000708A5 File Offset: 0x0006EAA5
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x06001167 RID: 4455 RVA: 0x000708A9 File Offset: 0x0006EAA9
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=pzvQ1DkE}Your companion has defeated the rival gang and protected the interests of {QUEST_GIVER.LINK} in {SETTLEMENT}.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001B3 RID: 435
			// (get) Token: 0x06001168 RID: 4456 RVA: 0x000708E9 File Offset: 0x0006EAE9
			// (set) Token: 0x06001169 RID: 4457 RVA: 0x000708F1 File Offset: 0x0006EAF1
			[SaveableProperty(207)]
			public Hero RivalGangLeader { get; private set; }

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x0600116A RID: 4458 RVA: 0x000708FA File Offset: 0x0006EAFA
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 4 + MathF.Ceiling(6f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001B5 RID: 437
			// (get) Token: 0x0600116B RID: 4459 RVA: 0x0007090F File Offset: 0x0006EB0F
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001B6 RID: 438
			// (get) Token: 0x0600116C RID: 4460 RVA: 0x00070924 File Offset: 0x0006EB24
			protected override int RewardGold
			{
				get
				{
					return (int)(600f + 1700f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001B7 RID: 439
			// (get) Token: 0x0600116D RID: 4461 RVA: 0x00070939 File Offset: 0x0006EB39
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(750f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x0600116E RID: 4462 RVA: 0x00070950 File Offset: 0x0006EB50
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=GXk6f9ah}I've got a problem... [ib:confident][if:convo_undecided_closed]And {?TARGET_NOTABLE.GENDER}her{?}his{\\?} name is {TARGET_NOTABLE.LINK}. {?TARGET_NOTABLE.GENDER}Her{?}His{\\?} people have been coming around outside the walls, robbing the dice-players and the drinkers enjoying themselves under our protection. Me and my boys are eager to teach them a lesson but I figure some extra muscle wouldn't hurt.", null);
					if (base.IssueOwner.RandomInt(2) == 0)
					{
						textObject = new TextObject("{=rgTGzfzI}Yeah. I have a problem all right. [ib:confident][if:convo_undecided_closed]{?TARGET_NOTABLE.GENDER}Her{?}His{\\?} name is {TARGET_NOTABLE.LINK}. {?TARGET_NOTABLE.GENDER}Her{?}His{\\?} people have been bothering shop owners under our protection, demanding money and making threats. Let me tell you something - those shop owners are my cows, and no one else gets to milk them. We're ready to teach these interlopers a lesson, but I could use some help.", null);
					}
					if (this.RivalGangLeader != null)
					{
						StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this.RivalGangLeader.CharacterObject, textObject, false);
					}
					return textObject;
				}
			}

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x0600116F RID: 4463 RVA: 0x000709A4 File Offset: 0x0006EBA4
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=kc6vCycY}What exactly do you want me to do?", null);
				}
			}

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x06001170 RID: 4464 RVA: 0x000709B1 File Offset: 0x0006EBB1
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=tyyAfWRR}We already had a small scuffle with them recently. [if:convo_mocking_revenge]They'll be waiting for us to come down hard. Instead, we'll hold off for {NUMBER} days. Let them think that we're backing off… Then, after {NUMBER} days, your men and mine will hit them in the middle of the night when they least expect it. I'll send you a messenger when the time comes and we'll strike them down together.", null);
					textObject.SetTextVariable("NUMBER", 2);
					return textObject;
				}
			}

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x06001171 RID: 4465 RVA: 0x000709CB File Offset: 0x0006EBCB
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=sSIjPCPO}If you'd rather not go into the fray yourself, [if:convo_mocking_aristocratic]you can leave me one of your companions together with {TROOP_COUNT} or so good men. If they stuck around for {RETURN_DAYS} days or so, I'd count it a very big favor.", null);
					textObject.SetTextVariable("TROOP_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x06001172 RID: 4466 RVA: 0x000709FC File Offset: 0x0006EBFC
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=ymbVPod1}{ISSUE_GIVER.LINK}, a gang leader from {SETTLEMENT}, has told you about a new gang that is trying to get a hold on the town. You asked {COMPANION.LINK} to take {TROOP_COUNT} of your best men to stay with {ISSUE_GIVER.LINK} and help {?ISSUE_GIVER.GENDER}her{?}him{\\?} in the coming gang war. They should return to you in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("TROOP_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001BD RID: 445
			// (get) Token: 0x06001173 RID: 4467 RVA: 0x00070A8D File Offset: 0x0006EC8D
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=LdCte9H0}I'll fight the other gang with you myself.", null);
				}
			}

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x06001174 RID: 4468 RVA: 0x00070A9A File Offset: 0x0006EC9A
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=AdbiUqtT}I'm busy, but I will leave a companion and some men.", null);
				}
			}

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x06001175 RID: 4469 RVA: 0x00070AA7 File Offset: 0x0006ECA7
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=0enbhess}Thank you. [ib:normal][if:convo_approving]I'm sure your guys are worth their salt..", null);
				}
			}

			// Token: 0x170001C0 RID: 448
			// (get) Token: 0x06001176 RID: 4470 RVA: 0x00070AB4 File Offset: 0x0006ECB4
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=QR0V8Ae5}Our lads are well hidden nearby,[ib:normal][if:convo_excited] waiting for the signal to go get those bastards. I won't forget this little favor you're doing me.", null);
				}
			}

			// Token: 0x170001C1 RID: 449
			// (get) Token: 0x06001177 RID: 4471 RVA: 0x00070AC1 File Offset: 0x0006ECC1
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170001C2 RID: 450
			// (get) Token: 0x06001178 RID: 4472 RVA: 0x00070AC4 File Offset: 0x0006ECC4
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001C3 RID: 451
			// (get) Token: 0x06001179 RID: 4473 RVA: 0x00070AC7 File Offset: 0x0006ECC7
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=vAjgn7yx}Rival Gang Moving in at {SETTLEMENT}", null);
					string text = "SETTLEMENT";
					Settlement issueSettlement = base.IssueSettlement;
					textObject.SetTextVariable(text, ((issueSettlement != null) ? issueSettlement.Name : null) ?? base.IssueOwner.HomeSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001C4 RID: 452
			// (get) Token: 0x0600117A RID: 4474 RVA: 0x00070B06 File Offset: 0x0006ED06
			public override TextObject Description
			{
				get
				{
					return new TextObject("{=H4EVfKAh}Gang leader needs help to beat the rival gang.", null);
				}
			}

			// Token: 0x170001C5 RID: 453
			// (get) Token: 0x0600117B RID: 4475 RVA: 0x00070B14 File Offset: 0x0006ED14
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=C9feTaca}I hear {QUEST_GIVER.LINK} is going to sort it out with {RIVAL_GANG_LEADER.LINK} once and for all.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("RIVAL_GANG_LEADER", this.RivalGangLeader.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x0600117C RID: 4476 RVA: 0x00070B5E File Offset: 0x0006ED5E
			protected override bool IssueQuestCanBeDuplicated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600117D RID: 4477 RVA: 0x00070B61 File Offset: 0x0006ED61
			public RivalGangMovingInIssue(Hero issueOwner, Hero rivalGangLeader)
				: base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this.RivalGangLeader = rivalGangLeader;
			}

			// Token: 0x0600117E RID: 4478 RVA: 0x00070B7B File Offset: 0x0006ED7B
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this.RivalGangLeader)
				{
					result = false;
				}
			}

			// Token: 0x0600117F RID: 4479 RVA: 0x00070B89 File Offset: 0x0006ED89
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -0.5f;
				}
				return 0f;
			}

			// Token: 0x06001180 RID: 4480 RVA: 0x00070BAC File Offset: 0x0006EDAC
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				ChangeRelationAction.ApplyPlayerRelation(this.RivalGangLeader, -5, true, true);
				base.IssueOwner.AddPower(10f);
				this.RivalGangLeader.AddPower(-10f);
			}

			// Token: 0x06001181 RID: 4481 RVA: 0x00070BE4 File Offset: 0x0006EDE4
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
				base.IssueSettlement.Town.Security += -10f;
				base.IssueOwner.AddPower(-10f);
			}

			// Token: 0x06001182 RID: 4482 RVA: 0x00070C1C File Offset: 0x0006EE1C
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				int skillValue = hero.GetSkillValue(DefaultSkills.OneHanded);
				int skillValue2 = hero.GetSkillValue(DefaultSkills.TwoHanded);
				int skillValue3 = hero.GetSkillValue(DefaultSkills.Polearm);
				int skillValue4 = hero.GetSkillValue(DefaultSkills.Roguery);
				if (skillValue >= skillValue2 && skillValue >= skillValue3 && skillValue >= skillValue4)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.OneHanded, 150);
				}
				if (skillValue2 >= skillValue3 && skillValue2 >= skillValue4)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.TwoHanded, 150);
				}
				if (skillValue3 < skillValue4)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.Roguery, 120);
				}
				return new ValueTuple<SkillObject, int>(DefaultSkills.Polearm, 150);
			}

			// Token: 0x06001183 RID: 4483 RVA: 0x00070CAD File Offset: 0x0006EEAD
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06001184 RID: 4484 RVA: 0x00070CC7 File Offset: 0x0006EEC7
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06001185 RID: 4485 RVA: 0x00070CD8 File Offset: 0x0006EED8
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06001186 RID: 4486 RVA: 0x00070CE6 File Offset: 0x0006EEE6
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06001187 RID: 4487 RVA: 0x00070CE8 File Offset: 0x0006EEE8
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001188 RID: 4488 RVA: 0x00070CEA File Offset: 0x0006EEEA
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest(questId, base.IssueOwner, this.RivalGangLeader, 8, this.RewardGold, base.IssueDifficultyMultiplier);
			}

			// Token: 0x06001189 RID: 4489 RVA: 0x00070D0B File Offset: 0x0006EF0B
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x0600118A RID: 4490 RVA: 0x00070D10 File Offset: 0x0006EF10
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				flag = IssueBase.PreconditionFlags.None;
				relationHero = null;
				requiredGold = 0;
				skill = null;
				if (Hero.MainHero.IsWounded)
				{
					flag |= IssueBase.PreconditionFlags.Wounded;
				}
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (base.IssueOwner.CurrentSettlement.OwnerClan == Clan.PlayerClan)
				{
					flag |= IssueBase.PreconditionFlags.PlayerIsOwnerOfSettlement;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x0600118B RID: 4491 RVA: 0x00070D98 File Offset: 0x0006EF98
			public override bool IssueStayAliveConditions()
			{
				return this.RivalGangLeader.IsAlive && base.IssueOwner.CurrentSettlement.OwnerClan != Clan.PlayerClan && base.IssueOwner.CurrentSettlement.Town.Security <= 80f;
			}

			// Token: 0x0600118C RID: 4492 RVA: 0x00070DEA File Offset: 0x0006EFEA
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x0600118D RID: 4493 RVA: 0x00070DEC File Offset: 0x0006EFEC
			internal static void AutoGeneratedStaticCollectObjectsRivalGangMovingInIssue(object o, List<object> collectedObjects)
			{
				((RivalGangMovingInIssueBehavior.RivalGangMovingInIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600118E RID: 4494 RVA: 0x00070DFA File Offset: 0x0006EFFA
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this.RivalGangLeader);
			}

			// Token: 0x0600118F RID: 4495 RVA: 0x00070E0F File Offset: 0x0006F00F
			internal static object AutoGeneratedGetMemberValueRivalGangLeader(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssue)o).RivalGangLeader;
			}

			// Token: 0x04000868 RID: 2152
			private const int AlternativeSolutionRelationChange = 5;

			// Token: 0x04000869 RID: 2153
			private const int AlternativeSolutionFailRelationChange = -5;

			// Token: 0x0400086A RID: 2154
			private const int AlternativeSolutionQuestGiverPowerChange = 10;

			// Token: 0x0400086B RID: 2155
			private const int AlternativeSolutionRivalGangLeaderPowerChange = -10;

			// Token: 0x0400086C RID: 2156
			private const int AlternativeSolutionFailQuestGiverPowerChange = -10;

			// Token: 0x0400086D RID: 2157
			private const int AlternativeSolutionFailSecurityChange = -10;

			// Token: 0x0400086E RID: 2158
			private const int AlternativeSolutionRivalGangLeaderRelationChange = -5;

			// Token: 0x0400086F RID: 2159
			private const int AlternativeSolutionMinimumTroopTier = 2;

			// Token: 0x04000870 RID: 2160
			private const int IssueDuration = 15;

			// Token: 0x04000871 RID: 2161
			private const int MinimumRequiredMenCount = 5;

			// Token: 0x04000872 RID: 2162
			private const int IssueQuestDuration = 8;

			// Token: 0x04000873 RID: 2163
			private const int MeleeSkillValueThreshold = 150;

			// Token: 0x04000874 RID: 2164
			private const int RoguerySkillValueThreshold = 120;

			// Token: 0x04000875 RID: 2165
			private const int PreparationDurationInDays = 2;
		}

		// Token: 0x020001CE RID: 462
		public class RivalGangMovingInIssueQuest : QuestBase
		{
			// Token: 0x170001C7 RID: 455
			// (get) Token: 0x06001190 RID: 4496 RVA: 0x00070E1C File Offset: 0x0006F01C
			private TextObject OnQuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=dav5rmDd}{QUEST_GIVER.LINK}, a gang leader from {SETTLEMENT} has told you about a rival that is trying to get a foothold in {?QUEST_GIVER.GENDER}her{?}his{\\?} town. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to wait {DAY_COUNT} days so that the other gang lets its guard down.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("DAY_COUNT", 2);
					return textObject;
				}
			}

			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x06001191 RID: 4497 RVA: 0x00070E74 File Offset: 0x0006F074
			private TextObject OnQuestFailedWithRejectionLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=aXMg9M7t}You didn't respond to the messenger {QUEST_GIVER.LINK} sent you. {?QUEST_GIVER.GENDER}She{?}He{\\?} will certainly lose to the rival gang without your help.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x06001192 RID: 4498 RVA: 0x00070EA8 File Offset: 0x0006F0A8
			private TextObject OnQuestFailedWithBetrayalLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=Rf0QqRIX}You have chosen to side with the rival gang leader, {RIVAL_GANG_LEADER.LINK}. {QUEST_GIVER.LINK} must be furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("RIVAL_GANG_LEADER", this._rivalGangLeader.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001CA RID: 458
			// (get) Token: 0x06001193 RID: 4499 RVA: 0x00070EF4 File Offset: 0x0006F0F4
			private TextObject OnQuestFailedWithDefeatLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=du3dpMaV}You were unable to defeat {RIVAL_GANG_LEADER.LINK}'s gang, and thus failed to fulfill your commitment to {QUEST_GIVER.LINK}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("RIVAL_GANG_LEADER", this._rivalGangLeader.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001CB RID: 459
			// (get) Token: 0x06001194 RID: 4500 RVA: 0x00070F40 File Offset: 0x0006F140
			private TextObject OnQuestSucceededLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vpUl7xcy}You have defeated the rival gang and protected the interests of {QUEST_GIVER.LINK} in {SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x06001195 RID: 4501 RVA: 0x00070F8C File Offset: 0x0006F18C
			private TextObject OnQuestPreperationsCompletedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=OIBiRTRP}{QUEST_GIVER.LINK} is waiting for you at {SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001CD RID: 461
			// (get) Token: 0x06001196 RID: 4502 RVA: 0x00070FD8 File Offset: 0x0006F1D8
			private TextObject OnQuestCancelledDueToWarLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vaUlAZba}Your clan is now at war with {QUEST_GIVER.LINK}. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x06001197 RID: 4503 RVA: 0x0007100C File Offset: 0x0006F20C
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x06001198 RID: 4504 RVA: 0x00071040 File Offset: 0x0006F240
			private TextObject OnQuestCancelledDueToSiegeLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=s1GWSE9Y}{QUEST_GIVER.LINK} cancels your plans due to the siege of {SETTLEMENT}. {?QUEST_GIVER.GENDER}She{?}He{\\?} has worse troubles than {?QUEST_GIVER.GENDER}her{?}his{\\?} quarrel with the rival gang.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001D0 RID: 464
			// (get) Token: 0x06001199 RID: 4505 RVA: 0x0007108C File Offset: 0x0006F28C
			private TextObject PlayerStartedAlleyFightWithRivalGangLeader
			{
				get
				{
					TextObject textObject = new TextObject("{=OeKgpuAv}After your attack on the rival gang's alley, {QUEST_GIVER.LINK} decided to change {?QUEST_GIVER.GENDER}her{?}his{\\?} plans, and doesn't need your assistance anymore. Quest is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D1 RID: 465
			// (get) Token: 0x0600119A RID: 4506 RVA: 0x000710C0 File Offset: 0x0006F2C0
			private TextObject PlayerStartedAlleyFightWithQuestgiver
			{
				get
				{
					TextObject textObject = new TextObject("{=VPGkIqlh}Your attack on {QUEST_GIVER.LINK}'s gang has angered {?QUEST_GIVER.GENDER}her{?}him{\\?} and {?QUEST_GIVER.GENDER}she{?}he{\\?} broke off the agreement that you had.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D2 RID: 466
			// (get) Token: 0x0600119B RID: 4507 RVA: 0x000710F4 File Offset: 0x0006F2F4
			private TextObject OwnerOfQuestSettlementIsPlayerClanLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=KxEnNEoD}Your clan is now owner of the settlement. As the {?PLAYER.GENDER}lady{?}lord{\\?} of the settlement you cannot get involved in gang wars anymore. Your agreement with the {QUEST_GIVER.LINK} has canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x0600119C RID: 4508 RVA: 0x00071138 File Offset: 0x0006F338
			public RivalGangMovingInIssueQuest(string questId, Hero questGiver, Hero rivalGangLeader, int duration, int rewardGold, float issueDifficulty)
				: base(questId, questGiver, CampaignTime.DaysFromNow((float)duration), rewardGold)
			{
				this._rivalGangLeader = rivalGangLeader;
				this._rewardGold = rewardGold;
				this._issueDifficulty = issueDifficulty;
				this._timeoutDurationInDays = (float)duration;
				this._preparationCompletionTime = CampaignTime.DaysFromNow(2f);
				this._questTimeoutTime = CampaignTime.DaysFromNow(this._timeoutDurationInDays);
				this._sentTroops = new List<CharacterObject>();
				this._allPlayerTroops = new List<TroopRosterElement>();
				this.InitializeQuestSettlement();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x170001D3 RID: 467
			// (get) Token: 0x0600119D RID: 4509 RVA: 0x000711C0 File Offset: 0x0006F3C0
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=vAjgn7yx}Rival Gang Moving in at {SETTLEMENT}", null);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001D4 RID: 468
			// (get) Token: 0x0600119E RID: 4510 RVA: 0x000711E4 File Offset: 0x0006F3E4
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600119F RID: 4511 RVA: 0x000711E8 File Offset: 0x0006F3E8
			protected override void InitializeQuestOnGameLoad()
			{
				this.InitializeQuestSettlement();
				this.SetDialogs();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRivalGangLeaderDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetQuestGiverPreparationCompletedDialogFlow(), this);
				MobileParty rivalGangLeaderParty = this._rivalGangLeaderParty;
				if (rivalGangLeaderParty != null)
				{
					rivalGangLeaderParty.SetPartyUsedByQuest(true);
				}
				this._sentTroops = new List<CharacterObject>();
				this._allPlayerTroops = new List<TroopRosterElement>();
			}

			// Token: 0x060011A0 RID: 4512 RVA: 0x00071255 File Offset: 0x0006F455
			private void InitializeQuestSettlement()
			{
				this._questSettlement = base.QuestGiver.CurrentSettlement;
			}

			// Token: 0x060011A1 RID: 4513 RVA: 0x00071268 File Offset: 0x0006F468
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine("{=Fwm0PwVb}Great. As I said we need minimum of {NUMBER} days,[ib:normal][if:convo_mocking_revenge] so they'll let their guard down. I will let you know when it's time. Remember, we wait for the dark of the night to strike.", null, null, null, null).Condition(delegate
				{
					MBTextManager.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName, false);
					MBTextManager.SetTextVariable("NUMBER", 2);
					return Hero.OneToOneConversationHero == base.QuestGiver;
				})
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.OnQuestAccepted))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine("{=z43j3Tzq}I'm still gathering my men for the fight. I'll send a runner for you when the time comes.", null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
					return Hero.OneToOneConversationHero == base.QuestGiver && !this._isFinalStage && !this._preparationsComplete;
				})
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=4IHRAmnA}All right. I am waiting for your runner.", null, null, null)
					.NpcLine("{=xEs830bT}You'll know right away once the preparations are complete.[ib:closed][if:convo_mocking_teasing] Just don't leave town.", null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=6g8qvD2M}I can't just hang on here forever. Be quick about it.", null, null, null)
					.NpcLine("{=lM7AscLo}I'm getting this together as quickly as I can.[ib:closed][if:convo_nervous]", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060011A2 RID: 4514 RVA: 0x00071340 File Offset: 0x0006F540
			private DialogFlow GetRivalGangLeaderDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=IfeN8lYd}Coming to fight us, eh? Did {QUEST_GIVER.LINK} put you up to this?[ib:aggressive2][if:convo_confused_annoyed] Look, there's no need for bloodshed. This town is big enough for all of us. But... if bloodshed is what you want, we will be happy to provide.", null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
					return Hero.OneToOneConversationHero == this._rivalGangLeaderHenchmanHero && this._isReadyToBeFinalized;
				})
					.NpcLine("{=WSJxl2Hu}What I want to say is... [if:convo_mocking_teasing]You don't need to be a part of this. My boss will double whatever {?QUEST_GIVER.GENDER}she{?}he{\\?} is paying you if you join us.", null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=GPBja02V}I gave my word to {QUEST_GIVER.LINK}, and I won't be bought.", null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							CombatMissionWithDialogueController missionBehavior = Mission.Current.GetMissionBehavior<CombatMissionWithDialogueController>();
							if (missionBehavior == null)
							{
								return;
							}
							missionBehavior.StartFight(false);
						};
					})
					.NpcLine("{=OSgBicif}You will regret this![ib:warrior][if:convo_furious]", null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=RB4uQpPV}You're going to pay me a lot then, {REWARD}{GOLD_ICON} to be exact. But at that price, I agree.", null, null, null)
					.Condition(delegate
					{
						MBTextManager.SetTextVariable("REWARD", this._rewardGold * 2);
						return true;
					})
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							this._hasBetrayedQuestGiver = true;
							CombatMissionWithDialogueController missionBehavior2 = Mission.Current.GetMissionBehavior<CombatMissionWithDialogueController>();
							if (missionBehavior2 == null)
							{
								return;
							}
							missionBehavior2.StartFight(true);
						};
					})
					.NpcLine("{=5jW4FVDc}Welcome to our ranks then. [ib:warrior][if:convo_evil_smile]Let's kill those bastards!", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060011A3 RID: 4515 RVA: 0x00071420 File Offset: 0x0006F620
			private DialogFlow GetQuestGiverPreparationCompletedDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).BeginNpcOptions(null, false).NpcOption(new TextObject("{=hM7LSuB1}Good to see you. But we still need to wait until after dusk. {HERO.LINK}'s men may be watching, so let's keep our distance from each other until night falls.", null), delegate
				{
					StringHelpers.SetCharacterProperties("HERO", this._rivalGangLeader.CharacterObject, null, false);
					return Hero.OneToOneConversationHero == base.QuestGiver && !this._isFinalStage && this._preparationCompletionTime.IsPast && (!this._preparationsComplete || !CampaignTime.Now.IsNightTime);
				}, null, null, null, null)
					.CloseDialog()
					.NpcOption("{=JxNlB547}Are you ready for the fight?[ib:normal][if:convo_undecided_open]", () => Hero.OneToOneConversationHero == base.QuestGiver && this._preparationsComplete && !this._isFinalStage && CampaignTime.Now.IsNightTime, null, null, null, null)
					.EndNpcOptions()
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=NzMX0s21}I am ready.", null, null, null)
					.Condition(() => !Hero.MainHero.IsWounded)
					.NpcLine("{=dNjepcKu}Let's finish this![ib:hip][if:convo_mocking_revenge]", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.rival_gang_start_fight_on_consequence;
					})
					.CloseDialog()
					.PlayerOption("{=B2Donbwz}I need more time.", null, null, null)
					.Condition(() => !Hero.MainHero.IsWounded)
					.NpcLine("{=advPT3WY}You'd better hurry up![ib:closed][if:convo_astonished]", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.rival_gang_need_more_time_on_consequence;
					})
					.CloseDialog()
					.PlayerOption("{=QaN26CZ5}My wounds are still fresh. I need some time to recover.", null, null, null)
					.Condition(() => Hero.MainHero.IsWounded)
					.NpcLine("{=s0jKaYo0}We must attack before the rival gang hears about our plan. You'd better hurry up![if:convo_astonished]", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060011A4 RID: 4516 RVA: 0x00071583 File Offset: 0x0006F783
			public override void OnHeroCanDieInfoIsRequested(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
			{
				if (hero == base.QuestGiver || hero == this._rivalGangLeader)
				{
					result = false;
				}
			}

			// Token: 0x060011A5 RID: 4517 RVA: 0x0007159A File Offset: 0x0006F79A
			private void rival_gang_start_fight_on_consequence()
			{
				this._isFinalStage = true;
				if (Mission.Current != null)
				{
					Mission.Current.EndMission();
				}
				Campaign.Current.GameMenuManager.SetNextMenu("rival_gang_quest_before_fight");
			}

			// Token: 0x060011A6 RID: 4518 RVA: 0x000715C8 File Offset: 0x0006F7C8
			private void rival_gang_need_more_time_on_consequence()
			{
				if (Campaign.Current.CurrentMenuContext.GameMenu.StringId == "rival_gang_quest_wait_duration_is_over")
				{
					Campaign.Current.GameMenuManager.SetNextMenu("town_wait_menus");
				}
			}

			// Token: 0x060011A7 RID: 4519 RVA: 0x00071600 File Offset: 0x0006F800
			private void AddQuestGiverGangLeaderOnSuccessDialogFlow()
			{
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=zNPzh5jO}Ah! Now that was as good a fight as any I've had. Here, take this purse, It is all yours as {QUEST_GIVER.LINK} has promised.[ib:hip2][if:convo_huge_smile]", null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
					return base.IsOngoing && Hero.OneToOneConversationHero == this._allyGangLeaderHenchmanHero;
				})
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.OnQuestSucceeded;
					})
					.CloseDialog(), null);
			}

			// Token: 0x060011A8 RID: 4520 RVA: 0x00071660 File Offset: 0x0006F860
			private CharacterObject GetTroopTypeTemplateForDifficulty()
			{
				int difficultyRange = MBMath.ClampInt(MathF.Ceiling(this._issueDifficulty / 0.1f), 1, 10);
				CharacterObject characterObject;
				if (difficultyRange == 1)
				{
					characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "looter");
				}
				else if (difficultyRange == 10)
				{
					characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "mercenary_8");
				}
				else
				{
					characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "mercenary_" + (difficultyRange - 1));
				}
				if (characterObject == null)
				{
					Debug.FailedAssert("Can't find troop in rival gang leader quest", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Issues\\RivalGangMovingInIssueBehavior.cs", "GetTroopTypeTemplateForDifficulty", 806);
					characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject t) => t.IsBasicTroop && t.IsSoldier);
				}
				return characterObject;
			}

			// Token: 0x060011A9 RID: 4521 RVA: 0x0007175C File Offset: 0x0006F95C
			internal void StartAlleyBattle()
			{
				this.CreateRivalGangLeaderParty();
				this.CreateAllyGangLeaderParty();
				this.PreparePlayerParty();
				PlayerEncounter.RestartPlayerEncounter(this._rivalGangLeaderParty.Party, PartyBase.MainParty, false, false);
				PlayerEncounter.StartBattle();
				this._allyGangLeaderParty.MapEventSide = PlayerEncounter.Battle.GetMapEventSide(PlayerEncounter.Battle.PlayerSide);
				GameMenu.ActivateGameMenu("rival_gang_quest_after_fight");
				this._isReadyToBeFinalized = true;
				PlayerEncounter.StartCombatMissionWithDialogueInTownCenter(this._rivalGangLeaderHenchmanHero.CharacterObject);
			}

			// Token: 0x060011AA RID: 4522 RVA: 0x000717D8 File Offset: 0x0006F9D8
			private void CreateRivalGangLeaderParty()
			{
				TextObject textObject = new TextObject("{=u4jhIFwG}{GANG_LEADER}'s Party", null);
				textObject.SetTextVariable("RIVAL_GANG_LEADER", this._rivalGangLeader.Name);
				textObject.SetTextVariable("GANG_LEADER", this._rivalGangLeader.Name);
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefaultQ<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				this._rivalGangLeaderParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._questSettlement.GatePosition, 1f, this._questSettlement, textObject, clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), null, "", "", 0f, false);
				this._rivalGangLeaderParty.SetPartyUsedByQuest(true);
				CharacterObject troopTypeTemplateForDifficulty = this.GetTroopTypeTemplateForDifficulty();
				this._rivalGangLeaderParty.MemberRoster.AddToCounts(troopTypeTemplateForDifficulty, 15, false, 0, 0, true, -1);
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_3");
				this._rivalGangLeaderHenchmanHero = HeroCreator.CreateSpecialHero(@object, null, null, null, -1);
				TextObject textObject2 = new TextObject("{=zJqEdDiq}Henchman of {GANG_LEADER}", null);
				textObject2.SetTextVariable("GANG_LEADER", this._rivalGangLeader.Name);
				this._rivalGangLeaderHenchmanHero.SetName(textObject2, textObject2);
				this._rivalGangLeaderHenchmanHero.HiddenInEncyclopedia = true;
				this._rivalGangLeaderHenchmanHero.Culture = this._rivalGangLeader.Culture;
				this._rivalGangLeaderHenchmanHero.SetNewOccupation(Occupation.Special);
				this._rivalGangLeaderHenchmanHero.ChangeState(Hero.CharacterStates.Active);
				this._rivalGangLeaderParty.MemberRoster.AddToCounts(this._rivalGangLeaderHenchmanHero.CharacterObject, 1, false, 0, 0, true, -1);
				this._rivalGangLeaderParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
				EnterSettlementAction.ApplyForParty(this._rivalGangLeaderParty, this._questSettlement);
			}

			// Token: 0x060011AB RID: 4523 RVA: 0x000719AC File Offset: 0x0006FBAC
			private void CreateAllyGangLeaderParty()
			{
				TextObject textObject = new TextObject("{=u4jhIFwG}{GANG_LEADER}'s Party", null);
				textObject.SetTextVariable("GANG_LEADER", base.QuestGiver.Name);
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefaultQ<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				this._allyGangLeaderParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._questSettlement.GatePosition, 1f, this._questSettlement, textObject, clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), null, "", "", 0f, false);
				this._allyGangLeaderParty.SetPartyUsedByQuest(true);
				CharacterObject troopTypeTemplateForDifficulty = this.GetTroopTypeTemplateForDifficulty();
				this._allyGangLeaderParty.MemberRoster.AddToCounts(troopTypeTemplateForDifficulty, 20, false, 0, 0, true, -1);
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_2");
				this._allyGangLeaderHenchmanHero = HeroCreator.CreateSpecialHero(@object, null, null, null, -1);
				TextObject textObject2 = new TextObject("{=zJqEdDiq}Henchman of {GANG_LEADER}", null);
				textObject2.SetTextVariable("GANG_LEADER", base.QuestGiver.Name);
				this._allyGangLeaderHenchmanHero.SetName(textObject2, textObject2);
				this._allyGangLeaderHenchmanHero.HiddenInEncyclopedia = true;
				this._allyGangLeaderHenchmanHero.Culture = base.QuestGiver.Culture;
				this._allyGangLeaderHenchmanHero.ChangeState(Hero.CharacterStates.Active);
				this._allyGangLeaderParty.MemberRoster.AddToCounts(this._allyGangLeaderHenchmanHero.CharacterObject, 1, false, 0, 0, true, -1);
				this._allyGangLeaderParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
				EnterSettlementAction.ApplyForParty(this._allyGangLeaderParty, this._questSettlement);
			}

			// Token: 0x060011AC RID: 4524 RVA: 0x00071B5C File Offset: 0x0006FD5C
			private void PreparePlayerParty()
			{
				this._allPlayerTroops.Clear();
				foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.MemberRoster.GetTroopRoster())
				{
					if (!troopRosterElement.Character.IsPlayerCharacter)
					{
						this._allPlayerTroops.Add(troopRosterElement);
					}
				}
				this._partyEngineer = MobileParty.MainParty.GetRoleHolder(PartyRole.Engineer);
				this._partyScout = MobileParty.MainParty.GetRoleHolder(PartyRole.Scout);
				this._partyQuartermaster = MobileParty.MainParty.GetRoleHolder(PartyRole.Quartermaster);
				this._partySurgeon = MobileParty.MainParty.GetRoleHolder(PartyRole.Surgeon);
				PartyBase.MainParty.MemberRoster.RemoveIf((TroopRosterElement t) => !t.Character.IsPlayerCharacter);
				if (!this._allPlayerTroops.IsEmpty<TroopRosterElement>())
				{
					this._sentTroops.Clear();
					int num = 5;
					foreach (TroopRosterElement troopRosterElement2 in this._allPlayerTroops.OrderByDescending<TroopRosterElement, int>((TroopRosterElement t) => t.Character.Level))
					{
						if (num <= 0)
						{
							break;
						}
						int num2 = 0;
						while (num2 < troopRosterElement2.Number - troopRosterElement2.WoundedNumber && num > 0)
						{
							this._sentTroops.Add(troopRosterElement2.Character);
							num--;
							num2++;
						}
					}
					foreach (CharacterObject characterObject in this._sentTroops)
					{
						PartyBase.MainParty.MemberRoster.AddToCounts(characterObject, 1, false, 0, 0, true, -1);
					}
				}
			}

			// Token: 0x060011AD RID: 4525 RVA: 0x00071D54 File Offset: 0x0006FF54
			internal void HandlePlayerEncounterResult(bool hasPlayerWon)
			{
				PlayerEncounter.Finish(false);
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, this._questSettlement);
				TroopRoster troopRoster = PartyBase.MainParty.MemberRoster.CloneRosterData();
				PartyBase.MainParty.MemberRoster.RemoveIf((TroopRosterElement t) => !t.Character.IsPlayerCharacter);
				using (List<TroopRosterElement>.Enumerator enumerator = this._allPlayerTroops.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TroopRosterElement playerTroop = enumerator.Current;
						int num = troopRoster.FindIndexOfTroop(playerTroop.Character);
						int num2 = playerTroop.Number;
						int num3 = playerTroop.WoundedNumber;
						int num4 = playerTroop.Xp;
						if (num >= 0)
						{
							TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(num);
							num2 -= this._sentTroops.Count<CharacterObject>((CharacterObject t) => t == playerTroop.Character) - elementCopyAtIndex.Number;
							num3 += elementCopyAtIndex.WoundedNumber;
							num4 += elementCopyAtIndex.Xp;
						}
						else if (this._sentTroops.Contains(playerTroop.Character))
						{
							num2 -= this._sentTroops.Count<CharacterObject>((CharacterObject t) => t == playerTroop.Character);
						}
						PartyBase.MainParty.MemberRoster.AddToCounts(playerTroop.Character, num2, false, num3, num4, true, -1);
					}
				}
				MobileParty.MainParty.SetPartyEngineer(this._partyEngineer);
				MobileParty.MainParty.SetPartyScout(this._partyScout);
				MobileParty.MainParty.SetPartyQuartermaster(this._partyQuartermaster);
				MobileParty.MainParty.SetPartySurgeon(this._partySurgeon);
				if (this._rivalGangLeader.PartyBelongedTo == this._rivalGangLeaderParty)
				{
					this._rivalGangLeaderParty.MemberRoster.AddToCounts(this._rivalGangLeader.CharacterObject, -1, false, 0, 0, true, -1);
				}
				if (hasPlayerWon)
				{
					if (!this._hasBetrayedQuestGiver)
					{
						this.AddQuestGiverGangLeaderOnSuccessDialogFlow();
						this.SpawnAllyHenchmanAfterMissionSuccess();
						PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationOfCharacter(this._allyGangLeaderHenchmanHero), null, this._allyGangLeaderHenchmanHero.CharacterObject, null);
						return;
					}
					this.OnBattleWonWithBetrayal();
					return;
				}
				else
				{
					if (!this._hasBetrayedQuestGiver)
					{
						this.OnQuestFailedWithDefeat();
						return;
					}
					this.OnBattleLostWithBetrayal();
					return;
				}
			}

			// Token: 0x060011AE RID: 4526 RVA: 0x00071FC0 File Offset: 0x000701C0
			protected override void RegisterEvents()
			{
				CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
				CampaignEvents.AlleyClearedByPlayer.AddNonSerializedListener(this, new Action<Alley>(this.OnAlleyClearedByPlayer));
				CampaignEvents.AlleyOccupiedByPlayer.AddNonSerializedListener(this, new Action<Alley, TroopRoster>(this.OnAlleyOccupiedByPlayer));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x060011AF RID: 4527 RVA: 0x00072070 File Offset: 0x00070270
			private void SpawnAllyHenchmanAfterMissionSuccess()
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(this._allyGangLeaderHenchmanHero.CharacterObject.Race, "_settlement");
				LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(this._allyGangLeaderHenchmanHero.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true, null, false);
				LocationComplex.Current.GetLocationWithId("center").AddCharacter(locationCharacter);
			}

			// Token: 0x060011B0 RID: 4528 RVA: 0x00072100 File Offset: 0x00070300
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == base.QuestGiver.CurrentSettlement && newOwner == Hero.MainHero)
				{
					base.AddLog(this.OwnerOfQuestSettlementIsPlayerClanLogText, false);
					base.QuestGiver.AddPower(-10f);
					ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x060011B1 RID: 4529 RVA: 0x00072157 File Offset: 0x00070357
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._rivalGangLeader)
				{
					result = false;
				}
			}

			// Token: 0x060011B2 RID: 4530 RVA: 0x00072165 File Offset: 0x00070365
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.OnQuestCancelledDueToWarLogText);
				}
			}

			// Token: 0x060011B3 RID: 4531 RVA: 0x00072194 File Offset: 0x00070394
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.OnQuestCancelledDueToWarLogText, false);
			}

			// Token: 0x060011B4 RID: 4532 RVA: 0x000721AC File Offset: 0x000703AC
			private void OnSiegeEventStarted(SiegeEvent siegeEvent)
			{
				if (siegeEvent.BesiegedSettlement == this._questSettlement)
				{
					base.AddLog(this.OnQuestCancelledDueToSiegeLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x060011B5 RID: 4533 RVA: 0x000721D4 File Offset: 0x000703D4
			protected override void HourlyTick()
			{
				if (RivalGangMovingInIssueBehavior.Instance != null && RivalGangMovingInIssueBehavior.Instance.IsOngoing && (2f - RivalGangMovingInIssueBehavior.Instance._preparationCompletionTime.RemainingDaysFromNow) / 2f >= 1f && !this._preparationsComplete && CampaignTime.Now.IsNightTime)
				{
					this.OnGuestGiverPreparationsCompleted();
				}
			}

			// Token: 0x060011B6 RID: 4534 RVA: 0x00072238 File Offset: 0x00070438
			private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
			{
				if (victim == this._rivalGangLeader)
				{
					TextObject textObject = ((detail == KillCharacterAction.KillCharacterActionDetail.Lost) ? this.TargetHeroDisappearedLogText : this.TargetHeroDiedLogText);
					StringHelpers.SetCharacterProperties("QUEST_TARGET", this._rivalGangLeader.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					base.AddLog(textObject, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x060011B7 RID: 4535 RVA: 0x000722A1 File Offset: 0x000704A1
			private void OnPlayerAlleyFightEnd(Alley alley)
			{
				if (!this._isReadyToBeFinalized)
				{
					if (alley.Owner == this._rivalGangLeader)
					{
						this.OnPlayerAttackedRivalGangAlley();
						return;
					}
					if (alley.Owner == base.QuestGiver)
					{
						this.OnPlayerAttackedQuestGiverAlley();
					}
				}
			}

			// Token: 0x060011B8 RID: 4536 RVA: 0x000722D4 File Offset: 0x000704D4
			private void OnAlleyClearedByPlayer(Alley alley)
			{
				this.OnPlayerAlleyFightEnd(alley);
			}

			// Token: 0x060011B9 RID: 4537 RVA: 0x000722DD File Offset: 0x000704DD
			private void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
			{
				this.OnPlayerAlleyFightEnd(alley);
			}

			// Token: 0x060011BA RID: 4538 RVA: 0x000722E6 File Offset: 0x000704E6
			private void OnPlayerAttackedRivalGangAlley()
			{
				base.AddLog(this.PlayerStartedAlleyFightWithRivalGangLeader, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x060011BB RID: 4539 RVA: 0x00072300 File Offset: 0x00070500
			private void OnPlayerAttackedQuestGiverAlley()
			{
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -150)
				});
				base.QuestGiver.AddPower(-10f);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -8, true, true);
				this._questSettlement.Town.Security += -10f;
				base.AddLog(this.PlayerStartedAlleyFightWithQuestgiver, false);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060011BC RID: 4540 RVA: 0x00072380 File Offset: 0x00070580
			protected override void OnTimedOut()
			{
				this.OnQuestFailedWithRejectionOrTimeout();
			}

			// Token: 0x060011BD RID: 4541 RVA: 0x00072388 File Offset: 0x00070588
			private void OnGuestGiverPreparationsCompleted()
			{
				this._preparationsComplete = true;
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == this._questSettlement && Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu.StringId == "town_wait_menus")
				{
					Campaign.Current.CurrentMenuContext.SwitchToMenu("rival_gang_quest_wait_duration_is_over");
				}
				TextObject textObject = new TextObject("{=DUKbtlNb}{QUEST_GIVER.LINK} has finally sent a messenger telling you it's time to meet {?QUEST_GIVER.GENDER}her{?}him{\\?} and join the fight.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				base.AddLog(this.OnQuestPreperationsCompletedLogText, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}

			// Token: 0x060011BE RID: 4542 RVA: 0x00072430 File Offset: 0x00070630
			private void OnQuestAccepted()
			{
				base.StartQuest();
				this._onQuestStartedLog = base.AddLog(this.OnQuestStartedLogText, false);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRivalGangLeaderDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetQuestGiverPreparationCompletedDialogFlow(), this);
			}

			// Token: 0x060011BF RID: 4543 RVA: 0x00072484 File Offset: 0x00070684
			private void OnQuestSucceeded()
			{
				this._onQuestSucceededLog = base.AddLog(this.OnQuestSucceededLogText, false);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				base.QuestGiver.AddPower(10f);
				this._rivalGangLeader.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = 5;
				ChangeRelationAction.ApplyPlayerRelation(this._rivalGangLeader, -5, true, true);
				GameMenu.ExitToLast();
				GameMenu.ActivateGameMenu("town");
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060011C0 RID: 4544 RVA: 0x00072531 File Offset: 0x00070731
			private void OnQuestFailedWithRejectionOrTimeout()
			{
				base.AddLog(this.OnQuestFailedWithRejectionLogText, false);
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				this.ApplyQuestFailConsequences();
			}

			// Token: 0x060011C1 RID: 4545 RVA: 0x00072570 File Offset: 0x00070770
			private void OnBattleWonWithBetrayal()
			{
				base.AddLog(this.OnQuestFailedWithBetrayalLogText, false);
				this.RelationshipChangeWithQuestGiver = -15;
				if (!this._rivalGangLeader.IsDead)
				{
					ChangeRelationAction.ApplyPlayerRelation(this._rivalGangLeader, 5, true, true);
				}
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold * 2, false);
				TraitLevelingHelper.OnIssueSolvedThroughBetrayal(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				this._rivalGangLeader.AddPower(10f);
				GameMenu.SwitchToMenu("town");
				this.ApplyQuestFailConsequences();
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x060011C2 RID: 4546 RVA: 0x0007260C File Offset: 0x0007080C
			private void OnBattleLostWithBetrayal()
			{
				base.AddLog(this.OnQuestFailedWithBetrayalLogText, false);
				this.RelationshipChangeWithQuestGiver = -10;
				if (!this._rivalGangLeader.IsDead)
				{
					ChangeRelationAction.ApplyPlayerRelation(this._rivalGangLeader, -5, true, true);
				}
				this._rivalGangLeader.AddPower(-10f);
				TraitLevelingHelper.OnIssueSolvedThroughBetrayal(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				GameMenu.SwitchToMenu("town");
				this.ApplyQuestFailConsequences();
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x060011C3 RID: 4547 RVA: 0x00072692 File Offset: 0x00070892
			private void OnQuestFailedWithDefeat()
			{
				this.RelationshipChangeWithQuestGiver = -5;
				GameMenu.SwitchToMenu("town");
				base.AddLog(this.OnQuestFailedWithDefeatLogText, false);
				this.ApplyQuestFailConsequences();
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060011C4 RID: 4548 RVA: 0x000726C4 File Offset: 0x000708C4
			private void ApplyQuestFailConsequences()
			{
				base.QuestGiver.AddPower(-10f);
				this._questSettlement.Town.Security += -10f;
				if (this._rivalGangLeaderParty != null && this._rivalGangLeaderParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._rivalGangLeaderParty);
				}
			}

			// Token: 0x060011C5 RID: 4549 RVA: 0x00072720 File Offset: 0x00070920
			protected override void OnFinalize()
			{
				if (this._rivalGangLeaderParty != null && this._rivalGangLeaderParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._rivalGangLeaderParty);
				}
				if (this._allyGangLeaderParty != null && this._allyGangLeaderParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._allyGangLeaderParty);
				}
				if (this._allyGangLeaderHenchmanHero != null && this._allyGangLeaderHenchmanHero.IsAlive)
				{
					this._allyGangLeaderHenchmanHero.SetNewOccupation(Occupation.Special);
					KillCharacterAction.ApplyByRemove(this._allyGangLeaderHenchmanHero, false, true);
				}
				if (this._rivalGangLeaderHenchmanHero != null && this._rivalGangLeaderHenchmanHero.IsAlive)
				{
					this._rivalGangLeaderHenchmanHero.SetNewOccupation(Occupation.NotAssigned);
					KillCharacterAction.ApplyByRemove(this._rivalGangLeaderHenchmanHero, false, true);
				}
			}

			// Token: 0x060011C6 RID: 4550 RVA: 0x000727CC File Offset: 0x000709CC
			internal static void AutoGeneratedStaticCollectObjectsRivalGangMovingInIssueQuest(object o, List<object> collectedObjects)
			{
				((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060011C7 RID: 4551 RVA: 0x000727DC File Offset: 0x000709DC
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._rivalGangLeader);
				collectedObjects.Add(this._rivalGangLeaderParty);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._preparationCompletionTime, collectedObjects);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._questTimeoutTime, collectedObjects);
			}

			// Token: 0x060011C8 RID: 4552 RVA: 0x0007282A File Offset: 0x00070A2A
			internal static object AutoGeneratedGetMemberValue_rivalGangLeader(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._rivalGangLeader;
			}

			// Token: 0x060011C9 RID: 4553 RVA: 0x00072837 File Offset: 0x00070A37
			internal static object AutoGeneratedGetMemberValue_timeoutDurationInDays(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._timeoutDurationInDays;
			}

			// Token: 0x060011CA RID: 4554 RVA: 0x00072849 File Offset: 0x00070A49
			internal static object AutoGeneratedGetMemberValue_isFinalStage(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._isFinalStage;
			}

			// Token: 0x060011CB RID: 4555 RVA: 0x0007285B File Offset: 0x00070A5B
			internal static object AutoGeneratedGetMemberValue_isReadyToBeFinalized(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._isReadyToBeFinalized;
			}

			// Token: 0x060011CC RID: 4556 RVA: 0x0007286D File Offset: 0x00070A6D
			internal static object AutoGeneratedGetMemberValue_hasBetrayedQuestGiver(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._hasBetrayedQuestGiver;
			}

			// Token: 0x060011CD RID: 4557 RVA: 0x0007287F File Offset: 0x00070A7F
			internal static object AutoGeneratedGetMemberValue_rivalGangLeaderParty(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._rivalGangLeaderParty;
			}

			// Token: 0x060011CE RID: 4558 RVA: 0x0007288C File Offset: 0x00070A8C
			internal static object AutoGeneratedGetMemberValue_preparationCompletionTime(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._preparationCompletionTime;
			}

			// Token: 0x060011CF RID: 4559 RVA: 0x0007289E File Offset: 0x00070A9E
			internal static object AutoGeneratedGetMemberValue_questTimeoutTime(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._questTimeoutTime;
			}

			// Token: 0x060011D0 RID: 4560 RVA: 0x000728B0 File Offset: 0x00070AB0
			internal static object AutoGeneratedGetMemberValue_preparationsComplete(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._preparationsComplete;
			}

			// Token: 0x060011D1 RID: 4561 RVA: 0x000728C2 File Offset: 0x00070AC2
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._rewardGold;
			}

			// Token: 0x060011D2 RID: 4562 RVA: 0x000728D4 File Offset: 0x00070AD4
			internal static object AutoGeneratedGetMemberValue_issueDifficulty(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._issueDifficulty;
			}

			// Token: 0x04000877 RID: 2167
			private const int QuestGiverRelationChangeOnSuccess = 5;

			// Token: 0x04000878 RID: 2168
			private const int RivalGangLeaderRelationChangeOnSuccess = -5;

			// Token: 0x04000879 RID: 2169
			private const int QuestGiverNotablePowerChangeOnSuccess = 10;

			// Token: 0x0400087A RID: 2170
			private const int RivalGangLeaderPowerChangeOnSuccess = -10;

			// Token: 0x0400087B RID: 2171
			private const int RenownChangeOnSuccess = 1;

			// Token: 0x0400087C RID: 2172
			private const int QuestGiverRelationChangeOnFail = -5;

			// Token: 0x0400087D RID: 2173
			private const int QuestGiverRelationChangeOnTimedOut = -5;

			// Token: 0x0400087E RID: 2174
			private const int NotablePowerChangeOnFail = -10;

			// Token: 0x0400087F RID: 2175
			private const int TownSecurityChangeOnFail = -10;

			// Token: 0x04000880 RID: 2176
			private const int RivalGangLeaderRelationChangeOnSuccessfulBetrayal = 5;

			// Token: 0x04000881 RID: 2177
			private const int QuestGiverRelationChangeOnSuccessfulBetrayal = -15;

			// Token: 0x04000882 RID: 2178
			private const int RivalGangLeaderPowerChangeOnSuccessfulBetrayal = 10;

			// Token: 0x04000883 RID: 2179
			private const int QuestGiverRelationChangeOnFailedBetrayal = -10;

			// Token: 0x04000884 RID: 2180
			private const int PlayerAttackedQuestGiverHonorChange = -150;

			// Token: 0x04000885 RID: 2181
			private const int PlayerAttackedQuestGiverPowerChange = -10;

			// Token: 0x04000886 RID: 2182
			private const int NumberOfRegularEnemyTroops = 15;

			// Token: 0x04000887 RID: 2183
			private const int PlayerAttackedQuestGiverRelationChange = -8;

			// Token: 0x04000888 RID: 2184
			private const int PlayerAttackedQuestGiverSecurityChange = -10;

			// Token: 0x04000889 RID: 2185
			private const int NumberOfRegularAllyTroops = 20;

			// Token: 0x0400088A RID: 2186
			private const int MaxNumberOfPlayerOwnedTroops = 5;

			// Token: 0x0400088B RID: 2187
			private const string AllyGangLeaderHenchmanStringId = "gangster_2";

			// Token: 0x0400088C RID: 2188
			private const string RivalGangLeaderHenchmanStringId = "gangster_3";

			// Token: 0x0400088D RID: 2189
			private const int PreparationDurationInDays = 2;

			// Token: 0x0400088E RID: 2190
			[SaveableField(10)]
			internal readonly Hero _rivalGangLeader;

			// Token: 0x0400088F RID: 2191
			[SaveableField(20)]
			private MobileParty _rivalGangLeaderParty;

			// Token: 0x04000890 RID: 2192
			private Hero _rivalGangLeaderHenchmanHero;

			// Token: 0x04000891 RID: 2193
			[SaveableField(30)]
			private readonly CampaignTime _preparationCompletionTime;

			// Token: 0x04000892 RID: 2194
			private Hero _allyGangLeaderHenchmanHero;

			// Token: 0x04000893 RID: 2195
			private MobileParty _allyGangLeaderParty;

			// Token: 0x04000894 RID: 2196
			[SaveableField(40)]
			private readonly CampaignTime _questTimeoutTime;

			// Token: 0x04000895 RID: 2197
			[SaveableField(60)]
			internal readonly float _timeoutDurationInDays;

			// Token: 0x04000896 RID: 2198
			[SaveableField(70)]
			internal bool _isFinalStage;

			// Token: 0x04000897 RID: 2199
			[SaveableField(80)]
			internal bool _isReadyToBeFinalized;

			// Token: 0x04000898 RID: 2200
			[SaveableField(90)]
			internal bool _hasBetrayedQuestGiver;

			// Token: 0x04000899 RID: 2201
			private List<TroopRosterElement> _allPlayerTroops;

			// Token: 0x0400089A RID: 2202
			private List<CharacterObject> _sentTroops;

			// Token: 0x0400089B RID: 2203
			private Hero _partyEngineer;

			// Token: 0x0400089C RID: 2204
			private Hero _partyScout;

			// Token: 0x0400089D RID: 2205
			private Hero _partyQuartermaster;

			// Token: 0x0400089E RID: 2206
			private Hero _partySurgeon;

			// Token: 0x0400089F RID: 2207
			[SaveableField(110)]
			private bool _preparationsComplete;

			// Token: 0x040008A0 RID: 2208
			[SaveableField(120)]
			private int _rewardGold;

			// Token: 0x040008A1 RID: 2209
			[SaveableField(130)]
			private float _issueDifficulty;

			// Token: 0x040008A2 RID: 2210
			private Settlement _questSettlement;

			// Token: 0x040008A3 RID: 2211
			private JournalLog _onQuestStartedLog;

			// Token: 0x040008A4 RID: 2212
			private JournalLog _onQuestSucceededLog;
		}
	}
}
