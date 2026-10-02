using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000014 RID: 20
	public class SPScoreboardSortControllerVM : ViewModel
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00005E50 File Offset: 0x00004050
		public SPScoreboardSortControllerVM(ref MBBindingList<SPScoreboardPartyVM> listToControl)
		{
			this._listToControl = listToControl;
			this._remainingComparer = new SPScoreboardSortControllerVM.ItemRemainingComparer();
			this._killComparer = new SPScoreboardSortControllerVM.ItemKillComparer();
			this._upgradeComparer = new SPScoreboardSortControllerVM.ItemUpgradeComparer();
			this._deadComparer = new SPScoreboardSortControllerVM.ItemDeadComparer();
			this._woundedComparer = new SPScoreboardSortControllerVM.ItemWoundedComparer();
			this._routedComparer = new SPScoreboardSortControllerVM.ItemRoutedComparer();
			this._memberComparer = new SPScoreboardSortControllerVM.ItemMemberComparer();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00005EB8 File Offset: 0x000040B8
		public void ExecuteSortByRemaining()
		{
			int remainingState = this.RemainingState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.RemainingState = (remainingState + 1) % 3;
			this._remainingComparer.SetSortMode(this.RemainingState == 1);
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._remainingComparer;
			if (this.RemainingState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsRemainingSelected = this.RemainingState != 0;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005F5C File Offset: 0x0000415C
		public void ExecuteSortByKill()
		{
			int killState = this.KillState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.KillState = (killState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._killComparer;
			if (this.KillState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._killComparer.SetSortMode(this.KillState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsKillSelected = this.KillState != 0;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006000 File Offset: 0x00004200
		public void ExecuteSortByUpgrade()
		{
			int upgradeState = this.UpgradeState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.UpgradeState = (upgradeState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._upgradeComparer;
			if (this.UpgradeState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._upgradeComparer.SetSortMode(this.UpgradeState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsUpgradeSelected = this.UpgradeState != 0;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000060A4 File Offset: 0x000042A4
		public void ExecuteSortByDead()
		{
			int deadState = this.DeadState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.DeadState = (deadState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._deadComparer;
			if (this.DeadState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._deadComparer.SetSortMode(this.DeadState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsDeadSelected = this.DeadState != 0;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006148 File Offset: 0x00004348
		public void ExecuteSortByWounded()
		{
			int woundedState = this.WoundedState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.WoundedState = (woundedState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._woundedComparer;
			if (this.WoundedState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._woundedComparer.SetSortMode(this.WoundedState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsWoundedSelected = this.WoundedState != 0;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000061EC File Offset: 0x000043EC
		public void ExecuteSortByRouted()
		{
			int routedState = this.RoutedState;
			this.SetAllStates(SPScoreboardSortControllerVM.SortState.Default);
			this.RoutedState = (routedState + 1) % 3;
			SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase scoreboardUnitItemComparerBase = this._routedComparer;
			if (this.RoutedState == 0)
			{
				scoreboardUnitItemComparerBase = this._memberComparer;
			}
			this._routedComparer.SetSortMode(this.RoutedState == 1);
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._listToControl)
			{
				spscoreboardPartyVM.Members.Sort(scoreboardUnitItemComparerBase);
			}
			this.IsRoutedSelected = this.RoutedState != 0;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006290 File Offset: 0x00004490
		private void SetAllStates(SPScoreboardSortControllerVM.SortState state)
		{
			this.RemainingState = (int)state;
			this.KillState = (int)state;
			this.UpgradeState = (int)state;
			this.DeadState = (int)state;
			this.WoundedState = (int)state;
			this.RoutedState = (int)state;
			this.IsRemainingSelected = false;
			this.IsKillSelected = false;
			this.IsUpgradeSelected = false;
			this.IsDeadSelected = false;
			this.IsWoundedSelected = false;
			this.IsRoutedSelected = false;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000062F1 File Offset: 0x000044F1
		// (set) Token: 0x06000184 RID: 388 RVA: 0x000062F9 File Offset: 0x000044F9
		[DataSourceProperty]
		public int RemainingState
		{
			get
			{
				return this._remainingState;
			}
			set
			{
				if (value != this._remainingState)
				{
					this._remainingState = value;
					base.OnPropertyChanged("RemainingState");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006316 File Offset: 0x00004516
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000631E File Offset: 0x0000451E
		[DataSourceProperty]
		public bool IsRemainingSelected
		{
			get
			{
				return this._isRemainingSelected;
			}
			set
			{
				if (value != this._isRemainingSelected)
				{
					this._isRemainingSelected = value;
					base.OnPropertyChanged("IsRemainingSelected");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000633B File Offset: 0x0000453B
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006343 File Offset: 0x00004543
		[DataSourceProperty]
		public int KillState
		{
			get
			{
				return this._killState;
			}
			set
			{
				if (value != this._killState)
				{
					this._killState = value;
					base.OnPropertyChanged("KillState");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006360 File Offset: 0x00004560
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00006368 File Offset: 0x00004568
		[DataSourceProperty]
		public bool IsKillSelected
		{
			get
			{
				return this._isKillSelected;
			}
			set
			{
				if (value != this._isKillSelected)
				{
					this._isKillSelected = value;
					base.OnPropertyChanged("IsKillSelected");
				}
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006385 File Offset: 0x00004585
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000638D File Offset: 0x0000458D
		[DataSourceProperty]
		public int UpgradeState
		{
			get
			{
				return this._upgradeState;
			}
			set
			{
				if (value != this._upgradeState)
				{
					this._upgradeState = value;
					base.OnPropertyChanged("UpgradeState");
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000063AA File Offset: 0x000045AA
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000063B2 File Offset: 0x000045B2
		[DataSourceProperty]
		public bool IsUpgradeSelected
		{
			get
			{
				return this._isUpgradeSelected;
			}
			set
			{
				if (value != this._isUpgradeSelected)
				{
					this._isUpgradeSelected = value;
					base.OnPropertyChanged("IsUpgradeSelected");
				}
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000063CF File Offset: 0x000045CF
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000063D7 File Offset: 0x000045D7
		[DataSourceProperty]
		public int DeadState
		{
			get
			{
				return this._deadState;
			}
			set
			{
				if (value != this._deadState)
				{
					this._deadState = value;
					base.OnPropertyChanged("DeadState");
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000063F4 File Offset: 0x000045F4
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000063FC File Offset: 0x000045FC
		[DataSourceProperty]
		public bool IsDeadSelected
		{
			get
			{
				return this._isDeadSelected;
			}
			set
			{
				if (value != this._isDeadSelected)
				{
					this._isDeadSelected = value;
					base.OnPropertyChanged("IsDeadSelected");
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00006419 File Offset: 0x00004619
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00006421 File Offset: 0x00004621
		[DataSourceProperty]
		public int WoundedState
		{
			get
			{
				return this._woundedState;
			}
			set
			{
				if (value != this._woundedState)
				{
					this._woundedState = value;
					base.OnPropertyChanged("WoundedState");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000643E File Offset: 0x0000463E
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00006446 File Offset: 0x00004646
		[DataSourceProperty]
		public bool IsWoundedSelected
		{
			get
			{
				return this._isWoundedSelected;
			}
			set
			{
				if (value != this._isWoundedSelected)
				{
					this._isWoundedSelected = value;
					base.OnPropertyChanged("IsWoundedSelected");
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006463 File Offset: 0x00004663
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000646B File Offset: 0x0000466B
		[DataSourceProperty]
		public int RoutedState
		{
			get
			{
				return this._routedState;
			}
			set
			{
				if (value != this._routedState)
				{
					this._routedState = value;
					base.OnPropertyChanged("RoutedState");
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006488 File Offset: 0x00004688
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00006490 File Offset: 0x00004690
		[DataSourceProperty]
		public bool IsRoutedSelected
		{
			get
			{
				return this._isRoutedSelected;
			}
			set
			{
				if (value != this._isRoutedSelected)
				{
					this._isRoutedSelected = value;
					base.OnPropertyChanged("IsRoutedSelected");
				}
			}
		}

		// Token: 0x040000AE RID: 174
		private readonly MBBindingList<SPScoreboardPartyVM> _listToControl;

		// Token: 0x040000AF RID: 175
		private readonly SPScoreboardSortControllerVM.ItemRemainingComparer _remainingComparer;

		// Token: 0x040000B0 RID: 176
		private readonly SPScoreboardSortControllerVM.ItemKillComparer _killComparer;

		// Token: 0x040000B1 RID: 177
		private readonly SPScoreboardSortControllerVM.ItemUpgradeComparer _upgradeComparer;

		// Token: 0x040000B2 RID: 178
		private readonly SPScoreboardSortControllerVM.ItemDeadComparer _deadComparer;

		// Token: 0x040000B3 RID: 179
		private readonly SPScoreboardSortControllerVM.ItemWoundedComparer _woundedComparer;

		// Token: 0x040000B4 RID: 180
		private readonly SPScoreboardSortControllerVM.ItemRoutedComparer _routedComparer;

		// Token: 0x040000B5 RID: 181
		private readonly SPScoreboardSortControllerVM.ItemMemberComparer _memberComparer;

		// Token: 0x040000B6 RID: 182
		private int _remainingState;

		// Token: 0x040000B7 RID: 183
		private bool _isRemainingSelected;

		// Token: 0x040000B8 RID: 184
		private int _killState;

		// Token: 0x040000B9 RID: 185
		private bool _isKillSelected;

		// Token: 0x040000BA RID: 186
		private int _upgradeState;

		// Token: 0x040000BB RID: 187
		private bool _isUpgradeSelected;

		// Token: 0x040000BC RID: 188
		private int _deadState;

		// Token: 0x040000BD RID: 189
		private bool _isDeadSelected;

		// Token: 0x040000BE RID: 190
		private int _woundedState;

		// Token: 0x040000BF RID: 191
		private bool _isWoundedSelected;

		// Token: 0x040000C0 RID: 192
		private int _routedState;

		// Token: 0x040000C1 RID: 193
		private bool _isRoutedSelected;

		// Token: 0x0200009D RID: 157
		private enum SortState
		{
			// Token: 0x04000568 RID: 1384
			Default,
			// Token: 0x04000569 RID: 1385
			Ascending,
			// Token: 0x0400056A RID: 1386
			Descending
		}

		// Token: 0x0200009E RID: 158
		public abstract class ScoreboardUnitItemComparerBase : IComparer<SPScoreboardUnitVM>
		{
			// Token: 0x06000BBA RID: 3002 RVA: 0x00028F54 File Offset: 0x00027154
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06000BBB RID: 3003
			public abstract int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y);

			// Token: 0x0400056B RID: 1387
			protected bool _isAscending;
		}

		// Token: 0x0200009F RID: 159
		public class ItemRemainingComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BBD RID: 3005 RVA: 0x00028F68 File Offset: 0x00027168
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Remaining.CompareTo(x.Score.Remaining) * -1;
				}
				return y.Score.Remaining.CompareTo(x.Score.Remaining);
			}
		}

		// Token: 0x020000A0 RID: 160
		public class ItemKillComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BBF RID: 3007 RVA: 0x00028FC4 File Offset: 0x000271C4
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Kill.CompareTo(x.Score.Kill) * -1;
				}
				return y.Score.Kill.CompareTo(x.Score.Kill);
			}
		}

		// Token: 0x020000A1 RID: 161
		public class ItemUpgradeComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC1 RID: 3009 RVA: 0x00029020 File Offset: 0x00027220
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.ReadyToUpgrade.CompareTo(x.Score.ReadyToUpgrade) * -1;
				}
				return y.Score.ReadyToUpgrade.CompareTo(x.Score.ReadyToUpgrade);
			}
		}

		// Token: 0x020000A2 RID: 162
		public class ItemDeadComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC3 RID: 3011 RVA: 0x0002907C File Offset: 0x0002727C
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Dead.CompareTo(x.Score.Dead) * -1;
				}
				return y.Score.Dead.CompareTo(x.Score.Dead);
			}
		}

		// Token: 0x020000A3 RID: 163
		public class ItemWoundedComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC5 RID: 3013 RVA: 0x000290D8 File Offset: 0x000272D8
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Wounded.CompareTo(x.Score.Wounded) * -1;
				}
				return y.Score.Wounded.CompareTo(x.Score.Wounded);
			}
		}

		// Token: 0x020000A4 RID: 164
		public class ItemRoutedComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC7 RID: 3015 RVA: 0x00029134 File Offset: 0x00027334
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (this._isAscending)
				{
					return y.Score.Routed.CompareTo(x.Score.Routed) * -1;
				}
				return y.Score.Routed.CompareTo(x.Score.Routed);
			}
		}

		// Token: 0x020000A5 RID: 165
		public class ItemMemberComparer : SPScoreboardSortControllerVM.ScoreboardUnitItemComparerBase
		{
			// Token: 0x06000BC9 RID: 3017 RVA: 0x00029190 File Offset: 0x00027390
			public override int Compare(SPScoreboardUnitVM x, SPScoreboardUnitVM y)
			{
				if (x.Character.IsPlayerCharacter && !y.Character.IsPlayerCharacter)
				{
					return -1;
				}
				if (!x.Character.IsPlayerCharacter && y.Character.IsPlayerCharacter)
				{
					return 1;
				}
				if (x.IsHero && !y.IsHero)
				{
					return -1;
				}
				if (!x.IsHero && y.IsHero)
				{
					return 1;
				}
				return x.Character.Name.ToString().CompareTo(y.Character.Name.ToString());
			}
		}
	}
}
