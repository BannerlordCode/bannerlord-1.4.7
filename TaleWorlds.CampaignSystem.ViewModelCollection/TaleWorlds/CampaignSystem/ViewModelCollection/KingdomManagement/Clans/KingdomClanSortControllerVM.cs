using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x02000087 RID: 135
	public class KingdomClanSortControllerVM : ViewModel
	{
		// Token: 0x06000B53 RID: 2899 RVA: 0x0002FE2C File Offset: 0x0002E02C
		public KingdomClanSortControllerVM(ref MBBindingList<KingdomClanItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._influenceComparer = new KingdomClanSortControllerVM.ItemInfluenceComparer();
			this._membersComparer = new KingdomClanSortControllerVM.ItemMembersComparer();
			this._nameComparer = new KingdomClanSortControllerVM.ItemNameComparer();
			this._fiefsComparer = new KingdomClanSortControllerVM.ItemFiefsComparer();
			this._typeComparer = new KingdomClanSortControllerVM.ItemTypeComparer();
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0002FE80 File Offset: 0x0002E080
		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsTypeSelected)
			{
				this._listToControl.Sort(this._typeComparer);
				return;
			}
			if (this.IsInfluenceSelected)
			{
				this._listToControl.Sort(this._influenceComparer);
				return;
			}
			if (this.IsMembersSelected)
			{
				this._listToControl.Sort(this._membersComparer);
				return;
			}
			if (this.IsFiefsSelected)
			{
				this._listToControl.Sort(this._fiefsComparer);
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002FF10 File Offset: 0x0002E110
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

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002FF78 File Offset: 0x0002E178
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

		// Token: 0x06000B57 RID: 2903 RVA: 0x0002FFE0 File Offset: 0x0002E1E0
		private void ExecuteSortByInfluence()
		{
			int influenceState = this.InfluenceState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.InfluenceState = (influenceState + 1) % 3;
			if (this.InfluenceState == 0)
			{
				this.InfluenceState++;
			}
			this._influenceComparer.SetSortMode(this.InfluenceState == 1);
			this._listToControl.Sort(this._influenceComparer);
			this.IsInfluenceSelected = true;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00030048 File Offset: 0x0002E248
		private void ExecuteSortByMembers()
		{
			int membersState = this.MembersState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.MembersState = (membersState + 1) % 3;
			if (this.MembersState == 0)
			{
				this.MembersState++;
			}
			this._membersComparer.SetSortMode(this.MembersState == 1);
			this._listToControl.Sort(this._membersComparer);
			this.IsMembersSelected = true;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x000300B0 File Offset: 0x0002E2B0
		private void ExecuteSortByFiefs()
		{
			int fiefsState = this.FiefsState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.FiefsState = (fiefsState + 1) % 3;
			if (this.FiefsState == 0)
			{
				this.FiefsState++;
			}
			this._fiefsComparer.SetSortMode(this.FiefsState == 1);
			this._listToControl.Sort(this._fiefsComparer);
			this.IsFiefsSelected = true;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00030118 File Offset: 0x0002E318
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.InfluenceState = (int)state;
			this.FiefsState = (int)state;
			this.MembersState = (int)state;
			this.NameState = (int)state;
			this.TypeState = (int)state;
			this.IsInfluenceSelected = false;
			this.IsFiefsSelected = false;
			this.IsNameSelected = false;
			this.IsMembersSelected = false;
			this.IsTypeSelected = false;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0003016B File Offset: 0x0002E36B
		// (set) Token: 0x06000B5C RID: 2908 RVA: 0x00030173 File Offset: 0x0002E373
		[DataSourceProperty]
		public int InfluenceState
		{
			get
			{
				return this._influenceState;
			}
			set
			{
				if (value != this._influenceState)
				{
					this._influenceState = value;
					base.OnPropertyChangedWithValue(value, "InfluenceState");
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00030191 File Offset: 0x0002E391
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x00030199 File Offset: 0x0002E399
		[DataSourceProperty]
		public int FiefsState
		{
			get
			{
				return this._fiefsState;
			}
			set
			{
				if (value != this._fiefsState)
				{
					this._fiefsState = value;
					base.OnPropertyChangedWithValue(value, "FiefsState");
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x000301B7 File Offset: 0x0002E3B7
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x000301BF File Offset: 0x0002E3BF
		[DataSourceProperty]
		public int MembersState
		{
			get
			{
				return this._membersState;
			}
			set
			{
				if (value != this._membersState)
				{
					this._membersState = value;
					base.OnPropertyChangedWithValue(value, "MembersState");
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x000301DD File Offset: 0x0002E3DD
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x000301E5 File Offset: 0x0002E3E5
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

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00030203 File Offset: 0x0002E403
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x0003020B File Offset: 0x0002E40B
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

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00030229 File Offset: 0x0002E429
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x00030231 File Offset: 0x0002E431
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

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0003024F File Offset: 0x0002E44F
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x00030257 File Offset: 0x0002E457
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

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00030275 File Offset: 0x0002E475
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x0003027D File Offset: 0x0002E47D
		[DataSourceProperty]
		public bool IsFiefsSelected
		{
			get
			{
				return this._isFiefsSelected;
			}
			set
			{
				if (value != this._isFiefsSelected)
				{
					this._isFiefsSelected = value;
					base.OnPropertyChangedWithValue(value, "IsFiefsSelected");
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0003029B File Offset: 0x0002E49B
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x000302A3 File Offset: 0x0002E4A3
		[DataSourceProperty]
		public bool IsMembersSelected
		{
			get
			{
				return this._isMembersSelected;
			}
			set
			{
				if (value != this._isMembersSelected)
				{
					this._isMembersSelected = value;
					base.OnPropertyChangedWithValue(value, "IsMembersSelected");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x000302C1 File Offset: 0x0002E4C1
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x000302C9 File Offset: 0x0002E4C9
		[DataSourceProperty]
		public bool IsInfluenceSelected
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
					base.OnPropertyChangedWithValue(value, "IsInfluenceSelected");
				}
			}
		}

		// Token: 0x0400050B RID: 1291
		private readonly MBBindingList<KingdomClanItemVM> _listToControl;

		// Token: 0x0400050C RID: 1292
		private readonly KingdomClanSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x0400050D RID: 1293
		private readonly KingdomClanSortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x0400050E RID: 1294
		private readonly KingdomClanSortControllerVM.ItemInfluenceComparer _influenceComparer;

		// Token: 0x0400050F RID: 1295
		private readonly KingdomClanSortControllerVM.ItemMembersComparer _membersComparer;

		// Token: 0x04000510 RID: 1296
		private readonly KingdomClanSortControllerVM.ItemFiefsComparer _fiefsComparer;

		// Token: 0x04000511 RID: 1297
		private int _influenceState;

		// Token: 0x04000512 RID: 1298
		private int _fiefsState;

		// Token: 0x04000513 RID: 1299
		private int _membersState;

		// Token: 0x04000514 RID: 1300
		private int _nameState;

		// Token: 0x04000515 RID: 1301
		private int _typeState;

		// Token: 0x04000516 RID: 1302
		private bool _isNameSelected;

		// Token: 0x04000517 RID: 1303
		private bool _isTypeSelected;

		// Token: 0x04000518 RID: 1304
		private bool _isFiefsSelected;

		// Token: 0x04000519 RID: 1305
		private bool _isMembersSelected;

		// Token: 0x0400051A RID: 1306
		private bool _isDistanceSelected;

		// Token: 0x020001E8 RID: 488
		public abstract class ItemComparerBase : IComparer<KingdomClanItemVM>
		{
			// Token: 0x060023EF RID: 9199 RVA: 0x0007F760 File Offset: 0x0007D960
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060023F0 RID: 9200
			public abstract int Compare(KingdomClanItemVM x, KingdomClanItemVM y);

			// Token: 0x060023F1 RID: 9201 RVA: 0x0007F769 File Offset: 0x0007D969
			protected int ResolveEquality(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				return x.Clan.Name.ToString().CompareTo(y.Clan.Name.ToString());
			}

			// Token: 0x04001161 RID: 4449
			protected bool _isAscending;
		}

		// Token: 0x020001E9 RID: 489
		public class ItemNameComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x060023F3 RID: 9203 RVA: 0x0007F798 File Offset: 0x0007D998
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				if (this._isAscending)
				{
					return y.Clan.Name.ToString().CompareTo(x.Clan.Name.ToString()) * -1;
				}
				return y.Clan.Name.ToString().CompareTo(x.Clan.Name.ToString());
			}
		}

		// Token: 0x020001EA RID: 490
		public class ItemTypeComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x060023F5 RID: 9205 RVA: 0x0007F804 File Offset: 0x0007DA04
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.ClanType.CompareTo(x.ClanType);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001EB RID: 491
		public class ItemInfluenceComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x060023F7 RID: 9207 RVA: 0x0007F848 File Offset: 0x0007DA48
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.Influence.CompareTo(x.Influence);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001EC RID: 492
		public class ItemMembersComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x060023F9 RID: 9209 RVA: 0x0007F88C File Offset: 0x0007DA8C
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.Members.Count.CompareTo(x.Members.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001ED RID: 493
		public class ItemFiefsComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x060023FB RID: 9211 RVA: 0x0007F8DC File Offset: 0x0007DADC
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.Fiefs.Count.CompareTo(x.Fiefs.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
