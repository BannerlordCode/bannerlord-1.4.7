using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.CampaignBehaviors;
using StoryMode.Quests.ThirdPhase;
using TaleWorlds.AchievementSystem;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x0200004D RID: 77
	public class AchievementsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000483 RID: 1155 RVA: 0x00019943 File Offset: 0x00017B43
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<bool>("_deactivateAchievements", ref this._deactivateAchievements);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00019958 File Offset: 0x00017B58
		public override void RegisterEvents()
		{
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action(this.CacheHighestSkillValue));
			CampaignEvents.WorkshopOwnerChangedEvent.AddNonSerializedListener(this, new Action<Workshop, Hero>(this.ProgressOwnedWorkshopCount));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.ProgressOwnedCaravanCount));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.ProgressCreatedKingdomCount));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.BeforeHeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnBeforeHeroKilled));
			CampaignEvents.ClanTierIncrease.AddNonSerializedListener(this, new Action<Clan, bool>(this.ProgressClanTier));
			CampaignEvents.OnHideoutBattleCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState>(this.OnHideoutBattleCompleted));
			CampaignEvents.HeroGainedSkill.AddNonSerializedListener(this, new Action<Hero, SkillObject, int, bool>(this.ProgressHeroSkillValue));
			CampaignEvents.PlayerInventoryExchangeEvent.AddNonSerializedListener(this, new Action<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool>(this.PlayerInventoryExchange));
			CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinish));
			CampaignEvents.SiegeCompletedEvent.AddNonSerializedListener(this, new Action<Settlement, MobileParty, bool, MapEvent.BattleTypes>(this.OnSiegeCompleted));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.OnBuildingLevelChangedEvent.AddNonSerializedListener(this, new Action<Town, Building, int>(this.OnBuildingLevelChanged));
			CampaignEvents.OnNewItemCraftedEvent.AddNonSerializedListener(this, new Action<ItemObject, ItemModifier, bool>(this.OnNewItemCrafted));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
			CampaignEvents.OnPlayerTradeProfitEvent.AddNonSerializedListener(this, new Action<int>(this.ProgressTotalTradeProfit));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.CheckHeroMarriage));
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.CheckKingdomDecisionConcluded));
			CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEnter));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
			CampaignEvents.RulingClanChanged.AddNonSerializedListener(this, new Action<Kingdom, Clan>(this.OnRulingClanChanged));
			CampaignEvents.OnConfigChangedEvent.AddNonSerializedListener(this, new Action(this.OnConfigChanged));
			CampaignEvents.CollectMetadataEntriesEvent.AddNonSerializedListener(this, new Action<List<KeyValuePair<string, string>>>(this.CollectMetadataEntries));
			StoryModeEvents.OnStoryModeTutorialEndedEvent.AddNonSerializedListener(this, new Action(this.CheckTutorialFinished));
			StoryModeEvents.OnBannerPieceCollectedEvent.AddNonSerializedListener(this, new Action(this.ProgressAssembledDragonBanner));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00019C73 File Offset: 0x00017E73
		private void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
			this.ProgressOwnedFortificationCount();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019C7C File Offset: 0x00017E7C
		private void OnIssueUpdated(IssueBase issueBase, IssueBase.IssueUpdateDetails detail, Hero issueSolver)
		{
			if (issueSolver == Hero.MainHero && !issueBase.IsSolvingWithAlternative && detail == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess && issueBase.IssueOwner.MapFaction != null && issueBase.IssueOwner.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				this.SetStatInternal("CompletedAnIssueInHostileTown", 1);
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00019CD2 File Offset: 0x00017ED2
		private void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
			if (hideoutEventComponent.MapEvent.InvolvedParties.Contains(PartyBase.MainParty) && winnerSide == hideoutEventComponent.MapEvent.PlayerSide)
			{
				this.ProgressHideoutClearedCount();
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00019CFF File Offset: 0x00017EFF
		private void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.ProgressKingOrQueenKilledInBattle(victim, killer, detail);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00019D0C File Offset: 0x00017F0C
		private void OnConfigChanged()
		{
			TextObject textObject;
			if (!this.CheckAchievementSystemActivity(out textObject))
			{
				this.DeactivateAchievements(textObject, true, false);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00019D2C File Offset: 0x00017F2C
		private void OnHeroCreated(Hero hero, bool isBornNaturally)
		{
			if (isBornNaturally)
			{
				if (hero.Father == Hero.MainHero || hero.Mother == Hero.MainHero)
				{
					this.ProgressChildCount();
				}
				this.CheckGrandparent();
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00019D58 File Offset: 0x00017F58
		private void OnGameLoadFinished()
		{
			TextObject textObject;
			if (this.CheckAchievementSystemActivity(out textObject))
			{
				this.CacheAndInitializeAchievementVariables();
				this.CacheHighestSkillValue();
				return;
			}
			this.DeactivateAchievements(textObject, true, false);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00019D88 File Offset: 0x00017F88
		private async void CacheAndInitializeAchievementVariables()
		{
			this._butter = MBObjectManager.Instance.GetObject<ItemObject>("butter");
			List<string> list = new List<string>
			{
				"CreatedKingdomCount", "ClearedHideoutCount", "RepelledSiegeAssaultCount", "KingOrQueenKilledInBattle", "SuccessfulSiegeCount", "WonTournamentCount", "SuccessfulBattlesAgainstArmyCount", "DefeatedArmyWhileAloneCount", "TotalTradeProfit", "MaxDailyIncome",
				"CapturedATownAloneCount", "DefeatedTroopCount", "FarthestHeadShot"
			};
			this._orderedSettlementList = (from x in Settlement.All
				where x.IsFortification
				orderby x.StringId descending
				select x).ToList<Settlement>();
			int neededIntegerCount = MathF.Ceiling((float)this._orderedSettlementList.Count / 30f);
			this._settlementIntegerSetList = new int[neededIntegerCount];
			for (int i = 0; i < neededIntegerCount; i++)
			{
				list.Add("SettlementSet" + i);
			}
			int[] array = await AchievementManager.GetStats(list.ToArray());
			if (array != null)
			{
				int num = 0;
				int[] array2 = array;
				num++;
				this._cachedCreatedKingdomCount = array2[num];
				int[] array3 = array;
				num++;
				this._cachedHideoutClearedCount = array3[num];
				int[] array4 = array;
				num++;
				this._cachedRepelledSiegeAssaultCount = array4[num];
				int[] array5 = array;
				num++;
				this._cachedKingOrQueenKilledInBattle = array5[num];
				int[] array6 = array;
				num++;
				this._cachedSuccessfulSiegeCount = array6[num];
				int[] array7 = array;
				num++;
				this._cachedWonTournamentCount = array7[num];
				int[] array8 = array;
				num++;
				this._cachedSuccessfulBattlesAgainstArmyCount = array8[num];
				int[] array9 = array;
				num++;
				this._cachedSuccessfulBattlesAgainstArmyAloneCount = array9[num];
				int[] array10 = array;
				num++;
				this._cachedTotalTradeProfit = array10[num];
				int[] array11 = array;
				num++;
				this._cachedMaxDailyIncome = array11[num];
				int[] array12 = array;
				num++;
				this._cachedCapturedTownAloneCount = array12[num];
				int[] array13 = array;
				num++;
				this._cachedDefeatedTroopCount = array13[num];
				int[] array14 = array;
				num++;
				this._cachedFarthestHeadShot = array14[num];
				for (int j = 0; j < neededIntegerCount; j++)
				{
					int num2 = array[num++];
					if (num2 == -1)
					{
						this._settlementIntegerSetList[j] = 0;
						this.SetStatInternal("SettlementSet" + j, 0);
					}
					else
					{
						this._settlementIntegerSetList[j] = num2;
					}
				}
			}
			else
			{
				this.DeactivateAchievements(new TextObject("{=4wS8eYYe}Achievements are disabled temporarily for this session due to service disconnection.", null), true, true);
				Debug.Print("Achievements are disabled because current platform does not support achievements!", 0, Debug.DebugColor.DarkRed, 17592186044416UL);
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00019DC4 File Offset: 0x00017FC4
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			TextObject textObject;
			if (this.CheckAchievementSystemActivity(out textObject))
			{
				this.CacheAndInitializeAchievementVariables();
				return;
			}
			this.DeactivateAchievements(textObject, true, false);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00019DEB File Offset: 0x00017FEB
		private void OnDailyTick()
		{
			this.ProgressDailyTribute();
			this.ProgressDailyIncome();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00019DF9 File Offset: 0x00017FF9
		private void OnClanDestroyed(Clan clan)
		{
			this.ProgressClansUnderKingdomCount();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00019E01 File Offset: 0x00018001
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			this.ProgressDailyIncome();
			this.ProgressClansUnderKingdomCount();
			this.ProgressOwnedFortificationCount();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00019E15 File Offset: 0x00018015
		private void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
			this.ProgressHighestTierSwordCrafted(itemObject);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00019E1E File Offset: 0x0001801E
		private void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
			this.ProgressDailyIncome();
			this.CheckProjectsInSettlement(town);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00019E2D File Offset: 0x0001802D
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			this.ProgressImperialBarbarianVictory(quest, detail);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00019E37 File Offset: 0x00018037
		private void OnTournamentFinish(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			this.ProgressTournamentWonCount(winner);
			this.ProgressTournamentRank(winner);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00019E47 File Offset: 0x00018047
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			this.ProgressRepelSiegeAssaultCount(mapEvent);
			this.CheckDefeatedSuperiorForce(mapEvent);
			this.ProgressSuccessfulBattlesAgainstArmyCount(mapEvent);
			this.ProgressSuccessfulBattlesAgainstArmyAloneCount(mapEvent);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00019E65 File Offset: 0x00018065
		private void OnSiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
			this.ProgressRepelSiegeAssaultCount(siegeSettlement, isWin);
			this.ProgressSuccessfulSiegeCount(attackerParty, isWin);
			this.ProgressCapturedATownAlone(attackerParty, isWin);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00019E80 File Offset: 0x00018080
		private void PlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			if (this._butter != null)
			{
				int itemNumber = PartyBase.MainParty.ItemRoster.GetItemNumber(this._butter);
				if (itemNumber > 0)
				{
					this.SetStatInternal("ButtersInInventoryCount", itemNumber);
				}
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00019EBC File Offset: 0x000180BC
		public bool CheckAchievementSystemActivity(out TextObject reason)
		{
			bool flag = DumpIntegrityCampaignBehavior.IsGameIntegrityAchieved(out reason);
			DumpIntegrityCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<DumpIntegrityCampaignBehavior>();
			return (!this._deactivateAchievements && behavior != null && flag) || MBDebug.IsTestMode();
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00019EFC File Offset: 0x000180FC
		private void OnSettlementEnter(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party == MobileParty.MainParty && settlement.IsFortification)
			{
				int num = this._orderedSettlementList.IndexOf(settlement);
				int num2 = MathF.Floor((float)num / 30f);
				int num3 = this._settlementIntegerSetList[num2];
				int num4 = 1 << (int)(30f - ((float)num % 30f + 1f));
				int num5 = num3 | num4;
				this.SetStatInternal("SettlementSet" + num2, num5);
				if (this._settlementIntegerSetList[num2] != num5)
				{
					this._settlementIntegerSetList[num2] = num5;
					this.CheckEnteredEverySettlement();
				}
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00019F8C File Offset: 0x0001818C
		private void CheckEnteredEverySettlement()
		{
			int num = 0;
			for (int i = 0; i < this._settlementIntegerSetList.Length; i++)
			{
				for (int j = this._settlementIntegerSetList[i]; j > 0; j >>= 1)
				{
					if (j % 2 == 1)
					{
						num++;
					}
				}
			}
			if (num == this._orderedSettlementList.Count)
			{
				this.SetStatInternal("EnteredEverySettlement", 1);
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00019FE8 File Offset: 0x000181E8
		private void CacheHighestSkillValue()
		{
			int num = 0;
			foreach (SkillObject skillObject in Skills.All)
			{
				int skillValue = Hero.MainHero.GetSkillValue(skillObject);
				if (skillValue > num)
				{
					num = skillValue;
				}
			}
			this._cachedHighestSkillValue = num;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001A050 File Offset: 0x00018250
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.CheckExecutedLordRelation(victim, killer, detail);
			this.CheckBestServedCold(victim, killer, detail);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001A064 File Offset: 0x00018264
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			this.ProgressDailyIncome();
			if (settlement.IsFortification)
			{
				this.ProgressOwnedFortificationCount();
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001A07C File Offset: 0x0001827C
		private void OnMissionStarted(IMission obj)
		{
			AchievementsCampaignBehavior.AchievementMissionLogic achievementMissionLogic = new AchievementsCampaignBehavior.AchievementMissionLogic(new Action<Agent, Agent>(this.OnAgentRemoved), new Action<Agent, WeaponComponentData, BoneBodyPartType, int>(this.OnAgentHit));
			Mission.Current.AddMissionBehavior(achievementMissionLogic);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001A0B2 File Offset: 0x000182B2
		private void OnAgentHit(Agent affectorAgent, WeaponComponentData attackerWeapon, BoneBodyPartType victimBoneBodyPartType, int hitDistance)
		{
			if (affectorAgent != null && affectorAgent == Agent.Main && attackerWeapon != null && !attackerWeapon.IsMeleeWeapon && victimBoneBodyPartType == BoneBodyPartType.Head && hitDistance > this._cachedFarthestHeadShot)
			{
				this.SetStatInternal("FarthestHeadShot", hitDistance);
				this._cachedFarthestHeadShot = hitDistance;
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001A0EC File Offset: 0x000182EC
		private void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent)
		{
			if (affectorAgent != null && affectorAgent == Agent.Main && affectedAgent.IsHuman)
			{
				string text = "DefeatedTroopCount";
				int num = this._cachedDefeatedTroopCount + 1;
				this._cachedDefeatedTroopCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001A128 File Offset: 0x00018328
		private void ProgressChildCount()
		{
			int num = Hero.MainHero.Children.Count;
			using (List<LogEntry>.Enumerator enumerator = Campaign.Current.LogEntryHistory.GameActionLogs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerCharacterChangedLogEntry playerCharacterChangedLogEntry;
					if ((playerCharacterChangedLogEntry = enumerator.Current as PlayerCharacterChangedLogEntry) != null)
					{
						num += playerCharacterChangedLogEntry.OldPlayerHero.Children.Count;
					}
				}
			}
			this.SetStatInternal("NumberOfChildrenBorn", num);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001A1B4 File Offset: 0x000183B4
		private void CheckGrandparent()
		{
			if (Hero.MainHero.Children.Any<Hero>((Hero x) => x.Children.Any<Hero>((Hero y) => y.Children.Any<Hero>())))
			{
				this.SetStatInternal("GreatGranny", 1);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001A1F2 File Offset: 0x000183F2
		public void OnRadagosDuelWon()
		{
			this.SetStatInternal("RadagosDefeatedInDuel", 1);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001A200 File Offset: 0x00018400
		private void CheckBestServedCold(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail)
		{
			if (killer == Hero.MainHero && (detail == KillCharacterAction.KillCharacterActionDetail.Executed || detail == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent || detail == KillCharacterAction.KillCharacterActionDetail.Murdered || detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle || detail == KillCharacterAction.KillCharacterActionDetail.WoundedInBattle))
			{
				using (List<LogEntry>.Enumerator enumerator = Campaign.Current.LogEntryHistory.GameActionLogs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CharacterKilledLogEntry characterKilledLogEntry;
						if ((characterKilledLogEntry = enumerator.Current as CharacterKilledLogEntry) != null && characterKilledLogEntry.Killer == victim && characterKilledLogEntry.VictimClan == Clan.PlayerClan)
						{
							this.SetStatInternal("BestServedCold", 1);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001A2A0 File Offset: 0x000184A0
		private void CheckProposedAndWonPolicy(KingdomDecision decision, DecisionOutcome chosenOutcome)
		{
			if (decision.ProposerClan == Clan.PlayerClan && decision.GetQueriedDecisionOutcome(new MBList<DecisionOutcome> { chosenOutcome }) != null)
			{
				this.SetStatInternal("ProposedAndWonAPolicy", 1);
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001A2D2 File Offset: 0x000184D2
		private void CheckKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			this.CheckProposedAndWonPolicy(decision, chosenOutcome);
			this.ProgressOwnedFortificationCount();
			this.ProgressClansUnderKingdomCount();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001A2E8 File Offset: 0x000184E8
		private void CheckHeroMarriage(Hero hero1, Hero hero2, bool showNotification = true)
		{
			if (hero1 == Hero.MainHero || hero2 == Hero.MainHero)
			{
				Hero hero3 = ((hero1 == Hero.MainHero) ? hero2 : hero1);
				using (List<LogEntry>.Enumerator enumerator = Campaign.Current.LogEntryHistory.GameActionLogs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CharacterKilledLogEntry characterKilledLogEntry;
						if ((characterKilledLogEntry = enumerator.Current as CharacterKilledLogEntry) != null && characterKilledLogEntry.Killer == Hero.MainHero && hero3.ExSpouses.Contains(characterKilledLogEntry.Victim))
						{
							this.SetStatInternal("Hearthbreaker", 1);
						}
					}
				}
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001A390 File Offset: 0x00018590
		private void ProgressClansUnderKingdomCount()
		{
			if (Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Leader == Hero.MainHero)
			{
				this.SetStatInternal("ClansUnderPlayerKingdomCount", Clan.PlayerClan.Kingdom.Clans.Count);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001A3E0 File Offset: 0x000185E0
		private void ProgressSuccessfulBattlesAgainstArmyCount(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent && mapEvent.Winner == mapEvent.GetMapEventSide(mapEvent.PlayerSide))
			{
				if (mapEvent.GetMapEventSide(mapEvent.DefeatedSide).Parties.Any<MapEventParty>((MapEventParty x) => x.Party.MobileParty != null && x.Party.MobileParty.AttachedTo != null))
				{
					string text = "SuccessfulBattlesAgainstArmyCount";
					int num = this._cachedSuccessfulBattlesAgainstArmyCount + 1;
					this._cachedSuccessfulBattlesAgainstArmyCount = num;
					this.SetStatInternal(text, num);
				}
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001A45C File Offset: 0x0001865C
		private void ProgressSuccessfulBattlesAgainstArmyAloneCount(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent && mapEvent.Winner == mapEvent.GetMapEventSide(mapEvent.PlayerSide))
			{
				if (mapEvent.GetMapEventSide(mapEvent.DefeatedSide).Parties.Any<MapEventParty>((MapEventParty x) => x.Party.MobileParty != null && x.Party.MobileParty.AttachedTo != null) && mapEvent.GetMapEventSide(mapEvent.PlayerSide).Parties.Count == 1)
				{
					string text = "DefeatedArmyWhileAloneCount";
					int num = this._cachedSuccessfulBattlesAgainstArmyAloneCount + 1;
					this._cachedSuccessfulBattlesAgainstArmyAloneCount = num;
					this.SetStatInternal(text, num);
				}
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001A4F4 File Offset: 0x000186F4
		private void ProgressDailyTribute()
		{
			IFaction mapFaction = Clan.PlayerClan.MapFaction;
			float num = 1f;
			int num2 = 0;
			if (Clan.PlayerClan.Kingdom != null)
			{
				num = AchievementsCampaignBehavior.CalculateTributeShareFactor(Clan.PlayerClan);
			}
			foreach (StanceLink stanceLink in FactionHelper.GetStances(mapFaction))
			{
				int dailyTributeToPay = stanceLink.GetDailyTributeToPay(mapFaction);
				if (stanceLink.IsNeutral && dailyTributeToPay < 0)
				{
					int num3 = (int)((float)dailyTributeToPay * num);
					num2 += num3;
				}
			}
			this.SetStatInternal("MaxDailyTributeGain", MathF.Abs(num2));
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001A598 File Offset: 0x00018798
		private static float CalculateTributeShareFactor(Clan clan)
		{
			Kingdom kingdom = clan.Kingdom;
			int num = kingdom.Fiefs.Sum<Town>(delegate(Town x)
			{
				if (!x.IsCastle)
				{
					return 3;
				}
				return 1;
			}) + 1 + kingdom.Clans.Count;
			return (float)(clan.Fiefs.Sum<Town>(delegate(Town x)
			{
				if (!x.IsCastle)
				{
					return 3;
				}
				return 1;
			}) + ((clan == kingdom.RulingClan) ? 1 : 0) + 1) / (float)num;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001A624 File Offset: 0x00018824
		private void ProgressDailyIncome()
		{
			int num = (int)Campaign.Current.Models.ClanFinanceModel.CalculateClanIncome(Clan.PlayerClan, false, false, false).ResultNumber;
			if (num > this._cachedMaxDailyIncome)
			{
				this.SetStatInternal("MaxDailyIncome", num);
				this._cachedMaxDailyIncome = num;
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001A673 File Offset: 0x00018873
		private void ProgressTotalTradeProfit(int profit)
		{
			this._cachedTotalTradeProfit += profit;
			this.SetStatInternal("TotalTradeProfit", this._cachedTotalTradeProfit);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001A694 File Offset: 0x00018894
		private void CheckProjectsInSettlement(Town town)
		{
			if (town.OwnerClan == Clan.PlayerClan)
			{
				foreach (Settlement settlement in Clan.PlayerClan.Settlements.Where<Settlement>((Settlement x) => x.IsFortification))
				{
					bool flag = true;
					foreach (Building building in settlement.Town.Buildings)
					{
						if (building.CurrentLevel != 3 && !building.BuildingType.IsDailyProject)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						this.SetStatInternal("CompletedAllProjects", 1);
					}
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001A77C File Offset: 0x0001897C
		private void ProgressHighestTierSwordCrafted(ItemObject itemObject)
		{
			WeaponComponentData primaryWeapon = itemObject.WeaponComponent.PrimaryWeapon;
			if (primaryWeapon.WeaponClass == WeaponClass.OneHandedSword || primaryWeapon.WeaponClass == WeaponClass.TwoHandedSword)
			{
				this.SetStatInternal("HighestTierSwordCrafted", (int)(itemObject.Tier + 1));
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001A7BA File Offset: 0x000189BA
		private void ProgressAssembledDragonBanner()
		{
			if (StoryModeManager.Current.MainStoryLine.FirstPhase != null && StoryModeManager.Current.MainStoryLine.FirstPhase.AllPiecesCollected)
			{
				this.SetStatInternal("AssembledDragonBanner", 1);
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001A7F0 File Offset: 0x000189F0
		private void ProgressImperialBarbarianVictory(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (quest.IsSpecialQuest && quest.GetType() == typeof(DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest))
			{
				if (StoryModeManager.Current.MainStoryLine.MainStoryLineSide == MainStoryLineSide.CreateAntiImperialKingdom || StoryModeManager.Current.MainStoryLine.MainStoryLineSide == MainStoryLineSide.SupportAntiImperialKingdom)
				{
					this.SetStatInternal("BarbarianVictory", 1);
					return;
				}
				this.SetStatInternal("ImperialVictory", 1);
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001A85C File Offset: 0x00018A5C
		private void CheckDefeatedSuperiorForce(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent && mapEvent.Winner == mapEvent.GetMapEventSide(mapEvent.PlayerSide))
			{
				int num = mapEvent.GetMapEventSide(mapEvent.DefeatedSide).Parties.Sum<MapEventParty>((MapEventParty x) => x.HealthyManCountAtStart);
				int num2 = mapEvent.GetMapEventSide(mapEvent.WinningSide).Parties.Sum<MapEventParty>((MapEventParty x) => x.HealthyManCountAtStart);
				if (num - num2 >= 500)
				{
					this.SetStatInternal("DefeatedSuperiorForce", 1);
				}
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001A90B File Offset: 0x00018B0B
		private void CheckTutorialFinished()
		{
			if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsSkipped)
			{
				this.SetStatInternal("FinishedTutorial", 1);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001A930 File Offset: 0x00018B30
		private void ProgressSuccessfulSiegeCount(MobileParty attackerParty, bool isWin)
		{
			if (attackerParty == MobileParty.MainParty && isWin)
			{
				string text = "SuccessfulSiegeCount";
				int num = this._cachedSuccessfulSiegeCount + 1;
				this._cachedSuccessfulSiegeCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001A968 File Offset: 0x00018B68
		private void ProgressCapturedATownAlone(MobileParty attackerParty, bool isWin)
		{
			if (attackerParty == MobileParty.MainParty && isWin && attackerParty.Army == null)
			{
				string text = "CapturedATownAloneCount";
				int num = this._cachedCapturedTownAloneCount + 1;
				this._cachedCapturedTownAloneCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001A9A8 File Offset: 0x00018BA8
		private void ProgressRepelSiegeAssaultCount(Settlement siegeSettlement, bool isWin)
		{
			if (siegeSettlement.OwnerClan == Clan.PlayerClan && !isWin)
			{
				string text = "RepelledSiegeAssaultCount";
				int num = this._cachedRepelledSiegeAssaultCount + 1;
				this._cachedRepelledSiegeAssaultCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001A9E4 File Offset: 0x00018BE4
		private void ProgressRepelSiegeAssaultCount(MapEvent mapEvent)
		{
			if (mapEvent.MapEventSettlement != null && mapEvent.MapEventSettlement.OwnerClan == Clan.PlayerClan && mapEvent.EventType == MapEvent.BattleTypes.Siege && mapEvent.BattleState == BattleState.None && PlayerEncounter.Battle != null && PlayerEncounter.CampaignBattleResult != null && PlayerEncounter.CampaignBattleResult.PlayerVictory)
			{
				string text = "RepelledSiegeAssaultCount";
				int num = this._cachedRepelledSiegeAssaultCount + 1;
				this._cachedRepelledSiegeAssaultCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001AA54 File Offset: 0x00018C54
		private void ProgressTournamentRank(CharacterObject winner)
		{
			if (winner == CharacterObject.PlayerCharacter && Campaign.Current.TournamentManager.GetLeaderboard()[0].Key == Hero.MainHero)
			{
				this.SetStatInternal("LeaderOfTournament", 1);
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001AA9C File Offset: 0x00018C9C
		private void ProgressHeroSkillValue(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			if (hero == Hero.MainHero && this._cachedHighestSkillValue > -1)
			{
				int skillValue = hero.GetSkillValue(skill);
				if (skillValue > this._cachedHighestSkillValue)
				{
					this.SetStatInternal("HighestSkillValue", skillValue);
					this._cachedHighestSkillValue = skillValue;
				}
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001AAE0 File Offset: 0x00018CE0
		private void ProgressHideoutClearedCount()
		{
			string text = "ClearedHideoutCount";
			int num = this._cachedHideoutClearedCount + 1;
			this._cachedHideoutClearedCount = num;
			this.SetStatInternal(text, num);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001AB09 File Offset: 0x00018D09
		private void CheckExecutedLordRelation(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail)
		{
			if (((killer == Hero.MainHero && detail == KillCharacterAction.KillCharacterActionDetail.Executed) || detail == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent) && (int)victim.GetRelationWithPlayer() <= -100)
			{
				this.SetStatInternal("ExecutedLordRelation100", 1);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001AB34 File Offset: 0x00018D34
		private void ProgressKingOrQueenKilledInBattle(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail)
		{
			if (killer == Hero.MainHero && victim.IsKingdomLeader && detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				string text = "KingOrQueenKilledInBattle";
				int num = this._cachedKingOrQueenKilledInBattle + 1;
				this._cachedKingOrQueenKilledInBattle = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001AB74 File Offset: 0x00018D74
		private void ProgressTournamentWonCount(CharacterObject winner)
		{
			if (winner == CharacterObject.PlayerCharacter)
			{
				string text = "WonTournamentCount";
				int num = this._cachedWonTournamentCount + 1;
				this._cachedWonTournamentCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001ABA5 File Offset: 0x00018DA5
		private void ProgressOwnedWorkshopCount(Workshop workshop, Hero oldOwner)
		{
			if (workshop.Owner == Hero.MainHero)
			{
				this.ProgressHasOwnedCaravanAndWorkshop();
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001ABBA File Offset: 0x00018DBA
		private void ProgressOwnedCaravanCount(MobileParty party)
		{
			if (party.IsCaravan && party.MapFaction == Hero.MainHero.MapFaction)
			{
				this.ProgressHasOwnedCaravanAndWorkshop();
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001ABDC File Offset: 0x00018DDC
		private void ProgressHasOwnedCaravanAndWorkshop()
		{
			if (Hero.MainHero.OwnedWorkshops.Count > 0 && Hero.MainHero.OwnedCaravans.Count > 0)
			{
				this.SetStatInternal("HasOwnedCaravanAndWorkshop", 1);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001AC10 File Offset: 0x00018E10
		private void ProgressOwnedFortificationCount()
		{
			int num;
			if (Hero.MainHero.IsKingdomLeader)
			{
				num = Hero.MainHero.MapFaction.Fiefs.Count;
			}
			else
			{
				num = Hero.MainHero.Clan.Fiefs.Count;
			}
			this.SetStatInternal("OwnedFortificationCount", num);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001AC64 File Offset: 0x00018E64
		private void ProgressCreatedKingdomCount(Kingdom kingdom)
		{
			if (kingdom.Leader == Hero.MainHero)
			{
				string text = "CreatedKingdomCount";
				int num = this._cachedCreatedKingdomCount + 1;
				this._cachedCreatedKingdomCount = num;
				this.SetStatInternal(text, num);
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001AC9A File Offset: 0x00018E9A
		private void ProgressClanTier(Clan clan, bool shouldNotify)
		{
			if (clan == Clan.PlayerClan && clan.Tier == 6)
			{
				this.SetStatInternal("ReachedClanTierSix", 1);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001ACBC File Offset: 0x00018EBC
		public void DeactivateAchievements(TextObject reason = null, bool showMessage = true, bool temporarily = false)
		{
			this._deactivateAchievements = !temporarily || this._deactivateAchievements;
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			CampaignEvents.CollectMetadataEntriesEvent.AddNonSerializedListener(this, new Action<List<KeyValuePair<string, string>>>(this.CollectMetadataEntries));
			if (showMessage)
			{
				if (TextObject.IsNullOrEmpty(reason))
				{
					reason = new TextObject("{=Z9mcDuDi}Achievements are disabled!", null);
				}
				MBInformationManager.AddQuickInformation(reason, 4000, null, null, "");
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001AD27 File Offset: 0x00018F27
		private void CollectMetadataEntries(List<KeyValuePair<string, string>> list)
		{
			list.Add(new KeyValuePair<string, string>("AchievementsDisabled", this._deactivateAchievements ? "1" : "0"));
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001AD4D File Offset: 0x00018F4D
		private void SetStatInternal(string statId, int value)
		{
			if (!this._deactivateAchievements)
			{
				AchievementManager.SetStat(statId, value);
			}
		}

		// Token: 0x04000196 RID: 406
		private const float SettlementCountStoredInIntegerSet = 30f;

		// Token: 0x04000197 RID: 407
		private const string CreatedKingdomCountStatID = "CreatedKingdomCount";

		// Token: 0x04000198 RID: 408
		private const string ClearedHideoutCountStatID = "ClearedHideoutCount";

		// Token: 0x04000199 RID: 409
		private const string RepelledSiegeAssaultStatID = "RepelledSiegeAssaultCount";

		// Token: 0x0400019A RID: 410
		private const string KingOrQueenKilledInBattleStatID = "KingOrQueenKilledInBattle";

		// Token: 0x0400019B RID: 411
		private const string SuccessfulSiegeCountStatID = "SuccessfulSiegeCount";

		// Token: 0x0400019C RID: 412
		private const string WonTournamentCountStatID = "WonTournamentCount";

		// Token: 0x0400019D RID: 413
		private const string HighestTierSwordCraftedStatID = "HighestTierSwordCrafted";

		// Token: 0x0400019E RID: 414
		private const string SuccessfulBattlesAgainstArmyCountStatID = "SuccessfulBattlesAgainstArmyCount";

		// Token: 0x0400019F RID: 415
		private const string DefeatedArmyWhileAloneCountStatID = "DefeatedArmyWhileAloneCount";

		// Token: 0x040001A0 RID: 416
		private const string TotalTradeProfitStatID = "TotalTradeProfit";

		// Token: 0x040001A1 RID: 417
		private const string MaxDailyTributeGainStatID = "MaxDailyTributeGain";

		// Token: 0x040001A2 RID: 418
		private const string MaxDailyIncomeStatID = "MaxDailyIncome";

		// Token: 0x040001A3 RID: 419
		private const string CapturedATownAloneCountStatID = "CapturedATownAloneCount";

		// Token: 0x040001A4 RID: 420
		private const string DefeatedTroopCountStatID = "DefeatedTroopCount";

		// Token: 0x040001A5 RID: 421
		private const string FarthestHeadStatID = "FarthestHeadShot";

		// Token: 0x040001A6 RID: 422
		private const string ButtersInInventoryStatID = "ButtersInInventoryCount";

		// Token: 0x040001A7 RID: 423
		private const string ReachedClanTierSixStatID = "ReachedClanTierSix";

		// Token: 0x040001A8 RID: 424
		private const string OwnedFortificationCountStatID = "OwnedFortificationCount";

		// Token: 0x040001A9 RID: 425
		private const string HasOwnedCaravanAndWorkshopStatID = "HasOwnedCaravanAndWorkshop";

		// Token: 0x040001AA RID: 426
		private const string ExecutedLordWithMinus100RelationStatID = "ExecutedLordRelation100";

		// Token: 0x040001AB RID: 427
		private const string HighestSkillValueStatID = "HighestSkillValue";

		// Token: 0x040001AC RID: 428
		private const string LeaderOfTournamentStatID = "LeaderOfTournament";

		// Token: 0x040001AD RID: 429
		private const string FinishedTutorialStatID = "FinishedTutorial";

		// Token: 0x040001AE RID: 430
		private const string DefeatedSuperiorForceStatID = "DefeatedSuperiorForce";

		// Token: 0x040001AF RID: 431
		private const string BarbarianVictoryStatID = "BarbarianVictory";

		// Token: 0x040001B0 RID: 432
		private const string ImperialVictoryStatID = "ImperialVictory";

		// Token: 0x040001B1 RID: 433
		private const string AssembledDragonBannerStatID = "AssembledDragonBanner";

		// Token: 0x040001B2 RID: 434
		private const string CompletedAllProjectsStatID = "CompletedAllProjects";

		// Token: 0x040001B3 RID: 435
		private const string ClansUnderPlayerKingdomCountStatID = "ClansUnderPlayerKingdomCount";

		// Token: 0x040001B4 RID: 436
		private const string HearthBreakerStatID = "Hearthbreaker";

		// Token: 0x040001B5 RID: 437
		private const string ProposedAndWonAPolicyStatID = "ProposedAndWonAPolicy";

		// Token: 0x040001B6 RID: 438
		private const string BestServedColdStatID = "BestServedCold";

		// Token: 0x040001B7 RID: 439
		private const string DefeatedRadagosInDUelStatID = "RadagosDefeatedInDuel";

		// Token: 0x040001B8 RID: 440
		private const string GreatGrannyStatID = "GreatGranny";

		// Token: 0x040001B9 RID: 441
		private const string NumberOfChildrenStatID = "NumberOfChildrenBorn";

		// Token: 0x040001BA RID: 442
		private const string UndercoverStatID = "CompletedAnIssueInHostileTown";

		// Token: 0x040001BB RID: 443
		private const string EnteredEverySettlemenStatID = "EnteredEverySettlement";

		// Token: 0x040001BC RID: 444
		private bool _deactivateAchievements;

		// Token: 0x040001BD RID: 445
		private int _cachedCreatedKingdomCount;

		// Token: 0x040001BE RID: 446
		private int _cachedHideoutClearedCount;

		// Token: 0x040001BF RID: 447
		private int _cachedHighestSkillValue = -1;

		// Token: 0x040001C0 RID: 448
		private int _cachedRepelledSiegeAssaultCount;

		// Token: 0x040001C1 RID: 449
		private int _cachedCapturedTownAloneCount;

		// Token: 0x040001C2 RID: 450
		private int _cachedKingOrQueenKilledInBattle;

		// Token: 0x040001C3 RID: 451
		private int _cachedSuccessfulSiegeCount;

		// Token: 0x040001C4 RID: 452
		private int _cachedWonTournamentCount;

		// Token: 0x040001C5 RID: 453
		private int _cachedSuccessfulBattlesAgainstArmyCount;

		// Token: 0x040001C6 RID: 454
		private int _cachedSuccessfulBattlesAgainstArmyAloneCount;

		// Token: 0x040001C7 RID: 455
		private int _cachedTotalTradeProfit;

		// Token: 0x040001C8 RID: 456
		private int _cachedMaxDailyIncome;

		// Token: 0x040001C9 RID: 457
		private int _cachedDefeatedTroopCount;

		// Token: 0x040001CA RID: 458
		private int _cachedFarthestHeadShot;

		// Token: 0x040001CB RID: 459
		private ItemObject _butter;

		// Token: 0x040001CC RID: 460
		private List<Settlement> _orderedSettlementList = new List<Settlement>();

		// Token: 0x040001CD RID: 461
		private int[] _settlementIntegerSetList;

		// Token: 0x0200008D RID: 141
		private class AchievementMissionLogic : MissionLogic
		{
			// Token: 0x060006AA RID: 1706 RVA: 0x00023CC8 File Offset: 0x00021EC8
			public AchievementMissionLogic(Action<Agent, Agent> onAgentRemoved, Action<Agent, WeaponComponentData, BoneBodyPartType, int> onAgentHitAction)
			{
				this.OnAgentRemovedAction = onAgentRemoved;
				this.OnAgentHitAction = onAgentHitAction;
			}

			// Token: 0x060006AB RID: 1707 RVA: 0x00023CDE File Offset: 0x00021EDE
			public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
			{
				Action<Agent, Agent> onAgentRemovedAction = this.OnAgentRemovedAction;
				if (onAgentRemovedAction == null)
				{
					return;
				}
				onAgentRemovedAction(affectedAgent, affectorAgent);
			}

			// Token: 0x060006AC RID: 1708 RVA: 0x00023CF2 File Offset: 0x00021EF2
			public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
			{
				Action<Agent, WeaponComponentData, BoneBodyPartType, int> onAgentHitAction = this.OnAgentHitAction;
				if (onAgentHitAction == null)
				{
					return;
				}
				onAgentHitAction(affectorAgent, attackerWeapon, blow.VictimBodyPart, (int)hitDistance);
			}

			// Token: 0x04000296 RID: 662
			private Action<Agent, Agent> OnAgentRemovedAction;

			// Token: 0x04000297 RID: 663
			private Action<Agent, WeaponComponentData, BoneBodyPartType, int> OnAgentHitAction;
		}
	}
}
