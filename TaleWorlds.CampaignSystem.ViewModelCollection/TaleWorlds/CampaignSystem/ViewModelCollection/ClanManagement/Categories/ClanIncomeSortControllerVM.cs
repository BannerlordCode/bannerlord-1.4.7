using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x0200013A RID: 314
	public class ClanIncomeSortControllerVM : ViewModel
	{
		// Token: 0x06001D60 RID: 7520 RVA: 0x0006CB54 File Offset: 0x0006AD54
		public ClanIncomeSortControllerVM(MBBindingList<ClanFinanceWorkshopItemVM> workshopList, MBBindingList<ClanSupporterGroupVM> supporterList, MBBindingList<ClanFinanceAlleyItemVM> alleyList)
		{
			this._workshopList = workshopList;
			this._supporterList = supporterList;
			this._alleyList = alleyList;
			this._workshopNameComparer = new ClanIncomeSortControllerVM.WorkshopItemNameComparer();
			this._supporterNameComparer = new ClanIncomeSortControllerVM.SupporterItemNameComparer();
			this._alleyNameComparer = new ClanIncomeSortControllerVM.AlleyItemNameComparer();
			this._workshopLocationComparer = new ClanIncomeSortControllerVM.WorkshopItemLocationComparer();
			this._alleyLocationComparer = new ClanIncomeSortControllerVM.AlleyItemLocationComparer();
			this._workshopIncomeComparer = new ClanIncomeSortControllerVM.WorkshopItemIncomeComparer();
			this._supporterIncomeComparer = new ClanIncomeSortControllerVM.SupporterItemIncomeComparer();
			this._alleyIncomeComparer = new ClanIncomeSortControllerVM.AlleyItemIncomeComparer();
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0006CBD4 File Offset: 0x0006ADD4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.IncomeText = GameTexts.FindText("str_income", null).ToString();
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x0006CC2C File Offset: 0x0006AE2C
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
			this._workshopNameComparer.SetSortMode(this.NameState == 1);
			this._supporterNameComparer.SetSortMode(this.NameState == 1);
			this._alleyNameComparer.SetSortMode(this.NameState == 1);
			this._workshopList.Sort(this._workshopNameComparer);
			this._supporterList.Sort(this._supporterNameComparer);
			this._alleyList.Sort(this._alleyNameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0006CCE0 File Offset: 0x0006AEE0
		public void ExecuteSortByLocation()
		{
			int locationState = this.LocationState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.LocationState = (locationState + 1) % 3;
			if (this.LocationState == 0)
			{
				int locationState2 = this.LocationState;
				this.LocationState = locationState2 + 1;
			}
			this._workshopLocationComparer.SetSortMode(this.LocationState == 1);
			this._alleyLocationComparer.SetSortMode(this.LocationState == 1);
			this._workshopList.Sort(this._workshopLocationComparer);
			this._alleyList.Sort(this._alleyLocationComparer);
			this.IsLocationSelected = true;
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0006CD70 File Offset: 0x0006AF70
		public void ExecuteSortByIncome()
		{
			int incomeState = this.IncomeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.IncomeState = (incomeState + 1) % 3;
			if (this.IncomeState == 0)
			{
				int incomeState2 = this.IncomeState;
				this.IncomeState = incomeState2 + 1;
			}
			this._workshopIncomeComparer.SetSortMode(this.IncomeState == 1);
			this._supporterIncomeComparer.SetSortMode(this.IncomeState == 1);
			this._alleyIncomeComparer.SetSortMode(this.IncomeState == 1);
			this._workshopList.Sort(this._workshopIncomeComparer);
			this._supporterList.Sort(this._supporterIncomeComparer);
			this._alleyList.Sort(this._alleyIncomeComparer);
			this.IsIncomeSelected = true;
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0006CE24 File Offset: 0x0006B024
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.LocationState = (int)state;
			this.IncomeState = (int)state;
			this.IsNameSelected = false;
			this.IsLocationSelected = false;
			this.IsIncomeSelected = false;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x0006CE50 File Offset: 0x0006B050
		public void ResetAllStates()
		{
			this.SetAllStates(CampaignUIHelper.SortState.Default);
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06001D67 RID: 7527 RVA: 0x0006CE59 File Offset: 0x0006B059
		// (set) Token: 0x06001D68 RID: 7528 RVA: 0x0006CE61 File Offset: 0x0006B061
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

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x0006CE7F File Offset: 0x0006B07F
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x0006CE87 File Offset: 0x0006B087
		[DataSourceProperty]
		public int LocationState
		{
			get
			{
				return this._locationState;
			}
			set
			{
				if (value != this._locationState)
				{
					this._locationState = value;
					base.OnPropertyChangedWithValue(value, "LocationState");
				}
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0006CEA5 File Offset: 0x0006B0A5
		// (set) Token: 0x06001D6C RID: 7532 RVA: 0x0006CEAD File Offset: 0x0006B0AD
		[DataSourceProperty]
		public int IncomeState
		{
			get
			{
				return this._incomeState;
			}
			set
			{
				if (value != this._incomeState)
				{
					this._incomeState = value;
					base.OnPropertyChangedWithValue(value, "IncomeState");
				}
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0006CECB File Offset: 0x0006B0CB
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x0006CED3 File Offset: 0x0006B0D3
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

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x0006CEF1 File Offset: 0x0006B0F1
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x0006CEF9 File Offset: 0x0006B0F9
		[DataSourceProperty]
		public bool IsLocationSelected
		{
			get
			{
				return this._isLocationSelected;
			}
			set
			{
				if (value != this._isLocationSelected)
				{
					this._isLocationSelected = value;
					base.OnPropertyChangedWithValue(value, "IsLocationSelected");
				}
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x0006CF17 File Offset: 0x0006B117
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x0006CF1F File Offset: 0x0006B11F
		[DataSourceProperty]
		public bool IsIncomeSelected
		{
			get
			{
				return this._isIncomeSelected;
			}
			set
			{
				if (value != this._isIncomeSelected)
				{
					this._isIncomeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsIncomeSelected");
				}
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x0006CF3D File Offset: 0x0006B13D
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x0006CF45 File Offset: 0x0006B145
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

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x0006CF68 File Offset: 0x0006B168
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x0006CF70 File Offset: 0x0006B170
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x0006CF93 File Offset: 0x0006B193
		// (set) Token: 0x06001D78 RID: 7544 RVA: 0x0006CF9B File Offset: 0x0006B19B
		[DataSourceProperty]
		public string IncomeText
		{
			get
			{
				return this._incomeText;
			}
			set
			{
				if (value != this._incomeText)
				{
					this._incomeText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeText");
				}
			}
		}

		// Token: 0x04000DB2 RID: 3506
		private readonly MBBindingList<ClanFinanceWorkshopItemVM> _workshopList;

		// Token: 0x04000DB3 RID: 3507
		private readonly MBBindingList<ClanSupporterGroupVM> _supporterList;

		// Token: 0x04000DB4 RID: 3508
		private readonly MBBindingList<ClanFinanceAlleyItemVM> _alleyList;

		// Token: 0x04000DB5 RID: 3509
		private readonly ClanIncomeSortControllerVM.WorkshopItemNameComparer _workshopNameComparer;

		// Token: 0x04000DB6 RID: 3510
		private readonly ClanIncomeSortControllerVM.SupporterItemNameComparer _supporterNameComparer;

		// Token: 0x04000DB7 RID: 3511
		private readonly ClanIncomeSortControllerVM.AlleyItemNameComparer _alleyNameComparer;

		// Token: 0x04000DB8 RID: 3512
		private readonly ClanIncomeSortControllerVM.WorkshopItemLocationComparer _workshopLocationComparer;

		// Token: 0x04000DB9 RID: 3513
		private readonly ClanIncomeSortControllerVM.AlleyItemLocationComparer _alleyLocationComparer;

		// Token: 0x04000DBA RID: 3514
		private readonly ClanIncomeSortControllerVM.WorkshopItemIncomeComparer _workshopIncomeComparer;

		// Token: 0x04000DBB RID: 3515
		private readonly ClanIncomeSortControllerVM.SupporterItemIncomeComparer _supporterIncomeComparer;

		// Token: 0x04000DBC RID: 3516
		private readonly ClanIncomeSortControllerVM.AlleyItemIncomeComparer _alleyIncomeComparer;

		// Token: 0x04000DBD RID: 3517
		private int _nameState;

		// Token: 0x04000DBE RID: 3518
		private int _locationState;

		// Token: 0x04000DBF RID: 3519
		private int _incomeState;

		// Token: 0x04000DC0 RID: 3520
		private bool _isNameSelected;

		// Token: 0x04000DC1 RID: 3521
		private bool _isLocationSelected;

		// Token: 0x04000DC2 RID: 3522
		private bool _isIncomeSelected;

		// Token: 0x04000DC3 RID: 3523
		private string _nameText;

		// Token: 0x04000DC4 RID: 3524
		private string _locationText;

		// Token: 0x04000DC5 RID: 3525
		private string _incomeText;

		// Token: 0x020002A9 RID: 681
		public abstract class WorkshopItemComparerBase : IComparer<ClanFinanceWorkshopItemVM>
		{
			// Token: 0x06002668 RID: 9832 RVA: 0x000839A8 File Offset: 0x00081BA8
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x06002669 RID: 9833
			public abstract int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y);

			// Token: 0x04001356 RID: 4950
			protected bool _isAcending;
		}

		// Token: 0x020002AA RID: 682
		public abstract class SupporterItemComparerBase : IComparer<ClanSupporterGroupVM>
		{
			// Token: 0x0600266B RID: 9835 RVA: 0x000839B9 File Offset: 0x00081BB9
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x0600266C RID: 9836
			public abstract int Compare(ClanSupporterGroupVM x, ClanSupporterGroupVM y);

			// Token: 0x04001357 RID: 4951
			protected bool _isAcending;
		}

		// Token: 0x020002AB RID: 683
		public abstract class AlleyItemComparerBase : IComparer<ClanFinanceAlleyItemVM>
		{
			// Token: 0x0600266E RID: 9838 RVA: 0x000839CA File Offset: 0x00081BCA
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x0600266F RID: 9839
			public abstract int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y);

			// Token: 0x04001358 RID: 4952
			protected bool _isAcending;
		}

		// Token: 0x020002AC RID: 684
		public class WorkshopItemNameComparer : ClanIncomeSortControllerVM.WorkshopItemComparerBase
		{
			// Token: 0x06002671 RID: 9841 RVA: 0x000839DB File Offset: 0x00081BDB
			public override int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002AD RID: 685
		public class SupporterItemNameComparer : ClanIncomeSortControllerVM.SupporterItemComparerBase
		{
			// Token: 0x06002673 RID: 9843 RVA: 0x00083A12 File Offset: 0x00081C12
			public override int Compare(ClanSupporterGroupVM x, ClanSupporterGroupVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002AE RID: 686
		public class AlleyItemNameComparer : ClanIncomeSortControllerVM.AlleyItemComparerBase
		{
			// Token: 0x06002675 RID: 9845 RVA: 0x00083A49 File Offset: 0x00081C49
			public override int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002AF RID: 687
		public class WorkshopItemLocationComparer : ClanIncomeSortControllerVM.WorkshopItemComparerBase
		{
			// Token: 0x06002677 RID: 9847 RVA: 0x00083A80 File Offset: 0x00081C80
			public override int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y)
			{
				int num = this.GetDistanceToMainParty(y).CompareTo(this.GetDistanceToMainParty(x));
				if (this._isAcending)
				{
					return num * -1;
				}
				return num;
			}

			// Token: 0x06002678 RID: 9848 RVA: 0x00083AB4 File Offset: 0x00081CB4
			private float GetDistanceToMainParty(ClanFinanceWorkshopItemVM item)
			{
				return item.Workshop.Settlement.Position.Distance(Hero.MainHero.GetCampaignPosition());
			}
		}

		// Token: 0x020002B0 RID: 688
		public class AlleyItemLocationComparer : ClanIncomeSortControllerVM.AlleyItemComparerBase
		{
			// Token: 0x0600267A RID: 9850 RVA: 0x00083AEC File Offset: 0x00081CEC
			public override int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y)
			{
				int num = this.GetDistanceToMainParty(y).CompareTo(this.GetDistanceToMainParty(x));
				if (this._isAcending)
				{
					return num * -1;
				}
				return num;
			}

			// Token: 0x0600267B RID: 9851 RVA: 0x00083B20 File Offset: 0x00081D20
			private float GetDistanceToMainParty(ClanFinanceAlleyItemVM item)
			{
				return item.Alley.Settlement.Position.Distance(Hero.MainHero.GetCampaignPosition());
			}
		}

		// Token: 0x020002B1 RID: 689
		public class WorkshopItemIncomeComparer : ClanIncomeSortControllerVM.WorkshopItemComparerBase
		{
			// Token: 0x0600267D RID: 9853 RVA: 0x00083B58 File Offset: 0x00081D58
			public override int Compare(ClanFinanceWorkshopItemVM x, ClanFinanceWorkshopItemVM y)
			{
				if (this._isAcending)
				{
					return y.Workshop.ProfitMade.CompareTo(x.Workshop.ProfitMade) * -1;
				}
				return y.Workshop.ProfitMade.CompareTo(x.Workshop.ProfitMade);
			}
		}

		// Token: 0x020002B2 RID: 690
		public class SupporterItemIncomeComparer : ClanIncomeSortControllerVM.SupporterItemComparerBase
		{
			// Token: 0x0600267F RID: 9855 RVA: 0x00083BB4 File Offset: 0x00081DB4
			public override int Compare(ClanSupporterGroupVM x, ClanSupporterGroupVM y)
			{
				if (this._isAcending)
				{
					return y.TotalInfluenceBonus.CompareTo(x.TotalInfluenceBonus) * -1;
				}
				return y.TotalInfluenceBonus.CompareTo(x.TotalInfluenceBonus);
			}
		}

		// Token: 0x020002B3 RID: 691
		public class AlleyItemIncomeComparer : ClanIncomeSortControllerVM.AlleyItemComparerBase
		{
			// Token: 0x06002681 RID: 9857 RVA: 0x00083BFC File Offset: 0x00081DFC
			public override int Compare(ClanFinanceAlleyItemVM x, ClanFinanceAlleyItemVM y)
			{
				if (this._isAcending)
				{
					return y.Income.CompareTo(x.Income) * -1;
				}
				return y.Income.CompareTo(x.Income);
			}
		}
	}
}
