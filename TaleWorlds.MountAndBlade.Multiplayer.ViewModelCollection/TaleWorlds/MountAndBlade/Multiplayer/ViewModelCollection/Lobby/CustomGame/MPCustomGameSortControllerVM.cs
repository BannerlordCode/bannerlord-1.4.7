using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x02000060 RID: 96
	public class MPCustomGameSortControllerVM : ViewModel
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0001C910 File Offset: 0x0001AB10
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x0001C918 File Offset: 0x0001AB18
		public MPCustomGameSortControllerVM.CustomServerSortOption? CurrentSortOption { get; private set; }

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001C924 File Offset: 0x0001AB24
		public MPCustomGameSortControllerVM(ref MBBindingList<MPCustomGameItemVM> listToControl, MPCustomGameVM.CustomGameMode customGameMode)
		{
			this._listToControl = listToControl;
			this.IsPremadeMatchesList = customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame;
			this.IsPingInfoAvailable = MPCustomGameVM.IsPingInfoAvailable && !this.IsPremadeMatchesList;
			this._numberOfSortOptions = 11;
			this._sortComparers = new MPCustomGameSortControllerVM.ItemComparer[this._numberOfSortOptions];
			for (MPCustomGameSortControllerVM.CustomServerSortOption customServerSortOption = MPCustomGameSortControllerVM.CustomServerSortOption.Name; customServerSortOption < MPCustomGameSortControllerVM.CustomServerSortOption.SortOptionsEndExclusive; customServerSortOption++)
			{
				MPCustomGameSortControllerVM.ItemComparer sortComparer = this.GetSortComparer(customServerSortOption);
				if (sortComparer != null)
				{
					this._sortComparers[(int)customServerSortOption] = sortComparer;
				}
				else
				{
					Debug.FailedAssert("No valid comparer for custom server sort option: " + customServerSortOption.ToString(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameSortControllerVM.cs", ".ctor", 59);
				}
			}
			this.RefreshValues();
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001C9CC File Offset: 0x0001ABCC
		private MPCustomGameSortControllerVM.ItemComparer GetSortComparer(MPCustomGameSortControllerVM.CustomServerSortOption option)
		{
			switch (option)
			{
			case MPCustomGameSortControllerVM.CustomServerSortOption.SortOptionsBeginExclusive:
			case MPCustomGameSortControllerVM.CustomServerSortOption.SortOptionsEndExclusive:
				return null;
			case MPCustomGameSortControllerVM.CustomServerSortOption.Name:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.ServerNameComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.GameType:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.GameTypeComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.PlayerCount:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PlayerCountComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.PasswordProtection:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PasswordComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.FirstFaction:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.FirstFactionComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.SecondFaction:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.SecondFactionComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Region:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.RegionComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.PremadeMatchType:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PremadeMatchTypeComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Host:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.HostComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Ping:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PingComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Favorite:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.FavoriteComparer();
			default:
				return null;
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001CAE3 File Offset: 0x0001ACE3
		public void InitializeWithSortState(MPCustomGameSortControllerVM.CustomServerSortOption? sortOption, MPCustomGameSortControllerVM.SortState sortState = MPCustomGameSortControllerVM.SortState.Default)
		{
			this.SetSortOption(sortOption);
			this.CurrentSortState = (int)sortState;
			this.SortByCurrentState();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001CAFC File Offset: 0x0001ACFC
		private void SetSortOption(MPCustomGameSortControllerVM.CustomServerSortOption? sortOption)
		{
			MPCustomGameSortControllerVM.CustomServerSortOption? currentSortOption = this.CurrentSortOption;
			MPCustomGameSortControllerVM.CustomServerSortOption? customServerSortOption = sortOption;
			if ((currentSortOption.GetValueOrDefault() == customServerSortOption.GetValueOrDefault()) & (currentSortOption != null == (customServerSortOption != null)))
			{
				return;
			}
			this.CurrentSortOption = sortOption;
			this.RefreshSelectedStates();
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001CB44 File Offset: 0x0001AD44
		public void SortByCurrentState()
		{
			if (this.CurrentSortOption == null)
			{
				return;
			}
			MPCustomGameSortControllerVM.ItemComparer sortComparer = this.GetSortComparer(this.CurrentSortOption.Value);
			MPCustomGameSortControllerVM.SortState currentSortState = (MPCustomGameSortControllerVM.SortState)this.CurrentSortState;
			if (sortComparer != null)
			{
				sortComparer.SetSortMode(currentSortState != MPCustomGameSortControllerVM.SortState.Descending);
				this._listToControl.Sort(sortComparer);
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001CB9C File Offset: 0x0001AD9C
		private void SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption option)
		{
			MPCustomGameSortControllerVM.CustomServerSortOption? currentSortOption = this.CurrentSortOption;
			if (!((option == currentSortOption.GetValueOrDefault()) & (currentSortOption != null)))
			{
				this.CurrentSortState = 1;
			}
			else
			{
				this.CurrentSortState = (this.CurrentSortState + 1) % 3;
			}
			this.SetSortOption(new MPCustomGameSortControllerVM.CustomServerSortOption?(option));
			this.SortByCurrentState();
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001CBEF File Offset: 0x0001ADEF
		public void ExecuteSortByFavorites()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Favorite);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001CBF9 File Offset: 0x0001ADF9
		public void ExecuteSortByServerName()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Name);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001CC02 File Offset: 0x0001AE02
		public void ExecuteSortByGameType()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.GameType);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001CC0B File Offset: 0x0001AE0B
		public void ExecuteSortByPlayerCount()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.PlayerCount);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001CC14 File Offset: 0x0001AE14
		public void ExecuteSortByPassword()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.PasswordProtection);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001CC1D File Offset: 0x0001AE1D
		public void ExecuteSortByFirstFaction()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.FirstFaction);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001CC26 File Offset: 0x0001AE26
		public void ExecuteSortBySecondFaction()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.SecondFaction);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001CC2F File Offset: 0x0001AE2F
		public void ExecuteSortByRegion()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Region);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001CC38 File Offset: 0x0001AE38
		public void ExecuteSortByPremadeMatchType()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.PremadeMatchType);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001CC41 File Offset: 0x0001AE41
		public void ExecuteSortByHost()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Host);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001CC4A File Offset: 0x0001AE4A
		public void ExecuteSortByPing()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Ping);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001CC54 File Offset: 0x0001AE54
		private void RefreshSelectedStates()
		{
			MPCustomGameSortControllerVM.CustomServerSortOption? customServerSortOption = this.CurrentSortOption;
			MPCustomGameSortControllerVM.CustomServerSortOption customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Favorite;
			this.IsFavoritesSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Name;
			this.IsServerNameSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.GameType;
			this.IsGameTypeSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.PlayerCount;
			this.IsPlayerCountSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.PasswordProtection;
			this.IsPasswordSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.FirstFaction;
			this.IsFirstFactionSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.SecondFaction;
			this.IsSecondFactionSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Region;
			this.IsRegionSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.PremadeMatchType;
			this.IsPremadeMatchTypeSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Host;
			this.IsHostSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Ping;
			this.IsPingSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x0001CDCE File Offset: 0x0001AFCE
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x0001CDD6 File Offset: 0x0001AFD6
		[DataSourceProperty]
		public bool IsPremadeMatchesList
		{
			get
			{
				return this._isPremadeMatchesList;
			}
			set
			{
				if (value != this._isPremadeMatchesList)
				{
					this._isPremadeMatchesList = value;
					base.OnPropertyChanged("IsPremadeMatchesList");
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x0001CDF3 File Offset: 0x0001AFF3
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x0001CDFB File Offset: 0x0001AFFB
		[DataSourceProperty]
		public bool IsPingInfoAvailable
		{
			get
			{
				return this._isPingInfoAvailable;
			}
			set
			{
				if (value != this._isPingInfoAvailable)
				{
					this._isPingInfoAvailable = value;
					base.OnPropertyChanged("IsPingInfoAvailable");
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0001CE18 File Offset: 0x0001B018
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x0001CE20 File Offset: 0x0001B020
		[DataSourceProperty]
		public int CurrentSortState
		{
			get
			{
				return this._currentSortState;
			}
			set
			{
				if (value != this._currentSortState)
				{
					this._currentSortState = value;
					base.OnPropertyChanged("CurrentSortState");
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x0001CE3D File Offset: 0x0001B03D
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x0001CE45 File Offset: 0x0001B045
		[DataSourceProperty]
		public bool IsFavoritesSelected
		{
			get
			{
				return this._isFavoritesSelected;
			}
			set
			{
				if (value != this._isFavoritesSelected)
				{
					this._isFavoritesSelected = value;
					base.OnPropertyChanged("IsFavoritesSelected");
				}
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0001CE62 File Offset: 0x0001B062
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x0001CE6A File Offset: 0x0001B06A
		[DataSourceProperty]
		public bool IsServerNameSelected
		{
			get
			{
				return this._isServerNameSelected;
			}
			set
			{
				if (value != this._isServerNameSelected)
				{
					this._isServerNameSelected = value;
					base.OnPropertyChanged("IsServerNameSelected");
				}
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0001CE87 File Offset: 0x0001B087
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x0001CE8F File Offset: 0x0001B08F
		[DataSourceProperty]
		public bool IsPasswordSelected
		{
			get
			{
				return this._isPasswordSelected;
			}
			set
			{
				if (value != this._isPasswordSelected)
				{
					this._isPasswordSelected = value;
					base.OnPropertyChanged("IsPasswordSelected");
				}
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x0001CEAC File Offset: 0x0001B0AC
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x0001CEB4 File Offset: 0x0001B0B4
		[DataSourceProperty]
		public bool IsPlayerCountSelected
		{
			get
			{
				return this._isPlayerCountSelected;
			}
			set
			{
				if (value != this._isPlayerCountSelected)
				{
					this._isPlayerCountSelected = value;
					base.OnPropertyChanged("IsPlayerCountSelected");
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0001CED1 File Offset: 0x0001B0D1
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x0001CED9 File Offset: 0x0001B0D9
		[DataSourceProperty]
		public bool IsFirstFactionSelected
		{
			get
			{
				return this._isFirstFactionSelected;
			}
			set
			{
				if (value != this._isFirstFactionSelected)
				{
					this._isFirstFactionSelected = value;
					base.OnPropertyChanged("IsFirstFactionSelected");
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0001CEF6 File Offset: 0x0001B0F6
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0001CEFE File Offset: 0x0001B0FE
		[DataSourceProperty]
		public bool IsGameTypeSelected
		{
			get
			{
				return this._isGameTypeSelected;
			}
			set
			{
				if (value != this._isGameTypeSelected)
				{
					this._isGameTypeSelected = value;
					base.OnPropertyChanged("IsGameTypeSelected");
				}
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0001CF1B File Offset: 0x0001B11B
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0001CF23 File Offset: 0x0001B123
		[DataSourceProperty]
		public bool IsSecondFactionSelected
		{
			get
			{
				return this._isSecondFactionSelected;
			}
			set
			{
				if (value != this._isSecondFactionSelected)
				{
					this._isSecondFactionSelected = value;
					base.OnPropertyChanged("IsSecondFactionSelected");
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0001CF40 File Offset: 0x0001B140
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x0001CF48 File Offset: 0x0001B148
		[DataSourceProperty]
		public bool IsRegionSelected
		{
			get
			{
				return this._isRegionSelected;
			}
			set
			{
				if (value != this._isRegionSelected)
				{
					this._isRegionSelected = value;
					base.OnPropertyChanged("IsRegionSelected");
				}
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0001CF65 File Offset: 0x0001B165
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0001CF6D File Offset: 0x0001B16D
		[DataSourceProperty]
		public bool IsPremadeMatchTypeSelected
		{
			get
			{
				return this._isPremadeMatchTypeSelected;
			}
			set
			{
				if (value != this._isPremadeMatchTypeSelected)
				{
					this._isPremadeMatchTypeSelected = value;
					base.OnPropertyChanged("IsPremadeMatchTypeSelected");
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001CF8A File Offset: 0x0001B18A
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0001CF92 File Offset: 0x0001B192
		[DataSourceProperty]
		public bool IsHostSelected
		{
			get
			{
				return this._isHostSelected;
			}
			set
			{
				if (value != this._isHostSelected)
				{
					this._isHostSelected = value;
					base.OnPropertyChanged("IsHostSelected");
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0001CFAF File Offset: 0x0001B1AF
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0001CFB7 File Offset: 0x0001B1B7
		[DataSourceProperty]
		public bool IsPingSelected
		{
			get
			{
				return this._isPingSelected;
			}
			set
			{
				if (value != this._isPingSelected)
				{
					this._isPingSelected = value;
					base.OnPropertyChanged("IsPingSelected");
				}
			}
		}

		// Token: 0x0400042C RID: 1068
		private MBBindingList<MPCustomGameItemVM> _listToControl;

		// Token: 0x0400042E RID: 1070
		private readonly MPCustomGameSortControllerVM.ItemComparer[] _sortComparers;

		// Token: 0x0400042F RID: 1071
		private readonly int _numberOfSortOptions;

		// Token: 0x04000430 RID: 1072
		private bool _isPremadeMatchesList;

		// Token: 0x04000431 RID: 1073
		private bool _isPingInfoAvailable;

		// Token: 0x04000432 RID: 1074
		private int _currentSortState;

		// Token: 0x04000433 RID: 1075
		private bool _isFavoritesSelected;

		// Token: 0x04000434 RID: 1076
		private bool _isServerNameSelected;

		// Token: 0x04000435 RID: 1077
		private bool _isGameTypeSelected;

		// Token: 0x04000436 RID: 1078
		private bool _isPlayerCountSelected;

		// Token: 0x04000437 RID: 1079
		private bool _isPasswordSelected;

		// Token: 0x04000438 RID: 1080
		private bool _isFirstFactionSelected;

		// Token: 0x04000439 RID: 1081
		private bool _isSecondFactionSelected;

		// Token: 0x0400043A RID: 1082
		private bool _isRegionSelected;

		// Token: 0x0400043B RID: 1083
		private bool _isPremadeMatchTypeSelected;

		// Token: 0x0400043C RID: 1084
		private bool _isHostSelected;

		// Token: 0x0400043D RID: 1085
		private bool _isPingSelected;

		// Token: 0x0200012C RID: 300
		public enum SortState
		{
			// Token: 0x04000956 RID: 2390
			Default,
			// Token: 0x04000957 RID: 2391
			Ascending,
			// Token: 0x04000958 RID: 2392
			Descending
		}

		// Token: 0x0200012D RID: 301
		public enum CustomServerSortOption
		{
			// Token: 0x0400095A RID: 2394
			SortOptionsBeginExclusive = -1,
			// Token: 0x0400095B RID: 2395
			Name,
			// Token: 0x0400095C RID: 2396
			GameType,
			// Token: 0x0400095D RID: 2397
			PlayerCount,
			// Token: 0x0400095E RID: 2398
			PasswordProtection,
			// Token: 0x0400095F RID: 2399
			FirstFaction,
			// Token: 0x04000960 RID: 2400
			SecondFaction,
			// Token: 0x04000961 RID: 2401
			Region,
			// Token: 0x04000962 RID: 2402
			PremadeMatchType,
			// Token: 0x04000963 RID: 2403
			Host,
			// Token: 0x04000964 RID: 2404
			Ping,
			// Token: 0x04000965 RID: 2405
			Favorite,
			// Token: 0x04000966 RID: 2406
			SortOptionsEndExclusive
		}

		// Token: 0x0200012E RID: 302
		private abstract class ItemComparer : IComparer<MPCustomGameItemVM>
		{
			// Token: 0x06001214 RID: 4628 RVA: 0x00038866 File Offset: 0x00036A66
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06001215 RID: 4629
			public abstract int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y);

			// Token: 0x04000967 RID: 2407
			protected bool _isAscending;
		}

		// Token: 0x0200012F RID: 303
		private class ServerNameComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001217 RID: 4631 RVA: 0x00038877 File Offset: 0x00036A77
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.NameText.CompareTo(x.NameText) * -1;
				}
				return y.NameText.CompareTo(x.NameText);
			}
		}

		// Token: 0x02000130 RID: 304
		private class GameTypeComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001219 RID: 4633 RVA: 0x000388AE File Offset: 0x00036AAE
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.GameTypeText.CompareTo(x.GameTypeText) * -1;
				}
				return y.GameTypeText.CompareTo(x.GameTypeText);
			}
		}

		// Token: 0x02000131 RID: 305
		private class PlayerCountComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x0600121B RID: 4635 RVA: 0x000388E8 File Offset: 0x00036AE8
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.PlayerCount.CompareTo(x.PlayerCount) * -1;
				}
				return y.PlayerCount.CompareTo(x.PlayerCount);
			}
		}

		// Token: 0x02000132 RID: 306
		private class PasswordComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x0600121D RID: 4637 RVA: 0x00038930 File Offset: 0x00036B30
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.IsPasswordProtected.CompareTo(x.IsPasswordProtected) * -1;
				}
				return y.IsPasswordProtected.CompareTo(x.IsPasswordProtected);
			}
		}

		// Token: 0x02000133 RID: 307
		private class FirstFactionComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x0600121F RID: 4639 RVA: 0x00038978 File Offset: 0x00036B78
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.FirstFactionName.CompareTo(x.FirstFactionName) * -1;
				}
				return y.FirstFactionName.CompareTo(x.FirstFactionName);
			}
		}

		// Token: 0x02000134 RID: 308
		private class SecondFactionComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001221 RID: 4641 RVA: 0x000389AF File Offset: 0x00036BAF
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.SecondFactionName.CompareTo(x.SecondFactionName) * -1;
				}
				return y.SecondFactionName.CompareTo(x.SecondFactionName);
			}
		}

		// Token: 0x02000135 RID: 309
		private class RegionComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001223 RID: 4643 RVA: 0x000389E6 File Offset: 0x00036BE6
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.RegionName.CompareTo(x.RegionName) * -1;
				}
				return y.RegionName.CompareTo(x.RegionName);
			}
		}

		// Token: 0x02000136 RID: 310
		private class PremadeMatchTypeComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001225 RID: 4645 RVA: 0x00038A1D File Offset: 0x00036C1D
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.PremadeMatchTypeText.CompareTo(x.PremadeMatchTypeText) * -1;
				}
				return y.PremadeMatchTypeText.CompareTo(x.PremadeMatchTypeText);
			}
		}

		// Token: 0x02000137 RID: 311
		private class HostComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001227 RID: 4647 RVA: 0x00038A54 File Offset: 0x00036C54
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (!(y.HostText == x.HostText))
				{
					string hostText = y.HostText;
					return ((hostText != null) ? hostText.CompareTo(x.HostText) : (-1)) * (this._isAscending ? (-1) : 1);
				}
				return 0;
			}
		}

		// Token: 0x02000138 RID: 312
		private class PingComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x06001229 RID: 4649 RVA: 0x00038A98 File Offset: 0x00036C98
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				int num = (this._isAscending ? (-1) : 1);
				if (y.PingText == x.PingText)
				{
					return 0;
				}
				if (y.PingText == "-" || y.PingText == null)
				{
					return num;
				}
				if (x.PingText == "-" || x.PingText == null)
				{
					return num * -1;
				}
				return (int)(long.Parse(y.PingText) - long.Parse(x.PingText)) * num;
			}
		}

		// Token: 0x02000139 RID: 313
		private class FavoriteComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x0600122B RID: 4651 RVA: 0x00038B24 File Offset: 0x00036D24
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				return y.IsFavorite.CompareTo(x.IsFavorite) * (this._isAscending ? (-1) : 1);
			}
		}
	}
}
