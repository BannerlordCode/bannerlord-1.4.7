using System;
using System.Threading.Tasks;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002C RID: 44
	public class MPLobbyMenuVM : ViewModel
	{
		// Token: 0x0600033D RID: 829 RVA: 0x0000C44D File Offset: 0x0000A64D
		public MPLobbyMenuVM(LobbyState lobbyState, Action<bool> setNavigationRestriction, Func<Task> onQuit)
		{
			this._lobbyState = lobbyState;
			this._setNavigationRestriction = setNavigationRestriction;
			this._onQuit = onQuit;
			this.RefreshValues();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C470 File Offset: 0x0000A670
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HomeText = new TextObject("{=hometab}Home", null).ToString();
			this.MatchmakingText = new TextObject("{=playgame}Play", null).ToString();
			this.ProfileText = new TextObject("{=0647tsif}Profile", null).ToString();
			this.ArmoryText = new TextObject("{=kG0xuyfE}Armory", null).ToString();
			this.PreviousPageInputKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"), true);
			this.NextPageInputKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"), true);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000C51B File Offset: 0x0000A71B
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.PreviousPageInputKey.OnFinalize();
			this.NextPageInputKey.OnFinalize();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000C539 File Offset: 0x0000A739
		public void SetPage(MPLobbyVM.LobbyPage lobbyPage)
		{
			this.PageIndex = (int)lobbyPage;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000C542 File Offset: 0x0000A742
		private void ExecuteHome()
		{
			this._lobbyState.OnActivateHome();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000C54F File Offset: 0x0000A74F
		private void ExecuteMatchmaking()
		{
			this._lobbyState.OnActivateMatchmaking();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000C55C File Offset: 0x0000A75C
		private void ExecuteCustomServer()
		{
			this._lobbyState.OnActivateCustomServer();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C569 File Offset: 0x0000A769
		private void ExecuteArmory()
		{
			this._lobbyState.OnActivateArmory();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000C576 File Offset: 0x0000A776
		private void ExecuteOptions()
		{
			this._lobbyState.OnActivateOptions();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000C583 File Offset: 0x0000A783
		private void ExecuteProfile()
		{
			this._lobbyState.OnActivateProfile();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C590 File Offset: 0x0000A790
		public async void ExecuteExit()
		{
			Func<Task> onQuit = this._onQuit;
			await ((onQuit != null) ? onQuit() : null);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000C5C9 File Offset: 0x0000A7C9
		public void OnSupportedFeaturesRefreshed(SupportedFeatures supportedFeatures)
		{
			this.IsMatchmakingSupported = supportedFeatures.SupportsFeatures(Features.Matchmaking);
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000C5FE File Offset: 0x0000A7FE
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000C606 File Offset: 0x0000A806
		[DataSourceProperty]
		public bool HasProfileNotification
		{
			get
			{
				return this._hasProfileNotification;
			}
			set
			{
				if (value != this._hasProfileNotification)
				{
					this._hasProfileNotification = value;
					base.OnPropertyChangedWithValue(value, "HasProfileNotification");
				}
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000C624 File Offset: 0x0000A824
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000C62C File Offset: 0x0000A82C
		[DataSourceProperty]
		public bool IsClanSupported
		{
			get
			{
				return this._isClanSupported;
			}
			set
			{
				if (value != this._isClanSupported)
				{
					this._isClanSupported = value;
					base.OnPropertyChangedWithValue(value, "IsClanSupported");
				}
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000C64A File Offset: 0x0000A84A
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000C652 File Offset: 0x0000A852
		[DataSourceProperty]
		public bool IsMatchmakingSupported
		{
			get
			{
				return this._isMatchmakingSupported;
			}
			set
			{
				if (value != this._isMatchmakingSupported)
				{
					this._isMatchmakingSupported = value;
					base.OnPropertyChangedWithValue(value, "IsMatchmakingSupported");
				}
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000C670 File Offset: 0x0000A870
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000C678 File Offset: 0x0000A878
		[DataSourceProperty]
		public int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
			set
			{
				if (value != this._pageIndex)
				{
					this._pageIndex = value;
					base.OnPropertyChangedWithValue(value, "PageIndex");
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000C696 File Offset: 0x0000A896
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000C69E File Offset: 0x0000A89E
		[DataSourceProperty]
		public string HomeText
		{
			get
			{
				return this._homeText;
			}
			set
			{
				if (value != this._homeText)
				{
					this._homeText = value;
					base.OnPropertyChangedWithValue<string>(value, "HomeText");
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000C6C1 File Offset: 0x0000A8C1
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000C6C9 File Offset: 0x0000A8C9
		[DataSourceProperty]
		public string MatchmakingText
		{
			get
			{
				return this._matchmakingText;
			}
			set
			{
				if (value != this._matchmakingText)
				{
					this._matchmakingText = value;
					base.OnPropertyChangedWithValue<string>(value, "MatchmakingText");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		[DataSourceProperty]
		public string ProfileText
		{
			get
			{
				return this._profileText;
			}
			set
			{
				if (value != this._profileText)
				{
					this._profileText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfileText");
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000C717 File Offset: 0x0000A917
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000C71F File Offset: 0x0000A91F
		[DataSourceProperty]
		public string ArmoryText
		{
			get
			{
				return this._armoryText;
			}
			set
			{
				if (value != this._armoryText)
				{
					this._armoryText = value;
					base.OnPropertyChangedWithValue<string>(value, "ArmoryText");
				}
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000C742 File Offset: 0x0000A942
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000C74A File Offset: 0x0000A94A
		[DataSourceProperty]
		public InputKeyItemVM PreviousPageInputKey
		{
			get
			{
				return this._previousPageInputKey;
			}
			set
			{
				if (value != this._previousPageInputKey)
				{
					this._previousPageInputKey = value;
					base.OnPropertyChanged("PreviousPageInputKey");
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000C767 File Offset: 0x0000A967
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000C76F File Offset: 0x0000A96F
		[DataSourceProperty]
		public InputKeyItemVM NextPageInputKey
		{
			get
			{
				return this._nextPageInputKey;
			}
			set
			{
				if (value != this._nextPageInputKey)
				{
					this._nextPageInputKey = value;
					base.OnPropertyChanged("NextPageInputKey");
				}
			}
		}

		// Token: 0x040001AC RID: 428
		private LobbyState _lobbyState;

		// Token: 0x040001AD RID: 429
		private readonly Action<bool> _setNavigationRestriction;

		// Token: 0x040001AE RID: 430
		private readonly Func<Task> _onQuit;

		// Token: 0x040001AF RID: 431
		private bool _isEnabled;

		// Token: 0x040001B0 RID: 432
		private bool _hasProfileNotification;

		// Token: 0x040001B1 RID: 433
		private bool _isClanSupported;

		// Token: 0x040001B2 RID: 434
		private bool _isMatchmakingSupported;

		// Token: 0x040001B3 RID: 435
		private int _pageIndex;

		// Token: 0x040001B4 RID: 436
		private string _homeText;

		// Token: 0x040001B5 RID: 437
		private string _matchmakingText;

		// Token: 0x040001B6 RID: 438
		private string _profileText;

		// Token: 0x040001B7 RID: 439
		private string _armoryText;

		// Token: 0x040001B8 RID: 440
		private InputKeyItemVM _previousPageInputKey;

		// Token: 0x040001B9 RID: 441
		private InputKeyItemVM _nextPageInputKey;
	}
}
