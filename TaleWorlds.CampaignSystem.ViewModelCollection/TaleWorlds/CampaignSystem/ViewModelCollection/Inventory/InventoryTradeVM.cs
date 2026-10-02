using System;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200008E RID: 142
	public class InventoryTradeVM : ViewModel
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000C38 RID: 3128 RVA: 0x000324F0 File Offset: 0x000306F0
		// (remove) Token: 0x06000C39 RID: 3129 RVA: 0x00032524 File Offset: 0x00030724
		public static event Action RemoveZeroCounts;

		// Token: 0x06000C3A RID: 3130 RVA: 0x00032558 File Offset: 0x00030758
		public InventoryTradeVM(InventoryLogic inventoryLogic, ItemRosterElement itemRoster, InventoryLogic.InventorySide side, Action<int, bool> onApplyTransaction)
		{
			this._inventoryLogic = inventoryLogic;
			this._referenceItemRoster = itemRoster;
			this._isPlayerItem = side == InventoryLogic.InventorySide.PlayerInventory;
			this._onApplyTransaction = onApplyTransaction;
			this.PieceLbl = this._pieceLblSingular;
			InventoryLogic inventoryLogic2 = this._inventoryLogic;
			this.IsTrading = inventoryLogic2 != null && inventoryLogic2.IsTrading;
			this.TakeHint = new HintViewModel(GameTexts.FindText("str_take", null), null);
			this.GiveHint = new HintViewModel(GameTexts.FindText("str_give", null), null);
			this.UpdateItemData(itemRoster, side, true);
			this.RefreshValues();
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000325FC File Offset: 0x000307FC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ThisStockLbl = GameTexts.FindText("str_inventory_this_stock", null).ToString();
			this.OtherStockLbl = GameTexts.FindText("str_inventory_total_stock", null).ToString();
			this.AveragePriceLbl = GameTexts.FindText("str_inventory_average_price", null).ToString();
			this._pieceLblSingular = GameTexts.FindText("str_inventory_piece", null).ToString();
			this._pieceLblPlural = GameTexts.FindText("str_inventory_pieces", null).ToString();
			this.ApplyExchangeHint = new HintViewModel(GameTexts.FindText("str_party_apply_exchange", null), null);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00032694 File Offset: 0x00030894
		public void UpdateItemData(ItemRosterElement itemRoster, InventoryLogic.InventorySide side, bool forceUpdate = true)
		{
			if (side != InventoryLogic.InventorySide.OtherInventory && side != InventoryLogic.InventorySide.PlayerInventory)
			{
				return;
			}
			ItemRosterElement? itemRosterElement = new ItemRosterElement?(itemRoster);
			ItemRosterElement? itemRosterElement2 = null;
			if (side == InventoryLogic.InventorySide.PlayerInventory)
			{
				itemRosterElement2 = this.FindItemFromSide(itemRoster.EquipmentElement, InventoryLogic.InventorySide.OtherInventory);
			}
			else if (side == InventoryLogic.InventorySide.OtherInventory)
			{
				itemRosterElement2 = this.FindItemFromSide(itemRoster.EquipmentElement, InventoryLogic.InventorySide.PlayerInventory);
			}
			if (forceUpdate)
			{
				this.InitialThisStock = ((itemRosterElement != null) ? itemRosterElement.GetValueOrDefault().Amount : 0);
				this.InitialOtherStock = ((itemRosterElement2 != null) ? itemRosterElement2.GetValueOrDefault().Amount : 0);
				this.TotalStock = this.InitialThisStock + this.InitialOtherStock;
				this.ThisStock = this.InitialThisStock;
				this.OtherStock = this.InitialOtherStock;
				this.ThisStockUpdated();
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00032756 File Offset: 0x00030956
		private ItemRosterElement? FindItemFromSide(EquipmentElement item, InventoryLogic.InventorySide side)
		{
			return this._inventoryLogic.FindItemFromSide(side, item);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00032768 File Offset: 0x00030968
		private void ThisStockUpdated()
		{
			this.ExecuteApplyTransaction();
			this.OtherStock = this.TotalStock - this.ThisStock;
			this.IsThisStockIncreasable = this.OtherStock > 0;
			this.IsOtherStockIncreasable = this.OtherStock < this.TotalStock;
			this.UpdateProperties();
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000327B8 File Offset: 0x000309B8
		private void UpdateProperties()
		{
			int num = this.ThisStock - this.InitialThisStock;
			bool flag = num >= 0;
			int num2 = (flag ? num : (-num));
			if (num2 == 0)
			{
				this.PieceChange = num2.ToString();
				this.PriceChange = "0";
				this.AveragePrice = "0";
				this.IsExchangeAvailable = false;
			}
			else
			{
				int num3;
				int itemTotalPrice = this._inventoryLogic.GetItemTotalPrice(this._referenceItemRoster, num2, out num3, flag);
				this.PieceChange = (flag ? "+" : "-") + num2;
				this.PriceChange = (flag ? "-" : "+") + itemTotalPrice * num2;
				this.AveragePrice = this.GetAveragePrice(itemTotalPrice, num3, flag);
				this.IsExchangeAvailable = true;
			}
			this.PieceLbl = ((num2 <= 1) ? this._pieceLblSingular : this._pieceLblPlural);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0003289C File Offset: 0x00030A9C
		public string GetAveragePrice(int totalPrice, int lastPrice, bool isBuying)
		{
			InventoryLogic.InventorySide inventorySide = (isBuying ? InventoryLogic.InventorySide.OtherInventory : InventoryLogic.InventorySide.PlayerInventory);
			int costOfItemRosterElement = this._inventoryLogic.GetCostOfItemRosterElement(this._referenceItemRoster, inventorySide);
			if (costOfItemRosterElement == lastPrice)
			{
				return costOfItemRosterElement.ToString();
			}
			if (costOfItemRosterElement < lastPrice)
			{
				return costOfItemRosterElement + " - " + lastPrice;
			}
			return lastPrice + " - " + costOfItemRosterElement;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00032901 File Offset: 0x00030B01
		public void ExecuteIncreaseThisStock()
		{
			if (this.ThisStock < this.TotalStock)
			{
				this.ThisStock++;
			}
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0003291F File Offset: 0x00030B1F
		public void ExecuteIncreaseOtherStock()
		{
			if (this.ThisStock > 0)
			{
				this.ThisStock--;
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00032938 File Offset: 0x00030B38
		public void ExecuteReset()
		{
			this.ThisStock = this.InitialThisStock;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00032948 File Offset: 0x00030B48
		public void ExecuteApplyTransaction()
		{
			int num = this.ThisStock - this.InitialThisStock;
			if (num == 0 || this._onApplyTransaction == null)
			{
				return;
			}
			bool flag = num >= 0;
			int num2 = (flag ? num : (-num));
			bool flag2 = (this._isPlayerItem ? flag : (!flag));
			this._onApplyTransaction(num2, flag2);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0003299D File Offset: 0x00030B9D
		public void ExecuteRemoveZeroCounts()
		{
			Action removeZeroCounts = InventoryTradeVM.RemoveZeroCounts;
			if (removeZeroCounts == null)
			{
				return;
			}
			removeZeroCounts();
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x000329AE File Offset: 0x00030BAE
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x000329B6 File Offset: 0x00030BB6
		[DataSourceProperty]
		public string ThisStockLbl
		{
			get
			{
				return this._thisStockLbl;
			}
			set
			{
				if (value != this._thisStockLbl)
				{
					this._thisStockLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ThisStockLbl");
				}
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x000329D9 File Offset: 0x00030BD9
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x000329E1 File Offset: 0x00030BE1
		[DataSourceProperty]
		public string OtherStockLbl
		{
			get
			{
				return this._otherStockLbl;
			}
			set
			{
				if (value != this._otherStockLbl)
				{
					this._otherStockLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherStockLbl");
				}
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00032A04 File Offset: 0x00030C04
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x00032A0C File Offset: 0x00030C0C
		[DataSourceProperty]
		public string PieceLbl
		{
			get
			{
				return this._pieceLbl;
			}
			set
			{
				if (value != this._pieceLbl)
				{
					this._pieceLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PieceLbl");
				}
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00032A2F File Offset: 0x00030C2F
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x00032A37 File Offset: 0x00030C37
		[DataSourceProperty]
		public string AveragePriceLbl
		{
			get
			{
				return this._averagePriceLbl;
			}
			set
			{
				if (value != this._averagePriceLbl)
				{
					this._averagePriceLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "AveragePriceLbl");
				}
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00032A5A File Offset: 0x00030C5A
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x00032A62 File Offset: 0x00030C62
		[DataSourceProperty]
		public HintViewModel ApplyExchangeHint
		{
			get
			{
				return this._applyExchangeHint;
			}
			set
			{
				if (value != this._applyExchangeHint)
				{
					this._applyExchangeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ApplyExchangeHint");
				}
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00032A80 File Offset: 0x00030C80
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x00032A88 File Offset: 0x00030C88
		[DataSourceProperty]
		public bool IsExchangeAvailable
		{
			get
			{
				return this._isExchangeAvailable;
			}
			set
			{
				if (value != this._isExchangeAvailable)
				{
					this._isExchangeAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsExchangeAvailable");
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00032AA6 File Offset: 0x00030CA6
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x00032AAE File Offset: 0x00030CAE
		[DataSourceProperty]
		public string PriceChange
		{
			get
			{
				return this._priceChange;
			}
			set
			{
				if (value != this._priceChange)
				{
					this._priceChange = value;
					base.OnPropertyChangedWithValue<string>(value, "PriceChange");
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x00032AD1 File Offset: 0x00030CD1
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x00032AD9 File Offset: 0x00030CD9
		[DataSourceProperty]
		public string PieceChange
		{
			get
			{
				return this._pieceChange;
			}
			set
			{
				if (value != this._pieceChange)
				{
					this._pieceChange = value;
					base.OnPropertyChangedWithValue<string>(value, "PieceChange");
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00032AFC File Offset: 0x00030CFC
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x00032B04 File Offset: 0x00030D04
		[DataSourceProperty]
		public string AveragePrice
		{
			get
			{
				return this._averagePrice;
			}
			set
			{
				if (value != this._averagePrice)
				{
					this._averagePrice = value;
					base.OnPropertyChangedWithValue<string>(value, "AveragePrice");
				}
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00032B27 File Offset: 0x00030D27
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x00032B2F File Offset: 0x00030D2F
		[DataSourceProperty]
		public int ThisStock
		{
			get
			{
				return this._thisStock;
			}
			set
			{
				if (value != this._thisStock)
				{
					this._thisStock = value;
					base.OnPropertyChangedWithValue(value, "ThisStock");
					this.ThisStockUpdated();
				}
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00032B53 File Offset: 0x00030D53
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x00032B5B File Offset: 0x00030D5B
		[DataSourceProperty]
		public int InitialThisStock
		{
			get
			{
				return this._initialThisStock;
			}
			set
			{
				if (value != this._initialThisStock)
				{
					this._initialThisStock = value;
					base.OnPropertyChangedWithValue(value, "InitialThisStock");
				}
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00032B79 File Offset: 0x00030D79
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x00032B81 File Offset: 0x00030D81
		[DataSourceProperty]
		public int OtherStock
		{
			get
			{
				return this._otherStock;
			}
			set
			{
				if (value != this._otherStock)
				{
					this._otherStock = value;
					base.OnPropertyChangedWithValue(value, "OtherStock");
				}
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00032B9F File Offset: 0x00030D9F
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x00032BA7 File Offset: 0x00030DA7
		[DataSourceProperty]
		public int InitialOtherStock
		{
			get
			{
				return this._initialOtherStock;
			}
			set
			{
				if (value != this._initialOtherStock)
				{
					this._initialOtherStock = value;
					base.OnPropertyChangedWithValue(value, "InitialOtherStock");
				}
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00032BC5 File Offset: 0x00030DC5
		// (set) Token: 0x06000C61 RID: 3169 RVA: 0x00032BCD File Offset: 0x00030DCD
		[DataSourceProperty]
		public int TotalStock
		{
			get
			{
				return this._totalStock;
			}
			set
			{
				if (value != this._totalStock)
				{
					this._totalStock = value;
					base.OnPropertyChangedWithValue(value, "TotalStock");
				}
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00032BEB File Offset: 0x00030DEB
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x00032BF3 File Offset: 0x00030DF3
		[DataSourceProperty]
		public bool IsThisStockIncreasable
		{
			get
			{
				return this._isThisStockIncreasable;
			}
			set
			{
				if (value != this._isThisStockIncreasable)
				{
					this._isThisStockIncreasable = value;
					base.OnPropertyChangedWithValue(value, "IsThisStockIncreasable");
				}
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00032C11 File Offset: 0x00030E11
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x00032C19 File Offset: 0x00030E19
		[DataSourceProperty]
		public bool IsOtherStockIncreasable
		{
			get
			{
				return this._isOtherStockIncreasable;
			}
			set
			{
				if (value != this._isOtherStockIncreasable)
				{
					this._isOtherStockIncreasable = value;
					base.OnPropertyChangedWithValue(value, "IsOtherStockIncreasable");
				}
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00032C37 File Offset: 0x00030E37
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x00032C3F File Offset: 0x00030E3F
		[DataSourceProperty]
		public bool IsTrading
		{
			get
			{
				return this._isTrading;
			}
			set
			{
				if (value != this._isTrading)
				{
					this._isTrading = value;
					base.OnPropertyChangedWithValue(value, "IsTrading");
				}
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00032C5D File Offset: 0x00030E5D
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x00032C65 File Offset: 0x00030E65
		[DataSourceProperty]
		public bool IsTradeable
		{
			get
			{
				return this._isTradeable;
			}
			set
			{
				if (value != this._isTradeable)
				{
					this._isTradeable = value;
					base.OnPropertyChangedWithValue(value, "IsTradeable");
				}
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00032C83 File Offset: 0x00030E83
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x00032C8B File Offset: 0x00030E8B
		[DataSourceProperty]
		public HintViewModel TakeHint
		{
			get
			{
				return this._takeHint;
			}
			set
			{
				if (value != this._takeHint)
				{
					this._takeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TakeHint");
				}
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00032CA9 File Offset: 0x00030EA9
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x00032CB1 File Offset: 0x00030EB1
		[DataSourceProperty]
		public HintViewModel GiveHint
		{
			get
			{
				return this._giveHint;
			}
			set
			{
				if (value != this._giveHint)
				{
					this._giveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GiveHint");
				}
			}
		}

		// Token: 0x04000578 RID: 1400
		private InventoryLogic _inventoryLogic;

		// Token: 0x04000579 RID: 1401
		private ItemRosterElement _referenceItemRoster;

		// Token: 0x0400057A RID: 1402
		private Action<int, bool> _onApplyTransaction;

		// Token: 0x0400057B RID: 1403
		private string _pieceLblSingular;

		// Token: 0x0400057C RID: 1404
		private string _pieceLblPlural;

		// Token: 0x0400057D RID: 1405
		private bool _isPlayerItem;

		// Token: 0x0400057E RID: 1406
		private string _thisStockLbl;

		// Token: 0x0400057F RID: 1407
		private string _otherStockLbl;

		// Token: 0x04000580 RID: 1408
		private string _averagePriceLbl;

		// Token: 0x04000581 RID: 1409
		private string _pieceLbl;

		// Token: 0x04000582 RID: 1410
		private HintViewModel _applyExchangeHint;

		// Token: 0x04000583 RID: 1411
		private bool _isExchangeAvailable;

		// Token: 0x04000584 RID: 1412
		private string _averagePrice;

		// Token: 0x04000585 RID: 1413
		private string _pieceChange;

		// Token: 0x04000586 RID: 1414
		private string _priceChange;

		// Token: 0x04000587 RID: 1415
		private int _thisStock = -1;

		// Token: 0x04000588 RID: 1416
		private int _initialThisStock;

		// Token: 0x04000589 RID: 1417
		private int _otherStock = -1;

		// Token: 0x0400058A RID: 1418
		private int _initialOtherStock;

		// Token: 0x0400058B RID: 1419
		private int _totalStock;

		// Token: 0x0400058C RID: 1420
		private bool _isThisStockIncreasable;

		// Token: 0x0400058D RID: 1421
		private bool _isOtherStockIncreasable;

		// Token: 0x0400058E RID: 1422
		private bool _isTrading;

		// Token: 0x0400058F RID: 1423
		private bool _isTradeable;

		// Token: 0x04000590 RID: 1424
		private HintViewModel _takeHint;

		// Token: 0x04000591 RID: 1425
		private HintViewModel _giveHint;
	}
}
