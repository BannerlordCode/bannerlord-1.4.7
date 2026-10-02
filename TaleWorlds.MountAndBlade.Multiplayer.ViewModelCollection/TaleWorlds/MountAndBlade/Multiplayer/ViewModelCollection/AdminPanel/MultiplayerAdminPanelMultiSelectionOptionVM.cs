using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000AC RID: 172
	public class MultiplayerAdminPanelMultiSelectionOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x00032F96 File Offset: 0x00031196
		public MultiplayerAdminPanelMultiSelectionOptionVM(IAdminPanelMultiSelectionOption option)
			: base(option)
		{
			this._option = option;
			this.MultiSelectionOptions = new MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM(-1, null);
			this.IsMultiSelectionOption = true;
			this.RefreshValues();
			this._initialValue = this.MultiSelectionOptions.SelectedItem;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00032FD1 File Offset: 0x000311D1
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshOptions();
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00032FDF File Offset: 0x000311DF
		private void OnSelectorChange(SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM> selector)
		{
			IAdminPanelOption<IAdminPanelMultiSelectionItem> option = this._option;
			MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM selectedItem = selector.SelectedItem;
			option.SetValue((selectedItem != null) ? selectedItem.SelectionItem : null);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00033000 File Offset: 0x00031200
		public override void UpdateValues()
		{
			base.UpdateValues();
			if (!this._option.GetAvailableOptions().SequenceEqual<IAdminPanelMultiSelectionItem>(this.MultiSelectionOptions.ItemList.Select<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM, IAdminPanelMultiSelectionItem>((MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM i) => i.SelectionItem)))
			{
				this.RefreshOptions();
			}
			for (int j = 0; j < this.MultiSelectionOptions.ItemList.Count; j++)
			{
				if (this.MultiSelectionOptions.ItemList[j].SelectionItem == this._option.GetValue())
				{
					this.MultiSelectionOptions.SelectedIndex = j;
					return;
				}
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x000330A5 File Offset: 0x000312A5
		public override void ExecuteRestoreDefaults()
		{
			base.ExecuteRestoreDefaults();
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000330AD File Offset: 0x000312AD
		public override void ExecuteRevertChanges()
		{
			base.ExecuteRevertChanges();
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000330B8 File Offset: 0x000312B8
		private void RefreshOptions()
		{
			if (this.MultiSelectionOptions == null)
			{
				return;
			}
			List<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM> list = new List<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>();
			if (this._option == null)
			{
				this.MultiSelectionOptions.Refresh(list, 0, new Action<SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>>(this.OnSelectorChange));
				return;
			}
			IAdminPanelMultiSelectionItem value = this._option.GetValue();
			MBReadOnlyList<IAdminPanelMultiSelectionItem> mbreadOnlyList = this._option.GetAvailableOptions() ?? new MBReadOnlyList<IAdminPanelMultiSelectionItem>();
			if (mbreadOnlyList != null)
			{
				for (int i = 0; i < mbreadOnlyList.Count; i++)
				{
					list.Add(new MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM(mbreadOnlyList[i]));
				}
			}
			this.MultiSelectionOptions.IsEnabled = !base.IsDisabled;
			this.MultiSelectionOptions.Refresh(list, 0, new Action<SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>>(this.OnSelectorChange));
			if (this.MultiSelectionOptions != null)
			{
				for (int j = 0; j < this.MultiSelectionOptions.ItemList.Count; j++)
				{
					if (this.MultiSelectionOptions.ItemList[j].SelectionItem == value)
					{
						this.MultiSelectionOptions.SelectedIndex = j;
						return;
					}
				}
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x000331B9 File Offset: 0x000313B9
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x000331C1 File Offset: 0x000313C1
		[DataSourceProperty]
		public bool IsMultiSelectionOption
		{
			get
			{
				return this._isMultiSelectionOption;
			}
			set
			{
				if (value != this._isMultiSelectionOption)
				{
					this._isMultiSelectionOption = value;
					base.OnPropertyChangedWithValue(value, "IsMultiSelectionOption");
				}
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x000331DF File Offset: 0x000313DF
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x000331E7 File Offset: 0x000313E7
		[DataSourceProperty]
		public MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM MultiSelectionOptions
		{
			get
			{
				return this._multiSelectionOptions;
			}
			set
			{
				if (value != this._multiSelectionOptions)
				{
					this._multiSelectionOptions = value;
					base.OnPropertyChangedWithValue<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM>(value, "MultiSelectionOptions");
				}
			}
		}

		// Token: 0x0400079E RID: 1950
		private new readonly IAdminPanelMultiSelectionOption _option;

		// Token: 0x0400079F RID: 1951
		private readonly SelectorItemVM _initialValue;

		// Token: 0x040007A0 RID: 1952
		private bool _isMultiSelectionOption;

		// Token: 0x040007A1 RID: 1953
		private MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM _multiSelectionOptions;

		// Token: 0x0200018F RID: 399
		public class AdminPanelOptionSelectorVM : SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>
		{
			// Token: 0x060012F5 RID: 4853 RVA: 0x0003AF21 File Offset: 0x00039121
			public AdminPanelOptionSelectorVM(int selectedIndex, Action<SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>> onChange)
				: base(selectedIndex, onChange)
			{
			}

			// Token: 0x170005AB RID: 1451
			// (get) Token: 0x060012F6 RID: 4854 RVA: 0x0003AF2B File Offset: 0x0003912B
			// (set) Token: 0x060012F7 RID: 4855 RVA: 0x0003AF33 File Offset: 0x00039133
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

			// Token: 0x04000A50 RID: 2640
			private bool _isEnabled;
		}

		// Token: 0x02000190 RID: 400
		public class AdminPanelOptionSelectorItemVM : SelectorItemVM
		{
			// Token: 0x060012F8 RID: 4856 RVA: 0x0003AF51 File Offset: 0x00039151
			public AdminPanelOptionSelectorItemVM(IAdminPanelMultiSelectionItem selectionItem)
				: base(((selectionItem != null) ? selectionItem.DisplayName : null) ?? ((selectionItem != null) ? selectionItem.Value : null))
			{
				this.SelectionItem = selectionItem;
			}

			// Token: 0x04000A51 RID: 2641
			public readonly IAdminPanelMultiSelectionItem SelectionItem;
		}
	}
}
