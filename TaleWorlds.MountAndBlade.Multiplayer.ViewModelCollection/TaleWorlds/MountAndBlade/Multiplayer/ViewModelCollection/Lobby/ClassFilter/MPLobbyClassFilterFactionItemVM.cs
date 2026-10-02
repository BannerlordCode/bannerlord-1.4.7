using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter
{
	// Token: 0x02000064 RID: 100
	public class MPLobbyClassFilterFactionItemVM : ViewModel
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0001E1E3 File Offset: 0x0001C3E3
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0001E1EB File Offset: 0x0001C3EB
		public BasicCultureObject Culture { get; private set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0001E1F4 File Offset: 0x0001C3F4
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0001E1FC File Offset: 0x0001C3FC
		public MPLobbyClassFilterClassItemVM SelectedClassItem { get; private set; }

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001E208 File Offset: 0x0001C408
		public MPLobbyClassFilterFactionItemVM(string cultureCode, bool isEnabled, Action<MPLobbyClassFilterFactionItemVM> onActiveChanged, Action<MPLobbyClassFilterClassItemVM> onClassSelect)
		{
			this._onActiveChanged = onActiveChanged;
			this._onClassSelect = onClassSelect;
			this.CultureCode = cultureCode;
			this.IsEnabled = isEnabled;
			this.Culture = MBObjectManager.Instance.GetObject<BasicCultureObject>(cultureCode);
			this.CreateClassGroupAndClasses(this.Culture);
			this.RefreshValues();
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001E25C File Offset: 0x0001C45C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Hint = new HintViewModel(this.Culture.Name, null);
			this.ClassGroups.ApplyActionOnAllItems(delegate(MPLobbyClassFilterClassGroupItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001E2B0 File Offset: 0x0001C4B0
		public override void OnFinalize()
		{
			this.Culture = null;
			this._classGroupDictionary.Clear();
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001E2C4 File Offset: 0x0001C4C4
		private void CreateClassGroupAndClasses(BasicCultureObject culture)
		{
			this._classGroupDictionary = new Dictionary<string, MPLobbyClassFilterClassGroupItemVM>();
			this.ClassGroups = new MBBindingList<MPLobbyClassFilterClassGroupItemVM>();
			foreach (MultiplayerClassDivisions.MPHeroClassGroup mpheroClassGroup in MultiplayerClassDivisions.MultiplayerHeroClassGroups)
			{
				MPLobbyClassFilterClassGroupItemVM mplobbyClassFilterClassGroupItemVM = new MPLobbyClassFilterClassGroupItemVM(mpheroClassGroup);
				this.ClassGroups.Add(mplobbyClassFilterClassGroupItemVM);
				this._classGroupDictionary.Add(mpheroClassGroup.StringId, mplobbyClassFilterClassGroupItemVM);
			}
			foreach (MultiplayerClassDivisions.MPHeroClass mpheroClass in MultiplayerClassDivisions.GetMPHeroClasses(this.Culture))
			{
				this._classGroupDictionary[mpheroClass.ClassGroup.StringId].AddClass(culture, mpheroClass, new Action<MPLobbyClassFilterClassItemVM>(this.OnClassItemSelect));
			}
			for (int i = this.ClassGroups.Count - 1; i >= 0; i--)
			{
				if (this.ClassGroups[i].Classes.Count == 0)
				{
					this.ClassGroups.RemoveAt(i);
				}
			}
			MPLobbyClassFilterClassItemVM mplobbyClassFilterClassItemVM = this.ClassGroups[0].Classes[0];
			mplobbyClassFilterClassItemVM.IsSelected = true;
			this.SelectedClassItem = mplobbyClassFilterClassItemVM;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0001E420 File Offset: 0x0001C620
		private void OnClassItemSelect(MPLobbyClassFilterClassItemVM selectedClassItem)
		{
			foreach (MPLobbyClassFilterClassGroupItemVM mplobbyClassFilterClassGroupItemVM in this.ClassGroups)
			{
				foreach (MPLobbyClassFilterClassItemVM mplobbyClassFilterClassItemVM in mplobbyClassFilterClassGroupItemVM.Classes)
				{
					if (mplobbyClassFilterClassItemVM != selectedClassItem)
					{
						mplobbyClassFilterClassItemVM.IsSelected = false;
					}
				}
			}
			this.SelectedClassItem = selectedClassItem;
			if (this._onClassSelect != null)
			{
				this._onClassSelect(selectedClassItem);
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0001E4C0 File Offset: 0x0001C6C0
		private void IsActiveChanged()
		{
			if (this.IsActive && this._onActiveChanged != null)
			{
				this._onActiveChanged(this);
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0001E4DE File Offset: 0x0001C6DE
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x0001E4E6 File Offset: 0x0001C6E6
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					this.IsActiveChanged();
				}
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0001E50A File Offset: 0x0001C70A
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0001E512 File Offset: 0x0001C712
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

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0001E530 File Offset: 0x0001C730
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0001E538 File Offset: 0x0001C738
		[DataSourceProperty]
		public string CultureCode
		{
			get
			{
				return this._cultureCode;
			}
			set
			{
				if (value != this._cultureCode)
				{
					this._cultureCode = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureCode");
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0001E55B File Offset: 0x0001C75B
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x0001E563 File Offset: 0x0001C763
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0001E581 File Offset: 0x0001C781
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0001E589 File Offset: 0x0001C789
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterClassGroupItemVM> ClassGroups
		{
			get
			{
				return this._classGroups;
			}
			set
			{
				if (value != this._classGroups)
				{
					this._classGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterClassGroupItemVM>>(value, "ClassGroups");
				}
			}
		}

		// Token: 0x04000470 RID: 1136
		private Action<MPLobbyClassFilterFactionItemVM> _onActiveChanged;

		// Token: 0x04000471 RID: 1137
		private Action<MPLobbyClassFilterClassItemVM> _onClassSelect;

		// Token: 0x04000472 RID: 1138
		private Dictionary<string, MPLobbyClassFilterClassGroupItemVM> _classGroupDictionary;

		// Token: 0x04000475 RID: 1141
		private bool _isActive;

		// Token: 0x04000476 RID: 1142
		private bool _isEnabled;

		// Token: 0x04000477 RID: 1143
		private string _cultureCode;

		// Token: 0x04000478 RID: 1144
		private HintViewModel _hint;

		// Token: 0x04000479 RID: 1145
		private MBBindingList<MPLobbyClassFilterClassGroupItemVM> _classGroups;
	}
}
