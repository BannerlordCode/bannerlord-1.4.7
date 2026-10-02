using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard
{
	// Token: 0x020000AE RID: 174
	public class TournamentLeaderboardSortControllerVM : ViewModel
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00044304 File Offset: 0x00042504
		public TournamentLeaderboardSortControllerVM(ref MBBindingList<TournamentLeaderboardEntryItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._prizeComparer = new TournamentLeaderboardSortControllerVM.ItemPrizeComparer();
			this._nameComparer = new TournamentLeaderboardSortControllerVM.ItemNameComparer();
			this._placementComparer = new TournamentLeaderboardSortControllerVM.ItemPlacementComparer();
			this._victoriesComparer = new TournamentLeaderboardSortControllerVM.ItemVictoriesComparer();
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00044340 File Offset: 0x00042540
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

		// Token: 0x060010DD RID: 4317 RVA: 0x000443AC File Offset: 0x000425AC
		public void ExecuteSortByPrize()
		{
			int prizeState = this.PrizeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.PrizeState = (prizeState + 1) % 3;
			if (this.PrizeState == 0)
			{
				int prizeState2 = this.PrizeState;
				this.PrizeState = prizeState2 + 1;
			}
			this._prizeComparer.SetSortMode(this.PrizeState == 1);
			this._listToControl.Sort(this._prizeComparer);
			this.IsPrizeSelected = true;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00044418 File Offset: 0x00042618
		public void ExecuteSortByPlacement()
		{
			int placementState = this.PlacementState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.PlacementState = (placementState + 1) % 3;
			if (this.PlacementState == 0)
			{
				int placementState2 = this.PlacementState;
				this.PlacementState = placementState2 + 1;
			}
			this._placementComparer.SetSortMode(this.PlacementState == 1);
			this._listToControl.Sort(this._placementComparer);
			this.IsPlacementSelected = true;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00044484 File Offset: 0x00042684
		public void ExecuteSortByVictories()
		{
			int victoriesState = this.VictoriesState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.VictoriesState = (victoriesState + 1) % 3;
			if (this.VictoriesState == 0)
			{
				int victoriesState2 = this.VictoriesState;
				this.VictoriesState = victoriesState2 + 1;
			}
			this._victoriesComparer.SetSortMode(this.VictoriesState == 1);
			this._listToControl.Sort(this._victoriesComparer);
			this.IsVictoriesSelected = true;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x000444EE File Offset: 0x000426EE
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.PrizeState = (int)state;
			this.PlacementState = (int)state;
			this.VictoriesState = (int)state;
			this.IsNameSelected = false;
			this.IsVictoriesSelected = false;
			this.IsPrizeSelected = false;
			this.IsPlacementSelected = false;
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x00044528 File Offset: 0x00042728
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x00044530 File Offset: 0x00042730
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

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0004454E File Offset: 0x0004274E
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x00044556 File Offset: 0x00042756
		[DataSourceProperty]
		public int VictoriesState
		{
			get
			{
				return this._victoriesState;
			}
			set
			{
				if (value != this._victoriesState)
				{
					this._victoriesState = value;
					base.OnPropertyChangedWithValue(value, "VictoriesState");
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x00044574 File Offset: 0x00042774
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x0004457C File Offset: 0x0004277C
		[DataSourceProperty]
		public int PrizeState
		{
			get
			{
				return this._prizeState;
			}
			set
			{
				if (value != this._prizeState)
				{
					this._prizeState = value;
					base.OnPropertyChangedWithValue(value, "PrizeState");
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x0004459A File Offset: 0x0004279A
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x000445A2 File Offset: 0x000427A2
		[DataSourceProperty]
		public int PlacementState
		{
			get
			{
				return this._placementState;
			}
			set
			{
				if (value != this._placementState)
				{
					this._placementState = value;
					base.OnPropertyChangedWithValue(value, "PlacementState");
				}
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x000445C0 File Offset: 0x000427C0
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x000445C8 File Offset: 0x000427C8
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

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x000445E6 File Offset: 0x000427E6
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x000445EE File Offset: 0x000427EE
		[DataSourceProperty]
		public bool IsPrizeSelected
		{
			get
			{
				return this._isPrizeSelected;
			}
			set
			{
				if (value != this._isPrizeSelected)
				{
					this._isPrizeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsPrizeSelected");
				}
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x0004460C File Offset: 0x0004280C
		// (set) Token: 0x060010EE RID: 4334 RVA: 0x00044614 File Offset: 0x00042814
		[DataSourceProperty]
		public bool IsPlacementSelected
		{
			get
			{
				return this._isPlacementSelected;
			}
			set
			{
				if (value != this._isPlacementSelected)
				{
					this._isPlacementSelected = value;
					base.OnPropertyChangedWithValue(value, "IsPlacementSelected");
				}
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00044632 File Offset: 0x00042832
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x0004463A File Offset: 0x0004283A
		[DataSourceProperty]
		public bool IsVictoriesSelected
		{
			get
			{
				return this._isVictoriesSelected;
			}
			set
			{
				if (value != this._isVictoriesSelected)
				{
					this._isVictoriesSelected = value;
					base.OnPropertyChangedWithValue(value, "IsVictoriesSelected");
				}
			}
		}

		// Token: 0x040007B1 RID: 1969
		private readonly MBBindingList<TournamentLeaderboardEntryItemVM> _listToControl;

		// Token: 0x040007B2 RID: 1970
		private readonly TournamentLeaderboardSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040007B3 RID: 1971
		private readonly TournamentLeaderboardSortControllerVM.ItemPrizeComparer _prizeComparer;

		// Token: 0x040007B4 RID: 1972
		private readonly TournamentLeaderboardSortControllerVM.ItemPlacementComparer _placementComparer;

		// Token: 0x040007B5 RID: 1973
		private readonly TournamentLeaderboardSortControllerVM.ItemVictoriesComparer _victoriesComparer;

		// Token: 0x040007B6 RID: 1974
		private int _nameState;

		// Token: 0x040007B7 RID: 1975
		private int _prizeState;

		// Token: 0x040007B8 RID: 1976
		private int _placementState;

		// Token: 0x040007B9 RID: 1977
		private int _victoriesState;

		// Token: 0x040007BA RID: 1978
		private bool _isNameSelected;

		// Token: 0x040007BB RID: 1979
		private bool _isPrizeSelected;

		// Token: 0x040007BC RID: 1980
		private bool _isPlacementSelected;

		// Token: 0x040007BD RID: 1981
		private bool _isVictoriesSelected;

		// Token: 0x02000220 RID: 544
		public abstract class ItemComparerBase : IComparer<TournamentLeaderboardEntryItemVM>
		{
			// Token: 0x06002499 RID: 9369 RVA: 0x0008093B File Offset: 0x0007EB3B
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x0600249A RID: 9370
			public abstract int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y);

			// Token: 0x040011FA RID: 4602
			protected bool _isAcending;
		}

		// Token: 0x02000221 RID: 545
		public class ItemNameComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600249C RID: 9372 RVA: 0x0008094C File Offset: 0x0007EB4C
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x02000222 RID: 546
		public class ItemPrizeComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600249E RID: 9374 RVA: 0x00080984 File Offset: 0x0007EB84
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.PrizeValue.CompareTo(x.PrizeValue) * -1;
				}
				return y.PrizeValue.CompareTo(x.PrizeValue);
			}
		}

		// Token: 0x02000223 RID: 547
		public class ItemPlacementComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A0 RID: 9376 RVA: 0x000809CC File Offset: 0x0007EBCC
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.PlacementOnLeaderboard.CompareTo(x.PlacementOnLeaderboard) * -1;
				}
				return y.PlacementOnLeaderboard.CompareTo(x.PlacementOnLeaderboard);
			}
		}

		// Token: 0x02000224 RID: 548
		public class ItemVictoriesComparer : TournamentLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024A2 RID: 9378 RVA: 0x00080A14 File Offset: 0x0007EC14
			public override int Compare(TournamentLeaderboardEntryItemVM x, TournamentLeaderboardEntryItemVM y)
			{
				if (this._isAcending)
				{
					return y.Victories.CompareTo(x.Victories) * -1;
				}
				return y.Victories.CompareTo(x.Victories);
			}
		}
	}
}
