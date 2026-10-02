using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting
{
	// Token: 0x02000112 RID: 274
	public class SmeltingSortControllerVM : ViewModel
	{
		// Token: 0x06001926 RID: 6438 RVA: 0x0005FD37 File Offset: 0x0005DF37
		public SmeltingSortControllerVM()
		{
			this._yieldComparer = new SmeltingSortControllerVM.ItemYieldComparer();
			this._typeComparer = new SmeltingSortControllerVM.ItemTypeComparer();
			this._nameComparer = new SmeltingSortControllerVM.ItemNameComparer();
			this.RefreshValues();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0005FD68 File Offset: 0x0005DF68
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SortNameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.SortTypeText = new TextObject("{=zMMqgxb1}Type", null).ToString();
			this.SortYieldText = new TextObject("{=v3OF6vBg}Yield", null).ToString();
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0005FDBD File Offset: 0x0005DFBD
		public void SetListToControl(MBBindingList<SmeltingItemVM> listToControl)
		{
			this._listToControl = listToControl;
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0005FDC8 File Offset: 0x0005DFC8
		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsYieldSelected)
			{
				this._listToControl.Sort(this._yieldComparer);
				return;
			}
			if (this.IsTypeSelected)
			{
				this._listToControl.Sort(this._typeComparer);
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0005FE24 File Offset: 0x0005E024
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0005FE8C File Offset: 0x0005E08C
		public void ExecuteSortByYield()
		{
			int yieldState = this.YieldState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.YieldState = (yieldState + 1) % 3;
			if (this.YieldState == 0)
			{
				this.YieldState++;
			}
			this._yieldComparer.SetSortMode(this.YieldState == 1);
			this._listToControl.Sort(this._yieldComparer);
			this.IsYieldSelected = true;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0005FEF4 File Offset: 0x0005E0F4
		public void ExecuteSortByType()
		{
			int typeState = this.TypeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.TypeState = (typeState + 1) % 3;
			if (this.TypeState == 0)
			{
				this.TypeState++;
			}
			this._typeComparer.SetSortMode(this.TypeState == 1);
			this._listToControl.Sort(this._typeComparer);
			this.IsTypeSelected = true;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0005FF5C File Offset: 0x0005E15C
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.TypeState = (int)state;
			this.YieldState = (int)state;
			this.IsNameSelected = false;
			this.IsTypeSelected = false;
			this.IsYieldSelected = false;
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x0005FF88 File Offset: 0x0005E188
		// (set) Token: 0x0600192F RID: 6447 RVA: 0x0005FF90 File Offset: 0x0005E190
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

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x0005FFAE File Offset: 0x0005E1AE
		// (set) Token: 0x06001931 RID: 6449 RVA: 0x0005FFB6 File Offset: 0x0005E1B6
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

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06001932 RID: 6450 RVA: 0x0005FFD4 File Offset: 0x0005E1D4
		// (set) Token: 0x06001933 RID: 6451 RVA: 0x0005FFDC File Offset: 0x0005E1DC
		[DataSourceProperty]
		public int YieldState
		{
			get
			{
				return this._yieldState;
			}
			set
			{
				if (value != this._yieldState)
				{
					this._yieldState = value;
					base.OnPropertyChangedWithValue(value, "YieldState");
				}
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x0005FFFA File Offset: 0x0005E1FA
		// (set) Token: 0x06001935 RID: 6453 RVA: 0x00060002 File Offset: 0x0005E202
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

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06001936 RID: 6454 RVA: 0x00060020 File Offset: 0x0005E220
		// (set) Token: 0x06001937 RID: 6455 RVA: 0x00060028 File Offset: 0x0005E228
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

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x00060046 File Offset: 0x0005E246
		// (set) Token: 0x06001939 RID: 6457 RVA: 0x0006004E File Offset: 0x0005E24E
		[DataSourceProperty]
		public bool IsYieldSelected
		{
			get
			{
				return this._isYieldSelected;
			}
			set
			{
				if (value != this._isYieldSelected)
				{
					this._isYieldSelected = value;
					base.OnPropertyChangedWithValue(value, "IsYieldSelected");
				}
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x0006006C File Offset: 0x0005E26C
		// (set) Token: 0x0600193B RID: 6459 RVA: 0x00060074 File Offset: 0x0005E274
		[DataSourceProperty]
		public string SortTypeText
		{
			get
			{
				return this._sortTypeText;
			}
			set
			{
				if (value != this._sortTypeText)
				{
					this._sortTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortTypeText");
				}
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x00060097 File Offset: 0x0005E297
		// (set) Token: 0x0600193D RID: 6461 RVA: 0x0006009F File Offset: 0x0005E29F
		[DataSourceProperty]
		public string SortNameText
		{
			get
			{
				return this._sortNameText;
			}
			set
			{
				if (value != this._sortNameText)
				{
					this._sortNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortNameText");
				}
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x000600C2 File Offset: 0x0005E2C2
		// (set) Token: 0x0600193F RID: 6463 RVA: 0x000600CA File Offset: 0x0005E2CA
		[DataSourceProperty]
		public string SortYieldText
		{
			get
			{
				return this._sortYieldText;
			}
			set
			{
				if (value != this._sortYieldText)
				{
					this._sortYieldText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortYieldText");
				}
			}
		}

		// Token: 0x04000B8D RID: 2957
		private MBBindingList<SmeltingItemVM> _listToControl;

		// Token: 0x04000B8E RID: 2958
		private readonly SmeltingSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000B8F RID: 2959
		private readonly SmeltingSortControllerVM.ItemYieldComparer _yieldComparer;

		// Token: 0x04000B90 RID: 2960
		private readonly SmeltingSortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x04000B91 RID: 2961
		private int _nameState;

		// Token: 0x04000B92 RID: 2962
		private int _yieldState;

		// Token: 0x04000B93 RID: 2963
		private int _typeState;

		// Token: 0x04000B94 RID: 2964
		private bool _isNameSelected;

		// Token: 0x04000B95 RID: 2965
		private bool _isYieldSelected;

		// Token: 0x04000B96 RID: 2966
		private bool _isTypeSelected;

		// Token: 0x04000B97 RID: 2967
		private string _sortTypeText;

		// Token: 0x04000B98 RID: 2968
		private string _sortNameText;

		// Token: 0x04000B99 RID: 2969
		private string _sortYieldText;

		// Token: 0x02000277 RID: 631
		public abstract class ItemComparerBase : IComparer<SmeltingItemVM>
		{
			// Token: 0x060025A9 RID: 9641 RVA: 0x00081EA1 File Offset: 0x000800A1
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060025AA RID: 9642
			public abstract int Compare(SmeltingItemVM x, SmeltingItemVM y);

			// Token: 0x060025AB RID: 9643 RVA: 0x00081EAA File Offset: 0x000800AA
			protected int ResolveEquality(SmeltingItemVM x, SmeltingItemVM y)
			{
				return x.Name.CompareTo(y.Name);
			}

			// Token: 0x040012CC RID: 4812
			protected bool _isAscending;
		}

		// Token: 0x02000278 RID: 632
		public class ItemNameComparer : SmeltingSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025AD RID: 9645 RVA: 0x00081EC5 File Offset: 0x000800C5
			public override int Compare(SmeltingItemVM x, SmeltingItemVM y)
			{
				if (this._isAscending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x02000279 RID: 633
		public class ItemYieldComparer : SmeltingSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025AF RID: 9647 RVA: 0x00081EFC File Offset: 0x000800FC
			public override int Compare(SmeltingItemVM x, SmeltingItemVM y)
			{
				int num = y.Yield.Count.CompareTo(x.Yield.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x0200027A RID: 634
		public class ItemTypeComparer : SmeltingSortControllerVM.ItemComparerBase
		{
			// Token: 0x060025B1 RID: 9649 RVA: 0x00081F4C File Offset: 0x0008014C
			public override int Compare(SmeltingItemVM x, SmeltingItemVM y)
			{
				int itemObjectTypeSortIndex = CampaignUIHelper.GetItemObjectTypeSortIndex(x.EquipmentElement.Item);
				int num = CampaignUIHelper.GetItemObjectTypeSortIndex(y.EquipmentElement.Item).CompareTo(itemObjectTypeSortIndex);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
