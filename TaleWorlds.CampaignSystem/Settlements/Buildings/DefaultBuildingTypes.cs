using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Settlements.Buildings
{
	// Token: 0x020003CD RID: 973
	public class DefaultBuildingTypes
	{
		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x000EDAF1 File Offset: 0x000EBCF1
		private static DefaultBuildingTypes Instance
		{
			get
			{
				return Campaign.Current.DefaultBuildingTypes;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x000EDAFD File Offset: 0x000EBCFD
		public static BuildingType SettlementFortifications
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementFortifications;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x000EDB09 File Offset: 0x000EBD09
		public static BuildingType SettlementBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementBarracks;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003A1E RID: 14878 RVA: 0x000EDB15 File Offset: 0x000EBD15
		public static BuildingType SettlementTrainingFields
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementTrainingFields;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003A1F RID: 14879 RVA: 0x000EDB21 File Offset: 0x000EBD21
		public static BuildingType SettlementGuardHouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementGuardHouse;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x000EDB2D File Offset: 0x000EBD2D
		public static BuildingType SettlementTaxOffice
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementTaxOffice;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003A21 RID: 14881 RVA: 0x000EDB39 File Offset: 0x000EBD39
		public static BuildingType SettlementWarehouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementWarehouse;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x000EDB45 File Offset: 0x000EBD45
		public static BuildingType SettlementMason
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementMason;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003A23 RID: 14883 RVA: 0x000EDB51 File Offset: 0x000EBD51
		public static BuildingType SettlementSiegeWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementSiegeWorkshop;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003A24 RID: 14884 RVA: 0x000EDB5D File Offset: 0x000EBD5D
		public static BuildingType SettlementWaterworks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementWaterworks;
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003A25 RID: 14885 RVA: 0x000EDB69 File Offset: 0x000EBD69
		public static BuildingType SettlementCourthouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementCourthouse;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x000EDB75 File Offset: 0x000EBD75
		public static BuildingType SettlementMarketplace
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementMarketplace;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003A27 RID: 14887 RVA: 0x000EDB81 File Offset: 0x000EBD81
		public static BuildingType SettlementRoadsAndPaths
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementRoadsAndPaths;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x000EDB8D File Offset: 0x000EBD8D
		public static BuildingType CastleFortifications
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleFortifications;
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06003A29 RID: 14889 RVA: 0x000EDB99 File Offset: 0x000EBD99
		public static BuildingType CastleBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleBarracks;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x000EDBA5 File Offset: 0x000EBDA5
		public static BuildingType CastleTrainingFields
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleTrainingFields;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x000EDBB1 File Offset: 0x000EBDB1
		public static BuildingType CastleGuardHouse
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleGuardHouse;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x000EDBBD File Offset: 0x000EBDBD
		public static BuildingType CastleCastallansOffice
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleCastallansOffice;
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003A2D RID: 14893 RVA: 0x000EDBC9 File Offset: 0x000EBDC9
		public static BuildingType CastleSiegeWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleSiegeWorkshop;
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x000EDBD5 File Offset: 0x000EBDD5
		public static BuildingType CastleCraftmansQuarters
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleCraftmansQuarters;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003A2F RID: 14895 RVA: 0x000EDBE1 File Offset: 0x000EBDE1
		public static BuildingType CastleFarmlands
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleFarmlands;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003A30 RID: 14896 RVA: 0x000EDBED File Offset: 0x000EBDED
		public static BuildingType CastleGranary
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleGranary;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003A31 RID: 14897 RVA: 0x000EDBF9 File Offset: 0x000EBDF9
		public static BuildingType CastleMason
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleMason;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003A32 RID: 14898 RVA: 0x000EDC05 File Offset: 0x000EBE05
		public static BuildingType CastleRoadsAndPaths
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleRoadsAndPaths;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003A33 RID: 14899 RVA: 0x000EDC11 File Offset: 0x000EBE11
		public static BuildingType SettlementDailyHousing
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyHousing;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003A34 RID: 14900 RVA: 0x000EDC1D File Offset: 0x000EBE1D
		public static BuildingType SettlementDailyTrainMilitia
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyTrainMilitia;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003A35 RID: 14901 RVA: 0x000EDC29 File Offset: 0x000EBE29
		public static BuildingType SettlementDailyFestivalAndGames
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyFestivalAndGames;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003A36 RID: 14902 RVA: 0x000EDC35 File Offset: 0x000EBE35
		public static BuildingType SettlementDailyIrrigation
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementDailyIrrigation;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003A37 RID: 14903 RVA: 0x000EDC41 File Offset: 0x000EBE41
		public static BuildingType CastleDailySlackenGarrison
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailySlackenGarrison;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003A38 RID: 14904 RVA: 0x000EDC4D File Offset: 0x000EBE4D
		public static BuildingType CastleDailyRaiseTroops
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailyRaiseTroops;
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003A39 RID: 14905 RVA: 0x000EDC59 File Offset: 0x000EBE59
		public static BuildingType CastleDailyDrills
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailyDrills;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x000EDC65 File Offset: 0x000EBE65
		public static BuildingType CastleDailyIrrigation
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleDailyIrrigation;
			}
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x000EDC71 File Offset: 0x000EBE71
		public DefaultBuildingTypes()
		{
			this.RegisterAll();
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x000EDC80 File Offset: 0x000EBE80
		private void RegisterAll()
		{
			this._buildingSettlementFortifications = this.Create("building_settlement_fortifications");
			this._buildingSettlementBarracks = this.Create("building_settlement_barracks");
			this._buildingSettlementTrainingFields = this.Create("building_settlement_training_fields");
			this._buildingSettlementGuardHouse = this.Create("building_settlement_guard_house");
			this._buildingSettlementSiegeWorkshop = this.Create("building_settlement_siege_workshop");
			this._buildingSettlementTaxOffice = this.Create("building_settlement_tax_office");
			this._buildingSettlementMarketplace = this.Create("building_settlement_marketplace");
			this._buildingSettlementWarehouse = this.Create("building_settlement_warehouse");
			this._buildingSettlementMason = this.Create("building_settlement_mason");
			this._buildingSettlementWaterworks = this.Create("building_settlement_waterworks");
			this._buildingSettlementCourthouse = this.Create("building_settlement_courthouse");
			this._buildingSettlementRoadsAndPaths = this.Create("building_settlement_roads_and_paths");
			this._buildingCastleFortifications = this.Create("building_castle_fortifications");
			this._buildingCastleBarracks = this.Create("building_castle_barracks");
			this._buildingCastleTrainingFields = this.Create("building_castle_training_fields");
			this._buildingCastleGuardHouse = this.Create("building_castle_guard_house");
			this._buildingCastleSiegeWorkshop = this.Create("building_castle_siege_workshop");
			this._buildingCastleCastallansOffice = this.Create("building_castle_castallans_office");
			this._buildingCastleGranary = this.Create("building_castle_granary");
			this._buildingCastleCraftmansQuarters = this.Create("building_castle_craftmans_quarters");
			this._buildingCastleFarmlands = this.Create("building_castle_farmlands");
			this._buildingCastleMason = this.Create("building_castle_mason");
			this._buildingCastleRoadsAndPaths = this.Create("building_castle_roads_and_paths");
			this._buildingSettlementDailyHousing = this.Create("building_settlement_daily_housing");
			this._buildingSettlementDailyTrainMilitia = this.Create("building_settlement_daily_train_militia");
			this._buildingSettlementDailyFestivalAndGames = this.Create("building_settlement_daily_festival_and_games");
			this._buildingSettlementDailyIrrigation = this.Create("building_settlement_daily_irrigation");
			this._buildingCastleDailySlackenGarrison = this.Create("building_castle_daily_slacken_garrison");
			this._buildingCastleDailyRaiseTroops = this.Create("building_castle_daily_raise_troops");
			this._buildingCastleDailyDrills = this.Create("building_castle_daily_drills");
			this._buildingCastleDailyIrrigation = this.Create("building_castle_daily_irrigation");
			this.InitializeAll();
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x000EDEA2 File Offset: 0x000EC0A2
		private BuildingType Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BuildingType>(new BuildingType(stringId));
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x000EDEBC File Offset: 0x000EC0BC
		private void InitializeAll()
		{
			this._buildingSettlementFortifications.Initialize(new TextObject("{=CVdK1ax1}Fortifications", null), new TextObject("{=dIM6xa2O}Better fortifications and higher walls around town, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[] { 0, 6000, 12000 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 60f, 90f, 120f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 50f, 75f, 100f)
			}, true, 0f, 1);
			this._buildingSettlementBarracks.Initialize(new TextObject("{=x2B0OjhI}Barracks", null), new TextObject("{=JalrbDBC}Lodgings for garrison troops. Each level increases garrison limit and decreases garrison wage.", null), new int[] { 1800, 3000, 4200 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 60f, 90f, 120f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.05f, -0.1f, -0.15f)
			}, true, 0f, 0);
			this._buildingSettlementTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=NYzORuQm}Provides experience for garrison troops and increases militia veterancy.", null), new int[] { 1500, 2100, 2700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ExperiencePerDay, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.MilitiaVeterancyChance, BuildingEffectIncrementType.Add, 0.1f, 0.15f, 0.2f)
			}, true, 0f, 0);
			this._buildingSettlementGuardHouse.Initialize(new TextObject("{=OHEiwoHC}Guard House", null), new TextObject("{=doojtAwr}Increases prisoner limit and provides a patrol party that improves security.", null), new int[] { 1500, 2100, 2700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PatrolPartyStrength, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 30f, 60f, 90f)
			}, true, 0f, 0);
			this._buildingSettlementSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=MharAceZ}Builds and maintains siege engines for defense of the settlement.", null), new int[] { 1200, 1800, 3000 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.BallistaOnSiegeStart, BuildingEffectIncrementType.Add, 1f, 1f, 2f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.CatapultOnSiegeStart, BuildingEffectIncrementType.Add, 0f, 1f, 1f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, BuildingEffectIncrementType.AddFactor, 0.3f, 0.6f, 1f)
			}, false, 0f, 0);
			this._buildingSettlementTaxOffice.Initialize(new TextObject("{=LG84byW0}Tax Office", null), new TextObject("{=nQ6ytZeF}Increases tax income.", null), new int[] { 1800, 3000, 4200 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.TaxPerDay, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f)
			}, false, 0f, 0);
			this._buildingSettlementMarketplace.Initialize(new TextObject("{=zLdXCpne}Marketplace", null), new TextObject("{=Z0xf3Bbd}Increases the tariff collected from trades made in town", null), new int[] { 2400, 3600, 4800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.TariffIncome, BuildingEffectIncrementType.AddFactor, 0.1f, 0.2f, 0.3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.CaravanAccessibility, BuildingEffectIncrementType.AddFactor, 1.02f, 1.04f, 1.06f)
			}, false, 0f, 0);
			this._buildingSettlementWarehouse.Initialize(new TextObject("{=anTRftmb}Warehouse", null), new TextObject("{=hhKDZJeM}Increases Food storage limits and improves workshop productivity.", null), new int[] { 1800, 2400, 3000 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodStock, BuildingEffectIncrementType.Add, 100f, 300f, 500f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.WorkshopProduction, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f)
			}, false, 0f, 0);
			this._buildingSettlementMason.Initialize(new TextObject("{=R7ssoDHW}Mason", null), new TextObject("{=hqUPvnaj}Increase bricks per day, increasing building and repair speed.", null), new int[] { 2400, 3000, 4800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ConstructionPerDay, BuildingEffectIncrementType.Add, 3f, 6f, 9f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.WallRepairSpeed, BuildingEffectIncrementType.AddFactor, 0.1f, 0.2f, 0.3f)
			}, false, 0f, 0);
			this._buildingSettlementWaterworks.Initialize(new TextObject("{=DA0y7B3S}Waterworks", null), new TextObject("{=SfbwSASh}Waterways and sanitation, decrease food consumption.", null), new int[] { 1800, 3600, 5400 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodConsumption, BuildingEffectIncrementType.AddFactor, -0.05f, -0.1f, -0.15f)
			}, false, 0f, 0);
			this._buildingSettlementCourthouse.Initialize(new TextObject("{=Bw8kAvGY}Courthouse", null), new TextObject("{=tmLJvPlz}Local judges manage disputes and maintain law and order. Provides influence and loyalty per day.", null), new int[] { 2400, 3600, 5400 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Loyalty, BuildingEffectIncrementType.Add, 0.3f, 0.6f, 1f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Influence, BuildingEffectIncrementType.Add, 0.2f, 0.5f, 1f)
			}, false, 0f, 0);
			this._buildingSettlementRoadsAndPaths.Initialize(new TextObject("{=maEmutDP}Roads and Paths", null), new TextObject("{=YPFDiwuy}Increase village production and village hearth growth.", null), new int[] { 2400, 3600, 4800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageProduction, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.Add, 0.1f, 0.2f, 0.3f)
			}, false, 0f, 0);
			this._buildingCastleFortifications.Initialize(new TextObject("{=CVdK1ax1}Fortifications", null), new TextObject("{=oS5Nesmi}Better fortifications and higher walls around the keep, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[] { 0, 1400, 2800 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 50f, 75f, 100f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 30f, 45f, 60f)
			}, true, 0f, 1);
			this._buildingCastleBarracks.Initialize(new TextObject("{=x2B0OjhI}Barracks", null), new TextObject("{=JalrbDBC}Lodgings for garrison troops. Each level increases garrison limit and decreases garrison wage.", null), new int[] { 420, 700, 1120 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonCapacity, BuildingEffectIncrementType.Add, 20f, 40f, 80f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.1f, -0.2f, -0.3f)
			}, true, 0f, 0);
			this._buildingCastleTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=otWlERkc}A field for military drills that increases the daily experience gain of all garrisoned units.", null), new int[] { 420, 560, 700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ExperiencePerDay, BuildingEffectIncrementType.Add, 3f, 4f, 5f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.MilitiaVeterancyChance, BuildingEffectIncrementType.Add, 0.1f, 0.15f, 0.2f)
			}, true, 0f, 0);
			this._buildingCastleGuardHouse.Initialize(new TextObject("{=OHEiwoHC}Guard House", null), new TextObject("{=K0cbj7o3}Increase militia recruitment, and prisoner limit.", null), new int[] { 350, 490, 630 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Militia, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.PrisonCapacity, BuildingEffectIncrementType.Add, 10f, 30f, 50f)
			}, true, 0f, 0);
			this._buildingCastleSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=YRCW0oFd}Builds and maintains siege engines for defense of the settlement.", null), new int[] { 280, 420, 700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.BallistaOnSiegeStart, BuildingEffectIncrementType.Add, 1f, 2f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.CatapultOnSiegeStart, BuildingEffectIncrementType.Add, 0f, 1f, 2f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, BuildingEffectIncrementType.AddFactor, 0.2f, 0.4f, 0.8f)
			}, true, 0f, 0);
			this._buildingCastleCastallansOffice.Initialize(new TextObject("{=kLNnFMR9}Castellan's Office", null), new TextObject("{=GDsI6daq}Increases auto recruitment, and decreases garrison wage.", null), new int[] { 560, 840, 1260 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.1f, -0.2f, -0.3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonAutoRecruitment, BuildingEffectIncrementType.Add, 1f, 2f, 3f)
			}, true, 0f, 0);
			this._buildingCastleGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=iazij7fO}Increases food storage limits.", null), new int[] { 420, 560, 700 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodStock, BuildingEffectIncrementType.Add, 100f, 200f, 300f)
			}, false, 0f, 0);
			this._buildingCastleCraftmansQuarters.Initialize(new TextObject("{=KE1KUayw}Craftmans Quarters", null), new TextObject("{=2qZ14G9p}Provides income based on bound village hearts", null), new int[] { 350, 490, 630 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.DenarByBoundVillageHeartPerDay, BuildingEffectIncrementType.Add, 0.2f, 0.4f, 0.6f)
			}, false, 0f, 0);
			this._buildingCastleFarmlands.Initialize(new TextObject("{=l4eZqegY}Farmlands", null), new TextObject("{=tajCl8Bg}Provides daily food.", null), new int[] { 420, 630, 840 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.FoodProduction, BuildingEffectIncrementType.Add, 6f, 12f, 18f)
			}, false, 0f, 0);
			this._buildingCastleMason.Initialize(new TextObject("{=R7ssoDHW}Mason", null), new TextObject("{=hqUPvnaj}Increase bricks per day, increasing building and repair speed.", null), new int[] { 560, 700, 1120 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ConstructionPerDay, BuildingEffectIncrementType.Add, 2f, 4f, 6f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.WallRepairSpeed, BuildingEffectIncrementType.AddFactor, 0.1f, 0.3f, 0.6f)
			}, false, 0f, 0);
			this._buildingCastleRoadsAndPaths.Initialize(new TextObject("{=maEmutDP}Roads and Paths", null), new TextObject("{=YPFDiwuy}Increase village production and village hearth growth.", null), new int[] { 560, 840, 1120 }, new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageProduction, BuildingEffectIncrementType.AddFactor, 0.05f, 0.1f, 0.15f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.Add, 0.1f, 0.2f, 0.3f)
			}, false, 0f, 0);
			this._buildingSettlementDailyHousing.InitializeDailyProject(new TextObject("{=F4V7oaVx}Housing", null), new TextObject("{=yWXtcxqb}Construct housing so that more folks can settle, increasing population.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Prosperity, BuildingEffectIncrementType.Add, 2f, 2f, 2f)
			});
			this._buildingSettlementDailyTrainMilitia.InitializeDailyProject(new TextObject("{=p1Y3EU5O}Train Militia", null), new TextObject("{=61J1wa6k}Schedule drills for commoners, increasing militia recruitment.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Militia, BuildingEffectIncrementType.Add, 2f, 2f, 2f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonAutoRecruitment, BuildingEffectIncrementType.Add, 1f, 1f, 1f)
			});
			this._buildingSettlementDailyFestivalAndGames.InitializeDailyProject(new TextObject("{=aEmYZadz}Festival and Games", null), new TextObject("{=ovDbQIo9}Organize festivals and games in the settlement, increasing loyalty.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Loyalty, BuildingEffectIncrementType.Add, 3f, 3f, 3f)
			});
			this._buildingSettlementDailyIrrigation.InitializeDailyProject(new TextObject("{=O4cknzhW}Irrigation", null), new TextObject("{=CU9g49fo}Provide irrigation, increasing hearth growth in bound villages.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.Add, 1f, 1f, 1f)
			});
			this._buildingCastleDailySlackenGarrison.InitializeDailyProject(new TextObject("{=cHIa0Xty}Slacken Garrison", null), new TextObject("{=5VBbLVBt}Decrease garrison wages.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonWageReduction, BuildingEffectIncrementType.AddFactor, -0.05f, -0.05f, -0.05f)
			});
			this._buildingCastleDailyRaiseTroops.InitializeDailyProject(new TextObject("{=jm1ScaoK}Raise Troops", null), new TextObject("{=UsHhePdk}Increase militia recruitment, and auto recruitment.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.Militia, BuildingEffectIncrementType.Add, 3f, 3f, 3f),
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.GarrisonAutoRecruitment, BuildingEffectIncrementType.Add, 2f, 2f, 2f)
			});
			this._buildingCastleDailyDrills.InitializeDailyProject(new TextObject("{=JpiQagYa}Drills", null), new TextObject("{=e9V1W7nW}Provides experience to garrison.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.ExperiencePerDay, BuildingEffectIncrementType.Add, 8f, 8f, 8f)
			});
			this._buildingCastleDailyIrrigation.InitializeDailyProject(new TextObject("{=O4cknzhW}Irrigation", null), new TextObject("{=CU9g49fo}Provide irrigation, increasing hearth growth in bound villages.", null), new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, BuildingEffectIncrementType, float, float, float>(BuildingEffectEnum.VillageHeartsPerDay, BuildingEffectIncrementType.AddFactor, 0.5f, 0.5f, 0.5f)
			});
		}

		// Token: 0x04001216 RID: 4630
		public const int MaxBuildingLevel = 3;

		// Token: 0x04001217 RID: 4631
		private BuildingType _buildingSettlementFortifications;

		// Token: 0x04001218 RID: 4632
		private BuildingType _buildingSettlementMarketplace;

		// Token: 0x04001219 RID: 4633
		private BuildingType _buildingSettlementTrainingFields;

		// Token: 0x0400121A RID: 4634
		private BuildingType _buildingSettlementBarracks;

		// Token: 0x0400121B RID: 4635
		private BuildingType _buildingSettlementSiegeWorkshop;

		// Token: 0x0400121C RID: 4636
		private BuildingType _buildingSettlementGuardHouse;

		// Token: 0x0400121D RID: 4637
		private BuildingType _buildingSettlementTaxOffice;

		// Token: 0x0400121E RID: 4638
		private BuildingType _buildingSettlementWarehouse;

		// Token: 0x0400121F RID: 4639
		private BuildingType _buildingSettlementMason;

		// Token: 0x04001220 RID: 4640
		private BuildingType _buildingSettlementCourthouse;

		// Token: 0x04001221 RID: 4641
		private BuildingType _buildingSettlementWaterworks;

		// Token: 0x04001222 RID: 4642
		private BuildingType _buildingSettlementRoadsAndPaths;

		// Token: 0x04001223 RID: 4643
		private BuildingType _buildingCastleFortifications;

		// Token: 0x04001224 RID: 4644
		private BuildingType _buildingCastleBarracks;

		// Token: 0x04001225 RID: 4645
		private BuildingType _buildingCastleTrainingFields;

		// Token: 0x04001226 RID: 4646
		private BuildingType _buildingCastleGranary;

		// Token: 0x04001227 RID: 4647
		private BuildingType _buildingCastleGuardHouse;

		// Token: 0x04001228 RID: 4648
		private BuildingType _buildingCastleCastallansOffice;

		// Token: 0x04001229 RID: 4649
		private BuildingType _buildingCastleSiegeWorkshop;

		// Token: 0x0400122A RID: 4650
		private BuildingType _buildingCastleCraftmansQuarters;

		// Token: 0x0400122B RID: 4651
		private BuildingType _buildingCastleFarmlands;

		// Token: 0x0400122C RID: 4652
		private BuildingType _buildingSettlementDailyHousing;

		// Token: 0x0400122D RID: 4653
		private BuildingType _buildingCastleMason;

		// Token: 0x0400122E RID: 4654
		private BuildingType _buildingCastleRoadsAndPaths;

		// Token: 0x0400122F RID: 4655
		private BuildingType _buildingSettlementDailyIrrigation;

		// Token: 0x04001230 RID: 4656
		private BuildingType _buildingSettlementDailyTrainMilitia;

		// Token: 0x04001231 RID: 4657
		private BuildingType _buildingCastleDailySlackenGarrison;

		// Token: 0x04001232 RID: 4658
		private BuildingType _buildingSettlementDailyFestivalAndGames;

		// Token: 0x04001233 RID: 4659
		private BuildingType _buildingCastleDailyRaiseTroops;

		// Token: 0x04001234 RID: 4660
		private BuildingType _buildingCastleDailyDrills;

		// Token: 0x04001235 RID: 4661
		private BuildingType _buildingCastleDailyIrrigation;
	}
}
