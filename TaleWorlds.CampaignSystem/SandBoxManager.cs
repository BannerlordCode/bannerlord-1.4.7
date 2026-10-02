using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A3 RID: 163
	public class SandBoxManager : GameHandler
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00056B06 File Offset: 0x00054D06
		// (set) Token: 0x06001329 RID: 4905 RVA: 0x00056B0E File Offset: 0x00054D0E
		public ISandBoxMissionManager SandBoxMissionManager { get; set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x00056B17 File Offset: 0x00054D17
		// (set) Token: 0x0600132B RID: 4907 RVA: 0x00056B1F File Offset: 0x00054D1F
		public IAgentBehaviorManager AgentBehaviorManager { get; set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x00056B28 File Offset: 0x00054D28
		// (set) Token: 0x0600132D RID: 4909 RVA: 0x00056B30 File Offset: 0x00054D30
		public ISaveManager SandBoxSaveManager { get; set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x00056B39 File Offset: 0x00054D39
		// (set) Token: 0x0600132F RID: 4911 RVA: 0x00056B40 File Offset: 0x00054D40
		public static SandBoxManager Instance { get; private set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001330 RID: 4912 RVA: 0x00056B48 File Offset: 0x00054D48
		// (set) Token: 0x06001331 RID: 4913 RVA: 0x00056B50 File Offset: 0x00054D50
		public CampaignGameStarter GameStarter { get; private set; }

		// Token: 0x06001332 RID: 4914 RVA: 0x00056B5C File Offset: 0x00054D5C
		public void Initialize(CampaignGameStarter gameStarter)
		{
			this.GameStarter = gameStarter;
			gameStarter.AddBehavior(new PartyUpgraderCampaignBehavior());
			gameStarter.AddBehavior(new EncounterGameMenuBehavior());
			gameStarter.AddBehavior(new PlayerCaptivityCampaignBehavior());
			gameStarter.AddBehavior(new BackstoryCampaignBehavior());
			gameStarter.AddBehavior(new TradeCampaignBehavior());
			gameStarter.AddBehavior(new BanditSpawnCampaignBehavior());
			gameStarter.AddBehavior(new DesertersCampaignBehavior());
			gameStarter.AddBehavior(new BanditInteractionsCampaignBehavior());
			gameStarter.AddBehavior(new WorkshopsCharactersCampaignBehavior());
			gameStarter.AddBehavior(new CharacterRelationCampaignBehavior());
			gameStarter.AddBehavior(new DesertionCampaignBehavior());
			gameStarter.AddBehavior(new FoodConsumptionBehavior());
			gameStarter.AddBehavior(new FindingItemOnMapBehavior());
			gameStarter.AddBehavior(new BuildingsCampaignBehavior());
			gameStarter.AddBehavior(new ItemConsumptionBehavior());
			gameStarter.AddBehavior(new GarrisonTroopsCampaignBehavior());
			gameStarter.AddBehavior(new CaravansCampaignBehavior());
			gameStarter.AddBehavior(new CaravanConversationsCampaignBehavior());
			gameStarter.AddBehavior(new GovernorCampaignBehavior());
			gameStarter.AddBehavior(new HideoutCampaignBehavior());
			gameStarter.AddBehavior(new PartiesBuyFoodCampaignBehavior());
			gameStarter.AddBehavior(new PartiesBuyHorseCampaignBehavior());
			gameStarter.AddBehavior(new PoliticalStagnationAndBorderIncidentCampaignBehavior());
			gameStarter.AddBehavior(new PrisonerReleaseCampaignBehavior());
			gameStarter.AddBehavior(new PrisonerCaptureCampaignBehavior());
			gameStarter.AddBehavior(new PrisonerRecruitCampaignBehavior());
			gameStarter.AddBehavior(new RomanceCampaignBehavior());
			gameStarter.AddBehavior(new LordDefectionCampaignBehavior());
			gameStarter.AddBehavior(new PartiesSellPrisonerCampaignBehavior());
			gameStarter.AddBehavior(new PartiesSellLootCampaignBehavior());
			gameStarter.AddBehavior(new SettlementVariablesBehavior());
			gameStarter.AddBehavior(new MilitiasCampaignBehavior());
			gameStarter.AddBehavior(new SettlementClaimantCampaignBehavior());
			gameStarter.AddBehavior(new TradeRumorsCampaignBehavior());
			gameStarter.AddBehavior(new NotablesCampaignBehavior());
			gameStarter.AddBehavior(new LordConversationsCampaignBehavior());
			gameStarter.AddBehavior(new CompanionsCampaignBehavior());
			gameStarter.AddBehavior(new RetrainOutlawPartyMembersBehavior());
			gameStarter.AddBehavior(new RecruitPrisonersCampaignBehavior());
			gameStarter.AddBehavior(new HeroSpawnCampaignBehavior());
			gameStarter.AddBehavior(new TournamentCampaignBehavior());
			gameStarter.AddBehavior(new CraftingCampaignBehavior());
			gameStarter.AddBehavior(new MapTracksCampaignBehavior());
			gameStarter.AddBehavior(new HeroAgentSpawnCampaignBehavior());
			gameStarter.AddBehavior(new NotableHelperCharacterCampaignBehavior());
			gameStarter.AddBehavior(new CharacterDevelopmentCampaignBehavior());
			gameStarter.AddBehavior(new TradeSkillCampaignBehavior());
			gameStarter.AddBehavior(new RecruitmentCampaignBehavior());
			gameStarter.AddBehavior(new VillageHostileActionCampaignBehavior());
			gameStarter.AddBehavior(new PlayerTownVisitCampaignBehavior());
			gameStarter.AddBehavior(new DynamicBodyCampaignBehavior());
			gameStarter.AddBehavior(new VillageTradeBoundCampaignBehavior());
			gameStarter.AddBehavior(new VillageGoodProductionCampaignBehavior());
			gameStarter.AddBehavior(new SiegeAftermathCampaignBehavior());
			gameStarter.AddBehavior(new NPCEquipmentsCampaignBehavior());
			gameStarter.AddBehavior(new VillagerCampaignBehavior());
			gameStarter.AddBehavior(new VillageHealCampaignBehavior());
			gameStarter.AddBehavior(new PlayerVariablesBehavior());
			gameStarter.AddBehavior(new MobilePartyTrainingBehavior());
			gameStarter.AddBehavior(new TradeAgreementsCampaignBehavior());
			gameStarter.AddBehavior(new EducationCampaignBehavior());
			gameStarter.AddBehavior(new RansomOfferCampaignBehavior());
			gameStarter.AddBehavior(new PeaceOfferCampaignBehavior());
			gameStarter.AddBehavior(new MarriageOfferCampaignBehavior());
			gameStarter.AddBehavior(new VassalAndMercenaryOfferCampaignBehavior());
			gameStarter.AddBehavior(new AllianceCampaignBehavior());
			gameStarter.AddBehavior(new TributesCampaignBehaviour());
			gameStarter.AddBehavior(new CommentOnLeaveFactionBehavior());
			gameStarter.AddBehavior(new CommentOnChangeRomanticStateBehavior());
			gameStarter.AddBehavior(new CommentOnChangeSettlementOwnerBehavior());
			gameStarter.AddBehavior(new CommentOnPlayerMeetLordBehavior());
			gameStarter.AddBehavior(new CommentOnEndPlayerBattleBehavior());
			gameStarter.AddBehavior(new CommentOnDefeatCharacterBehavior());
			gameStarter.AddBehavior(new CommentOnCharacterKilledBehavior());
			gameStarter.AddBehavior(new CommentOnChangeVillageStateBehavior());
			gameStarter.AddBehavior(new CommentOnDestroyMobilePartyBehavior());
			gameStarter.AddBehavior(new CommentOnMakePeaceBehavior());
			gameStarter.AddBehavior(new CommentOnDeclareWarBehavior());
			gameStarter.AddBehavior(new CommentOnKingdomDestroyedBehavior());
			gameStarter.AddBehavior(new CommentOnClanDestroyedBehavior());
			gameStarter.AddBehavior(new CommentOnClanLeaderChangedBehavior());
			gameStarter.AddBehavior(new CommentPregnancyBehavior());
			gameStarter.AddBehavior(new CommentChildbirthBehavior());
			gameStarter.AddBehavior(new CommentCharacterBornBehavior());
			gameStarter.AddBehavior(new DefaultLogsCampaignBehavior());
			gameStarter.AddBehavior(new JournalLogsCampaignBehavior());
			gameStarter.AddBehavior(new ViewDataTrackerCampaignBehavior());
			gameStarter.AddBehavior(new AiArmyMemberBehavior());
			gameStarter.AddBehavior(new AiMilitaryBehavior());
			gameStarter.AddBehavior(new AiPatrollingBehavior());
			gameStarter.AddBehavior(new AiEngagePartyBehavior());
			gameStarter.AddBehavior(new AiLandBanditPatrollingBehavior());
			gameStarter.AddBehavior(new AiVisitSettlementBehavior());
			gameStarter.AddBehavior(new AiPartyThinkBehavior());
			gameStarter.AddBehavior(new AIMoveToNearestLandBehavior());
			gameStarter.AddBehavior(new DiplomaticBartersBehavior());
			gameStarter.AddBehavior(new SetPrisonerFreeBarterBehavior());
			gameStarter.AddBehavior(new FiefBarterBehavior());
			gameStarter.AddBehavior(new ItemBarterBehavior());
			gameStarter.AddBehavior(new GoldBarterBehavior());
			gameStarter.AddBehavior(new TransferPrisonerBarterBehavior());
			gameStarter.AddBehavior(new CompanionGrievanceBehavior());
			gameStarter.AddBehavior(new CompanionRolesCampaignBehavior());
			gameStarter.AddBehavior(new PlayerTrackCompanionBehavior());
			gameStarter.AddBehavior(new RebellionsCampaignBehavior());
			gameStarter.AddBehavior(new SallyOutsCampaignBehavior());
			gameStarter.AddBehavior(new CrimeCampaignBehavior());
			gameStarter.AddBehavior(new PlayerArmyWaitBehavior());
			gameStarter.AddBehavior(new ClanVariablesCampaignBehavior());
			gameStarter.AddBehavior(new FactionDiscontinuationCampaignBehavior());
			gameStarter.AddBehavior(new AgingCampaignBehavior());
			gameStarter.AddBehavior(new BattleCampaignBehavior());
			gameStarter.AddBehavior(new WorkshopsCampaignBehavior());
			gameStarter.AddBehavior(new PregnancyCampaignBehavior());
			gameStarter.AddBehavior(new InitialChildGenerationCampaignBehavior());
			gameStarter.AddBehavior(new NotablePowerManagementBehavior());
			gameStarter.AddBehavior(new PerkActivationHandlerCampaignBehavior());
			gameStarter.AddBehavior(new TownSecurityCampaignBehavior());
			gameStarter.AddBehavior(new HeroKnownInformationCampaignBehavior());
			gameStarter.AddBehavior(new DisbandPartyCampaignBehavior());
			gameStarter.AddBehavior(new PartyHealCampaignBehavior());
			gameStarter.AddBehavior(new CampaignBattleRecoveryBehavior());
			gameStarter.AddBehavior(new CampaignWarManagerBehavior());
			gameStarter.AddBehavior(new KingdomDecisionProposalBehavior());
			gameStarter.AddBehavior(new PartyRolesCampaignBehavior());
			gameStarter.AddBehavior(new EmissarySystemCampaignBehavior());
			gameStarter.AddBehavior(new CampaignFactionManagerBehaviour());
			gameStarter.AddBehavior(new SiegeEventCampaignBehavior());
			gameStarter.AddBehavior(new IssuesCampaignBehavior());
			gameStarter.AddBehavior(new InfluenceGainCampaignBehavior());
			gameStarter.AddBehavior(new BannerCampaignBehavior());
			gameStarter.AddBehavior(new TeleportationCampaignBehavior());
			gameStarter.AddBehavior(new ArmyNeedsSuppliesIssueBehavior());
			gameStarter.AddBehavior(new ArtisanCantSellProductsAtAFairPriceIssueBehavior());
			gameStarter.AddBehavior(new ArtisanOverpricedGoodsIssueBehavior());
			gameStarter.AddBehavior(new CapturedByBountyHuntersIssueBehavior());
			gameStarter.AddBehavior(new CaravanAmbushIssueBehavior());
			gameStarter.AddBehavior(new EscortMerchantCaravanIssueBehavior());
			gameStarter.AddBehavior(new ExtortionByDesertersIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsToOffloadStolenGoodsIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsWeaponsIssueQuestBehavior());
			gameStarter.AddBehavior(new RevenueFarmingIssueBehavior());
			gameStarter.AddBehavior(new HeadmanNeedsGrainIssueBehavior());
			gameStarter.AddBehavior(new HeadmanNeedsToDeliverAHerdIssueBehavior());
			gameStarter.AddBehavior(new HeadmanVillageNeedsDraughtAnimalsIssueBehavior());
			gameStarter.AddBehavior(new LadysKnightOutIssueBehavior());
			gameStarter.AddBehavior(new LandLordCompanyOfTroubleIssueBehavior());
			gameStarter.AddBehavior(new LandLordTheArtOfTheTradeIssueBehavior());
			gameStarter.AddBehavior(new LandlordNeedsAccessToVillageCommonsIssueBehavior());
			gameStarter.AddBehavior(new LandLordNeedsManualLaborersIssueBehavior());
			gameStarter.AddBehavior(new LandlordTrainingForRetainersIssueBehavior());
			gameStarter.AddBehavior(new LordNeedsGarrisonTroopsIssueQuestBehavior());
			gameStarter.AddBehavior(new TheConquestOfSettlementIssueBehavior());
			gameStarter.AddBehavior(new VillageNeedsCraftingMaterialsIssueBehavior());
			gameStarter.AddBehavior(new SmugglersIssueBehavior());
			gameStarter.AddBehavior(new LordNeedsHorsesIssueBehavior());
			gameStarter.AddBehavior(new LordsNeedsTutorIssueBehavior());
			gameStarter.AddBehavior(new LordWantsRivalCapturedIssueBehavior());
			gameStarter.AddBehavior(new MerchantArmyOfPoachersIssueBehavior());
			gameStarter.AddBehavior(new MerchantNeedsHelpWithOutlawsIssueQuestBehavior());
			gameStarter.AddBehavior(new NearbyBanditBaseIssueBehavior());
			gameStarter.AddBehavior(new RaidAnEnemyTerritoryIssueBehavior());
			gameStarter.AddBehavior(new ScoutEnemyGarrisonsIssueBehavior());
			gameStarter.AddBehavior(new VillageNeedsToolsIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsRecruitsIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsSpecialWeaponsIssueBehavior());
			gameStarter.AddBehavior(new LesserNobleRevoltIssueBehavior());
			gameStarter.AddBehavior(new BettingFraudIssueBehavior());
			gameStarter.AddBehavior(new DiscardItemsCampaignBehavior());
			gameStarter.AddBehavior(new OrderOfBattleCampaignBehavior());
			gameStarter.AddBehavior(new DisorganizedStateCampaignBehavior());
			gameStarter.AddBehavior(new PerkResetCampaignBehavior());
			gameStarter.AddBehavior(new SiegeAmbushCampaignBehavior());
			gameStarter.AddBehavior(new MapWeatherCampaignBehavior());
			gameStarter.AddBehavior(new GarrisonRecruitmentCampaignBehavior());
			gameStarter.AddBehavior(new PartyDiplomaticHandlerCampaignBehavior());
			gameStarter.AddBehavior(new ParleyCampaignBehavior());
			gameStarter.AddBehavior(new CharacterCreationCampaignBehavior());
			gameStarter.AddBehavior(new IncidentsCampaignBehaviour());
			gameStarter.AddBehavior(new PatrolPartiesCampaignBehavior());
			gameStarter.AddBehavior(new NotableSupportersCampaignBehavior());
			gameStarter.AddModel<CharacterDevelopmentModel>(new DefaultCharacterDevelopmentModel());
			gameStarter.AddModel<ValuationModel>(new DefaultValuationModel());
			gameStarter.AddModel<ItemDiscardModel>(new DefaultItemDiscardModel());
			gameStarter.AddModel<MapVisibilityModel>(new DefaultMapVisibilityModel());
			gameStarter.AddModel<InformationRestrictionModel>(new DefaultInformationRestrictionModel());
			gameStarter.AddModel<MapDistanceModel>(new DefaultMapDistanceModel());
			gameStarter.AddModel<PartyHealingModel>(new DefaultPartyHealingModel());
			gameStarter.AddModel<CaravanModel>(new DefaultCaravanModel());
			gameStarter.AddModel<PartyTrainingModel>(new DefaultPartyTrainingModel());
			gameStarter.AddModel<PartyTradeModel>(new DefaultPartyTradeModel());
			gameStarter.AddModel<RansomValueCalculationModel>(new DefaultRansomValueCalculationModel());
			gameStarter.AddModel<RaidModel>(new DefaultRaidModel());
			gameStarter.AddModel<CombatSimulationModel>(new DefaultCombatSimulationModel());
			gameStarter.AddModel<FleetManagementModel>(new DefaultFleetManagementModel());
			gameStarter.AddModel<CombatXpModel>(new DefaultCombatXpModel());
			gameStarter.AddModel<GenericXpModel>(new DefaultGenericXpModel());
			gameStarter.AddModel<SmithingModel>(new DefaultSmithingModel());
			gameStarter.AddModel<TradeAgreementModel>(new DefaultTradeAgreementModel());
			gameStarter.AddModel<PartySpeedModel>(new DefaultPartySpeedCalculatingModel());
			gameStarter.AddModel<PartyImpairmentModel>(new DefaultPartyImpairmentModel());
			gameStarter.AddModel<CharacterStatsModel>(new DefaultCharacterStatsModel());
			gameStarter.AddModel<EncounterModel>(new DefaultEncounterModel());
			gameStarter.AddModel<MobilePartyFoodConsumptionModel>(new DefaultMobilePartyFoodConsumptionModel());
			gameStarter.AddModel<SceneModel>(new DefaultSceneModel());
			gameStarter.AddModel<PartyFoodBuyingModel>(new DefaultPartyFoodBuyingModel());
			gameStarter.AddModel<PartyMoraleModel>(new DefaultPartyMoraleModel());
			gameStarter.AddModel<DiplomacyModel>(new DefaultDiplomacyModel());
			gameStarter.AddModel<PartyTransitionModel>(new DefaultPartyTransitionModel());
			gameStarter.AddModel<HideoutModel>(new DefaultHideoutModel());
			gameStarter.AddModel<KingdomCreationModel>(new DefaultKingdomCreationModel());
			gameStarter.AddModel<VillageProductionCalculatorModel>(new DefaultVillageProductionCalculatorModel());
			gameStarter.AddModel<VolunteerModel>(new DefaultVolunteerModel());
			gameStarter.AddModel<ArmyManagementCalculationModel>(new DefaultArmyManagementCalculationModel());
			gameStarter.AddModel<BanditDensityModel>(new DefaultBanditDensityModel());
			gameStarter.AddModel<NotableSpawnModel>(new DefaultNotableSpawnModel());
			gameStarter.AddModel<EncounterGameMenuModel>(new DefaultEncounterGameMenuModel());
			gameStarter.AddModel<BattleRewardModel>(new DefaultBattleRewardModel());
			gameStarter.AddModel<RomanceModel>(new DefaultRomanceModel());
			gameStarter.AddModel<MapTrackModel>(new DefaultMapTrackModel());
			gameStarter.AddModel<MapWeatherModel>(new DefaultMapWeatherModel());
			gameStarter.AddModel<RidingModel>(new DefaultRidingModel());
			gameStarter.AddModel<TargetScoreCalculatingModel>(new DefaultTargetScoreCalculatingModel());
			gameStarter.AddModel<CrimeModel>(new DefaultCrimeModel());
			gameStarter.AddModel<DisguiseDetectionModel>(new DefaultDisguiseDetectionModel());
			gameStarter.AddModel<BribeCalculationModel>(new DefaultBribeCalculationModel());
			gameStarter.AddModel<TroopSacrificeModel>(new DefaultTroopSacrificeModel());
			gameStarter.AddModel<SettlementAccessModel>(new DefaultSettlementAccessModel());
			gameStarter.AddModel<KingdomDecisionPermissionModel>(new DefaultKingdomDecisionPermissionModel());
			gameStarter.AddModel<EmissaryModel>(new DefaultEmissaryModel());
			gameStarter.AddModel<MilitaryPowerModel>(new DefaultMilitaryPowerModel());
			gameStarter.AddModel<CampaignShipDamageModel>(new DefaultCampaignShipDamageModel());
			gameStarter.AddModel<ShipCostModel>(new DefaultShipCostModel());
			gameStarter.AddModel<CampaignShipParametersModel>(new DefaultCampaignShipParametersModel());
			gameStarter.AddModel<ClanMemberPartyRoleModel>(new DefaultClanMemberPartyRoleModel());
			gameStarter.AddModel<PartySizeLimitModel>(new DefaultPartySizeLimitModel());
			gameStarter.AddModel<PartyShipLimitModel>(new DefaultPartyShipLimitModel());
			gameStarter.AddModel<PartyWageModel>(new DefaultPartyWageModel());
			gameStarter.AddModel<PartyDesertionModel>(new DefaultPartyDesertionModel());
			gameStarter.AddModel<InventoryCapacityModel>(new DefaultInventoryCapacityModel());
			gameStarter.AddModel<ItemCategorySelector>(new DefaultItemCategorySelector());
			gameStarter.AddModel<ItemValueModel>(new DefaultItemValueModel());
			gameStarter.AddModel<TradeItemPriceFactorModel>(new DefaultTradeItemPriceFactorModel());
			gameStarter.AddModel<SettlementValueModel>(new DefaultSettlementValueModel());
			gameStarter.AddModel<SettlementMilitiaModel>(new DefaultSettlementMilitiaModel());
			gameStarter.AddModel<SettlementEconomyModel>(new DefaultSettlementEconomyModel());
			gameStarter.AddModel<SettlementFoodModel>(new DefaultSettlementFoodModel());
			gameStarter.AddModel<SettlementLoyaltyModel>(new DefaultSettlementLoyaltyModel());
			gameStarter.AddModel<SettlementSecurityModel>(new DefaultSettlementSecurityModel());
			gameStarter.AddModel<SettlementProsperityModel>(new DefaultSettlementProsperityModel());
			gameStarter.AddModel<SettlementGarrisonModel>(new DefaultSettlementGarrisonModel());
			gameStarter.AddModel<SettlementTaxModel>(new DefaultSettlementTaxModel());
			gameStarter.AddModel<HeroAgentLocationModel>(new DefaultHeroAgentLocationModel());
			gameStarter.AddModel<BarterModel>(new DefaultBarterModel());
			gameStarter.AddModel<PersuasionModel>(new DefaultPersuasionModel());
			gameStarter.AddModel<ClanTierModel>(new DefaultClanTierModel());
			gameStarter.AddModel<MinorFactionsModel>(new DefaultMinorFactionsModel());
			gameStarter.AddModel<DefaultDefectionModel>(new DefaultDefectionModel());
			gameStarter.AddModel<ClanPoliticsModel>(new DefaultClanPoliticsModel());
			gameStarter.AddModel<VassalRewardsModel>(new DefaultVassalRewardsModel());
			gameStarter.AddModel<ClanFinanceModel>(new DefaultClanFinanceModel());
			gameStarter.AddModel<HeirSelectionCalculationModel>(new DefaultHeirSelectionCalculationModel());
			gameStarter.AddModel<HeroDeathProbabilityCalculationModel>(new DefaultHeroDeathProbabilityCalculationModel());
			gameStarter.AddModel<BuildingConstructionModel>(new DefaultBuildingConstructionModel());
			gameStarter.AddModel<BuildingEffectModel>(new DefaultBuildingEffectModel());
			gameStarter.AddModel<WallHitPointCalculationModel>(new DefaultWallHitPointCalculationModel());
			gameStarter.AddModel<MarriageModel>(new DefaultMarriageModel());
			gameStarter.AddModel<AgeModel>(new DefaultAgeModel());
			gameStarter.AddModel<PlayerProgressionModel>(new DefaultPlayerProgressionModel());
			gameStarter.AddModel<DailyTroopXpBonusModel>(new DefaultDailyTroopXpBonusModel());
			gameStarter.AddModel<PregnancyModel>(new DefaultPregnancyModel());
			gameStarter.AddModel<NotablePowerModel>(new DefaultNotablePowerModel());
			gameStarter.AddModel<TournamentModel>(new DefaultTournamentModel());
			gameStarter.AddModel<SiegeStrategyActionModel>(new DefaultSiegeStrategyActionModel());
			gameStarter.AddModel<SiegeEventModel>(new DefaultSiegeEventModel());
			gameStarter.AddModel<SiegeAftermathModel>(new DefaultSiegeAftermathModel());
			gameStarter.AddModel<SiegeLordsHallFightModel>(new DefaultSiegeLordsHallFightModel());
			gameStarter.AddModel<CompanionHiringPriceCalculationModel>(new DefaultCompanionHiringPriceCalculationModel());
			gameStarter.AddModel<BuildingScoreCalculationModel>(new DefaultBuildingScoreCalculationModel());
			gameStarter.AddModel<IssueModel>(new DefaultIssueModel());
			gameStarter.AddModel<PrisonerRecruitmentCalculationModel>(new DefaultPrisonerRecruitmentCalculationModel());
			gameStarter.AddModel<PartyTroopUpgradeModel>(new DefaultPartyTroopUpgradeModel());
			gameStarter.AddModel<TavernMercenaryTroopsModel>(new DefaultTavernMercenaryTroopsModel());
			gameStarter.AddModel<WorkshopModel>(new DefaultWorkshopModel());
			gameStarter.AddModel<DifficultyModel>(new DefaultDifficultyModel());
			gameStarter.AddModel<LocationModel>(new DefaultLocationModel());
			gameStarter.AddModel<PrisonerDonationModel>(new DefaultPrisonerDonationModel());
			gameStarter.AddModel<PrisonBreakModel>(new DefaultPrisonBreakModel());
			gameStarter.AddModel<BattleCaptainModel>(new DefaultBattleCaptainModel());
			gameStarter.AddModel<ExecutionRelationModel>(new DefaultExecutionRelationModel());
			gameStarter.AddModel<BannerItemModel>(new DefaultBannerItemModel());
			gameStarter.AddModel<DelayedTeleportationModel>(new DefaultDelayedTeleportationModel());
			gameStarter.AddModel<TroopSupplierProbabilityModel>(new DefaultTroopSupplierProbabilityModel());
			gameStarter.AddModel<CutsceneSelectionModel>(new DefaultCutsceneSelectionModel());
			gameStarter.AddModel<EquipmentSelectionModel>(new DefaultEquipmentSelectionModel());
			gameStarter.AddModel<AlleyModel>(new DefaultAlleyModel());
			gameStarter.AddModel<VoiceOverModel>(new DefaultVoiceOverModel());
			gameStarter.AddModel<CampaignTimeModel>(new DefaultCampaignTimeModel());
			gameStarter.AddModel<VillageTradeModel>(new DefaultVillageTradeModel());
			gameStarter.AddModel<PartyNavigationModel>(new DefaultPartyNavigationModel());
			gameStarter.AddModel<MobilePartyAIModel>(new DefaultMobilePartyAIModel());
			gameStarter.AddModel<HeroCreationModel>(new DefaultHeroCreationModel());
			gameStarter.AddModel<BuildingModel>(new DefaultBuildingModel());
			gameStarter.AddModel<ShipStatModel>(new DefaultShipStatModel());
			gameStarter.AddModel<IncidentModel>(new DefaultIncidentModel());
			gameStarter.AddModel<BodyPropertiesModel>(new DefaultBodyPropertiesModel());
			gameStarter.AddModel<SettlementPatrolModel>(new DefaultSettlementPatrolModel());
			gameStarter.AddModel<AllianceModel>(new DefaultAllianceModel());
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x000578AC File Offset: 0x00055AAC
		public void OnCampaignStart(CampaignGameStarter gameInitializer, GameManagerBase gameManager, bool isSavedCampaign)
		{
			gameManager.RegisterSubModuleObjects(isSavedCampaign);
			gameManager.AfterRegisterSubModuleObjects(isSavedCampaign);
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && isSavedCampaign)
			{
				MBObjectManager.Instance.RemoveTemporaryTypes();
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x000578D7 File Offset: 0x00055AD7
		protected override void OnGameStart()
		{
			SandBoxManager.Instance = Game.Current.GetGameHandler<SandBoxManager>();
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x000578E8 File Offset: 0x00055AE8
		protected override void OnGameEnd()
		{
			SandBoxManager.Instance = null;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x000578F0 File Offset: 0x00055AF0
		public void InitializeSandboxXMLs(bool isSavedCampaign)
		{
			MBObjectManager.Instance.LoadXML("NPCCharacters", false);
			if (!isSavedCampaign)
			{
				MBObjectManager.Instance.LoadXML("Heroes", false);
			}
			if (Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				MBObjectManager.Instance.LoadXML("MPCharacters", false);
			}
			if (!isSavedCampaign)
			{
				MBObjectManager.Instance.LoadXML("Kingdoms", false);
				MBObjectManager.Instance.LoadXML("Factions", false);
			}
			MBObjectManager.Instance.LoadXML("WorkshopTypes", false);
			MBObjectManager.Instance.LoadXML("LocationComplexTemplates", false);
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && !Game.Current.IsEditModeOn)
			{
				MBObjectManager.Instance.LoadXML("Settlements", false);
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000579AC File Offset: 0x00055BAC
		public void InitializeCharactersAfterLoad(bool isSavedCampaign)
		{
			if (isSavedCampaign)
			{
				foreach (Hero hero in Campaign.Current.AliveHeroes)
				{
					if (!hero.CharacterObject.IsOriginalCharacter)
					{
						hero.CharacterObject.InitializeHeroCharacterOnAfterLoad();
					}
				}
				foreach (Hero hero2 in Campaign.Current.DeadOrDisabledHeroes)
				{
					if (!hero2.CharacterObject.IsOriginalCharacter)
					{
						hero2.CharacterObject.InitializeHeroCharacterOnAfterLoad();
					}
				}
				List<CharacterObject> list = new List<CharacterObject>();
				foreach (CharacterObject characterObject in Campaign.Current.ObjectManager.GetObjectTypeList<CharacterObject>())
				{
					if (!characterObject.IsReady && !characterObject.IsOriginalCharacter)
					{
						if (characterObject.HeroObject != null)
						{
							characterObject.InitializeHeroCharacterOnAfterLoad();
						}
						else
						{
							Debug.FailedAssert("saved a characterobject but not its heroobject", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SandBoxManager.cs", "InitializeCharactersAfterLoad", 485);
							list.Add(characterObject);
						}
					}
				}
				foreach (CharacterObject characterObject2 in list)
				{
					Campaign.Current.ObjectManager.UnregisterObject(characterObject2);
				}
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00057B4C File Offset: 0x00055D4C
		protected override void OnTick(float dt)
		{
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00057B4E File Offset: 0x00055D4E
		public override void OnBeforeSave()
		{
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00057B50 File Offset: 0x00055D50
		public override void OnAfterSave()
		{
		}
	}
}
