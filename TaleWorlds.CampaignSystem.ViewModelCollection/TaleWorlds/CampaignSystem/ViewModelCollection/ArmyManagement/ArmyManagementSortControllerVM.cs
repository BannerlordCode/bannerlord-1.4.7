using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x0200015D RID: 349
	public class ArmyManagementSortControllerVM : ViewModel
	{
		// Token: 0x0600215E RID: 8542 RVA: 0x0007915C File Offset: 0x0007735C
		public ArmyManagementSortControllerVM(MBBindingList<ArmyManagementItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._distanceComparer = new ArmyManagementSortControllerVM.ItemDistanceComparer();
			this._costComparer = new ArmyManagementSortControllerVM.ItemCostComparer();
			this._strengthComparer = new ArmyManagementSortControllerVM.ItemStrengthComparer();
			this._nameComparer = new ArmyManagementSortControllerVM.ItemNameComparer();
			this._clanComparer = new ArmyManagementSortControllerVM.ItemClanComparer();
			this._shipCountComparer = new ArmyManagementSortControllerVM.ItemShipCountComparer();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000791B8 File Offset: 0x000773B8
		public void ExecuteSortByDistance()
		{
			int distanceState = this.DistanceState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.DistanceState = (distanceState + 1) % 3;
			if (this.DistanceState == 0)
			{
				int distanceState2 = this.DistanceState;
				this.DistanceState = distanceState2 + 1;
			}
			this._distanceComparer.SetSortMode(this.DistanceState == 1);
			this._listToControl.Sort(this._distanceComparer);
			this.IsDistanceSelected = true;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00079224 File Offset: 0x00077424
		public void ExecuteSortByCost()
		{
			int costState = this.CostState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.CostState = (costState + 1) % 3;
			if (this.CostState == 0)
			{
				int costState2 = this.CostState;
				this.CostState = costState2 + 1;
			}
			this._costComparer.SetSortMode(this.CostState == 1);
			this._listToControl.Sort(this._costComparer);
			this.IsCostSelected = true;
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x00079290 File Offset: 0x00077490
		public void ExecuteSortByStrength()
		{
			int strengthState = this.StrengthState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.StrengthState = (strengthState + 1) % 3;
			if (this.StrengthState == 0)
			{
				int strengthState2 = this.StrengthState;
				this.StrengthState = strengthState2 + 1;
			}
			this._strengthComparer.SetSortMode(this.StrengthState == 1);
			this._listToControl.Sort(this._strengthComparer);
			this.IsStrengthSelected = true;
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000792FC File Offset: 0x000774FC
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				int nameState2 = this.NameState;
				this.NameState = nameState2 + 1;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x00079368 File Offset: 0x00077568
		public void ExecuteSortByClan()
		{
			int clanState = this.ClanState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ClanState = (clanState + 1) % 3;
			if (this.ClanState == 0)
			{
				int clanState2 = this.ClanState;
				this.ClanState = clanState2 + 1;
			}
			this._clanComparer.SetSortMode(this.ClanState == 1);
			this._listToControl.Sort(this._clanComparer);
			this.IsClanSelected = true;
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000793D4 File Offset: 0x000775D4
		public void ExecuteSortByShipCount()
		{
			int shipCountState = this.ShipCountState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ShipCountState = (shipCountState + 1) % 3;
			if (this.ShipCountState == 0)
			{
				int shipCountState2 = this.ShipCountState;
				this.ShipCountState = shipCountState2 + 1;
			}
			this._shipCountComparer.SetSortMode(this.ShipCountState == 1);
			this._listToControl.Sort(this._shipCountComparer);
			this.IsShipCountSelected = true;
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00079440 File Offset: 0x00077640
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.DistanceState = (int)state;
			this.CostState = (int)state;
			this.StrengthState = (int)state;
			this.NameState = (int)state;
			this.ClanState = (int)state;
			this.ShipCountState = (int)state;
			this.IsDistanceSelected = false;
			this.IsCostSelected = false;
			this.IsNameSelected = false;
			this.IsClanSelected = false;
			this.IsStrengthSelected = false;
			this.IsShipCountSelected = false;
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000794A1 File Offset: 0x000776A1
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x000794A9 File Offset: 0x000776A9
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

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000794C7 File Offset: 0x000776C7
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000794CF File Offset: 0x000776CF
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

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000794ED File Offset: 0x000776ED
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x000794F5 File Offset: 0x000776F5
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

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00079513 File Offset: 0x00077713
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x0007951B File Offset: 0x0007771B
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

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x00079539 File Offset: 0x00077739
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x00079541 File Offset: 0x00077741
		[DataSourceProperty]
		public int ClanState
		{
			get
			{
				return this._clanState;
			}
			set
			{
				if (value != this._clanState)
				{
					this._clanState = value;
					base.OnPropertyChangedWithValue(value, "ClanState");
				}
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x0007955F File Offset: 0x0007775F
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x00079567 File Offset: 0x00077767
		[DataSourceProperty]
		public int ShipCountState
		{
			get
			{
				return this._shipCountState;
			}
			set
			{
				if (value != this._shipCountState)
				{
					this._shipCountState = value;
					base.OnPropertyChangedWithValue(value, "ShipCountState");
				}
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x00079585 File Offset: 0x00077785
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x0007958D File Offset: 0x0007778D
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

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000795AB File Offset: 0x000777AB
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x000795B3 File Offset: 0x000777B3
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

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000795D1 File Offset: 0x000777D1
		// (set) Token: 0x06002177 RID: 8567 RVA: 0x000795D9 File Offset: 0x000777D9
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

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000795F7 File Offset: 0x000777F7
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x000795FF File Offset: 0x000777FF
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

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x0007961D File Offset: 0x0007781D
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x00079625 File Offset: 0x00077825
		[DataSourceProperty]
		public bool IsClanSelected
		{
			get
			{
				return this._isClanSelected;
			}
			set
			{
				if (value != this._isClanSelected)
				{
					this._isClanSelected = value;
					base.OnPropertyChangedWithValue(value, "IsClanSelected");
				}
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x00079643 File Offset: 0x00077843
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x0007964B File Offset: 0x0007784B
		[DataSourceProperty]
		public bool IsShipCountSelected
		{
			get
			{
				return this._isShipCountSelected;
			}
			set
			{
				if (value != this._isShipCountSelected)
				{
					this._isShipCountSelected = value;
					base.OnPropertyChangedWithValue(value, "IsShipCountSelected");
				}
			}
		}

		// Token: 0x04000F82 RID: 3970
		private readonly MBBindingList<ArmyManagementItemVM> _listToControl;

		// Token: 0x04000F83 RID: 3971
		private readonly ArmyManagementSortControllerVM.ItemDistanceComparer _distanceComparer;

		// Token: 0x04000F84 RID: 3972
		private readonly ArmyManagementSortControllerVM.ItemCostComparer _costComparer;

		// Token: 0x04000F85 RID: 3973
		private readonly ArmyManagementSortControllerVM.ItemStrengthComparer _strengthComparer;

		// Token: 0x04000F86 RID: 3974
		private readonly ArmyManagementSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000F87 RID: 3975
		private readonly ArmyManagementSortControllerVM.ItemClanComparer _clanComparer;

		// Token: 0x04000F88 RID: 3976
		private readonly ArmyManagementSortControllerVM.ItemShipCountComparer _shipCountComparer;

		// Token: 0x04000F89 RID: 3977
		private int _distanceState;

		// Token: 0x04000F8A RID: 3978
		private int _costState;

		// Token: 0x04000F8B RID: 3979
		private int _strengthState;

		// Token: 0x04000F8C RID: 3980
		private int _nameState;

		// Token: 0x04000F8D RID: 3981
		private int _clanState;

		// Token: 0x04000F8E RID: 3982
		private int _shipCountState;

		// Token: 0x04000F8F RID: 3983
		private bool _isNameSelected;

		// Token: 0x04000F90 RID: 3984
		private bool _isCostSelected;

		// Token: 0x04000F91 RID: 3985
		private bool _isStrengthSelected;

		// Token: 0x04000F92 RID: 3986
		private bool _isDistanceSelected;

		// Token: 0x04000F93 RID: 3987
		private bool _isClanSelected;

		// Token: 0x04000F94 RID: 3988
		private bool _isShipCountSelected;

		// Token: 0x020002EA RID: 746
		public abstract class ItemComparerBase : IComparer<ArmyManagementItemVM>
		{
			// Token: 0x06002753 RID: 10067 RVA: 0x00085175 File Offset: 0x00083375
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06002754 RID: 10068
			public abstract int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y);

			// Token: 0x06002755 RID: 10069 RVA: 0x0008517E File Offset: 0x0008337E
			protected int ResolveEquality(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				return x.LeaderNameText.CompareTo(y.LeaderNameText);
			}

			// Token: 0x04001401 RID: 5121
			protected bool _isAscending;
		}

		// Token: 0x020002EB RID: 747
		public class ItemDistanceComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002757 RID: 10071 RVA: 0x0008519C File Offset: 0x0008339C
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.DistInTime.CompareTo(x.DistInTime);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002EC RID: 748
		public class ItemCostComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002759 RID: 10073 RVA: 0x000851E0 File Offset: 0x000833E0
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.Cost.CompareTo(x.Cost);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002ED RID: 749
		public class ItemStrengthComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600275B RID: 10075 RVA: 0x00085224 File Offset: 0x00083424
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.Strength.CompareTo(x.Strength);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				int num2 = y.ShipCount.CompareTo(x.ShipCount);
				if (num2 != 0)
				{
					return num2 * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002EE RID: 750
		public class ItemNameComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600275D RID: 10077 RVA: 0x0008528F File Offset: 0x0008348F
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				if (this._isAscending)
				{
					return y.LeaderNameText.CompareTo(x.LeaderNameText) * -1;
				}
				return y.LeaderNameText.CompareTo(x.LeaderNameText);
			}
		}

		// Token: 0x020002EF RID: 751
		public class ItemClanComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600275F RID: 10079 RVA: 0x000852C8 File Offset: 0x000834C8
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.Clan.Name.ToString().CompareTo(x.Clan.Name.ToString());
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020002F0 RID: 752
		public class ItemShipCountComparer : ArmyManagementSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002761 RID: 10081 RVA: 0x00085320 File Offset: 0x00083520
			public override int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				int num = y.ShipCount.CompareTo(x.ShipCount);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
