using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AB RID: 171
	public class TownManagementVillageItemVM : ViewModel
	{
		// Token: 0x0600106B RID: 4203 RVA: 0x00042ED8 File Offset: 0x000410D8
		public TownManagementVillageItemVM(Village village)
		{
			this._village = village;
			this.Background = village.Settlement.SettlementComponent.BackgroundMeshName + "_t";
			this.VillageType = (int)this.DetermineVillageType(village.VillageType);
			this.RefreshValues();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00042F2A File Offset: 0x0004112A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._village.Name.ToString();
			this.ProductionName = this._village.VillageType.PrimaryProduction.Name.ToString();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00042F68 File Offset: 0x00041168
		public void ExecuteShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this._village.Settlement });
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00042F8D File Offset: 0x0004118D
		public void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00042F94 File Offset: 0x00041194
		private TownManagementVillageItemVM.VillageTypes DetermineVillageType(VillageType village)
		{
			if (village == DefaultVillageTypes.EuropeHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.EuropeHorseRanch;
			}
			if (village == DefaultVillageTypes.BattanianHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.BattanianHorseRanch;
			}
			if (village == DefaultVillageTypes.SteppeHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.SteppeHorseRanch;
			}
			if (village == DefaultVillageTypes.DesertHorseRanch)
			{
				return TownManagementVillageItemVM.VillageTypes.DesertHorseRanch;
			}
			if (village == DefaultVillageTypes.WheatFarm)
			{
				return TownManagementVillageItemVM.VillageTypes.WheatFarm;
			}
			if (village == DefaultVillageTypes.Lumberjack)
			{
				return TownManagementVillageItemVM.VillageTypes.Lumberjack;
			}
			if (village == DefaultVillageTypes.ClayMine)
			{
				return TownManagementVillageItemVM.VillageTypes.ClayMine;
			}
			if (village == DefaultVillageTypes.SaltMine)
			{
				return TownManagementVillageItemVM.VillageTypes.SaltMine;
			}
			if (village == DefaultVillageTypes.IronMine)
			{
				return TownManagementVillageItemVM.VillageTypes.IronMine;
			}
			if (village == DefaultVillageTypes.Fisherman)
			{
				return TownManagementVillageItemVM.VillageTypes.Fisherman;
			}
			if (village == DefaultVillageTypes.CattleRange)
			{
				return TownManagementVillageItemVM.VillageTypes.CattleRange;
			}
			if (village == DefaultVillageTypes.SheepFarm)
			{
				return TownManagementVillageItemVM.VillageTypes.SheepFarm;
			}
			if (village == DefaultVillageTypes.VineYard)
			{
				return TownManagementVillageItemVM.VillageTypes.VineYard;
			}
			if (village == DefaultVillageTypes.FlaxPlant)
			{
				return TownManagementVillageItemVM.VillageTypes.FlaxPlant;
			}
			if (village == DefaultVillageTypes.DateFarm)
			{
				return TownManagementVillageItemVM.VillageTypes.DateFarm;
			}
			if (village == DefaultVillageTypes.OliveTrees)
			{
				return TownManagementVillageItemVM.VillageTypes.OliveTrees;
			}
			if (village == DefaultVillageTypes.SilkPlant)
			{
				return TownManagementVillageItemVM.VillageTypes.SilkPlant;
			}
			if (village == DefaultVillageTypes.SilverMine)
			{
				return TownManagementVillageItemVM.VillageTypes.SilverMine;
			}
			return TownManagementVillageItemVM.VillageTypes.None;
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00043060 File Offset: 0x00041260
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x00043068 File Offset: 0x00041268
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0004308B File Offset: 0x0004128B
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x00043093 File Offset: 0x00041293
		[DataSourceProperty]
		public string ProductionName
		{
			get
			{
				return this._productionName;
			}
			set
			{
				if (value != this._productionName)
				{
					this._productionName = value;
					base.OnPropertyChangedWithValue<string>(value, "ProductionName");
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x000430B6 File Offset: 0x000412B6
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x000430BE File Offset: 0x000412BE
		[DataSourceProperty]
		public string Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChangedWithValue<string>(value, "Background");
				}
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x000430E1 File Offset: 0x000412E1
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x000430E9 File Offset: 0x000412E9
		[DataSourceProperty]
		public int VillageType
		{
			get
			{
				return this._villageType;
			}
			set
			{
				if (value != this._villageType)
				{
					this._villageType = value;
					base.OnPropertyChangedWithValue(value, "VillageType");
				}
			}
		}

		// Token: 0x04000782 RID: 1922
		private readonly Village _village;

		// Token: 0x04000783 RID: 1923
		private string _name;

		// Token: 0x04000784 RID: 1924
		private string _background;

		// Token: 0x04000785 RID: 1925
		private string _productionName;

		// Token: 0x04000786 RID: 1926
		private int _villageType;

		// Token: 0x0200021B RID: 539
		private enum VillageTypes
		{
			// Token: 0x040011DB RID: 4571
			None,
			// Token: 0x040011DC RID: 4572
			EuropeHorseRanch,
			// Token: 0x040011DD RID: 4573
			BattanianHorseRanch,
			// Token: 0x040011DE RID: 4574
			SteppeHorseRanch,
			// Token: 0x040011DF RID: 4575
			DesertHorseRanch,
			// Token: 0x040011E0 RID: 4576
			WheatFarm,
			// Token: 0x040011E1 RID: 4577
			Lumberjack,
			// Token: 0x040011E2 RID: 4578
			ClayMine,
			// Token: 0x040011E3 RID: 4579
			SaltMine,
			// Token: 0x040011E4 RID: 4580
			IronMine,
			// Token: 0x040011E5 RID: 4581
			Fisherman,
			// Token: 0x040011E6 RID: 4582
			CattleRange,
			// Token: 0x040011E7 RID: 4583
			SheepFarm,
			// Token: 0x040011E8 RID: 4584
			VineYard,
			// Token: 0x040011E9 RID: 4585
			FlaxPlant,
			// Token: 0x040011EA RID: 4586
			DateFarm,
			// Token: 0x040011EB RID: 4587
			OliveTrees,
			// Token: 0x040011EC RID: 4588
			SilkPlant,
			// Token: 0x040011ED RID: 4589
			SilverMine
		}
	}
}
