using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000138 RID: 312
	public class ClanFiefsSortControllerVM : ViewModel
	{
		// Token: 0x06001D12 RID: 7442 RVA: 0x0006BAE5 File Offset: 0x00069CE5
		public ClanFiefsSortControllerVM(List<MBBindingList<ClanSettlementItemVM>> listsToControl)
		{
			this._listsToControl = listsToControl;
			this._nameComparer = new ClanFiefsSortControllerVM.ItemNameComparer();
			this._governorComparer = new ClanFiefsSortControllerVM.ItemGovernorComparer();
			this._profitComparer = new ClanFiefsSortControllerVM.ItemProfitComparer();
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x0006BB18 File Offset: 0x00069D18
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.GovernorText = GameTexts.FindText("str_notable_governor", null).ToString();
			this.ProfitText = GameTexts.FindText("str_profit", null).ToString();
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x0006BB70 File Offset: 0x00069D70
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
			foreach (MBBindingList<ClanSettlementItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._nameComparer);
			}
			this.IsNameSelected = true;
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x0006BC14 File Offset: 0x00069E14
		public void ExecuteSortByGovernor()
		{
			int governorState = this.GovernorState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.GovernorState = (governorState + 1) % 3;
			if (this.GovernorState == 0)
			{
				int governorState2 = this.GovernorState;
				this.GovernorState = governorState2 + 1;
			}
			this._governorComparer.SetSortMode(this.GovernorState == 1);
			foreach (MBBindingList<ClanSettlementItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._governorComparer);
			}
			this.IsGovernorSelected = true;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x0006BCB8 File Offset: 0x00069EB8
		public void ExecuteSortByProfit()
		{
			int profitState = this.ProfitState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ProfitState = (profitState + 1) % 3;
			if (this.ProfitState == 0)
			{
				int profitState2 = this.ProfitState;
				this.ProfitState = profitState2 + 1;
			}
			this._profitComparer.SetSortMode(this.ProfitState == 1);
			foreach (MBBindingList<ClanSettlementItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._profitComparer);
			}
			this.IsProfitSelected = true;
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x0006BD5C File Offset: 0x00069F5C
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.GovernorState = (int)state;
			this.ProfitState = (int)state;
			this.IsNameSelected = false;
			this.IsGovernorSelected = false;
			this.IsProfitSelected = false;
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x0006BD88 File Offset: 0x00069F88
		public void ResetAllStates()
		{
			this.SetAllStates(CampaignUIHelper.SortState.Default);
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0006BD91 File Offset: 0x00069F91
		// (set) Token: 0x06001D1A RID: 7450 RVA: 0x0006BD99 File Offset: 0x00069F99
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

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x0006BDB7 File Offset: 0x00069FB7
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x0006BDBF File Offset: 0x00069FBF
		[DataSourceProperty]
		public int GovernorState
		{
			get
			{
				return this._governorState;
			}
			set
			{
				if (value != this._governorState)
				{
					this._governorState = value;
					base.OnPropertyChangedWithValue(value, "GovernorState");
				}
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x0006BDDD File Offset: 0x00069FDD
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x0006BDE5 File Offset: 0x00069FE5
		[DataSourceProperty]
		public int ProfitState
		{
			get
			{
				return this._profitState;
			}
			set
			{
				if (value != this._profitState)
				{
					this._profitState = value;
					base.OnPropertyChangedWithValue(value, "ProfitState");
				}
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0006BE03 File Offset: 0x0006A003
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x0006BE0B File Offset: 0x0006A00B
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

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x0006BE29 File Offset: 0x0006A029
		// (set) Token: 0x06001D22 RID: 7458 RVA: 0x0006BE31 File Offset: 0x0006A031
		[DataSourceProperty]
		public bool IsGovernorSelected
		{
			get
			{
				return this._isGovernorSelected;
			}
			set
			{
				if (value != this._isGovernorSelected)
				{
					this._isGovernorSelected = value;
					base.OnPropertyChangedWithValue(value, "IsGovernorSelected");
				}
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x0006BE4F File Offset: 0x0006A04F
		// (set) Token: 0x06001D24 RID: 7460 RVA: 0x0006BE57 File Offset: 0x0006A057
		[DataSourceProperty]
		public bool IsProfitSelected
		{
			get
			{
				return this._isProfitSelected;
			}
			set
			{
				if (value != this._isProfitSelected)
				{
					this._isProfitSelected = value;
					base.OnPropertyChangedWithValue(value, "IsProfitSelected");
				}
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0006BE75 File Offset: 0x0006A075
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x0006BE7D File Offset: 0x0006A07D
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x0006BEA0 File Offset: 0x0006A0A0
		// (set) Token: 0x06001D28 RID: 7464 RVA: 0x0006BEA8 File Offset: 0x0006A0A8
		[DataSourceProperty]
		public string GovernorText
		{
			get
			{
				return this._governorText;
			}
			set
			{
				if (value != this._governorText)
				{
					this._governorText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorText");
				}
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x0006BECB File Offset: 0x0006A0CB
		// (set) Token: 0x06001D2A RID: 7466 RVA: 0x0006BED3 File Offset: 0x0006A0D3
		[DataSourceProperty]
		public string ProfitText
		{
			get
			{
				return this._profitText;
			}
			set
			{
				if (value != this._profitText)
				{
					this._profitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfitText");
				}
			}
		}

		// Token: 0x04000D8F RID: 3471
		private readonly List<MBBindingList<ClanSettlementItemVM>> _listsToControl;

		// Token: 0x04000D90 RID: 3472
		private readonly ClanFiefsSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000D91 RID: 3473
		private readonly ClanFiefsSortControllerVM.ItemGovernorComparer _governorComparer;

		// Token: 0x04000D92 RID: 3474
		private readonly ClanFiefsSortControllerVM.ItemProfitComparer _profitComparer;

		// Token: 0x04000D93 RID: 3475
		private int _nameState;

		// Token: 0x04000D94 RID: 3476
		private int _governorState;

		// Token: 0x04000D95 RID: 3477
		private int _profitState;

		// Token: 0x04000D96 RID: 3478
		private bool _isNameSelected;

		// Token: 0x04000D97 RID: 3479
		private bool _isGovernorSelected;

		// Token: 0x04000D98 RID: 3480
		private bool _isProfitSelected;

		// Token: 0x04000D99 RID: 3481
		private string _nameText;

		// Token: 0x04000D9A RID: 3482
		private string _governorText;

		// Token: 0x04000D9B RID: 3483
		private string _profitText;

		// Token: 0x0200029D RID: 669
		public abstract class ItemComparerBase : IComparer<ClanSettlementItemVM>
		{
			// Token: 0x06002632 RID: 9778 RVA: 0x00082E47 File Offset: 0x00081047
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x06002633 RID: 9779
			public abstract int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y);

			// Token: 0x04001332 RID: 4914
			protected bool _isAcending;
		}

		// Token: 0x0200029E RID: 670
		public class ItemNameComparer : ClanFiefsSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002635 RID: 9781 RVA: 0x00082E58 File Offset: 0x00081058
			public override int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x0200029F RID: 671
		public class ItemGovernorComparer : ClanFiefsSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002637 RID: 9783 RVA: 0x00082E90 File Offset: 0x00081090
			public override int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y)
			{
				if (this._isAcending)
				{
					if (y.HasGovernor && x.HasGovernor)
					{
						return y.Governor.NameText.CompareTo(x.Governor.NameText) * -1;
					}
					if (y.HasGovernor)
					{
						return 1;
					}
					if (x.HasGovernor)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (y.HasGovernor && x.HasGovernor)
					{
						return y.Governor.NameText.CompareTo(x.Governor.NameText);
					}
					if (y.HasGovernor)
					{
						return 1;
					}
					if (x.HasGovernor)
					{
						return -1;
					}
					return 0;
				}
			}
		}

		// Token: 0x020002A0 RID: 672
		public class ItemProfitComparer : ClanFiefsSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002639 RID: 9785 RVA: 0x00082F34 File Offset: 0x00081134
			public override int Compare(ClanSettlementItemVM x, ClanSettlementItemVM y)
			{
				if (this._isAcending)
				{
					return y.TotalProfit.Value.CompareTo(x.TotalProfit.Value) * -1;
				}
				return y.TotalProfit.Value.CompareTo(x.TotalProfit.Value);
			}
		}
	}
}
