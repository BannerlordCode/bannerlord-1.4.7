using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x020000FF RID: 255
	public class CraftingHistoryVM : ViewModel
	{
		// Token: 0x06001738 RID: 5944 RVA: 0x00059E4C File Offset: 0x0005804C
		public CraftingHistoryVM(Crafting crafting, ICraftingCampaignBehavior craftingBehavior, Func<CraftingOrder> getActiveOrder, Action<WeaponDesignSelectorVM> onDone)
		{
			this._crafting = crafting;
			this._craftingBehavior = craftingBehavior;
			this._getActiveOrder = getActiveOrder;
			this._onDone = onDone;
			this.CraftingHistory = new MBBindingList<WeaponDesignSelectorVM>();
			this.HistoryHint = new HintViewModel(CraftingHistoryVM._craftingHistoryText, null);
			this.HistoryDisabledHint = new HintViewModel(CraftingHistoryVM._noItemsHint, null);
			this.RefreshValues();
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00059EB0 File Offset: 0x000580B0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = CraftingHistoryVM._craftingHistoryText.ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.RefreshAvailability();
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00059F08 File Offset: 0x00058108
		private void RefreshCraftingHistory()
		{
			this.FinalizeHistory();
			CraftingOrder craftingOrder = this._getActiveOrder();
			foreach (WeaponDesign weaponDesign in this._craftingBehavior.CraftingHistory)
			{
				if (craftingOrder == null || weaponDesign.Template.TemplateName.ToString() == craftingOrder.PreCraftedWeaponDesignItem.WeaponDesign.Template.TemplateName.ToString())
				{
					this.CraftingHistory.Add(new WeaponDesignSelectorVM(weaponDesign, new Action<WeaponDesignSelectorVM>(this.ExecuteSelect)));
				}
			}
			this.HasItemsInHistory = this.CraftingHistory.Count > 0;
			this.ExecuteSelect(null);
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00059FD4 File Offset: 0x000581D4
		private void FinalizeHistory()
		{
			if (this.CraftingHistory.Count > 0)
			{
				foreach (WeaponDesignSelectorVM weaponDesignSelectorVM in this.CraftingHistory)
				{
					weaponDesignSelectorVM.OnFinalize();
				}
			}
			this.CraftingHistory.Clear();
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x0005A038 File Offset: 0x00058238
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.FinalizeHistory();
			this.DoneKey.OnFinalize();
			this.CancelKey.OnFinalize();
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0005A05C File Offset: 0x0005825C
		public void RefreshAvailability()
		{
			CraftingOrder activeOrder = this._getActiveOrder();
			this.HasItemsInHistory = ((activeOrder == null) ? (this._craftingBehavior.CraftingHistory.Count > 0) : this._craftingBehavior.CraftingHistory.Any<WeaponDesign>((WeaponDesign x) => x.Template.StringId == activeOrder.PreCraftedWeaponDesignItem.WeaponDesign.Template.StringId));
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0005A0BF File Offset: 0x000582BF
		public void ExecuteOpen()
		{
			this.RefreshCraftingHistory();
			this.IsVisible = true;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0005A0CE File Offset: 0x000582CE
		public void ExecuteCancel()
		{
			this.IsVisible = false;
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x0005A0D7 File Offset: 0x000582D7
		public void ExecuteDone()
		{
			Action<WeaponDesignSelectorVM> onDone = this._onDone;
			if (onDone != null)
			{
				onDone(this.SelectedDesign);
			}
			this.ExecuteCancel();
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x0005A0F6 File Offset: 0x000582F6
		private void ExecuteSelect(WeaponDesignSelectorVM selector)
		{
			this.IsDoneAvailable = selector != null;
			if (this.SelectedDesign != null)
			{
				this.SelectedDesign.IsSelected = false;
			}
			this.SelectedDesign = selector;
			if (this.SelectedDesign != null)
			{
				this.SelectedDesign.IsSelected = true;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x0005A131 File Offset: 0x00058331
		// (set) Token: 0x06001743 RID: 5955 RVA: 0x0005A139 File Offset: 0x00058339
		[DataSourceProperty]
		public bool IsDoneAvailable
		{
			get
			{
				return this._isDoneAvailable;
			}
			set
			{
				if (value != this._isDoneAvailable)
				{
					this._isDoneAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsDoneAvailable");
				}
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x0005A157 File Offset: 0x00058357
		// (set) Token: 0x06001745 RID: 5957 RVA: 0x0005A15F File Offset: 0x0005835F
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x0005A17D File Offset: 0x0005837D
		// (set) Token: 0x06001747 RID: 5959 RVA: 0x0005A185 File Offset: 0x00058385
		[DataSourceProperty]
		public bool HasItemsInHistory
		{
			get
			{
				return this._hasItemsInHistory;
			}
			set
			{
				if (value != this._hasItemsInHistory)
				{
					this._hasItemsInHistory = value;
					base.OnPropertyChangedWithValue(value, "HasItemsInHistory");
				}
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x0005A1A3 File Offset: 0x000583A3
		// (set) Token: 0x06001749 RID: 5961 RVA: 0x0005A1AB File Offset: 0x000583AB
		[DataSourceProperty]
		public HintViewModel HistoryHint
		{
			get
			{
				return this._historyHint;
			}
			set
			{
				if (value != this._historyHint)
				{
					this._historyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HistoryHint");
				}
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x0005A1C9 File Offset: 0x000583C9
		// (set) Token: 0x0600174B RID: 5963 RVA: 0x0005A1D1 File Offset: 0x000583D1
		[DataSourceProperty]
		public HintViewModel HistoryDisabledHint
		{
			get
			{
				return this._historyDisabledHint;
			}
			set
			{
				if (value != this._historyDisabledHint)
				{
					this._historyDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HistoryDisabledHint");
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x0005A1EF File Offset: 0x000583EF
		// (set) Token: 0x0600174D RID: 5965 RVA: 0x0005A1F7 File Offset: 0x000583F7
		[DataSourceProperty]
		public MBBindingList<WeaponDesignSelectorVM> CraftingHistory
		{
			get
			{
				return this._craftingHistory;
			}
			set
			{
				if (value != this._craftingHistory)
				{
					this._craftingHistory = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponDesignSelectorVM>>(value, "CraftingHistory");
				}
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x0005A215 File Offset: 0x00058415
		// (set) Token: 0x0600174F RID: 5967 RVA: 0x0005A21D File Offset: 0x0005841D
		[DataSourceProperty]
		public WeaponDesignSelectorVM SelectedDesign
		{
			get
			{
				return this._selectedDesign;
			}
			set
			{
				if (value != this._selectedDesign)
				{
					this._selectedDesign = value;
					base.OnPropertyChangedWithValue<WeaponDesignSelectorVM>(value, "SelectedDesign");
				}
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x0005A23B File Offset: 0x0005843B
		// (set) Token: 0x06001751 RID: 5969 RVA: 0x0005A243 File Offset: 0x00058443
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

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x0005A266 File Offset: 0x00058466
		// (set) Token: 0x06001753 RID: 5971 RVA: 0x0005A26E File Offset: 0x0005846E
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

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x0005A291 File Offset: 0x00058491
		// (set) Token: 0x06001755 RID: 5973 RVA: 0x0005A299 File Offset: 0x00058499
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

		// Token: 0x06001756 RID: 5974 RVA: 0x0005A2BC File Offset: 0x000584BC
		public void SetDoneKey(HotKey hotkey)
		{
			this.DoneKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x0005A2CB File Offset: 0x000584CB
		public void SetCancelKey(HotKey hotkey)
		{
			this.CancelKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x0005A2DA File Offset: 0x000584DA
		// (set) Token: 0x06001759 RID: 5977 RVA: 0x0005A2E2 File Offset: 0x000584E2
		public InputKeyItemVM CancelKey
		{
			get
			{
				return this._cancelKey;
			}
			set
			{
				if (value != this._cancelKey)
				{
					this._cancelKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelKey");
				}
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x0005A300 File Offset: 0x00058500
		// (set) Token: 0x0600175B RID: 5979 RVA: 0x0005A308 File Offset: 0x00058508
		public InputKeyItemVM DoneKey
		{
			get
			{
				return this._doneKey;
			}
			set
			{
				if (value != this._doneKey)
				{
					this._doneKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneKey");
				}
			}
		}

		// Token: 0x04000A9B RID: 2715
		private static TextObject _noItemsHint = new TextObject("{=saHYZKLt}There are no available items in history", null);

		// Token: 0x04000A9C RID: 2716
		private static TextObject _craftingHistoryText = new TextObject("{=xW4BPVLX}Crafting History", null);

		// Token: 0x04000A9D RID: 2717
		private ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000A9E RID: 2718
		private Func<CraftingOrder> _getActiveOrder;

		// Token: 0x04000A9F RID: 2719
		private Action<WeaponDesignSelectorVM> _onDone;

		// Token: 0x04000AA0 RID: 2720
		private Crafting _crafting;

		// Token: 0x04000AA1 RID: 2721
		private bool _isDoneAvailable;

		// Token: 0x04000AA2 RID: 2722
		private bool _isVisible;

		// Token: 0x04000AA3 RID: 2723
		private bool _hasItemsInHistory;

		// Token: 0x04000AA4 RID: 2724
		private HintViewModel _historyHint;

		// Token: 0x04000AA5 RID: 2725
		private HintViewModel _historyDisabledHint;

		// Token: 0x04000AA6 RID: 2726
		private MBBindingList<WeaponDesignSelectorVM> _craftingHistory;

		// Token: 0x04000AA7 RID: 2727
		private WeaponDesignSelectorVM _selectedDesign;

		// Token: 0x04000AA8 RID: 2728
		private string _titleText;

		// Token: 0x04000AA9 RID: 2729
		private string _doneText;

		// Token: 0x04000AAA RID: 2730
		private string _cancelText;

		// Token: 0x04000AAB RID: 2731
		private InputKeyItemVM _cancelKey;

		// Token: 0x04000AAC RID: 2732
		private InputKeyItemVM _doneKey;
	}
}
