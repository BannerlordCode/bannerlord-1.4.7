using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x0200008B RID: 139
	public class KingdomArmySortControllerVM : ViewModel
	{
		// Token: 0x06000BD1 RID: 3025 RVA: 0x00031454 File Offset: 0x0002F654
		public KingdomArmySortControllerVM(ref MBBindingList<KingdomArmyItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._ownerComparer = new KingdomArmySortControllerVM.ItemOwnerComparer();
			this._strengthComparer = new KingdomArmySortControllerVM.ItemStrengthComparer();
			this._nameComparer = new KingdomArmySortControllerVM.ItemNameComparer();
			this._partiesComparer = new KingdomArmySortControllerVM.ItemPartiesComparer();
			this._distanceComparer = new KingdomArmySortControllerVM.ItemDistanceComparer();
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000314A8 File Offset: 0x0002F6A8
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

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00031510 File Offset: 0x0002F710
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

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00031578 File Offset: 0x0002F778
		private void ExecuteSortByStrength()
		{
			int strengthState = this.StrengthState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.StrengthState = (strengthState + 1) % 3;
			if (this.StrengthState == 0)
			{
				this.StrengthState++;
			}
			this._strengthComparer.SetSortMode(this.StrengthState == 1);
			this._listToControl.Sort(this._strengthComparer);
			this.IsStrengthSelected = true;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000315E0 File Offset: 0x0002F7E0
		private void ExecuteSortByParties()
		{
			int partiesState = this.PartiesState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.PartiesState = (partiesState + 1) % 3;
			if (this.PartiesState == 0)
			{
				this.PartiesState++;
			}
			this._partiesComparer.SetSortMode(this.PartiesState == 1);
			this._listToControl.Sort(this._partiesComparer);
			this.IsPartiesSelected = true;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00031648 File Offset: 0x0002F848
		private void ExecuteSortByDistance()
		{
			int distanceState = this.DistanceState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.DistanceState = (distanceState + 1) % 3;
			if (this.DistanceState == 0)
			{
				this.DistanceState++;
			}
			this._distanceComparer.SetSortMode(this.DistanceState == 1);
			this._listToControl.Sort(this._distanceComparer);
			this.IsDistanceSelected = true;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000316B0 File Offset: 0x0002F8B0
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.OwnerState = (int)state;
			this.StrengthState = (int)state;
			this.PartiesState = (int)state;
			this.DistanceState = (int)state;
			this.IsNameSelected = false;
			this.IsOwnerSelected = false;
			this.IsStrengthSelected = false;
			this.IsPartiesSelected = false;
			this.IsDistanceSelected = false;
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00031703 File Offset: 0x0002F903
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0003170B File Offset: 0x0002F90B
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

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00031729 File Offset: 0x0002F929
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x00031731 File Offset: 0x0002F931
		[DataSourceProperty]
		public int PartiesState
		{
			get
			{
				return this._partiesState;
			}
			set
			{
				if (value != this._partiesState)
				{
					this._partiesState = value;
					base.OnPropertyChangedWithValue(value, "PartiesState");
				}
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0003174F File Offset: 0x0002F94F
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x00031757 File Offset: 0x0002F957
		[DataSourceProperty]
		public int StrengthState
		{
			get
			{
				return this._strengthState;
			}
			set
			{
				if (value != this._strengthState)
				{
					this._strengthState = value;
					base.OnPropertyChangedWithValue(value, "StrengthState");
				}
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00031775 File Offset: 0x0002F975
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0003177D File Offset: 0x0002F97D
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

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0003179B File Offset: 0x0002F99B
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x000317A3 File Offset: 0x0002F9A3
		[DataSourceProperty]
		public int DistanceState
		{
			get
			{
				return this._distanceState;
			}
			set
			{
				if (value != this._distanceState)
				{
					this._distanceState = value;
					base.OnPropertyChangedWithValue(value, "DistanceState");
				}
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x000317C1 File Offset: 0x0002F9C1
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x000317C9 File Offset: 0x0002F9C9
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

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x000317E7 File Offset: 0x0002F9E7
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x000317EF File Offset: 0x0002F9EF
		[DataSourceProperty]
		public bool IsPartiesSelected
		{
			get
			{
				return this._isPartiesSelected;
			}
			set
			{
				if (value != this._isPartiesSelected)
				{
					this._isPartiesSelected = value;
					base.OnPropertyChangedWithValue(value, "IsPartiesSelected");
				}
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0003180D File Offset: 0x0002FA0D
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x00031815 File Offset: 0x0002FA15
		[DataSourceProperty]
		public bool IsStrengthSelected
		{
			get
			{
				return this._isStrengthSelected;
			}
			set
			{
				if (value != this._isStrengthSelected)
				{
					this._isStrengthSelected = value;
					base.OnPropertyChangedWithValue(value, "IsStrengthSelected");
				}
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00031833 File Offset: 0x0002FA33
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0003183B File Offset: 0x0002FA3B
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

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00031859 File Offset: 0x0002FA59
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00031861 File Offset: 0x0002FA61
		[DataSourceProperty]
		public bool IsDistanceSelected
		{
			get
			{
				return this._isDistanceSelected;
			}
			set
			{
				if (value != this._isDistanceSelected)
				{
					this._isDistanceSelected = value;
					base.OnPropertyChangedWithValue(value, "IsDistanceSelected");
				}
			}
		}

		// Token: 0x04000544 RID: 1348
		private readonly MBBindingList<KingdomArmyItemVM> _listToControl;

		// Token: 0x04000545 RID: 1349
		private readonly KingdomArmySortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000546 RID: 1350
		private readonly KingdomArmySortControllerVM.ItemOwnerComparer _ownerComparer;

		// Token: 0x04000547 RID: 1351
		private readonly KingdomArmySortControllerVM.ItemStrengthComparer _strengthComparer;

		// Token: 0x04000548 RID: 1352
		private readonly KingdomArmySortControllerVM.ItemPartiesComparer _partiesComparer;

		// Token: 0x04000549 RID: 1353
		private readonly KingdomArmySortControllerVM.ItemDistanceComparer _distanceComparer;

		// Token: 0x0400054A RID: 1354
		private int _nameState;

		// Token: 0x0400054B RID: 1355
		private int _ownerState;

		// Token: 0x0400054C RID: 1356
		private int _strengthState;

		// Token: 0x0400054D RID: 1357
		private int _partiesState;

		// Token: 0x0400054E RID: 1358
		private int _distanceState;

		// Token: 0x0400054F RID: 1359
		private bool _isNameSelected;

		// Token: 0x04000550 RID: 1360
		private bool _isOwnerSelected;

		// Token: 0x04000551 RID: 1361
		private bool _isStrengthSelected;

		// Token: 0x04000552 RID: 1362
		private bool _isPartiesSelected;

		// Token: 0x04000553 RID: 1363
		private bool _isDistanceSelected;

		// Token: 0x020001EF RID: 495
		public abstract class ItemComparerBase : IComparer<KingdomArmyItemVM>
		{
			// Token: 0x06002402 RID: 9218 RVA: 0x0007F966 File Offset: 0x0007DB66
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06002403 RID: 9219
			public abstract int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y);

			// Token: 0x06002404 RID: 9220 RVA: 0x0007F96F File Offset: 0x0007DB6F
			protected int ResolveEquality(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				return x.ArmyName.CompareTo(y.ArmyName);
			}

			// Token: 0x04001166 RID: 4454
			protected bool _isAscending;
		}

		// Token: 0x020001F0 RID: 496
		public class ItemNameComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x06002406 RID: 9222 RVA: 0x0007F98A File Offset: 0x0007DB8A
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				if (this._isAscending)
				{
					return y.ArmyName.CompareTo(x.ArmyName) * -1;
				}
				return y.ArmyName.CompareTo(x.ArmyName);
			}
		}

		// Token: 0x020001F1 RID: 497
		public class ItemOwnerComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x06002408 RID: 9224 RVA: 0x0007F9C4 File Offset: 0x0007DBC4
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.Leader.NameText.ToString().CompareTo(x.Leader.NameText.ToString());
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F2 RID: 498
		public class ItemStrengthComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x0600240A RID: 9226 RVA: 0x0007FA1C File Offset: 0x0007DC1C
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.Strength.CompareTo(x.Strength);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F3 RID: 499
		public class ItemPartiesComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x0600240C RID: 9228 RVA: 0x0007FA60 File Offset: 0x0007DC60
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.Parties.Count.CompareTo(x.Parties.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F4 RID: 500
		public class ItemDistanceComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x0600240E RID: 9230 RVA: 0x0007FAB0 File Offset: 0x0007DCB0
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.DistanceToMainParty.CompareTo(x.DistanceToMainParty);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
