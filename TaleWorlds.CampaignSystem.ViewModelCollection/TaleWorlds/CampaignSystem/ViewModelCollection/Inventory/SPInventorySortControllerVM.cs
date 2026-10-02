using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000093 RID: 147
	public class SPInventorySortControllerVM : ViewModel
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000365F2 File Offset: 0x000347F2
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x000365FA File Offset: 0x000347FA
		public SPInventorySortControllerVM.InventoryItemSortOption? CurrentSortOption { get; private set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00036603 File Offset: 0x00034803
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x0003660B File Offset: 0x0003480B
		public SPInventorySortControllerVM.InventoryItemSortState? CurrentSortState { get; private set; }

		// Token: 0x06000CDB RID: 3291 RVA: 0x00036614 File Offset: 0x00034814
		public SPInventorySortControllerVM(ref MBBindingList<SPItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._typeComparer = new SPInventorySortControllerVM.ItemTypeComparer();
			this._nameComparer = new SPInventorySortControllerVM.ItemNameComparer();
			this._quantityComparer = new SPInventorySortControllerVM.ItemQuantityComparer();
			this._costComparer = new SPInventorySortControllerVM.ItemCostComparer();
			this.RefreshValues();
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00036661 File Offset: 0x00034861
		public void SortByOption(SPInventorySortControllerVM.InventoryItemSortOption sortOption, SPInventorySortControllerVM.InventoryItemSortState sortState)
		{
			this.SetAllStates((sortState == SPInventorySortControllerVM.InventoryItemSortState.Ascending) ? SPInventorySortControllerVM.InventoryItemSortState.Descending : SPInventorySortControllerVM.InventoryItemSortState.Ascending);
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Type)
			{
				this.ExecuteSortByType();
				return;
			}
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Name)
			{
				this.ExecuteSortByName();
				return;
			}
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Quantity)
			{
				this.ExecuteSortByQuantity();
				return;
			}
			if (sortOption == SPInventorySortControllerVM.InventoryItemSortOption.Cost)
			{
				this.ExecuteSortByCost();
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003669B File Offset: 0x0003489B
		public void SortByDefaultState()
		{
			this.ExecuteSortByType();
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000366A4 File Offset: 0x000348A4
		public void SortByCurrentState()
		{
			if (this.IsTypeSelected)
			{
				this._listToControl.Sort(this._typeComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Type);
				return;
			}
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Name);
				return;
			}
			if (this.IsQuantitySelected)
			{
				this._listToControl.Sort(this._quantityComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Quantity);
				return;
			}
			if (this.IsCostSelected)
			{
				this._listToControl.Sort(this._costComparer);
				this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Cost);
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00036748 File Offset: 0x00034948
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.NameState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Name);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000367D4 File Offset: 0x000349D4
		public void ExecuteSortByType()
		{
			int typeState = this.TypeState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.TypeState = (typeState + 1) % 3;
			if (this.TypeState == 0)
			{
				this.TypeState++;
			}
			this._typeComparer.SetSortMode(this.TypeState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.TypeState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._typeComparer);
			this.IsTypeSelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Type);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00036860 File Offset: 0x00034A60
		public void ExecuteSortByQuantity()
		{
			int quantityState = this.QuantityState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.QuantityState = (quantityState + 1) % 3;
			if (this.QuantityState == 0)
			{
				this.QuantityState++;
			}
			this._quantityComparer.SetSortMode(this.QuantityState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.QuantityState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._quantityComparer);
			this.IsQuantitySelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Quantity);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000368EC File Offset: 0x00034AEC
		public void ExecuteSortByCost()
		{
			int costState = this.CostState;
			this.SetAllStates(SPInventorySortControllerVM.InventoryItemSortState.Default);
			this.CostState = (costState + 1) % 3;
			if (this.CostState == 0)
			{
				this.CostState++;
			}
			this._costComparer.SetSortMode(this.CostState == 1);
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?((this.CostState == 1) ? SPInventorySortControllerVM.InventoryItemSortState.Ascending : SPInventorySortControllerVM.InventoryItemSortState.Descending);
			this._listToControl.Sort(this._costComparer);
			this.IsCostSelected = true;
			this.CurrentSortOption = new SPInventorySortControllerVM.InventoryItemSortOption?(SPInventorySortControllerVM.InventoryItemSortOption.Cost);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00036978 File Offset: 0x00034B78
		private void SetAllStates(SPInventorySortControllerVM.InventoryItemSortState state)
		{
			this.TypeState = (int)state;
			this.NameState = (int)state;
			this.QuantityState = (int)state;
			this.CostState = (int)state;
			this.IsTypeSelected = false;
			this.IsNameSelected = false;
			this.IsQuantitySelected = false;
			this.IsCostSelected = false;
			this.CurrentSortState = new SPInventorySortControllerVM.InventoryItemSortState?(state);
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x000369C9 File Offset: 0x00034BC9
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x000369D1 File Offset: 0x00034BD1
		[DataSourceProperty]
		public int TypeState
		{
			get
			{
				return this._typeState;
			}
			set
			{
				if (value != this._typeState)
				{
					this._typeState = value;
					base.OnPropertyChangedWithValue(value, "TypeState");
				}
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000369EF File Offset: 0x00034BEF
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x000369F7 File Offset: 0x00034BF7
		[DataSourceProperty]
		public int NameState
		{
			get
			{
				return this._nameState;
			}
			set
			{
				if (value != this._nameState)
				{
					this._nameState = value;
					base.OnPropertyChangedWithValue(value, "NameState");
				}
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00036A15 File Offset: 0x00034C15
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x00036A1D File Offset: 0x00034C1D
		[DataSourceProperty]
		public int QuantityState
		{
			get
			{
				return this._quantityState;
			}
			set
			{
				if (value != this._quantityState)
				{
					this._quantityState = value;
					base.OnPropertyChangedWithValue(value, "QuantityState");
				}
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00036A3B File Offset: 0x00034C3B
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x00036A43 File Offset: 0x00034C43
		[DataSourceProperty]
		public int CostState
		{
			get
			{
				return this._costState;
			}
			set
			{
				if (value != this._costState)
				{
					this._costState = value;
					base.OnPropertyChangedWithValue(value, "CostState");
				}
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00036A61 File Offset: 0x00034C61
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00036A69 File Offset: 0x00034C69
		[DataSourceProperty]
		public bool IsTypeSelected
		{
			get
			{
				return this._isTypeSelected;
			}
			set
			{
				if (value != this._isTypeSelected)
				{
					this._isTypeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsTypeSelected");
				}
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00036A87 File Offset: 0x00034C87
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x00036A8F File Offset: 0x00034C8F
		[DataSourceProperty]
		public bool IsNameSelected
		{
			get
			{
				return this._isNameSelected;
			}
			set
			{
				if (value != this._isNameSelected)
				{
					this._isNameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsNameSelected");
				}
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00036AAD File Offset: 0x00034CAD
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00036AB5 File Offset: 0x00034CB5
		[DataSourceProperty]
		public bool IsQuantitySelected
		{
			get
			{
				return this._isQuantitySelected;
			}
			set
			{
				if (value != this._isQuantitySelected)
				{
					this._isQuantitySelected = value;
					base.OnPropertyChangedWithValue(value, "IsQuantitySelected");
				}
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00036AD3 File Offset: 0x00034CD3
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00036ADB File Offset: 0x00034CDB
		[DataSourceProperty]
		public bool IsCostSelected
		{
			get
			{
				return this._isCostSelected;
			}
			set
			{
				if (value != this._isCostSelected)
				{
					this._isCostSelected = value;
					base.OnPropertyChangedWithValue(value, "IsCostSelected");
				}
			}
		}

		// Token: 0x040005DC RID: 1500
		private MBBindingList<SPItemVM> _listToControl;

		// Token: 0x040005DD RID: 1501
		private SPInventorySortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x040005DE RID: 1502
		private SPInventorySortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040005DF RID: 1503
		private SPInventorySortControllerVM.ItemQuantityComparer _quantityComparer;

		// Token: 0x040005E0 RID: 1504
		private SPInventorySortControllerVM.ItemCostComparer _costComparer;

		// Token: 0x040005E3 RID: 1507
		private int _typeState;

		// Token: 0x040005E4 RID: 1508
		private int _nameState;

		// Token: 0x040005E5 RID: 1509
		private int _quantityState;

		// Token: 0x040005E6 RID: 1510
		private int _costState;

		// Token: 0x040005E7 RID: 1511
		private bool _isTypeSelected;

		// Token: 0x040005E8 RID: 1512
		private bool _isNameSelected;

		// Token: 0x040005E9 RID: 1513
		private bool _isQuantitySelected;

		// Token: 0x040005EA RID: 1514
		private bool _isCostSelected;

		// Token: 0x020001FA RID: 506
		public enum InventoryItemSortState
		{
			// Token: 0x04001172 RID: 4466
			Default,
			// Token: 0x04001173 RID: 4467
			Ascending,
			// Token: 0x04001174 RID: 4468
			Descending
		}

		// Token: 0x020001FB RID: 507
		public enum InventoryItemSortOption
		{
			// Token: 0x04001176 RID: 4470
			Type,
			// Token: 0x04001177 RID: 4471
			Name,
			// Token: 0x04001178 RID: 4472
			Quantity,
			// Token: 0x04001179 RID: 4473
			Cost
		}

		// Token: 0x020001FC RID: 508
		public abstract class ItemComparer : IComparer<SPItemVM>
		{
			// Token: 0x0600241F RID: 9247 RVA: 0x0007FBA7 File Offset: 0x0007DDA7
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06002420 RID: 9248
			public abstract int Compare(SPItemVM x, SPItemVM y);

			// Token: 0x06002421 RID: 9249 RVA: 0x0007FBB0 File Offset: 0x0007DDB0
			protected int ResolveEquality(SPItemVM x, SPItemVM y)
			{
				return x.ItemDescription.CompareTo(y.ItemDescription);
			}

			// Token: 0x0400117A RID: 4474
			protected bool _isAscending;
		}

		// Token: 0x020001FD RID: 509
		public class ItemTypeComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002423 RID: 9251 RVA: 0x0007FBCC File Offset: 0x0007DDCC
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				int itemObjectTypeSortIndex = CampaignUIHelper.GetItemObjectTypeSortIndex(x.ItemRosterElement.EquipmentElement.Item);
				int num = CampaignUIHelper.GetItemObjectTypeSortIndex(y.ItemRosterElement.EquipmentElement.Item).CompareTo(itemObjectTypeSortIndex);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				num = x.ItemCost.CompareTo(y.ItemCost);
				if (num != 0)
				{
					return num;
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001FE RID: 510
		public class ItemNameComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002425 RID: 9253 RVA: 0x0007FC51 File Offset: 0x0007DE51
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				if (this._isAscending)
				{
					return y.ItemDescription.CompareTo(x.ItemDescription) * -1;
				}
				return y.ItemDescription.CompareTo(x.ItemDescription);
			}
		}

		// Token: 0x020001FF RID: 511
		public class ItemQuantityComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002427 RID: 9255 RVA: 0x0007FC88 File Offset: 0x0007DE88
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				int num = y.ItemCount.CompareTo(x.ItemCount);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x02000200 RID: 512
		public class ItemCostComparer : SPInventorySortControllerVM.ItemComparer
		{
			// Token: 0x06002429 RID: 9257 RVA: 0x0007FCCC File Offset: 0x0007DECC
			public override int Compare(SPItemVM x, SPItemVM y)
			{
				int num = y.ItemCost.CompareTo(x.ItemCost);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
