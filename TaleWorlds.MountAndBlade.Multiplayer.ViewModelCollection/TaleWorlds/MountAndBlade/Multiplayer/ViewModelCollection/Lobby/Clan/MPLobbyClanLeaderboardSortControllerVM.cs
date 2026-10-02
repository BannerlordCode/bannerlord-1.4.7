using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006D RID: 109
	public class MPLobbyClanLeaderboardSortControllerVM : ViewModel
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x00020893 File Offset: 0x0001EA93
		public MPLobbyClanLeaderboardSortControllerVM(ref ClanLeaderboardEntry[] listToControl, Action onSorted)
		{
			this._listToControl = listToControl;
			this._winComparer = new MPLobbyClanLeaderboardSortControllerVM.ItemWinComparer();
			this._lossComparer = new MPLobbyClanLeaderboardSortControllerVM.ItemLossComparer();
			this._nameComparer = new MPLobbyClanLeaderboardSortControllerVM.ItemNameComparer();
			this._onSorted = onSorted;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000208CC File Offset: 0x0001EACC
		private void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			Array.Sort<ClanLeaderboardEntry>(this._listToControl, this._nameComparer);
			this.IsNameSelected = true;
			this._onSorted();
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00020940 File Offset: 0x0001EB40
		private void ExecuteSortByWin()
		{
			int winState = this.WinState;
			this.SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState.Default);
			this.WinState = (winState + 1) % 3;
			if (this.WinState == 0)
			{
				this.WinState++;
			}
			this._winComparer.SetSortMode(this.WinState == 1);
			Array.Sort<ClanLeaderboardEntry>(this._listToControl, this._winComparer);
			this.IsWinSelected = true;
			this._onSorted();
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000209B4 File Offset: 0x0001EBB4
		private void ExecuteSortByLoss()
		{
			int lossState = this.LossState;
			this.SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState.Default);
			this.LossState = (lossState + 1) % 3;
			if (this.LossState == 0)
			{
				this.LossState++;
			}
			this._lossComparer.SetSortMode(this.LossState == 1);
			Array.Sort<ClanLeaderboardEntry>(this._listToControl, this._lossComparer);
			this.IsLossSelected = true;
			this._onSorted();
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00020A27 File Offset: 0x0001EC27
		private void SetAllStates(MPLobbyClanLeaderboardSortControllerVM.SortState state)
		{
			this.NameState = (int)state;
			this.WinState = (int)state;
			this.LossState = (int)state;
			this.IsNameSelected = false;
			this.IsWinSelected = false;
			this.IsLossSelected = false;
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00020A53 File Offset: 0x0001EC53
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x00020A5B File Offset: 0x0001EC5B
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

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00020A79 File Offset: 0x0001EC79
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x00020A81 File Offset: 0x0001EC81
		[DataSourceProperty]
		public int WinState
		{
			get
			{
				return this._winState;
			}
			set
			{
				if (value != this._winState)
				{
					this._winState = value;
					base.OnPropertyChangedWithValue(value, "WinState");
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x00020A9F File Offset: 0x0001EC9F
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x00020AA7 File Offset: 0x0001ECA7
		[DataSourceProperty]
		public int LossState
		{
			get
			{
				return this._lossState;
			}
			set
			{
				if (value != this._lossState)
				{
					this._lossState = value;
					base.OnPropertyChangedWithValue(value, "LossState");
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x00020AC5 File Offset: 0x0001ECC5
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x00020ACD File Offset: 0x0001ECCD
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

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00020AEB File Offset: 0x0001ECEB
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00020AF3 File Offset: 0x0001ECF3
		[DataSourceProperty]
		public bool IsWinSelected
		{
			get
			{
				return this._isWinSelected;
			}
			set
			{
				if (value != this._isWinSelected)
				{
					this._isWinSelected = value;
					base.OnPropertyChangedWithValue(value, "IsWinSelected");
				}
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00020B11 File Offset: 0x0001ED11
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x00020B19 File Offset: 0x0001ED19
		[DataSourceProperty]
		public bool IsLossSelected
		{
			get
			{
				return this._isLossSelected;
			}
			set
			{
				if (value != this._isLossSelected)
				{
					this._isLossSelected = value;
					base.OnPropertyChangedWithValue(value, "IsLossSelected");
				}
			}
		}

		// Token: 0x040004D1 RID: 1233
		private readonly ClanLeaderboardEntry[] _listToControl;

		// Token: 0x040004D2 RID: 1234
		private readonly MPLobbyClanLeaderboardSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040004D3 RID: 1235
		private readonly MPLobbyClanLeaderboardSortControllerVM.ItemWinComparer _winComparer;

		// Token: 0x040004D4 RID: 1236
		private readonly MPLobbyClanLeaderboardSortControllerVM.ItemLossComparer _lossComparer;

		// Token: 0x040004D5 RID: 1237
		private Action _onSorted;

		// Token: 0x040004D6 RID: 1238
		private int _nameState;

		// Token: 0x040004D7 RID: 1239
		private int _winState;

		// Token: 0x040004D8 RID: 1240
		private int _lossState;

		// Token: 0x040004D9 RID: 1241
		private bool _isNameSelected;

		// Token: 0x040004DA RID: 1242
		private bool _isWinSelected;

		// Token: 0x040004DB RID: 1243
		private bool _isLossSelected;

		// Token: 0x02000148 RID: 328
		private enum SortState
		{
			// Token: 0x04000999 RID: 2457
			Default,
			// Token: 0x0400099A RID: 2458
			Ascending,
			// Token: 0x0400099B RID: 2459
			Descending
		}

		// Token: 0x02000149 RID: 329
		private abstract class ItemComparerBase : IComparer<ClanLeaderboardEntry>
		{
			// Token: 0x0600124C RID: 4684 RVA: 0x000395CE File Offset: 0x000377CE
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x0600124D RID: 4685
			public abstract int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y);

			// Token: 0x0400099C RID: 2460
			protected bool _isAcending;
		}

		// Token: 0x0200014A RID: 330
		private class ItemNameComparer : MPLobbyClanLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600124F RID: 4687 RVA: 0x000395DF File Offset: 0x000377DF
			public override int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x0200014B RID: 331
		private class ItemWinComparer : MPLobbyClanLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x06001251 RID: 4689 RVA: 0x00039618 File Offset: 0x00037818
			public override int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y)
			{
				if (this._isAcending)
				{
					return y.WinCount.CompareTo(x.WinCount) * -1;
				}
				return y.WinCount.CompareTo(x.WinCount);
			}
		}

		// Token: 0x0200014C RID: 332
		private class ItemLossComparer : MPLobbyClanLeaderboardSortControllerVM.ItemComparerBase
		{
			// Token: 0x06001253 RID: 4691 RVA: 0x00039660 File Offset: 0x00037860
			public override int Compare(ClanLeaderboardEntry x, ClanLeaderboardEntry y)
			{
				if (this._isAcending)
				{
					return y.LossCount.CompareTo(x.LossCount) * -1;
				}
				return y.LossCount.CompareTo(x.LossCount);
			}
		}
	}
}
