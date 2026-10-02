using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003E RID: 62
	public class CampaignEvents : CampaignEventReceiver
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00021DBD File Offset: 0x0001FFBD
		private static CampaignEvents Instance
		{
			get
			{
				return Campaign.Current.CampaignEvents;
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00021DCC File Offset: 0x0001FFCC
		public override void RemoveListeners(object obj)
		{
			this._heroLevelledUp.ClearListeners(obj);
			this._onHomeHideoutChangedEvent.ClearListeners(obj);
			this._heroGainedSkill.ClearListeners(obj);
			this._heroRelationChanged.ClearListeners(obj);
			this._questLogAddedEvent.ClearListeners(obj);
			this._issueLogAddedEvent.ClearListeners(obj);
			this._onCharacterCreationIsOverEvent.ClearListeners(obj);
			this._clanChangedKingdom.ClearListeners(obj);
			this._onClanDefected.ClearListeners(obj);
			this._onClanCreatedEvent.ClearListeners(obj);
			this._onHeroJoinedPartyEvent.ClearListeners(obj);
			this._partyAttachedParty.ClearListeners(obj);
			this._nearbyPartyAddedToPlayerMapEvent.ClearListeners(obj);
			this._armyCreated.ClearListeners(obj);
			this._armyGathered.ClearListeners(obj);
			this._armyDispersed.ClearListeners(obj);
			this._villageStateChanged.ClearListeners(obj);
			this._settlementEntered.ClearListeners(obj);
			this._afterSettlementEntered.ClearListeners(obj);
			this._beforeSettlementEntered.ClearListeners(obj);
			this._mercenaryTroopChangedInTown.ClearListeners(obj);
			this._mercenaryNumberChangedInTown.ClearListeners(obj);
			this._alleyOwnerChanged.ClearListeners(obj);
			this._alleyOccupiedByPlayer.ClearListeners(obj);
			this._alleyClearedByPlayer.ClearListeners(obj);
			this._romanticStateChanged.ClearListeners(obj);
			this._warDeclared.ClearListeners(obj);
			this._battleStarted.ClearListeners(obj);
			this._rebellionFinished.ClearListeners(obj);
			this._townRebelliousStateChanged.ClearListeners(obj);
			this._rebelliousClanDisbandedAtSettlement.ClearListeners(obj);
			this._mobilePartyDestroyed.ClearListeners(obj);
			this._mobilePartyCreated.ClearListeners(obj);
			this._mapInteractableCreated.ClearListeners(obj);
			this._mapInteractableDestroyed.ClearListeners(obj);
			this._mobilePartyQuestStatusChanged.ClearListeners(obj);
			this._heroKilled.ClearListeners(obj);
			this._characterDefeated.ClearListeners(obj);
			this._heroPrisonerTaken.ClearListeners(obj);
			this._onPartySizeChangedEvent.ClearListeners(obj);
			this._characterBecameFugitiveEvent.ClearListeners(obj);
			this._playerMetHero.ClearListeners(obj);
			this._playerLearnsAboutHero.ClearListeners(obj);
			this._renownGained.ClearListeners(obj);
			this._barterablesRequested.ClearListeners(obj);
			this._crimeRatingChanged.ClearListeners(obj);
			this._newCompanionAdded.ClearListeners(obj);
			this._afterMissionStarted.ClearListeners(obj);
			this._gameMenuOpened.ClearListeners(obj);
			this._makePeace.ClearListeners(obj);
			this._kingdomCreated.ClearListeners(obj);
			this._kingdomDestroyed.ClearListeners(obj);
			this._canKingdomBeDiscontinued.ClearListeners(obj);
			this._villageBeingRaided.ClearListeners(obj);
			this._villageLooted.ClearListeners(obj);
			this._mapEventEnded.ClearListeners(obj);
			this._mapEventStarted.ClearListeners(obj);
			this._prisonersChangeInSettlement.ClearListeners(obj);
			this._onMissionStartedEvent.ClearListeners(obj);
			this._beforeMissionOpenedEvent.ClearListeners(obj);
			this._onPartyRemovedEvent.ClearListeners(obj);
			this._onPartyLeaderChangedEvent.ClearListeners(obj);
			this._banditPartyRecruited.ClearListeners(obj);
			this._onSettlementOwnerChangedEvent.ClearListeners(obj);
			this._onGovernorChangedEvent.ClearListeners(obj);
			this._onSettlementLeftEvent.ClearListeners(obj);
			this._weeklyTickEvent.ClearListeners(obj);
			this._dailyTickEvent.ClearListeners(obj);
			this._dailyTickPartyEvent.ClearListeners(obj);
			this._hourlyTickEvent.ClearListeners(obj);
			this._tickEvent.ClearListeners(obj);
			this._onSessionLaunchedEvent.ClearListeners(obj);
			this._onAfterSessionLaunchedEvent.ClearListeners(obj);
			this._onNewGameCreatedPartialFollowUpEvent.ClearListeners(obj);
			this._onNewGameCreatedPartialFollowUpEndEvent.ClearListeners(obj);
			this._onNewGameCreatedEvent.ClearListeners(obj);
			this._onGameLoadedEvent.ClearListeners(obj);
			this._onBarterAcceptedEvent.ClearListeners(obj);
			this._onBarterCanceledEvent.ClearListeners(obj);
			this._onGameEarlyLoadedEvent.ClearListeners(obj);
			this._onGameLoadFinishedEvent.ClearListeners(obj);
			this._aiHourlyTickEvent.ClearListeners(obj);
			this._tickPartialHourlyAiEvent.ClearListeners(obj);
			this._onPartyJoinedArmyEvent.ClearListeners(obj);
			this._onPartyRemovedFromArmyEvent.ClearListeners(obj);
			this._onMissionEndedEvent.ClearListeners(obj);
			this._onPlayerBattleEndEvent.ClearListeners(obj);
			this._onPlayerBoardGameOver.ClearListeners(obj);
			this._onRansomOfferedToPlayer.ClearListeners(obj);
			this._onRansomOfferCancelled.ClearListeners(obj);
			this._onPeaceOfferedToPlayer.ClearListeners(obj);
			this._onTradeAgreementSignedEvent.ClearListeners(obj);
			this._onPeaceOfferResolved.ClearListeners(obj);
			this._onMarriageOfferedToPlayerEvent.ClearListeners(obj);
			this._onMarriageOfferCanceledEvent.ClearListeners(obj);
			this._onVassalOrMercenaryServiceOfferedToPlayerEvent.ClearListeners(obj);
			this._onVassalOrMercenaryServiceOfferCanceledEvent.ClearListeners(obj);
			this._afterGameMenuInitializedEvent.ClearListeners(obj);
			this._beforeGameMenuOpenedEvent.ClearListeners(obj);
			this._onChildConceived.ClearListeners(obj);
			this._onGivenBirthEvent.ClearListeners(obj);
			this._missionTickEvent.ClearListeners(obj);
			this._armyOverlaySetDirty.ClearListeners(obj);
			this._onPlayerArmyLeaderChangedBehaviorEvent.ClearListeners(obj);
			this._partyVisibilityChanged.ClearListeners(obj);
			this._onHeroCreated.ClearListeners(obj);
			this._heroOccupationChangedEvent.ClearListeners(obj);
			this._onHeroWounded.ClearListeners(obj);
			this._playerDesertedBattle.ClearListeners(obj);
			this._companionRemoved.ClearListeners(obj);
			this._trackLostEvent.ClearListeners(obj);
			this._trackDetectedEvent.ClearListeners(obj);
			this._locationCharactersAreReadyToSpawn.ClearListeners(obj);
			this._locationCharactersSimulatedSpawned.ClearListeners(obj);
			this._playerUpgradedTroopsEvent.ClearListeners(obj);
			this._onHeroCombatHitEvent.ClearListeners(obj);
			this._characterPortraitPopUpOpenedEvent.ClearListeners(obj);
			this._characterPortraitPopUpClosedEvent.ClearListeners(obj);
			this._playerStartTalkFromMenu.ClearListeners(obj);
			this._gameMenuOptionSelectedEvent.ClearListeners(obj);
			this._playerStartRecruitmentEvent.ClearListeners(obj);
			this._onAgentJoinedConversationEvent.ClearListeners(obj);
			this._onConversationEnded.ClearListeners(obj);
			this._beforeHeroesMarried.ClearListeners(obj);
			this._onTroopsDesertedEvent.ClearListeners(obj);
			this._onBeforePlayerCharacterChangedEvent.ClearListeners(obj);
			this._onPlayerCharacterChangedEvent.ClearListeners(obj);
			this._onClanLeaderChangedEvent.ClearListeners(obj);
			this._onSiegeEventStartedEvent.ClearListeners(obj);
			this._onPlayerSiegeStartedEvent.ClearListeners(obj);
			this._onSiegeEventEndedEvent.ClearListeners(obj);
			this._siegeAftermathAppliedEvent.ClearListeners(obj);
			this._onSiegeBombardmentHitEvent.ClearListeners(obj);
			this._onSiegeBombardmentWallHitEvent.ClearListeners(obj);
			this._onSiegeEngineDestroyedEvent.ClearListeners(obj);
			this._kingdomDecisionAdded.ClearListeners(obj);
			this._kingdomDecisionCancelled.ClearListeners(obj);
			this._kingdomDecisionConcluded.ClearListeners(obj);
			this._childEducationCompleted.ClearListeners(obj);
			this._heroComesOfAge.ClearListeners(obj);
			this._heroGrowsOutOfInfancyEvent.ClearListeners(obj);
			this._heroReachesTeenAgeEvent.ClearListeners(obj);
			this._onCheckForIssueEvent.ClearListeners(obj);
			this._onIssueUpdatedEvent.ClearListeners(obj);
			this._onTroopRecruitedEvent.ClearListeners(obj);
			this._onTroopGivenToSettlementEvent.ClearListeners(obj);
			this._onItemSoldEvent.ClearListeners(obj);
			this._onCaravanTransactionCompletedEvent.ClearListeners(obj);
			this._onPrisonerSoldEvent.ClearListeners(obj);
			this._heroPrisonerReleased.ClearListeners(obj);
			this._heroOrPartyTradedGold.ClearListeners(obj);
			this._heroOrPartyGaveItem.ClearListeners(obj);
			this._perkOpenedEvent.ClearListeners(obj);
			this._playerTraitChangedEvent.ClearListeners(obj);
			this._onPartyDisbandedEvent.ClearListeners(obj);
			this._onPartyDisbandStartedEvent.ClearListeners(obj);
			this._onPartyDisbandCanceledEvent.ClearListeners(obj);
			this._itemsLooted.ClearListeners(obj);
			this._hideoutSpottedEvent.ClearListeners(obj);
			this._hideoutBattleCompletedEvent.ClearListeners(obj);
			this._hideoutDeactivatedEvent.ClearListeners(obj);
			this._heroSharedFoodWithAnotherHeroEvent.ClearListeners(obj);
			this._onQuestCompletedEvent.ClearListeners(obj);
			this._itemProducedEvent.ClearListeners(obj);
			this._itemConsumedEvent.ClearListeners(obj);
			this._onQuestStartedEvent.ClearListeners(obj);
			this._onPartyConsumedFoodEvent.ClearListeners(obj);
			this._siegeCompletedEvent.ClearListeners(obj);
			this._afterSiegeCompletedEvent.ClearListeners(obj);
			this._raidCompletedEvent.ClearListeners(obj);
			this._forceVolunteersCompletedEvent.ClearListeners(obj);
			this._forceSuppliesCompletedEvent.ClearListeners(obj);
			this._onBeforeMainCharacterDiedEvent.ClearListeners(obj);
			this._onGameOverEvent.ClearListeners(obj);
			this._onClanDestroyedEvent.ClearListeners(obj);
			this._onNewIssueCreatedEvent.ClearListeners(obj);
			this._onIssueOwnerChangedEvent.ClearListeners(obj);
			this._onTutorialCompletedEvent.ClearListeners(obj);
			this._collectAvailableTutorialsEvent.ClearListeners(obj);
			this._playerEliminatedFromTournament.ClearListeners(obj);
			this._playerStartedTournamentMatch.ClearListeners(obj);
			this._tournamentStarted.ClearListeners(obj);
			this._tournamentFinished.ClearListeners(obj);
			this._tournamentCancelled.ClearListeners(obj);
			this._playerInventoryExchangeEvent.ClearListeners(obj);
			this._onItemsDiscardedByPlayerEvent.ClearListeners(obj);
			this._onNewItemCraftedEvent.ClearListeners(obj);
			this._craftingPartUnlockedEvent.ClearListeners(obj);
			this._onWorkshopInitializedEvent.ClearListeners(obj);
			this._onWorkshopOwnerChangedEvent.ClearListeners(obj);
			this._onWorkshopTypeChangedEvent.ClearListeners(obj);
			this._persuasionProgressCommittedEvent.ClearListeners(obj);
			this._onBeforeSaveEvent.ClearListeners(obj);
			this._onPrisonerTakenEvent.ClearListeners(obj);
			this._onPrisonerReleasedEvent.ClearListeners(obj);
			this._onMainPartyPrisonerRecruitedEvent.ClearListeners(obj);
			this._onPrisonerDonatedToSettlementEvent.ClearListeners(obj);
			this._onEquipmentSmeltedByHero.ClearListeners(obj);
			this._onPlayerTradeProfit.ClearListeners(obj);
			this._onBeforeHeroKilled.ClearListeners(obj);
			this._onBuildingLevelChangedEvent.ClearListeners(obj);
			this._hourlyTickSettlementEvent.ClearListeners(obj);
			this._hourlyTickClanEvent.ClearListeners(obj);
			this._onUnitRecruitedEvent.ClearListeners(obj);
			this._trackDetectedEvent.ClearListeners(obj);
			this._trackLostEvent.ClearListeners(obj);
			this._onTradeRumorIsTakenEvent.ClearListeners(obj);
			this._siegeEngineBuiltEvent.ClearListeners(obj);
			this._dailyTickHeroEvent.ClearListeners(obj);
			this._dailyTickSettlementEvent.ClearListeners(obj);
			this._hourlyTickPartyEvent.ClearListeners(obj);
			this._dailyTickClanEvent.ClearListeners(obj);
			this._villageBecomeNormal.ClearListeners(obj);
			this._clanTierIncrease.ClearListeners(obj);
			this._dailyTickTownEvent.ClearListeners(obj);
			this._onHeroChangedClan.ClearListeners(obj);
			this._onHeroGetsBusy.ClearListeners(obj);
			this._onSaveStartedEvent.ClearListeners(obj);
			this._onSaveOverEvent.ClearListeners(obj);
			this._collectMetadataEntriesEvent.ClearListeners(obj);
			this._onPlayerBodyPropertiesChangedEvent.ClearListeners(obj);
			this._rulingClanChanged.ClearListeners(obj);
			this._onCollectLootItems.ClearListeners(obj);
			this._onLootDistributedToPartyEvent.ClearListeners(obj);
			this._onHeroTeleportationRequestedEvent.ClearListeners(obj);
			this._onPartyLeaderChangeOfferCanceledEvent.ClearListeners(obj);
			this._canBeGovernorOrHavePartyRoleEvent.ClearListeners(obj);
			this._canHeroLeadPartyEvent.ClearListeners(obj);
			this._canMarryEvent.ClearListeners(obj);
			this._canHeroDieEvent.ClearListeners(obj);
			this._canHeroBecomePrisonerEvent.ClearListeners(obj);
			this._canHeroEquipmentBeChangedEvent.ClearListeners(obj);
			this._canHaveCampaignIssues.ClearListeners(obj);
			this._isSettlementBusy.ClearListeners(obj);
			this._canMoveToSettlementEvent.ClearListeners(obj);
			this._onQuarterDailyPartyTick.ClearListeners(obj);
			this._onMainPartyStarving.ClearListeners(obj);
			this._onClanInfluenceChangedEvent.ClearListeners(obj);
			this._onPlayerPartyKnockedOrKilledTroopEvent.ClearListeners(obj);
			this._onPlayerEarnedGoldFromAssetEvent.ClearListeners(obj);
			this._onClanEarnedGoldFromTributeEvent.ClearListeners(obj);
			this._onPlayerJoinedTournamentEvent.ClearListeners(obj);
			this._onHeroUnregisteredEvent.ClearListeners(obj);
			this._onConfigChanged.ClearListeners(obj);
			this._onCraftingOrderCompleted.ClearListeners(obj);
			this._onItemsRefined.ClearListeners(obj);
			this._onMapEventContinuityNeedsUpdate.ClearListeners(obj);
			this._onPlayerAgentSpawned.ClearListeners(obj);
			this._onBeforePlayerAgentSpawn.ClearListeners(obj);
			this._perkResetEvent.ClearListeners(obj);
			this._onHeirSelectionOver.ClearListeners(obj);
			this._onMobilePartyRaftStateChanged.ClearListeners(obj);
			this._onCharacterCreationInitialized.ClearListeners(obj);
			this._onShipDestroyedEvent.ClearListeners(obj);
			this._onShipRepairedEvent.ClearListeners(obj);
			this._onShipCreatedEvent.ClearListeners(obj);
			this._onPartyLeftArmyEvent.ClearListeners(obj);
			this._onPartyAddedToMapEventEvent.ClearListeners(obj);
			this._onIncidentResolvedEvent.ClearListeners(obj);
			this._onFigureheadUnlockedEvent.ClearListeners(obj);
			this._onShipOwnerChangedEvent.ClearListeners(obj);
			this._onMobilePartyNavigationStateChangedEvent.ClearListeners(obj);
			this._onMobilePartyJoinedToSiegeEventEvent.ClearListeners(obj);
			this._onMobilePartyLeftSiegeEventEvent.ClearListeners(obj);
			this._onBlockadeActivatedEvent.ClearListeners(obj);
			this._onBlockadeDeactivatedEvent.ClearListeners(obj);
			this._onMercenaryServiceStartedEvent.ClearListeners(obj);
			this._onMercenaryServiceEndedEvent.ClearListeners(obj);
			this._canPlayerMeetWithHeroAfterConversationEvent.ClearListeners(obj);
			this._onMapMarkerCreatedEvent.ClearListeners(obj);
			this._onMapMarkerRemovedEvent.ClearListeners(obj);
			this._onAllianceStartedEvent.ClearListeners(obj);
			this._onAllianceEndedEvent.ClearListeners(obj);
			this._onCallToWarAgreementStartedEvent.ClearListeners(obj);
			this._onCallToWarAgreementEndedEvent.ClearListeners(obj);
			this._onHeroActivatedEvent.ClearListeners(obj);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00022AC9 File Offset: 0x00020CC9
		public static IMbEvent OnPlayerBodyPropertiesChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerBodyPropertiesChangedEvent;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00022AD5 File Offset: 0x00020CD5
		public override void OnPlayerBodyPropertiesChanged()
		{
			CampaignEvents.Instance._onPlayerBodyPropertiesChangedEvent.Invoke();
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00022AE6 File Offset: 0x00020CE6
		public static IMbEvent<BarterData> BarterablesRequested
		{
			get
			{
				return CampaignEvents.Instance._barterablesRequested;
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00022AF2 File Offset: 0x00020CF2
		public override void OnBarterablesRequested(BarterData args)
		{
			CampaignEvents.Instance._barterablesRequested.Invoke(args);
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00022B04 File Offset: 0x00020D04
		public static IMbEvent<Hero, bool> HeroLevelledUp
		{
			get
			{
				return CampaignEvents.Instance._heroLevelledUp;
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00022B10 File Offset: 0x00020D10
		public override void OnHeroLevelledUp(Hero hero, bool shouldNotify = true)
		{
			this._heroLevelledUp.Invoke(hero, shouldNotify);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00022B1F File Offset: 0x00020D1F
		public static IMbEvent<BanditPartyComponent, Hideout> OnHomeHideoutChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onHomeHideoutChangedEvent;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00022B2B File Offset: 0x00020D2B
		public override void OnHomeHideoutChanged(BanditPartyComponent banditPartyComponent, Hideout oldHomeHideout)
		{
			CampaignEvents.Instance._onHomeHideoutChangedEvent.Invoke(banditPartyComponent, oldHomeHideout);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00022B3E File Offset: 0x00020D3E
		public static IMbEvent<Hero, SkillObject, int, bool> HeroGainedSkill
		{
			get
			{
				return CampaignEvents.Instance._heroGainedSkill;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00022B4A File Offset: 0x00020D4A
		public override void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			this._heroGainedSkill.Invoke(hero, skill, change, shouldNotify);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00022B5C File Offset: 0x00020D5C
		public static IMbEvent OnCharacterCreationIsOverEvent
		{
			get
			{
				return CampaignEvents.Instance._onCharacterCreationIsOverEvent;
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00022B68 File Offset: 0x00020D68
		public override void OnCharacterCreationIsOver()
		{
			CampaignEvents.Instance._onCharacterCreationIsOverEvent.Invoke();
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00022B79 File Offset: 0x00020D79
		public static IMbEvent<Hero, bool> HeroCreated
		{
			get
			{
				return CampaignEvents.Instance._onHeroCreated;
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00022B85 File Offset: 0x00020D85
		public override void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
			this._onHeroCreated.Invoke(hero, isBornNaturally);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00022B94 File Offset: 0x00020D94
		public static IMbEvent<Hero, Hero.CharacterStates> OnHeroActivatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroActivatedEvent;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00022BA0 File Offset: 0x00020DA0
		public override void OnHeroActivated(Hero hero, Hero.CharacterStates previousState)
		{
			this._onHeroActivatedEvent.Invoke(hero, previousState);
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00022BAF File Offset: 0x00020DAF
		public static IMbEvent<Hero, Occupation> HeroOccupationChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._heroOccupationChangedEvent;
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00022BBB File Offset: 0x00020DBB
		public override void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
			this._heroOccupationChangedEvent.Invoke(hero, oldOccupation);
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00022BCA File Offset: 0x00020DCA
		public static IMbEvent<Hero> HeroWounded
		{
			get
			{
				return CampaignEvents.Instance._onHeroWounded;
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00022BD6 File Offset: 0x00020DD6
		public override void OnHeroWounded(Hero woundedHero)
		{
			this._onHeroWounded.Invoke(woundedHero);
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00022BE4 File Offset: 0x00020DE4
		public static IMbEvent<Hero, Hero, List<Barterable>> OnBarterAcceptedEvent
		{
			get
			{
				return CampaignEvents.Instance._onBarterAcceptedEvent;
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00022BF0 File Offset: 0x00020DF0
		public override void OnBarterAccepted(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			CampaignEvents.Instance._onBarterAcceptedEvent.Invoke(offererHero, otherHero, barters);
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00022C04 File Offset: 0x00020E04
		public static IMbEvent<Hero, Hero, List<Barterable>> OnBarterCanceledEvent
		{
			get
			{
				return CampaignEvents.Instance._onBarterCanceledEvent;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00022C10 File Offset: 0x00020E10
		public override void OnBarterCanceled(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			CampaignEvents.Instance._onBarterCanceledEvent.Invoke(offererHero, otherHero, barters);
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00022C24 File Offset: 0x00020E24
		public static IMbEvent<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero> HeroRelationChanged
		{
			get
			{
				return CampaignEvents.Instance._heroRelationChanged;
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00022C30 File Offset: 0x00020E30
		public override void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			CampaignEvents.Instance._heroRelationChanged.Invoke(effectiveHero, effectiveHeroGainedRelationWith, relationChange, showNotification, detail, originalHero, originalGainedRelationWith);
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00022C4C File Offset: 0x00020E4C
		public static IMbEvent<QuestBase, bool> QuestLogAddedEvent
		{
			get
			{
				return CampaignEvents.Instance._questLogAddedEvent;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00022C58 File Offset: 0x00020E58
		public override void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
			CampaignEvents.Instance._questLogAddedEvent.Invoke(quest, hideInformation);
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00022C6B File Offset: 0x00020E6B
		public static IMbEvent<IssueBase, bool> IssueLogAddedEvent
		{
			get
			{
				return CampaignEvents.Instance._issueLogAddedEvent;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00022C77 File Offset: 0x00020E77
		public override void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
			CampaignEvents.Instance._issueLogAddedEvent.Invoke(issue, hideInformation);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00022C8A File Offset: 0x00020E8A
		public static IMbEvent<Clan, bool> ClanTierIncrease
		{
			get
			{
				return CampaignEvents.Instance._clanTierIncrease;
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00022C96 File Offset: 0x00020E96
		public override void OnClanTierChanged(Clan clan, bool shouldNotify = true)
		{
			CampaignEvents.Instance._clanTierIncrease.Invoke(clan, shouldNotify);
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00022CA9 File Offset: 0x00020EA9
		public static IMbEvent<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool> OnClanChangedKingdomEvent
		{
			get
			{
				return CampaignEvents.Instance._clanChangedKingdom;
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00022CB5 File Offset: 0x00020EB5
		public override void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			CampaignEvents.Instance._clanChangedKingdom.Invoke(clan, oldKingdom, newKingdom, detail, showNotification);
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00022CCD File Offset: 0x00020ECD
		public static IMbEvent<Clan, Kingdom, Kingdom> OnClanDefectedEvent
		{
			get
			{
				return CampaignEvents.Instance._onClanDefected;
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00022CD9 File Offset: 0x00020ED9
		public override void OnClanDefected(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
			CampaignEvents.Instance._onClanDefected.Invoke(clan, oldKingdom, newKingdom);
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00022CED File Offset: 0x00020EED
		public static IMbEvent<Clan, bool> OnClanCreatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onClanCreatedEvent;
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00022CF9 File Offset: 0x00020EF9
		public override void OnClanCreated(Clan clan, bool isCompanion)
		{
			CampaignEvents.Instance._onClanCreatedEvent.Invoke(clan, isCompanion);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00022D0C File Offset: 0x00020F0C
		public static IMbEvent<Hero, MobileParty> OnHeroJoinedPartyEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroJoinedPartyEvent;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00022D18 File Offset: 0x00020F18
		public override void OnHeroJoinedParty(Hero hero, MobileParty mobileParty)
		{
			CampaignEvents.Instance._onHeroJoinedPartyEvent.Invoke(hero, mobileParty);
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00022D2B File Offset: 0x00020F2B
		public static IMbEvent<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ValueTuple<int, string>, bool> HeroOrPartyTradedGold
		{
			get
			{
				return CampaignEvents.Instance._heroOrPartyTradedGold;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00022D37 File Offset: 0x00020F37
		public override void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
			CampaignEvents.Instance._heroOrPartyTradedGold.Invoke(giver, recipient, goldAmount, showNotification);
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00022D4D File Offset: 0x00020F4D
		public static IMbEvent<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ItemRosterElement, bool> HeroOrPartyGaveItem
		{
			get
			{
				return CampaignEvents.Instance._heroOrPartyGaveItem;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00022D59 File Offset: 0x00020F59
		public override void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
			CampaignEvents.Instance._heroOrPartyGaveItem.Invoke(giver, receiver, itemRosterElement, showNotification);
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00022D6F File Offset: 0x00020F6F
		public static IMbEvent<MobileParty> BanditPartyRecruited
		{
			get
			{
				return CampaignEvents.Instance._banditPartyRecruited;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00022D7B File Offset: 0x00020F7B
		public override void OnBanditPartyRecruited(MobileParty banditParty)
		{
			CampaignEvents.Instance._banditPartyRecruited.Invoke(banditParty);
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00022D8D File Offset: 0x00020F8D
		public static IMbEvent<KingdomDecision, bool> KingdomDecisionAdded
		{
			get
			{
				return CampaignEvents.Instance._kingdomDecisionAdded;
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00022D99 File Offset: 0x00020F99
		public override void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			CampaignEvents.Instance._kingdomDecisionAdded.Invoke(decision, isPlayerInvolved);
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00022DAC File Offset: 0x00020FAC
		public static IMbEvent<KingdomDecision, bool> KingdomDecisionCancelled
		{
			get
			{
				return CampaignEvents.Instance._kingdomDecisionCancelled;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00022DB8 File Offset: 0x00020FB8
		public override void OnKingdomDecisionCancelled(KingdomDecision decision, bool isPlayerInvolved)
		{
			CampaignEvents.Instance._kingdomDecisionCancelled.Invoke(decision, isPlayerInvolved);
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00022DCB File Offset: 0x00020FCB
		public static IMbEvent<KingdomDecision, DecisionOutcome, bool> KingdomDecisionConcluded
		{
			get
			{
				return CampaignEvents.Instance._kingdomDecisionConcluded;
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00022DD7 File Offset: 0x00020FD7
		public override void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			CampaignEvents.Instance._kingdomDecisionConcluded.Invoke(decision, chosenOutcome, isPlayerInvolved);
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00022DEB File Offset: 0x00020FEB
		public static IMbEvent<MobileParty> PartyAttachedAnotherParty
		{
			get
			{
				return CampaignEvents.Instance._partyAttachedParty;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00022DF7 File Offset: 0x00020FF7
		public override void OnPartyAttachedAnotherParty(MobileParty mobileParty)
		{
			CampaignEvents.Instance._partyAttachedParty.Invoke(mobileParty);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00022E09 File Offset: 0x00021009
		public static IMbEvent<MobileParty> NearbyPartyAddedToPlayerMapEvent
		{
			get
			{
				return CampaignEvents.Instance._nearbyPartyAddedToPlayerMapEvent;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00022E15 File Offset: 0x00021015
		public override void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
			CampaignEvents.Instance._nearbyPartyAddedToPlayerMapEvent.Invoke(mobileParty);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00022E27 File Offset: 0x00021027
		public static IMbEvent<Army> ArmyCreated
		{
			get
			{
				return CampaignEvents.Instance._armyCreated;
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00022E33 File Offset: 0x00021033
		public override void OnArmyCreated(Army army)
		{
			CampaignEvents.Instance._armyCreated.Invoke(army);
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00022E45 File Offset: 0x00021045
		public static IMbEvent<Army, Army.ArmyDispersionReason, bool> ArmyDispersed
		{
			get
			{
				return CampaignEvents.Instance._armyDispersed;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00022E51 File Offset: 0x00021051
		public override void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			CampaignEvents.Instance._armyDispersed.Invoke(army, reason, isPlayersArmy);
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00022E65 File Offset: 0x00021065
		public static IMbEvent<Army, IMapPoint> ArmyGathered
		{
			get
			{
				return CampaignEvents.Instance._armyGathered;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00022E71 File Offset: 0x00021071
		public override void OnArmyGathered(Army army, IMapPoint gatheringPoint)
		{
			CampaignEvents.Instance._armyGathered.Invoke(army, gatheringPoint);
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00022E84 File Offset: 0x00021084
		public static IMbEvent<Hero, PerkObject> PerkOpenedEvent
		{
			get
			{
				return CampaignEvents.Instance._perkOpenedEvent;
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00022E90 File Offset: 0x00021090
		public override void OnPerkOpened(Hero hero, PerkObject perk)
		{
			CampaignEvents.Instance._perkOpenedEvent.Invoke(hero, perk);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00022EA3 File Offset: 0x000210A3
		public static IMbEvent<Hero, PerkObject> PerkResetEvent
		{
			get
			{
				return CampaignEvents.Instance._perkResetEvent;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00022EAF File Offset: 0x000210AF
		public override void OnPerkReset(Hero hero, PerkObject perk)
		{
			CampaignEvents.Instance._perkResetEvent.Invoke(hero, perk);
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00022EC2 File Offset: 0x000210C2
		public static IMbEvent<TraitObject, int> PlayerTraitChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._playerTraitChangedEvent;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00022ECE File Offset: 0x000210CE
		public override void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
			CampaignEvents.Instance._playerTraitChangedEvent.Invoke(trait, previousLevel);
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00022EE1 File Offset: 0x000210E1
		public static IMbEvent<Village, Village.VillageStates, Village.VillageStates, MobileParty> VillageStateChanged
		{
			get
			{
				return CampaignEvents.Instance._villageStateChanged;
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00022EED File Offset: 0x000210ED
		public override void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
			CampaignEvents.Instance._villageStateChanged.Invoke(village, oldState, newState, raiderParty);
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00022F03 File Offset: 0x00021103
		public static IMbEvent<MobileParty, Settlement, Hero> SettlementEntered
		{
			get
			{
				return CampaignEvents.Instance._settlementEntered;
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00022F0F File Offset: 0x0002110F
		public override void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEvents.Instance._settlementEntered.Invoke(party, settlement, hero);
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00022F23 File Offset: 0x00021123
		public static IMbEvent<MobileParty, Settlement, Hero> AfterSettlementEntered
		{
			get
			{
				return CampaignEvents.Instance._afterSettlementEntered;
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00022F2F File Offset: 0x0002112F
		public override void OnAfterSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEvents.Instance._afterSettlementEntered.Invoke(party, settlement, hero);
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00022F43 File Offset: 0x00021143
		public static IMbEvent<MobileParty, Settlement, Hero> BeforeSettlementEnteredEvent
		{
			get
			{
				return CampaignEvents.Instance._beforeSettlementEntered;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00022F4F File Offset: 0x0002114F
		public override void OnBeforeSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEvents.Instance._beforeSettlementEntered.Invoke(party, settlement, hero);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00022F63 File Offset: 0x00021163
		public static IMbEvent<Town, CharacterObject, CharacterObject> MercenaryTroopChangedInTown
		{
			get
			{
				return CampaignEvents.Instance._mercenaryTroopChangedInTown;
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00022F6F File Offset: 0x0002116F
		public override void OnMercenaryTroopChangedInTown(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
			CampaignEvents.Instance._mercenaryTroopChangedInTown.Invoke(town, oldTroopType, newTroopType);
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00022F83 File Offset: 0x00021183
		public static IMbEvent<Town, int, int> MercenaryNumberChangedInTown
		{
			get
			{
				return CampaignEvents.Instance._mercenaryNumberChangedInTown;
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00022F8F File Offset: 0x0002118F
		public override void OnMercenaryNumberChangedInTown(Town town, int oldNumber, int newNumber)
		{
			CampaignEvents.Instance._mercenaryNumberChangedInTown.Invoke(town, oldNumber, newNumber);
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00022FA3 File Offset: 0x000211A3
		public static IMbEvent<Alley, Hero, Hero> AlleyOwnerChanged
		{
			get
			{
				return CampaignEvents.Instance._alleyOwnerChanged;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00022FAF File Offset: 0x000211AF
		public static IMbEvent<Alley, TroopRoster> AlleyOccupiedByPlayer
		{
			get
			{
				return CampaignEvents.Instance._alleyOccupiedByPlayer;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00022FBB File Offset: 0x000211BB
		public override void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
		{
			CampaignEvents.Instance._alleyOccupiedByPlayer.Invoke(alley, troops);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00022FCE File Offset: 0x000211CE
		public override void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			CampaignEvents.Instance._alleyOwnerChanged.Invoke(alley, newOwner, oldOwner);
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00022FE2 File Offset: 0x000211E2
		public static IMbEvent<Alley> AlleyClearedByPlayer
		{
			get
			{
				return CampaignEvents.Instance._alleyClearedByPlayer;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00022FEE File Offset: 0x000211EE
		public override void OnAlleyClearedByPlayer(Alley alley)
		{
			CampaignEvents.Instance._alleyClearedByPlayer.Invoke(alley);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00023000 File Offset: 0x00021200
		public static IMbEvent<Hero, Hero, Romance.RomanceLevelEnum> RomanticStateChanged
		{
			get
			{
				return CampaignEvents.Instance._romanticStateChanged;
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002300C File Offset: 0x0002120C
		public override void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum romanceLevel)
		{
			CampaignEvents.Instance._romanticStateChanged.Invoke(hero1, hero2, romanceLevel);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00023020 File Offset: 0x00021220
		public static IMbEvent<Hero, Hero, bool> BeforeHeroesMarried
		{
			get
			{
				return CampaignEvents.Instance._beforeHeroesMarried;
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0002302C File Offset: 0x0002122C
		public override void OnBeforeHeroesMarried(Hero hero1, Hero hero2, bool showNotification = true)
		{
			CampaignEvents.Instance._beforeHeroesMarried.Invoke(hero1, hero2, showNotification);
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00023040 File Offset: 0x00021240
		public static IMbEvent<int, Town> PlayerEliminatedFromTournament
		{
			get
			{
				return CampaignEvents.Instance._playerEliminatedFromTournament;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002304C File Offset: 0x0002124C
		public override void OnPlayerEliminatedFromTournament(int round, Town town)
		{
			CampaignEvents.Instance._playerEliminatedFromTournament.Invoke(round, town);
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0002305F File Offset: 0x0002125F
		public static IMbEvent<Town> PlayerStartedTournamentMatch
		{
			get
			{
				return CampaignEvents.Instance._playerStartedTournamentMatch;
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002306B File Offset: 0x0002126B
		public override void OnPlayerStartedTournamentMatch(Town town)
		{
			CampaignEvents.Instance._playerStartedTournamentMatch.Invoke(town);
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0002307D File Offset: 0x0002127D
		public static IMbEvent<Town> TournamentStarted
		{
			get
			{
				return CampaignEvents.Instance._tournamentStarted;
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00023089 File Offset: 0x00021289
		public override void OnTournamentStarted(Town town)
		{
			CampaignEvents.Instance._tournamentStarted.Invoke(town);
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0002309B File Offset: 0x0002129B
		public static IMbEvent<IFaction, IFaction, DeclareWarAction.DeclareWarDetail> WarDeclared
		{
			get
			{
				return CampaignEvents.Instance._warDeclared;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000230A7 File Offset: 0x000212A7
		public override void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			CampaignEvents.Instance._warDeclared.Invoke(faction1, faction2, declareWarDetail);
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x000230BB File Offset: 0x000212BB
		public static IMbEvent<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject> TournamentFinished
		{
			get
			{
				return CampaignEvents.Instance._tournamentFinished;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000230C7 File Offset: 0x000212C7
		public override void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			CampaignEvents.Instance._tournamentFinished.Invoke(winner, participants, town, prize);
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x000230DD File Offset: 0x000212DD
		public static IMbEvent<Town> TournamentCancelled
		{
			get
			{
				return CampaignEvents.Instance._tournamentCancelled;
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000230E9 File Offset: 0x000212E9
		public override void OnTournamentCancelled(Town town)
		{
			CampaignEvents.Instance._tournamentCancelled.Invoke(town);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x000230FB File Offset: 0x000212FB
		public static IMbEvent<PartyBase, PartyBase, object, bool> BattleStarted
		{
			get
			{
				return CampaignEvents.Instance._battleStarted;
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00023107 File Offset: 0x00021307
		public override void OnStartBattle(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
			CampaignEvents.Instance._battleStarted.Invoke(attackerParty, defenderParty, subject, showNotification);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0002311D File Offset: 0x0002131D
		public static IMbEvent<Settlement, Clan> RebellionFinished
		{
			get
			{
				return CampaignEvents.Instance._rebellionFinished;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00023129 File Offset: 0x00021329
		public override void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			CampaignEvents.Instance._rebellionFinished.Invoke(settlement, oldOwnerClan);
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0002313C File Offset: 0x0002133C
		public static IMbEvent<Town, bool> TownRebelliosStateChanged
		{
			get
			{
				return CampaignEvents.Instance._townRebelliousStateChanged;
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00023148 File Offset: 0x00021348
		public override void TownRebelliousStateChanged(Town town, bool rebelliousState)
		{
			CampaignEvents.Instance._townRebelliousStateChanged.Invoke(town, rebelliousState);
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0002315B File Offset: 0x0002135B
		public static IMbEvent<Settlement, Clan> RebelliousClanDisbandedAtSettlement
		{
			get
			{
				return CampaignEvents.Instance._rebelliousClanDisbandedAtSettlement;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00023167 File Offset: 0x00021367
		public override void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan clan)
		{
			CampaignEvents.Instance._rebelliousClanDisbandedAtSettlement.Invoke(settlement, clan);
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0002317A File Offset: 0x0002137A
		public static IMbEvent<MobileParty, ItemRoster> ItemsLooted
		{
			get
			{
				return CampaignEvents.Instance._itemsLooted;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00023186 File Offset: 0x00021386
		public override void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
			CampaignEvents.Instance._itemsLooted.Invoke(mobileParty, items);
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00023199 File Offset: 0x00021399
		public static IMbEvent<MobileParty, PartyBase> MobilePartyDestroyed
		{
			get
			{
				return CampaignEvents.Instance._mobilePartyDestroyed;
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000231A5 File Offset: 0x000213A5
		public override void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			CampaignEvents.Instance._mobilePartyDestroyed.Invoke(mobileParty, destroyerParty);
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x000231B8 File Offset: 0x000213B8
		public static IMbEvent<MobileParty> MobilePartyCreated
		{
			get
			{
				return CampaignEvents.Instance._mobilePartyCreated;
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000231C4 File Offset: 0x000213C4
		public override void OnMobilePartyCreated(MobileParty party)
		{
			CampaignEvents.Instance._mobilePartyCreated.Invoke(party);
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x000231D6 File Offset: 0x000213D6
		public static IMbEvent<IInteractablePoint> MapInteractableCreated
		{
			get
			{
				return CampaignEvents.Instance._mapInteractableCreated;
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000231E2 File Offset: 0x000213E2
		public override void OnMapInteractableCreated(IInteractablePoint interactable)
		{
			CampaignEvents.Instance._mapInteractableCreated.Invoke(interactable);
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x000231F4 File Offset: 0x000213F4
		public static IMbEvent<IInteractablePoint> MapInteractableDestroyed
		{
			get
			{
				return CampaignEvents.Instance._mapInteractableDestroyed;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00023200 File Offset: 0x00021400
		public override void OnMapInteractableDestroyed(IInteractablePoint interactable)
		{
			CampaignEvents.Instance._mapInteractableDestroyed.Invoke(interactable);
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00023212 File Offset: 0x00021412
		public static IMbEvent<MobileParty, bool> MobilePartyQuestStatusChanged
		{
			get
			{
				return CampaignEvents.Instance._mobilePartyQuestStatusChanged;
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002321E File Offset: 0x0002141E
		public override void OnMobilePartyQuestStatusChanged(MobileParty party, bool isUsedByQuest)
		{
			CampaignEvents.Instance._mobilePartyQuestStatusChanged.Invoke(party, isUsedByQuest);
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00023231 File Offset: 0x00021431
		public static IMbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool> HeroKilledEvent
		{
			get
			{
				return CampaignEvents.Instance._heroKilled;
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002323D File Offset: 0x0002143D
		public override void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEvents.Instance._heroKilled.Invoke(victim, killer, detail, showNotification);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x00023253 File Offset: 0x00021453
		public static IMbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool> BeforeHeroKilledEvent
		{
			get
			{
				return CampaignEvents.Instance._onBeforeHeroKilled;
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0002325F File Offset: 0x0002145F
		public override void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEvents.Instance._onBeforeHeroKilled.Invoke(victim, killer, detail, showNotification);
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00023275 File Offset: 0x00021475
		public static IMbEvent<Hero, int> ChildEducationCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._childEducationCompleted;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00023281 File Offset: 0x00021481
		public override void OnChildEducationCompleted(Hero hero, int age)
		{
			CampaignEvents.Instance._childEducationCompleted.Invoke(hero, age);
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00023294 File Offset: 0x00021494
		public static IMbEvent<Hero> HeroComesOfAgeEvent
		{
			get
			{
				return CampaignEvents.Instance._heroComesOfAge;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000232A0 File Offset: 0x000214A0
		public override void OnHeroComesOfAge(Hero hero)
		{
			CampaignEvents.Instance._heroComesOfAge.Invoke(hero);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000232B2 File Offset: 0x000214B2
		public static IMbEvent<Hero> HeroGrowsOutOfInfancyEvent
		{
			get
			{
				return CampaignEvents.Instance._heroGrowsOutOfInfancyEvent;
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000232BE File Offset: 0x000214BE
		public override void OnHeroGrowsOutOfInfancy(Hero hero)
		{
			CampaignEvents.Instance._heroGrowsOutOfInfancyEvent.Invoke(hero);
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000232D0 File Offset: 0x000214D0
		public static IMbEvent<Hero> HeroReachesTeenAgeEvent
		{
			get
			{
				return CampaignEvents.Instance._heroReachesTeenAgeEvent;
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000232DC File Offset: 0x000214DC
		public override void OnHeroReachesTeenAge(Hero hero)
		{
			CampaignEvents.Instance._heroReachesTeenAgeEvent.Invoke(hero);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x000232EE File Offset: 0x000214EE
		public static IMbEvent<Hero, Hero> CharacterDefeated
		{
			get
			{
				return CampaignEvents.Instance._characterDefeated;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000232FA File Offset: 0x000214FA
		public override void OnCharacterDefeated(Hero winner, Hero loser)
		{
			CampaignEvents.Instance._characterDefeated.Invoke(winner, loser);
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0002330D File Offset: 0x0002150D
		public static IMbEvent<Kingdom, Clan> RulingClanChanged
		{
			get
			{
				return CampaignEvents.Instance._rulingClanChanged;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00023319 File Offset: 0x00021519
		public override void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
			CampaignEvents.Instance._rulingClanChanged.Invoke(kingdom, oldRulingClan);
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0002332C File Offset: 0x0002152C
		public static IMbEvent<PartyBase, Hero> HeroPrisonerTaken
		{
			get
			{
				return CampaignEvents.Instance._heroPrisonerTaken;
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00023338 File Offset: 0x00021538
		public override void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			CampaignEvents.Instance._heroPrisonerTaken.Invoke(capturer, prisoner);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0002334B File Offset: 0x0002154B
		public static IMbEvent<Hero, PartyBase, IFaction, EndCaptivityDetail, bool> HeroPrisonerReleased
		{
			get
			{
				return CampaignEvents.Instance._heroPrisonerReleased;
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00023357 File Offset: 0x00021557
		public override void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification = true)
		{
			CampaignEvents.Instance._heroPrisonerReleased.Invoke(prisoner, party, capturerFaction, detail, showNotification);
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0002336F File Offset: 0x0002156F
		public static IMbEvent<Hero, bool> CharacterBecameFugitiveEvent
		{
			get
			{
				return CampaignEvents.Instance._characterBecameFugitiveEvent;
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002337B File Offset: 0x0002157B
		public override void OnCharacterBecameFugitive(Hero hero, bool showNotification)
		{
			CampaignEvents.Instance._characterBecameFugitiveEvent.Invoke(hero, showNotification);
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0002338E File Offset: 0x0002158E
		public static IMbEvent<Hero> OnPlayerMetHeroEvent
		{
			get
			{
				return CampaignEvents.Instance._playerMetHero;
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002339A File Offset: 0x0002159A
		public override void OnPlayerMetHero(Hero hero)
		{
			CampaignEvents.Instance._playerMetHero.Invoke(hero);
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x000233AC File Offset: 0x000215AC
		public static IMbEvent<Hero> OnPlayerLearnsAboutHeroEvent
		{
			get
			{
				return CampaignEvents.Instance._playerLearnsAboutHero;
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000233B8 File Offset: 0x000215B8
		public override void OnPlayerLearnsAboutHero(Hero hero)
		{
			CampaignEvents.Instance._playerLearnsAboutHero.Invoke(hero);
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x000233CA File Offset: 0x000215CA
		public static IMbEvent<Hero, int, bool> RenownGained
		{
			get
			{
				return CampaignEvents.Instance._renownGained;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000233D6 File Offset: 0x000215D6
		public override void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
			CampaignEvents.Instance._renownGained.Invoke(hero, gainedRenown, doNotNotify);
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x000233EA File Offset: 0x000215EA
		public static IMbEvent<IFaction, float> CrimeRatingChanged
		{
			get
			{
				return CampaignEvents.Instance._crimeRatingChanged;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000233F6 File Offset: 0x000215F6
		public override void OnCrimeRatingChanged(IFaction kingdom, float deltaCrimeAmount)
		{
			CampaignEvents.Instance._crimeRatingChanged.Invoke(kingdom, deltaCrimeAmount);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00023409 File Offset: 0x00021609
		public static IMbEvent<Hero> NewCompanionAdded
		{
			get
			{
				return CampaignEvents.Instance._newCompanionAdded;
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00023415 File Offset: 0x00021615
		public override void OnNewCompanionAdded(Hero newCompanion)
		{
			CampaignEvents.Instance._newCompanionAdded.Invoke(newCompanion);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00023427 File Offset: 0x00021627
		public static IMbEvent<IMission> AfterMissionStarted
		{
			get
			{
				return CampaignEvents.Instance._afterMissionStarted;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00023433 File Offset: 0x00021633
		public override void OnAfterMissionStarted(IMission iMission)
		{
			CampaignEvents.Instance._afterMissionStarted.Invoke(iMission);
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00023445 File Offset: 0x00021645
		public static IMbEvent<MenuCallbackArgs> GameMenuOpened
		{
			get
			{
				return CampaignEvents.Instance._gameMenuOpened;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00023451 File Offset: 0x00021651
		public override void OnGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEvents.Instance._gameMenuOpened.Invoke(args);
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00023463 File Offset: 0x00021663
		public static IMbEvent<MenuCallbackArgs> AfterGameMenuInitializedEvent
		{
			get
			{
				return CampaignEvents.Instance._afterGameMenuInitializedEvent;
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002346F File Offset: 0x0002166F
		public override void AfterGameMenuInitialized(MenuCallbackArgs args)
		{
			CampaignEvents.Instance._afterGameMenuInitializedEvent.Invoke(args);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00023481 File Offset: 0x00021681
		public static IMbEvent<MenuCallbackArgs> BeforeGameMenuOpenedEvent
		{
			get
			{
				return CampaignEvents.Instance._beforeGameMenuOpenedEvent;
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002348D File Offset: 0x0002168D
		public override void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEvents.Instance._beforeGameMenuOpenedEvent.Invoke(args);
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0002349F File Offset: 0x0002169F
		public static IMbEvent<IFaction, IFaction, MakePeaceAction.MakePeaceDetail> MakePeace
		{
			get
			{
				return CampaignEvents.Instance._makePeace;
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000234AB File Offset: 0x000216AB
		public override void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			CampaignEvents.Instance._makePeace.Invoke(side1Faction, side2Faction, detail);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x000234BF File Offset: 0x000216BF
		public static IMbEvent<Kingdom> KingdomDestroyedEvent
		{
			get
			{
				return CampaignEvents.Instance._kingdomDestroyed;
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000234CB File Offset: 0x000216CB
		public override void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			CampaignEvents.Instance._kingdomDestroyed.Invoke(destroyedKingdom);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x000234DD File Offset: 0x000216DD
		public static ReferenceIMBEvent<Kingdom, bool> CanKingdomBeDiscontinuedEvent
		{
			get
			{
				return CampaignEvents.Instance._canKingdomBeDiscontinued;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000234E9 File Offset: 0x000216E9
		public override void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
			CampaignEvents.Instance._canKingdomBeDiscontinued.Invoke(kingdom, ref result);
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x000234FC File Offset: 0x000216FC
		public static IMbEvent<Kingdom> KingdomCreatedEvent
		{
			get
			{
				return CampaignEvents.Instance._kingdomCreated;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00023508 File Offset: 0x00021708
		public override void OnKingdomCreated(Kingdom createdKingdom)
		{
			CampaignEvents.Instance._kingdomCreated.Invoke(createdKingdom);
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0002351A File Offset: 0x0002171A
		public static IMbEvent<Village> VillageBecomeNormal
		{
			get
			{
				return CampaignEvents.Instance._villageBecomeNormal;
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00023526 File Offset: 0x00021726
		public override void OnVillageBecomeNormal(Village village)
		{
			CampaignEvents.Instance._villageBecomeNormal.Invoke(village);
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00023538 File Offset: 0x00021738
		public static IMbEvent<Village> VillageBeingRaided
		{
			get
			{
				return CampaignEvents.Instance._villageBeingRaided;
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00023544 File Offset: 0x00021744
		public override void OnVillageBeingRaided(Village village)
		{
			CampaignEvents.Instance._villageBeingRaided.Invoke(village);
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00023556 File Offset: 0x00021756
		public static IMbEvent<Village> VillageLooted
		{
			get
			{
				return CampaignEvents.Instance._villageLooted;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00023562 File Offset: 0x00021762
		public override void OnVillageLooted(Village village)
		{
			CampaignEvents.Instance._villageLooted.Invoke(village);
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00023574 File Offset: 0x00021774
		public static IMbEvent<Hero, RemoveCompanionAction.RemoveCompanionDetail> CompanionRemoved
		{
			get
			{
				return CampaignEvents.Instance._companionRemoved;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00023580 File Offset: 0x00021780
		public override void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			CampaignEvents.Instance._companionRemoved.Invoke(companion, detail);
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00023593 File Offset: 0x00021793
		public static IMbEvent<IAgent> OnAgentJoinedConversationEvent
		{
			get
			{
				return CampaignEvents.Instance._onAgentJoinedConversationEvent;
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002359F File Offset: 0x0002179F
		public override void OnAgentJoinedConversation(IAgent agent)
		{
			CampaignEvents.Instance._onAgentJoinedConversationEvent.Invoke(agent);
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x000235B1 File Offset: 0x000217B1
		public static IMbEvent<IEnumerable<CharacterObject>> ConversationEnded
		{
			get
			{
				return CampaignEvents.Instance._onConversationEnded;
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000235BD File Offset: 0x000217BD
		public override void OnConversationEnded(IEnumerable<CharacterObject> characters)
		{
			CampaignEvents.Instance._onConversationEnded.Invoke(characters);
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x000235CF File Offset: 0x000217CF
		public static IMbEvent<MapEvent> MapEventEnded
		{
			get
			{
				return CampaignEvents.Instance._mapEventEnded;
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000235DB File Offset: 0x000217DB
		public override void OnMapEventEnded(MapEvent mapEvent)
		{
			CampaignEvents.Instance._mapEventEnded.Invoke(mapEvent);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x000235ED File Offset: 0x000217ED
		public static IMbEvent<MapEvent, PartyBase, PartyBase> MapEventStarted
		{
			get
			{
				return CampaignEvents.Instance._mapEventStarted;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000235F9 File Offset: 0x000217F9
		public override void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			CampaignEvents.Instance._mapEventStarted.Invoke(mapEvent, attackerParty, defenderParty);
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002360D File Offset: 0x0002180D
		public static IMbEvent<Settlement, FlattenedTroopRoster, Hero, bool> PrisonersChangeInSettlement
		{
			get
			{
				return CampaignEvents.Instance._prisonersChangeInSettlement;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00023619 File Offset: 0x00021819
		public override void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
			CampaignEvents.Instance._prisonersChangeInSettlement.Invoke(settlement, prisonerRoster, prisonerHero, takenFromDungeon);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0002362F File Offset: 0x0002182F
		public static IMbEvent<Hero, BoardGameHelper.BoardGameState> OnPlayerBoardGameOverEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerBoardGameOver;
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002363B File Offset: 0x0002183B
		public override void OnPlayerBoardGameOver(Hero opposingHero, BoardGameHelper.BoardGameState state)
		{
			CampaignEvents.Instance._onPlayerBoardGameOver.Invoke(opposingHero, state);
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0002364E File Offset: 0x0002184E
		public static IMbEvent<Hero> OnRansomOfferedToPlayerEvent
		{
			get
			{
				return CampaignEvents.Instance._onRansomOfferedToPlayer;
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002365A File Offset: 0x0002185A
		public override void OnRansomOfferedToPlayer(Hero captiveHero)
		{
			CampaignEvents.Instance._onRansomOfferedToPlayer.Invoke(captiveHero);
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0002366C File Offset: 0x0002186C
		public static IMbEvent<Hero> OnRansomOfferCancelledEvent
		{
			get
			{
				return CampaignEvents.Instance._onRansomOfferCancelled;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00023678 File Offset: 0x00021878
		public override void OnRansomOfferCancelled(Hero captiveHero)
		{
			CampaignEvents.Instance._onRansomOfferCancelled.Invoke(captiveHero);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0002368A File Offset: 0x0002188A
		public static IMbEvent<IFaction, int, int> OnPeaceOfferedToPlayerEvent
		{
			get
			{
				return CampaignEvents.Instance._onPeaceOfferedToPlayer;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00023696 File Offset: 0x00021896
		public override void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount, int tributeDurationInDays)
		{
			CampaignEvents.Instance._onPeaceOfferedToPlayer.Invoke(opponentFaction, tributeAmount, tributeDurationInDays);
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x000236AA File Offset: 0x000218AA
		public static IMbEvent<Kingdom, Kingdom> OnTradeAgreementSignedEvent
		{
			get
			{
				return CampaignEvents.Instance._onTradeAgreementSignedEvent;
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000236B6 File Offset: 0x000218B6
		public override void OnTradeAgreementSigned(Kingdom kingdom, Kingdom other)
		{
			CampaignEvents.Instance._onTradeAgreementSignedEvent.Invoke(kingdom, other);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x000236C9 File Offset: 0x000218C9
		public static IMbEvent<IFaction> OnPeaceOfferResolvedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPeaceOfferResolved;
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000236D5 File Offset: 0x000218D5
		public override void OnPeaceOfferResolved(IFaction opponentFaction)
		{
			CampaignEvents.Instance._onPeaceOfferResolved.Invoke(opponentFaction);
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x000236E7 File Offset: 0x000218E7
		public static IMbEvent<Hero, Hero> OnMarriageOfferedToPlayerEvent
		{
			get
			{
				return CampaignEvents.Instance._onMarriageOfferedToPlayerEvent;
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000236F3 File Offset: 0x000218F3
		public override void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
			CampaignEvents.Instance._onMarriageOfferedToPlayerEvent.Invoke(suitor, maiden);
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00023706 File Offset: 0x00021906
		public static IMbEvent<Hero, Hero> OnMarriageOfferCanceledEvent
		{
			get
			{
				return CampaignEvents.Instance._onMarriageOfferCanceledEvent;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00023712 File Offset: 0x00021912
		public override void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			CampaignEvents.Instance._onMarriageOfferCanceledEvent.Invoke(suitor, maiden);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00023725 File Offset: 0x00021925
		public static IMbEvent<Kingdom> OnVassalOrMercenaryServiceOfferedToPlayerEvent
		{
			get
			{
				return CampaignEvents.Instance._onVassalOrMercenaryServiceOfferedToPlayerEvent;
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00023731 File Offset: 0x00021931
		public override void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom offeredKingdom)
		{
			CampaignEvents.Instance._onVassalOrMercenaryServiceOfferedToPlayerEvent.Invoke(offeredKingdom);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00023743 File Offset: 0x00021943
		public static IMbEvent<Kingdom> OnVassalOrMercenaryServiceOfferCanceledEvent
		{
			get
			{
				return CampaignEvents.Instance._onVassalOrMercenaryServiceOfferCanceledEvent;
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002374F File Offset: 0x0002194F
		public override void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
			CampaignEvents.Instance._onVassalOrMercenaryServiceOfferCanceledEvent.Invoke(offeredKingdom);
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00023761 File Offset: 0x00021961
		public static IMbEvent<Clan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails> OnMercenaryServiceStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMercenaryServiceStartedEvent;
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002376D File Offset: 0x0002196D
		public override void OnMercenaryServiceStarted(Clan mercenaryClan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails details)
		{
			CampaignEvents.Instance._onMercenaryServiceStartedEvent.Invoke(mercenaryClan, details);
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00023780 File Offset: 0x00021980
		public static IMbEvent<Clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails> OnMercenaryServiceEndedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMercenaryServiceEndedEvent;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002378C File Offset: 0x0002198C
		public override void OnMercenaryServiceEnded(Clan mercenaryClan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
			CampaignEvents.Instance._onMercenaryServiceEndedEvent.Invoke(mercenaryClan, details);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0002379F File Offset: 0x0002199F
		public static IMbEvent<IMission> OnMissionStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMissionStartedEvent;
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000237AB File Offset: 0x000219AB
		public override void OnMissionStarted(IMission mission)
		{
			CampaignEvents.Instance._onMissionStartedEvent.Invoke(mission);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000237BD File Offset: 0x000219BD
		public static IMbEvent BeforeMissionOpenedEvent
		{
			get
			{
				return CampaignEvents.Instance._beforeMissionOpenedEvent;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000237C9 File Offset: 0x000219C9
		public override void BeforeMissionOpened()
		{
			CampaignEvents.Instance._beforeMissionOpenedEvent.Invoke();
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000237DA File Offset: 0x000219DA
		public static IMbEvent<PartyBase> OnPartyRemovedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyRemovedEvent;
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000237E6 File Offset: 0x000219E6
		public override void OnPartyRemoved(PartyBase party)
		{
			CampaignEvents.Instance._onPartyRemovedEvent.Invoke(party);
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000237F8 File Offset: 0x000219F8
		public static IMbEvent<PartyBase> OnPartySizeChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartySizeChangedEvent;
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00023804 File Offset: 0x00021A04
		public override void OnPartySizeChanged(PartyBase party)
		{
			CampaignEvents.Instance._onPartySizeChangedEvent.Invoke(party);
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00023816 File Offset: 0x00021A16
		public static IMbEvent<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail> OnSettlementOwnerChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onSettlementOwnerChangedEvent;
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00023822 File Offset: 0x00021A22
		public override void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			CampaignEvents.Instance._onSettlementOwnerChangedEvent.Invoke(settlement, openToClaim, newOwner, oldOwner, capturerHero, detail);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0002383C File Offset: 0x00021A3C
		public static IMbEvent<Town, Hero, Hero> OnGovernorChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onGovernorChangedEvent;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00023848 File Offset: 0x00021A48
		public override void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			CampaignEvents.Instance._onGovernorChangedEvent.Invoke(fortification, oldGovernor, newGovernor);
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0002385C File Offset: 0x00021A5C
		public static IMbEvent<MobileParty, Settlement> OnSettlementLeftEvent
		{
			get
			{
				return CampaignEvents.Instance._onSettlementLeftEvent;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00023868 File Offset: 0x00021A68
		public override void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			CampaignEvents.Instance._onSettlementLeftEvent.Invoke(party, settlement);
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0002387B File Offset: 0x00021A7B
		public static IMbEvent WeeklyTickEvent
		{
			get
			{
				return CampaignEvents.Instance._weeklyTickEvent;
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00023887 File Offset: 0x00021A87
		public override void WeeklyTick()
		{
			CampaignEvents.Instance._weeklyTickEvent.Invoke();
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00023898 File Offset: 0x00021A98
		public static IMbEvent DailyTickEvent
		{
			get
			{
				return CampaignEvents.Instance._dailyTickEvent;
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000238A4 File Offset: 0x00021AA4
		public override void DailyTick()
		{
			CampaignEvents.Instance._dailyTickEvent.Invoke();
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000238B5 File Offset: 0x00021AB5
		public static IMbEvent<MobileParty> DailyTickPartyEvent
		{
			get
			{
				return CampaignEvents.Instance._dailyTickPartyEvent;
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000238C1 File Offset: 0x00021AC1
		public override void DailyTickParty(MobileParty mobileParty)
		{
			CampaignEvents.Instance._dailyTickPartyEvent.Invoke(mobileParty);
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x000238D3 File Offset: 0x00021AD3
		public static IMbEvent<Town> DailyTickTownEvent
		{
			get
			{
				return CampaignEvents.Instance._dailyTickTownEvent;
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000238DF File Offset: 0x00021ADF
		public override void DailyTickTown(Town town)
		{
			CampaignEvents.Instance._dailyTickTownEvent.Invoke(town);
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x000238F1 File Offset: 0x00021AF1
		public static IMbEvent<Settlement> DailyTickSettlementEvent
		{
			get
			{
				return CampaignEvents.Instance._dailyTickSettlementEvent;
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000238FD File Offset: 0x00021AFD
		public override void DailyTickSettlement(Settlement settlement)
		{
			CampaignEvents.Instance._dailyTickSettlementEvent.Invoke(settlement);
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002390F File Offset: 0x00021B0F
		public static IMbEvent<Hero> DailyTickHeroEvent
		{
			get
			{
				return CampaignEvents.Instance._dailyTickHeroEvent;
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002391B File Offset: 0x00021B1B
		public override void DailyTickHero(Hero hero)
		{
			CampaignEvents.Instance._dailyTickHeroEvent.Invoke(hero);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002392D File Offset: 0x00021B2D
		public static IMbEvent<Clan> DailyTickClanEvent
		{
			get
			{
				return CampaignEvents.Instance._dailyTickClanEvent;
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00023939 File Offset: 0x00021B39
		public override void DailyTickClan(Clan clan)
		{
			CampaignEvents.Instance._dailyTickClanEvent.Invoke(clan);
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0002394B File Offset: 0x00021B4B
		public static IMbEvent<List<CampaignTutorial>> CollectAvailableTutorialsEvent
		{
			get
			{
				return CampaignEvents.Instance._collectAvailableTutorialsEvent;
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00023957 File Offset: 0x00021B57
		public override void CollectAvailableTutorials(ref List<CampaignTutorial> tutorials)
		{
			CampaignEvents.Instance._collectAvailableTutorialsEvent.Invoke(tutorials);
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002396A File Offset: 0x00021B6A
		public static IMbEvent<string> OnTutorialCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._onTutorialCompletedEvent;
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00023976 File Offset: 0x00021B76
		public override void OnTutorialCompleted(string tutorial)
		{
			CampaignEvents.Instance._onTutorialCompletedEvent.Invoke(tutorial);
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00023988 File Offset: 0x00021B88
		public static IMbEvent<Town, Building, int> OnBuildingLevelChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onBuildingLevelChangedEvent;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00023994 File Offset: 0x00021B94
		public override void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
			CampaignEvents.Instance._onBuildingLevelChangedEvent.Invoke(town, building, levelChange);
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x000239A8 File Offset: 0x00021BA8
		public static IMbEvent HourlyTickEvent
		{
			get
			{
				return CampaignEvents.Instance._hourlyTickEvent;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000239B4 File Offset: 0x00021BB4
		public override void HourlyTick()
		{
			CampaignEvents.Instance._hourlyTickEvent.Invoke();
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x000239C5 File Offset: 0x00021BC5
		public static IMbEvent QuarterHourlyTickEvent
		{
			get
			{
				return CampaignEvents.Instance._quarterHourlyTickEvent;
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000239D1 File Offset: 0x00021BD1
		public override void QuarterHourlyTick()
		{
			CampaignEvents.Instance._quarterHourlyTickEvent.Invoke();
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x000239E2 File Offset: 0x00021BE2
		public static IMbEvent<MobileParty> HourlyTickPartyEvent
		{
			get
			{
				return CampaignEvents.Instance._hourlyTickPartyEvent;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000239EE File Offset: 0x00021BEE
		public override void HourlyTickParty(MobileParty mobileParty)
		{
			CampaignEvents.Instance._hourlyTickPartyEvent.Invoke(mobileParty);
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00023A00 File Offset: 0x00021C00
		public static IMbEvent<Settlement> HourlyTickSettlementEvent
		{
			get
			{
				return CampaignEvents.Instance._hourlyTickSettlementEvent;
			}
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00023A0C File Offset: 0x00021C0C
		public override void HourlyTickSettlement(Settlement settlement)
		{
			CampaignEvents.Instance._hourlyTickSettlementEvent.Invoke(settlement);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00023A1E File Offset: 0x00021C1E
		public static IMbEvent<Clan> HourlyTickClanEvent
		{
			get
			{
				return CampaignEvents.Instance._hourlyTickClanEvent;
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00023A2A File Offset: 0x00021C2A
		public override void HourlyTickClan(Clan clan)
		{
			CampaignEvents.Instance._hourlyTickClanEvent.Invoke(clan);
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00023A3C File Offset: 0x00021C3C
		public static IMbEvent<float> TickEvent
		{
			get
			{
				return CampaignEvents.Instance._tickEvent;
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00023A48 File Offset: 0x00021C48
		public override void Tick(float dt)
		{
			CampaignEvents.Instance._tickEvent.Invoke(dt);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00023A5A File Offset: 0x00021C5A
		public static IMbEvent<CampaignGameStarter> OnSessionLaunchedEvent
		{
			get
			{
				return CampaignEvents.Instance._onSessionLaunchedEvent;
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00023A66 File Offset: 0x00021C66
		public override void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEvents.Instance._onSessionLaunchedEvent.Invoke(campaignGameStarter);
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00023A78 File Offset: 0x00021C78
		public static IMbEvent<CampaignGameStarter> OnAfterSessionLaunchedEvent
		{
			get
			{
				return CampaignEvents.Instance._onAfterSessionLaunchedEvent;
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00023A84 File Offset: 0x00021C84
		public override void OnAfterSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEvents.Instance._onAfterSessionLaunchedEvent.Invoke(campaignGameStarter);
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00023A96 File Offset: 0x00021C96
		public static IMbEvent<CampaignGameStarter> OnNewGameCreatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onNewGameCreatedEvent;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00023AA2 File Offset: 0x00021CA2
		public static IMbEvent<CampaignGameStarter, int> OnNewGameCreatedPartialFollowUpEvent
		{
			get
			{
				return CampaignEvents.Instance._onNewGameCreatedPartialFollowUpEvent;
			}
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00023AB0 File Offset: 0x00021CB0
		public override void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			CampaignEvents.Instance._onNewGameCreatedEvent.Invoke(campaignGameStarter);
			for (int i = 0; i < 100; i++)
			{
				CampaignEvents.Instance._onNewGameCreatedPartialFollowUpEvent.Invoke(campaignGameStarter, i);
			}
			CampaignEvents.Instance._onNewGameCreatedPartialFollowUpEndEvent.Invoke(campaignGameStarter);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00023AFB File Offset: 0x00021CFB
		public static IMbEvent<CampaignGameStarter> OnNewGameCreatedPartialFollowUpEndEvent
		{
			get
			{
				return CampaignEvents.Instance._onNewGameCreatedPartialFollowUpEndEvent;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00023B07 File Offset: 0x00021D07
		public static IMbEvent<CampaignGameStarter> OnGameEarlyLoadedEvent
		{
			get
			{
				return CampaignEvents.Instance._onGameEarlyLoadedEvent;
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00023B13 File Offset: 0x00021D13
		public override void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
			CampaignEvents.Instance._onGameEarlyLoadedEvent.Invoke(campaignGameStarter);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00023B25 File Offset: 0x00021D25
		public static IMbEvent<CampaignGameStarter> OnGameLoadedEvent
		{
			get
			{
				return CampaignEvents.Instance._onGameLoadedEvent;
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00023B31 File Offset: 0x00021D31
		public override void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			CampaignEvents.Instance._onGameLoadedEvent.Invoke(campaignGameStarter);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00023B43 File Offset: 0x00021D43
		public static IMbEvent OnGameLoadFinishedEvent
		{
			get
			{
				return CampaignEvents.Instance._onGameLoadFinishedEvent;
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00023B4F File Offset: 0x00021D4F
		public override void OnGameLoadFinished()
		{
			CampaignEvents.Instance._onGameLoadFinishedEvent.Invoke();
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00023B60 File Offset: 0x00021D60
		public static IMbEvent<MobileParty, PartyThinkParams> AiHourlyTickEvent
		{
			get
			{
				return CampaignEvents.Instance._aiHourlyTickEvent;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00023B6C File Offset: 0x00021D6C
		public override void AiHourlyTick(MobileParty party, PartyThinkParams partyThinkParams)
		{
			CampaignEvents.Instance._aiHourlyTickEvent.Invoke(party, partyThinkParams);
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00023B7F File Offset: 0x00021D7F
		public static IMbEvent<MobileParty> TickPartialHourlyAiEvent
		{
			get
			{
				return CampaignEvents.Instance._tickPartialHourlyAiEvent;
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00023B8B File Offset: 0x00021D8B
		public override void TickPartialHourlyAi(MobileParty party)
		{
			CampaignEvents.Instance._tickPartialHourlyAiEvent.Invoke(party);
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00023B9D File Offset: 0x00021D9D
		public static IMbEvent<MobileParty> OnPartyJoinedArmyEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyJoinedArmyEvent;
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00023BA9 File Offset: 0x00021DA9
		public override void OnPartyJoinedArmy(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onPartyJoinedArmyEvent.Invoke(mobileParty);
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00023BBB File Offset: 0x00021DBB
		public static IMbEvent<MobileParty> PartyRemovedFromArmyEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyRemovedFromArmyEvent;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00023BC7 File Offset: 0x00021DC7
		public override void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onPartyRemovedFromArmyEvent.Invoke(mobileParty);
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00023BD9 File Offset: 0x00021DD9
		public static IMbEvent OnPlayerArmyLeaderChangedBehaviorEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerArmyLeaderChangedBehaviorEvent;
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00023BE5 File Offset: 0x00021DE5
		public override void OnPlayerArmyLeaderChangedBehavior()
		{
			CampaignEvents.Instance._onPlayerArmyLeaderChangedBehaviorEvent.Invoke();
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00023BF6 File Offset: 0x00021DF6
		public static IMbEvent<IMission> OnMissionEndedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMissionEndedEvent;
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00023C02 File Offset: 0x00021E02
		public override void OnMissionEnded(IMission mission)
		{
			CampaignEvents.Instance._onMissionEndedEvent.Invoke(mission);
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00023C14 File Offset: 0x00021E14
		public static IMbEvent<MobileParty> OnQuarterDailyPartyTick
		{
			get
			{
				return CampaignEvents.Instance._onQuarterDailyPartyTick;
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00023C20 File Offset: 0x00021E20
		public override void QuarterDailyPartyTick(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onQuarterDailyPartyTick.Invoke(mobileParty);
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00023C32 File Offset: 0x00021E32
		public static IMbEvent<MapEvent> OnPlayerBattleEndEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerBattleEndEvent;
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00023C3E File Offset: 0x00021E3E
		public override void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			CampaignEvents.Instance._onPlayerBattleEndEvent.Invoke(mapEvent);
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00023C50 File Offset: 0x00021E50
		public static IMbEvent<CharacterObject, int> OnUnitRecruitedEvent
		{
			get
			{
				return CampaignEvents.Instance._onUnitRecruitedEvent;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00023C5C File Offset: 0x00021E5C
		public override void OnUnitRecruited(CharacterObject character, int amount)
		{
			CampaignEvents.Instance._onUnitRecruitedEvent.Invoke(character, amount);
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00023C6F File Offset: 0x00021E6F
		public static IMbEvent<Hero> OnChildConceivedEvent
		{
			get
			{
				return CampaignEvents.Instance._onChildConceived;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00023C7B File Offset: 0x00021E7B
		public override void OnChildConceived(Hero mother)
		{
			CampaignEvents.Instance._onChildConceived.Invoke(mother);
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00023C8D File Offset: 0x00021E8D
		public static IMbEvent<Hero, List<Hero>, int> OnGivenBirthEvent
		{
			get
			{
				return CampaignEvents.Instance._onGivenBirthEvent;
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00023C99 File Offset: 0x00021E99
		public override void OnGivenBirth(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
			CampaignEvents.Instance._onGivenBirthEvent.Invoke(mother, aliveChildren, stillbornCount);
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00023CAD File Offset: 0x00021EAD
		public static IMbEvent<float> MissionTickEvent
		{
			get
			{
				return CampaignEvents.Instance._missionTickEvent;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00023CB9 File Offset: 0x00021EB9
		public override void MissionTick(float dt)
		{
			CampaignEvents.Instance._missionTickEvent.Invoke(dt);
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00023CCC File Offset: 0x00021ECC
		public static IMbEvent ArmyOverlaySetDirtyEvent
		{
			get
			{
				MbEvent mbEvent;
				if ((mbEvent = CampaignEvents.Instance._armyOverlaySetDirty) == null)
				{
					mbEvent = (CampaignEvents.Instance._armyOverlaySetDirty = new MbEvent());
				}
				return mbEvent;
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00023CF9 File Offset: 0x00021EF9
		public override void OnArmyOverlaySetDirty()
		{
			if (CampaignEvents.Instance._armyOverlaySetDirty == null)
			{
				CampaignEvents.Instance._armyOverlaySetDirty = new MbEvent();
			}
			CampaignEvents.Instance._armyOverlaySetDirty.Invoke();
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00023D25 File Offset: 0x00021F25
		public static IMbEvent<int> PlayerDesertedBattleEvent
		{
			get
			{
				return CampaignEvents.Instance._playerDesertedBattle;
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00023D31 File Offset: 0x00021F31
		public override void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			CampaignEvents.Instance._playerDesertedBattle.Invoke(sacrificedMenCount);
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00023D44 File Offset: 0x00021F44
		public static IMbEvent<PartyBase> PartyVisibilityChangedEvent
		{
			get
			{
				MbEvent<PartyBase> mbEvent;
				if ((mbEvent = CampaignEvents.Instance._partyVisibilityChanged) == null)
				{
					mbEvent = (CampaignEvents.Instance._partyVisibilityChanged = new MbEvent<PartyBase>());
				}
				return mbEvent;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00023D71 File Offset: 0x00021F71
		public override void OnPartyVisibilityChanged(PartyBase party)
		{
			if (CampaignEvents.Instance._partyVisibilityChanged == null)
			{
				CampaignEvents.Instance._partyVisibilityChanged = new MbEvent<PartyBase>();
			}
			CampaignEvents.Instance._partyVisibilityChanged.Invoke(party);
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00023D9E File Offset: 0x00021F9E
		public static IMbEvent<Track> TrackDetectedEvent
		{
			get
			{
				return CampaignEvents.Instance._trackDetectedEvent;
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00023DAA File Offset: 0x00021FAA
		public override void TrackDetected(Track track)
		{
			CampaignEvents.Instance._trackDetectedEvent.Invoke(track);
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x00023DBC File Offset: 0x00021FBC
		public static IMbEvent<Track> TrackLostEvent
		{
			get
			{
				return CampaignEvents.Instance._trackLostEvent;
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00023DC8 File Offset: 0x00021FC8
		public override void TrackLost(Track track)
		{
			CampaignEvents.Instance._trackLostEvent.Invoke(track);
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x00023DDA File Offset: 0x00021FDA
		public static IMbEvent<Dictionary<string, int>> LocationCharactersAreReadyToSpawnEvent
		{
			get
			{
				return CampaignEvents.Instance._locationCharactersAreReadyToSpawn;
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00023DE8 File Offset: 0x00021FE8
		public override void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			foreach (KeyValuePair<string, int> keyValuePair in unusedUsablePointCount)
			{
			}
			CampaignEvents.Instance._locationCharactersAreReadyToSpawn.Invoke(unusedUsablePointCount);
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x00023E40 File Offset: 0x00022040
		public static ReferenceIMBEvent<MatrixFrame> BeforePlayerAgentSpawnEvent
		{
			get
			{
				return CampaignEvents.Instance._onBeforePlayerAgentSpawn;
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00023E4C File Offset: 0x0002204C
		public override void OnBeforePlayerAgentSpawn(ref MatrixFrame spawnFrame)
		{
			CampaignEvents.Instance._onBeforePlayerAgentSpawn.Invoke(ref spawnFrame);
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00023E5E File Offset: 0x0002205E
		public static IMbEvent PlayerAgentSpawned
		{
			get
			{
				return CampaignEvents.Instance._onPlayerAgentSpawned;
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00023E6A File Offset: 0x0002206A
		public override void OnPlayerAgentSpawned()
		{
			CampaignEvents.Instance._onPlayerAgentSpawned.Invoke();
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00023E7B File Offset: 0x0002207B
		public static IMbEvent LocationCharactersSimulatedEvent
		{
			get
			{
				return CampaignEvents.Instance._locationCharactersSimulatedSpawned;
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00023E87 File Offset: 0x00022087
		public override void LocationCharactersSimulated()
		{
			CampaignEvents.Instance._locationCharactersSimulatedSpawned.Invoke();
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00023E98 File Offset: 0x00022098
		public static IMbEvent<CharacterObject, CharacterObject, int> PlayerUpgradedTroopsEvent
		{
			get
			{
				return CampaignEvents.Instance._playerUpgradedTroopsEvent;
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00023EA4 File Offset: 0x000220A4
		public override void OnPlayerUpgradedTroops(CharacterObject upgradeFromTroop, CharacterObject upgradeToTroop, int number)
		{
			CampaignEvents.Instance._playerUpgradedTroopsEvent.Invoke(upgradeFromTroop, upgradeToTroop, number);
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00023EB8 File Offset: 0x000220B8
		public static IMbEvent<CharacterObject, CharacterObject, PartyBase, WeaponComponentData, bool, int> OnHeroCombatHitEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroCombatHitEvent;
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00023EC4 File Offset: 0x000220C4
		public override void OnHeroCombatHit(CharacterObject attackerTroop, CharacterObject attackedTroop, PartyBase party, WeaponComponentData usedWeapon, bool isFatal, int xp)
		{
			CampaignEvents.Instance._onHeroCombatHitEvent.Invoke(attackerTroop, attackedTroop, party, usedWeapon, isFatal, xp);
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00023EDE File Offset: 0x000220DE
		public static IMbEvent<CharacterObject> CharacterPortraitPopUpOpenedEvent
		{
			get
			{
				return CampaignEvents.Instance._characterPortraitPopUpOpenedEvent;
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00023EEA File Offset: 0x000220EA
		public override void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
			this._timeControlModeBeforePopUpOpened = Campaign.Current.TimeControlMode;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.SetTimeControlModeLock(true);
			CampaignEvents.Instance._characterPortraitPopUpOpenedEvent.Invoke(character);
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x00023F22 File Offset: 0x00022122
		public static IMbEvent CharacterPortraitPopUpClosedEvent
		{
			get
			{
				return CampaignEvents.Instance._characterPortraitPopUpClosedEvent;
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00023F2E File Offset: 0x0002212E
		public override void OnCharacterPortraitPopUpClosed()
		{
			Campaign.Current.SetTimeControlModeLock(false);
			Campaign.Current.TimeControlMode = this._timeControlModeBeforePopUpOpened;
			this._timeControlModeBeforePopUpOpened = CampaignTimeControlMode.Stop;
			CampaignEvents.Instance._characterPortraitPopUpClosedEvent.Invoke();
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00023F61 File Offset: 0x00022161
		public static IMbEvent<Hero> PlayerStartTalkFromMenu
		{
			get
			{
				return CampaignEvents.Instance._playerStartTalkFromMenu;
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00023F6D File Offset: 0x0002216D
		public override void OnPlayerStartTalkFromMenu(Hero hero)
		{
			CampaignEvents.Instance._playerStartTalkFromMenu.Invoke(hero);
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00023F7F File Offset: 0x0002217F
		public static IMbEvent<GameMenu, GameMenuOption> GameMenuOptionSelectedEvent
		{
			get
			{
				return CampaignEvents.Instance._gameMenuOptionSelectedEvent;
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00023F8B File Offset: 0x0002218B
		public override void OnGameMenuOptionSelected(GameMenu gameMenu, GameMenuOption gameMenuOption)
		{
			CampaignEvents.Instance._gameMenuOptionSelectedEvent.Invoke(gameMenu, gameMenuOption);
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00023F9E File Offset: 0x0002219E
		public static IMbEvent<CharacterObject> PlayerStartRecruitmentEvent
		{
			get
			{
				return CampaignEvents.Instance._playerStartRecruitmentEvent;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00023FAA File Offset: 0x000221AA
		public override void OnPlayerStartRecruitment(CharacterObject recruitTroopCharacter)
		{
			CampaignEvents.Instance._playerStartRecruitmentEvent.Invoke(recruitTroopCharacter);
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00023FBC File Offset: 0x000221BC
		public static IMbEvent<Hero, Hero> OnBeforePlayerCharacterChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onBeforePlayerCharacterChangedEvent;
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00023FC8 File Offset: 0x000221C8
		public override void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
			CampaignEvents.Instance._onBeforePlayerCharacterChangedEvent.Invoke(oldPlayer, newPlayer);
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00023FDB File Offset: 0x000221DB
		public static IMbEvent<Hero, Hero, MobileParty, bool> OnPlayerCharacterChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerCharacterChangedEvent;
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00023FE7 File Offset: 0x000221E7
		public override void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			CampaignEvents.Instance._onPlayerCharacterChangedEvent.Invoke(oldPlayer, newPlayer, newMainParty, isMainPartyChanged);
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00023FFD File Offset: 0x000221FD
		public static IMbEvent<Hero, Hero> OnClanLeaderChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onClanLeaderChangedEvent;
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00024009 File Offset: 0x00022209
		public override void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			CampaignEvents.Instance._onClanLeaderChangedEvent.Invoke(oldLeader, newLeader);
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0002401C File Offset: 0x0002221C
		public static IMbEvent<SiegeEvent> OnSiegeEventStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onSiegeEventStartedEvent;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00024028 File Offset: 0x00022228
		public override void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			CampaignEvents.Instance._onSiegeEventStartedEvent.Invoke(siegeEvent);
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0002403A File Offset: 0x0002223A
		public static IMbEvent OnPlayerSiegeStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerSiegeStartedEvent;
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00024046 File Offset: 0x00022246
		public override void OnPlayerSiegeStarted()
		{
			CampaignEvents.Instance._onPlayerSiegeStartedEvent.Invoke();
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00024057 File Offset: 0x00022257
		public static IMbEvent<SiegeEvent> OnSiegeEventEndedEvent
		{
			get
			{
				return CampaignEvents.Instance._onSiegeEventEndedEvent;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00024063 File Offset: 0x00022263
		public override void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
			CampaignEvents.Instance._onSiegeEventEndedEvent.Invoke(siegeEvent);
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00024075 File Offset: 0x00022275
		public static IMbEvent<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>> OnSiegeAftermathAppliedEvent
		{
			get
			{
				return CampaignEvents.Instance._siegeAftermathAppliedEvent;
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00024081 File Offset: 0x00022281
		public override void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			CampaignEvents.Instance._siegeAftermathAppliedEvent.Invoke(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00024099 File Offset: 0x00022299
		public static IMbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, SiegeBombardTargets> OnSiegeBombardmentHitEvent
		{
			get
			{
				return CampaignEvents.Instance._onSiegeBombardmentHitEvent;
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000240A5 File Offset: 0x000222A5
		public override void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
			CampaignEvents.Instance._onSiegeBombardmentHitEvent.Invoke(besiegerParty, besiegedSettlement, side, weapon, target);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x000240BD File Offset: 0x000222BD
		public static IMbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool> OnSiegeBombardmentWallHitEvent
		{
			get
			{
				return CampaignEvents.Instance._onSiegeBombardmentWallHitEvent;
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000240C9 File Offset: 0x000222C9
		public override void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
			CampaignEvents.Instance._onSiegeBombardmentWallHitEvent.Invoke(besiegerParty, besiegedSettlement, side, weapon, isWallCracked);
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x000240E1 File Offset: 0x000222E1
		public static IMbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType> OnSiegeEngineDestroyedEvent
		{
			get
			{
				return CampaignEvents.Instance._onSiegeEngineDestroyedEvent;
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000240ED File Offset: 0x000222ED
		public override void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
			CampaignEvents.Instance._onSiegeEngineDestroyedEvent.Invoke(besiegerParty, besiegedSettlement, side, destroyedEngine);
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00024103 File Offset: 0x00022303
		public static IMbEvent<List<TradeRumor>, Settlement> OnTradeRumorIsTakenEvent
		{
			get
			{
				return CampaignEvents.Instance._onTradeRumorIsTakenEvent;
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002410F File Offset: 0x0002230F
		public override void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
			CampaignEvents.Instance._onTradeRumorIsTakenEvent.Invoke(newRumors, sourceSettlement);
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00024122 File Offset: 0x00022322
		public static IMbEvent<Hero> OnCheckForIssueEvent
		{
			get
			{
				return CampaignEvents.Instance._onCheckForIssueEvent;
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002412E File Offset: 0x0002232E
		public override void OnCheckForIssue(Hero hero)
		{
			CampaignEvents.Instance._onCheckForIssueEvent.Invoke(hero);
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00024140 File Offset: 0x00022340
		public static IMbEvent<IssueBase, IssueBase.IssueUpdateDetails, Hero> OnIssueUpdatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onIssueUpdatedEvent;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002414C File Offset: 0x0002234C
		public override void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver = null)
		{
			CampaignEvents.Instance._onIssueUpdatedEvent.Invoke(issue, details, issueSolver);
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00024160 File Offset: 0x00022360
		public static IMbEvent<MobileParty, TroopRoster> OnTroopsDesertedEvent
		{
			get
			{
				return CampaignEvents.Instance._onTroopsDesertedEvent;
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002416C File Offset: 0x0002236C
		public override void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
			CampaignEvents.Instance._onTroopsDesertedEvent.Invoke(mobileParty, desertedTroops);
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0002417F File Offset: 0x0002237F
		public static IMbEvent<Hero, Settlement, Hero, CharacterObject, int> OnTroopRecruitedEvent
		{
			get
			{
				return CampaignEvents.Instance._onTroopRecruitedEvent;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002418B File Offset: 0x0002238B
		public override void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
			CampaignEvents.Instance._onTroopRecruitedEvent.Invoke(recruiterHero, recruitmentSettlement, recruitmentSource, troop, amount);
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x000241A3 File Offset: 0x000223A3
		public static IMbEvent<Hero, Settlement, TroopRoster> OnTroopGivenToSettlementEvent
		{
			get
			{
				return CampaignEvents.Instance._onTroopGivenToSettlementEvent;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000241AF File Offset: 0x000223AF
		public override void OnTroopGivenToSettlement(Hero giverHero, Settlement recipientSettlement, TroopRoster roster)
		{
			CampaignEvents.Instance._onTroopGivenToSettlementEvent.Invoke(giverHero, recipientSettlement, roster);
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x000241C3 File Offset: 0x000223C3
		public static IMbEvent<PartyBase, PartyBase, ItemRosterElement, int, Settlement> OnItemSoldEvent
		{
			get
			{
				return CampaignEvents.Instance._onItemSoldEvent;
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000241CF File Offset: 0x000223CF
		public override void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement itemRosterElement, int number, Settlement currentSettlement)
		{
			CampaignEvents.Instance._onItemSoldEvent.Invoke(receiverParty, payerParty, itemRosterElement, number, currentSettlement);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x000241E7 File Offset: 0x000223E7
		public static IMbEvent<MobileParty, Town, List<ValueTuple<EquipmentElement, int>>> OnCaravanTransactionCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._onCaravanTransactionCompletedEvent;
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000241F3 File Offset: 0x000223F3
		public override void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> itemRosterElements)
		{
			CampaignEvents.Instance._onCaravanTransactionCompletedEvent.Invoke(caravanParty, town, itemRosterElements);
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00024207 File Offset: 0x00022407
		public static IMbEvent<PartyBase, PartyBase, TroopRoster> OnPrisonerSoldEvent
		{
			get
			{
				return CampaignEvents.Instance._onPrisonerSoldEvent;
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00024213 File Offset: 0x00022413
		public override void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
			CampaignEvents.Instance._onPrisonerSoldEvent.Invoke(sellerParty, buyerParty, prisoners);
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00024227 File Offset: 0x00022427
		public static IMbEvent<MobileParty> OnPartyDisbandStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyDisbandStartedEvent;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00024233 File Offset: 0x00022433
		public override void OnPartyDisbandStarted(MobileParty disbandParty)
		{
			CampaignEvents.Instance._onPartyDisbandStartedEvent.Invoke(disbandParty);
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00024245 File Offset: 0x00022445
		public static IMbEvent<MobileParty, Settlement> OnPartyDisbandedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyDisbandedEvent;
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00024251 File Offset: 0x00022451
		public override void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			CampaignEvents.Instance._onPartyDisbandedEvent.Invoke(disbandParty, relatedSettlement);
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00024264 File Offset: 0x00022464
		public static IMbEvent<MobileParty> OnPartyDisbandCanceledEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyDisbandCanceledEvent;
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00024270 File Offset: 0x00022470
		public override void OnPartyDisbandCanceled(MobileParty disbandParty)
		{
			CampaignEvents.Instance._onPartyDisbandCanceledEvent.Invoke(disbandParty);
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x00024282 File Offset: 0x00022482
		public static IMbEvent<PartyBase, PartyBase> OnHideoutSpottedEvent
		{
			get
			{
				return CampaignEvents.Instance._hideoutSpottedEvent;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002428E File Offset: 0x0002248E
		public override void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
			CampaignEvents.Instance._hideoutSpottedEvent.Invoke(party, hideoutParty);
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x000242A1 File Offset: 0x000224A1
		public static IMbEvent<Settlement> OnHideoutDeactivatedEvent
		{
			get
			{
				return CampaignEvents.Instance._hideoutDeactivatedEvent;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000242AD File Offset: 0x000224AD
		public override void OnHideoutDeactivated(Settlement hideout)
		{
			CampaignEvents.Instance._hideoutDeactivatedEvent.Invoke(hideout);
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x000242BF File Offset: 0x000224BF
		public static IMbEvent<Hero, Hero, float> OnHeroSharedFoodWithAnotherHeroEvent
		{
			get
			{
				return CampaignEvents.Instance._heroSharedFoodWithAnotherHeroEvent;
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000242CB File Offset: 0x000224CB
		public override void OnHeroSharedFoodWithAnother(Hero supporterHero, Hero supportedHero, float influence)
		{
			CampaignEvents.Instance._heroSharedFoodWithAnotherHeroEvent.Invoke(supporterHero, supportedHero, influence);
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x000242DF File Offset: 0x000224DF
		public static IMbEvent<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool> PlayerInventoryExchangeEvent
		{
			get
			{
				return CampaignEvents.Instance._playerInventoryExchangeEvent;
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x000242EB File Offset: 0x000224EB
		public override void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			CampaignEvents.Instance._playerInventoryExchangeEvent.Invoke(purchasedItems, soldItems, isTrading);
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x000242FF File Offset: 0x000224FF
		public static IMbEvent<ItemRoster> OnItemsDiscardedByPlayerEvent
		{
			get
			{
				return CampaignEvents.Instance._onItemsDiscardedByPlayerEvent;
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002430B File Offset: 0x0002250B
		public override void OnItemsDiscardedByPlayer(ItemRoster discardedItems)
		{
			CampaignEvents.Instance._onItemsDiscardedByPlayerEvent.Invoke(discardedItems);
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0002431D File Offset: 0x0002251D
		public static IMbEvent<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> PersuasionProgressCommittedEvent
		{
			get
			{
				return CampaignEvents.Instance._persuasionProgressCommittedEvent;
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00024329 File Offset: 0x00022529
		public override void OnPersuasionProgressCommitted(Tuple<PersuasionOptionArgs, PersuasionOptionResult> progress)
		{
			CampaignEvents.Instance._persuasionProgressCommittedEvent.Invoke(progress);
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0002433B File Offset: 0x0002253B
		public static IMbEvent<QuestBase, QuestBase.QuestCompleteDetails> OnQuestCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._onQuestCompletedEvent;
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00024347 File Offset: 0x00022547
		public override void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			CampaignEvents.Instance._onQuestCompletedEvent.Invoke(quest, detail);
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0002435A File Offset: 0x0002255A
		public static IMbEvent<QuestBase> OnQuestStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onQuestStartedEvent;
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00024366 File Offset: 0x00022566
		public override void OnQuestStarted(QuestBase quest)
		{
			CampaignEvents.Instance._onQuestStartedEvent.Invoke(quest);
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00024378 File Offset: 0x00022578
		public static IMbEvent<ItemObject, Settlement, int> OnItemProducedEvent
		{
			get
			{
				return CampaignEvents.Instance._itemProducedEvent;
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00024384 File Offset: 0x00022584
		public override void OnItemProduced(ItemObject itemObject, Settlement settlement, int count)
		{
			CampaignEvents.Instance._itemProducedEvent.Invoke(itemObject, settlement, count);
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00024398 File Offset: 0x00022598
		public static IMbEvent<ItemObject, Settlement, int> OnItemConsumedEvent
		{
			get
			{
				return CampaignEvents.Instance._itemConsumedEvent;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000243A4 File Offset: 0x000225A4
		public override void OnItemConsumed(ItemObject itemObject, Settlement settlement, int count)
		{
			CampaignEvents.Instance._itemConsumedEvent.Invoke(itemObject, settlement, count);
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x000243B8 File Offset: 0x000225B8
		public static IMbEvent<MobileParty> OnPartyConsumedFoodEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyConsumedFoodEvent;
			}
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000243C4 File Offset: 0x000225C4
		public override void OnPartyConsumedFood(MobileParty party)
		{
			CampaignEvents.Instance._onPartyConsumedFoodEvent.Invoke(party);
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x000243D6 File Offset: 0x000225D6
		public static IMbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool> OnBeforeMainCharacterDiedEvent
		{
			get
			{
				return CampaignEvents.Instance._onBeforeMainCharacterDiedEvent;
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000243E2 File Offset: 0x000225E2
		public override void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEvents.Instance._onBeforeMainCharacterDiedEvent.Invoke(victim, killer, detail, showNotification);
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000243F8 File Offset: 0x000225F8
		public static IMbEvent<IssueBase> OnNewIssueCreatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onNewIssueCreatedEvent;
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00024404 File Offset: 0x00022604
		public override void OnNewIssueCreated(IssueBase issue)
		{
			CampaignEvents.Instance._onNewIssueCreatedEvent.Invoke(issue);
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00024416 File Offset: 0x00022616
		public static IMbEvent<IssueBase, Hero> OnIssueOwnerChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onIssueOwnerChangedEvent;
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00024422 File Offset: 0x00022622
		public override void OnIssueOwnerChanged(IssueBase issue, Hero oldOwner)
		{
			CampaignEvents.Instance._onIssueOwnerChangedEvent.Invoke(issue, oldOwner);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00024435 File Offset: 0x00022635
		public override void OnGameOver()
		{
			CampaignEvents.Instance._onGameOverEvent.Invoke();
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00024446 File Offset: 0x00022646
		public static IMbEvent OnGameOverEvent
		{
			get
			{
				return CampaignEvents.Instance._onGameOverEvent;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00024452 File Offset: 0x00022652
		public static IMbEvent<Settlement, MobileParty, bool, MapEvent.BattleTypes> SiegeCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._siegeCompletedEvent;
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002445E File Offset: 0x0002265E
		public override void SiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
			CampaignEvents.Instance._siegeCompletedEvent.Invoke(siegeSettlement, attackerParty, isWin, battleType);
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00024474 File Offset: 0x00022674
		public static IMbEvent<Settlement, MobileParty, bool, MapEvent.BattleTypes> AfterSiegeCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._afterSiegeCompletedEvent;
			}
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00024480 File Offset: 0x00022680
		public override void AfterSiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
			CampaignEvents.Instance._afterSiegeCompletedEvent.Invoke(siegeSettlement, attackerParty, isWin, battleType);
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00024496 File Offset: 0x00022696
		public static IMbEvent<SiegeEvent, BattleSideEnum, SiegeEngineType> SiegeEngineBuiltEvent
		{
			get
			{
				return CampaignEvents.Instance._siegeEngineBuiltEvent;
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000244A2 File Offset: 0x000226A2
		public override void SiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngineType)
		{
			CampaignEvents.Instance._siegeEngineBuiltEvent.Invoke(siegeEvent, side, siegeEngineType);
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000244B6 File Offset: 0x000226B6
		public static IMbEvent<BattleSideEnum, RaidEventComponent> RaidCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._raidCompletedEvent;
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000244C2 File Offset: 0x000226C2
		public override void RaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			CampaignEvents.Instance._raidCompletedEvent.Invoke(winnerSide, raidEvent);
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000244D5 File Offset: 0x000226D5
		public static IMbEvent<BattleSideEnum, ForceVolunteersEventComponent> ForceVolunteersCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._forceVolunteersCompletedEvent;
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000244E1 File Offset: 0x000226E1
		public override void ForceVolunteersCompleted(BattleSideEnum winnerSide, ForceVolunteersEventComponent forceVolunteersEvent)
		{
			CampaignEvents.Instance._forceVolunteersCompletedEvent.Invoke(winnerSide, forceVolunteersEvent);
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000244F4 File Offset: 0x000226F4
		public static IMbEvent<BattleSideEnum, ForceSuppliesEventComponent> ForceSuppliesCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._forceSuppliesCompletedEvent;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00024500 File Offset: 0x00022700
		public override void ForceSuppliesCompleted(BattleSideEnum winnerSide, ForceSuppliesEventComponent forceSuppliesEvent)
		{
			CampaignEvents.Instance._forceSuppliesCompletedEvent.Invoke(winnerSide, forceSuppliesEvent);
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00024513 File Offset: 0x00022713
		public static MbEvent<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState> OnHideoutBattleCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._hideoutBattleCompletedEvent;
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0002451F File Offset: 0x0002271F
		public override void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
			CampaignEvents.Instance._hideoutBattleCompletedEvent.Invoke(winnerSide, hideoutEventComponent, battleEndState);
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00024533 File Offset: 0x00022733
		public static IMbEvent<Clan> OnClanDestroyedEvent
		{
			get
			{
				return CampaignEvents.Instance._onClanDestroyedEvent;
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002453F File Offset: 0x0002273F
		public override void OnClanDestroyed(Clan destroyedClan)
		{
			CampaignEvents.Instance._onClanDestroyedEvent.Invoke(destroyedClan);
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00024551 File Offset: 0x00022751
		public static IMbEvent<ItemObject, ItemModifier, bool> OnNewItemCraftedEvent
		{
			get
			{
				return CampaignEvents.Instance._onNewItemCraftedEvent;
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002455D File Offset: 0x0002275D
		public override void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
			CampaignEvents.Instance._onNewItemCraftedEvent.Invoke(itemObject, overriddenItemModifier, isCraftingOrderItem);
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00024571 File Offset: 0x00022771
		public static IMbEvent<CraftingPiece> CraftingPartUnlockedEvent
		{
			get
			{
				return CampaignEvents.Instance._craftingPartUnlockedEvent;
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002457D File Offset: 0x0002277D
		public override void CraftingPartUnlocked(CraftingPiece craftingPiece)
		{
			CampaignEvents.Instance._craftingPartUnlockedEvent.Invoke(craftingPiece);
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0002458F File Offset: 0x0002278F
		public static IMbEvent<Workshop> WorkshopInitializedEvent
		{
			get
			{
				return CampaignEvents.Instance._onWorkshopInitializedEvent;
			}
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002459B File Offset: 0x0002279B
		public override void OnWorkshopInitialized(Workshop workshop)
		{
			CampaignEvents.Instance._onWorkshopInitializedEvent.Invoke(workshop);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000245AD File Offset: 0x000227AD
		public override void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
			CampaignEvents.Instance._onWorkshopOwnerChangedEvent.Invoke(workshop, oldOwner);
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000245C0 File Offset: 0x000227C0
		public static IMbEvent<Workshop, Hero> WorkshopOwnerChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onWorkshopOwnerChangedEvent;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000245CC File Offset: 0x000227CC
		public static IMbEvent<Workshop> WorkshopTypeChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onWorkshopTypeChangedEvent;
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000245D8 File Offset: 0x000227D8
		public override void OnWorkshopTypeChanged(Workshop workshop)
		{
			CampaignEvents.Instance._onWorkshopTypeChangedEvent.Invoke(workshop);
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000245EA File Offset: 0x000227EA
		public static IMbEvent OnBeforeSaveEvent
		{
			get
			{
				return CampaignEvents.Instance._onBeforeSaveEvent;
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000245F6 File Offset: 0x000227F6
		public override void OnBeforeSave()
		{
			CampaignEvents.Instance._onBeforeSaveEvent.Invoke();
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00024607 File Offset: 0x00022807
		public static IMbEvent OnSaveStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onSaveStartedEvent;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00024613 File Offset: 0x00022813
		public override void OnSaveStarted()
		{
			CampaignEvents.Instance._onSaveStartedEvent.Invoke();
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00024624 File Offset: 0x00022824
		public static IMbEvent<bool, string> OnSaveOverEvent
		{
			get
			{
				return CampaignEvents.Instance._onSaveOverEvent;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00024630 File Offset: 0x00022830
		public override void OnSaveOver(bool isSuccessful, string saveName)
		{
			CampaignEvents.Instance._onSaveOverEvent.Invoke(isSuccessful, saveName);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00024643 File Offset: 0x00022843
		public static IMbEvent<List<KeyValuePair<string, string>>> CollectMetadataEntriesEvent
		{
			get
			{
				return CampaignEvents.Instance._collectMetadataEntriesEvent;
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002464F File Offset: 0x0002284F
		public override void CollectMetadataEntries(List<KeyValuePair<string, string>> pairs)
		{
			CampaignEvents.Instance._collectMetadataEntriesEvent.Invoke(pairs);
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00024661 File Offset: 0x00022861
		public static IMbEvent<FlattenedTroopRoster> OnPrisonerTakenEvent
		{
			get
			{
				return CampaignEvents.Instance._onPrisonerTakenEvent;
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002466D File Offset: 0x0002286D
		public override void OnPrisonerTaken(FlattenedTroopRoster roster)
		{
			CampaignEvents.Instance._onPrisonerTakenEvent.Invoke(roster);
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002467F File Offset: 0x0002287F
		public static IMbEvent<FlattenedTroopRoster> OnPrisonerReleasedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPrisonerReleasedEvent;
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002468B File Offset: 0x0002288B
		public override void OnPrisonerReleased(FlattenedTroopRoster roster)
		{
			CampaignEvents.Instance._onPrisonerReleasedEvent.Invoke(roster);
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002469D File Offset: 0x0002289D
		public static IMbEvent<FlattenedTroopRoster> OnMainPartyPrisonerRecruitedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMainPartyPrisonerRecruitedEvent;
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000246A9 File Offset: 0x000228A9
		public override void OnMainPartyPrisonerRecruited(FlattenedTroopRoster roster)
		{
			CampaignEvents.Instance._onMainPartyPrisonerRecruitedEvent.Invoke(roster);
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x000246BB File Offset: 0x000228BB
		public static IMbEvent<MobileParty, FlattenedTroopRoster, Settlement> OnPrisonerDonatedToSettlementEvent
		{
			get
			{
				return CampaignEvents.Instance._onPrisonerDonatedToSettlementEvent;
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000246C7 File Offset: 0x000228C7
		public override void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
			CampaignEvents.Instance._onPrisonerDonatedToSettlementEvent.Invoke(donatingParty, donatedPrisoners, donatedSettlement);
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x000246DB File Offset: 0x000228DB
		public static IMbEvent<Hero, EquipmentElement> OnEquipmentSmeltedByHeroEvent
		{
			get
			{
				return CampaignEvents.Instance._onEquipmentSmeltedByHero;
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000246E7 File Offset: 0x000228E7
		public override void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement smeltedEquipmentElement)
		{
			CampaignEvents.Instance._onEquipmentSmeltedByHero.Invoke(hero, smeltedEquipmentElement);
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000246FA File Offset: 0x000228FA
		public static IMbEvent<int> OnPlayerTradeProfitEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerTradeProfit;
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00024706 File Offset: 0x00022906
		public override void OnPlayerTradeProfit(int profit)
		{
			CampaignEvents.Instance._onPlayerTradeProfit.Invoke(profit);
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00024718 File Offset: 0x00022918
		public static IMbEvent<Hero, Clan> OnHeroChangedClanEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroChangedClan;
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00024724 File Offset: 0x00022924
		public override void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			CampaignEvents.Instance._onHeroChangedClan.Invoke(hero, oldClan);
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00024737 File Offset: 0x00022937
		public static IMbEvent<Hero, HeroGetsBusyReasons> OnHeroGetsBusyEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroGetsBusy;
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00024743 File Offset: 0x00022943
		public override void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
			CampaignEvents.Instance._onHeroGetsBusy.Invoke(hero, heroGetsBusyReason);
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00024756 File Offset: 0x00022956
		public static IMbEvent<PartyBase, ItemRoster> OnCollectLootsItemsEvent
		{
			get
			{
				return CampaignEvents.Instance._onCollectLootItems;
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00024762 File Offset: 0x00022962
		public override void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
			CampaignEvents.Instance._onCollectLootItems.Invoke(winnerParty, gainedLoots);
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00024775 File Offset: 0x00022975
		public static IMbEvent<PartyBase, PartyBase, ItemRoster> OnLootDistributedToPartyEvent
		{
			get
			{
				return CampaignEvents.Instance._onLootDistributedToPartyEvent;
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00024781 File Offset: 0x00022981
		public override void OnLootDistributedToParty(PartyBase winnerParty, PartyBase defeatedParty, ItemRoster lootedItems)
		{
			CampaignEvents.Instance._onLootDistributedToPartyEvent.Invoke(winnerParty, defeatedParty, lootedItems);
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00024795 File Offset: 0x00022995
		public static IMbEvent<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail> OnHeroTeleportationRequestedEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroTeleportationRequestedEvent;
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000247A1 File Offset: 0x000229A1
		public override void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			CampaignEvents.Instance._onHeroTeleportationRequestedEvent.Invoke(hero, targetSettlement, targetParty, detail);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x000247B7 File Offset: 0x000229B7
		public static IMbEvent<MobileParty> OnPartyLeaderChangeOfferCanceledEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyLeaderChangeOfferCanceledEvent;
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000247C3 File Offset: 0x000229C3
		public override void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
			CampaignEvents.Instance._onPartyLeaderChangeOfferCanceledEvent.Invoke(party);
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x000247D5 File Offset: 0x000229D5
		public static IMbEvent<MobileParty, Hero> OnPartyLeaderChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyLeaderChangedEvent;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000247E1 File Offset: 0x000229E1
		public override void OnPartyLeaderChanged(MobileParty mobileParty, Hero oldLeader)
		{
			CampaignEvents.Instance._onPartyLeaderChangedEvent.Invoke(mobileParty, oldLeader);
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x000247F4 File Offset: 0x000229F4
		public static IMbEvent<Clan, float> OnClanInfluenceChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onClanInfluenceChangedEvent;
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00024800 File Offset: 0x00022A00
		public override void OnClanInfluenceChanged(Clan clan, float change)
		{
			CampaignEvents.Instance._onClanInfluenceChangedEvent.Invoke(clan, change);
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00024813 File Offset: 0x00022A13
		public static IMbEvent<CharacterObject> OnPlayerPartyKnockedOrKilledTroopEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerPartyKnockedOrKilledTroopEvent;
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002481F File Offset: 0x00022A1F
		public override void OnPlayerPartyKnockedOrKilledTroop(CharacterObject strikedTroop)
		{
			CampaignEvents.Instance._onPlayerPartyKnockedOrKilledTroopEvent.Invoke(strikedTroop);
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00024831 File Offset: 0x00022A31
		public static IMbEvent<DefaultClanFinanceModel.AssetIncomeType, int> OnPlayerEarnedGoldFromAssetEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerEarnedGoldFromAssetEvent;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002483D File Offset: 0x00022A3D
		public override void OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType incomeType, int incomeAmount)
		{
			CampaignEvents.Instance._onPlayerEarnedGoldFromAssetEvent.Invoke(incomeType, incomeAmount);
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00024850 File Offset: 0x00022A50
		public static IMbEvent<Clan, IFaction> OnClanEarnedGoldFromTributeEvent
		{
			get
			{
				return CampaignEvents.Instance._onClanEarnedGoldFromTributeEvent;
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002485C File Offset: 0x00022A5C
		public override void OnClanEarnedGoldFromTribute(Clan receiverClan, IFaction payingFaction)
		{
			CampaignEvents.Instance._onClanEarnedGoldFromTributeEvent.Invoke(receiverClan, payingFaction);
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002486F File Offset: 0x00022A6F
		public static IMbEvent OnMainPartyStarvingEvent
		{
			get
			{
				return CampaignEvents.Instance._onMainPartyStarving;
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0002487B File Offset: 0x00022A7B
		public override void OnMainPartyStarving()
		{
			CampaignEvents.Instance._onMainPartyStarving.Invoke();
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002488C File Offset: 0x00022A8C
		public static IMbEvent<Town, bool> OnPlayerJoinedTournamentEvent
		{
			get
			{
				return CampaignEvents.Instance._onPlayerJoinedTournamentEvent;
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00024898 File Offset: 0x00022A98
		public override void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
			CampaignEvents.Instance._onPlayerJoinedTournamentEvent.Invoke(town, isParticipant);
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000248AB File Offset: 0x00022AAB
		public static IMbEvent<Hero> OnHeroUnregisteredEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeroUnregisteredEvent;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000248B7 File Offset: 0x00022AB7
		public override void OnHeroUnregistered(Hero hero)
		{
			CampaignEvents.Instance._onHeroUnregisteredEvent.Invoke(hero);
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000248C9 File Offset: 0x00022AC9
		public static IMbEvent OnConfigChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onConfigChanged;
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000248D5 File Offset: 0x00022AD5
		public override void OnConfigChanged()
		{
			CampaignEvents.Instance._onConfigChanged.Invoke();
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000248E6 File Offset: 0x00022AE6
		public static IMbEvent<Town, CraftingOrder, ItemObject, Hero> OnCraftingOrderCompletedEvent
		{
			get
			{
				return CampaignEvents.Instance._onCraftingOrderCompleted;
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000248F2 File Offset: 0x00022AF2
		public override void OnCraftingOrderCompleted(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
			CampaignEvents.Instance._onCraftingOrderCompleted.Invoke(town, craftingOrder, craftedItem, completerHero);
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00024908 File Offset: 0x00022B08
		public static IMbEvent<Hero, Crafting.RefiningFormula> OnItemsRefinedEvent
		{
			get
			{
				return CampaignEvents.Instance._onItemsRefined;
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00024914 File Offset: 0x00022B14
		public override void OnItemsRefined(Hero hero, Crafting.RefiningFormula refineFormula)
		{
			CampaignEvents.Instance._onItemsRefined.Invoke(hero, refineFormula);
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00024927 File Offset: 0x00022B27
		public static IMbEvent<Dictionary<Hero, int>> OnHeirSelectionRequestedEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeirSelectionRequested;
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00024933 File Offset: 0x00022B33
		public override void OnHeirSelectionRequested(Dictionary<Hero, int> heirApparents)
		{
			CampaignEvents.Instance._onHeirSelectionRequested.Invoke(heirApparents);
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00024945 File Offset: 0x00022B45
		public static IMbEvent<Hero> OnHeirSelectionOverEvent
		{
			get
			{
				return CampaignEvents.Instance._onHeirSelectionOver;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00024951 File Offset: 0x00022B51
		public override void OnHeirSelectionOver(Hero selectedHero)
		{
			CampaignEvents.Instance._onHeirSelectionOver.Invoke(selectedHero);
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00024963 File Offset: 0x00022B63
		public static IMbEvent<CharacterCreationManager> OnCharacterCreationInitializedEvent
		{
			get
			{
				return CampaignEvents.Instance._onCharacterCreationInitialized;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002496F File Offset: 0x00022B6F
		public override void OnMobilePartyRaftStateChanged(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onMobilePartyRaftStateChanged.Invoke(mobileParty);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00024981 File Offset: 0x00022B81
		public override void OnCharacterCreationInitialized(CharacterCreationManager characterCreationManager)
		{
			CampaignEvents.Instance._onCharacterCreationInitialized.Invoke(characterCreationManager);
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00024993 File Offset: 0x00022B93
		public static IMbEvent<MobileParty> OnMobilePartyRaftStateChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMobilePartyRaftStateChanged;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0002499F File Offset: 0x00022B9F
		public static IMbEvent<PartyBase, Ship, DestroyShipAction.ShipDestroyDetail> OnShipDestroyedEvent
		{
			get
			{
				return CampaignEvents.Instance._onShipDestroyedEvent;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x000249AB File Offset: 0x00022BAB
		public override void OnShipDestroyed(PartyBase owner, Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
			CampaignEvents.Instance._onShipDestroyedEvent.Invoke(owner, ship, detail);
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x000249BF File Offset: 0x00022BBF
		public static IMbEvent<Ship, PartyBase, ChangeShipOwnerAction.ShipOwnerChangeDetail> OnShipOwnerChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onShipOwnerChangedEvent;
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000249CB File Offset: 0x00022BCB
		public override void OnShipOwnerChanged(Ship ship, PartyBase oldOwner, ChangeShipOwnerAction.ShipOwnerChangeDetail changeDetail)
		{
			CampaignEvents.Instance._onShipOwnerChangedEvent.Invoke(ship, oldOwner, changeDetail);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000249DF File Offset: 0x00022BDF
		public static IMbEvent<Ship, Settlement> OnShipRepairedEvent
		{
			get
			{
				return CampaignEvents.Instance._onShipRepairedEvent;
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000249EB File Offset: 0x00022BEB
		public override void OnShipRepaired(Ship ship, Settlement repairPort)
		{
			CampaignEvents.Instance._onShipRepairedEvent.Invoke(ship, repairPort);
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x000249FE File Offset: 0x00022BFE
		public static IMbEvent<Ship, Settlement> OnShipCreatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onShipCreatedEvent;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00024A0A File Offset: 0x00022C0A
		public override void OnShipCreated(Ship ship, Settlement createdSettlement)
		{
			CampaignEvents.Instance._onShipCreatedEvent.Invoke(ship, createdSettlement);
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x00024A1D File Offset: 0x00022C1D
		public static IMbEvent<Figurehead> OnFigureheadUnlockedEvent
		{
			get
			{
				return CampaignEvents.Instance._onFigureheadUnlockedEvent;
			}
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00024A29 File Offset: 0x00022C29
		public override void OnFigureheadUnlocked(Figurehead figurehead)
		{
			CampaignEvents.Instance._onFigureheadUnlockedEvent.Invoke(figurehead);
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00024A3B File Offset: 0x00022C3B
		public static IMbEvent<MobileParty, Army> OnPartyLeftArmyEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyLeftArmyEvent;
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00024A47 File Offset: 0x00022C47
		public override void OnPartyLeftArmy(MobileParty party, Army army)
		{
			CampaignEvents.Instance._onPartyLeftArmyEvent.Invoke(party, army);
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00024A5A File Offset: 0x00022C5A
		public static IMbEvent<PartyBase> OnPartyAddedToMapEventEvent
		{
			get
			{
				return CampaignEvents.Instance._onPartyAddedToMapEventEvent;
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00024A66 File Offset: 0x00022C66
		public override void OnPartyAddedToMapEvent(PartyBase partyBase)
		{
			CampaignEvents.Instance._onPartyAddedToMapEventEvent.Invoke(partyBase);
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00024A78 File Offset: 0x00022C78
		public static IMbEvent<Incident> OnIncidentResolvedEvent
		{
			get
			{
				return CampaignEvents.Instance._onIncidentResolvedEvent;
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00024A84 File Offset: 0x00022C84
		public override void OnIncidentResolved(Incident incident)
		{
			CampaignEvents.Instance._onIncidentResolvedEvent.Invoke(incident);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00024A96 File Offset: 0x00022C96
		public static IMbEvent<MobileParty> OnMobilePartyNavigationStateChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMobilePartyNavigationStateChangedEvent;
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00024AA2 File Offset: 0x00022CA2
		public override void OnMobilePartyNavigationStateChanged(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onMobilePartyNavigationStateChangedEvent.Invoke(mobileParty);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x00024AB4 File Offset: 0x00022CB4
		public static IMbEvent<MobileParty> OnMobilePartyJoinedToSiegeEventEvent
		{
			get
			{
				return CampaignEvents.Instance._onMobilePartyJoinedToSiegeEventEvent;
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00024AC0 File Offset: 0x00022CC0
		public override void OnMobilePartyJoinedToSiegeEvent(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onMobilePartyJoinedToSiegeEventEvent.Invoke(mobileParty);
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00024AD2 File Offset: 0x00022CD2
		public static IMbEvent<MobileParty> OnMobilePartyLeftSiegeEventEvent
		{
			get
			{
				return CampaignEvents.Instance._onMobilePartyLeftSiegeEventEvent;
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00024ADE File Offset: 0x00022CDE
		public override void OnMobilePartyLeftSiegeEvent(MobileParty mobileParty)
		{
			CampaignEvents.Instance._onMobilePartyLeftSiegeEventEvent.Invoke(mobileParty);
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x00024AF0 File Offset: 0x00022CF0
		public static IMbEvent<SiegeEvent> OnBlockadeActivatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onBlockadeActivatedEvent;
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00024AFC File Offset: 0x00022CFC
		public override void OnBlockadeActivated(SiegeEvent siegeEvent)
		{
			CampaignEvents.Instance._onBlockadeActivatedEvent.Invoke(siegeEvent);
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x00024B0E File Offset: 0x00022D0E
		public static IMbEvent<SiegeEvent> OnBlockadeDeactivatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onBlockadeDeactivatedEvent;
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00024B1A File Offset: 0x00022D1A
		public override void OnBlockadeDeactivated(SiegeEvent siegeEvent)
		{
			CampaignEvents.Instance._onBlockadeDeactivatedEvent.Invoke(siegeEvent);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00024B2C File Offset: 0x00022D2C
		public static IMbEvent<MapMarker> OnMapMarkerCreatedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMapMarkerCreatedEvent;
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00024B38 File Offset: 0x00022D38
		public override void OnMapMarkerCreated(MapMarker mapMarker)
		{
			CampaignEvents.Instance._onMapMarkerCreatedEvent.Invoke(mapMarker);
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00024B4A File Offset: 0x00022D4A
		public static IMbEvent<MapMarker> OnMapMarkerRemovedEvent
		{
			get
			{
				return CampaignEvents.Instance._onMapMarkerRemovedEvent;
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00024B56 File Offset: 0x00022D56
		public override void OnMapMarkerRemoved(MapMarker mapMarker)
		{
			CampaignEvents.Instance._onMapMarkerRemovedEvent.Invoke(mapMarker);
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00024B68 File Offset: 0x00022D68
		public static IMbEvent<Kingdom, Kingdom> OnAllianceStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onAllianceStartedEvent;
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00024B74 File Offset: 0x00022D74
		public override void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
			CampaignEvents.Instance._onAllianceStartedEvent.Invoke(kingdom1, kingdom2);
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00024B87 File Offset: 0x00022D87
		public static IMbEvent<Kingdom, Kingdom> OnAllianceEndedEvent
		{
			get
			{
				return CampaignEvents.Instance._onAllianceEndedEvent;
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00024B93 File Offset: 0x00022D93
		public override void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			CampaignEvents.Instance._onAllianceEndedEvent.Invoke(kingdom1, kingdom2);
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00024BA6 File Offset: 0x00022DA6
		public static IMbEvent<Kingdom, Kingdom, Kingdom> OnCallToWarAgreementStartedEvent
		{
			get
			{
				return CampaignEvents.Instance._onCallToWarAgreementStartedEvent;
			}
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00024BB2 File Offset: 0x00022DB2
		public override void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			CampaignEvents.Instance._onCallToWarAgreementStartedEvent.Invoke(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00024BC6 File Offset: 0x00022DC6
		public static IMbEvent<Kingdom, Kingdom, Kingdom> OnCallToWarAgreementEndedEvent
		{
			get
			{
				return CampaignEvents.Instance._onCallToWarAgreementEndedEvent;
			}
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00024BD2 File Offset: 0x00022DD2
		public override void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			CampaignEvents.Instance._onCallToWarAgreementEndedEvent.Invoke(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00024BE6 File Offset: 0x00022DE6
		public static ReferenceIMBEvent<Hero, bool> CanHeroLeadPartyEvent
		{
			get
			{
				return CampaignEvents.Instance._canHeroLeadPartyEvent;
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00024BF2 File Offset: 0x00022DF2
		public override void CanHeroLeadParty(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canHeroLeadPartyEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00024C05 File Offset: 0x00022E05
		public static ReferenceIMBEvent<Hero, bool> CanHeroMarryEvent
		{
			get
			{
				return CampaignEvents.Instance._canMarryEvent;
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00024C11 File Offset: 0x00022E11
		public override void CanHeroMarry(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canMarryEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00024C24 File Offset: 0x00022E24
		public static ReferenceIMBEvent<Hero, bool> CanHeroEquipmentBeChangedEvent
		{
			get
			{
				return CampaignEvents.Instance._canHeroEquipmentBeChangedEvent;
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00024C30 File Offset: 0x00022E30
		public override void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canHeroEquipmentBeChangedEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00024C43 File Offset: 0x00022E43
		public static ReferenceIMBEvent<Hero, bool> CanBeGovernorOrHavePartyRoleEvent
		{
			get
			{
				return CampaignEvents.Instance._canBeGovernorOrHavePartyRoleEvent;
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00024C4F File Offset: 0x00022E4F
		public override void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canBeGovernorOrHavePartyRoleEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00024C62 File Offset: 0x00022E62
		public static ReferenceIMBEvent<Hero, KillCharacterAction.KillCharacterActionDetail, bool> CanHeroDieEvent
		{
			get
			{
				return CampaignEvents.Instance._canHeroDieEvent;
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00024C6E File Offset: 0x00022E6E
		public override void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			CampaignEvents.Instance._canHeroDieEvent.Invoke(hero, causeOfDeath, ref result);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00024C82 File Offset: 0x00022E82
		public static ReferenceIMBEvent<Hero, bool> CanPlayerMeetWithHeroAfterConversationEvent
		{
			get
			{
				return CampaignEvents.Instance._canPlayerMeetWithHeroAfterConversationEvent;
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00024C8E File Offset: 0x00022E8E
		public override void CanPlayerMeetWithHeroAfterConversation(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canPlayerMeetWithHeroAfterConversationEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00024CA1 File Offset: 0x00022EA1
		public static ReferenceIMBEvent<Hero, bool> CanHeroBecomePrisonerEvent
		{
			get
			{
				return CampaignEvents.Instance._canHeroBecomePrisonerEvent;
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00024CAD File Offset: 0x00022EAD
		public override void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canHeroBecomePrisonerEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00024CC0 File Offset: 0x00022EC0
		public static ReferenceIMBEvent<Hero, bool> CanMoveToSettlementEvent
		{
			get
			{
				return CampaignEvents.Instance._canMoveToSettlementEvent;
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00024CCC File Offset: 0x00022ECC
		public override void CanMoveToSettlement(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canMoveToSettlementEvent.Invoke(hero, ref result);
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00024CDF File Offset: 0x00022EDF
		public static ReferenceIMBEvent<Hero, bool> CanHaveCampaignIssuesEvent
		{
			get
			{
				return CampaignEvents.Instance._canHaveCampaignIssues;
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00024CEB File Offset: 0x00022EEB
		public override void CanHaveCampaignIssues(Hero hero, ref bool result)
		{
			CampaignEvents.Instance._canHaveCampaignIssues.Invoke(hero, ref result);
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00024CFE File Offset: 0x00022EFE
		public static ReferenceIMBEvent<Settlement, object, int> IsSettlementBusyEvent
		{
			get
			{
				return CampaignEvents.Instance._isSettlementBusy;
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00024D0A File Offset: 0x00022F0A
		public override void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
		{
			CampaignEvents.Instance._isSettlementBusy.Invoke(settlement, asker, ref priority);
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00024D1E File Offset: 0x00022F1E
		public static IMbEvent<IFaction> OnMapEventContinuityNeedsUpdateEvent
		{
			get
			{
				return CampaignEvents.Instance._onMapEventContinuityNeedsUpdate;
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00024D2A File Offset: 0x00022F2A
		public override void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
			CampaignEvents.Instance._onMapEventContinuityNeedsUpdate.Invoke(faction);
		}

		// Token: 0x0400018C RID: 396
		private readonly MbEvent _onPlayerBodyPropertiesChangedEvent = new MbEvent();

		// Token: 0x0400018D RID: 397
		private readonly MbEvent<BarterData> _barterablesRequested = new MbEvent<BarterData>();

		// Token: 0x0400018E RID: 398
		private readonly MbEvent<Hero, bool> _heroLevelledUp = new MbEvent<Hero, bool>();

		// Token: 0x0400018F RID: 399
		private readonly MbEvent<BanditPartyComponent, Hideout> _onHomeHideoutChangedEvent = new MbEvent<BanditPartyComponent, Hideout>();

		// Token: 0x04000190 RID: 400
		private readonly MbEvent<Hero, SkillObject, int, bool> _heroGainedSkill = new MbEvent<Hero, SkillObject, int, bool>();

		// Token: 0x04000191 RID: 401
		private readonly MbEvent _onCharacterCreationIsOverEvent = new MbEvent();

		// Token: 0x04000192 RID: 402
		private readonly MbEvent<Hero, bool> _onHeroCreated = new MbEvent<Hero, bool>();

		// Token: 0x04000193 RID: 403
		private readonly MbEvent<Hero, Hero.CharacterStates> _onHeroActivatedEvent = new MbEvent<Hero, Hero.CharacterStates>();

		// Token: 0x04000194 RID: 404
		private readonly MbEvent<Hero, Occupation> _heroOccupationChangedEvent = new MbEvent<Hero, Occupation>();

		// Token: 0x04000195 RID: 405
		private readonly MbEvent<Hero> _onHeroWounded = new MbEvent<Hero>();

		// Token: 0x04000196 RID: 406
		private readonly MbEvent<Hero, Hero, List<Barterable>> _onBarterAcceptedEvent = new MbEvent<Hero, Hero, List<Barterable>>();

		// Token: 0x04000197 RID: 407
		private readonly MbEvent<Hero, Hero, List<Barterable>> _onBarterCanceledEvent = new MbEvent<Hero, Hero, List<Barterable>>();

		// Token: 0x04000198 RID: 408
		private readonly MbEvent<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero> _heroRelationChanged = new MbEvent<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>();

		// Token: 0x04000199 RID: 409
		private readonly MbEvent<QuestBase, bool> _questLogAddedEvent = new MbEvent<QuestBase, bool>();

		// Token: 0x0400019A RID: 410
		private readonly MbEvent<IssueBase, bool> _issueLogAddedEvent = new MbEvent<IssueBase, bool>();

		// Token: 0x0400019B RID: 411
		private readonly MbEvent<Clan, bool> _clanTierIncrease = new MbEvent<Clan, bool>();

		// Token: 0x0400019C RID: 412
		private readonly MbEvent<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool> _clanChangedKingdom = new MbEvent<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>();

		// Token: 0x0400019D RID: 413
		private readonly MbEvent<Clan, Kingdom, Kingdom> _onClanDefected = new MbEvent<Clan, Kingdom, Kingdom>();

		// Token: 0x0400019E RID: 414
		private readonly MbEvent<Clan, bool> _onClanCreatedEvent = new MbEvent<Clan, bool>();

		// Token: 0x0400019F RID: 415
		private readonly MbEvent<Hero, MobileParty> _onHeroJoinedPartyEvent = new MbEvent<Hero, MobileParty>();

		// Token: 0x040001A0 RID: 416
		private readonly MbEvent<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ValueTuple<int, string>, bool> _heroOrPartyTradedGold = new MbEvent<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ValueTuple<int, string>, bool>();

		// Token: 0x040001A1 RID: 417
		private readonly MbEvent<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ItemRosterElement, bool> _heroOrPartyGaveItem = new MbEvent<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ItemRosterElement, bool>();

		// Token: 0x040001A2 RID: 418
		private readonly MbEvent<MobileParty> _banditPartyRecruited = new MbEvent<MobileParty>();

		// Token: 0x040001A3 RID: 419
		private readonly MbEvent<KingdomDecision, bool> _kingdomDecisionAdded = new MbEvent<KingdomDecision, bool>();

		// Token: 0x040001A4 RID: 420
		private readonly MbEvent<KingdomDecision, bool> _kingdomDecisionCancelled = new MbEvent<KingdomDecision, bool>();

		// Token: 0x040001A5 RID: 421
		private readonly MbEvent<KingdomDecision, DecisionOutcome, bool> _kingdomDecisionConcluded = new MbEvent<KingdomDecision, DecisionOutcome, bool>();

		// Token: 0x040001A6 RID: 422
		private readonly MbEvent<MobileParty> _partyAttachedParty = new MbEvent<MobileParty>();

		// Token: 0x040001A7 RID: 423
		private readonly MbEvent<MobileParty> _nearbyPartyAddedToPlayerMapEvent = new MbEvent<MobileParty>();

		// Token: 0x040001A8 RID: 424
		private readonly MbEvent<Army> _armyCreated = new MbEvent<Army>();

		// Token: 0x040001A9 RID: 425
		private readonly MbEvent<Army, Army.ArmyDispersionReason, bool> _armyDispersed = new MbEvent<Army, Army.ArmyDispersionReason, bool>();

		// Token: 0x040001AA RID: 426
		private readonly MbEvent<Army, IMapPoint> _armyGathered = new MbEvent<Army, IMapPoint>();

		// Token: 0x040001AB RID: 427
		private readonly MbEvent<Hero, PerkObject> _perkOpenedEvent = new MbEvent<Hero, PerkObject>();

		// Token: 0x040001AC RID: 428
		private readonly MbEvent<Hero, PerkObject> _perkResetEvent = new MbEvent<Hero, PerkObject>();

		// Token: 0x040001AD RID: 429
		private readonly MbEvent<TraitObject, int> _playerTraitChangedEvent = new MbEvent<TraitObject, int>();

		// Token: 0x040001AE RID: 430
		private readonly MbEvent<Village, Village.VillageStates, Village.VillageStates, MobileParty> _villageStateChanged = new MbEvent<Village, Village.VillageStates, Village.VillageStates, MobileParty>();

		// Token: 0x040001AF RID: 431
		private readonly MbEvent<MobileParty, Settlement, Hero> _settlementEntered = new MbEvent<MobileParty, Settlement, Hero>();

		// Token: 0x040001B0 RID: 432
		private readonly MbEvent<MobileParty, Settlement, Hero> _afterSettlementEntered = new MbEvent<MobileParty, Settlement, Hero>();

		// Token: 0x040001B1 RID: 433
		private readonly MbEvent<MobileParty, Settlement, Hero> _beforeSettlementEntered = new MbEvent<MobileParty, Settlement, Hero>();

		// Token: 0x040001B2 RID: 434
		private readonly MbEvent<Town, CharacterObject, CharacterObject> _mercenaryTroopChangedInTown = new MbEvent<Town, CharacterObject, CharacterObject>();

		// Token: 0x040001B3 RID: 435
		private readonly MbEvent<Town, int, int> _mercenaryNumberChangedInTown = new MbEvent<Town, int, int>();

		// Token: 0x040001B4 RID: 436
		private readonly MbEvent<Alley, Hero, Hero> _alleyOwnerChanged = new MbEvent<Alley, Hero, Hero>();

		// Token: 0x040001B5 RID: 437
		private readonly MbEvent<Alley, TroopRoster> _alleyOccupiedByPlayer = new MbEvent<Alley, TroopRoster>();

		// Token: 0x040001B6 RID: 438
		private readonly MbEvent<Alley> _alleyClearedByPlayer = new MbEvent<Alley>();

		// Token: 0x040001B7 RID: 439
		private readonly MbEvent<Hero, Hero, Romance.RomanceLevelEnum> _romanticStateChanged = new MbEvent<Hero, Hero, Romance.RomanceLevelEnum>();

		// Token: 0x040001B8 RID: 440
		private readonly MbEvent<Hero, Hero, bool> _beforeHeroesMarried = new MbEvent<Hero, Hero, bool>();

		// Token: 0x040001B9 RID: 441
		private readonly MbEvent<int, Town> _playerEliminatedFromTournament = new MbEvent<int, Town>();

		// Token: 0x040001BA RID: 442
		private readonly MbEvent<Town> _playerStartedTournamentMatch = new MbEvent<Town>();

		// Token: 0x040001BB RID: 443
		private readonly MbEvent<Town> _tournamentStarted = new MbEvent<Town>();

		// Token: 0x040001BC RID: 444
		private readonly MbEvent<IFaction, IFaction, DeclareWarAction.DeclareWarDetail> _warDeclared = new MbEvent<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>();

		// Token: 0x040001BD RID: 445
		private readonly MbEvent<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject> _tournamentFinished = new MbEvent<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>();

		// Token: 0x040001BE RID: 446
		private readonly MbEvent<Town> _tournamentCancelled = new MbEvent<Town>();

		// Token: 0x040001BF RID: 447
		private readonly MbEvent<PartyBase, PartyBase, object, bool> _battleStarted = new MbEvent<PartyBase, PartyBase, object, bool>();

		// Token: 0x040001C0 RID: 448
		private readonly MbEvent<Settlement, Clan> _rebellionFinished = new MbEvent<Settlement, Clan>();

		// Token: 0x040001C1 RID: 449
		private readonly MbEvent<Town, bool> _townRebelliousStateChanged = new MbEvent<Town, bool>();

		// Token: 0x040001C2 RID: 450
		private readonly MbEvent<Settlement, Clan> _rebelliousClanDisbandedAtSettlement = new MbEvent<Settlement, Clan>();

		// Token: 0x040001C3 RID: 451
		private readonly MbEvent<MobileParty, ItemRoster> _itemsLooted = new MbEvent<MobileParty, ItemRoster>();

		// Token: 0x040001C4 RID: 452
		private readonly MbEvent<MobileParty, PartyBase> _mobilePartyDestroyed = new MbEvent<MobileParty, PartyBase>();

		// Token: 0x040001C5 RID: 453
		private readonly MbEvent<MobileParty> _mobilePartyCreated = new MbEvent<MobileParty>();

		// Token: 0x040001C6 RID: 454
		private readonly MbEvent<IInteractablePoint> _mapInteractableCreated = new MbEvent<IInteractablePoint>();

		// Token: 0x040001C7 RID: 455
		private readonly MbEvent<IInteractablePoint> _mapInteractableDestroyed = new MbEvent<IInteractablePoint>();

		// Token: 0x040001C8 RID: 456
		private readonly MbEvent<MobileParty, bool> _mobilePartyQuestStatusChanged = new MbEvent<MobileParty, bool>();

		// Token: 0x040001C9 RID: 457
		private readonly MbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool> _heroKilled = new MbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>();

		// Token: 0x040001CA RID: 458
		private readonly MbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool> _onBeforeHeroKilled = new MbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>();

		// Token: 0x040001CB RID: 459
		private readonly MbEvent<Hero, int> _childEducationCompleted = new MbEvent<Hero, int>();

		// Token: 0x040001CC RID: 460
		private readonly MbEvent<Hero> _heroComesOfAge = new MbEvent<Hero>();

		// Token: 0x040001CD RID: 461
		private readonly MbEvent<Hero> _heroGrowsOutOfInfancyEvent = new MbEvent<Hero>();

		// Token: 0x040001CE RID: 462
		private readonly MbEvent<Hero> _heroReachesTeenAgeEvent = new MbEvent<Hero>();

		// Token: 0x040001CF RID: 463
		private readonly MbEvent<Hero, Hero> _characterDefeated = new MbEvent<Hero, Hero>();

		// Token: 0x040001D0 RID: 464
		private readonly MbEvent<Kingdom, Clan> _rulingClanChanged = new MbEvent<Kingdom, Clan>();

		// Token: 0x040001D1 RID: 465
		private readonly MbEvent<PartyBase, Hero> _heroPrisonerTaken = new MbEvent<PartyBase, Hero>();

		// Token: 0x040001D2 RID: 466
		private readonly MbEvent<Hero, PartyBase, IFaction, EndCaptivityDetail, bool> _heroPrisonerReleased = new MbEvent<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>();

		// Token: 0x040001D3 RID: 467
		private readonly MbEvent<Hero, bool> _characterBecameFugitiveEvent = new MbEvent<Hero, bool>();

		// Token: 0x040001D4 RID: 468
		private readonly MbEvent<Hero> _playerMetHero = new MbEvent<Hero>();

		// Token: 0x040001D5 RID: 469
		private readonly MbEvent<Hero> _playerLearnsAboutHero = new MbEvent<Hero>();

		// Token: 0x040001D6 RID: 470
		private readonly MbEvent<Hero, int, bool> _renownGained = new MbEvent<Hero, int, bool>();

		// Token: 0x040001D7 RID: 471
		private readonly MbEvent<IFaction, float> _crimeRatingChanged = new MbEvent<IFaction, float>();

		// Token: 0x040001D8 RID: 472
		private readonly MbEvent<Hero> _newCompanionAdded = new MbEvent<Hero>();

		// Token: 0x040001D9 RID: 473
		private readonly MbEvent<IMission> _afterMissionStarted = new MbEvent<IMission>();

		// Token: 0x040001DA RID: 474
		private readonly MbEvent<MenuCallbackArgs> _gameMenuOpened = new MbEvent<MenuCallbackArgs>();

		// Token: 0x040001DB RID: 475
		private readonly MbEvent<MenuCallbackArgs> _afterGameMenuInitializedEvent = new MbEvent<MenuCallbackArgs>();

		// Token: 0x040001DC RID: 476
		private readonly MbEvent<MenuCallbackArgs> _beforeGameMenuOpenedEvent = new MbEvent<MenuCallbackArgs>();

		// Token: 0x040001DD RID: 477
		private readonly MbEvent<IFaction, IFaction, MakePeaceAction.MakePeaceDetail> _makePeace = new MbEvent<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>();

		// Token: 0x040001DE RID: 478
		private readonly MbEvent<Kingdom> _kingdomDestroyed = new MbEvent<Kingdom>();

		// Token: 0x040001DF RID: 479
		private readonly ReferenceMBEvent<Kingdom, bool> _canKingdomBeDiscontinued = new ReferenceMBEvent<Kingdom, bool>();

		// Token: 0x040001E0 RID: 480
		private readonly MbEvent<Kingdom> _kingdomCreated = new MbEvent<Kingdom>();

		// Token: 0x040001E1 RID: 481
		private readonly MbEvent<Village> _villageBecomeNormal = new MbEvent<Village>();

		// Token: 0x040001E2 RID: 482
		private readonly MbEvent<Village> _villageBeingRaided = new MbEvent<Village>();

		// Token: 0x040001E3 RID: 483
		private readonly MbEvent<Village> _villageLooted = new MbEvent<Village>();

		// Token: 0x040001E4 RID: 484
		private readonly MbEvent<Hero, RemoveCompanionAction.RemoveCompanionDetail> _companionRemoved = new MbEvent<Hero, RemoveCompanionAction.RemoveCompanionDetail>();

		// Token: 0x040001E5 RID: 485
		private readonly MbEvent<IAgent> _onAgentJoinedConversationEvent = new MbEvent<IAgent>();

		// Token: 0x040001E6 RID: 486
		private readonly MbEvent<IEnumerable<CharacterObject>> _onConversationEnded = new MbEvent<IEnumerable<CharacterObject>>();

		// Token: 0x040001E7 RID: 487
		private readonly MbEvent<MapEvent> _mapEventEnded = new MbEvent<MapEvent>();

		// Token: 0x040001E8 RID: 488
		private readonly MbEvent<MapEvent, PartyBase, PartyBase> _mapEventStarted = new MbEvent<MapEvent, PartyBase, PartyBase>();

		// Token: 0x040001E9 RID: 489
		private readonly MbEvent<Settlement, FlattenedTroopRoster, Hero, bool> _prisonersChangeInSettlement = new MbEvent<Settlement, FlattenedTroopRoster, Hero, bool>();

		// Token: 0x040001EA RID: 490
		private readonly MbEvent<Hero, BoardGameHelper.BoardGameState> _onPlayerBoardGameOver = new MbEvent<Hero, BoardGameHelper.BoardGameState>();

		// Token: 0x040001EB RID: 491
		private readonly MbEvent<Hero> _onRansomOfferedToPlayer = new MbEvent<Hero>();

		// Token: 0x040001EC RID: 492
		private readonly MbEvent<Hero> _onRansomOfferCancelled = new MbEvent<Hero>();

		// Token: 0x040001ED RID: 493
		private readonly MbEvent<IFaction, int, int> _onPeaceOfferedToPlayer = new MbEvent<IFaction, int, int>();

		// Token: 0x040001EE RID: 494
		private readonly MbEvent<Kingdom, Kingdom> _onTradeAgreementSignedEvent = new MbEvent<Kingdom, Kingdom>();

		// Token: 0x040001EF RID: 495
		private readonly MbEvent<IFaction> _onPeaceOfferResolved = new MbEvent<IFaction>();

		// Token: 0x040001F0 RID: 496
		private readonly MbEvent<Hero, Hero> _onMarriageOfferedToPlayerEvent = new MbEvent<Hero, Hero>();

		// Token: 0x040001F1 RID: 497
		private readonly MbEvent<Hero, Hero> _onMarriageOfferCanceledEvent = new MbEvent<Hero, Hero>();

		// Token: 0x040001F2 RID: 498
		private readonly MbEvent<Kingdom> _onVassalOrMercenaryServiceOfferedToPlayerEvent = new MbEvent<Kingdom>();

		// Token: 0x040001F3 RID: 499
		private readonly MbEvent<Kingdom> _onVassalOrMercenaryServiceOfferCanceledEvent = new MbEvent<Kingdom>();

		// Token: 0x040001F4 RID: 500
		private readonly MbEvent<Clan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails> _onMercenaryServiceStartedEvent = new MbEvent<Clan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails>();

		// Token: 0x040001F5 RID: 501
		private readonly MbEvent<Clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails> _onMercenaryServiceEndedEvent = new MbEvent<Clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails>();

		// Token: 0x040001F6 RID: 502
		private readonly MbEvent<IMission> _onMissionStartedEvent = new MbEvent<IMission>();

		// Token: 0x040001F7 RID: 503
		private readonly MbEvent _beforeMissionOpenedEvent = new MbEvent();

		// Token: 0x040001F8 RID: 504
		private readonly MbEvent<PartyBase> _onPartyRemovedEvent = new MbEvent<PartyBase>();

		// Token: 0x040001F9 RID: 505
		private readonly MbEvent<PartyBase> _onPartySizeChangedEvent = new MbEvent<PartyBase>();

		// Token: 0x040001FA RID: 506
		private readonly MbEvent<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail> _onSettlementOwnerChangedEvent = new MbEvent<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>();

		// Token: 0x040001FB RID: 507
		private readonly MbEvent<Town, Hero, Hero> _onGovernorChangedEvent = new MbEvent<Town, Hero, Hero>();

		// Token: 0x040001FC RID: 508
		private readonly MbEvent<MobileParty, Settlement> _onSettlementLeftEvent = new MbEvent<MobileParty, Settlement>();

		// Token: 0x040001FD RID: 509
		private readonly MbEvent _weeklyTickEvent = new MbEvent();

		// Token: 0x040001FE RID: 510
		private readonly MbEvent _dailyTickEvent = new MbEvent();

		// Token: 0x040001FF RID: 511
		private readonly MbEvent<MobileParty> _dailyTickPartyEvent = new MbEvent<MobileParty>();

		// Token: 0x04000200 RID: 512
		private readonly MbEvent<Town> _dailyTickTownEvent = new MbEvent<Town>();

		// Token: 0x04000201 RID: 513
		private readonly MbEvent<Settlement> _dailyTickSettlementEvent = new MbEvent<Settlement>();

		// Token: 0x04000202 RID: 514
		private readonly MbEvent<Hero> _dailyTickHeroEvent = new MbEvent<Hero>();

		// Token: 0x04000203 RID: 515
		private readonly MbEvent<Clan> _dailyTickClanEvent = new MbEvent<Clan>();

		// Token: 0x04000204 RID: 516
		private readonly MbEvent<List<CampaignTutorial>> _collectAvailableTutorialsEvent = new MbEvent<List<CampaignTutorial>>();

		// Token: 0x04000205 RID: 517
		private readonly MbEvent<string> _onTutorialCompletedEvent = new MbEvent<string>();

		// Token: 0x04000206 RID: 518
		private readonly MbEvent<Town, Building, int> _onBuildingLevelChangedEvent = new MbEvent<Town, Building, int>();

		// Token: 0x04000207 RID: 519
		private readonly MbEvent _hourlyTickEvent = new MbEvent();

		// Token: 0x04000208 RID: 520
		private readonly MbEvent _quarterHourlyTickEvent = new MbEvent();

		// Token: 0x04000209 RID: 521
		private readonly MbEvent<MobileParty> _hourlyTickPartyEvent = new MbEvent<MobileParty>();

		// Token: 0x0400020A RID: 522
		private readonly MbEvent<Settlement> _hourlyTickSettlementEvent = new MbEvent<Settlement>();

		// Token: 0x0400020B RID: 523
		private readonly MbEvent<Clan> _hourlyTickClanEvent = new MbEvent<Clan>();

		// Token: 0x0400020C RID: 524
		private readonly MbEvent<float> _tickEvent = new MbEvent<float>();

		// Token: 0x0400020D RID: 525
		private readonly MbEvent<CampaignGameStarter> _onSessionLaunchedEvent = new MbEvent<CampaignGameStarter>();

		// Token: 0x0400020E RID: 526
		private readonly MbEvent<CampaignGameStarter> _onAfterSessionLaunchedEvent = new MbEvent<CampaignGameStarter>();

		// Token: 0x0400020F RID: 527
		public const int OnNewGameCreatedPartialFollowUpEventMaxIndex = 100;

		// Token: 0x04000210 RID: 528
		private readonly MbEvent<CampaignGameStarter> _onNewGameCreatedEvent = new MbEvent<CampaignGameStarter>();

		// Token: 0x04000211 RID: 529
		private readonly MbEvent<CampaignGameStarter, int> _onNewGameCreatedPartialFollowUpEvent = new MbEvent<CampaignGameStarter, int>();

		// Token: 0x04000212 RID: 530
		private readonly MbEvent<CampaignGameStarter> _onNewGameCreatedPartialFollowUpEndEvent = new MbEvent<CampaignGameStarter>();

		// Token: 0x04000213 RID: 531
		private readonly MbEvent<CampaignGameStarter> _onGameEarlyLoadedEvent = new MbEvent<CampaignGameStarter>();

		// Token: 0x04000214 RID: 532
		private readonly MbEvent<CampaignGameStarter> _onGameLoadedEvent = new MbEvent<CampaignGameStarter>();

		// Token: 0x04000215 RID: 533
		private readonly MbEvent _onGameLoadFinishedEvent = new MbEvent();

		// Token: 0x04000216 RID: 534
		private readonly MbEvent<MobileParty, PartyThinkParams> _aiHourlyTickEvent = new MbEvent<MobileParty, PartyThinkParams>();

		// Token: 0x04000217 RID: 535
		private readonly MbEvent<MobileParty> _tickPartialHourlyAiEvent = new MbEvent<MobileParty>();

		// Token: 0x04000218 RID: 536
		private readonly MbEvent<MobileParty> _onPartyJoinedArmyEvent = new MbEvent<MobileParty>();

		// Token: 0x04000219 RID: 537
		private readonly MbEvent<MobileParty> _onPartyRemovedFromArmyEvent = new MbEvent<MobileParty>();

		// Token: 0x0400021A RID: 538
		private readonly MbEvent _onPlayerArmyLeaderChangedBehaviorEvent = new MbEvent();

		// Token: 0x0400021B RID: 539
		private readonly MbEvent<IMission> _onMissionEndedEvent = new MbEvent<IMission>();

		// Token: 0x0400021C RID: 540
		private readonly MbEvent<MobileParty> _onQuarterDailyPartyTick = new MbEvent<MobileParty>();

		// Token: 0x0400021D RID: 541
		private readonly MbEvent<MapEvent> _onPlayerBattleEndEvent = new MbEvent<MapEvent>();

		// Token: 0x0400021E RID: 542
		private readonly MbEvent<CharacterObject, int> _onUnitRecruitedEvent = new MbEvent<CharacterObject, int>();

		// Token: 0x0400021F RID: 543
		private readonly MbEvent<Hero> _onChildConceived = new MbEvent<Hero>();

		// Token: 0x04000220 RID: 544
		private readonly MbEvent<Hero, List<Hero>, int> _onGivenBirthEvent = new MbEvent<Hero, List<Hero>, int>();

		// Token: 0x04000221 RID: 545
		private readonly MbEvent<float> _missionTickEvent = new MbEvent<float>();

		// Token: 0x04000222 RID: 546
		private MbEvent _armyOverlaySetDirty = new MbEvent();

		// Token: 0x04000223 RID: 547
		private readonly MbEvent<int> _playerDesertedBattle = new MbEvent<int>();

		// Token: 0x04000224 RID: 548
		private MbEvent<PartyBase> _partyVisibilityChanged = new MbEvent<PartyBase>();

		// Token: 0x04000225 RID: 549
		private readonly MbEvent<Track> _trackDetectedEvent = new MbEvent<Track>();

		// Token: 0x04000226 RID: 550
		private readonly MbEvent<Track> _trackLostEvent = new MbEvent<Track>();

		// Token: 0x04000227 RID: 551
		private readonly MbEvent<Dictionary<string, int>> _locationCharactersAreReadyToSpawn = new MbEvent<Dictionary<string, int>>();

		// Token: 0x04000228 RID: 552
		private readonly ReferenceMBEvent<MatrixFrame> _onBeforePlayerAgentSpawn = new ReferenceMBEvent<MatrixFrame>();

		// Token: 0x04000229 RID: 553
		private readonly MbEvent _onPlayerAgentSpawned = new MbEvent();

		// Token: 0x0400022A RID: 554
		private readonly MbEvent _locationCharactersSimulatedSpawned = new MbEvent();

		// Token: 0x0400022B RID: 555
		private readonly MbEvent<CharacterObject, CharacterObject, int> _playerUpgradedTroopsEvent = new MbEvent<CharacterObject, CharacterObject, int>();

		// Token: 0x0400022C RID: 556
		private readonly MbEvent<CharacterObject, CharacterObject, PartyBase, WeaponComponentData, bool, int> _onHeroCombatHitEvent = new MbEvent<CharacterObject, CharacterObject, PartyBase, WeaponComponentData, bool, int>();

		// Token: 0x0400022D RID: 557
		private readonly MbEvent<CharacterObject> _characterPortraitPopUpOpenedEvent = new MbEvent<CharacterObject>();

		// Token: 0x0400022E RID: 558
		private CampaignTimeControlMode _timeControlModeBeforePopUpOpened;

		// Token: 0x0400022F RID: 559
		private readonly MbEvent _characterPortraitPopUpClosedEvent = new MbEvent();

		// Token: 0x04000230 RID: 560
		private readonly MbEvent<Hero> _playerStartTalkFromMenu = new MbEvent<Hero>();

		// Token: 0x04000231 RID: 561
		private readonly MbEvent<GameMenu, GameMenuOption> _gameMenuOptionSelectedEvent = new MbEvent<GameMenu, GameMenuOption>();

		// Token: 0x04000232 RID: 562
		private readonly MbEvent<CharacterObject> _playerStartRecruitmentEvent = new MbEvent<CharacterObject>();

		// Token: 0x04000233 RID: 563
		private readonly MbEvent<Hero, Hero> _onBeforePlayerCharacterChangedEvent = new MbEvent<Hero, Hero>();

		// Token: 0x04000234 RID: 564
		private readonly MbEvent<Hero, Hero, MobileParty, bool> _onPlayerCharacterChangedEvent = new MbEvent<Hero, Hero, MobileParty, bool>();

		// Token: 0x04000235 RID: 565
		private readonly MbEvent<Hero, Hero> _onClanLeaderChangedEvent = new MbEvent<Hero, Hero>();

		// Token: 0x04000236 RID: 566
		private readonly MbEvent<SiegeEvent> _onSiegeEventStartedEvent = new MbEvent<SiegeEvent>();

		// Token: 0x04000237 RID: 567
		private readonly MbEvent _onPlayerSiegeStartedEvent = new MbEvent();

		// Token: 0x04000238 RID: 568
		private readonly MbEvent<SiegeEvent> _onSiegeEventEndedEvent = new MbEvent<SiegeEvent>();

		// Token: 0x04000239 RID: 569
		private readonly MbEvent<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>> _siegeAftermathAppliedEvent = new MbEvent<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>>();

		// Token: 0x0400023A RID: 570
		private readonly MbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, SiegeBombardTargets> _onSiegeBombardmentHitEvent = new MbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, SiegeBombardTargets>();

		// Token: 0x0400023B RID: 571
		private readonly MbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool> _onSiegeBombardmentWallHitEvent = new MbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool>();

		// Token: 0x0400023C RID: 572
		private readonly MbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType> _onSiegeEngineDestroyedEvent = new MbEvent<MobileParty, Settlement, BattleSideEnum, SiegeEngineType>();

		// Token: 0x0400023D RID: 573
		private readonly MbEvent<List<TradeRumor>, Settlement> _onTradeRumorIsTakenEvent = new MbEvent<List<TradeRumor>, Settlement>();

		// Token: 0x0400023E RID: 574
		private readonly MbEvent<Hero> _onCheckForIssueEvent = new MbEvent<Hero>();

		// Token: 0x0400023F RID: 575
		private readonly MbEvent<IssueBase, IssueBase.IssueUpdateDetails, Hero> _onIssueUpdatedEvent = new MbEvent<IssueBase, IssueBase.IssueUpdateDetails, Hero>();

		// Token: 0x04000240 RID: 576
		private readonly MbEvent<MobileParty, TroopRoster> _onTroopsDesertedEvent = new MbEvent<MobileParty, TroopRoster>();

		// Token: 0x04000241 RID: 577
		private readonly MbEvent<Hero, Settlement, Hero, CharacterObject, int> _onTroopRecruitedEvent = new MbEvent<Hero, Settlement, Hero, CharacterObject, int>();

		// Token: 0x04000242 RID: 578
		private readonly MbEvent<Hero, Settlement, TroopRoster> _onTroopGivenToSettlementEvent = new MbEvent<Hero, Settlement, TroopRoster>();

		// Token: 0x04000243 RID: 579
		private readonly MbEvent<PartyBase, PartyBase, ItemRosterElement, int, Settlement> _onItemSoldEvent = new MbEvent<PartyBase, PartyBase, ItemRosterElement, int, Settlement>();

		// Token: 0x04000244 RID: 580
		private readonly MbEvent<MobileParty, Town, List<ValueTuple<EquipmentElement, int>>> _onCaravanTransactionCompletedEvent = new MbEvent<MobileParty, Town, List<ValueTuple<EquipmentElement, int>>>();

		// Token: 0x04000245 RID: 581
		private readonly MbEvent<PartyBase, PartyBase, TroopRoster> _onPrisonerSoldEvent = new MbEvent<PartyBase, PartyBase, TroopRoster>();

		// Token: 0x04000246 RID: 582
		private readonly MbEvent<MobileParty> _onPartyDisbandStartedEvent = new MbEvent<MobileParty>();

		// Token: 0x04000247 RID: 583
		private readonly MbEvent<MobileParty, Settlement> _onPartyDisbandedEvent = new MbEvent<MobileParty, Settlement>();

		// Token: 0x04000248 RID: 584
		private readonly MbEvent<MobileParty> _onPartyDisbandCanceledEvent = new MbEvent<MobileParty>();

		// Token: 0x04000249 RID: 585
		private readonly MbEvent<PartyBase, PartyBase> _hideoutSpottedEvent = new MbEvent<PartyBase, PartyBase>();

		// Token: 0x0400024A RID: 586
		private readonly MbEvent<Settlement> _hideoutDeactivatedEvent = new MbEvent<Settlement>();

		// Token: 0x0400024B RID: 587
		private readonly MbEvent<Hero, Hero, float> _heroSharedFoodWithAnotherHeroEvent = new MbEvent<Hero, Hero, float>();

		// Token: 0x0400024C RID: 588
		private readonly MbEvent<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool> _playerInventoryExchangeEvent = new MbEvent<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool>();

		// Token: 0x0400024D RID: 589
		private readonly MbEvent<ItemRoster> _onItemsDiscardedByPlayerEvent = new MbEvent<ItemRoster>();

		// Token: 0x0400024E RID: 590
		private readonly MbEvent<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> _persuasionProgressCommittedEvent = new MbEvent<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();

		// Token: 0x0400024F RID: 591
		private readonly MbEvent<QuestBase, QuestBase.QuestCompleteDetails> _onQuestCompletedEvent = new MbEvent<QuestBase, QuestBase.QuestCompleteDetails>();

		// Token: 0x04000250 RID: 592
		private readonly MbEvent<QuestBase> _onQuestStartedEvent = new MbEvent<QuestBase>();

		// Token: 0x04000251 RID: 593
		private readonly MbEvent<ItemObject, Settlement, int> _itemProducedEvent = new MbEvent<ItemObject, Settlement, int>();

		// Token: 0x04000252 RID: 594
		private readonly MbEvent<ItemObject, Settlement, int> _itemConsumedEvent = new MbEvent<ItemObject, Settlement, int>();

		// Token: 0x04000253 RID: 595
		private readonly MbEvent<MobileParty> _onPartyConsumedFoodEvent = new MbEvent<MobileParty>();

		// Token: 0x04000254 RID: 596
		private readonly MbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool> _onBeforeMainCharacterDiedEvent = new MbEvent<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>();

		// Token: 0x04000255 RID: 597
		private readonly MbEvent<IssueBase> _onNewIssueCreatedEvent = new MbEvent<IssueBase>();

		// Token: 0x04000256 RID: 598
		private readonly MbEvent<IssueBase, Hero> _onIssueOwnerChangedEvent = new MbEvent<IssueBase, Hero>();

		// Token: 0x04000257 RID: 599
		private readonly MbEvent _onGameOverEvent = new MbEvent();

		// Token: 0x04000258 RID: 600
		private readonly MbEvent<Settlement, MobileParty, bool, MapEvent.BattleTypes> _siegeCompletedEvent = new MbEvent<Settlement, MobileParty, bool, MapEvent.BattleTypes>();

		// Token: 0x04000259 RID: 601
		private readonly MbEvent<Settlement, MobileParty, bool, MapEvent.BattleTypes> _afterSiegeCompletedEvent = new MbEvent<Settlement, MobileParty, bool, MapEvent.BattleTypes>();

		// Token: 0x0400025A RID: 602
		private readonly MbEvent<SiegeEvent, BattleSideEnum, SiegeEngineType> _siegeEngineBuiltEvent = new MbEvent<SiegeEvent, BattleSideEnum, SiegeEngineType>();

		// Token: 0x0400025B RID: 603
		private readonly MbEvent<BattleSideEnum, RaidEventComponent> _raidCompletedEvent = new MbEvent<BattleSideEnum, RaidEventComponent>();

		// Token: 0x0400025C RID: 604
		private readonly MbEvent<BattleSideEnum, ForceVolunteersEventComponent> _forceVolunteersCompletedEvent = new MbEvent<BattleSideEnum, ForceVolunteersEventComponent>();

		// Token: 0x0400025D RID: 605
		private readonly MbEvent<BattleSideEnum, ForceSuppliesEventComponent> _forceSuppliesCompletedEvent = new MbEvent<BattleSideEnum, ForceSuppliesEventComponent>();

		// Token: 0x0400025E RID: 606
		private readonly MbEvent<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState> _hideoutBattleCompletedEvent = new MbEvent<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState>();

		// Token: 0x0400025F RID: 607
		private readonly MbEvent<Clan> _onClanDestroyedEvent = new MbEvent<Clan>();

		// Token: 0x04000260 RID: 608
		private readonly MbEvent<ItemObject, ItemModifier, bool> _onNewItemCraftedEvent = new MbEvent<ItemObject, ItemModifier, bool>();

		// Token: 0x04000261 RID: 609
		private readonly MbEvent<CraftingPiece> _craftingPartUnlockedEvent = new MbEvent<CraftingPiece>();

		// Token: 0x04000262 RID: 610
		private readonly MbEvent<Workshop> _onWorkshopInitializedEvent = new MbEvent<Workshop>();

		// Token: 0x04000263 RID: 611
		private readonly MbEvent<Workshop, Hero> _onWorkshopOwnerChangedEvent = new MbEvent<Workshop, Hero>();

		// Token: 0x04000264 RID: 612
		private readonly MbEvent<Workshop> _onWorkshopTypeChangedEvent = new MbEvent<Workshop>();

		// Token: 0x04000265 RID: 613
		private readonly MbEvent _onBeforeSaveEvent = new MbEvent();

		// Token: 0x04000266 RID: 614
		private readonly MbEvent _onSaveStartedEvent = new MbEvent();

		// Token: 0x04000267 RID: 615
		private readonly MbEvent<bool, string> _onSaveOverEvent = new MbEvent<bool, string>();

		// Token: 0x04000268 RID: 616
		private readonly MbEvent<List<KeyValuePair<string, string>>> _collectMetadataEntriesEvent = new MbEvent<List<KeyValuePair<string, string>>>();

		// Token: 0x04000269 RID: 617
		private readonly MbEvent<FlattenedTroopRoster> _onPrisonerTakenEvent = new MbEvent<FlattenedTroopRoster>();

		// Token: 0x0400026A RID: 618
		private readonly MbEvent<FlattenedTroopRoster> _onPrisonerReleasedEvent = new MbEvent<FlattenedTroopRoster>();

		// Token: 0x0400026B RID: 619
		private readonly MbEvent<FlattenedTroopRoster> _onMainPartyPrisonerRecruitedEvent = new MbEvent<FlattenedTroopRoster>();

		// Token: 0x0400026C RID: 620
		private readonly MbEvent<MobileParty, FlattenedTroopRoster, Settlement> _onPrisonerDonatedToSettlementEvent = new MbEvent<MobileParty, FlattenedTroopRoster, Settlement>();

		// Token: 0x0400026D RID: 621
		private readonly MbEvent<Hero, EquipmentElement> _onEquipmentSmeltedByHero = new MbEvent<Hero, EquipmentElement>();

		// Token: 0x0400026E RID: 622
		private readonly MbEvent<int> _onPlayerTradeProfit = new MbEvent<int>();

		// Token: 0x0400026F RID: 623
		private readonly MbEvent<Hero, Clan> _onHeroChangedClan = new MbEvent<Hero, Clan>();

		// Token: 0x04000270 RID: 624
		private readonly MbEvent<Hero, HeroGetsBusyReasons> _onHeroGetsBusy = new MbEvent<Hero, HeroGetsBusyReasons>();

		// Token: 0x04000271 RID: 625
		private readonly MbEvent<PartyBase, ItemRoster> _onCollectLootItems = new MbEvent<PartyBase, ItemRoster>();

		// Token: 0x04000272 RID: 626
		private readonly MbEvent<PartyBase, PartyBase, ItemRoster> _onLootDistributedToPartyEvent = new MbEvent<PartyBase, PartyBase, ItemRoster>();

		// Token: 0x04000273 RID: 627
		private readonly MbEvent<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail> _onHeroTeleportationRequestedEvent = new MbEvent<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>();

		// Token: 0x04000274 RID: 628
		private readonly MbEvent<MobileParty> _onPartyLeaderChangeOfferCanceledEvent = new MbEvent<MobileParty>();

		// Token: 0x04000275 RID: 629
		private readonly MbEvent<MobileParty, Hero> _onPartyLeaderChangedEvent = new MbEvent<MobileParty, Hero>();

		// Token: 0x04000276 RID: 630
		private readonly MbEvent<Clan, float> _onClanInfluenceChangedEvent = new MbEvent<Clan, float>();

		// Token: 0x04000277 RID: 631
		private readonly MbEvent<CharacterObject> _onPlayerPartyKnockedOrKilledTroopEvent = new MbEvent<CharacterObject>();

		// Token: 0x04000278 RID: 632
		private readonly MbEvent<DefaultClanFinanceModel.AssetIncomeType, int> _onPlayerEarnedGoldFromAssetEvent = new MbEvent<DefaultClanFinanceModel.AssetIncomeType, int>();

		// Token: 0x04000279 RID: 633
		private readonly MbEvent<Clan, IFaction> _onClanEarnedGoldFromTributeEvent = new MbEvent<Clan, IFaction>();

		// Token: 0x0400027A RID: 634
		private readonly MbEvent _onMainPartyStarving = new MbEvent();

		// Token: 0x0400027B RID: 635
		private readonly MbEvent<Town, bool> _onPlayerJoinedTournamentEvent = new MbEvent<Town, bool>();

		// Token: 0x0400027C RID: 636
		private readonly MbEvent<Hero> _onHeroUnregisteredEvent = new MbEvent<Hero>();

		// Token: 0x0400027D RID: 637
		private readonly MbEvent _onConfigChanged = new MbEvent();

		// Token: 0x0400027E RID: 638
		private readonly MbEvent<Town, CraftingOrder, ItemObject, Hero> _onCraftingOrderCompleted = new MbEvent<Town, CraftingOrder, ItemObject, Hero>();

		// Token: 0x0400027F RID: 639
		private readonly MbEvent<Hero, Crafting.RefiningFormula> _onItemsRefined = new MbEvent<Hero, Crafting.RefiningFormula>();

		// Token: 0x04000280 RID: 640
		private readonly MbEvent<Dictionary<Hero, int>> _onHeirSelectionRequested = new MbEvent<Dictionary<Hero, int>>();

		// Token: 0x04000281 RID: 641
		private readonly MbEvent<Hero> _onHeirSelectionOver = new MbEvent<Hero>();

		// Token: 0x04000282 RID: 642
		private readonly MbEvent<MobileParty> _onMobilePartyRaftStateChanged = new MbEvent<MobileParty>();

		// Token: 0x04000283 RID: 643
		private readonly MbEvent<CharacterCreationManager> _onCharacterCreationInitialized = new MbEvent<CharacterCreationManager>();

		// Token: 0x04000284 RID: 644
		private readonly MbEvent<PartyBase, Ship, DestroyShipAction.ShipDestroyDetail> _onShipDestroyedEvent = new MbEvent<PartyBase, Ship, DestroyShipAction.ShipDestroyDetail>();

		// Token: 0x04000285 RID: 645
		private readonly MbEvent<Ship, PartyBase, ChangeShipOwnerAction.ShipOwnerChangeDetail> _onShipOwnerChangedEvent = new MbEvent<Ship, PartyBase, ChangeShipOwnerAction.ShipOwnerChangeDetail>();

		// Token: 0x04000286 RID: 646
		private readonly MbEvent<Ship, Settlement> _onShipRepairedEvent = new MbEvent<Ship, Settlement>();

		// Token: 0x04000287 RID: 647
		private readonly MbEvent<Ship, Settlement> _onShipCreatedEvent = new MbEvent<Ship, Settlement>();

		// Token: 0x04000288 RID: 648
		private readonly MbEvent<Figurehead> _onFigureheadUnlockedEvent = new MbEvent<Figurehead>();

		// Token: 0x04000289 RID: 649
		private readonly MbEvent<MobileParty, Army> _onPartyLeftArmyEvent = new MbEvent<MobileParty, Army>();

		// Token: 0x0400028A RID: 650
		private readonly MbEvent<PartyBase> _onPartyAddedToMapEventEvent = new MbEvent<PartyBase>();

		// Token: 0x0400028B RID: 651
		private readonly MbEvent<Incident> _onIncidentResolvedEvent = new MbEvent<Incident>();

		// Token: 0x0400028C RID: 652
		private readonly MbEvent<MobileParty> _onMobilePartyNavigationStateChangedEvent = new MbEvent<MobileParty>();

		// Token: 0x0400028D RID: 653
		private readonly MbEvent<MobileParty> _onMobilePartyJoinedToSiegeEventEvent = new MbEvent<MobileParty>();

		// Token: 0x0400028E RID: 654
		private readonly MbEvent<MobileParty> _onMobilePartyLeftSiegeEventEvent = new MbEvent<MobileParty>();

		// Token: 0x0400028F RID: 655
		private readonly MbEvent<SiegeEvent> _onBlockadeActivatedEvent = new MbEvent<SiegeEvent>();

		// Token: 0x04000290 RID: 656
		private readonly MbEvent<SiegeEvent> _onBlockadeDeactivatedEvent = new MbEvent<SiegeEvent>();

		// Token: 0x04000291 RID: 657
		private readonly MbEvent<MapMarker> _onMapMarkerCreatedEvent = new MbEvent<MapMarker>();

		// Token: 0x04000292 RID: 658
		private readonly MbEvent<MapMarker> _onMapMarkerRemovedEvent = new MbEvent<MapMarker>();

		// Token: 0x04000293 RID: 659
		private readonly MbEvent<Kingdom, Kingdom> _onAllianceStartedEvent = new MbEvent<Kingdom, Kingdom>();

		// Token: 0x04000294 RID: 660
		private readonly MbEvent<Kingdom, Kingdom> _onAllianceEndedEvent = new MbEvent<Kingdom, Kingdom>();

		// Token: 0x04000295 RID: 661
		private readonly MbEvent<Kingdom, Kingdom, Kingdom> _onCallToWarAgreementStartedEvent = new MbEvent<Kingdom, Kingdom, Kingdom>();

		// Token: 0x04000296 RID: 662
		private readonly MbEvent<Kingdom, Kingdom, Kingdom> _onCallToWarAgreementEndedEvent = new MbEvent<Kingdom, Kingdom, Kingdom>();

		// Token: 0x04000297 RID: 663
		private readonly ReferenceMBEvent<Hero, bool> _canHeroLeadPartyEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x04000298 RID: 664
		private readonly ReferenceMBEvent<Hero, bool> _canMarryEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x04000299 RID: 665
		private readonly ReferenceMBEvent<Hero, bool> _canHeroEquipmentBeChangedEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x0400029A RID: 666
		private readonly ReferenceMBEvent<Hero, bool> _canBeGovernorOrHavePartyRoleEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x0400029B RID: 667
		private readonly ReferenceMBEvent<Hero, KillCharacterAction.KillCharacterActionDetail, bool> _canHeroDieEvent = new ReferenceMBEvent<Hero, KillCharacterAction.KillCharacterActionDetail, bool>();

		// Token: 0x0400029C RID: 668
		private readonly ReferenceMBEvent<Hero, bool> _canPlayerMeetWithHeroAfterConversationEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x0400029D RID: 669
		private readonly ReferenceMBEvent<Hero, bool> _canHeroBecomePrisonerEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x0400029E RID: 670
		private readonly ReferenceMBEvent<Hero, bool> _canMoveToSettlementEvent = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x0400029F RID: 671
		private readonly ReferenceMBEvent<Hero, bool> _canHaveCampaignIssues = new ReferenceMBEvent<Hero, bool>();

		// Token: 0x040002A0 RID: 672
		private readonly ReferenceMBEvent<Settlement, object, int> _isSettlementBusy = new ReferenceMBEvent<Settlement, object, int>();

		// Token: 0x040002A1 RID: 673
		private readonly MbEvent<IFaction> _onMapEventContinuityNeedsUpdate = new MbEvent<IFaction>();
	}
}
