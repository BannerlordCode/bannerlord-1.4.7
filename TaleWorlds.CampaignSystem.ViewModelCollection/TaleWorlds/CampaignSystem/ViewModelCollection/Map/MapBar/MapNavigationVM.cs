using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x0200005E RID: 94
	public class MapNavigationVM : ViewModel
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x00021DB4 File Offset: 0x0001FFB4
		public MapNavigationVM(INavigationHandler navigationHandler, Func<MapBarShortcuts> getMapBarShortcuts)
		{
			this._navigationHandler = navigationHandler;
			this._getMapBarShortcuts = getMapBarShortcuts;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.NavigationItems = new MBBindingList<MapNavigationItemVM>();
			INavigationElement[] elements = navigationHandler.GetElements();
			for (int i = 0; i < elements.Length; i++)
			{
				this.NavigationItems.Add(new MapNavigationItemVM(elements[i]));
			}
			this.RefreshValues();
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00021E20 File Offset: 0x00020020
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._shortcuts = this._getMapBarShortcuts();
			this.EncyclopediaHint = new HintViewModel(GameTexts.FindText("str_encyclopedia", null), null);
			this.CampHint = new HintViewModel(GameTexts.FindText("str_camp", null), null);
			this.FinanceHint = new HintViewModel(GameTexts.FindText("str_finance", null), null);
			this.CenterCameraHint = new HintViewModel(GameTexts.FindText("str_return_to_hero", null), null);
			this.Refresh();
			this.NavigationItems.ApplyActionOnAllItems(delegate(MapNavigationItemVM n)
			{
				n.RefreshValues();
			});
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00021ED0 File Offset: 0x000200D0
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._navigationHandler = null;
			this._getMapBarShortcuts = null;
			this.NavigationItems.ApplyActionOnAllItems(delegate(MapNavigationItemVM n)
			{
				n.OnFinalize();
			});
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00021F10 File Offset: 0x00020110
		public void Refresh()
		{
			this.RefreshStates();
			this._viewDataTracker.UpdatePartyNotification();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00021F23 File Offset: 0x00020123
		public void Tick()
		{
			this.RefreshStates();
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00021F2B File Offset: 0x0002012B
		protected virtual void RefreshStates()
		{
			this.NavigationItems.ApplyActionOnAllItems(delegate(MapNavigationItemVM n)
			{
				n.RefreshStates(false);
			});
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00021F57 File Offset: 0x00020157
		public void ExecuteOpenQuests()
		{
			this._navigationHandler.OpenQuests();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00021F64 File Offset: 0x00020164
		public void ExecuteOpenInventory()
		{
			this._navigationHandler.OpenInventory();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00021F71 File Offset: 0x00020171
		public void ExecuteOpenParty()
		{
			this._navigationHandler.OpenParty();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00021F7E File Offset: 0x0002017E
		public void ExecuteOpenCharacterDeveloper()
		{
			this._navigationHandler.OpenCharacterDeveloper();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00021F8B File Offset: 0x0002018B
		public void ExecuteOpenKingdom()
		{
			this._navigationHandler.OpenKingdom();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00021F98 File Offset: 0x00020198
		public void ExecuteOpenClan()
		{
			this._navigationHandler.OpenClan();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00021FA5 File Offset: 0x000201A5
		public void ExecuteOpenEscapeMenu()
		{
			this._navigationHandler.OpenEscapeMenu();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00021FB2 File Offset: 0x000201B2
		public void ExecuteOpenMainHeroKingdomEncyclopedia()
		{
			if (Hero.MainHero.MapFaction != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.MapFaction.EncyclopediaLink);
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00021FDE File Offset: 0x000201DE
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00021FE6 File Offset: 0x000201E6
		[DataSourceProperty]
		public MBBindingList<MapNavigationItemVM> NavigationItems
		{
			get
			{
				return this._navigationItems;
			}
			set
			{
				if (value != this._navigationItems)
				{
					this._navigationItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapNavigationItemVM>>(value, "NavigationItems");
				}
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00022004 File Offset: 0x00020204
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0002200C File Offset: 0x0002020C
		[DataSourceProperty]
		public HintViewModel FinanceHint
		{
			get
			{
				return this._financeHint;
			}
			set
			{
				if (value != this._financeHint)
				{
					this._financeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FinanceHint");
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0002202A File Offset: 0x0002022A
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00022032 File Offset: 0x00020232
		[DataSourceProperty]
		public HintViewModel EncyclopediaHint
		{
			get
			{
				return this._encyclopediaHint;
			}
			set
			{
				if (value != this._encyclopediaHint)
				{
					this._encyclopediaHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EncyclopediaHint");
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00022050 File Offset: 0x00020250
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x00022058 File Offset: 0x00020258
		[DataSourceProperty]
		public HintViewModel CenterCameraHint
		{
			get
			{
				return this._centerCameraHint;
			}
			set
			{
				if (value != this._centerCameraHint)
				{
					this._centerCameraHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CenterCameraHint");
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00022076 File Offset: 0x00020276
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0002207E File Offset: 0x0002027E
		[DataSourceProperty]
		public HintViewModel CampHint
		{
			get
			{
				return this._campHint;
			}
			set
			{
				if (value != this._campHint)
				{
					this._campHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CampHint");
				}
			}
		}

		// Token: 0x040002F4 RID: 756
		protected INavigationHandler _navigationHandler;

		// Token: 0x040002F5 RID: 757
		protected Func<MapBarShortcuts> _getMapBarShortcuts;

		// Token: 0x040002F6 RID: 758
		protected MapBarShortcuts _shortcuts;

		// Token: 0x040002F7 RID: 759
		protected readonly IViewDataTracker _viewDataTracker;

		// Token: 0x040002F8 RID: 760
		private MBBindingList<MapNavigationItemVM> _navigationItems;

		// Token: 0x040002F9 RID: 761
		private HintViewModel _encyclopediaHint;

		// Token: 0x040002FA RID: 762
		private HintViewModel _financeHint;

		// Token: 0x040002FB RID: 763
		private HintViewModel _centerCameraHint;

		// Token: 0x040002FC RID: 764
		private HintViewModel _campHint;
	}
}
