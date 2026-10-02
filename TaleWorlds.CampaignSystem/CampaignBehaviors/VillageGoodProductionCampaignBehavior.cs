using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044E RID: 1102
	public class VillageGoodProductionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004712 RID: 18194 RVA: 0x0016367D File Offset: 0x0016187D
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x001636AD File Offset: 0x001618AD
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x001636B0 File Offset: 0x001618B0
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 1)
			{
				this.DistributeInitialItemsToTowns();
				this.CalculateInitialAccumulatedTaxes();
				foreach (Village village in Village.All)
				{
					float num = MBRandom.RandomFloat * 5f;
					int num2 = 0;
					while ((float)num2 < num)
					{
						this.TickProductions(village.Settlement, false);
						num2++;
					}
				}
			}
			if (i % 20 == 0)
			{
				foreach (Village village2 in Village.All)
				{
					this.TickProductions(village2.Settlement, true);
				}
			}
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x00163780 File Offset: 0x00161980
		private void DailyTickSettlement(Settlement settlement)
		{
			this.TickProductions(settlement, false);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0016378C File Offset: 0x0016198C
		private void DistributeInitialItemsToTowns()
		{
			int num = 25;
			foreach (SettlementComponent settlementComponent in Campaign.Current.AllTowns)
			{
				float num2 = 0f;
				Settlement settlement = settlementComponent.Settlement;
				foreach (Village village in Campaign.Current.AllVillages)
				{
					float num3 = 0f;
					if (village.TradeBound == settlement)
					{
						num3 += 1f;
					}
					else
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, village.Settlement, false, false, MobileParty.NavigationType.Default);
						float num4 = 0.5f * (600f / MathF.Pow(distance, 1.5f));
						if (num4 > 0.5f)
						{
							num4 = 0.5f;
						}
						float distance2 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, village.TradeBound, false, false, MobileParty.NavigationType.Default);
						float num5 = 0.5f * (600f / MathF.Pow(distance2, 1.5f));
						if (num5 > 0.5f)
						{
							num5 = 0.5f;
						}
						num3 = ((village.Settlement.MapFaction == settlement.MapFaction) ? 1f : 0.6f) * 0.5f * ((num4 + num5) / 2f);
					}
					num2 += num3;
				}
				foreach (Village village2 in Campaign.Current.AllVillages)
				{
					float num6 = 0f;
					if (village2.TradeBound == settlement)
					{
						num6 += 1f;
					}
					else
					{
						float distance3 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, village2.Settlement, false, false, MobileParty.NavigationType.Default);
						float num7 = 0.5f * (600f / MathF.Pow(distance3, 1.5f));
						if (num7 > 0.5f)
						{
							num7 = 0.5f;
						}
						float distance4 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, village2.TradeBound, false, false, MobileParty.NavigationType.Default);
						float num8 = 0.5f * (600f / MathF.Pow(distance4, 1.5f));
						if (num8 > 0.5f)
						{
							num8 = 0.5f;
						}
						num6 = ((village2.Settlement.MapFaction == settlement.MapFaction) ? 1f : 0.6f) * 0.5f * ((num7 + num8) / 2f);
					}
					foreach (ValueTuple<ItemObject, float> valueTuple in village2.VillageType.Productions)
					{
						ItemObject item = valueTuple.Item1;
						float item2 = valueTuple.Item2;
						num6 *= 0.12244235f;
						int num9 = MBRandom.RoundRandomized(item2 * num6 * ((float)num * (12f / num2)));
						for (int i = 0; i < num9; i++)
						{
							ItemModifier itemModifier = null;
							EquipmentElement equipmentElement = new EquipmentElement(item, itemModifier, null, false);
							settlement.ItemRoster.AddToCounts(equipmentElement, 1);
						}
					}
				}
			}
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00163B24 File Offset: 0x00161D24
		private void CalculateInitialAccumulatedTaxes()
		{
			foreach (Village village in Village.All)
			{
				float num = 0f;
				foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
				{
					float resultNumber = Campaign.Current.Models.VillageProductionCalculatorModel.CalculateDailyProductionAmount(village, valueTuple.Item1).ResultNumber;
					num += (float)valueTuple.Item1.Value * resultNumber;
				}
				village.TradeTaxAccumulated = (int)(num * (0.6f + 0.3f * MBRandom.RandomFloat) * Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction());
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x00163C28 File Offset: 0x00161E28
		private void TickProductions(Settlement settlement, bool initialProductionForTowns = false)
		{
			Village village = settlement.Village;
			if (village != null && !village.IsDeserted)
			{
				int num = 0;
				for (int i = 0; i < village.Owner.ItemRoster.Count; i++)
				{
					num += village.Owner.ItemRoster[i].Amount;
				}
				int warehouseCapacity = village.GetWarehouseCapacity();
				if ((float)num < (float)warehouseCapacity * 1.5f)
				{
					this.TickGoodProduction(village, initialProductionForTowns);
					this.TickFoodProduction(village, initialProductionForTowns);
				}
			}
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x00163CA4 File Offset: 0x00161EA4
		private void TickGoodProduction(Village village, bool initialProductionForTowns)
		{
			foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
			{
				ItemObject item = valueTuple.Item1;
				int num = MBRandom.RoundRandomized(Campaign.Current.Models.VillageProductionCalculatorModel.CalculateDailyProductionAmount(village, valueTuple.Item1).ResultNumber);
				if (num > 0)
				{
					if (!initialProductionForTowns)
					{
						village.Owner.ItemRoster.AddToCounts(item, num);
						CampaignEventDispatcher.Instance.OnItemProduced(item, village.Owner.Settlement, num);
					}
					else if (village.TradeBound != null)
					{
						village.TradeBound.ItemRoster.AddToCounts(item, num);
					}
				}
			}
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x00163D7C File Offset: 0x00161F7C
		private void TickFoodProduction(Village village, bool initialProductionForTowns)
		{
			int num = MBRandom.RoundRandomized(Campaign.Current.Models.VillageProductionCalculatorModel.CalculateDailyFoodProductionAmount(village));
			for (int i = 0; i < num; i++)
			{
				float num2 = 0f;
				foreach (ItemObject itemObject in Campaign.Current.DefaultVillageTypes.ConsumableRawItems)
				{
					float num3 = 1f / (float)itemObject.Value;
					num2 += num3;
				}
				float num4 = MBRandom.RandomFloat * num2;
				foreach (ItemObject itemObject2 in Campaign.Current.DefaultVillageTypes.ConsumableRawItems)
				{
					float num5 = 1f / (float)itemObject2.Value;
					num4 -= num5;
					if (num4 < 1E-05f)
					{
						if (!initialProductionForTowns)
						{
							village.Owner.ItemRoster.AddToCounts(itemObject2, 1);
							CampaignEventDispatcher.Instance.OnItemProduced(itemObject2, village.Owner.Settlement, 1);
							break;
						}
						if (village.TradeBound != null)
						{
							village.TradeBound.ItemRoster.AddToCounts(itemObject2, 1);
							break;
						}
					}
				}
			}
		}

		// Token: 0x040013EB RID: 5099
		public const float DistributingItemsAtWorldConstant = 1.5f;
	}
}
