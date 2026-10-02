using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Settlements
{
	// Token: 0x02000068 RID: 104
	public class KingdomSettlementSortControllerVM : ViewModel
	{
		// Token: 0x0600081C RID: 2076 RVA: 0x000254E8 File Offset: 0x000236E8
		public KingdomSettlementSortControllerVM(MBBindingList<KingdomSettlementItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._typeComparer = new KingdomSettlementSortControllerVM.ItemTypeComparer();
			this._prosperityComparer = new KingdomSettlementSortControllerVM.ItemProsperityComparer();
			this._defendersComparer = new KingdomSettlementSortControllerVM.ItemDefendersComparer();
			this._ownerComparer = new KingdomSettlementSortControllerVM.ItemOwnerComparer();
			this._nameComparer = new KingdomSettlementSortControllerVM.ItemNameComparer();
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0002553C File Offset: 0x0002373C
		private void ExecuteSortByType()
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

		// Token: 0x0600081E RID: 2078 RVA: 0x000255A4 File Offset: 0x000237A4
		private void ExecuteSortByName()
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

		// Token: 0x0600081F RID: 2079 RVA: 0x0002560C File Offset: 0x0002380C
		private void ExecuteSortByOwner()
		{
			int ownerState = this.OwnerState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.OwnerState = (ownerState + 1) % 3;
			if (this.OwnerState == 0)
			{
				this.OwnerState++;
			}
			this._ownerComparer.SetSortMode(this.OwnerState == 1);
			this._listToControl.Sort(this._ownerComparer);
			this.IsOwnerSelected = true;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00025674 File Offset: 0x00023874
		private void ExecuteSortByProsperity()
		{
			int prosperityState = this.ProsperityState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ProsperityState = (prosperityState + 1) % 3;
			if (this.ProsperityState == 0)
			{
				this.ProsperityState++;
			}
			this._prosperityComparer.SetSortMode(this.ProsperityState == 1);
			this._listToControl.Sort(this._prosperityComparer);
			this.IsProsperitySelected = true;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x000256DC File Offset: 0x000238DC
		private void ExecuteSortByDefenders()
		{
			int defendersState = this.DefendersState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.DefendersState = (defendersState + 1) % 3;
			if (this.DefendersState == 0)
			{
				int defendersState2 = this.DefendersState;
				this.DefendersState = defendersState2 + 1;
			}
			this._defendersComparer.SetSortMode(this.DefendersState == 1);
			this._listToControl.Sort(this._defendersComparer);
			this.IsDefendersSelected = true;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00025748 File Offset: 0x00023948
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.TypeState = (int)state;
			this.NameState = (int)state;
			this.OwnerState = (int)state;
			this.ProsperityState = (int)state;
			this.DefendersState = (int)state;
			this.IsTypeSelected = false;
			this.IsNameSelected = false;
			this.IsProsperitySelected = false;
			this.IsOwnerSelected = false;
			this.IsDefendersSelected = false;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0002579B File Offset: 0x0002399B
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x000257A3 File Offset: 0x000239A3
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

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000257C1 File Offset: 0x000239C1
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x000257C9 File Offset: 0x000239C9
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

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x000257E7 File Offset: 0x000239E7
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x000257EF File Offset: 0x000239EF
		[DataSourceProperty]
		public int OwnerState
		{
			get
			{
				return this._ownerState;
			}
			set
			{
				if (value != this._ownerState)
				{
					this._ownerState = value;
					base.OnPropertyChangedWithValue(value, "OwnerState");
				}
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0002580D File Offset: 0x00023A0D
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x00025815 File Offset: 0x00023A15
		[DataSourceProperty]
		public int ProsperityState
		{
			get
			{
				return this._prosperityState;
			}
			set
			{
				if (value != this._prosperityState)
				{
					this._prosperityState = value;
					base.OnPropertyChangedWithValue(value, "ProsperityState");
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00025833 File Offset: 0x00023A33
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0002583B File Offset: 0x00023A3B
		[DataSourceProperty]
		public int DefendersState
		{
			get
			{
				return this._defendersState;
			}
			set
			{
				if (value != this._defendersState)
				{
					this._defendersState = value;
					base.OnPropertyChangedWithValue(value, "DefendersState");
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00025859 File Offset: 0x00023A59
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x00025861 File Offset: 0x00023A61
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

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0002587F File Offset: 0x00023A7F
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x00025887 File Offset: 0x00023A87
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

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x000258A5 File Offset: 0x00023AA5
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x000258AD File Offset: 0x00023AAD
		[DataSourceProperty]
		public bool IsDefendersSelected
		{
			get
			{
				return this._isDefendersSelected;
			}
			set
			{
				if (value != this._isDefendersSelected)
				{
					this._isDefendersSelected = value;
					base.OnPropertyChangedWithValue(value, "IsDefendersSelected");
				}
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x000258CB File Offset: 0x00023ACB
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x000258D3 File Offset: 0x00023AD3
		[DataSourceProperty]
		public bool IsOwnerSelected
		{
			get
			{
				return this._isOwnerSelected;
			}
			set
			{
				if (value != this._isOwnerSelected)
				{
					this._isOwnerSelected = value;
					base.OnPropertyChangedWithValue(value, "IsOwnerSelected");
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x000258F1 File Offset: 0x00023AF1
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x000258F9 File Offset: 0x00023AF9
		[DataSourceProperty]
		public bool IsProsperitySelected
		{
			get
			{
				return this._isProsperitySelected;
			}
			set
			{
				if (value != this._isProsperitySelected)
				{
					this._isProsperitySelected = value;
					base.OnPropertyChangedWithValue(value, "IsProsperitySelected");
				}
			}
		}

		// Token: 0x04000382 RID: 898
		private readonly MBBindingList<KingdomSettlementItemVM> _listToControl;

		// Token: 0x04000383 RID: 899
		private readonly KingdomSettlementSortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x04000384 RID: 900
		private readonly KingdomSettlementSortControllerVM.ItemProsperityComparer _prosperityComparer;

		// Token: 0x04000385 RID: 901
		private readonly KingdomSettlementSortControllerVM.ItemDefendersComparer _defendersComparer;

		// Token: 0x04000386 RID: 902
		private readonly KingdomSettlementSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000387 RID: 903
		private readonly KingdomSettlementSortControllerVM.ItemOwnerComparer _ownerComparer;

		// Token: 0x04000388 RID: 904
		private int _typeState;

		// Token: 0x04000389 RID: 905
		private int _nameState;

		// Token: 0x0400038A RID: 906
		private int _ownerState;

		// Token: 0x0400038B RID: 907
		private int _prosperityState;

		// Token: 0x0400038C RID: 908
		private int _defendersState;

		// Token: 0x0400038D RID: 909
		private bool _isTypeSelected;

		// Token: 0x0400038E RID: 910
		private bool _isNameSelected;

		// Token: 0x0400038F RID: 911
		private bool _isOwnerSelected;

		// Token: 0x04000390 RID: 912
		private bool _isProsperitySelected;

		// Token: 0x04000391 RID: 913
		private bool _isDefendersSelected;

		// Token: 0x020001C5 RID: 453
		public abstract class ItemComparerBase : IComparer<KingdomSettlementItemVM>
		{
			// Token: 0x0600237E RID: 9086 RVA: 0x0007EE65 File Offset: 0x0007D065
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600237F RID: 9087
			public abstract int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y);

			// Token: 0x06002380 RID: 9088 RVA: 0x0007EE6E File Offset: 0x0007D06E
			protected int ResolveEquality(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				return x.Settlement.Name.ToString().CompareTo(y.Settlement.Name.ToString());
			}

			// Token: 0x04001107 RID: 4359
			protected bool _isAscending;
		}

		// Token: 0x020001C6 RID: 454
		public class ItemNameComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002382 RID: 9090 RVA: 0x0007EEA0 File Offset: 0x0007D0A0
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				if (this._isAscending)
				{
					return y.Settlement.Name.ToString().CompareTo(x.Settlement.Name.ToString()) * -1;
				}
				return y.Settlement.Name.ToString().CompareTo(x.Settlement.Name.ToString());
			}
		}

		// Token: 0x020001C7 RID: 455
		public class ItemClanComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002384 RID: 9092 RVA: 0x0007EF0C File Offset: 0x0007D10C
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Settlement.OwnerClan.Name.ToString().CompareTo(x.Settlement.OwnerClan.Name.ToString());
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001C8 RID: 456
		public class ItemOwnerComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002386 RID: 9094 RVA: 0x0007EF6C File Offset: 0x0007D16C
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Owner.NameText.CompareTo(x.Owner.NameText);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001C9 RID: 457
		public class ItemVillagesComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002388 RID: 9096 RVA: 0x0007EFB8 File Offset: 0x0007D1B8
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Villages.Count.CompareTo(x.Villages.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CA RID: 458
		public class ItemTypeComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600238A RID: 9098 RVA: 0x0007F008 File Offset: 0x0007D208
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Settlement.IsCastle.CompareTo(x.Settlement.IsCastle);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CB RID: 459
		public class ItemProsperityComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600238C RID: 9100 RVA: 0x0007F058 File Offset: 0x0007D258
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Prosperity.CompareTo(x.Prosperity);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CC RID: 460
		public class ItemFoodComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600238E RID: 9102 RVA: 0x0007F09C File Offset: 0x0007D29C
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				float num = ((y.Settlement.Town != null) ? y.Settlement.Town.FoodStocks : 0f);
				float num2 = ((x.Settlement.Town != null) ? x.Settlement.Town.FoodStocks : 0f);
				int num3 = num.CompareTo(num2);
				if (num3 != 0)
				{
					return num3 * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CD RID: 461
		public class ItemGarrisonComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002390 RID: 9104 RVA: 0x0007F120 File Offset: 0x0007D320
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Garrison.CompareTo(x.Garrison);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001CE RID: 462
		private class ItemDefendersComparer : KingdomSettlementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002392 RID: 9106 RVA: 0x0007F164 File Offset: 0x0007D364
			public override int Compare(KingdomSettlementItemVM x, KingdomSettlementItemVM y)
			{
				int num = y.Defenders.CompareTo(x.Defenders);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
