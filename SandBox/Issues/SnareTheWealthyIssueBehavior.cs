using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000BA RID: 186
	public class SnareTheWealthyIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x0003416C File Offset: 0x0003236C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00034188 File Offset: 0x00032388
		private void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(SnareTheWealthyIssueBehavior.SnareTheWealthyIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(SnareTheWealthyIssueBehavior.SnareTheWealthyIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000341EC File Offset: 0x000323EC
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsGangLeader && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.IsTown && !issueGiver.CurrentSettlement.HasPort && issueGiver.CurrentSettlement.Town.Security <= 50f && this.GetTargetMerchant(issueGiver) != null;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00034248 File Offset: 0x00032448
		private Hero GetTargetMerchant(Hero issueOwner)
		{
			Hero hero = null;
			foreach (Hero hero2 in issueOwner.CurrentSettlement.Notables)
			{
				if (hero2 != issueOwner && hero2.IsMerchant && hero2.Power >= 150f && hero2.GetTraitLevel(DefaultTraits.Mercy) + hero2.GetTraitLevel(DefaultTraits.Honor) < 0 && hero2.CanHaveCampaignIssues() && !Campaign.Current.IssueManager.HasIssueCoolDown(typeof(SnareTheWealthyIssueBehavior.SnareTheWealthyIssue), hero2) && !Campaign.Current.IssueManager.HasIssueCoolDown(typeof(EscortMerchantCaravanIssueBehavior), hero2) && !Campaign.Current.IssueManager.HasIssueCoolDown(typeof(CaravanAmbushIssueBehavior), hero2))
				{
					hero = hero2;
					break;
				}
			}
			return hero;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0003433C File Offset: 0x0003253C
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			Hero targetMerchant = this.GetTargetMerchant(issueOwner);
			return new SnareTheWealthyIssueBehavior.SnareTheWealthyIssue(issueOwner, targetMerchant.CharacterObject);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0003435D File Offset: 0x0003255D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0400041E RID: 1054
		private const IssueBase.IssueFrequency SnareTheWealthyIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x020001D2 RID: 466
		public class SnareTheWealthyIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06001234 RID: 4660 RVA: 0x00073C9D File Offset: 0x00071E9D
			public SnareTheWealthyIssueTypeDefiner()
				: base(340000)
			{
			}

			// Token: 0x06001235 RID: 4661 RVA: 0x00073CAA File Offset: 0x00071EAA
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(SnareTheWealthyIssueBehavior.SnareTheWealthyIssue), 1, null);
				base.AddClassDefinition(typeof(SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest), 2, null);
			}

			// Token: 0x06001236 RID: 4662 RVA: 0x00073CD0 File Offset: 0x00071ED0
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice), 3, null);
			}
		}

		// Token: 0x020001D3 RID: 467
		public class SnareTheWealthyIssue : IssueBase
		{
			// Token: 0x170001F3 RID: 499
			// (get) Token: 0x06001237 RID: 4663 RVA: 0x00073CE4 File Offset: 0x00071EE4
			private int AlternativeSolutionReward
			{
				get
				{
					return MathF.Floor(1000f + 3000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06001238 RID: 4664 RVA: 0x00073CFD File Offset: 0x00071EFD
			public SnareTheWealthyIssue(Hero issueOwner, CharacterObject targetMerchant)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._targetMerchantCharacter = targetMerchant;
			}

			// Token: 0x170001F4 RID: 500
			// (get) Token: 0x06001239 RID: 4665 RVA: 0x00073D18 File Offset: 0x00071F18
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=bLigh8Sd}Well, let's just say there's an idea I've been mulling over.[ib:confident2][if:convo_bemused] You may be able to help. Have you met {TARGET_MERCHANT.NAME}? {?TARGET_MERCHANT.GENDER}She{?}He{\\?} is a very rich merchant. Very rich indeed. But not very honest… It's not right that someone without morals should have so much wealth, is it? I have a plan to redistribute it a bit.", null);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001F5 RID: 501
			// (get) Token: 0x0600123A RID: 4666 RVA: 0x00073D45 File Offset: 0x00071F45
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=keKEFagm}So what's the plan?", null);
				}
			}

			// Token: 0x170001F6 RID: 502
			// (get) Token: 0x0600123B RID: 4667 RVA: 0x00073D54 File Offset: 0x00071F54
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=SliFGAX4}{TARGET_MERCHANT.NAME} is always looking for extra swords to protect[if:convo_evil_smile] {?TARGET_MERCHANT.GENDER}her{?}his{\\?} caravans. The wicked are the ones who fear wickedness the most, you might say. What if those guards turned out to be robbers? {TARGET_MERCHANT.NAME} wouldn't trust just anyone but I think {?TARGET_MERCHANT.GENDER}she{?}he{\\?} might hire a renowned warrior like yourself. And if that warrior were to lead the caravan into an ambush… Oh I suppose it's all a bit dishonorable, but I wouldn't worry too much about your reputation. {TARGET_MERCHANT.NAME} is known to defraud {?TARGET_MERCHANT.GENDER}her{?}his{\\?} partners. If something happened to one of {?TARGET_MERCHANT.GENDER}her{?}his{\\?} caravans - well, most people won't know who to believe, and won't really care either.", null);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x0600123C RID: 4668 RVA: 0x00073D81 File Offset: 0x00071F81
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=4upBpsnb}All right. I am in.", null);
				}
			}

			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x0600123D RID: 4669 RVA: 0x00073D8E File Offset: 0x00071F8E
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=ivNVRP69}I prefer if you do this yourself, but one of your trusted companions with a strong[if:convo_evil_smile] sword-arm and enough brains to set an ambush can do the job with {TROOP_COUNT} fighters. We'll split the loot, and I'll throw in a little bonus on top of that for you..", null);
					textObject.SetTextVariable("TROOP_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x0600123E RID: 4670 RVA: 0x00073DAD File Offset: 0x00071FAD
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=biqYiCnr}My companion can handle it. Do not worry.", null);
				}
			}

			// Token: 0x170001FA RID: 506
			// (get) Token: 0x0600123F RID: 4671 RVA: 0x00073DBA File Offset: 0x00071FBA
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=UURamhdC}Thank you. This should make both of us a pretty penny.[if:convo_delighted]", null);
				}
			}

			// Token: 0x170001FB RID: 507
			// (get) Token: 0x06001240 RID: 4672 RVA: 0x00073DC7 File Offset: 0x00071FC7
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=pmuEeFV8}We are still arranging with your men how we'll spring this ambush. Do not worry. Everything will go smoothly.", null);
				}
			}

			// Token: 0x170001FC RID: 508
			// (get) Token: 0x06001241 RID: 4673 RVA: 0x00073DD4 File Offset: 0x00071FD4
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=28lLrXOe}{ISSUE_GIVER.LINK} shared their plan for robbing {TARGET_MERCHANT.LINK} with you. You agreed to send your companion along with {TROOP_COUNT} men to lead the ambush for them. They will return after {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					textObject.SetTextVariable("TROOP_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001FD RID: 509
			// (get) Token: 0x06001242 RID: 4674 RVA: 0x00073E44 File Offset: 0x00072044
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170001FE RID: 510
			// (get) Token: 0x06001243 RID: 4675 RVA: 0x00073E47 File Offset: 0x00072047
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001FF RID: 511
			// (get) Token: 0x06001244 RID: 4676 RVA: 0x00073E4A File Offset: 0x0007204A
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=IeihUvCD}Snare the Wealthy", null);
				}
			}

			// Token: 0x17000200 RID: 512
			// (get) Token: 0x06001245 RID: 4677 RVA: 0x00073E58 File Offset: 0x00072058
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=8LghFfQO}Help {ISSUE_GIVER.NAME} to rob {TARGET_MERCHANT.NAME} by acting as their guard.", null);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000201 RID: 513
			// (get) Token: 0x06001246 RID: 4678 RVA: 0x00073E9D File Offset: 0x0007209D
			protected override bool IssueQuestCanBeDuplicated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06001247 RID: 4679 RVA: 0x00073EA0 File Offset: 0x000720A0
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementLoyalty)
				{
					return -0.1f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -0.5f;
				}
				return 0f;
			}

			// Token: 0x17000202 RID: 514
			// (get) Token: 0x06001248 RID: 4680 RVA: 0x00073EC3 File Offset: 0x000720C3
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x17000203 RID: 515
			// (get) Token: 0x06001249 RID: 4681 RVA: 0x00073EC8 File Offset: 0x000720C8
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 10 + MathF.Ceiling(16f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000204 RID: 516
			// (get) Token: 0x0600124A RID: 4682 RVA: 0x00073EDE File Offset: 0x000720DE
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 2 + MathF.Ceiling(4f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000205 RID: 517
			// (get) Token: 0x0600124B RID: 4683 RVA: 0x00073EF3 File Offset: 0x000720F3
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x0600124C RID: 4684 RVA: 0x00073F08 File Offset: 0x00072108
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Roguery) >= hero.GetSkillValue(DefaultSkills.Tactics)) ? DefaultSkills.Roguery : DefaultSkills.Tactics, 120);
			}

			// Token: 0x0600124D RID: 4685 RVA: 0x00073F35 File Offset: 0x00072135
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x0600124E RID: 4686 RVA: 0x00073F4F File Offset: 0x0007214F
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x0600124F RID: 4687 RVA: 0x00073F5D File Offset: 0x0007215D
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = null;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06001250 RID: 4688 RVA: 0x00073F74 File Offset: 0x00072174
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				TraitLevelingHelper.OnIssueSolvedThroughAlternativeSolution(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
				});
				TraitLevelingHelper.OnIssueSolvedThroughAlternativeSolution(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 50)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.IssueOwner, 5, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -10, true, true);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.AlternativeSolutionReward, false);
			}

			// Token: 0x06001251 RID: 4689 RVA: 0x00073FF4 File Offset: 0x000721F4
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				TraitLevelingHelper.OnIssueSolvedThroughAlternativeSolution(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				TraitLevelingHelper.OnIssueSolvedThroughAlternativeSolution(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 100)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.IssueOwner, -10, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -10, true, true);
			}

			// Token: 0x06001252 RID: 4690 RVA: 0x00074062 File Offset: 0x00072262
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06001253 RID: 4691 RVA: 0x00074064 File Offset: 0x00072264
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001254 RID: 4692 RVA: 0x00074066 File Offset: 0x00072266
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest(questId, base.IssueOwner, this._targetMerchantCharacter, base.IssueDifficultyMultiplier, CampaignTime.DaysFromNow(10f));
			}

			// Token: 0x06001255 RID: 4693 RVA: 0x0007408C File Offset: 0x0007228C
			protected override void OnIssueFinalized()
			{
				if (base.IsSolvingWithQuest)
				{
					Campaign.Current.IssueManager.AddIssueCoolDownData(base.GetType(), new HeroRelatedIssueCoolDownData(this._targetMerchantCharacter.HeroObject, CampaignTime.DaysFromNow((float)Campaign.Current.Models.IssueModel.IssueOwnerCoolDownInDays)));
					Campaign.Current.IssueManager.AddIssueCoolDownData(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest), new HeroRelatedIssueCoolDownData(this._targetMerchantCharacter.HeroObject, CampaignTime.DaysFromNow((float)Campaign.Current.Models.IssueModel.IssueOwnerCoolDownInDays)));
					Campaign.Current.IssueManager.AddIssueCoolDownData(typeof(CaravanAmbushIssueBehavior.CaravanAmbushIssueQuest), new HeroRelatedIssueCoolDownData(this._targetMerchantCharacter.HeroObject, CampaignTime.DaysFromNow((float)Campaign.Current.Models.IssueModel.IssueOwnerCoolDownInDays)));
				}
			}

			// Token: 0x06001256 RID: 4694 RVA: 0x00074169 File Offset: 0x00072369
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06001257 RID: 4695 RVA: 0x0007416C File Offset: 0x0007236C
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				flag = IssueBase.PreconditionFlags.None;
				relationHero = null;
				requiredGold = 0;
				skill = null;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 20)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (issueGiver.CurrentSettlement.OwnerClan == Clan.PlayerClan)
				{
					flag |= IssueBase.PreconditionFlags.PlayerIsOwnerOfSettlement;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06001258 RID: 4696 RVA: 0x000741DB File Offset: 0x000723DB
			public override bool IssueStayAliveConditions()
			{
				return base.IssueOwner.IsAlive && base.IssueOwner.CurrentSettlement.Town.Security <= 80f && this._targetMerchantCharacter.HeroObject.IsAlive;
			}

			// Token: 0x06001259 RID: 4697 RVA: 0x00074218 File Offset: 0x00072418
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x0600125A RID: 4698 RVA: 0x0007421A File Offset: 0x0007241A
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._targetMerchantCharacter.HeroObject)
				{
					result = false;
				}
			}

			// Token: 0x0600125B RID: 4699 RVA: 0x0007422D File Offset: 0x0007242D
			internal static void AutoGeneratedStaticCollectObjectsSnareTheWealthyIssue(object o, List<object> collectedObjects)
			{
				((SnareTheWealthyIssueBehavior.SnareTheWealthyIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600125C RID: 4700 RVA: 0x0007423B File Offset: 0x0007243B
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetMerchantCharacter);
			}

			// Token: 0x0600125D RID: 4701 RVA: 0x00074250 File Offset: 0x00072450
			internal static object AutoGeneratedGetMemberValue_targetMerchantCharacter(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssue)o)._targetMerchantCharacter;
			}

			// Token: 0x040008B1 RID: 2225
			private const int IssueDuration = 30;

			// Token: 0x040008B2 RID: 2226
			private const int IssueQuestDuration = 10;

			// Token: 0x040008B3 RID: 2227
			private const int MinimumRequiredMenCount = 20;

			// Token: 0x040008B4 RID: 2228
			private const int MinimumRequiredRelationWithIssueGiver = -10;

			// Token: 0x040008B5 RID: 2229
			private const int AlternativeSolutionMinimumTroopTier = 2;

			// Token: 0x040008B6 RID: 2230
			private const int CompanionRoguerySkillValueThreshold = 120;

			// Token: 0x040008B7 RID: 2231
			[SaveableField(1)]
			private readonly CharacterObject _targetMerchantCharacter;
		}

		// Token: 0x020001D4 RID: 468
		public class SnareTheWealthyIssueQuest : QuestBase
		{
			// Token: 0x17000206 RID: 518
			// (get) Token: 0x0600125E RID: 4702 RVA: 0x0007425D File Offset: 0x0007245D
			private float CaravanEncounterStartDistance
			{
				get
				{
					return Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius * 7f;
				}
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x0600125F RID: 4703 RVA: 0x00074279 File Offset: 0x00072479
			private int CaravanPartyTroopCount
			{
				get
				{
					return 20 + MathF.Ceiling(40f * this._questDifficulty);
				}
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06001260 RID: 4704 RVA: 0x0007428F File Offset: 0x0007248F
			private int GangPartyTroopCount
			{
				get
				{
					return 10 + MathF.Ceiling(25f * this._questDifficulty);
				}
			}

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06001261 RID: 4705 RVA: 0x000742A5 File Offset: 0x000724A5
			private int Reward1
			{
				get
				{
					return MathF.Floor(1000f + 3000f * this._questDifficulty);
				}
			}

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x06001262 RID: 4706 RVA: 0x000742BE File Offset: 0x000724BE
			private int Reward2
			{
				get
				{
					return MathF.Floor((float)this.Reward1 * 0.4f);
				}
			}

			// Token: 0x06001263 RID: 4707 RVA: 0x000742D2 File Offset: 0x000724D2
			public SnareTheWealthyIssueQuest(string questId, Hero questGiver, CharacterObject targetMerchantCharacter, float questDifficulty, CampaignTime duration)
				: base(questId, questGiver, duration, 0)
			{
				this._targetMerchantCharacter = targetMerchantCharacter;
				this._targetSettlement = this.GetTargetSettlement();
				this._questDifficulty = questDifficulty;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x1700020B RID: 523
			// (get) Token: 0x06001264 RID: 4708 RVA: 0x0007430D File Offset: 0x0007250D
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=IeihUvCD}Snare the Wealthy", null);
				}
			}

			// Token: 0x1700020C RID: 524
			// (get) Token: 0x06001265 RID: 4709 RVA: 0x0007431A File Offset: 0x0007251A
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700020D RID: 525
			// (get) Token: 0x06001266 RID: 4710 RVA: 0x00074320 File Offset: 0x00072520
			private TextObject QuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=Ba9nsfHc}{QUEST_GIVER.LINK} shared their plan for robbing {TARGET_MERCHANT.LINK} with you. You agreed to talk with {TARGET_MERCHANT.LINK} to convince {?TARGET_MERCHANT.GENDER}her{?}him{\\?} to guard {?TARGET_MERCHANT.GENDER}her{?}his{\\?} caravan and lead the caravan to ambush around {TARGET_SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700020E RID: 526
			// (get) Token: 0x06001267 RID: 4711 RVA: 0x0007437C File Offset: 0x0007257C
			private TextObject Success1LogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bblwaDi1}You have successfully robbed {TARGET_MERCHANT.LINK}'s caravan with {QUEST_GIVER.LINK}.", null);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06001268 RID: 4712 RVA: 0x000743C4 File Offset: 0x000725C4
			private TextObject SidedWithGangLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=lZjj3MZg}When {QUEST_GIVER.LINK} arrived, you kept your side of the bargain and attacked the caravan", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000210 RID: 528
			// (get) Token: 0x06001269 RID: 4713 RVA: 0x000743F8 File Offset: 0x000725F8
			private TextObject TimedOutWithoutTalkingToMerchantText
			{
				get
				{
					TextObject textObject = new TextObject("{=OMKgidoP}You have failed to convince the merchant to guard {?TARGET_MERCHANT.GENDER}her{?}his{\\?} caravan in time. {QUEST_GIVER.LINK} must be furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x0600126A RID: 4714 RVA: 0x0007443D File Offset: 0x0007263D
			private TextObject Fail1LogText
			{
				get
				{
					return new TextObject("{=DRpcqEMI}The caravan leader said your decisions were wasting their time and decided to go on his way. You have failed to uphold your part in the plan.", null);
				}
			}

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x0600126B RID: 4715 RVA: 0x0007444A File Offset: 0x0007264A
			private TextObject Fail2LogText
			{
				get
				{
					return new TextObject("{=EFjas6hI}At the last moment, you decided to side with the caravan guard and defend them.", null);
				}
			}

			// Token: 0x17000213 RID: 531
			// (get) Token: 0x0600126C RID: 4716 RVA: 0x00074458 File Offset: 0x00072658
			private TextObject Fail2OutcomeLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=JgrG0uoO}Having the {TARGET_MERCHANT.LINK} by your side, you were successful in protecting the caravan.", null);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x0600126D RID: 4717 RVA: 0x00074485 File Offset: 0x00072685
			private TextObject Fail3LogText
			{
				get
				{
					return new TextObject("{=0NxiTi8b}You didn't feel like splitting the loot, so you betrayed both the merchant and the gang leader.", null);
				}
			}

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x0600126E RID: 4718 RVA: 0x00074492 File Offset: 0x00072692
			private TextObject Fail3OutcomeLogText
			{
				get
				{
					return new TextObject("{=KbMew14D}Although the gang leader and the caravaneer joined their forces, you have successfully defeated them and kept the loot for yourself.", null);
				}
			}

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x0600126F RID: 4719 RVA: 0x000744A0 File Offset: 0x000726A0
			private TextObject Fail4LogText
			{
				get
				{
					TextObject textObject = new TextObject("{=22nahm29}You have lost the battle against the merchant's caravan and failed to help {QUEST_GIVER.LINK}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06001270 RID: 4720 RVA: 0x000744D4 File Offset: 0x000726D4
			private TextObject Fail5LogText
			{
				get
				{
					TextObject textObject = new TextObject("{=QEgzLRnC}You have lost the battle against {QUEST_GIVER.LINK} and failed to help the merchant as you promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x06001271 RID: 4721 RVA: 0x00074508 File Offset: 0x00072708
			private TextObject Fail6LogText
			{
				get
				{
					TextObject textObject = new TextObject("{=pGu2mcar}You have lost the battle against the combined forces of the {QUEST_GIVER.LINK} and the caravan.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x06001272 RID: 4722 RVA: 0x0007453A File Offset: 0x0007273A
			private TextObject PlayerCapturedQuestSettlementLogText
			{
				get
				{
					return new TextObject("{=gPFfHluf}Your clan is now owner of the settlement. As the lord of the settlement you cannot be part of the criminal activities anymore. Your agreement with the questgiver has canceled.", null);
				}
			}

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06001273 RID: 4723 RVA: 0x00074548 File Offset: 0x00072748
			private TextObject QuestSettlementWasCapturedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=uVigJ3LP}{QUEST_GIVER.LINK} has lost the control of {SETTLEMENT} and the deal is now invalid.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06001274 RID: 4724 RVA: 0x00074598 File Offset: 0x00072798
			private TextObject WarDeclaredBetweenPlayerAndQuestGiverLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=ojpW4WRD}Your clan is now at war with the {QUEST_GIVER.LINK}'s lord. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x06001275 RID: 4725 RVA: 0x000745CC File Offset: 0x000727CC
			private TextObject TargetSettlementRaidedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=QkbkesNJ}{QUEST_GIVER.LINK} called off the ambush after {TARGET_SETTLEMENT} was raided.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x06001276 RID: 4726 RVA: 0x00074618 File Offset: 0x00072818
			private TextObject TalkedToMerchantLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=N1ZiaLRL}You talked to {TARGET_MERCHANT.LINK} as {QUEST_GIVER.LINK} asked. The caravan is waiting for you outside the gates to be escorted to {TARGET_SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x06001277 RID: 4727 RVA: 0x00074674 File Offset: 0x00072874
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetEncounterDialogue(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogueWithMerchant(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogueWithCaravan(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogueWithGangWithoutCaravan(), this);
			}

			// Token: 0x06001278 RID: 4728 RVA: 0x000746E0 File Offset: 0x000728E0
			private Settlement GetTargetSettlement()
			{
				MapDistanceModel model = Campaign.Current.Models.MapDistanceModel;
				return Settlement.All.Where<Settlement>((Settlement t) => t != this.QuestGiver.CurrentSettlement && t.IsTown).MinBy<Settlement, float>((Settlement t) => model.GetDistance(t, this.QuestGiver.CurrentSettlement, false, false, MobileParty.NavigationType.Default)).BoundVillages.GetRandomElement<Village>().Settlement;
			}

			// Token: 0x06001279 RID: 4729 RVA: 0x00074748 File Offset: 0x00072948
			protected override void SetDialogs()
			{
				TextObject discussIntroDialogue = new TextObject("{=lOFR5sq6}Have you talked with {TARGET_MERCHANT.NAME}? It would be a damned waste if we waited too long and word of our plans leaked out.", null);
				TextObject textObject = new TextObject("{=cc4EEDMg}Splendid. Go have a word with {TARGET_MERCHANT.LINK}. [if:convo_focused_happy]If you can convince {?TARGET_MERCHANT.GENDER}her{?}him{\\?} to guide the caravan, we will wait in ambush along their route.", null);
				StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, textObject, false);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(textObject, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == this.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.OnQuestAccepted))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(discussIntroDialogue, null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, discussIntroDialogue, false);
					return Hero.OneToOneConversationHero == this.QuestGiver;
				})
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=YuabHAbV}I'll take care of it shortly..", null, null, null)
					.NpcLine("{=CDXUehf0}Good, good.", null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=2haJj9mp}I have but I need to deal with some other problems before leading the caravan.", null, null, null)
					.NpcLine("{=bSDIHQzO}Please do so. Hate to have word leak out.[if:convo_nervous]", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600127A RID: 4730 RVA: 0x00074858 File Offset: 0x00072A58
			private DialogFlow GetDialogueWithMerchant()
			{
				TextObject textObject = new TextObject("{=OJtUNAbN}Very well. You'll find the caravan [if:convo_calm_friendly]getting ready outside the gates. You will get your payment after the job. Good luck, friend.", null);
				return DialogFlow.CreateDialogFlow("hero_main_options", 125).BeginPlayerOptions(null, false).PlayerOption(new TextObject("{=K1ICRis9}I have heard you are looking for extra swords to protect your caravan. I am here to offer my services.", null), null, null, null)
					.Condition(() => Hero.OneToOneConversationHero == this._targetMerchantCharacter.HeroObject && this._caravanParty == null)
					.NpcLine("{=ltbu3S63}Yes, you have heard correctly. I am looking for a capable [if:convo_astonished]leader with a good number of followers. You only need to escort the caravan until they reach {TARGET_SETTLEMENT}. A simple job, but the cargo is very important. I'm willing to pay {MERCHANT_REWARD} denars. And of course, if you betrayed me...", null, null, null, null)
					.Condition(delegate
					{
						MBTextManager.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName, false);
						MBTextManager.SetTextVariable("MERCHANT_REWARD", this.Reward2);
						return true;
					})
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.SpawnQuestParties))
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=AGnd7nDb}Worry not. The outlaws in these parts know my name well, and fear it.", null, null, null)
					.NpcLine(textObject, null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=RCsbpizl}If you have the denars we'll do the job.", null, null, null)
					.NpcLine(textObject, null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=TfDomerj}I think my men and I are more than enough to protect the caravan, good {?TARGET_MERCHANT.GENDER}madam{?}sir{\\?}.", null, null, null)
					.Condition(delegate
					{
						StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, null, false);
						return true;
					})
					.NpcLine(textObject, null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600127B RID: 4731 RVA: 0x00074954 File Offset: 0x00072B54
			private DialogFlow GetDialogueWithCaravan()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=Xs7Qweuw}Lead the way, {PLAYER.NAME}.", null, null, null, null).Condition(() => MobileParty.ConversationParty == this._caravanParty && this._caravanParty != null && !this._canEncounterConversationStart)
					.Consequence(delegate
					{
						PlayerEncounter.LeaveEncounter = true;
					})
					.CloseDialog();
			}

			// Token: 0x0600127C RID: 4732 RVA: 0x000749B8 File Offset: 0x00072BB8
			private DialogFlow GetDialogueWithGangWithoutCaravan()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=F44s8kPB}Where is the caravan? My men can't wait here for too long.[if:convo_undecided_open]", null, null, null, null).Condition(() => MobileParty.ConversationParty == this._gangParty && this._gangParty != null && !this._canEncounterConversationStart)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=Yqv1jk7D}Don't worry, they are coming towards our trap.", null, null, null)
					.NpcLine("{=fHc6fwrb}Good, let's finish this.", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600127D RID: 4733 RVA: 0x00074A24 File Offset: 0x00072C24
			private DialogFlow GetEncounterDialogue()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=vVH7wT07}Who are these men? Be on your guard {PLAYER.NAME}, I smell trouble![if:convo_confused_annoyed]", null, null, null, null).Condition(() => MobileParty.ConversationParty == this._caravanParty && this._caravanParty != null && this._canEncounterConversationStart)
					.Consequence(delegate
					{
						StringHelpers.SetCharacterProperties("TARGET_MERCHANT", this._targetMerchantCharacter, null, false);
						AgentBuildData agentBuildData = new AgentBuildData(ConversationHelper.GetConversationCharacterPartyLeader(this._gangParty.Party));
						agentBuildData.TroopOrigin(new SimpleAgentOrigin(agentBuildData.AgentCharacter, -1, null, default(UniqueTroopDescriptor)));
						Vec3 vec = Agent.Main.LookDirection * 10f;
						vec.RotateAboutZ(1.3962634f);
						AgentBuildData agentBuildData2 = agentBuildData;
						Vec3 vec2 = Agent.Main.Position + vec;
						agentBuildData2.InitialPosition(in vec2);
						AgentBuildData agentBuildData3 = agentBuildData;
						vec2 = Agent.Main.LookDirection;
						Vec2 vec3 = vec2.AsVec2;
						vec3 = -vec3.Normalized();
						agentBuildData3.InitialDirection(in vec3);
						Agent agent = Mission.Current.SpawnAgent(agentBuildData, false);
						Campaign.Current.ConversationManager.AddConversationAgents(new List<IAgent> { agent }, true);
					})
					.NpcLine("{=LJ2AoQyS}Well, well. What do we have here? Must be one of our lucky days, [if:convo_huge_smile]huh? Release all the valuables you carry and nobody gets hurt.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), null, null)
					.NpcLine("{=SdgDF4OZ}Hah! You're making a big mistake. See that group of men over there, [if:convo_excited]led by the warrior {PLAYER.NAME}? They're with us, and they'll cut you open.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), null, null)
					.NpcLine("{=LaHWB3r0}Oh… I'm afraid there's been a misunderstanding. {PLAYER.NAME} is with us, you see.[if:convo_evil_smile] Did {TARGET_MERCHANT.LINK} stuff you with lies and then send you out to your doom? Oh, shameful, shameful. {?TARGET_MERCHANT.GENDER}She{?}He{\\?} does that fairly often, unfortunately.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), null, null)
					.NpcLine("{=EGC4BA4h}{PLAYER.NAME}! Is this true? Look, you're a smart {?PLAYER.GENDER}woman{?}man{\\?}. [if:convo_shocked]You know that {TARGET_MERCHANT.LINK} can pay more than these scum. Take the money and keep your reputation.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.NpcLine("{=zUKqWeUa}Come on, {PLAYER.NAME}. All this back-and-forth  is making me anxious. Let's finish this.[if:convo_nervous]", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=UEY5aQ2l}I'm here to rob {TARGET_MERCHANT.NAME}, not be {?TARGET_MERCHANT.GENDER}her{?}his{\\?} lackey. Now, cough up the goods or fight.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), null, null)
					.NpcLine("{=tHUHfe6C}You're with them? This is the basest treachery I have ever witnessed![if:convo_furious]", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						base.AddLog(this.SidedWithGangLogText, false);
					})
					.NpcLine("{=IKeZLbIK}No offense, captain, but if that's the case you need to get out more. [if:convo_mocking_teasing]Anyway, shall we go to it?", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						this.StartBattle(SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithGang);
					})
					.CloseDialog()
					.PlayerOption("{=W7TD4yTc}You know, {TARGET_MERCHANT.NAME}'s man makes a good point. I'm guarding this caravan.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), null, null)
					.NpcLine("{=VXp0R7da}Heaven protect you! I knew you'd never be tempted by such a perfidious offer.[if:convo_huge_smile]", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						base.AddLog(this.Fail2LogText, false);
					})
					.NpcLine("{=XJOqws2b}Hmf. A funny sense of honor you have… Anyway, I'm not going home empty handed, so let's do this.[if:convo_furious]", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						this.StartBattle(SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithCaravan);
					})
					.CloseDialog()
					.PlayerOption("{=ILrYPvTV}You know, I think I'd prefer to take all the loot for myself.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), null, null)
					.NpcLine("{=cpTMttNb}Is that so? Hey, caravan captain, whatever your name is… [if:convo_contemptuous]As long as we're all switching sides here, how about I join with you to defeat this miscreant who just betrayed both of us? Whichever of us comes out of this with the most men standing keeps your goods.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsGangPartyLeader), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						base.AddLog(this.Fail3LogText, false);
					})
					.NpcLine("{=15UCTrNA}I have no choice, do I? Well, better an honest robber than a traitor![if:convo_aggressive] Let's take {?PLAYER.GENDER}her{?}him{\\?} down.", new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCaravanMaster), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						this.StartBattle(SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.BetrayedBoth);
					})
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600127E RID: 4734 RVA: 0x00074CC8 File Offset: 0x00072EC8
			private void OnQuestAccepted()
			{
				base.StartQuest();
				base.AddLog(this.QuestStartedLogText, false);
				base.AddTrackedObject(this._targetMerchantCharacter.HeroObject);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetEncounterDialogue(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogueWithMerchant(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogueWithCaravan(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogueWithGangWithoutCaravan(), this);
			}

			// Token: 0x0600127F RID: 4735 RVA: 0x00074D54 File Offset: 0x00072F54
			public void GetMountAndHarnessVisualIdsForQuestCaravan(CultureObject culture, out string mountStringId, out string harnessStringId)
			{
				if (culture.StringId == "khuzait" || culture.StringId == "aserai")
				{
					mountStringId = "camel";
					harnessStringId = "camel_saddle_b";
					return;
				}
				mountStringId = "mule";
				harnessStringId = "mule_load_c";
			}

			// Token: 0x06001280 RID: 4736 RVA: 0x00074DA4 File Offset: 0x00072FA4
			private void SpawnQuestParties()
			{
				TextObject textObject = GameTexts.FindText("str_caravan_party_name", null);
				textObject.SetCharacterProperties("OWNER", this._targetMerchantCharacter, false);
				string text;
				string text2;
				this.GetMountAndHarnessVisualIdsForQuestCaravan(this._targetMerchantCharacter.Culture, out text, out text2);
				PartyTemplateObject randomCaravanTemplate = CaravanHelper.GetRandomCaravanTemplate(this._targetMerchantCharacter.Culture, false, true);
				this._caravanParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._targetMerchantCharacter.HeroObject.CurrentSettlement.GatePosition, 0.1f, this._targetMerchantCharacter.HeroObject.CurrentSettlement, textObject, this._targetMerchantCharacter.HeroObject.Clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), this._targetMerchantCharacter.HeroObject, text, text2, MobileParty.MainParty.Speed, false);
				MobilePartyHelper.FillPartyManuallyAfterCreation(this._caravanParty, randomCaravanTemplate, this.CaravanPartyTroopCount);
				this._caravanParty.MemberRoster.AddToCounts(this._targetMerchantCharacter.Culture.CaravanMaster, 1, false, 0, 0, true, -1);
				this._caravanParty.ItemRoster.AddToCounts(Game.Current.ObjectManager.GetObject<ItemObject>("grain"), 40);
				this._caravanParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
				SetPartyAiAction.GetActionForEscortingParty(this._caravanParty, MobileParty.MainParty, MobileParty.NavigationType.Default, false, false);
				this._caravanParty.Ai.SetDoNotMakeNewDecisions(true);
				this._caravanParty.SetPartyUsedByQuest(true);
				base.AddTrackedObject(this._caravanParty);
				MobilePartyHelper.TryMatchPartySpeedWithItemWeight(this._caravanParty, MobileParty.MainParty.Speed * 1.5f, null);
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.Default, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				CampaignVec2 gatePosition = this._targetSettlement.GatePosition;
				PartyTemplateObject partyTemplateObject = Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("kingdom_hero_party_caravan_ambushers") ?? base.QuestGiver.Culture.BanditBossPartyTemplate;
				this._gangParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(gatePosition, 0.1f, this._targetSettlement, new TextObject("{=gJNdkwHV}Gang Party", null), null, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), base.QuestGiver, "", "", 0f, false);
				MobilePartyHelper.FillPartyManuallyAfterCreation(this._gangParty, partyTemplateObject, this.GangPartyTroopCount);
				this._gangParty.MemberRoster.AddToCounts(clan.Culture.BanditBoss, 1, true, 0, 0, true, -1);
				this._gangParty.ItemRoster.AddToCounts(Game.Current.ObjectManager.GetObject<ItemObject>("grain"), 40);
				this._gangParty.SetPartyUsedByQuest(true);
				this._gangParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
				this._gangParty.Ai.SetDoNotMakeNewDecisions(true);
				this._gangParty.Ai.DisableAi();
				MobilePartyHelper.TryMatchPartySpeedWithItemWeight(this._gangParty, 0.2f, null);
				this._gangParty.SetMoveGoToSettlement(this._targetSettlement, MobileParty.NavigationType.Default, false);
				EnterSettlementAction.ApplyForParty(this._gangParty, this._targetSettlement);
				base.AddTrackedObject(this._targetSettlement);
				base.AddLog(this.TalkedToMerchantLogText, false);
			}

			// Token: 0x06001281 RID: 4737 RVA: 0x000750E0 File Offset: 0x000732E0
			private void StartBattle(SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice playerChoice)
			{
				this._playerChoice = playerChoice;
				if (this._caravanParty.MapEvent != null)
				{
					this._caravanParty.MapEvent.FinalizeEvent();
				}
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.Default, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				Clan clan2 = ((playerChoice != SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithCaravan) ? clan : this._caravanParty.Owner.SupporterOf);
				this._caravanParty.ActualClan = clan2;
				Clan clan3 = ((playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithGang) ? base.QuestGiver.SupporterOf : clan);
				this._gangParty.ActualClan = clan3;
				PartyBase partyBase = ((playerChoice != SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithGang) ? this._gangParty.Party : this._caravanParty.Party);
				PlayerEncounter.Start();
				PlayerEncounter.Current.SetupFields(partyBase, PartyBase.MainParty);
				PlayerEncounter.StartBattle();
				if (playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.BetrayedBoth)
				{
					this._caravanParty.MapEventSide = this._gangParty.MapEventSide;
					return;
				}
				if (playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithCaravan)
				{
					this._caravanParty.MapEventSide = PartyBase.MainParty.MapEventSide;
					return;
				}
				this._gangParty.MapEventSide = PartyBase.MainParty.MapEventSide;
			}

			// Token: 0x06001282 RID: 4738 RVA: 0x00075228 File Offset: 0x00073428
			private void StartEncounterDialogue()
			{
				if (this._gangParty.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForParty(this._gangParty);
				}
				PlayerEncounter.Finish(true);
				this._canEncounterConversationStart = true;
				ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, true, false, false, false, false, false);
				ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(this._caravanParty.Party), this._caravanParty.Party, true, false, false, false, false, true);
				CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
			}

			// Token: 0x06001283 RID: 4739 RVA: 0x000752AC File Offset: 0x000734AC
			private void StartDialogueWithoutCaravan()
			{
				PlayerEncounter.Finish(true);
				ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, true, false, false, false, false, false);
				ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(this._gangParty.Party), this._gangParty.Party, true, false, false, false, false, false);
				CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
			}

			// Token: 0x06001284 RID: 4740 RVA: 0x00075310 File Offset: 0x00073510
			protected override void HourlyTick()
			{
				if (this._caravanParty != null)
				{
					if (this._caravanParty.DefaultBehavior != AiBehavior.EscortParty || this._caravanParty.ShortTermBehavior != AiBehavior.EscortParty)
					{
						SetPartyAiAction.GetActionForEscortingParty(this._caravanParty, MobileParty.MainParty, MobileParty.NavigationType.Default, false, false);
					}
					(this._caravanParty.PartyComponent as CustomPartyComponent).CustomPartyBaseSpeed = MobileParty.MainParty.Speed;
					if (MobileParty.MainParty.TargetParty == this._caravanParty)
					{
						this._caravanParty.SetMoveModeHold();
						this._isCaravanFollowing = false;
						return;
					}
					if (!this._isCaravanFollowing)
					{
						SetPartyAiAction.GetActionForEscortingParty(this._caravanParty, MobileParty.MainParty, MobileParty.NavigationType.Default, false, false);
						this._isCaravanFollowing = true;
					}
				}
			}

			// Token: 0x06001285 RID: 4741 RVA: 0x000753BF File Offset: 0x000735BF
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == base.QuestGiver.CurrentSettlement)
				{
					if (newOwner.Clan == Clan.PlayerClan)
					{
						this.OnCancel4();
						return;
					}
					this.OnCancel2();
				}
			}

			// Token: 0x06001286 RID: 4742 RVA: 0x000753E9 File Offset: 0x000735E9
			public void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail reason)
			{
				if ((faction1 == base.QuestGiver.MapFaction && faction2 == Hero.MainHero.MapFaction) || (faction2 == base.QuestGiver.MapFaction && faction1 == Hero.MainHero.MapFaction))
				{
					this.OnCancel1();
				}
			}

			// Token: 0x06001287 RID: 4743 RVA: 0x00075427 File Offset: 0x00073627
			public void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
			{
				if (village == this._targetSettlement.Village && newState != Village.VillageStates.Normal)
				{
					this.OnCancel3();
				}
			}

			// Token: 0x06001288 RID: 4744 RVA: 0x00075440 File Offset: 0x00073640
			public void OnMapEventEnded(MapEvent mapEvent)
			{
				if (mapEvent.IsPlayerMapEvent && this._caravanParty != null)
				{
					if (mapEvent.InvolvedParties.Contains(this._caravanParty.Party))
					{
						if (!mapEvent.InvolvedParties.Contains(this._gangParty.Party))
						{
							this.OnFail1();
							return;
						}
						if (mapEvent.WinningSide == mapEvent.PlayerSide)
						{
							if (this._playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithGang)
							{
								this.OnSuccess1();
								return;
							}
							if (this._playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithCaravan)
							{
								this.OnFail2();
								return;
							}
							this.OnFail3();
							return;
						}
						else
						{
							if (this._playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithGang)
							{
								this.OnFail4();
								return;
							}
							if (this._playerChoice == SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.SidedWithCaravan)
							{
								this.OnFail5();
								return;
							}
							this.OnFail6();
							return;
						}
					}
					else
					{
						this.OnFail1();
					}
				}
			}

			// Token: 0x06001289 RID: 4745 RVA: 0x000754FC File Offset: 0x000736FC
			private void OnPartyJoinedArmy(MobileParty mobileParty)
			{
				if (mobileParty == MobileParty.MainParty && this._caravanParty != null)
				{
					this.OnFail1();
				}
			}

			// Token: 0x0600128A RID: 4746 RVA: 0x00075514 File Offset: 0x00073714
			private void OnGameMenuOpened(MenuCallbackArgs args)
			{
				if (this._startConversationDelegate != null && MobileParty.MainParty.CurrentSettlement == this._targetSettlement && this._caravanParty != null)
				{
					this._startConversationDelegate();
					this._startConversationDelegate = null;
				}
			}

			// Token: 0x0600128B RID: 4747 RVA: 0x0007554C File Offset: 0x0007374C
			public void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
			{
				if (party == MobileParty.MainParty && settlement == this._targetSettlement && this._caravanParty != null)
				{
					if (this._caravanParty.Position.DistanceSquared(this._targetSettlement.Position) <= this.CaravanEncounterStartDistance)
					{
						this._startConversationDelegate = new SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.QuestEndDelegate(this.StartEncounterDialogue);
						return;
					}
					this._startConversationDelegate = new SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.QuestEndDelegate(this.StartDialogueWithoutCaravan);
				}
			}

			// Token: 0x0600128C RID: 4748 RVA: 0x000755BD File Offset: 0x000737BD
			public void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party == MobileParty.MainParty && this._caravanParty != null)
				{
					SetPartyAiAction.GetActionForEscortingParty(this._caravanParty, MobileParty.MainParty, MobileParty.NavigationType.Default, false, false);
				}
			}

			// Token: 0x0600128D RID: 4749 RVA: 0x000755E2 File Offset: 0x000737E2
			private void CanHeroBecomePrisoner(Hero hero, ref bool result)
			{
				if (hero == Hero.MainHero && this._playerChoice != SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice.None)
				{
					result = false;
				}
			}

			// Token: 0x0600128E RID: 4750 RVA: 0x000755F8 File Offset: 0x000737F8
			protected override void OnFinalize()
			{
				if (this._caravanParty != null && this._caravanParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._caravanParty);
				}
				if (this._gangParty != null && this._gangParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._gangParty);
				}
			}

			// Token: 0x0600128F RID: 4751 RVA: 0x00075648 File Offset: 0x00073848
			private void OnSuccess1()
			{
				base.AddLog(this.Success1LogText, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				TraitLevelingHelper.OnIssueSolvedThroughQuest(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 50)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, 5, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -10, true, true);
				base.QuestGiver.AddPower(30f);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.Reward1, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06001290 RID: 4752 RVA: 0x000756EB File Offset: 0x000738EB
			private void OnTimedOutWithoutTalkingToMerchant()
			{
				base.AddLog(this.TimedOutWithoutTalkingToMerchantText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
			}

			// Token: 0x06001291 RID: 4753 RVA: 0x00075729 File Offset: 0x00073929
			private void OnFail1()
			{
				this.ApplyFail1Consequences();
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06001292 RID: 4754 RVA: 0x00075738 File Offset: 0x00073938
			private void ApplyFail1Consequences()
			{
				base.AddLog(this.Fail1LogText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -5, true, true);
			}

			// Token: 0x06001293 RID: 4755 RVA: 0x00075798 File Offset: 0x00073998
			private void OnFail2()
			{
				base.AddLog(this.Fail2OutcomeLogText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 100)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -10, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, 5, true, true);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.Reward2, false);
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x06001294 RID: 4756 RVA: 0x00075810 File Offset: 0x00073A10
			private void OnFail3()
			{
				base.AddLog(this.Fail3OutcomeLogText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -200)
				});
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 100)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -15, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -20, true, true);
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x06001295 RID: 4757 RVA: 0x00075898 File Offset: 0x00073A98
			private void OnFail4()
			{
				base.AddLog(this.Fail4LogText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 100)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -10, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -10, true, true);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06001296 RID: 4758 RVA: 0x0007591C File Offset: 0x00073B1C
			private void OnFail5()
			{
				base.AddLog(this.Fail5LogText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 100)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -10, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -10, true, true);
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x06001297 RID: 4759 RVA: 0x000759A0 File Offset: 0x00073BA0
			private void OnFail6()
			{
				base.AddLog(this.Fail6LogText, false);
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -200)
				});
				TraitLevelingHelper.OnIssueFailed(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Calculating, 100)
				});
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -15, true, true);
				ChangeRelationAction.ApplyPlayerRelation(this._targetMerchantCharacter.HeroObject, -20, true, true);
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x06001298 RID: 4760 RVA: 0x00075A26 File Offset: 0x00073C26
			protected override void OnTimedOut()
			{
				if (this._caravanParty == null)
				{
					this.OnTimedOutWithoutTalkingToMerchant();
					return;
				}
				this.ApplyFail1Consequences();
			}

			// Token: 0x06001299 RID: 4761 RVA: 0x00075A3D File Offset: 0x00073C3D
			private void OnCancel1()
			{
				base.AddLog(this.WarDeclaredBetweenPlayerAndQuestGiverLogText, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x0600129A RID: 4762 RVA: 0x00075A54 File Offset: 0x00073C54
			private void OnCancel2()
			{
				base.AddLog(this.QuestSettlementWasCapturedLogText, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x0600129B RID: 4763 RVA: 0x00075A6B File Offset: 0x00073C6B
			private void OnCancel3()
			{
				base.AddLog(this.TargetSettlementRaidedLogText, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x0600129C RID: 4764 RVA: 0x00075A82 File Offset: 0x00073C82
			private void OnCancel4()
			{
				base.AddLog(this.PlayerCapturedQuestSettlementLogText, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x0600129D RID: 4765 RVA: 0x00075A99 File Offset: 0x00073C99
			private bool IsGangPartyLeader(IAgent agent)
			{
				return agent.Character == ConversationHelper.GetConversationCharacterPartyLeader(this._gangParty.Party);
			}

			// Token: 0x0600129E RID: 4766 RVA: 0x00075AB3 File Offset: 0x00073CB3
			private bool IsCaravanMaster(IAgent agent)
			{
				return agent.Character == ConversationHelper.GetConversationCharacterPartyLeader(this._caravanParty.Party);
			}

			// Token: 0x0600129F RID: 4767 RVA: 0x00075ACD File Offset: 0x00073CCD
			private bool IsMainHero(IAgent agent)
			{
				return agent.Character == CharacterObject.PlayerCharacter;
			}

			// Token: 0x060012A0 RID: 4768 RVA: 0x00075ADC File Offset: 0x00073CDC
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._targetMerchantCharacter.HeroObject)
				{
					result = false;
				}
			}

			// Token: 0x060012A1 RID: 4769 RVA: 0x00075AF0 File Offset: 0x00073CF0
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.VillageStateChanged.AddNonSerializedListener(this, new Action<Village, Village.VillageStates, Village.VillageStates, MobileParty>(this.OnVillageStateChanged));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.OnPartyJoinedArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyJoinedArmy));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.CanHeroBecomePrisonerEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanHeroBecomePrisoner));
				CampaignEvents.CanHaveCampaignIssuesEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.OnHeroCanHaveCampaignIssuesInfoIsRequested));
			}

			// Token: 0x060012A2 RID: 4770 RVA: 0x00075BE4 File Offset: 0x00073DE4
			internal static void AutoGeneratedStaticCollectObjectsSnareTheWealthyIssueQuest(object o, List<object> collectedObjects)
			{
				((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060012A3 RID: 4771 RVA: 0x00075BF2 File Offset: 0x00073DF2
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetMerchantCharacter);
				collectedObjects.Add(this._targetSettlement);
				collectedObjects.Add(this._caravanParty);
				collectedObjects.Add(this._gangParty);
			}

			// Token: 0x060012A4 RID: 4772 RVA: 0x00075C2B File Offset: 0x00073E2B
			internal static object AutoGeneratedGetMemberValue_targetMerchantCharacter(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._targetMerchantCharacter;
			}

			// Token: 0x060012A5 RID: 4773 RVA: 0x00075C38 File Offset: 0x00073E38
			internal static object AutoGeneratedGetMemberValue_targetSettlement(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._targetSettlement;
			}

			// Token: 0x060012A6 RID: 4774 RVA: 0x00075C45 File Offset: 0x00073E45
			internal static object AutoGeneratedGetMemberValue_caravanParty(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._caravanParty;
			}

			// Token: 0x060012A7 RID: 4775 RVA: 0x00075C52 File Offset: 0x00073E52
			internal static object AutoGeneratedGetMemberValue_gangParty(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._gangParty;
			}

			// Token: 0x060012A8 RID: 4776 RVA: 0x00075C5F File Offset: 0x00073E5F
			internal static object AutoGeneratedGetMemberValue_questDifficulty(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._questDifficulty;
			}

			// Token: 0x060012A9 RID: 4777 RVA: 0x00075C71 File Offset: 0x00073E71
			internal static object AutoGeneratedGetMemberValue_playerChoice(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._playerChoice;
			}

			// Token: 0x060012AA RID: 4778 RVA: 0x00075C83 File Offset: 0x00073E83
			internal static object AutoGeneratedGetMemberValue_canEncounterConversationStart(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._canEncounterConversationStart;
			}

			// Token: 0x060012AB RID: 4779 RVA: 0x00075C95 File Offset: 0x00073E95
			internal static object AutoGeneratedGetMemberValue_isCaravanFollowing(object o)
			{
				return ((SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest)o)._isCaravanFollowing;
			}

			// Token: 0x040008B8 RID: 2232
			private SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.QuestEndDelegate _startConversationDelegate;

			// Token: 0x040008B9 RID: 2233
			[SaveableField(1)]
			private CharacterObject _targetMerchantCharacter;

			// Token: 0x040008BA RID: 2234
			[SaveableField(2)]
			private Settlement _targetSettlement;

			// Token: 0x040008BB RID: 2235
			[SaveableField(3)]
			private MobileParty _caravanParty;

			// Token: 0x040008BC RID: 2236
			[SaveableField(4)]
			private MobileParty _gangParty;

			// Token: 0x040008BD RID: 2237
			[SaveableField(5)]
			private readonly float _questDifficulty;

			// Token: 0x040008BE RID: 2238
			[SaveableField(6)]
			private SnareTheWealthyIssueBehavior.SnareTheWealthyIssueQuest.SnareTheWealthyQuestChoice _playerChoice;

			// Token: 0x040008BF RID: 2239
			[SaveableField(7)]
			private bool _canEncounterConversationStart;

			// Token: 0x040008C0 RID: 2240
			[SaveableField(8)]
			private bool _isCaravanFollowing = true;

			// Token: 0x02000249 RID: 585
			internal enum SnareTheWealthyQuestChoice
			{
				// Token: 0x04000A21 RID: 2593
				None,
				// Token: 0x04000A22 RID: 2594
				SidedWithCaravan,
				// Token: 0x04000A23 RID: 2595
				SidedWithGang,
				// Token: 0x04000A24 RID: 2596
				BetrayedBoth
			}

			// Token: 0x0200024A RID: 586
			// (Invoke) Token: 0x06001481 RID: 5249
			private delegate void QuestEndDelegate();
		}
	}
}
