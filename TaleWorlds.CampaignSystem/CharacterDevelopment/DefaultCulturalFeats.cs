using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003A7 RID: 935
	public class DefaultCulturalFeats
	{
		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x0600360B RID: 13835 RVA: 0x000DB0B8 File Offset: 0x000D92B8
		private static DefaultCulturalFeats Instance
		{
			get
			{
				return Campaign.Current.DefaultFeats;
			}
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000DB0C4 File Offset: 0x000D92C4
		public DefaultCulturalFeats()
		{
			this.RegisterAll();
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000DB0D4 File Offset: 0x000D92D4
		private void RegisterAll()
		{
			this._aseraiTraderFeat = this.Create("aserai_cheaper_caravans");
			this._aseraiDesertSpeedFeat = this.Create("aserai_desert_speed");
			this._aseraiWageFeat = this.Create("aserai_increased_wages");
			this._battaniaForestSpeedFeat = this.Create("battanian_forest_speed");
			this._battaniaMilitiaFeat = this.Create("battanian_militia_production");
			this._battaniaConstructionFeat = this.Create("battanian_slower_construction");
			this._empireGarrisonWageFeat = this.Create("empire_decreased_garrison_wage");
			this._empireArmyInfluenceFeat = this.Create("empire_army_influence");
			this._empireVillageHearthFeat = this.Create("empire_slower_hearth_production");
			this._khuzaitCheaperRecruitsFeat = this.Create("khuzait_cheaper_recruits_mounted");
			this._khuzaitAnimalProductionFeat = this.Create("khuzait_increased_animal_production");
			this._khuzaitDecreasedTaxFeat = this.Create("khuzait_decreased_town_tax");
			this._sturgianGrainProductionFeat = this.Create("sturgian_increased_grain_production");
			this._sturgianArmyInfluenceCostFeat = this.Create("sturgian_decreased_army_influence_cost");
			this._sturgianDecisionPenaltyFeat = this.Create("sturgian_increased_decision_penalty");
			this._vlandianRenownIncomeFeat = this.Create("vlandian_renown_mercenary_income");
			this._vlandianVillageProductionFeat = this.Create("vlandian_villages_production_bonus");
			this._vlandianArmyInfluenceCostFeat = this.Create("vlandian_increased_army_influence_cost");
			this.InitializeAll();
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000DB219 File Offset: 0x000D9419
		private FeatObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<FeatObject>(new FeatObject(stringId));
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000DB230 File Offset: 0x000D9430
		private void InitializeAll()
		{
			this._aseraiTraderFeat.Initialize("{=!}aserai_cheaper_caravans", "{=7kGGgkro}Caravans are 30% cheaper to build. 10% less trade penalty.", 0.7f, true, FeatObject.AdditionType.AddFactor);
			this._aseraiDesertSpeedFeat.Initialize("{=!}aserai_desert_speed", "{=6aFTN1Nb}No speed penalty on desert.", 1f, true, FeatObject.AdditionType.AddFactor);
			this._aseraiWageFeat.Initialize("{=!}aserai_increased_wages", "{=GacrZ1Jl}Daily wages of troops in the party are increased by 5%.", 0.05f, false, FeatObject.AdditionType.AddFactor);
			this._battaniaForestSpeedFeat.Initialize("{=!}battanian_forest_speed", "{=38W2WloI}50% less speed penalty and 15% sight range bonus in forests.", 0.5f, true, FeatObject.AdditionType.AddFactor);
			this._battaniaMilitiaFeat.Initialize("{=!}battanian_militia_production", "{=HLI5zAMV}Towns owned by Battanian rulers will have +20% chance of militias to spawn as veteran militias.", 0.2f, true, FeatObject.AdditionType.Add);
			this._battaniaConstructionFeat.Initialize("{=!}battanian_slower_construction", "{=ruP9jbSq}10% slower build rate for town projects in settlements.", -0.1f, false, FeatObject.AdditionType.AddFactor);
			this._empireGarrisonWageFeat.Initialize("{=!}empire_decreased_garrison_wage", "{=a2eM0QUb}20% less garrison troop wage.", -0.2f, true, FeatObject.AdditionType.AddFactor);
			this._empireArmyInfluenceFeat.Initialize("{=!}empire_army_influence", "{=xgPNGOa8}Being in army brings 25% more influence.", 0.25f, true, FeatObject.AdditionType.AddFactor);
			this._empireVillageHearthFeat.Initialize("{=!}empire_slower_hearth_production", "{=UWiqIFUb}Village hearths increase 20% less.", -0.2f, false, FeatObject.AdditionType.AddFactor);
			this._khuzaitCheaperRecruitsFeat.Initialize("{=!}khuzait_cheaper_recruits_mounted", "{=JUpZuals}Recruiting and upgrading mounted troops are 10% cheaper.", -0.1f, true, FeatObject.AdditionType.AddFactor);
			this._khuzaitAnimalProductionFeat.Initialize("{=!}khuzait_increased_animal_production", "{=Xaw2CoCG}25% production bonus to horse, mule, cow and sheep in villages owned by Khuzait rulers.", 0.25f, true, FeatObject.AdditionType.AddFactor);
			this._khuzaitDecreasedTaxFeat.Initialize("{=!}khuzait_decreased_town_tax", "{=8PsaGhI8}20% less tax income from towns.", -0.2f, false, FeatObject.AdditionType.AddFactor);
			this._sturgianGrainProductionFeat.Initialize("{=!}sturgian_increased_grain_production", "{=5BabRyaa}Villages grain production is increased by 10%.", 0.1f, true, FeatObject.AdditionType.AddFactor);
			this._sturgianArmyInfluenceCostFeat.Initialize("{=!}sturgian_decreased_army_influence_cost", "{=Lmjm5Q9D}Armies are gathered with 50% less influence.", -0.5f, true, FeatObject.AdditionType.AddFactor);
			this._sturgianDecisionPenaltyFeat.Initialize("{=!}sturgian_increased_decision_penalty", "{=fB7kS9Cx}20% more relationship penalty from kingdom decisions.", 0.2f, false, FeatObject.AdditionType.AddFactor);
			this._vlandianRenownIncomeFeat.Initialize("{=!}vlandian_renown_mercenary_income", "{=ppdrgOL8}5% more renown from the battles, 15% more income while serving as a mercenary.", 0.05f, true, FeatObject.AdditionType.AddFactor);
			this._vlandianVillageProductionFeat.Initialize("{=!}vlandian_villages_production_bonus", "{=3GsZXXOi}10% production bonus to villages that are bound to castles.", 0.1f, true, FeatObject.AdditionType.AddFactor);
			this._vlandianArmyInfluenceCostFeat.Initialize("{=!}vlandian_increased_army_influence_cost", "{=O1XCNeZr}Recruiting lords to armies costs 20% more influence.", 0.2f, false, FeatObject.AdditionType.AddFactor);
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06003610 RID: 13840 RVA: 0x000DB435 File Offset: 0x000D9635
		public static FeatObject AseraiTraderFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiTraderFeat;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003611 RID: 13841 RVA: 0x000DB441 File Offset: 0x000D9641
		public static FeatObject AseraiDesertFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiDesertSpeedFeat;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003612 RID: 13842 RVA: 0x000DB44D File Offset: 0x000D964D
		public static FeatObject AseraiIncreasedWageFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiWageFeat;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003613 RID: 13843 RVA: 0x000DB459 File Offset: 0x000D9659
		public static FeatObject BattanianForestSpeedFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaForestSpeedFeat;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003614 RID: 13844 RVA: 0x000DB465 File Offset: 0x000D9665
		public static FeatObject BattanianMilitiaFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaMilitiaFeat;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x000DB471 File Offset: 0x000D9671
		public static FeatObject BattanianConstructionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaConstructionFeat;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003616 RID: 13846 RVA: 0x000DB47D File Offset: 0x000D967D
		public static FeatObject EmpireGarrisonWageFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireGarrisonWageFeat;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003617 RID: 13847 RVA: 0x000DB489 File Offset: 0x000D9689
		public static FeatObject EmpireArmyInfluenceFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireArmyInfluenceFeat;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003618 RID: 13848 RVA: 0x000DB495 File Offset: 0x000D9695
		public static FeatObject EmpireVillageHearthFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireVillageHearthFeat;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003619 RID: 13849 RVA: 0x000DB4A1 File Offset: 0x000D96A1
		public static FeatObject KhuzaitRecruitUpgradeFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitCheaperRecruitsFeat;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x0600361A RID: 13850 RVA: 0x000DB4AD File Offset: 0x000D96AD
		public static FeatObject KhuzaitAnimalProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitAnimalProductionFeat;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000DB4B9 File Offset: 0x000D96B9
		public static FeatObject KhuzaitDecreasedTaxFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitDecreasedTaxFeat;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x0600361C RID: 13852 RVA: 0x000DB4C5 File Offset: 0x000D96C5
		public static FeatObject SturgianGrainProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianGrainProductionFeat;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x0600361D RID: 13853 RVA: 0x000DB4D1 File Offset: 0x000D96D1
		public static FeatObject SturgianArmyInfluenceCostFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianArmyInfluenceCostFeat;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x0600361E RID: 13854 RVA: 0x000DB4DD File Offset: 0x000D96DD
		public static FeatObject SturgianDecisionPenaltyFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianDecisionPenaltyFeat;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x0600361F RID: 13855 RVA: 0x000DB4E9 File Offset: 0x000D96E9
		public static FeatObject VlandianRenownMercenaryFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianRenownIncomeFeat;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003620 RID: 13856 RVA: 0x000DB4F5 File Offset: 0x000D96F5
		public static FeatObject VlandianCastleVillageProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianVillageProductionFeat;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06003621 RID: 13857 RVA: 0x000DB501 File Offset: 0x000D9701
		public static FeatObject VlandianArmyInfluenceFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianArmyInfluenceCostFeat;
			}
		}

		// Token: 0x04000F67 RID: 3943
		private FeatObject _aseraiTraderFeat;

		// Token: 0x04000F68 RID: 3944
		private FeatObject _aseraiDesertSpeedFeat;

		// Token: 0x04000F69 RID: 3945
		private FeatObject _aseraiWageFeat;

		// Token: 0x04000F6A RID: 3946
		private FeatObject _battaniaForestSpeedFeat;

		// Token: 0x04000F6B RID: 3947
		private FeatObject _battaniaMilitiaFeat;

		// Token: 0x04000F6C RID: 3948
		private FeatObject _battaniaConstructionFeat;

		// Token: 0x04000F6D RID: 3949
		private FeatObject _empireGarrisonWageFeat;

		// Token: 0x04000F6E RID: 3950
		private FeatObject _empireArmyInfluenceFeat;

		// Token: 0x04000F6F RID: 3951
		private FeatObject _empireVillageHearthFeat;

		// Token: 0x04000F70 RID: 3952
		private FeatObject _khuzaitCheaperRecruitsFeat;

		// Token: 0x04000F71 RID: 3953
		private FeatObject _khuzaitAnimalProductionFeat;

		// Token: 0x04000F72 RID: 3954
		private FeatObject _khuzaitDecreasedTaxFeat;

		// Token: 0x04000F73 RID: 3955
		private FeatObject _sturgianGrainProductionFeat;

		// Token: 0x04000F74 RID: 3956
		private FeatObject _sturgianArmyInfluenceCostFeat;

		// Token: 0x04000F75 RID: 3957
		private FeatObject _sturgianDecisionPenaltyFeat;

		// Token: 0x04000F76 RID: 3958
		private FeatObject _vlandianRenownIncomeFeat;

		// Token: 0x04000F77 RID: 3959
		private FeatObject _vlandianVillageProductionFeat;

		// Token: 0x04000F78 RID: 3960
		private FeatObject _vlandianArmyInfluenceCostFeat;
	}
}
