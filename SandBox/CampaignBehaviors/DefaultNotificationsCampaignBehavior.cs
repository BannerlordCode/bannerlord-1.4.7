using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D8 RID: 216
	public class DefaultNotificationsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x00048F10 File Offset: 0x00047110
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnRelationChanged));
			CampaignEvents.HeroLevelledUp.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroLevelledUp));
			CampaignEvents.HeroGainedSkill.AddNonSerializedListener(this, new Action<Hero, SkillObject, int, bool>(this.OnHeroGainedSkill));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedFaction));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnArmyCreated));
			CampaignEvents.OnPlayerArmyLeaderChangedBehaviorEvent.AddNonSerializedListener(this, new Action(this.OnPlayerArmyLeaderChangedBehaviorEvent));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.RenownGained.AddNonSerializedListener(this, new Action<Hero, int, bool>(this.OnRenownGained));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.OnHeroesMarried));
			CampaignEvents.PartyRemovedFromArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyRemovedFromArmy));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.OnPartyJoinedArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyJoinedArmy));
			CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnChildConceived));
			CampaignEvents.OnGivenBirthEvent.AddNonSerializedListener(this, new Action<Hero, List<Hero>, int>(this.OnGivenBirth));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroOrPartyTradedGold.AddNonSerializedListener(this, new Action<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ValueTuple<int, string>, bool>(this.OnHeroOrPartyTradedGold));
			CampaignEvents.OnTroopsDesertedEvent.AddNonSerializedListener(this, new Action<MobileParty, TroopRoster>(this.OnTroopsDeserted));
			CampaignEvents.ClanTierIncrease.AddNonSerializedListener(this, new Action<Clan, bool>(this.OnClanTierIncreased));
			CampaignEvents.OnSiegeBombardmentHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, SiegeBombardTargets>(this.OnSiegeBombardmentHit));
			CampaignEvents.OnSiegeBombardmentWallHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool>(this.OnSiegeBombardmentWallHit));
			CampaignEvents.OnSiegeEngineDestroyedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineDestroyed));
			CampaignEvents.BattleStarted.AddNonSerializedListener(this, new Action<PartyBase, PartyBase, object, bool>(this.OnBattleStarted));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.ItemsLooted.AddNonSerializedListener(this, new Action<MobileParty, ItemRoster>(this.OnItemsLooted));
			CampaignEvents.OnHideoutSpottedEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnHideoutSpotted));
			CampaignEvents.OnHeroSharedFoodWithAnotherHeroEvent.AddNonSerializedListener(this, new Action<Hero, Hero, float>(this.OnHeroSharedFoodWithAnotherHero));
			CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(this.OnQuestStarted));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.QuestLogAddedEvent.AddNonSerializedListener(this, new Action<QuestBase, bool>(this.OnQuestLogAdded));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnPrisonerTaken));
			CampaignEvents.CharacterBecameFugitiveEvent.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroBecameFugitive));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
			CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
			CampaignEvents.HeroOrPartyGaveItem.AddNonSerializedListener(this, new Action<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ItemRosterElement, bool>(this.OnHeroOrPartyGaveItem));
			CampaignEvents.RebellionFinished.AddNonSerializedListener(this, new Action<Settlement, Clan>(this.OnRebellionFinished));
			CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
			CampaignEvents.OnBuildingLevelChangedEvent.AddNonSerializedListener(this, new Action<Town, Building, int>(this.OnBuildingLevelChanged));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			CampaignEvents.OnPartyAddedToMapEventEvent.AddNonSerializedListener(this, new Action<PartyBase>(this.OnPartyAddedToMapEvent));
			CampaignEvents.OnCallToWarAgreementStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom, Kingdom>(this.OnCallToWarAgreementStarted));
			CampaignEvents.OnCallToWarAgreementEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom, Kingdom>(this.OnCallToWarAgreementEnded));
			CampaignEvents.OnAllianceStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceStarted));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEnded));
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00049358 File Offset: 0x00047558
		private void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
			if (Clan.PlayerClan.IsUnderMercenaryService && (kingdom1.Clans.Contains(Clan.PlayerClan) || kingdom2.Clans.Contains(Clan.PlayerClan)))
			{
				TextObject textObject = new TextObject("{=hymvJALX}The {KINGDOM_1} and the {KINGDOM_2} have formed an alliance.", null);
				textObject.SetTextVariable("KINGDOM_1", kingdom1.Name);
				textObject.SetTextVariable("KINGDOM_2", kingdom2.Name);
				MBInformationManager.AddQuickInformation(textObject, 5000, null, null, "");
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000493D8 File Offset: 0x000475D8
		private void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			if (kingdom1.Clans.Contains(Clan.PlayerClan) || kingdom2.Clans.Contains(Clan.PlayerClan))
			{
				TextObject textObject = new TextObject("{=KC8CqquX}The alliance between the {KINGDOM_1} and the {KINGDOM_2} is dissolved.", null);
				textObject.SetTextVariable("KINGDOM_1", kingdom1.Name);
				textObject.SetTextVariable("KINGDOM_2", kingdom2.Name);
				MBInformationManager.AddQuickInformation(textObject, 5000, null, null, "");
			}
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0004944C File Offset: 0x0004764C
		private void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			if (Clan.PlayerClan.IsUnderMercenaryService && callingKingdom.Clans.Contains(Clan.PlayerClan))
			{
				TextObject textObject = new TextObject("{=zVmDmLCW}Your realm called the {CALLED_KINGDOM} to war against the {KINGDOM_TO_CALL_TO_WAR_AGAINST}.", null);
				textObject.SetTextVariable("CALLED_KINGDOM", calledKingdom.Name);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				MBInformationManager.AddQuickInformation(textObject, 5000, null, null, "");
			}
			if (Clan.PlayerClan.IsUnderMercenaryService && calledKingdom.Clans.Contains(Clan.PlayerClan))
			{
				TextObject textObject2 = new TextObject("{=2ihQeId5}The {CALLING_KINGDOM} has called your realm to war against the {KINGDOM_TO_CALL_TO_WAR_AGAINST}.", null);
				textObject2.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
				textObject2.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				MBInformationManager.AddQuickInformation(textObject2, 5000, null, null, "");
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00049518 File Offset: 0x00047718
		private void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			if (callingKingdom.Clans.Contains(Clan.PlayerClan))
			{
				TextObject textObject = new TextObject("{=ocNWAQsu}The call that your realm issued to the {CALLED_KINGDOM}, to go to war against the {KINGDOM_TO_CALL_TO_WAR_AGAINST}, has ended.", null);
				textObject.SetTextVariable("CALLED_KINGDOM", calledKingdom.Name);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				MBInformationManager.AddQuickInformation(textObject, 5000, null, null, "");
			}
			if (calledKingdom.Clans.Contains(Clan.PlayerClan))
			{
				TextObject textObject2 = new TextObject("{=6eHPy57Z}The call that the {CALLING_KINGDOM} issued to your realm, to go to war against the {KINGDOM_TO_CALL_TO_WAR_AGAINST}, has ended.", null);
				textObject2.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
				textObject2.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				MBInformationManager.AddQuickInformation(textObject2, 5000, null, null, "");
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000495CC File Offset: 0x000477CC
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (mobileParty != null && mobileParty.IsTargetingPort && settlement.SiegeEvent != null && !settlement.SiegeEvent.IsBlockadeActive && settlement.SiegeEvent.BesiegerCamp.IsBesiegerSideParty(MobileParty.MainParty))
			{
				TextObject textObject = new TextObject("{=dPE4A7fO}{ENEMY_PARTY_NAME} has entered the town with {MAN_COUNT} men from the port.", null);
				textObject.SetTextVariable("ENEMY_PARTY_NAME", mobileParty.Name);
				textObject.SetTextVariable("MAN_COUNT", mobileParty.MemberRoster.TotalManCount);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00049654 File Offset: 0x00047854
		private void OnPartyAddedToMapEvent(PartyBase involvedParty)
		{
			if (involvedParty.LeaderHero != null && involvedParty.LeaderHero.Clan == Clan.PlayerClan && involvedParty != PartyBase.MainParty && involvedParty.Side == BattleSideEnum.Defender && involvedParty.MapEvent.AttackerSide.LeaderParty != null)
			{
				bool flag = false;
				using (IEnumerator<PartyBase> enumerator = involvedParty.MapEvent.InvolvedParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == PartyBase.MainParty)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					Settlement settlement = Hero.MainHero.HomeSettlement;
					float num = Campaign.MapDiagonalSquared;
					foreach (Settlement settlement2 in Settlement.All)
					{
						if (settlement2.IsVillage || settlement2.IsFortification)
						{
							float num3;
							float num2 = (involvedParty.IsMobile ? Campaign.Current.Models.MapDistanceModel.GetDistance(involvedParty.MobileParty, settlement2, false, MobileParty.NavigationType.All, out num3) : Campaign.Current.Models.MapDistanceModel.GetDistance(involvedParty.Settlement, settlement2, false, false, MobileParty.NavigationType.All));
							if (num2 < num)
							{
								num = num2;
								settlement = settlement2;
							}
						}
					}
					if (settlement != null)
					{
						TextObject textObject = GameTexts.FindText("str_party_attacked", null);
						textObject.SetTextVariable("CLAN_PARTY_NAME", involvedParty.Name);
						textObject.SetTextVariable("ENEMY_PARTY_NAME", involvedParty.MapEvent.AttackerSide.LeaderParty.Name);
						textObject.SetTextVariable("SETTLEMENT_NAME", settlement.Name);
						MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
					}
				}
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00049814 File Offset: 0x00047A14
		private void OnCompanionRemoved(Hero hero, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (detail == RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord)
			{
				TextObject textObject = new TextObject("{=2Lj0WkSF}{COMPANION.NAME} is now a {?COMPANION.GENDER}noblewoman{?}lord{\\?} of the {KINGDOM}.", null);
				textObject.SetCharacterProperties("COMPANION", hero.CharacterObject, false);
				textObject.SetTextVariable("KINGDOM", Clan.PlayerClan.Kingdom.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "event:/ui/notification/relation");
			}
			if (detail == RemoveCompanionAction.RemoveCompanionDetail.Fire)
			{
				TextObject textObject2 = new TextObject("{=4zdyeTGn}{COMPANION.NAME} left your clan.", null);
				textObject2.SetCharacterProperties("COMPANION", hero.CharacterObject, false);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "event:/ui/notification/relation");
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00049897 File Offset: 0x00047A97
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Tuple<bool, float>>>("_foodNotificationList", ref this._foodNotificationList);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x000498AB File Offset: 0x00047AAB
		private void OnHourlyTick()
		{
			if (MobileParty.MainParty.Army != null)
			{
				this.CheckFoodNotifications();
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000498C0 File Offset: 0x00047AC0
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
			if (details == IssueBase.IssueUpdateDetails.PlayerSentTroopsToQuest)
			{
				TextObject textObject = GameTexts.FindText("str_issue_updated", details.ToString());
				textObject.SetTextVariable("ISSUE_NAME", issue.Title);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "event:/ui/notification/quest_start");
				return;
			}
			if (details == IssueBase.IssueUpdateDetails.SentTroopsFinishedQuest)
			{
				TextObject textObject2 = GameTexts.FindText("str_issue_updated", details.ToString());
				textObject2.SetTextVariable("ISSUE_NAME", issue.Title);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "event:/ui/notification/quest_finished");
				return;
			}
			if (details == IssueBase.IssueUpdateDetails.SentTroopsFailedQuest)
			{
				TextObject textObject3 = GameTexts.FindText("str_issue_updated", details.ToString());
				textObject3.SetTextVariable("ISSUE_NAME", issue.Title);
				MBInformationManager.AddQuickInformation(textObject3, 0, null, null, "event:/ui/notification/quest_fail");
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0004997D File Offset: 0x00047B7D
		private void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
			if (!hideInformation)
			{
				TextObject textObject = GameTexts.FindText("str_quest_log_added", null);
				textObject.SetTextVariable("TITLE", quest.Title);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "event:/ui/notification/quest_update");
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000499AC File Offset: 0x00047BAC
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (detail == QuestBase.QuestCompleteDetails.Success)
			{
				TextObject textObject = GameTexts.FindText("str_quest_completed", detail.ToString());
				textObject.SetTextVariable("QUEST_TITLE", quest.Title);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "event:/ui/notification/quest_finished");
				return;
			}
			if (detail - QuestBase.QuestCompleteDetails.Cancel > 3)
			{
				return;
			}
			TextObject textObject2 = GameTexts.FindText("str_quest_completed", detail.ToString());
			textObject2.SetTextVariable("QUEST_TITLE", quest.Title);
			MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "event:/ui/notification/quest_fail");
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00049A31 File Offset: 0x00047C31
		private void OnQuestStarted(QuestBase quest)
		{
			TextObject textObject = GameTexts.FindText("str_quest_started", null);
			textObject.SetTextVariable("QUEST_TITLE", quest.Title);
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "event:/ui/notification/quest_start");
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00049A60 File Offset: 0x00047C60
		private void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotifyPlayer)
		{
			if (hero.Clan == Clan.PlayerClan && !doNotNotifyPlayer)
			{
				TextObject textObject;
				if (hero.PartyBelongedTo != null)
				{
					textObject = GameTexts.FindText("str_party_gained_renown", null);
					textObject.SetTextVariable("PARTY", hero.PartyBelongedTo.Name);
				}
				else
				{
					textObject = GameTexts.FindText("str_clan_gained_renown", null);
				}
				textObject.SetTextVariable("NEW_RENOWN", string.Format("{0:0.#}", hero.Clan.Renown));
				textObject.SetTextVariable("AMOUNT_TO_ADD", gainedRenown);
				textObject.SetTextVariable("CLAN", hero.Clan.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00049B14 File Offset: 0x00047D14
		private void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
			if (party == PartyBase.MainParty)
			{
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_hideout_spotted", null).ToString(), new Color(1f, 0f, 0f, 1f)));
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00049B51 File Offset: 0x00047D51
		private void OnHeroBecameFugitive(Hero hero, bool showNotification)
		{
			if (showNotification && hero.Clan == Clan.PlayerClan)
			{
				TextObject textObject = GameTexts.FindText("str_fugitive_news", null);
				textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00049B90 File Offset: 0x00047D90
		private void OnPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			if (prisoner.Clan == Clan.PlayerClan)
			{
				TextObject textObject = GameTexts.FindText("str_on_prisoner_taken", null);
				if (capturer.IsSettlement && capturer.Settlement.IsTown)
				{
					TextObject textObject2 = GameTexts.FindText("str_garrison_party_name", null);
					textObject2.SetTextVariable("MAJOR_PARTY_LEADER", capturer.Settlement.Name);
					textObject.SetTextVariable("CAPTOR_NAME", textObject2);
				}
				else
				{
					textObject.SetTextVariable("CAPTOR_NAME", capturer.Name);
				}
				StringHelpers.SetCharacterProperties("PRISONER", prisoner.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00049C34 File Offset: 0x00047E34
		private void OnHeroPrisonerReleased(Hero hero, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification)
		{
			TextObject textObject = null;
			if (showNotification)
			{
				if (hero.Clan == Clan.PlayerClan)
				{
					if (detail <= EndCaptivityDetail.ReleasedAfterEscape)
					{
						textObject = GameTexts.FindText("str_on_prisoner_released_main_clan", detail.ToString());
					}
					else
					{
						textObject = GameTexts.FindText("str_on_prisoner_released_main_clan_default", null);
					}
				}
				else if (party != null && party.IsSettlement && party.Settlement.IsFortification && party.Settlement.OwnerClan == Clan.PlayerClan)
				{
					if (detail == EndCaptivityDetail.ReleasedAfterEscape)
					{
						textObject = GameTexts.FindText("str_on_prisoner_released_escaped_from_settlement", null);
						textObject.SetTextVariable("SETTLEMENT", party.Settlement.Name);
					}
				}
				else if (party != null && party.IsMobile && party.MobileParty == MobileParty.MainParty && detail == EndCaptivityDetail.ReleasedAfterEscape)
				{
					textObject = GameTexts.FindText("str_on_prisoner_released_escaped_from_party", null);
				}
				if (textObject != null)
				{
					StringHelpers.SetCharacterProperties("PRISONER", hero.CharacterObject, textObject, false);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00049D30 File Offset: 0x00047F30
		private void OnBattleStarted(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
			Settlement settlement;
			if (showNotification && (settlement = subject as Settlement) != null && settlement.OwnerClan == Clan.PlayerClan && defenderParty.MapEvent != null && defenderParty.MapEvent.DefenderSide.Parties.FindIndexQ<MapEventParty>((MapEventParty p) => p.Party == settlement.Party) >= 0)
			{
				MBTextManager.SetTextVariable("PARTY", (attackerParty.MobileParty.Army != null) ? attackerParty.MobileParty.ArmyName : attackerParty.Name, false);
				MBTextManager.SetTextVariable("FACTION", attackerParty.MapFaction.Name, false);
				MBTextManager.SetTextVariable("SETTLEMENT", settlement.EncyclopediaLinkWithName, false);
				MBInformationManager.AddQuickInformation(new TextObject("{=ASOW1MuQ}Your settlement {SETTLEMENT} is under attack by {PARTY} of {FACTION}!", null), 0, null, null, "");
			}
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00049E14 File Offset: 0x00048014
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			if (siegeEvent.BesiegedSettlement != null && siegeEvent.BesiegedSettlement.OwnerClan == Clan.PlayerClan && siegeEvent.BesiegerCamp.LeaderParty != null)
			{
				MBTextManager.SetTextVariable("PARTY", (siegeEvent.BesiegerCamp.LeaderParty.Army != null) ? siegeEvent.BesiegerCamp.LeaderParty.ArmyName : siegeEvent.BesiegerCamp.LeaderParty.Name, false);
				MBTextManager.SetTextVariable("FACTION", siegeEvent.BesiegerCamp.MapFaction.Name, false);
				MBTextManager.SetTextVariable("SETTLEMENT", siegeEvent.BesiegedSettlement.EncyclopediaLinkWithName, false);
				MBInformationManager.AddQuickInformation(new TextObject("{=3FvGk8k6}Your settlement {SETTLEMENT} is besieged by {PARTY} of {FACTION}!", null), 0, null, null, "");
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00049EDC File Offset: 0x000480DC
		private void OnClanTierIncreased(Clan clan, bool shouldNotify = true)
		{
			if (shouldNotify && clan == Clan.PlayerClan)
			{
				MBTextManager.SetTextVariable("CLAN", clan.Name, false);
				MBTextManager.SetTextVariable("TIER_LEVEL", clan.Tier);
				MBInformationManager.AddQuickInformation(new TextObject("{=No04urXt}{CLAN} tier is increased to {TIER_LEVEL}", null), 0, null, null, "");
			}
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00049F30 File Offset: 0x00048130
		private void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				bool flag = true;
				for (int i = 0; i < items.Count; i++)
				{
					ItemRosterElement elementCopyAtIndex = items.GetElementCopyAtIndex(i);
					int elementNumber = items.GetElementNumber(i);
					MBTextManager.SetTextVariable("NUMBER_OF", elementNumber);
					MBTextManager.SetTextVariable("ITEM", elementCopyAtIndex.EquipmentElement.Item.Name, false);
					if (flag)
					{
						MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_number_of_item", null).ToString(), false);
						flag = false;
					}
					else
					{
						MBTextManager.SetTextVariable("RIGHT", GameTexts.FindText("str_number_of_item", null).ToString(), false);
						MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString(), false);
					}
				}
				MBTextManager.SetTextVariable("PRODUCTS", GameTexts.FindText("str_LEFT_ONLY", null).ToString(), false);
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=GW8ITTMb}You plundered {PRODUCTS}.", null).ToString()));
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0004A02C File Offset: 0x0004822C
		private void OnRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (showNotification && relationChange != 0 && (effectiveHero == Hero.MainHero || effectiveHeroGainedRelationWith == Hero.MainHero))
			{
				Hero hero = (effectiveHero.IsHumanPlayerCharacter ? effectiveHeroGainedRelationWith : effectiveHero);
				TextObject textObject;
				if (hero.Clan == null || hero.Clan == Clan.PlayerClan)
				{
					textObject = ((relationChange > 0) ? GameTexts.FindText("str_your_relation_increased_with_notable", null) : GameTexts.FindText("str_your_relation_decreased_with_notable", null));
					StringHelpers.SetCharacterProperties("HERO", hero.CharacterObject, textObject, false);
				}
				else
				{
					textObject = ((relationChange > 0) ? GameTexts.FindText("str_your_relation_increased_with_clan", null) : GameTexts.FindText("str_your_relation_decreased_with_clan", null));
					textObject.SetTextVariable("CLAN_LEADER", hero.Clan.Name);
				}
				textObject.SetTextVariable("VALUE", hero.GetRelation(Hero.MainHero));
				textObject.SetTextVariable("MAGNITUDE", MathF.Abs(relationChange));
				MBInformationManager.AddQuickInformation(textObject, 0, hero.IsNotable ? hero.CharacterObject : null, null, "event:/ui/notification/relation");
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0004A128 File Offset: 0x00048328
		private void OnHeroLevelledUp(Hero hero, bool shouldNotify)
		{
			if (!shouldNotify)
			{
				return;
			}
			if (hero == Hero.MainHero || hero.Clan == Clan.PlayerClan)
			{
				TextObject textObject = new TextObject("{=3wzCrzEq}{HERO.NAME} gained a level.", null);
				StringHelpers.SetCharacterProperties("HERO", hero.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "event:/ui/notification/levelup");
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0004A17C File Offset: 0x0004837C
		private void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			if (shouldNotify && BannerlordConfig.ReportExperience && (hero == Hero.MainHero || hero.Clan == Clan.PlayerClan || hero.PartyBelongedTo == MobileParty.MainParty || (hero.CompanionOf != null && hero.CompanionOf == Clan.PlayerClan)))
			{
				TextObject textObject = GameTexts.FindText("str_skill_gained_notification", null);
				StringHelpers.SetCharacterProperties("HERO", hero.CharacterObject, textObject, false);
				textObject.SetTextVariable("PLURAL", (change > 1) ? 1 : 0);
				textObject.SetTextVariable("GAINED_POINTS", change);
				textObject.SetTextVariable("SKILL_NAME", skill.Name);
				textObject.SetTextVariable("UPDATED_SKILL_LEVEL", hero.GetSkillValue(skill));
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0004A24C File Offset: 0x0004844C
		private void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
			if (mobileParty == MobileParty.MainParty || mobileParty.Party.Owner == Hero.MainHero)
			{
				TextObject textObject = GameTexts.FindText("str_troops_deserting", null);
				textObject.SetTextVariable("PARTY", mobileParty.Name);
				textObject.SetTextVariable("DESERTER_COUNT", desertedTroops.TotalManCount);
				textObject.SetTextVariable("PLURAL", (desertedTroops.TotalManCount == 1) ? 0 : 1);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0004A2C8 File Offset: 0x000484C8
		private void OnClanChangedFaction(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan || Hero.MainHero.MapFaction == oldKingdom || Hero.MainHero.MapFaction == newKingdom)
			{
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary)
				{
					this.OnMercenaryClanChangedKingdom(clan, oldKingdom, newKingdom);
					return;
				}
				if (showNotification)
				{
					this.OnRegularClanChangedKingdom(clan, oldKingdom, newKingdom);
				}
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0004A318 File Offset: 0x00048518
		private void OnRegularClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
			TextObject textObject;
			if (oldKingdom != null && newKingdom == null)
			{
				textObject = new TextObject("{=WNKkdpN3}The {CLAN_NAME} left the {OLD_FACTION_NAME}.", null);
			}
			else if (oldKingdom == null && newKingdom != null)
			{
				textObject = new TextObject("{=qeiVFn9s}The {CLAN_NAME} joined the {NEW_FACTION_NAME}", null);
			}
			else if (oldKingdom != null && newKingdom != null && oldKingdom != newKingdom)
			{
				textObject = new TextObject("{=HlrGpPkV}The {CLAN_NAME} changed from the {OLD_FACTION_NAME} to the {NEW_FACTION_NAME}.", null);
			}
			else if (oldKingdom != null && oldKingdom == newKingdom && !clan.IsUnderMercenaryService)
			{
				textObject = new TextObject("{=6f9Hs5zp}The {CLAN_NAME} ended its mercenary contract and became a vassal of the {NEW_FACTION_NAME}", null);
			}
			else
			{
				textObject = null;
			}
			if (!TextObject.IsNullOrEmpty(textObject))
			{
				textObject.SetTextVariable("CLAN_NAME", (clan.AliveLords.Count == 1) ? clan.AliveLords[0].Name : clan.Name);
				if (oldKingdom != null)
				{
					textObject.SetTextVariable("OLD_FACTION_NAME", oldKingdom.InformalName);
				}
				if (newKingdom != null)
				{
					textObject.SetTextVariable("NEW_FACTION_NAME", newKingdom.InformalName);
				}
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0004A3F8 File Offset: 0x000485F8
		private void OnMercenaryClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
			if (clan == Clan.PlayerClan || Hero.MainHero.MapFaction == oldKingdom || Hero.MainHero.MapFaction == newKingdom)
			{
				if (oldKingdom != null && (clan == Hero.MainHero.Clan || oldKingdom == Hero.MainHero.MapFaction))
				{
					TextObject textObject = (clan.IsUnderMercenaryService ? new TextObject("{=a2AO5T1Q}The {CLAN_NAME} and the {KINGDOM_NAME} have ended their mercenary contract.", null) : new TextObject("{=g7qhnsnJ}The {CLAN_NAME} clan has left the {KINGDOM_NAME}.", null));
					textObject.SetTextVariable("CLAN_NAME", clan.Name);
					textObject.SetTextVariable("KINGDOM_NAME", oldKingdom.InformalName);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
				if (newKingdom != null && (clan == Hero.MainHero.Clan || newKingdom == Hero.MainHero.MapFaction) && clan.IsUnderMercenaryService)
				{
					TextObject textObject2 = new TextObject("{=AozaGCru}The {CLAN_NAME} and the {KINGDOM_NAME} have signed a mercenary contract.", null);
					textObject2.SetTextVariable("CLAN_NAME", clan.Name);
					textObject2.SetTextVariable("KINGDOM_NAME", newKingdom.InformalName);
					MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
				}
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0004A4FC File Offset: 0x000486FC
		private void OnArmyCreated(Army army)
		{
			if ((army.Kingdom == MobileParty.MainParty.MapFaction && MobileParty.MainParty.Army == null) || this._notificationCheatEnabled)
			{
				TextObject textObject = new TextObject("{=VEHPTzhO}{LEADER.NAME} is gathering an army near {SETTLEMENT}.", null);
				textObject.SetTextVariable("SETTLEMENT", army.AiBehaviorObject.Name);
				StringHelpers.SetCharacterProperties("LEADER", army.LeaderParty.LeaderHero.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, army.LeaderParty.LeaderHero.CharacterObject, null, "");
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0004A58C File Offset: 0x0004878C
		private void OnPlayerArmyLeaderChangedBehaviorEvent()
		{
			MBInformationManager.AddQuickInformation(GameTexts.FindText("str_army_leader_think", "Unknown"), 0, MobileParty.MainParty.Army.LeaderParty.LeaderHero.CharacterObject, null, "");
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0004A5C4 File Offset: 0x000487C4
		private void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
			if ((besiegerParty.Army != null && besiegerParty.Army.Parties.Contains(MobileParty.MainParty)) || besiegerParty == MobileParty.MainParty || (MobileParty.MainParty.CurrentSettlement != null && MobileParty.MainParty.CurrentSettlement == besiegedSettlement))
			{
				TextObject textObject;
				if (target != SiegeBombardTargets.RangedEngines)
				{
					if (target != SiegeBombardTargets.People)
					{
						Debug.FailedAssert("invalid bombardment type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\CampaignBehaviors\\DefaultNotificationsCampaignBehavior.cs", "OnSiegeBombardmentHit", 697);
						textObject = null;
					}
					else
					{
						textObject = ((side == BattleSideEnum.Defender) ? new TextObject("{=7WlQ0Twr}{WEAPON} of {SETTLEMENT} hit some soldiers of {BESIEGER}!", null) : new TextObject("{=ZrMeSyPu}The {WEAPON} of {BESIEGER} hit some soldiers in {SETTLEMENT}!", null));
					}
				}
				else
				{
					textObject = ((side == BattleSideEnum.Defender) ? new TextObject("{=gqdsXVNi}{WEAPON} of {SETTLEMENT} hit ranged engines of {BESIEGER}!", null) : new TextObject("{=FnkYfyGa}the {WEAPON} of {BESIEGER} hit ranged engines of {SETTLEMENT}!", null));
				}
				if (!TextObject.IsNullOrEmpty(textObject))
				{
					textObject.SetTextVariable("WEAPON", weapon.Name);
					textObject.SetTextVariable("BESIEGER", (besiegerParty.Army != null) ? besiegerParty.Army.Name : besiegerParty.Name);
					textObject.SetTextVariable("SETTLEMENT", besiegedSettlement.Name);
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				}
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0004A6E0 File Offset: 0x000488E0
		private void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
			if ((besiegerParty.Army != null && besiegerParty.Army.Parties.Contains(MobileParty.MainParty)) || besiegerParty == MobileParty.MainParty || (MobileParty.MainParty.CurrentSettlement != null && MobileParty.MainParty.CurrentSettlement == besiegedSettlement))
			{
				TextObject textObject = new TextObject("{=8Wy1OCsr}The {WEAPON} of {BESIEGER} hit wall of {SETTLEMENT}!", null);
				textObject.SetTextVariable("WEAPON", weapon.Name);
				textObject.SetTextVariable("BESIEGER", (besiegerParty.Army != null) ? besiegerParty.Army.Name : besiegerParty.Name);
				textObject.SetTextVariable("SETTLEMENT", besiegedSettlement.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				if (isWallCracked)
				{
					TextObject textObject2 = new TextObject("{=uJNvbag5}The walls of {SETTLEMENT} has been cracked.", null);
					textObject2.SetTextVariable("SETTLEMENT", besiegedSettlement.Name);
					MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
				}
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0004A7C8 File Offset: 0x000489C8
		private void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
			if ((besiegerParty.Army != null && besiegerParty.Army.Parties.Contains(MobileParty.MainParty)) || besiegerParty == MobileParty.MainParty || (MobileParty.MainParty.CurrentSettlement != null && MobileParty.MainParty.CurrentSettlement == besiegedSettlement))
			{
				TextObject textObject = ((side == BattleSideEnum.Attacker) ? new TextObject("{=fa8sla4i}The {SIEGE_ENGINE} of {BESIEGER_PARTY} has been destroyed.", null) : new TextObject("{=U9zFz8Et}The {SIEGE_ENGINE} of {SIEGED_SETTLEMENT_NAME} has been cracked.", null));
				textObject.SetTextVariable("SIEGED_SETTLEMENT_NAME", besiegedSettlement.Name);
				textObject.SetTextVariable("BESIEGER_PARTY", besiegerParty.Name);
				textObject.SetTextVariable("SIEGE_ENGINE", destroyedEngine.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0004A878 File Offset: 0x00048A78
		private void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giverSide, ValueTuple<Hero, PartyBase> recipientSide, ValueTuple<int, string> transactionAmountAndId, bool displayNotification)
		{
			if (displayNotification)
			{
				int item = transactionAmountAndId.Item1;
				MBTextManager.SetTextVariable("GOLD_AMOUNT", MathF.Abs(item));
				bool flag = giverSide.Item1 == Hero.MainHero || giverSide.Item2 == PartyBase.MainParty;
				bool flag2 = recipientSide.Item1 == Hero.MainHero || recipientSide.Item2 == PartyBase.MainParty;
				if ((flag && item > 0) || (flag2 && item < 0))
				{
					InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_gold_removed_with_icon", null).ToString(), "event:/ui/notification/coins_negative"));
					return;
				}
				if ((flag && item < 0) || (flag2 && item > 0))
				{
					InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_you_received_gold_with_icon", null).ToString(), "event:/ui/notification/coins_positive"));
				}
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0004A93C File Offset: 0x00048B3C
		private void OnPartyJoinedArmy(MobileParty party)
		{
			if (party.Army == MobileParty.MainParty.Army && party.LeaderHero != party.Army.LeaderParty.LeaderHero)
			{
				TextObject textObject = new TextObject("{=wD1YDmmg}{PARTY_NAME} has enlisted in {ARMY_NAME}.", null);
				textObject.SetTextVariable("PARTY_NAME", party.Name);
				textObject.SetTextVariable("ARMY_NAME", party.Army.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0004A9B8 File Offset: 0x00048BB8
		private void OnPartyAttachedAnotherParty(MobileParty party)
		{
			if (party.Army == MobileParty.MainParty.Army && party.LeaderHero != party.Army.LeaderParty.LeaderHero)
			{
				TextObject textObject = new TextObject("{=0aGYre5B}{LEADER.LINK} has arrived at {ARMY_NAME}.", null);
				StringHelpers.SetCharacterProperties("LEADER", party.LeaderHero.CharacterObject, textObject, false);
				textObject.SetTextVariable("ARMY_NAME", party.Army.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0004AA3C File Offset: 0x00048C3C
		private void OnPartyRemovedFromArmy(MobileParty party)
		{
			if (party.Army == MobileParty.MainParty.Army)
			{
				TextObject textObject = new TextObject("{=ApG1xg7O}{PARTY_NAME} has left {ARMY_NAME}.", null);
				textObject.SetTextVariable("PARTY_NAME", party.Name);
				textObject.SetTextVariable("ARMY_NAME", party.Army.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
			}
			if (party == MobileParty.MainParty)
			{
				this.CheckFoodNotifications();
			}
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0004AAAC File Offset: 0x00048CAC
		private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			if (isPlayersArmy)
			{
				this.CheckFoodNotifications();
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0004AAB8 File Offset: 0x00048CB8
		private void OnHeroesMarried(Hero firstHero, Hero secondHero, bool showNotification)
		{
			if (showNotification && (firstHero.Clan == Clan.PlayerClan || secondHero.Clan == Clan.PlayerClan))
			{
				StringHelpers.SetCharacterProperties("MARRIED_TO", firstHero.CharacterObject, null, false);
				StringHelpers.SetCharacterProperties("MARRIED_HERO", secondHero.CharacterObject, null, false);
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_hero_married_hero", null), 0, null, null, "");
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0004AB20 File Offset: 0x00048D20
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero previousOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege && settlement.MapFaction == Hero.MainHero.MapFaction && settlement.IsFortification)
			{
				TextObject textObject = (Hero.MainHero.MapFaction.IsKingdomFaction ? new TextObject("{=OiCCfAeC}{SETTLEMENT} is taken. Election is started.", null) : new TextObject("{=2VRTPyZY}{SETTLEMENT} is yours.", null));
				textObject.SetTextVariable("SETTLEMENT", settlement.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0004AB94 File Offset: 0x00048D94
		private void OnChildConceived(Hero mother)
		{
			if (mother == Hero.MainHero)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=ZhpT2qVh}You have just learned that you are with child.", null), 0, null, null, "");
				return;
			}
			if (mother == Hero.MainHero.Spouse)
			{
				TextObject textObject = new TextObject("{=7v2dMsW5}Your spouse {MOTHER} has just learned that she is with child.", null);
				textObject.SetTextVariable("MOTHER", mother.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				return;
			}
			if (mother.Clan == Clan.PlayerClan)
			{
				TextObject textObject2 = new TextObject("{=2AGIxoUN}Your clan member {MOTHER} has just learned that she is with child.", null);
				textObject2.SetTextVariable("MOTHER", mother.Name);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0004AC34 File Offset: 0x00048E34
		private void OnGivenBirth(Hero mother, List<Hero> aliveOffsprings, int stillbornCount)
		{
			if (mother == Hero.MainHero || mother == Hero.MainHero.Spouse || mother.Clan == Clan.PlayerClan)
			{
				TextObject textObject;
				if (mother == Hero.MainHero)
				{
					textObject = new TextObject("{=oIA9lkpc}You have given birth to {DELIVERED_CHILDREN}.", null);
				}
				else if (mother == Hero.MainHero.Spouse)
				{
					textObject = new TextObject("{=TsbjAsxs}Your wife {MOTHER.NAME} has given birth to {DELIVERED_CHILDREN}.", null);
				}
				else
				{
					textObject = new TextObject("{=LsDRCPp0}Your clan member {MOTHER.NAME} has given birth to {DELIVERED_CHILDREN}.", null);
				}
				if (stillbornCount == 2)
				{
					textObject.SetTextVariable("DELIVERED_CHILDREN", new TextObject("{=Sn9a1Aba}two stillborn babies", null));
				}
				else if (stillbornCount == 1 && aliveOffsprings.Count == 0)
				{
					textObject.SetTextVariable("DELIVERED_CHILDREN", new TextObject("{=qWLq2y84}a stillborn baby", null));
				}
				else if (stillbornCount == 1 && aliveOffsprings.Count == 1)
				{
					textObject.SetTextVariable("DELIVERED_CHILDREN", new TextObject("{=vn13OyFV}one healthy and one stillborn baby", null));
				}
				else if (stillbornCount == 0 && aliveOffsprings.Count == 1)
				{
					textObject.SetTextVariable("DELIVERED_CHILDREN", new TextObject("{=lbRMmZym}a healthy baby", null));
				}
				else if (stillbornCount == 0 && aliveOffsprings.Count == 2)
				{
					textObject.SetTextVariable("DELIVERED_CHILDREN", new TextObject("{=EPbHr2DX}two healthy babies", null));
				}
				StringHelpers.SetCharacterProperties("MOTHER", mother.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0004AD77 File Offset: 0x00048F77
		private void OnHeroKilled(Hero victimHero, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (showNotification && victimHero != null && victimHero.Clan == Clan.PlayerClan)
			{
				MBInformationManager.AddQuickInformation(CharacterHelper.GetDeathNotification(victimHero, killer, detail), 0, null, null, "");
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0004ADA2 File Offset: 0x00048FA2
		private void OnHeroSharedFoodWithAnotherHero(Hero supporterHero, Hero supportedHero, float influence)
		{
			if (supporterHero == Hero.MainHero)
			{
				this._foodNotificationList.Add(new Tuple<bool, float>(true, influence));
				return;
			}
			if (supportedHero == Hero.MainHero)
			{
				this._foodNotificationList.Add(new Tuple<bool, float>(false, influence));
			}
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0004ADDC File Offset: 0x00048FDC
		private void CheckFoodNotifications()
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			bool flag2 = false;
			foreach (Tuple<bool, float> tuple in this._foodNotificationList)
			{
				if (tuple.Item1)
				{
					num += tuple.Item2;
					flag = true;
				}
				else
				{
					num2 += tuple.Item2;
					flag2 = true;
				}
			}
			if (flag)
			{
				TextObject textObject = new TextObject("{=B0eBWPoO} You shared your food with starving soldiers of your army. You gained {INFLUENCE}{INFLUENCE_ICON}.", null);
				textObject.SetTextVariable("INFLUENCE", num.ToString("0.00"));
				textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
			}
			if (flag2)
			{
				TextObject textObject2 = new TextObject("{=qQ71Ux7D} Your army shared their food with your starving soldiers. You spent {INFLUENCE}{INFLUENCE_ICON}.", null);
				textObject2.SetTextVariable("INFLUENCE", num2.ToString("0.00"));
				textObject2.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
				InformationManager.DisplayMessage(new InformationMessage(textObject2.ToString()));
			}
			this._foodNotificationList.Clear();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0004AEF4 File Offset: 0x000490F4
		private void OnClanDestroyed(Clan destroyedClan)
		{
			TextObject textObject = new TextObject("{=PBq1FyrJ}{CLAN_NAME} clan was destroyed.", null);
			textObject.SetTextVariable("CLAN_NAME", destroyedClan.Name);
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0004AF20 File Offset: 0x00049120
		private void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
			if (showNotification && itemRosterElement.Amount > 0)
			{
				TextObject textObject = null;
				if (giver.Item1 == Hero.MainHero || giver.Item2 == PartyBase.MainParty)
				{
					if (receiver.Item1 != null)
					{
						textObject = GameTexts.FindText("str_hero_gave_item_to_hero", null);
						StringHelpers.SetCharacterProperties("HERO", receiver.Item1.CharacterObject, textObject, false);
					}
					else
					{
						textObject = GameTexts.FindText("str_hero_gave_item_to_party", null);
						textObject.SetTextVariable("PARTY_NAME", receiver.Item2.Name);
					}
				}
				else if (receiver.Item1 == Hero.MainHero || receiver.Item2 == PartyBase.MainParty)
				{
					if (giver.Item1 != null)
					{
						textObject = GameTexts.FindText("str_hero_received_item_from_hero", null);
						StringHelpers.SetCharacterProperties("HERO", giver.Item1.CharacterObject, textObject, false);
					}
					else
					{
						textObject = GameTexts.FindText("str_hero_received_item_from_party", null);
						textObject.SetTextVariable("PARTY_NAME", giver.Item2.Name);
					}
				}
				if (textObject != null)
				{
					textObject.SetTextVariable("ITEM", itemRosterElement.EquipmentElement.Item.Name);
					textObject.SetTextVariable("COUNT", itemRosterElement.Amount);
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				}
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0004B06C File Offset: 0x0004926C
		private void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			TextObject textObject = GameTexts.FindText("str_rebellion_finished", null);
			textObject.SetTextVariable("SETTLEMENT", settlement.Name);
			textObject.SetTextVariable("RULER", oldOwnerClan.Leader.Name);
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0004B0BC File Offset: 0x000492BC
		private void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			if (winner.IsHero && winner.HeroObject.Clan == Clan.PlayerClan && winner.HeroObject.PartyBelongedTo == MobileParty.MainParty)
			{
				TextObject textObject = GameTexts.FindText("str_tournament_companion_won_prize", null);
				textObject.SetTextVariable("ITEM_NAME", prize.Name);
				textObject.SetCharacterProperties("COMPANION", winner, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0004B130 File Offset: 0x00049330
		private void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
			if (levelChange > 0 && town.OwnerClan == Clan.PlayerClan)
			{
				TextObject textObject = ((building.CurrentLevel == 1) ? GameTexts.FindText("str_building_completed", null) : GameTexts.FindText("str_building_level_gained", null));
				textObject.SetTextVariable("SETTLEMENT_NAME", town.Name);
				textObject.SetTextVariable("BUILDING_NAME", building.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), new Color(0f, 1f, 0f, 1f)));
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0004B1BC File Offset: 0x000493BC
		private void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			if (detail == TeleportHeroAction.TeleportationDetail.ImmediateTeleportToParty && targetParty == MobileParty.MainParty && MobileParty.MainParty.IsActive)
			{
				TextObject textObject = new TextObject("{=abux36nq}{HERO.NAME} joined your party.", null);
				textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
			if (detail == TeleportHeroAction.TeleportationDetail.ImmediateTeleportToPartyAsPartyLeader && targetParty.ActualClan == Clan.PlayerClan && targetParty != MobileParty.MainParty)
			{
				TextObject textObject2 = new TextObject("{=xxSlIDCW}{HERO.NAME} has joined {?HERO.GENDER}her{?}his{\\?} party and assumed command.", null);
				textObject2.SetCharacterProperties("HERO", hero.CharacterObject, false);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
			}
			if (detail == TeleportHeroAction.TeleportationDetail.ImmediateTeleportToSettlement && hero.Clan == Clan.PlayerClan && targetSettlement.IsFortification && targetSettlement.Town.Governor == hero && hero.HeroState == Hero.CharacterStates.Traveling)
			{
				TextObject textObject3 = new TextObject("{=btynhBAn}The new governor of {SETTLEMENT}, {HERO.NAME}, has arrived and taken up the reins of office.", null);
				textObject3.SetCharacterProperties("HERO", hero.CharacterObject, false);
				textObject3.SetTextVariable("SETTLEMENT", targetSettlement.Name);
				MBInformationManager.AddQuickInformation(textObject3, 0, null, null, "");
			}
		}

		// Token: 0x04000497 RID: 1175
		private List<Tuple<bool, float>> _foodNotificationList = new List<Tuple<bool, float>>();

		// Token: 0x04000498 RID: 1176
		private bool _notificationCheatEnabled;
	}
}
