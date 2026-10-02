using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000378 RID: 888
	public class ScoutEnemyGarrisonsIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600343C RID: 13372 RVA: 0x000D7CD1 File Offset: 0x000D5ED1
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000D7CEC File Offset: 0x000D5EEC
		public void OnCheckForIssue(Hero hero)
		{
			List<Settlement> list;
			if (this.ConditionsHold(hero, out list))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), IssueBase.IssueFrequency.VeryCommon, list));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000D7D54 File Offset: 0x000D5F54
		private bool ConditionsHold(Hero issueGiver, out List<Settlement> settlements)
		{
			settlements = new List<Settlement>();
			if (issueGiver.MapFaction.IsKingdomFaction && issueGiver.IsFactionLeader && !issueGiver.IsMinorFactionHero && !issueGiver.IsPrisoner && !issueGiver.IsFugitive)
			{
				Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => x.IsAtWarWith(issueGiver.MapFaction));
				MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
				IMapPoint mapPoint = issueGiver.GetMapPoint();
				float num = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) * 5f;
				if (randomElementWithPredicate != null && mapPoint != null)
				{
					ValueTuple<Settlement, float>[] array = new ValueTuple<Settlement, float>[3];
					foreach (Settlement settlement in randomElementWithPredicate.Settlements)
					{
						if (ScoutEnemyGarrisonsIssueBehavior.SuitableSettlementCondition(settlement, issueGiver))
						{
							float num2 = float.MaxValue;
							if (issueGiver.CurrentSettlement != null)
							{
								num2 = mapDistanceModel.GetDistance(issueGiver.CurrentSettlement, settlement, false, false, MobileParty.NavigationType.All);
							}
							else if (issueGiver.PartyBelongedTo != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(issueGiver.PartyBelongedTo, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							else if (issueGiver.PartyBelongedToAsPrisoner != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(issueGiver.PartyBelongedToAsPrisoner.MobileParty, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							if (num2 <= num)
							{
								if (array[2].Item1 == null || array[2].Item2 > num2)
								{
									array[2] = new ValueTuple<Settlement, float>(settlement, num2);
								}
								int num4 = array.Length - 1;
								while (num4 > 0 && (array[num4 - 1].Item1 == null || array[num4].Item2 < array[num4 - 1].Item2))
								{
									ValueTuple<Settlement, float> valueTuple = array[num4 - 1];
									array[num4 - 1] = array[num4];
									array[num4] = valueTuple;
									num4--;
								}
							}
						}
					}
					if (array[2].Item1 != null)
					{
						settlements.Add(array[2].Item1);
						settlements.Add(array[1].Item1);
						settlements.Add(array[0].Item1);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000D8008 File Offset: 0x000D6208
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue(issueOwner, potentialIssueData.RelatedObject as List<Settlement>);
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000D802E File Offset: 0x000D622E
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000D8030 File Offset: 0x000D6230
		private static bool SuitableSettlementCondition(Settlement settlement, Hero issueGiver)
		{
			return settlement.IsFortification && settlement.MapFaction.IsAtWarWith(issueGiver.MapFaction) && (!settlement.IsUnderSiege || settlement.SiegeEvent.BesiegerCamp.MapFaction != Hero.MainHero.MapFaction);
		}

		// Token: 0x04000EE8 RID: 3816
		private const IssueBase.IssueFrequency ScoutEnemyGarrisonsIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000EE9 RID: 3817
		private const int QuestDurationInDays = 30;

		// Token: 0x02000755 RID: 1877
		public class ScoutEnemyGarrisonsIssue : IssueBase
		{
			// Token: 0x06005F69 RID: 24425 RVA: 0x001B9B17 File Offset: 0x001B7D17
			internal static void AutoGeneratedStaticCollectObjectsScoutEnemyGarrisonsIssue(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005F6A RID: 24426 RVA: 0x001B9B25 File Offset: 0x001B7D25
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._settlement1);
				collectedObjects.Add(this._settlement2);
				collectedObjects.Add(this._settlement3);
			}

			// Token: 0x06005F6B RID: 24427 RVA: 0x001B9B52 File Offset: 0x001B7D52
			internal static object AutoGeneratedGetMemberValue_settlement1(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement1;
			}

			// Token: 0x06005F6C RID: 24428 RVA: 0x001B9B5F File Offset: 0x001B7D5F
			internal static object AutoGeneratedGetMemberValue_settlement2(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement2;
			}

			// Token: 0x06005F6D RID: 24429 RVA: 0x001B9B6C File Offset: 0x001B7D6C
			internal static object AutoGeneratedGetMemberValue_settlement3(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement3;
			}

			// Token: 0x1700132B RID: 4907
			// (get) Token: 0x06005F6E RID: 24430 RVA: 0x001B9B79 File Offset: 0x001B7D79
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700132C RID: 4908
			// (get) Token: 0x06005F6F RID: 24431 RVA: 0x001B9B7C File Offset: 0x001B7D7C
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700132D RID: 4909
			// (get) Token: 0x06005F70 RID: 24432 RVA: 0x001B9B7F File Offset: 0x001B7D7F
			protected override int RewardGold
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x1700132E RID: 4910
			// (get) Token: 0x06005F71 RID: 24433 RVA: 0x001B9B82 File Offset: 0x001B7D82
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=rrCkJgtd}We don't know enough about the enemy, [ib:closed][if:convo_thinking]where they are strong and where they are weak. I don't want to lead a huge army through their territory on a wild goose hunt. We need someone to ride through there swiftly, scouting out their garrisons. Can you do this?", null);
				}
			}

			// Token: 0x1700132F RID: 4911
			// (get) Token: 0x06005F72 RID: 24434 RVA: 0x001B9B90 File Offset: 0x001B7D90
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=dGakGflE}Yes, your {?QUEST_GIVER.GENDER}ladyship{?}lordship{\\?}, I'll gladly do it.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001330 RID: 4912
			// (get) Token: 0x06005F73 RID: 24435 RVA: 0x001B9BC4 File Offset: 0x001B7DC4
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=seEyGLMz}Go deep into {ENEMY} territory, to {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}. [ib:hip][if:convo_normal]I want to know every detail about them, what sort of fortifications they have, whether the walls are well-manned or undergarrisoned, and any other enemy forces in the vicinity.", null);
					textObject.SetTextVariable("ENEMY", this._settlement1.MapFaction.Name);
					textObject.SetTextVariable("SETTLEMENT_1", this._settlement1.Name);
					textObject.SetTextVariable("SETTLEMENT_2", this._settlement2.Name);
					textObject.SetTextVariable("SETTLEMENT_3", this._settlement3.Name);
					return textObject;
				}
			}

			// Token: 0x17001331 RID: 4913
			// (get) Token: 0x06005F74 RID: 24436 RVA: 0x001B9C3D File Offset: 0x001B7E3D
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=g6P6nKIf}Consider it done, commander.", null);
				}
			}

			// Token: 0x17001332 RID: 4914
			// (get) Token: 0x06005F75 RID: 24437 RVA: 0x001B9C4A File Offset: 0x001B7E4A
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=G79IzJsZ}Scout Enemy Garrisons", null);
				}
			}

			// Token: 0x17001333 RID: 4915
			// (get) Token: 0x06005F76 RID: 24438 RVA: 0x001B9C58 File Offset: 0x001B7E58
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=AdoaDR26}{QUEST_GIVER.LINK} asks you to scout {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT_1", this._settlement1.Name);
					textObject.SetTextVariable("SETTLEMENT_2", this._settlement2.Name);
					textObject.SetTextVariable("SETTLEMENT_3", this._settlement3.Name);
					return textObject;
				}
			}

			// Token: 0x06005F77 RID: 24439 RVA: 0x001B9CCF File Offset: 0x001B7ECF
			public ScoutEnemyGarrisonsIssue(Hero issueOwner, List<Settlement> settlements)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._settlement1 = settlements[0];
				this._settlement2 = settlements[1];
				this._settlement3 = settlements[2];
			}

			// Token: 0x06005F78 RID: 24440 RVA: 0x001B9D09 File Offset: 0x001B7F09
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005F79 RID: 24441 RVA: 0x001B9D0B File Offset: 0x001B7F0B
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005F7A RID: 24442 RVA: 0x001B9D0D File Offset: 0x001B7F0D
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest(questId, base.IssueOwner, this._settlement1, this._settlement2, this._settlement3);
			}

			// Token: 0x06005F7B RID: 24443 RVA: 0x001B9D2D File Offset: 0x001B7F2D
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x06005F7C RID: 24444 RVA: 0x001B9D30 File Offset: 0x001B7F30
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				relationHero = null;
				skill = null;
				requiredGold = 0;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (Hero.MainHero.IsKingdomLeader)
				{
					flag |= IssueBase.PreconditionFlags.MainHeroIsKingdomLeader;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				if (Clan.PlayerClan.Tier < 2)
				{
					flag |= IssueBase.PreconditionFlags.ClanTier;
				}
				if (Hero.MainHero.GetSkillValue(DefaultSkills.Scouting) < 30)
				{
					flag |= IssueBase.PreconditionFlags.Skill;
					skill = DefaultSkills.Scouting;
				}
				if (Hero.MainHero.MapFaction != base.IssueOwner.MapFaction)
				{
					flag |= IssueBase.PreconditionFlags.NotInSameFaction;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005F7D RID: 24445 RVA: 0x001B9DF4 File Offset: 0x001B7FF4
			public override bool IssueStayAliveConditions()
			{
				bool flag = this._settlement1.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction) && this._settlement2.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction) && this._settlement3.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction);
				if (!flag)
				{
					flag = this.TryToUpdateSettlements();
				}
				return flag && base.IssueOwner.MapFaction.IsKingdomFaction;
			}

			// Token: 0x06005F7E RID: 24446 RVA: 0x001B9E79 File Offset: 0x001B8079
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x06005F7F RID: 24447 RVA: 0x001B9E90 File Offset: 0x001B8090
			private bool TryToUpdateSettlements()
			{
				Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => x.IsAtWarWith(base.IssueOwner.MapFaction));
				MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
				IMapPoint mapPoint = base.IssueOwner.GetMapPoint();
				float num = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) * 5f;
				if (randomElementWithPredicate != null && mapPoint != null)
				{
					ValueTuple<Settlement, float>[] array = new ValueTuple<Settlement, float>[3];
					foreach (Settlement settlement in randomElementWithPredicate.Settlements)
					{
						if (ScoutEnemyGarrisonsIssueBehavior.SuitableSettlementCondition(settlement, base.IssueOwner))
						{
							float num2 = float.MaxValue;
							if (base.IssueOwner.CurrentSettlement != null)
							{
								num2 = mapDistanceModel.GetDistance(base.IssueOwner.CurrentSettlement, settlement, false, false, MobileParty.NavigationType.All);
							}
							else if (base.IssueOwner.PartyBelongedTo != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(base.IssueOwner.PartyBelongedTo, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							else if (base.IssueOwner.PartyBelongedToAsPrisoner != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(base.IssueOwner.PartyBelongedToAsPrisoner.MobileParty, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							if (num2 <= num)
							{
								if (array[2].Item1 == null || array[2].Item2 > num2)
								{
									array[2] = new ValueTuple<Settlement, float>(settlement, num2);
								}
								int num4 = array.Length - 1;
								while (num4 > 0 && (array[num4 - 1].Item1 == null || array[num4].Item2 < array[num4 - 1].Item2))
								{
									ValueTuple<Settlement, float> valueTuple = array[num4 - 1];
									array[num4 - 1] = array[num4];
									array[num4] = valueTuple;
									num4--;
								}
							}
						}
					}
					if (array[2].Item1 != null)
					{
						this._settlement1 = array[2].Item1;
						this._settlement2 = array[1].Item1;
						this._settlement3 = array[0].Item1;
						return true;
					}
				}
				return false;
			}

			// Token: 0x06005F80 RID: 24448 RVA: 0x001BA0D4 File Offset: 0x001B82D4
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001E37 RID: 7735
			private const int MinimumRelationToTakeQuest = -10;

			// Token: 0x04001E38 RID: 7736
			[SaveableField(10)]
			private Settlement _settlement1;

			// Token: 0x04001E39 RID: 7737
			[SaveableField(20)]
			private Settlement _settlement2;

			// Token: 0x04001E3A RID: 7738
			[SaveableField(30)]
			private Settlement _settlement3;
		}

		// Token: 0x02000756 RID: 1878
		public class ScoutEnemyGarrisonsQuest : QuestBase
		{
			// Token: 0x06005F82 RID: 24450 RVA: 0x001BA0E9 File Offset: 0x001B82E9
			internal static void AutoGeneratedStaticCollectObjectsScoutEnemyGarrisonsQuest(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005F83 RID: 24451 RVA: 0x001BA0F7 File Offset: 0x001B82F7
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._questSettlement1);
				collectedObjects.Add(this._questSettlement2);
				collectedObjects.Add(this._questSettlement3);
				collectedObjects.Add(this._startQuestLog);
			}

			// Token: 0x06005F84 RID: 24452 RVA: 0x001BA130 File Offset: 0x001B8330
			internal static object AutoGeneratedGetMemberValue_questSettlement1(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement1;
			}

			// Token: 0x06005F85 RID: 24453 RVA: 0x001BA13D File Offset: 0x001B833D
			internal static object AutoGeneratedGetMemberValue_questSettlement2(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement2;
			}

			// Token: 0x06005F86 RID: 24454 RVA: 0x001BA14A File Offset: 0x001B834A
			internal static object AutoGeneratedGetMemberValue_questSettlement3(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement3;
			}

			// Token: 0x06005F87 RID: 24455 RVA: 0x001BA157 File Offset: 0x001B8357
			internal static object AutoGeneratedGetMemberValue_scoutedSettlementCount(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._scoutedSettlementCount;
			}

			// Token: 0x06005F88 RID: 24456 RVA: 0x001BA169 File Offset: 0x001B8369
			internal static object AutoGeneratedGetMemberValue_startQuestLog(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._startQuestLog;
			}

			// Token: 0x17001334 RID: 4916
			// (get) Token: 0x06005F89 RID: 24457 RVA: 0x001BA176 File Offset: 0x001B8376
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001335 RID: 4917
			// (get) Token: 0x06005F8A RID: 24458 RVA: 0x001BA179 File Offset: 0x001B8379
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=G79IzJsZ}Scout Enemy Garrisons", null);
				}
			}

			// Token: 0x17001336 RID: 4918
			// (get) Token: 0x06005F8B RID: 24459 RVA: 0x001BA188 File Offset: 0x001B8388
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=8avwit9N}{QUEST_GIVER.LINK}, the army commander of {FACTION} has told you that they need detailed information about enemy fortifications and troop numbers of the enemy. {?QUEST_GIVER.GENDER}She{?}He{\\?} wanted you to scout {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("FACTION", base.QuestGiver.MapFaction.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_1", this._questSettlement1.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_2", this._questSettlement2.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_3", this._questSettlement3.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17001337 RID: 4919
			// (get) Token: 0x06005F8C RID: 24460 RVA: 0x001BA22A File Offset: 0x001B842A
			private TextObject SettlementBecomeNeutralLogText
			{
				get
				{
					return new TextObject("{=wgX2nL5Z}{SETTLEMENT} is no longer in control of enemy. There is no need to scout that settlement.", null);
				}
			}

			// Token: 0x17001338 RID: 4920
			// (get) Token: 0x06005F8D RID: 24461 RVA: 0x001BA237 File Offset: 0x001B8437
			private TextObject ArmyDisbandedQuestCancelLogText
			{
				get
				{
					return new TextObject("{=JiHaL6IV}Army has disbanded and your mission has been canceled.", null);
				}
			}

			// Token: 0x17001339 RID: 4921
			// (get) Token: 0x06005F8E RID: 24462 RVA: 0x001BA244 File Offset: 0x001B8444
			private TextObject NoLongerAllyQuestCancelLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vTnSa9rr}You are no longer allied with {QUEST_GIVER.LINK}'s faction. Your agreement with {QUEST_GIVER.LINK} was terminated.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700133A RID: 4922
			// (get) Token: 0x06005F8F RID: 24463 RVA: 0x001BA276 File Offset: 0x001B8476
			private TextObject AllTargetsAreNeutral
			{
				get
				{
					return new TextObject("{=LC2F84GR}None of the target settlements are in control of the enemy. Army Commander has canceled the mission.", null);
				}
			}

			// Token: 0x1700133B RID: 4923
			// (get) Token: 0x06005F90 RID: 24464 RVA: 0x001BA283 File Offset: 0x001B8483
			private TextObject ScoutFinishedForSettlementWallLevel1LogText
			{
				get
				{
					return new TextObject("{=5kxDhBWk}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are not high enough but can be useful with sufficient garrison support.", null);
				}
			}

			// Token: 0x1700133C RID: 4924
			// (get) Token: 0x06005F91 RID: 24465 RVA: 0x001BA290 File Offset: 0x001B8490
			private TextObject ScoutFinishedForSettlementWallLevel2LogText
			{
				get
				{
					return new TextObject("{=GUqjL6xk}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are high enough to defend against invaders.", null);
				}
			}

			// Token: 0x1700133D RID: 4925
			// (get) Token: 0x06005F92 RID: 24466 RVA: 0x001BA29D File Offset: 0x001B849D
			private TextObject ScoutFinishedForSettlementWallLevel3LogText
			{
				get
				{
					return new TextObject("{=YErURO5l}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are too high and hard to breach.", null);
				}
			}

			// Token: 0x1700133E RID: 4926
			// (get) Token: 0x06005F93 RID: 24467 RVA: 0x001BA2AC File Offset: 0x001B84AC
			private TextObject QuestSuccess
			{
				get
				{
					TextObject textObject = new TextObject("{=Qy7Zmmvk}You have successfully scouted the target settlements and sent the report to {QUEST_GIVER.LINK}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700133F RID: 4927
			// (get) Token: 0x06005F94 RID: 24468 RVA: 0x001BA2DE File Offset: 0x001B84DE
			private TextObject QuestTimedOut
			{
				get
				{
					return new TextObject("{=GzodT3vS}You have failed to scout the enemy settlements in time.", null);
				}
			}

			// Token: 0x06005F95 RID: 24469 RVA: 0x001BA2EC File Offset: 0x001B84EC
			public ScoutEnemyGarrisonsQuest(string questId, Hero questGiver, Settlement settlement1, Settlement settlement2, Settlement settlement3)
				: base(questId, questGiver, CampaignTime.DaysFromNow(30f), 0)
			{
				this._questSettlement1 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement1, 0);
				this._questSettlement2 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement2, 0);
				this._questSettlement3 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement3, 0);
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06005F96 RID: 24470 RVA: 0x001BA341 File Offset: 0x001B8541
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x06005F97 RID: 24471 RVA: 0x001BA34C File Offset: 0x001B854C
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=lyGvyZK4}Very well. When you reach one of their fortresses, spend some time observing. Don't move on to the next one at once. You don't need to find me to report back the details, just send your messengers.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=x3TO0gkN}Is there any progress on the task I gave you?[ib:closed][if:convo_normal]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
					})
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=W5ab31gQ}Soon, commander. We are still working on it.", null), null, null, null)
					.NpcLine(new TextObject("{=U3LR7dyK}Good. I'll be waiting for your messengers.[if:convo_thinking]", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=v75k1FoT}Not yet. We need to make more preparations.", null), null, null, null)
					.NpcLine(new TextObject("{=zYKeYZAo}All right. Don't rush this but also don't wait too long.", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x06005F98 RID: 24472 RVA: 0x001BA46C File Offset: 0x001B866C
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddTrackedObject(this._questSettlement1.Settlement);
				base.AddTrackedObject(this._questSettlement2.Settlement);
				base.AddTrackedObject(this._questSettlement3.Settlement);
				this._scoutedSettlementCount = 0;
				this._startQuestLog = base.AddDiscreteLog(this.PlayerStartsQuestLogText, new TextObject("{=jpBpwgAs}Settlements", null), this._scoutedSettlementCount, 3, null, false);
			}

			// Token: 0x06005F99 RID: 24473 RVA: 0x001BA4E0 File Offset: 0x001B86E0
			protected override void RegisterEvents()
			{
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x06005F9A RID: 24474 RVA: 0x001BA534 File Offset: 0x001B8734
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> list = new List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> { this._questSettlement1, this._questSettlement2, this._questSettlement3 };
					if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => !x.Settlement.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction)))
					{
						base.AddLog(this.AllTargetsAreNeutral, false);
						base.CompleteQuestWithCancel(null);
						return;
					}
					foreach (ScoutEnemyGarrisonsIssueBehavior.QuestSettlement questSettlement in list)
					{
						if (!questSettlement.IsScoutingCompleted())
						{
							if (DistanceHelper.FindClosestDistanceFromMobilePartyToSettlement(MobileParty.MainParty, questSettlement.Settlement, MobileParty.NavigationType.Default) <= MobileParty.MainParty.SeeingRange)
							{
								questSettlement.CurrentScoutProgress++;
								if (questSettlement.CurrentScoutProgress == 1)
								{
									TextObject textObject = new TextObject("{=qfjRGjM4}Your scouts started to gather information about {SETTLEMENT}.", null);
									textObject.SetTextVariable("SETTLEMENT", questSettlement.Settlement.Name);
									MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
								}
								else if (questSettlement.IsScoutingCompleted())
								{
									JournalLog startQuestLog = this._startQuestLog;
									int num = this._scoutedSettlementCount + 1;
									this._scoutedSettlementCount = num;
									startQuestLog.UpdateCurrentProgress(num);
									base.RemoveTrackedObject(questSettlement.Settlement);
									TextObject textObject2;
									if (questSettlement.Settlement.Town.GetWallLevel() == 1)
									{
										textObject2 = this.ScoutFinishedForSettlementWallLevel1LogText;
									}
									else if (questSettlement.Settlement.Town.GetWallLevel() == 2)
									{
										textObject2 = this.ScoutFinishedForSettlementWallLevel2LogText;
									}
									else
									{
										textObject2 = this.ScoutFinishedForSettlementWallLevel3LogText;
									}
									textObject2.SetTextVariable("SETTLEMENT", questSettlement.Settlement.EncyclopediaLinkWithName);
									MobileParty garrisonParty = questSettlement.Settlement.Town.GarrisonParty;
									int num2 = ((garrisonParty != null) ? garrisonParty.MemberRoster.TotalHealthyCount : 0);
									int num3 = (int)questSettlement.Settlement.Militia;
									textObject2.SetTextVariable("GARRISON_SIZE", num2 + num3);
									base.AddLog(textObject2, false);
								}
							}
							else
							{
								questSettlement.ResetCurrentProgress();
							}
						}
					}
					if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => x.IsScoutingCompleted()))
					{
						this.AllScoutingDone();
					}
				}
			}

			// Token: 0x06005F9B RID: 24475 RVA: 0x001BA770 File Offset: 0x001B8970
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> list = new List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> { this._questSettlement1, this._questSettlement2, this._questSettlement3 };
				foreach (ScoutEnemyGarrisonsIssueBehavior.QuestSettlement questSettlement in list)
				{
					if (settlement == questSettlement.Settlement && !questSettlement.IsScoutingCompleted() && (newOwner.MapFaction == base.QuestGiver.MapFaction || !newOwner.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction)))
					{
						questSettlement.IsCompletedThroughBeingNeutral = true;
						questSettlement.SetScoutingCompleted();
						JournalLog startQuestLog = this._startQuestLog;
						int num = this._scoutedSettlementCount + 1;
						this._scoutedSettlementCount = num;
						startQuestLog.UpdateCurrentProgress(num);
						if (base.IsTracked(questSettlement.Settlement))
						{
							base.RemoveTrackedObject(questSettlement.Settlement);
						}
						TextObject settlementBecomeNeutralLogText = this.SettlementBecomeNeutralLogText;
						settlementBecomeNeutralLogText.SetTextVariable("SETTLEMENT", questSettlement.Settlement.EncyclopediaLinkWithName);
						base.AddLog(settlementBecomeNeutralLogText, false);
						if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => x.IsCompletedThroughBeingNeutral))
						{
							base.AddLog(this.AllTargetsAreNeutral, false);
							base.CompleteQuestWithCancel(null);
							break;
						}
						break;
					}
				}
			}

			// Token: 0x06005F9C RID: 24476 RVA: 0x001BA8E4 File Offset: 0x001B8AE4
			private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
			{
				if (army.ArmyOwner == base.QuestGiver)
				{
					base.AddLog(this.ArmyDisbandedQuestCancelLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06005F9D RID: 24477 RVA: 0x001BA909 File Offset: 0x001B8B09
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == base.QuestGiver.MapFaction)
				{
					base.AddLog(this.NoLongerAllyQuestCancelLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06005F9E RID: 24478 RVA: 0x001BA936 File Offset: 0x001B8B36
			private void AllScoutingDone()
			{
				base.AddLog(this.QuestSuccess, false);
				GainRenownAction.Apply(Hero.MainHero, 3f, false);
				GainKingdomInfluenceAction.ApplyForDefault(Hero.MainHero, 10f);
				this.RelationshipChangeWithQuestGiver = 3;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06005F9F RID: 24479 RVA: 0x001BA972 File Offset: 0x001B8B72
			protected override void OnTimedOut()
			{
				base.AddLog(this.QuestTimedOut, false);
				this.RelationshipChangeWithQuestGiver = -2;
			}

			// Token: 0x04001E3B RID: 7739
			[SaveableField(10)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement1;

			// Token: 0x04001E3C RID: 7740
			[SaveableField(20)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement2;

			// Token: 0x04001E3D RID: 7741
			[SaveableField(30)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement3;

			// Token: 0x04001E3E RID: 7742
			[SaveableField(40)]
			private int _scoutedSettlementCount;

			// Token: 0x04001E3F RID: 7743
			[SaveableField(50)]
			private JournalLog _startQuestLog;
		}

		// Token: 0x02000757 RID: 1879
		public class QuestSettlement
		{
			// Token: 0x06005FA3 RID: 24483 RVA: 0x001BA9C8 File Offset: 0x001B8BC8
			internal static void AutoGeneratedStaticCollectObjectsQuestSettlement(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005FA4 RID: 24484 RVA: 0x001BA9D6 File Offset: 0x001B8BD6
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Settlement);
			}

			// Token: 0x06005FA5 RID: 24485 RVA: 0x001BA9E4 File Offset: 0x001B8BE4
			internal static object AutoGeneratedGetMemberValueSettlement(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).Settlement;
			}

			// Token: 0x06005FA6 RID: 24486 RVA: 0x001BA9F1 File Offset: 0x001B8BF1
			internal static object AutoGeneratedGetMemberValueCurrentScoutProgress(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).CurrentScoutProgress;
			}

			// Token: 0x06005FA7 RID: 24487 RVA: 0x001BAA03 File Offset: 0x001B8C03
			public QuestSettlement(Settlement settlement, int currentScoutProgress)
			{
				this.Settlement = settlement;
				this.CurrentScoutProgress = currentScoutProgress;
				this.IsCompletedThroughBeingNeutral = false;
			}

			// Token: 0x06005FA8 RID: 24488 RVA: 0x001BAA20 File Offset: 0x001B8C20
			public bool IsScoutingCompleted()
			{
				return this.CurrentScoutProgress >= 8;
			}

			// Token: 0x06005FA9 RID: 24489 RVA: 0x001BAA2E File Offset: 0x001B8C2E
			public void SetScoutingCompleted()
			{
				this.CurrentScoutProgress = 8;
			}

			// Token: 0x06005FAA RID: 24490 RVA: 0x001BAA37 File Offset: 0x001B8C37
			public void ResetCurrentProgress()
			{
				this.CurrentScoutProgress = 0;
			}

			// Token: 0x04001E40 RID: 7744
			private const int CompleteScoutAfterHours = 8;

			// Token: 0x04001E41 RID: 7745
			[SaveableField(10)]
			public Settlement Settlement;

			// Token: 0x04001E42 RID: 7746
			[SaveableField(20)]
			public int CurrentScoutProgress;

			// Token: 0x04001E43 RID: 7747
			public bool IsCompletedThroughBeingNeutral;
		}

		// Token: 0x02000758 RID: 1880
		public class ScoutEnemyGarrisonsIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005FAB RID: 24491 RVA: 0x001BAA40 File Offset: 0x001B8C40
			public ScoutEnemyGarrisonsIssueTypeDefiner()
				: base(97600)
			{
			}

			// Token: 0x06005FAC RID: 24492 RVA: 0x001BAA4D File Offset: 0x001B8C4D
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), 1, null);
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest), 2, null);
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.QuestSettlement), 3, null);
			}
		}
	}
}
