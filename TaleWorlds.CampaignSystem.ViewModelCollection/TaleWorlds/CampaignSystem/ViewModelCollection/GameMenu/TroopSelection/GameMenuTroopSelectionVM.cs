using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TroopSelection
{
	// Token: 0x0200009F RID: 159
	public class GameMenuTroopSelectionVM : ViewModel
	{
		// Token: 0x06000F72 RID: 3954 RVA: 0x00040650 File Offset: 0x0003E850
		public GameMenuTroopSelectionVM(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount)
		{
			this._canChangeChangeStatusOfTroop = canChangeChangeStatusOfTroop;
			this._onDone = onDone;
			this._fullRoster = fullRoster;
			this._initialSelections = initialSelections;
			this._maxSelectableTroopCount = maxSelectableTroopCount;
			this._minSelectableTroopCount = minSelectableTroopCount;
			this.DoneHint = new HintViewModel();
			this.InitList();
			this.RefreshValues();
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x000406D0 File Offset: 0x0003E8D0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = this._titleTextObject.ToString();
			this.CurrentSelectedAmountTitle = this._chosenTitleTextObject.ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.ClearSelectionText = new TextObject("{=QMNWbmao}Clear Selection", null).ToString();
			this.RefreshDoneHint();
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00040750 File Offset: 0x0003E950
		protected virtual void RefreshDoneHint()
		{
			if (this.IsDoneEnabled)
			{
				this.DoneHint.HintText = TextObject.GetEmpty();
				return;
			}
			if (this._currentTotalSelectedTroopCount < this._minSelectableTroopCount)
			{
				this.DoneHint.HintText = new TextObject("{=LlV29O9B}You must select at least {TROOP_COUNT} troops", null).SetTextVariable("TROOP_COUNT", this._minSelectableTroopCount);
				return;
			}
			this.DoneHint.HintText = new TextObject("{=TdWQM7QZ}You must select less than {TROOP_COUNT} troops", null).SetTextVariable("TROOP_COUNT", this._maxSelectableTroopCount);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x000407D4 File Offset: 0x0003E9D4
		protected virtual void InitList()
		{
			this.Troops = new MBBindingList<TroopSelectionItemVM>();
			this._currentTotalSelectedTroopCount = 0;
			foreach (TroopRosterElement troopRosterElement in this._fullRoster.GetTroopRoster())
			{
				TroopSelectionItemVM troopSelectionItemVM = new TroopSelectionItemVM(troopRosterElement, new Action<TroopSelectionItemVM>(this.OnAddTroop), new Action<TroopSelectionItemVM>(this.OnRemoveTroop));
				troopSelectionItemVM.IsLocked = !this._canChangeChangeStatusOfTroop(troopRosterElement.Character) || troopRosterElement.Number - troopRosterElement.WoundedNumber <= 0;
				this.Troops.Add(troopSelectionItemVM);
				int troopCount = this._initialSelections.GetTroopCount(troopRosterElement.Character);
				if (troopCount > 0)
				{
					troopSelectionItemVM.CurrentAmount = troopCount;
					this._currentTotalSelectedTroopCount += troopCount;
				}
			}
			this.Troops.Sort(new TroopItemComparer());
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000408D4 File Offset: 0x0003EAD4
		private void OnRemoveTroop(TroopSelectionItemVM troopItem)
		{
			if (troopItem.CurrentAmount > 0)
			{
				int num = 1;
				if (this.IsEntireStackModifierActive)
				{
					num = troopItem.CurrentAmount;
				}
				else if (this.IsFiveStackModifierActive)
				{
					num = MathF.Min(troopItem.CurrentAmount, 5);
				}
				troopItem.CurrentAmount -= num;
				this._currentTotalSelectedTroopCount -= num;
			}
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00040934 File Offset: 0x0003EB34
		private void OnAddTroop(TroopSelectionItemVM troopItem)
		{
			if (troopItem.CurrentAmount < troopItem.MaxAmount && this._currentTotalSelectedTroopCount < this._maxSelectableTroopCount)
			{
				int num = 1;
				if (this.IsEntireStackModifierActive)
				{
					num = MathF.Min(troopItem.MaxAmount - troopItem.CurrentAmount, this._maxSelectableTroopCount - this._currentTotalSelectedTroopCount);
				}
				else if (this.IsFiveStackModifierActive)
				{
					num = MathF.Min(MathF.Min(troopItem.MaxAmount - troopItem.CurrentAmount, this._maxSelectableTroopCount - this._currentTotalSelectedTroopCount), 5);
				}
				troopItem.CurrentAmount += num;
				this._currentTotalSelectedTroopCount += num;
			}
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000409DC File Offset: 0x0003EBDC
		protected virtual void OnCurrentSelectedAmountChange()
		{
			foreach (TroopSelectionItemVM troopSelectionItemVM in this.Troops)
			{
				troopSelectionItemVM.IsRosterFull = this._currentTotalSelectedTroopCount >= this._maxSelectableTroopCount;
			}
			GameTexts.SetVariable("LEFT", this._currentTotalSelectedTroopCount);
			GameTexts.SetVariable("RIGHT", this._maxSelectableTroopCount);
			this.CurrentSelectedAmountText = GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
			this.IsDoneEnabled = this._currentTotalSelectedTroopCount <= this._maxSelectableTroopCount && this._currentTotalSelectedTroopCount >= this._minSelectableTroopCount;
			this.RefreshDoneHint();
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00040A9C File Offset: 0x0003EC9C
		protected TroopRoster BuildSelectedTroopRoster()
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopSelectionItemVM troopSelectionItemVM in this.Troops)
			{
				if (troopSelectionItemVM.CurrentAmount > 0)
				{
					troopRoster.AddToCounts(troopSelectionItemVM.Troop.Character, troopSelectionItemVM.CurrentAmount, false, 0, 0, true, -1);
				}
			}
			return troopRoster;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00040B10 File Offset: 0x0003ED10
		protected virtual void OnDone()
		{
			TroopRoster troopRoster = this.BuildSelectedTroopRoster();
			this.IsEnabled = false;
			this._onDone.DynamicInvokeWithLog(new object[] { troopRoster });
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00040B41 File Offset: 0x0003ED41
		protected void UpdateMaxSelectableTroopCount(int maxValue)
		{
			this._maxSelectableTroopCount = maxValue;
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00040B50 File Offset: 0x0003ED50
		public void ExecuteDone()
		{
			TextObject warningMessageOnDone = this.GetWarningMessageOnDone();
			if (!TextObject.IsNullOrEmpty(warningMessageOnDone))
			{
				string text = warningMessageOnDone.ToString();
				InformationManager.ShowInquiry(new InquiryData(this.TitleText, text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.OnDone), null, "", 0f, null, null, null), false, false);
				return;
			}
			this.OnDone();
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00040BCA File Offset: 0x0003EDCA
		protected virtual TextObject GetWarningMessageOnDone()
		{
			if (this.GetAvailableSelectableTroopCount() > 0)
			{
				return new TextObject("{=z2Slmx4N}There are still some room for more soldiers. Do you want to proceed?", null);
			}
			return null;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00040BE4 File Offset: 0x0003EDE4
		protected int GetAvailableSelectableTroopCount()
		{
			int num = 0;
			foreach (TroopSelectionItemVM troopSelectionItemVM in this.Troops)
			{
				if (!troopSelectionItemVM.IsLocked && troopSelectionItemVM.CurrentAmount < troopSelectionItemVM.MaxAmount)
				{
					num += troopSelectionItemVM.MaxAmount - troopSelectionItemVM.CurrentAmount;
				}
			}
			if (this._currentTotalSelectedTroopCount + num > this._maxSelectableTroopCount)
			{
				num = this._maxSelectableTroopCount - this._currentTotalSelectedTroopCount;
			}
			return num;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00040C74 File Offset: 0x0003EE74
		public void ExecuteCancel()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00040C7D File Offset: 0x0003EE7D
		public void ExecuteReset()
		{
			this.InitList();
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00040C8B File Offset: 0x0003EE8B
		public void ExecuteClearSelection()
		{
			this.Troops.ApplyActionOnAllItems(delegate(TroopSelectionItemVM troopItem)
			{
				if (this._canChangeChangeStatusOfTroop(troopItem.Troop.Character))
				{
					int currentAmount = troopItem.CurrentAmount;
					for (int i = 0; i < currentAmount; i++)
					{
						troopItem.ExecuteRemove();
					}
				}
			});
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00040CA4 File Offset: 0x0003EEA4
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM resetInputKey = this.ResetInputKey;
			if (resetInputKey == null)
			{
				return;
			}
			resetInputKey.OnFinalize();
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00040CDE File Offset: 0x0003EEDE
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00040CED File Offset: 0x0003EEED
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00040CFC File Offset: 0x0003EEFC
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00040D0B File Offset: 0x0003EF0B
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x00040D13 File Offset: 0x0003EF13
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00040D31 File Offset: 0x0003EF31
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x00040D39 File Offset: 0x0003EF39
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00040D57 File Offset: 0x0003EF57
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00040D5F File Offset: 0x0003EF5F
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00040D7D File Offset: 0x0003EF7D
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x00040D85 File Offset: 0x0003EF85
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

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x00040DA3 File Offset: 0x0003EFA3
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x00040DAB File Offset: 0x0003EFAB
		[DataSourceProperty]
		public bool IsDoneEnabled
		{
			get
			{
				return this._isDoneEnabled;
			}
			set
			{
				if (value != this._isDoneEnabled)
				{
					this._isDoneEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsDoneEnabled");
				}
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x00040DC9 File Offset: 0x0003EFC9
		// (set) Token: 0x06000F91 RID: 3985 RVA: 0x00040DD1 File Offset: 0x0003EFD1
		[DataSourceProperty]
		public HintViewModel DoneHint
		{
			get
			{
				return this._doneHint;
			}
			set
			{
				if (value != this._doneHint)
				{
					this._doneHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneHint");
				}
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00040DEF File Offset: 0x0003EFEF
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x00040DF7 File Offset: 0x0003EFF7
		[DataSourceProperty]
		public MBBindingList<TroopSelectionItemVM> Troops
		{
			get
			{
				return this._troops;
			}
			set
			{
				if (value != this._troops)
				{
					this._troops = value;
					base.OnPropertyChangedWithValue<MBBindingList<TroopSelectionItemVM>>(value, "Troops");
				}
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x00040E15 File Offset: 0x0003F015
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x00040E1D File Offset: 0x0003F01D
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x00040E40 File Offset: 0x0003F040
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x00040E48 File Offset: 0x0003F048
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00040E6B File Offset: 0x0003F06B
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x00040E73 File Offset: 0x0003F073
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x00040E96 File Offset: 0x0003F096
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x00040E9E File Offset: 0x0003F09E
		[DataSourceProperty]
		public string ClearSelectionText
		{
			get
			{
				return this._clearSelectionText;
			}
			set
			{
				if (value != this._clearSelectionText)
				{
					this._clearSelectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClearSelectionText");
				}
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x00040EC9 File Offset: 0x0003F0C9
		[DataSourceProperty]
		public string CurrentSelectedAmountText
		{
			get
			{
				return this._currentSelectedAmountText;
			}
			set
			{
				if (value != this._currentSelectedAmountText)
				{
					this._currentSelectedAmountText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentSelectedAmountText");
				}
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x00040EEC File Offset: 0x0003F0EC
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x00040EF4 File Offset: 0x0003F0F4
		[DataSourceProperty]
		public string CurrentSelectedAmountTitle
		{
			get
			{
				return this._currentSelectedAmountTitle;
			}
			set
			{
				if (value != this._currentSelectedAmountTitle)
				{
					this._currentSelectedAmountTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentSelectedAmountTitle");
				}
			}
		}

		// Token: 0x04000710 RID: 1808
		private readonly Action<TroopRoster> _onDone;

		// Token: 0x04000711 RID: 1809
		private readonly TroopRoster _fullRoster;

		// Token: 0x04000712 RID: 1810
		private readonly TroopRoster _initialSelections;

		// Token: 0x04000713 RID: 1811
		private readonly Func<CharacterObject, bool> _canChangeChangeStatusOfTroop;

		// Token: 0x04000714 RID: 1812
		private int _maxSelectableTroopCount;

		// Token: 0x04000715 RID: 1813
		private readonly int _minSelectableTroopCount;

		// Token: 0x04000716 RID: 1814
		private readonly TextObject _titleTextObject = new TextObject("{=uQgNPJnc}Manage Troops", null);

		// Token: 0x04000717 RID: 1815
		private readonly TextObject _chosenTitleTextObject = new TextObject("{=InqmgBiF}Chosen Crew", null);

		// Token: 0x04000718 RID: 1816
		private int _currentTotalSelectedTroopCount;

		// Token: 0x04000719 RID: 1817
		public bool IsFiveStackModifierActive;

		// Token: 0x0400071A RID: 1818
		public bool IsEntireStackModifierActive;

		// Token: 0x0400071B RID: 1819
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400071C RID: 1820
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400071D RID: 1821
		private InputKeyItemVM _resetInputKey;

		// Token: 0x0400071E RID: 1822
		private bool _isEnabled;

		// Token: 0x0400071F RID: 1823
		private bool _isDoneEnabled;

		// Token: 0x04000720 RID: 1824
		private HintViewModel _doneHint;

		// Token: 0x04000721 RID: 1825
		private string _doneText;

		// Token: 0x04000722 RID: 1826
		private string _cancelText;

		// Token: 0x04000723 RID: 1827
		private string _titleText;

		// Token: 0x04000724 RID: 1828
		private string _clearSelectionText;

		// Token: 0x04000725 RID: 1829
		private string _currentSelectedAmountText;

		// Token: 0x04000726 RID: 1830
		private string _currentSelectedAmountTitle;

		// Token: 0x04000727 RID: 1831
		private MBBindingList<TroopSelectionItemVM> _troops;
	}
}
