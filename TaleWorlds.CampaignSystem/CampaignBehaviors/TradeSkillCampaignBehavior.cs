using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044A RID: 1098
	public class TradeSkillCampaignBehavior : CampaignBehaviorBase, IPlayerTradeBehavior
	{
		// Token: 0x0600469C RID: 18076 RVA: 0x001613C6 File Offset: 0x0015F5C6
		public override void RegisterEvents()
		{
			CampaignEvents.PlayerInventoryExchangeEvent.AddNonSerializedListener(this, new Action<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool>(this.PlayerInventoryUpdated));
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x001613E0 File Offset: 0x0015F5E0
		private void RecordPurchases(ItemRosterElement itemRosterElement, int totalPrice)
		{
			TradeSkillCampaignBehavior.ItemTradeData itemTradeData;
			if (!this.ItemsTradeData.TryGetValue(itemRosterElement.EquipmentElement.Item, out itemTradeData))
			{
				itemTradeData = default(TradeSkillCampaignBehavior.ItemTradeData);
			}
			int num = itemTradeData.NumItemsPurchased + itemRosterElement.Amount;
			float num2 = (itemTradeData.AveragePrice * (float)itemTradeData.NumItemsPurchased + (float)totalPrice) / MathF.Max(0.0001f, (float)num);
			this.ItemsTradeData[itemRosterElement.EquipmentElement.Item] = new TradeSkillCampaignBehavior.ItemTradeData(num2, num);
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00161464 File Offset: 0x0015F664
		private int RecordSales(ItemRosterElement itemRosterElement, int totalPrice, bool isTrading)
		{
			int num = 0;
			TradeSkillCampaignBehavior.ItemTradeData itemTradeData;
			if (this.ItemsTradeData.TryGetValue(itemRosterElement.EquipmentElement.Item, out itemTradeData))
			{
				if (isTrading)
				{
					int num2 = MathF.Min(itemTradeData.NumItemsPurchased, itemRosterElement.Amount);
					int num3 = itemTradeData.NumItemsPurchased - num2;
					float num4 = (float)num2 * itemTradeData.AveragePrice;
					float num5 = (float)totalPrice / MathF.Max(0.001f, (float)itemRosterElement.Amount);
					int num6 = MathF.Round((float)num2 * num5);
					num = MathF.Max(0, num6 - MathF.Floor(num4));
					if (num3 == 0)
					{
						this.ItemsTradeData.Remove(itemRosterElement.EquipmentElement.Item);
					}
					else
					{
						this.ItemsTradeData[itemRosterElement.EquipmentElement.Item] = new TradeSkillCampaignBehavior.ItemTradeData(itemTradeData.AveragePrice, num3);
					}
				}
				else
				{
					int num7 = MobileParty.MainParty.ItemRoster.FindIndexOfElement(itemRosterElement.EquipmentElement);
					if (num7 == -1)
					{
						this.ItemsTradeData.Remove(itemRosterElement.EquipmentElement.Item);
					}
					else
					{
						int amount = MobileParty.MainParty.ItemRoster.GetElementCopyAtIndex(num7).Amount;
						if (itemTradeData.NumItemsPurchased > amount)
						{
							this.ItemsTradeData[itemRosterElement.EquipmentElement.Item] = new TradeSkillCampaignBehavior.ItemTradeData(itemTradeData.AveragePrice, amount);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x001615D0 File Offset: 0x0015F7D0
		private int GetAveragePriceForItem(ItemRosterElement itemRosterElement)
		{
			TradeSkillCampaignBehavior.ItemTradeData itemTradeData;
			if (!this.ItemsTradeData.TryGetValue(itemRosterElement.EquipmentElement.Item, out itemTradeData))
			{
				return 0;
			}
			return MathF.Round(itemTradeData.AveragePrice);
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x00161608 File Offset: 0x0015F808
		private void PlayerInventoryUpdated(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			int num = 0;
			if (isTrading)
			{
				foreach (ValueTuple<ItemRosterElement, int> valueTuple in purchasedItems)
				{
					this.ProcessPurchases(valueTuple.Item1, valueTuple.Item2);
				}
			}
			foreach (ValueTuple<ItemRosterElement, int> valueTuple2 in soldItems)
			{
				num += this.ProcessSales(valueTuple2.Item1, valueTuple2.Item2, isTrading);
			}
			if (isTrading)
			{
				SkillLevelingManager.OnTradeProfitMade(PartyBase.MainParty, num);
				CampaignEventDispatcher.Instance.OnPlayerTradeProfit(num);
			}
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x001616CC File Offset: 0x0015F8CC
		private int ProcessSales(ItemRosterElement itemRosterElement, int totalPrice, bool isTrading)
		{
			if (itemRosterElement.EquipmentElement.ItemModifier == null)
			{
				return this.RecordSales(itemRosterElement, totalPrice, isTrading);
			}
			return 0;
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x001616F8 File Offset: 0x0015F8F8
		private void ProcessPurchases(ItemRosterElement itemRosterElement, int totalPrice)
		{
			if (itemRosterElement.EquipmentElement.ItemModifier == null)
			{
				this.RecordPurchases(itemRosterElement, totalPrice);
			}
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x0016171E File Offset: 0x0015F91E
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData>>("ItemsTradeData", ref this.ItemsTradeData);
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x00161734 File Offset: 0x0015F934
		public int GetProjectedProfit(ItemRosterElement itemRosterElement, int itemCost)
		{
			if (itemRosterElement.EquipmentElement.ItemModifier != null)
			{
				return 0;
			}
			int averagePriceForItem = this.GetAveragePriceForItem(itemRosterElement);
			return itemCost - averagePriceForItem;
		}

		// Token: 0x040013C2 RID: 5058
		private Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData> ItemsTradeData = new Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData>();

		// Token: 0x02000864 RID: 2148
		public class TradeSkillCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600684C RID: 26700 RVA: 0x001CA463 File Offset: 0x001C8663
			public TradeSkillCampaignBehaviorTypeDefiner()
				: base(150794)
			{
			}

			// Token: 0x0600684D RID: 26701 RVA: 0x001CA470 File Offset: 0x001C8670
			protected override void DefineStructTypes()
			{
				base.AddStructDefinition(typeof(TradeSkillCampaignBehavior.ItemTradeData), 10, null);
			}

			// Token: 0x0600684E RID: 26702 RVA: 0x001CA485 File Offset: 0x001C8685
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData>));
			}
		}

		// Token: 0x02000865 RID: 2149
		internal struct ItemTradeData
		{
			// Token: 0x0600684F RID: 26703 RVA: 0x001CA497 File Offset: 0x001C8697
			public ItemTradeData(float averagePrice, int numItemsPurchased)
			{
				this.AveragePrice = averagePrice;
				this.NumItemsPurchased = numItemsPurchased;
			}

			// Token: 0x06006850 RID: 26704 RVA: 0x001CA4A8 File Offset: 0x001C86A8
			public static void AutoGeneratedStaticCollectObjectsItemTradeData(object o, List<object> collectedObjects)
			{
				((TradeSkillCampaignBehavior.ItemTradeData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006851 RID: 26705 RVA: 0x001CA4C4 File Offset: 0x001C86C4
			private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x06006852 RID: 26706 RVA: 0x001CA4C6 File Offset: 0x001C86C6
			internal static object AutoGeneratedGetMemberValueAveragePrice(object o)
			{
				return ((TradeSkillCampaignBehavior.ItemTradeData)o).AveragePrice;
			}

			// Token: 0x06006853 RID: 26707 RVA: 0x001CA4D8 File Offset: 0x001C86D8
			internal static object AutoGeneratedGetMemberValueNumItemsPurchased(object o)
			{
				return ((TradeSkillCampaignBehavior.ItemTradeData)o).NumItemsPurchased;
			}

			// Token: 0x04002422 RID: 9250
			[SaveableField(10)]
			public readonly float AveragePrice;

			// Token: 0x04002423 RID: 9251
			[SaveableField(20)]
			public readonly int NumItemsPurchased;
		}
	}
}
