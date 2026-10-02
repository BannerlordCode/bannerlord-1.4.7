using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000074 RID: 116
	public class KingdomWarSortControllerVM : ViewModel
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x00029F56 File Offset: 0x00028156
		public KingdomWarSortControllerVM(ref MBBindingList<KingdomWarItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._scoreComparer = new KingdomWarSortControllerVM.ItemScoreComparer();
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00029F74 File Offset: 0x00028174
		private void ExecuteSortByScore()
		{
			int scoreState = this.ScoreState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ScoreState = (scoreState + 1) % 3;
			if (this.ScoreState == 0)
			{
				int scoreState2 = this.ScoreState;
				this.ScoreState = scoreState2 + 1;
			}
			this._scoreComparer.SetSortMode(this.ScoreState == 1);
			this._listToControl.Sort(this._scoreComparer);
			this.IsScoreSelected = true;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00029FDE File Offset: 0x000281DE
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.ScoreState = (int)state;
			this.IsScoreSelected = false;
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00029FEE File Offset: 0x000281EE
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x00029FF6 File Offset: 0x000281F6
		[DataSourceProperty]
		public int ScoreState
		{
			get
			{
				return this._scoreState;
			}
			set
			{
				if (value != this._scoreState)
				{
					this._scoreState = value;
					base.OnPropertyChangedWithValue(value, "ScoreState");
				}
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0002A014 File Offset: 0x00028214
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0002A01C File Offset: 0x0002821C
		[DataSourceProperty]
		public bool IsScoreSelected
		{
			get
			{
				return this._isScoreSelected;
			}
			set
			{
				if (value != this._isScoreSelected)
				{
					this._isScoreSelected = value;
					base.OnPropertyChangedWithValue(value, "IsScoreSelected");
				}
			}
		}

		// Token: 0x0400042E RID: 1070
		private readonly MBBindingList<KingdomWarItemVM> _listToControl;

		// Token: 0x0400042F RID: 1071
		private readonly KingdomWarSortControllerVM.ItemScoreComparer _scoreComparer;

		// Token: 0x04000430 RID: 1072
		private int _scoreState;

		// Token: 0x04000431 RID: 1073
		private bool _isScoreSelected;

		// Token: 0x020001D7 RID: 471
		public abstract class ItemComparerBase : IComparer<KingdomWarItemVM>
		{
			// Token: 0x060023B4 RID: 9140 RVA: 0x0007F470 File Offset: 0x0007D670
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060023B5 RID: 9141
			public abstract int Compare(KingdomWarItemVM x, KingdomWarItemVM y);

			// Token: 0x04001125 RID: 4389
			protected bool _isAscending;
		}

		// Token: 0x020001D8 RID: 472
		public class ItemScoreComparer : KingdomWarSortControllerVM.ItemComparerBase
		{
			// Token: 0x060023B7 RID: 9143 RVA: 0x0007F484 File Offset: 0x0007D684
			public override int Compare(KingdomWarItemVM x, KingdomWarItemVM y)
			{
				if (this._isAscending)
				{
					return x.Score.CompareTo(y.Score);
				}
				return x.Score.CompareTo(y.Score) * -1;
			}
		}
	}
}
