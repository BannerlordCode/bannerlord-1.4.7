using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000B6 RID: 182
	public class NotableWantsDaughterFoundIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x000336F7 File Offset: 0x000318F7
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00033728 File Offset: 0x00031928
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.2.8.31599", 0)))
			{
				foreach (Hero hero in Hero.DeadOrDisabledHeroes)
				{
					if (hero.IsDead && hero.CompanionOf == Clan.PlayerClan && hero.Father != null && hero.Father.IsNotable && hero.Father.CurrentSettlement.IsVillage)
					{
						RemoveCompanionAction.ApplyByDeath(Clan.PlayerClan, hero);
					}
				}
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000337E0 File Offset: 0x000319E0
		public void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00033844 File Offset: 0x00031A44
		private bool ConditionsHold(Hero issueGiver)
		{
			if (issueGiver.IsRuralNotable && issueGiver.CurrentSettlement.IsVillage && issueGiver.CurrentSettlement.Village.Bound != null && issueGiver.CurrentSettlement.Village.Bound.BoundVillages.Count > 2 && issueGiver.CanHaveCampaignIssues() && issueGiver.Age > (float)(Campaign.Current.Models.AgeModel.HeroComesOfAge * 2) && CharacterHelper.GetRandomCompanionTemplateWithPredicate((CharacterObject x) => x.IsFemale && x.Culture == issueGiver.CurrentSettlement.Culture) != null)
			{
				if (issueGiver.CurrentSettlement.Culture.NotableTemplates.Any<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.GangLeader && !x.IsFemale) && issueGiver.GetTraitLevel(DefaultTraits.Mercy) <= 0)
				{
					return issueGiver.GetTraitLevel(DefaultTraits.Generosity) <= 0;
				}
			}
			return false;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0003396F File Offset: 0x00031B6F
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue(issueOwner);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00033977 File Offset: 0x00031B77
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000411 RID: 1041
		private const IssueBase.IssueFrequency NotableWantsDaughterFoundIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000412 RID: 1042
		private const int IssueDuration = 30;

		// Token: 0x04000413 RID: 1043
		private const int QuestTimeLimit = 19;

		// Token: 0x04000414 RID: 1044
		private const int BaseRewardGold = 500;

		// Token: 0x020001C1 RID: 449
		public class NotableWantsDaughterFoundIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600103C RID: 4156 RVA: 0x0006B01A File Offset: 0x0006921A
			public NotableWantsDaughterFoundIssueTypeDefiner()
				: base(1088000)
			{
			}

			// Token: 0x0600103D RID: 4157 RVA: 0x0006B027 File Offset: 0x00069227
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue), 1, null);
				base.AddClassDefinition(typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest), 2, null);
			}
		}

		// Token: 0x020001C2 RID: 450
		public class NotableWantsDaughterFoundIssue : IssueBase
		{
			// Token: 0x17000170 RID: 368
			// (get) Token: 0x0600103E RID: 4158 RVA: 0x0006B04D File Offset: 0x0006924D
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x0600103F RID: 4159 RVA: 0x0006B050 File Offset: 0x00069250
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000172 RID: 370
			// (get) Token: 0x06001040 RID: 4160 RVA: 0x0006B053 File Offset: 0x00069253
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x06001041 RID: 4161 RVA: 0x0006B056 File Offset: 0x00069256
			protected override int RewardGold
			{
				get
				{
					return 500 + MathF.Round(1200f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x06001042 RID: 4162 RVA: 0x0006B06F File Offset: 0x0006926F
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 2 + MathF.Ceiling(4f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x06001043 RID: 4163 RVA: 0x0006B084 File Offset: 0x00069284
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 4 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06001044 RID: 4164 RVA: 0x0006B099 File Offset: 0x00069299
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(500f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06001045 RID: 4165 RVA: 0x0006B0AE File Offset: 0x000692AE
			public NotableWantsDaughterFoundIssue(Hero issueOwner)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
			}

			// Token: 0x06001046 RID: 4166 RVA: 0x0006B0C1 File Offset: 0x000692C1
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06001047 RID: 4167 RVA: 0x0006B0D8 File Offset: 0x000692D8
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=x9VgLEzi}Yes... I've suffered a great misfortune. [ib:demure][if:convo_shocked]My daughter, a headstrong girl, has been bewitched by this never-do-well. I told her to stop seeing him but she wouldn't listen! Now she's missing - I'm sure she's been abducted by him! I'm offering a bounty of {BASE_REWARD_GOLD}{GOLD_ICON} to anyone who brings her back. Please {?PLAYER.GENDER}ma'am{?}sir{\\?}! Don't let a father's heart be broken.", null);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06001048 RID: 4168 RVA: 0x0006B127 File Offset: 0x00069327
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=35w6g8gM}Tell me more. What's wrong with the man? ", null);
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x06001049 RID: 4169 RVA: 0x0006B134 File Offset: 0x00069334
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=IY5b9vZV}Everything is wrong. [if:convo_annoyed]He is from a low family, the kind who is always involved in some land fraud scheme, or seen dealing with known bandits. Every village has a black sheep like that but I never imagined he would get his hooks into my daughter!", null);
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x0600104A RID: 4170 RVA: 0x0006B141 File Offset: 0x00069341
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=v0XsM7Zz}If you send your best tracker with a few men, I am sure they will find my girl [if:convo_pondering]and be back to you in no more than {ALTERNATIVE_SOLUTION_WAIT_DAYS} days.", null);
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_WAIT_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x0600104B RID: 4171 RVA: 0x0006B160 File Offset: 0x00069360
			public override TextObject IssuePlayerResponseAfterAlternativeExplanation
			{
				get
				{
					return new TextObject("{=Ldp6ckgj}Don't worry, either I or one of my companions should be able to find her and see what's going on.", null);
				}
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x0600104C RID: 4172 RVA: 0x0006B16D File Offset: 0x0006936D
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=uYrxCtDa}I should be able to find her and see what's going on.", null);
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x0600104D RID: 4173 RVA: 0x0006B17A File Offset: 0x0006937A
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=WSrGHkal}I will have one of my trackers and {REQUIRED_TROOP_AMOUNT} of my men to find your daughter.", null);
					textObject.SetTextVariable("REQUIRED_TROOP_AMOUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x1700017E RID: 382
			// (get) Token: 0x0600104E RID: 4174 RVA: 0x0006B199 File Offset: 0x00069399
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=mBPcZddA}{?PLAYER.GENDER}Madam{?}Sir{\\?}, we are still waiting [ib:demure][if:convo_undecided_open]for your men to bring my daughter back. I pray for their success.", null);
				}
			}

			// Token: 0x1700017F RID: 383
			// (get) Token: 0x0600104F RID: 4175 RVA: 0x0006B1A8 File Offset: 0x000693A8
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=Hhd3KaKu}Thank you, my {?PLAYER.GENDER}lady{?}lord{\\?}. If your men can find my girl and bring her back to me, I will be so grateful.[if:convo_happy] I will pay you {BASE_REWARD_GOLD}{GOLD_ICON} for your trouble.", null);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000180 RID: 384
			// (get) Token: 0x06001050 RID: 4176 RVA: 0x0006B1F8 File Offset: 0x000693F8
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=6OmbzoBs}{ISSUE_GIVER.LINK}, a merchant from {ISSUE_GIVER_SETTLEMENT}, has told you that {?ISSUE_GIVER.GENDER}her{?}his{\\?} daughter has gone missing. You choose {COMPANION.LINK} and {REQUIRED_TROOP_AMOUNT} men to search for her and bring her back. You expect them to complete this task and return in {ALTERNATIVE_SOLUTION_DAYS} days.", null);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("ISSUE_GIVER_SETTLEMENT", base.IssueOwner.CurrentSettlement.Name);
					textObject.SetTextVariable("REQUIRED_TROOP_AMOUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06001051 RID: 4177 RVA: 0x0006B2C0 File Offset: 0x000694C0
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.ApplySuccessRewards();
				float randomFloat = MBRandom.RandomFloat;
				SkillObject skillObject;
				if (randomFloat <= 0.33f)
				{
					skillObject = DefaultSkills.OneHanded;
				}
				else if (randomFloat <= 0.66f)
				{
					skillObject = DefaultSkills.TwoHanded;
				}
				else
				{
					skillObject = DefaultSkills.Polearm;
				}
				base.AlternativeSolutionHero.AddSkillXp(skillObject, (float)((int)(500f + 1000f * base.IssueDifficultyMultiplier)));
			}

			// Token: 0x06001052 RID: 4178 RVA: 0x0006B324 File Offset: 0x00069524
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -10;
				if (base.IssueOwner.CurrentSettlement.Village.Bound != null)
				{
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security -= 5f;
				}
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x06001053 RID: 4179 RVA: 0x0006B3A8 File Offset: 0x000695A8
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=MaXA5HJi}Your companions report that the {ISSUE_GIVER.LINK}'s daughter returns to {?ISSUE_GIVER.GENDER}her{?}him{\\?} safe and sound. {?ISSUE_GIVER.GENDER}She{?}He{\\?} is happy and sends {?ISSUE_GIVER.GENDER}her{?}his{\\?} regards with a large pouch of {BASE_REWARD_GOLD}{GOLD_ICON}.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x06001054 RID: 4180 RVA: 0x0006B400 File Offset: 0x00069600
			private void ApplySuccessRewards()
			{
				GainRenownAction.Apply(Hero.MainHero, 2f, false);
				base.IssueOwner.AddPower(10f);
				this.RelationshipChangeWithIssueOwner = 10;
				if (base.IssueOwner.CurrentSettlement.Village.Bound != null)
				{
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security += 10f;
				}
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x06001055 RID: 4181 RVA: 0x0006B478 File Offset: 0x00069678
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=kr68V5pm}{ISSUE_GIVER.NAME} Wants {?ISSUE_GIVER.GENDER}Her{?}His{\\?} Daughter Found", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x06001056 RID: 4182 RVA: 0x0006B4AC File Offset: 0x000696AC
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=SkzM5eSv}{ISSUE_GIVER.LINK}'s daughter is missing. {?ISSUE_GIVER.GENDER}She{?}He{\\?} is offering a substantial reward to find the young woman and bring her back safely.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x06001057 RID: 4183 RVA: 0x0006B4E0 File Offset: 0x000696E0
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=7RyXSkEE}Wouldn't want to be the poor lovesick sap who ran off with {QUEST_GIVER.NAME}'s daughter.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06001058 RID: 4184 RVA: 0x0006B512 File Offset: 0x00069712
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06001059 RID: 4185 RVA: 0x0006B514 File Offset: 0x00069714
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600105A RID: 4186 RVA: 0x0006B516 File Offset: 0x00069716
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(19f), this.RewardGold, base.IssueDifficultyMultiplier);
			}

			// Token: 0x0600105B RID: 4187 RVA: 0x0006B53A File Offset: 0x0006973A
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x0600105C RID: 4188 RVA: 0x0006B53D File Offset: 0x0006973D
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Charm) >= hero.GetSkillValue(DefaultSkills.Scouting)) ? DefaultSkills.Charm : DefaultSkills.Scouting, 120);
			}

			// Token: 0x0600105D RID: 4189 RVA: 0x0006B56A File Offset: 0x0006976A
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x0600105E RID: 4190 RVA: 0x0006B584 File Offset: 0x00069784
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x0600105F RID: 4191 RVA: 0x0006B595 File Offset: 0x00069795
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06001060 RID: 4192 RVA: 0x0006B5A4 File Offset: 0x000697A4
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				bool flag2 = issueGiver.GetRelationWithPlayer() >= -10f && !issueGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction);
				flag = (flag2 ? IssueBase.PreconditionFlags.None : ((!issueGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction)) ? IssueBase.PreconditionFlags.Relation : IssueBase.PreconditionFlags.AtWar));
				relationHero = issueGiver;
				skill = null;
				requiredGold = 0;
				return flag2;
			}

			// Token: 0x06001061 RID: 4193 RVA: 0x0006B614 File Offset: 0x00069814
			public override bool IssueStayAliveConditions()
			{
				return !base.IssueOwner.CurrentSettlement.IsRaided && !base.IssueOwner.CurrentSettlement.IsUnderRaid;
			}

			// Token: 0x06001062 RID: 4194 RVA: 0x0006B63D File Offset: 0x0006983D
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x06001063 RID: 4195 RVA: 0x0006B63F File Offset: 0x0006983F
			internal static void AutoGeneratedStaticCollectObjectsNotableWantsDaughterFoundIssue(object o, List<object> collectedObjects)
			{
				((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001064 RID: 4196 RVA: 0x0006B64D File Offset: 0x0006984D
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x04000831 RID: 2097
			private const int TroopTierForAlternativeSolution = 2;

			// Token: 0x04000832 RID: 2098
			private const int RequiredSkillLevelForAlternativeSolution = 120;
		}

		// Token: 0x020001C3 RID: 451
		public class NotableWantsDaughterFoundIssueQuest : QuestBase
		{
			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06001065 RID: 4197 RVA: 0x0006B658 File Offset: 0x00069858
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=kr68V5pm}{ISSUE_GIVER.NAME} Wants {?ISSUE_GIVER.GENDER}Her{?}His{\\?} Daughter Found", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06001066 RID: 4198 RVA: 0x0006B68A File Offset: 0x0006988A
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x06001067 RID: 4199 RVA: 0x0006B68D File Offset: 0x0006988D
			private bool DoesMainPartyHasEnoughScoutingSkill
			{
				get
				{
					return (float)MobilePartyHelper.GetMainPartySkillCounsellor(DefaultSkills.Scouting).GetSkillValue(DefaultSkills.Scouting) >= 150f * this._questDifficultyMultiplier;
				}
			}

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x06001068 RID: 4200 RVA: 0x0006B6B8 File Offset: 0x000698B8
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=1jExD58d}{QUEST_GIVER.LINK}, a merchant from {SETTLEMENT_NAME}, told you that {?QUEST_GIVER.GENDER}her{?}his{\\?} daughter {TARGET_HERO.NAME} has either been abducted or run off with a local rogue. You have agreed to search for her and bring her back to {SETTLEMENT_NAME}. If you cannot find their tracks when you exit settlement, you should visit the nearby villages of {SETTLEMENT_NAME} to look for clues and tracks of the kidnapper.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT_NAME", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17000189 RID: 393
			// (get) Token: 0x06001069 RID: 4201 RVA: 0x0006B740 File Offset: 0x00069940
			private TextObject SuccessQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=asVE53ac}Daughter returns to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}She{?}He{\\?} is happy. Sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards with a large pouch of {BASE_REWARD}{GOLD_ICON}.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x1700018A RID: 394
			// (get) Token: 0x0600106A RID: 4202 RVA: 0x0006B794 File Offset: 0x00069994
			private TextObject PlayerDefeatedByRogueLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=i1sth9Ls}You were defeated by the rogue. He and {TARGET_HERO.NAME} ran off while you were unconscious. You failed to bring the daughter back to her {?QUEST_GIVER.GENDER}mother{?}father{\\?} as promised to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}She{?}He{\\?} is furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018B RID: 395
			// (get) Token: 0x0600106B RID: 4203 RVA: 0x0006B7DE File Offset: 0x000699DE
			private TextObject FailQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=ak2EMWWR}You failed to bring the daughter back to her {?QUEST_GIVER.GENDER}mother{?}father{\\?} as promised to {QUEST_GIVER.LINK}. {QUEST_GIVER.LINK} is furious", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x0600106C RID: 4204 RVA: 0x0006B802 File Offset: 0x00069A02
			private TextObject QuestCanceledWarDeclaredLog
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x0600106D RID: 4205 RVA: 0x0006B826 File Offset: 0x00069A26
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x0600106E RID: 4206 RVA: 0x0006B84A File Offset: 0x00069A4A
			private TextObject VillageRaidedCancelQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=aN85Kfnq}{SETTLEMENT} was raided. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x0600106F RID: 4207 RVA: 0x0006B88C File Offset: 0x00069A8C
			public NotableWantsDaughterFoundIssueQuest(string questId, Hero questGiver, CampaignTime duration, int baseReward, float issueDifficultyMultiplier)
				: base(questId, questGiver, duration, baseReward)
			{
				this._questDifficultyMultiplier = issueDifficultyMultiplier;
				this._targetVillage = questGiver.CurrentSettlement.Village.Bound.BoundVillages.GetRandomElementWithPredicate<Village>((Village x) => x != questGiver.CurrentSettlement.Village);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture = this._rogueCharacterBasedOnCulture;
				string text = "khuzait";
				Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "steppe_bandits");
				rogueCharacterBasedOnCulture.Add(text, (clan != null) ? clan.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture2 = this._rogueCharacterBasedOnCulture;
				string text2 = "vlandia";
				Clan clan2 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture2.Add(text2, (clan2 != null) ? clan2.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture3 = this._rogueCharacterBasedOnCulture;
				string text3 = "aserai";
				Clan clan3 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "desert_bandits");
				rogueCharacterBasedOnCulture3.Add(text3, (clan3 != null) ? clan3.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture4 = this._rogueCharacterBasedOnCulture;
				string text4 = "battania";
				Clan clan4 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "forest_bandits");
				rogueCharacterBasedOnCulture4.Add(text4, (clan4 != null) ? clan4.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture5 = this._rogueCharacterBasedOnCulture;
				string text5 = "sturgia";
				Clan clan5 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "sea_raiders");
				rogueCharacterBasedOnCulture5.Add(text5, (clan5 != null) ? clan5.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture6 = this._rogueCharacterBasedOnCulture;
				string text6 = "empire_w";
				Clan clan6 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture6.Add(text6, (clan6 != null) ? clan6.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture7 = this._rogueCharacterBasedOnCulture;
				string text7 = "empire_s";
				Clan clan7 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture7.Add(text7, (clan7 != null) ? clan7.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture8 = this._rogueCharacterBasedOnCulture;
				string text8 = "empire";
				Clan clan8 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture8.Add(text8, (clan8 != null) ? clan8.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture9 = this._rogueCharacterBasedOnCulture;
				string text9 = "nord";
				Clan clan9 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "sea_raiders");
				rogueCharacterBasedOnCulture9.Add(text9, (clan9 != null) ? clan9.Culture.BanditBoss : null);
				int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
				int num = MBRandom.RandomInt(heroComesOfAge, 25);
				int num2 = MBRandom.RandomInt(heroComesOfAge, 25);
				CharacterObject randomCompanionTemplateWithPredicate = CharacterHelper.GetRandomCompanionTemplateWithPredicate((CharacterObject x) => x.IsFemale && x.Culture == questGiver.CurrentSettlement.Culture);
				this._daughterHero = HeroCreator.CreateSpecialHero(randomCompanionTemplateWithPredicate, questGiver.HomeSettlement, questGiver.Clan, null, num);
				this._daughterHero.HiddenInEncyclopedia = true;
				this._daughterHero.Father = questGiver;
				this._rogueHero = HeroCreator.CreateSpecialHero(this.GetRogueCharacterBasedOnCulture(questGiver.Culture.StringId), questGiver.HomeSettlement, questGiver.Clan, null, num2);
				this._rogueHero.Culture = questGiver.Culture;
				this._rogueHero.HiddenInEncyclopedia = true;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06001070 RID: 4208 RVA: 0x0006BC88 File Offset: 0x00069E88
			private CharacterObject GetRogueCharacterBasedOnCulture(string cultureStrId)
			{
				CharacterObject characterObject;
				if (this._rogueCharacterBasedOnCulture.ContainsKey(cultureStrId))
				{
					characterObject = this._rogueCharacterBasedOnCulture[cultureStrId];
				}
				else
				{
					characterObject = base.QuestGiver.CurrentSettlement.Culture.NotableTemplates.GetRandomElementWithPredicate<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.GangLeader && !x.IsFemale);
				}
				return characterObject;
			}

			// Token: 0x06001071 RID: 4209 RVA: 0x0006BCF0 File Offset: 0x00069EF0
			protected override void SetDialogs()
			{
				TextObject textObject = new TextObject("{=PZq1EMcx}Thank you for your help. I am still very worried about my girl {TARGET_HERO.FIRSTNAME}. Please find her and bring her back to me as soon as you can.[if:convo_worried]", null);
				StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=sglD6abb}Please! Bring my daughter back.", null);
				TextObject textObject3 = new TextObject("{=ddEu5IFQ}I hope so.", null);
				TextObject textObject4 = new TextObject("{=IdKG3IaS}Good to hear that.", null);
				TextObject textObject5 = new TextObject("{=0hXofVLx}Don't worry I'll bring her.", null);
				TextObject textObject6 = new TextObject("{=zpqP5LsC}I'll go right away.", null);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(textObject, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver && !this._didPlayerBeatRouge)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(textObject2, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver && !this._didPlayerBeatRouge)
					.BeginPlayerOptions(null, false)
					.PlayerOption(textObject5, null, null, null)
					.NpcLine(textObject3, null, null, null, null)
					.CloseDialog()
					.PlayerOption(textObject6, null, null, null)
					.NpcLine(textObject4, null, null, null, null)
					.CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRougeDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterAfterFightDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterAfterAcceptDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterAfterPersuadedDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterDialogWhenVillageRaid(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRougeAfterAcceptDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRogueAfterPersuadedDialog(), this);
			}

			// Token: 0x06001072 RID: 4210 RVA: 0x0006BEA2 File Offset: 0x0006A0A2
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
				if (this._daughterHero != null)
				{
					this._daughterHero.HiddenInEncyclopedia = true;
				}
				if (this._rogueHero != null)
				{
					this._rogueHero.HiddenInEncyclopedia = true;
				}
			}

			// Token: 0x06001073 RID: 4211 RVA: 0x0006BED2 File Offset: 0x0006A0D2
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001074 RID: 4212 RVA: 0x0006BED4 File Offset: 0x0006A0D4
			private bool IsRougeHero(IAgent agent)
			{
				return agent.Character == this._rogueHero.CharacterObject;
			}

			// Token: 0x06001075 RID: 4213 RVA: 0x0006BEE9 File Offset: 0x0006A0E9
			private bool IsDaughterHero(IAgent agent)
			{
				return agent.Character == this._daughterHero.CharacterObject;
			}

			// Token: 0x06001076 RID: 4214 RVA: 0x0006BEFE File Offset: 0x0006A0FE
			private bool IsMainHero(IAgent agent)
			{
				return agent.Character == CharacterObject.PlayerCharacter;
			}

			// Token: 0x06001077 RID: 4215 RVA: 0x0006BF10 File Offset: 0x0006A110
			private bool multi_character_conversation_on_condition()
			{
				if (!this._villageIsRaidedTalkWithDaughter && !this._isDaughterPersuaded && !this._didPlayerBeatRouge && !this._acceptedDaughtersEscape && this._isQuestTargetMission && (CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject || CharacterObject.OneToOneConversationCharacter == this._rogueHero.CharacterObject))
				{
					MBList<Agent> mblist = new MBList<Agent>();
					foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 100f, mblist))
					{
						if (agent.Character == this._daughterHero.CharacterObject)
						{
							this._daughterAgent = agent;
							if (Mission.Current.GetMissionBehavior<MissionConversationLogic>() != null && Hero.OneToOneConversationHero != this._daughterHero)
							{
								Campaign.Current.ConversationManager.AddConversationAgents(new List<Agent> { this._daughterAgent }, true);
							}
						}
						else if (agent.Character == this._rogueHero.CharacterObject)
						{
							this._rogueAgent = agent;
							if (Mission.Current.GetMissionBehavior<MissionConversationLogic>() != null && Hero.OneToOneConversationHero != this._rogueHero)
							{
								Campaign.Current.ConversationManager.AddConversationAgents(new List<Agent> { this._rogueAgent }, true);
							}
						}
					}
					return this._daughterAgent != null && this._rogueAgent != null && this._daughterAgent.Health > 10f && this._rogueAgent.Health > 10f;
				}
				return false;
			}

			// Token: 0x06001078 RID: 4216 RVA: 0x0006C0C8 File Offset: 0x0006A2C8
			private bool daughter_conversation_after_fight_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject && this._didPlayerBeatRouge;
			}

			// Token: 0x06001079 RID: 4217 RVA: 0x0006C0E4 File Offset: 0x0006A2E4
			private void multi_agent_conversation_on_consequence()
			{
				this._task = this.GetPersuasionTask();
			}

			// Token: 0x0600107A RID: 4218 RVA: 0x0006C0F4 File Offset: 0x0006A2F4
			private DialogFlow GetRougeDialogFlow()
			{
				TextObject textObject = new TextObject("{=ovFbMMTJ}Who are you? Are you one of the bounty hunters sent by {QUEST_GIVER.LINK} to track us? Like we're animals or something? Look friend, we have done nothing wrong. As you may have figured out already, this woman and I, we love each other. I didn't force her to do anything.[ib:closed][if:convo_innocent_smile]", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=D25oY3j1}Thank you {?PLAYER.GENDER}lady{?}sir{\\?}. For your kindness and understanding. We won't forget this.[ib:demure][if:convo_happy]", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject2, false);
				TextObject textObject3 = new TextObject("{=oL3amiu1}Come {DAUGHTER_NAME.NAME}, let's go before other hounds sniff our trail... I mean... No offense {?PLAYER.GENDER}madam{?}sir{\\?}.", null);
				StringHelpers.SetCharacterProperties("DAUGHTER_NAME", this._daughterHero.CharacterObject, textObject3, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject3, false);
				TextObject textObject4 = new TextObject("{=92sbq1YY}I'm no child, {?PLAYER.GENDER}lady{?}sir{\\?}! Draw your weapon! I challenge you to a duel![ib:warrior2][if:convo_excited]", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject4, false);
				TextObject textObject5 = new TextObject("{=jfzErupx}He is right! I ran away with him willingly. I love my {?QUEST_GIVER.GENDER}mother{?}father{\\?},[ib:closed][if:convo_grave] but {?QUEST_GIVER.GENDER}she{?}he{\\?} can be such a tyrant. Please {?PLAYER.GENDER}lady{?}sir{\\?}, if you believe in freedom and love, please leave us be.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject5, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject5, false);
				TextObject textObject6 = new TextObject("{=5NljlbLA}Thank you kind {?PLAYER.GENDER}lady{?}sir{\\?}, thank you.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject6, false);
				TextObject textObject7 = new TextObject("{=i5fNZrhh}Please, {?PLAYER.GENDER}lady{?}sir{\\?}. I love him truly and I wish to spend the rest of my life with him.[ib:demure][if:convo_worried] I beg of you, please don't stand in our way.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject7, false);
				TextObject textObject8 = new TextObject("{=0RCdPKj2}Yes {?QUEST_GIVER.GENDER}she{?}he{\\?} would probably be sad. But not because of what you think. See, {QUEST_GIVER.LINK} promised me to one of {?QUEST_GIVER.GENDER}her{?}his{\\?} allies' sons and this will devastate {?QUEST_GIVER.GENDER}her{?}his{\\?} plans. That is true.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject8, false);
				TextObject textObject9 = new TextObject("{=5W7Kxfq9}I understand. If that is the case, I will let you go.", null);
				TextObject textObject10 = new TextObject("{=3XimdHOn}How do I know he's not forcing you to say that?", null);
				TextObject textObject11 = new TextObject("{=zNqDEuAw}But I've promised to find you and return you to your {?QUEST_GIVER.GENDER}mother{?}father{\\?}. {?QUEST_GIVER.GENDER}She{?}He{\\?} would be devastated.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject11, false);
				TextObject textObject12 = new TextObject("{=tuaQ5uU3}I guess the only way to free you from this pretty boy's spell is to kill him.", null);
				TextObject textObject13 = new TextObject("{=HDCmeGhG}I'm sorry but I gave a promise. I don't break my promises.", null);
				TextObject textObject14 = new TextObject("{=VGrHWxzf}This will be a massacre, not a duel, but I'm fine with that.", null);
				TextObject textObject15 = new TextObject("{=sytYViXb}I accept your duel.", null);
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.multi_character_conversation_on_condition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.multi_agent_conversation_on_consequence))
					.NpcLine(textObject5, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(textObject9, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject2, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.NpcLine(textObject3, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject6, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerAcceptedDaughtersEscape;
					})
					.CloseDialog()
					.PlayerOption(textObject10, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject7, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.PlayerLine(textObject11, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject8, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.GotoDialogState("start_daughter_persuade_to_come_persuasion")
					.GoBackToDialogState("daughter_persuade_to_come_persuasion_finished")
					.PlayerLine((Hero.MainHero.GetTraitLevel(DefaultTraits.Mercy) < 0) ? textObject12 : textObject13, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject4, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(textObject14, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null)
					.NpcLine(new TextObject("{=XWVW0oTB}You bastard![ib:aggressive][if:convo_furious]", null), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerRejectsDuelFight;
					})
					.CloseDialog()
					.PlayerOption(textObject15, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null)
					.NpcLine(new TextObject("{=jqahxjWD}Heaven protect me![ib:aggressive][if:convo_astonished]", null), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerAcceptsDuelFight;
					})
					.CloseDialog()
					.EndPlayerOptions()
					.EndPlayerOptions()
					.CloseDialog();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x0600107B RID: 4219 RVA: 0x0006C530 File Offset: 0x0006A730
			private DialogFlow GetDaughterAfterFightDialog()
			{
				TextObject textObject = new TextObject("{=MN2v1AZQ}I hate you! You killed him! I can't believe it! I will hate you with all my heart till my dying days.[if:convo_angry]", null);
				TextObject textObject2 = new TextObject("{=TTkVcObg}What choice do I have, you heartless bastard?![if:convo_furious]", null);
				TextObject textObject3 = new TextObject("{=XqsrsjiL}I did what I had to do. Pack up, you need to go.", null);
				TextObject textObject4 = new TextObject("{=KQ3aYvp3}Some day you'll see I did you a favor. Pack up, you need to go.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.daughter_conversation_after_fight_on_condition))
					.PlayerLine((Hero.MainHero.GetTraitLevel(DefaultTraits.Mercy) < 0) ? textObject3 : textObject4, null, null, null)
					.NpcLine(textObject2, null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerWonTheFight;
					})
					.CloseDialog();
			}

			// Token: 0x0600107C RID: 4220 RVA: 0x0006C5D4 File Offset: 0x0006A7D4
			private DialogFlow GetDaughterAfterAcceptDialog()
			{
				TextObject textObject = new TextObject("{=0Wg00sfN}Thank you, {?PLAYER.GENDER}madam{?}sir{\\?}. We will be moving immediately.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				TextObject textObject2 = new TextObject("{=kUReBc04}Good.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.daughter_conversation_after_accept_on_condition))
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x0600107D RID: 4221 RVA: 0x0006C640 File Offset: 0x0006A840
			private bool daughter_conversation_after_accept_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject && this._acceptedDaughtersEscape;
			}

			// Token: 0x0600107E RID: 4222 RVA: 0x0006C65C File Offset: 0x0006A85C
			private DialogFlow GetDaughterAfterPersuadedDialog()
			{
				TextObject textObject = new TextObject("{=B8bHpJRP}You are right, {?PLAYER.GENDER}my lady{?}sir{\\?}. I should be moving immediately.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				TextObject textObject2 = new TextObject("{=kUReBc04}Good.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.daughter_conversation_after_persuaded_on_condition))
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x0600107F RID: 4223 RVA: 0x0006C6C8 File Offset: 0x0006A8C8
			private DialogFlow GetDaughterDialogWhenVillageRaid()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=w0HPC53e}Who are you? What do you want from me?[ib:nervous][if:convo_bared_teeth]", null), null, null, null, null).Condition(() => this._villageIsRaidedTalkWithDaughter)
					.PlayerLine(new TextObject("{=iRupMGI0}Calm down! Your father has sent me to find you.", null), null, null, null)
					.NpcLine(new TextObject("{=dwNquUNr}My father? Oh, thank god! I saw terrible things. [ib:nervous2][if:convo_shocked]They took my beloved one and slew many innocents without hesitation.", null), null, null, null, null)
					.PlayerLine("{=HtAr22re}Try to forget all about these and return to your father's house.", null, null, null)
					.NpcLine("{=FgSIsasF}Yes, you are right. I shall be on my way...", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							this.ApplyDeliverySuccessConsequences();
							base.CompleteQuestWithSuccess();
							base.AddLog(this.SuccessQuestLogText, false);
							this._villageIsRaidedTalkWithDaughter = false;
						};
					})
					.CloseDialog();
			}

			// Token: 0x06001080 RID: 4224 RVA: 0x0006C75E File Offset: 0x0006A95E
			private bool daughter_conversation_after_persuaded_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject && this._isDaughterPersuaded;
			}

			// Token: 0x06001081 RID: 4225 RVA: 0x0006C77C File Offset: 0x0006A97C
			private DialogFlow GetRougeAfterAcceptDialog()
			{
				TextObject textObject = new TextObject("{=wlKtDR2z}Thank you, {?PLAYER.GENDER}my lady{?}sir{\\?}.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.rogue_conversation_after_accept_on_condition))
					.PlayerLine(new TextObject("{=0YJGvJ7o}You should leave now.", null), null, null, null)
					.NpcLine(new TextObject("{=6Q4cPOSG}Yes, we will.", null), null, null, null, null)
					.CloseDialog();
			}

			// Token: 0x06001082 RID: 4226 RVA: 0x0006C7FA File Offset: 0x0006A9FA
			private bool rogue_conversation_after_accept_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._rogueHero.CharacterObject && this._acceptedDaughtersEscape;
			}

			// Token: 0x06001083 RID: 4227 RVA: 0x0006C818 File Offset: 0x0006AA18
			private DialogFlow GetRogueAfterPersuadedDialog()
			{
				TextObject textObject = new TextObject("{=GFt9KiHP}You are right. Maybe we need to persuade {QUEST_GIVER.NAME}.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=btJkBTSF}I am sure you can solve it.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.rogue_conversation_after_persuaded_on_condition))
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x06001084 RID: 4228 RVA: 0x0006C88A File Offset: 0x0006AA8A
			private bool rogue_conversation_after_persuaded_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._rogueHero.CharacterObject && this._isDaughterPersuaded;
			}

			// Token: 0x06001085 RID: 4229 RVA: 0x0006C8A8 File Offset: 0x0006AAA8
			protected override void OnTimedOut()
			{
				this.ApplyDeliveryRejectedFailConsequences();
				TextObject textObject = new TextObject("{=KAvwytDK}You didn't bring {DAUGHTER.NAME} to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}she{?}he{\\?} must be furious.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("DAUGHTER", this._daughterHero.CharacterObject, textObject, false);
				base.AddLog(textObject, false);
			}

			// Token: 0x06001086 RID: 4230 RVA: 0x0006C900 File Offset: 0x0006AB00
			private void PlayerAcceptedDaughtersEscape()
			{
				this._acceptedDaughtersEscape = true;
			}

			// Token: 0x06001087 RID: 4231 RVA: 0x0006C909 File Offset: 0x0006AB09
			private void PlayerWonTheFight()
			{
				this._isDaughterCaptured = true;
				Mission.Current.SetMissionMode(MissionMode.StartUp, false);
			}

			// Token: 0x06001088 RID: 4232 RVA: 0x0006C920 File Offset: 0x0006AB20
			private void ApplyDaughtersEscapeAcceptedFailConsequences()
			{
				this.RelationshipChangeWithQuestGiver = -10;
				if (base.QuestGiver.CurrentSettlement.Village.Bound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
				}
			}

			// Token: 0x06001089 RID: 4233 RVA: 0x0006C9A4 File Offset: 0x0006ABA4
			private void ApplyDeliveryRejectedFailConsequences()
			{
				this.RelationshipChangeWithQuestGiver = -10;
				if (base.QuestGiver.CurrentSettlement.Village.Bound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
				}
			}

			// Token: 0x0600108A RID: 4234 RVA: 0x0006CA28 File Offset: 0x0006AC28
			private void ApplyDeliverySuccessConsequences()
			{
				GainRenownAction.Apply(Hero.MainHero, 2f, false);
				base.QuestGiver.AddPower(10f);
				this.RelationshipChangeWithQuestGiver = 10;
				if (base.QuestGiver.CurrentSettlement.Village.Bound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security += 10f;
				}
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
			}

			// Token: 0x0600108B RID: 4235 RVA: 0x0006CAB4 File Offset: 0x0006ACB4
			private void PlayerRejectsDuelFight()
			{
				this._rogueAgent = (Agent)MissionConversationLogic.Current.ConversationManager.ConversationAgents.First<IAgent>((IAgent x) => !x.Character.IsFemale);
				List<Agent> list = new List<Agent> { Agent.Main };
				List<Agent> list2 = new List<Agent> { this._rogueAgent };
				MBList<Agent> mblist = new MBList<Agent>();
				foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 30f, mblist))
				{
					foreach (Hero hero in Hero.MainHero.CompanionsInParty)
					{
						if (agent.Character == hero.CharacterObject)
						{
							list.Add(agent);
							break;
						}
					}
				}
				this._rogueAgent.Health = (float)(150 + list.Count * 20);
				this._rogueAgent.Defensiveness = 1f;
				Mission.Current.GetMissionBehavior<MissionFightHandler>().StartCustomFight(list, list2, false, false, new MissionFightHandler.OnFightEndDelegate(this.StartConversationAfterFight), float.Epsilon);
			}

			// Token: 0x0600108C RID: 4236 RVA: 0x0006CC30 File Offset: 0x0006AE30
			private void PlayerAcceptsDuelFight()
			{
				this._rogueAgent = (Agent)MissionConversationLogic.Current.ConversationManager.ConversationAgents.First<IAgent>((IAgent x) => !x.Character.IsFemale);
				List<Agent> list = new List<Agent> { Agent.Main };
				List<Agent> list2 = new List<Agent> { this._rogueAgent };
				MBList<Agent> mblist = new MBList<Agent>();
				foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 30f, mblist))
				{
					foreach (Hero hero in Hero.MainHero.CompanionsInParty)
					{
						if (agent.Character == hero.CharacterObject)
						{
							agent.SetTeam(Mission.Current.SpectatorTeam, false);
							DailyBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
							if (behaviorGroup.GetActiveBehavior() is FollowAgentBehavior)
							{
								behaviorGroup.GetBehavior<FollowAgentBehavior>().SetTargetAgent(null);
								break;
							}
							break;
						}
					}
				}
				this._rogueAgent.Health = 200f;
				Mission.Current.GetMissionBehavior<MissionFightHandler>().StartCustomFight(list, list2, false, false, new MissionFightHandler.OnFightEndDelegate(this.StartConversationAfterFight), float.Epsilon);
			}

			// Token: 0x0600108D RID: 4237 RVA: 0x0006CDCC File Offset: 0x0006AFCC
			private void StartConversationAfterFight(bool isPlayerSideWon)
			{
				if (isPlayerSideWon)
				{
					this._didPlayerBeatRouge = true;
					Campaign.Current.ConversationManager.SetupAndStartMissionConversation(this._daughterAgent, Mission.Current.MainAgent, false);
					TraitLevelingHelper.OnHostileAction(-50);
					return;
				}
				this._playerDefeatedByRogue = true;
			}

			// Token: 0x0600108E RID: 4238 RVA: 0x0006CE08 File Offset: 0x0006B008
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				TextObject textObject = new TextObject("{=ob5SejgJ}I will not abandon my love, {?PLAYER.GENDER}lady{?}sir{\\?}!", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				TextObject textObject2 = new TextObject("{=cqe8FU8M}{?QUEST_GIVER.GENDER}She{?}He{\\?} cares nothing about me! Only about {?QUEST_GIVER.GENDER}her{?}his{\\?} reputation in our district.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject2, false);
				dialog.AddDialogLine("daughter_persuade_to_come_introduction", "start_daughter_persuade_to_come_persuasion", "daughter_persuade_to_come_start_reservation", textObject2.ToString(), null, new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_daughter_on_consequence), this, 100, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				dialog.AddDialogLine("daughter_persuade_to_come_rejected", "daughter_persuade_to_come_start_reservation", "daughter_persuade_to_come_persuasion_failed", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.daughter_persuade_to_come_persuasion_failed_on_condition), new ConversationSentence.OnConsequenceDelegate(this.daughter_persuade_to_come_persuasion_failed_on_consequence), this, 100, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				dialog.AddDialogLine("daughter_persuade_to_come_failed", "daughter_persuade_to_come_persuasion_failed", "daughter_persuade_to_come_persuasion_finished", textObject.ToString(), null, null, this, 100, null, null, null);
				dialog.AddDialogLine("daughter_persuade_to_come_start", "daughter_persuade_to_come_start_reservation", "daughter_persuade_to_come_persuasion_select_option", "{=9b2BETct}I have already decided. Don't expect me to change my mind.", () => !this.daughter_persuade_to_come_persuasion_failed_on_condition(), null, this, 100, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				dialog.AddDialogLine("daughter_persuade_to_come_success", "daughter_persuade_to_come_start_reservation", "close_window", "{=3tmXBpRH}You're right. I cannot do this. I will return to my family. ", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.daughter_persuade_to_come_persuasion_success_on_consequence), this, int.MaxValue, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				string text = "daughter_persuade_to_come_select_option_1";
				string text2 = "daughter_persuade_to_come_persuasion_select_option";
				string text3 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text4 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate onConditionDelegate = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(text, text2, text3, text4, onConditionDelegate, onConsequenceDelegate, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				string text5 = "daughter_persuade_to_come_select_option_2";
				string text6 = "daughter_persuade_to_come_persuasion_select_option";
				string text7 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text8 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate onConditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_2_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_2);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(text5, text6, text7, text8, onConditionDelegate2, onConsequenceDelegate2, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				string text9 = "daughter_persuade_to_come_select_option_3";
				string text10 = "daughter_persuade_to_come_persuasion_select_option";
				string text11 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text12 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate onConditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_3_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_3);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(text9, text10, text11, text12, onConditionDelegate3, onConsequenceDelegate3, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				string text13 = "daughter_persuade_to_come_select_option_4";
				string text14 = "daughter_persuade_to_come_persuasion_select_option";
				string text15 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text16 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_4}";
				ConversationSentence.OnConditionDelegate onConditionDelegate4 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_4_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate4 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_4_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_4);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_4_on_condition);
				dialog.AddPlayerLine(text13, text14, text15, text16, onConditionDelegate4, onConsequenceDelegate4, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				dialog.AddDialogLine("daughter_persuade_to_come_select_option_reaction", "daughter_persuade_to_come_persuasion_selected_option_response", "daughter_persuade_to_come_start_reservation", "{=D0xDRqvm}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x0600108F RID: 4239 RVA: 0x0006D188 File Offset: 0x0006B388
			private void persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.Hard);
				float num;
				float num2;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out num, out num2, difficulty);
				this._task.ApplyEffects(num, num2);
			}

			// Token: 0x06001090 RID: 4240 RVA: 0x0006D1E4 File Offset: 0x0006B3E4
			private bool persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				return true;
			}

			// Token: 0x06001091 RID: 4241 RVA: 0x0006D214 File Offset: 0x0006B414
			private bool persuasion_select_option_1_on_condition()
			{
				if (this._task.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(0).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06001092 RID: 4242 RVA: 0x0006D294 File Offset: 0x0006B494
			private bool persuasion_select_option_2_on_condition()
			{
				if (this._task.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(1).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06001093 RID: 4243 RVA: 0x0006D314 File Offset: 0x0006B514
			private bool persuasion_select_option_3_on_condition()
			{
				if (this._task.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(2).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06001094 RID: 4244 RVA: 0x0006D394 File Offset: 0x0006B594
			private bool persuasion_select_option_4_on_condition()
			{
				if (this._task.Options.Count > 3)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(3), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(3).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_4", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06001095 RID: 4245 RVA: 0x0006D414 File Offset: 0x0006B614
			private void persuasion_select_option_1_on_consequence()
			{
				if (this._task.Options.Count > 0)
				{
					this._task.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x06001096 RID: 4246 RVA: 0x0006D440 File Offset: 0x0006B640
			private void persuasion_select_option_2_on_consequence()
			{
				if (this._task.Options.Count > 1)
				{
					this._task.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x06001097 RID: 4247 RVA: 0x0006D46C File Offset: 0x0006B66C
			private void persuasion_select_option_3_on_consequence()
			{
				if (this._task.Options.Count > 2)
				{
					this._task.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x06001098 RID: 4248 RVA: 0x0006D498 File Offset: 0x0006B698
			private void persuasion_select_option_4_on_consequence()
			{
				if (this._task.Options.Count > 3)
				{
					this._task.Options[3].BlockTheOption(true);
				}
			}

			// Token: 0x06001099 RID: 4249 RVA: 0x0006D4C4 File Offset: 0x0006B6C4
			private PersuasionOptionArgs persuasion_setup_option_1()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(0);
			}

			// Token: 0x0600109A RID: 4250 RVA: 0x0006D4D7 File Offset: 0x0006B6D7
			private PersuasionOptionArgs persuasion_setup_option_2()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(1);
			}

			// Token: 0x0600109B RID: 4251 RVA: 0x0006D4EA File Offset: 0x0006B6EA
			private PersuasionOptionArgs persuasion_setup_option_3()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(2);
			}

			// Token: 0x0600109C RID: 4252 RVA: 0x0006D4FD File Offset: 0x0006B6FD
			private PersuasionOptionArgs persuasion_setup_option_4()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(3);
			}

			// Token: 0x0600109D RID: 4253 RVA: 0x0006D510 File Offset: 0x0006B710
			private bool persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 0)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x0600109E RID: 4254 RVA: 0x0006D578 File Offset: 0x0006B778
			private bool persuasion_clickable_option_2_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 1)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked;
				}
				return false;
			}

			// Token: 0x0600109F RID: 4255 RVA: 0x0006D5E0 File Offset: 0x0006B7E0
			private bool persuasion_clickable_option_3_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 2)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked;
				}
				return false;
			}

			// Token: 0x060010A0 RID: 4256 RVA: 0x0006D648 File Offset: 0x0006B848
			private bool persuasion_clickable_option_4_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 3)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(3).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(3).IsBlocked;
				}
				return false;
			}

			// Token: 0x060010A1 RID: 4257 RVA: 0x0006D6B0 File Offset: 0x0006B8B0
			private PersuasionTask GetPersuasionTask()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=5aDlmdmb}No... No. It does not make sense.", null);
				persuasionTask.TryLaterLine = TextObject.GetEmpty();
				persuasionTask.SpokenLine = new TextObject("{=6P1ruzsC}Maybe...", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Leadership, DefaultTraits.Honor, TraitEffect.Positive, PersuasionArgumentStrength.Hard, true, new TextObject("{=Nhfl6tcM}Maybe, but that is your duty to your family.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				TextObject textObject = new TextObject("{=lustkZ7s}Perhaps {?QUEST_GIVER.GENDER}she{?}he{\\?} made those plans because {?QUEST_GIVER.GENDER}she{?}he{\\?} loves you.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Mercy, TraitEffect.Positive, PersuasionArgumentStrength.VeryEasy, false, textObject, null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.VeryHard, false, new TextObject("{=Ns6Svjsn}Do you think this one will be faithful to you over many years? I know a rogue when I see one.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				PersuasionOptionArgs persuasionOptionArgs4 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Mercy, TraitEffect.Negative, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=2dL6j8Hp}You want to marry a corpse? Because I'm going to kill your lover if you don't listen.", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs4);
				return persuasionTask;
			}

			// Token: 0x060010A2 RID: 4258 RVA: 0x0006D7B2 File Offset: 0x0006B9B2
			private void persuasion_start_with_daughter_on_consequence()
			{
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.Hard);
			}

			// Token: 0x060010A3 RID: 4259 RVA: 0x0006D7D8 File Offset: 0x0006B9D8
			private void daughter_persuade_to_come_persuasion_success_on_consequence()
			{
				ConversationManager.EndPersuasion();
				this._isDaughterPersuaded = true;
			}

			// Token: 0x060010A4 RID: 4260 RVA: 0x0006D7E8 File Offset: 0x0006B9E8
			private bool daughter_persuade_to_come_persuasion_failed_on_condition()
			{
				if (this._task.Options.All<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._task.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x060010A5 RID: 4261 RVA: 0x0006D846 File Offset: 0x0006BA46
			private void daughter_persuade_to_come_persuasion_failed_on_consequence()
			{
				ConversationManager.EndPersuasion();
			}

			// Token: 0x060010A6 RID: 4262 RVA: 0x0006D850 File Offset: 0x0006BA50
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party.IsMainParty && settlement == base.QuestGiver.CurrentSettlement && this._exitedQuestSettlementForTheFirstTime)
				{
					if (this.DoesMainPartyHasEnoughScoutingSkill)
					{
						QuestHelper.AddMapArrowFromPointToTarget(new TextObject("{=YdwLnWa1}Direction of daughter and rogue", null), settlement.Position, this._targetVillage.Settlement.Position, 5f, 0.1f);
						MBInformationManager.AddQuickInformation(new TextObject("{=O15PyNUK}With the help of your scouting skill, you were able to trace their tracks.", null), 0, null, null, "");
						MBInformationManager.AddQuickInformation(new TextObject("{=gOWebWiK}Their direction is marked with an arrow in the campaign map.", null), 0, null, null, "");
						base.AddTrackedObject(this._targetVillage.Settlement);
					}
					else
					{
						foreach (Village village in base.QuestGiver.CurrentSettlement.Village.Bound.BoundVillages)
						{
							if (village != base.QuestGiver.CurrentSettlement.Village)
							{
								this._villagesAndAlreadyVisitedBooleans.Add(village, false);
								base.AddTrackedObject(village.Settlement);
							}
						}
					}
					TextObject textObject = new TextObject("{=FvtAJE2Q}In order to find {QUEST_GIVER.LINK}'s daughter, you have decided to visit nearby villages.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					base.AddLog(textObject, this.DoesMainPartyHasEnoughScoutingSkill);
					this._exitedQuestSettlementForTheFirstTime = false;
				}
				if (party.IsMainParty && settlement == this._targetVillage.Settlement)
				{
					this._isQuestTargetMission = false;
				}
			}

			// Token: 0x060010A7 RID: 4263 RVA: 0x0006D9D4 File Offset: 0x0006BBD4
			public void OnBeforeMissionOpened()
			{
				if (this._isQuestTargetMission)
				{
					Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("village_center");
					if (locationWithId != null)
					{
						this.HandleRogueEquipment();
						locationWithId.AddCharacter(this.CreateQuestLocationCharacter(this._daughterHero.CharacterObject, LocationCharacter.CharacterRelations.Neutral));
						locationWithId.AddCharacter(this.CreateQuestLocationCharacter(this._rogueHero.CharacterObject, LocationCharacter.CharacterRelations.Neutral));
					}
				}
			}

			// Token: 0x060010A8 RID: 4264 RVA: 0x0006DA38 File Offset: 0x0006BC38
			private void HandleRogueEquipment()
			{
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("short_sword_t3");
				this._rogueHero.CivilianEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(@object, null, null, false));
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					ItemObject item = this._rogueHero.BattleEquipment[equipmentIndex].Item;
					if (item != null && item.WeaponComponent.PrimaryWeapon.IsShield)
					{
						this._rogueHero.BattleEquipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, default(EquipmentElement));
					}
				}
			}

			// Token: 0x060010A9 RID: 4265 RVA: 0x0006DAC4 File Offset: 0x0006BCC4
			private void OnMissionEnded(IMission mission)
			{
				if (this._isQuestTargetMission)
				{
					this._daughterAgent = null;
					this._rogueAgent = null;
					if (this._isDaughterPersuaded)
					{
						this.ApplyDeliverySuccessConsequences();
						base.CompleteQuestWithSuccess();
						base.AddLog(this.SuccessQuestLogText, false);
						this.RemoveQuestCharacters();
						return;
					}
					if (this._acceptedDaughtersEscape)
					{
						this.ApplyDaughtersEscapeAcceptedFailConsequences();
						base.CompleteQuestWithFail(this.FailQuestLogText);
						this.RemoveQuestCharacters();
						return;
					}
					if (this._isDaughterCaptured)
					{
						this.ApplyDeliverySuccessConsequences();
						base.CompleteQuestWithSuccess();
						base.AddLog(this.SuccessQuestLogText, false);
						this.RemoveQuestCharacters();
						return;
					}
					if (this._playerDefeatedByRogue)
					{
						this.ApplyDeliveryFailedDueToDuelLostConsequences();
						base.CompleteQuestWithFail(null);
						base.AddLog(this.PlayerDefeatedByRogueLogText, false);
						this.RemoveQuestCharacters();
					}
				}
			}

			// Token: 0x060010AA RID: 4266 RVA: 0x0006DB88 File Offset: 0x0006BD88
			private void ApplyDeliveryFailedDueToDuelLostConsequences()
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._daughterHero, -5, true);
				this.RelationshipChangeWithQuestGiver = -10;
				if (base.QuestGiver.CurrentSettlement.Village.Bound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
				}
			}

			// Token: 0x060010AB RID: 4267 RVA: 0x0006DC20 File Offset: 0x0006BE20
			private LocationCharacter CreateQuestLocationCharacter(CharacterObject character, LocationCharacter.CharacterRelations relation)
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
				Tuple<string, Monster> tuple = new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, character.IsFemale, "_villager"), monsterWithSuffix);
				return new LocationCharacter(new AgentData(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))).Monster(tuple.Item2), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors), "alley_2", true, relation, tuple.Item1, false, false, null, false, true, true, null, false);
			}

			// Token: 0x060010AC RID: 4268 RVA: 0x0006DCA7 File Offset: 0x0006BEA7
			private void RemoveQuestCharacters()
			{
				Settlement.CurrentSettlement.LocationComplex.RemoveCharacterIfExists(this._daughterHero);
				Settlement.CurrentSettlement.LocationComplex.RemoveCharacterIfExists(this._rogueHero);
			}

			// Token: 0x060010AD RID: 4269 RVA: 0x0006DCD4 File Offset: 0x0006BED4
			private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
			{
				if (party != null && party.IsMainParty && settlement.IsVillage)
				{
					if (this._villagesAndAlreadyVisitedBooleans.ContainsKey(settlement.Village) && !this._villagesAndAlreadyVisitedBooleans[settlement.Village])
					{
						if (settlement.Village != this._targetVillage)
						{
							if (settlement.IsSettlementBusy(this))
							{
								TextObject textObject = (settlement.IsRaided ? new TextObject("{=YTaM6G1E}It seems the village has been raided a short while ago. You found nothing but smoke, fire and crying people.", null) : new TextObject("{=2P3UJ8be}You ask around the village if anyone saw {TARGET_HERO.NAME} or some suspicious characters with a young woman.{newline}{newline}Villagers say that they saw a young man and woman ride in early in the morning. They bought some supplies and trotted off towards {TARGET_VILLAGE}.", null));
								textObject.SetTextVariable("TARGET_VILLAGE", this._targetVillage.Name);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject, false);
								InformationManager.ShowInquiry(new InquiryData(this.Title.ToString(), textObject.ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
							}
							if (!this._isTrackerLogAdded)
							{
								TextObject textObject2 = new TextObject("{=WGi3Zuv7}You asked the villagers around {CURRENT_SETTLEMENT} if they saw a young woman matching the description of {QUEST_GIVER.LINK}'s daughter, {TARGET_HERO.NAME}.{newline}{newline}They said a young woman and a young man dropped by early in the morning to buy some supplies and then rode off towards {TARGET_VILLAGE}.", null);
								textObject2.SetTextVariable("CURRENT_SETTLEMENT", Hero.MainHero.CurrentSettlement.Name);
								textObject2.SetTextVariable("TARGET_VILLAGE", this._targetVillage.Settlement.EncyclopediaLinkWithName);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject2, false);
								StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject2, false);
								base.AddLog(textObject2, false);
								this._isTrackerLogAdded = true;
							}
						}
						else
						{
							InquiryData inquiryData = null;
							if (settlement.IsRaided)
							{
								TextObject textObject3 = new TextObject("{=edoXFdmg}You have found {QUEST_GIVER.NAME}'s daughter.", null);
								StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject3, false);
								TextObject textObject4 = new TextObject("{=aYMW8bWi}Talk to her", null);
								inquiryData = new InquiryData(this.Title.ToString(), textObject3.ToString(), true, false, textObject4.ToString(), null, new Action(this.TalkWithDaughterAfterRaid), null, "", 0f, null, null, null);
							}
							else if (settlement.IsSettlementBusy(this))
							{
								TextObject textObject5 = new TextObject("{=aj4DZYyX}You ask around the village if anyone saw {TARGET_HERO.NAME} or some suspicious characters with a young woman. Villagers say that there was a young man and woman who arrived here exhausted. The villagers allowed them to stay for a while. You should search the village to find her.", null);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject5, false);
								base.AddLog(textObject5, false);
							}
							else
							{
								TextObject textObject6 = new TextObject("{=bbwNIIKI}You ask around the village if anyone saw {TARGET_HERO.NAME} or some suspicious characters with a young woman.{newline}{newline}Villagers say that there was a young man and woman who arrived here exhausted. The villagers allowed them to stay for a while.{newline}You can check the area, and see if they are still hiding here.", null);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject6, false);
								inquiryData = new InquiryData(this.Title.ToString(), textObject6.ToString(), true, true, new TextObject("{=bb6e8DoM}Search the village", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), new Action(this.SearchTheVillage), null, "", 0f, null, null, null);
							}
							if (inquiryData != null)
							{
								InformationManager.ShowInquiry(inquiryData, false, false);
							}
						}
						this._villagesAndAlreadyVisitedBooleans[settlement.Village] = true;
					}
					if (settlement == this._targetVillage.Settlement)
					{
						if (!base.IsTracked(this._daughterHero))
						{
							base.AddTrackedObject(this._daughterHero);
						}
						if (!base.IsTracked(this._rogueHero))
						{
							base.AddTrackedObject(this._rogueHero);
						}
						this._isQuestTargetMission = true;
					}
				}
			}

			// Token: 0x060010AE RID: 4270 RVA: 0x0006E002 File Offset: 0x0006C202
			private void SearchTheVillage()
			{
				VillageEncounter villageEncounter = PlayerEncounter.LocationEncounter as VillageEncounter;
				if (villageEncounter == null)
				{
					return;
				}
				villageEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("village_center"), null, null, null);
			}

			// Token: 0x060010AF RID: 4271 RVA: 0x0006E02C File Offset: 0x0006C22C
			private void TalkWithDaughterAfterRaid()
			{
				this._villageIsRaidedTalkWithDaughter = true;
				CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._daughterHero.CharacterObject, null, false, false, false, false, false, false));
			}

			// Token: 0x060010B0 RID: 4272 RVA: 0x0006E06D File Offset: 0x0006C26D
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this.PlayerStartsQuestLogText, false);
			}

			// Token: 0x060010B1 RID: 4273 RVA: 0x0006E083 File Offset: 0x0006C283
			private void CanHeroDie(Hero victim, KillCharacterAction.KillCharacterActionDetail detail, ref bool result)
			{
				if (victim == Hero.MainHero && Settlement.CurrentSettlement == this._targetVillage.Settlement && Mission.Current != null)
				{
					result = false;
				}
			}

			// Token: 0x060010B2 RID: 4274 RVA: 0x0006E0AC File Offset: 0x0006C2AC
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
				CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
			}

			// Token: 0x060010B3 RID: 4275 RVA: 0x0006E188 File Offset: 0x0006C388
			private void OnRaidCompleted(BattleSideEnum side, RaidEventComponent raidEventComponent)
			{
				if (raidEventComponent.MapEventSettlement == base.QuestGiver.CurrentSettlement)
				{
					base.CompleteQuestWithCancel(this.VillageRaidedCancelQuestLogText);
				}
			}

			// Token: 0x060010B4 RID: 4276 RVA: 0x0006E1A9 File Offset: 0x0006C3A9
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._rogueHero || hero == this._daughterHero)
				{
					result = false;
				}
			}

			// Token: 0x060010B5 RID: 4277 RVA: 0x0006E1C0 File Offset: 0x0006C3C0
			public override void OnHeroCanMoveToSettlementInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._rogueHero || hero == this._daughterHero)
				{
					result = false;
				}
			}

			// Token: 0x060010B6 RID: 4278 RVA: 0x0006E1D7 File Offset: 0x0006C3D7
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x060010B7 RID: 4279 RVA: 0x0006E1EA File Offset: 0x0006C3EA
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclaredLog);
				}
			}

			// Token: 0x060010B8 RID: 4280 RVA: 0x0006E219 File Offset: 0x0006C419
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclaredLog, false);
			}

			// Token: 0x060010B9 RID: 4281 RVA: 0x0006E234 File Offset: 0x0006C434
			protected override void OnFinalize()
			{
				if (base.IsTracked(this._targetVillage.Settlement))
				{
					base.RemoveTrackedObject(this._targetVillage.Settlement);
				}
				if (!Hero.MainHero.IsPrisoner && !this.DoesMainPartyHasEnoughScoutingSkill)
				{
					foreach (Village village in base.QuestGiver.CurrentSettlement.BoundVillages)
					{
						if (base.IsTracked(village.Settlement))
						{
							base.RemoveTrackedObject(village.Settlement);
						}
					}
				}
				if (this._rogueHero != null && this._rogueHero.IsAlive)
				{
					KillCharacterAction.ApplyByRemove(this._rogueHero, false, true);
				}
				if (this._daughterHero != null && this._daughterHero.IsAlive)
				{
					KillCharacterAction.ApplyByRemove(this._daughterHero, false, true);
				}
			}

			// Token: 0x060010BA RID: 4282 RVA: 0x0006E320 File Offset: 0x0006C520
			internal static void AutoGeneratedStaticCollectObjectsNotableWantsDaughterFoundIssueQuest(object o, List<object> collectedObjects)
			{
				((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060010BB RID: 4283 RVA: 0x0006E32E File Offset: 0x0006C52E
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._daughterHero);
				collectedObjects.Add(this._rogueHero);
				collectedObjects.Add(this._targetVillage);
				collectedObjects.Add(this._villagesAndAlreadyVisitedBooleans);
			}

			// Token: 0x060010BC RID: 4284 RVA: 0x0006E367 File Offset: 0x0006C567
			internal static object AutoGeneratedGetMemberValue_daughterHero(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._daughterHero;
			}

			// Token: 0x060010BD RID: 4285 RVA: 0x0006E374 File Offset: 0x0006C574
			internal static object AutoGeneratedGetMemberValue_rogueHero(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._rogueHero;
			}

			// Token: 0x060010BE RID: 4286 RVA: 0x0006E381 File Offset: 0x0006C581
			internal static object AutoGeneratedGetMemberValue_isQuestTargetMission(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isQuestTargetMission;
			}

			// Token: 0x060010BF RID: 4287 RVA: 0x0006E393 File Offset: 0x0006C593
			internal static object AutoGeneratedGetMemberValue_didPlayerBeatRouge(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._didPlayerBeatRouge;
			}

			// Token: 0x060010C0 RID: 4288 RVA: 0x0006E3A5 File Offset: 0x0006C5A5
			internal static object AutoGeneratedGetMemberValue_exitedQuestSettlementForTheFirstTime(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._exitedQuestSettlementForTheFirstTime;
			}

			// Token: 0x060010C1 RID: 4289 RVA: 0x0006E3B7 File Offset: 0x0006C5B7
			internal static object AutoGeneratedGetMemberValue_isTrackerLogAdded(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isTrackerLogAdded;
			}

			// Token: 0x060010C2 RID: 4290 RVA: 0x0006E3C9 File Offset: 0x0006C5C9
			internal static object AutoGeneratedGetMemberValue_isDaughterPersuaded(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isDaughterPersuaded;
			}

			// Token: 0x060010C3 RID: 4291 RVA: 0x0006E3DB File Offset: 0x0006C5DB
			internal static object AutoGeneratedGetMemberValue_isDaughterCaptured(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isDaughterCaptured;
			}

			// Token: 0x060010C4 RID: 4292 RVA: 0x0006E3ED File Offset: 0x0006C5ED
			internal static object AutoGeneratedGetMemberValue_acceptedDaughtersEscape(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._acceptedDaughtersEscape;
			}

			// Token: 0x060010C5 RID: 4293 RVA: 0x0006E3FF File Offset: 0x0006C5FF
			internal static object AutoGeneratedGetMemberValue_targetVillage(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._targetVillage;
			}

			// Token: 0x060010C6 RID: 4294 RVA: 0x0006E40C File Offset: 0x0006C60C
			internal static object AutoGeneratedGetMemberValue_villageIsRaidedTalkWithDaughter(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._villageIsRaidedTalkWithDaughter;
			}

			// Token: 0x060010C7 RID: 4295 RVA: 0x0006E41E File Offset: 0x0006C61E
			internal static object AutoGeneratedGetMemberValue_villagesAndAlreadyVisitedBooleans(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._villagesAndAlreadyVisitedBooleans;
			}

			// Token: 0x060010C8 RID: 4296 RVA: 0x0006E42B File Offset: 0x0006C62B
			internal static object AutoGeneratedGetMemberValue_questDifficultyMultiplier(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._questDifficultyMultiplier;
			}

			// Token: 0x04000833 RID: 2099
			[SaveableField(10)]
			private readonly Hero _daughterHero;

			// Token: 0x04000834 RID: 2100
			[SaveableField(20)]
			private readonly Hero _rogueHero;

			// Token: 0x04000835 RID: 2101
			private Agent _daughterAgent;

			// Token: 0x04000836 RID: 2102
			private Agent _rogueAgent;

			// Token: 0x04000837 RID: 2103
			[SaveableField(50)]
			private bool _isQuestTargetMission;

			// Token: 0x04000838 RID: 2104
			[SaveableField(60)]
			private bool _didPlayerBeatRouge;

			// Token: 0x04000839 RID: 2105
			[SaveableField(70)]
			private bool _exitedQuestSettlementForTheFirstTime = true;

			// Token: 0x0400083A RID: 2106
			[SaveableField(80)]
			private bool _isTrackerLogAdded;

			// Token: 0x0400083B RID: 2107
			[SaveableField(90)]
			private bool _isDaughterPersuaded;

			// Token: 0x0400083C RID: 2108
			[SaveableField(91)]
			private bool _isDaughterCaptured;

			// Token: 0x0400083D RID: 2109
			[SaveableField(100)]
			private bool _acceptedDaughtersEscape;

			// Token: 0x0400083E RID: 2110
			[SaveableField(110)]
			private readonly Village _targetVillage;

			// Token: 0x0400083F RID: 2111
			[SaveableField(120)]
			private bool _villageIsRaidedTalkWithDaughter;

			// Token: 0x04000840 RID: 2112
			[SaveableField(140)]
			private Dictionary<Village, bool> _villagesAndAlreadyVisitedBooleans = new Dictionary<Village, bool>();

			// Token: 0x04000841 RID: 2113
			private Dictionary<string, CharacterObject> _rogueCharacterBasedOnCulture = new Dictionary<string, CharacterObject>();

			// Token: 0x04000842 RID: 2114
			private bool _playerDefeatedByRogue;

			// Token: 0x04000843 RID: 2115
			private PersuasionTask _task;

			// Token: 0x04000844 RID: 2116
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.Hard;

			// Token: 0x04000845 RID: 2117
			private const int MaxAgeForDaughterAndRogue = 25;

			// Token: 0x04000846 RID: 2118
			[SaveableField(130)]
			private readonly float _questDifficultyMultiplier;
		}
	}
}
