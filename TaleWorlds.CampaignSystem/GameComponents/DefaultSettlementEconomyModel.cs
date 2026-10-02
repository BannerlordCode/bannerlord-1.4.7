using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014A RID: 330
	public class DefaultSettlementEconomyModel : SettlementEconomyModel
	{
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00082D02 File Offset: 0x00080F02
		private DefaultSettlementEconomyModel.CategoryValues CategoryValuesCache
		{
			get
			{
				if (this._categoryValues == null)
				{
					this._categoryValues = new DefaultSettlementEconomyModel.CategoryValues();
				}
				return this._categoryValues;
			}
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00082D20 File Offset: 0x00080F20
		public override ValueTuple<float, float> GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand)
		{
			float num = oldSupply * 0.85f + dailySupply * 0.15f;
			float num2 = oldDemand * 0.85f + dailyDemand * 0.15f;
			num = MathF.Max(0.1f, num);
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x00082D64 File Offset: 0x00080F64
		public override float GetDailyDemandForCategory(Town town, ItemCategory category, int extraProsperity)
		{
			float num = MathF.Max(0f, town.Prosperity + (float)extraProsperity);
			float num2 = MathF.Max(0f, town.Prosperity - 3000f);
			float num3 = category.BaseDemand * num;
			float num4 = category.LuxuryDemand * num2;
			float num5 = num3 + num4;
			if (category.BaseDemand < 1E-08f)
			{
				num5 = num * 0.01f;
			}
			return num5;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x00082DC8 File Offset: 0x00080FC8
		public override int GetTownGoldChange(Town town)
		{
			float num = 10000f + town.Prosperity * 12f - (float)town.Gold;
			return MathF.Round(0.25f * num);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00082DFC File Offset: 0x00080FFC
		public override float CalculateDailySettlementBudgetForItemCategory(Town town, float demand, ItemCategory category)
		{
			return demand * MathF.Pow(town.GetItemCategoryPriceIndex(category), 0.3f);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00082E11 File Offset: 0x00081011
		public override float GetDemandChangeFromValue(float purchaseValue)
		{
			return purchaseValue * 0.15f;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00082E1A File Offset: 0x0008101A
		public override float GetEstimatedDemandForCategory(Town town, ItemData itemData, ItemCategory category)
		{
			return Campaign.Current.Models.SettlementEconomyModel.GetDailyDemandForCategory(town, category, 1000);
		}

		// Token: 0x0400088E RID: 2190
		private DefaultSettlementEconomyModel.CategoryValues _categoryValues;

		// Token: 0x0400088F RID: 2191
		private const int ProsperityLuxuryTreshold = 3000;

		// Token: 0x04000890 RID: 2192
		private const float dailyChangeFactor = 0.15f;

		// Token: 0x04000891 RID: 2193
		private const float oneMinusDailyChangeFactor = 0.85f;

		// Token: 0x020005A0 RID: 1440
		private class CategoryValues
		{
			// Token: 0x06004E7C RID: 20092 RVA: 0x00181C90 File Offset: 0x0017FE90
			public CategoryValues()
			{
				this.PriceDict = new Dictionary<ItemCategory, int>();
				foreach (ItemObject itemObject in Items.All)
				{
					this.PriceDict[itemObject.GetItemCategory()] = itemObject.Value;
				}
			}

			// Token: 0x06004E7D RID: 20093 RVA: 0x00181D04 File Offset: 0x0017FF04
			public int GetValueOfCategory(ItemCategory category)
			{
				int num = 1;
				this.PriceDict.TryGetValue(category, out num);
				return num;
			}

			// Token: 0x040017EC RID: 6124
			public Dictionary<ItemCategory, int> PriceDict;
		}
	}
}
