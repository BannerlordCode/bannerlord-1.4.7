using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x0200001D RID: 29
	public class TroopTypeSelectionPopUpVM : ViewModel
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00009F94 File Offset: 0x00008194
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_cancel", null).ToString();
			this.SelectAllLbl = GameTexts.FindText("str_custom_battle_select_all", null).ToString();
			this.BackToDefaultLbl = GameTexts.FindText("str_custom_battle_back_to_default", null).ToString();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009FFF File Offset: 0x000081FF
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.CancelInputKey.OnFinalize();
			this.ResetInputKey.OnFinalize();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000A028 File Offset: 0x00008228
		public void OpenPopUp(string title, MBBindingList<CustomBattleTroopTypeVM> troops)
		{
			this._itemSelectionsBackUp = new List<bool>();
			foreach (CustomBattleTroopTypeVM customBattleTroopTypeVM in troops)
			{
				this._itemSelectionsBackUp.Add(customBattleTroopTypeVM.IsSelected);
			}
			this._selectedItemCount = troops.Count<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected);
			this.Title = title;
			this.Items = troops;
			this.IsOpen = true;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000A0C8 File Offset: 0x000082C8
		public void OnItemSelectionToggled(CustomBattleTroopTypeVM item)
		{
			if (this._selectedItemCount > 1 || !item.IsSelected)
			{
				item.IsSelected = !item.IsSelected;
				this._selectedItemCount += (item.IsSelected ? 1 : (-1));
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A114 File Offset: 0x00008314
		public void ExecuteSelectAll()
		{
			this.Items.ApplyActionOnAllItems(delegate(CustomBattleTroopTypeVM x)
			{
				x.IsSelected = true;
			});
			this._selectedItemCount = this.Items.Count;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A154 File Offset: 0x00008354
		public void ExecuteBackToDefault()
		{
			this.Items.ApplyActionOnAllItems(delegate(CustomBattleTroopTypeVM x)
			{
				x.IsSelected = x.IsDefault;
			});
			this._selectedItemCount = this.Items.Count<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A1BB File Offset: 0x000083BB
		public void ExecuteCancel()
		{
			this.ExecuteReset();
			Action onPopUpClosed = this.OnPopUpClosed;
			if (onPopUpClosed != null)
			{
				onPopUpClosed();
			}
			this.IsOpen = false;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000A1DB File Offset: 0x000083DB
		public void ExecuteDone()
		{
			this.IsOpen = false;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A1E4 File Offset: 0x000083E4
		public void ExecuteReset()
		{
			int count = this._itemSelectionsBackUp.Count;
			if (count != this.Items.Count)
			{
				Debug.FailedAssert("Backup troop count does not match with the actual troop count.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.CustomBattle\\CustomBattle\\TroopTypeSelectionPopUpVM.cs", "ExecuteReset", 100);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				this.Items[i].IsSelected = this._itemSelectionsBackUp[i];
			}
			this._selectedItemCount = this.Items.Count<CustomBattleTroopTypeVM>((CustomBattleTroopTypeVM x) => x.IsSelected);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000A27B File Offset: 0x0000847B
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000A28A File Offset: 0x0000848A
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000A299 File Offset: 0x00008499
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000A2A8 File Offset: 0x000084A8
		// (set) Token: 0x06000197 RID: 407 RVA: 0x0000A2B0 File Offset: 0x000084B0
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000A2CE File Offset: 0x000084CE
		// (set) Token: 0x06000199 RID: 409 RVA: 0x0000A2D6 File Offset: 0x000084D6
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000A2F4 File Offset: 0x000084F4
		// (set) Token: 0x0600019B RID: 411 RVA: 0x0000A2FC File Offset: 0x000084FC
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000A31A File Offset: 0x0000851A
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000A322 File Offset: 0x00008522
		[DataSourceProperty]
		public MBBindingList<CustomBattleTroopTypeVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<CustomBattleTroopTypeVM>>(value, "Items");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000A340 File Offset: 0x00008540
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000A348 File Offset: 0x00008548
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000A36B File Offset: 0x0000856B
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000A373 File Offset: 0x00008573
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000A396 File Offset: 0x00008596
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000A39E File Offset: 0x0000859E
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000A3C1 File Offset: 0x000085C1
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000A3C9 File Offset: 0x000085C9
		[DataSourceProperty]
		public string SelectAllLbl
		{
			get
			{
				return this._selectAllLbl;
			}
			set
			{
				if (value != this._selectAllLbl)
				{
					this._selectAllLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectAllLbl");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000A3EC File Offset: 0x000085EC
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000A3F4 File Offset: 0x000085F4
		[DataSourceProperty]
		public string BackToDefaultLbl
		{
			get
			{
				return this._backToDefaultLbl;
			}
			set
			{
				if (value != this._backToDefaultLbl)
				{
					this._backToDefaultLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "BackToDefaultLbl");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000A417 File Offset: 0x00008617
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x0000A41F File Offset: 0x0000861F
		[DataSourceProperty]
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
			set
			{
				if (value != this._isOpen)
				{
					this._isOpen = value;
					base.OnPropertyChangedWithValue(value, "IsOpen");
				}
			}
		}

		// Token: 0x040000F7 RID: 247
		public Action OnPopUpClosed;

		// Token: 0x040000F8 RID: 248
		private List<bool> _itemSelectionsBackUp;

		// Token: 0x040000F9 RID: 249
		private int _selectedItemCount;

		// Token: 0x040000FA RID: 250
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040000FB RID: 251
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040000FC RID: 252
		private InputKeyItemVM _resetInputKey;

		// Token: 0x040000FD RID: 253
		private MBBindingList<CustomBattleTroopTypeVM> _items;

		// Token: 0x040000FE RID: 254
		private string _title;

		// Token: 0x040000FF RID: 255
		private string _doneLbl;

		// Token: 0x04000100 RID: 256
		private string _cancelLbl;

		// Token: 0x04000101 RID: 257
		private string _selectAllLbl;

		// Token: 0x04000102 RID: 258
		private string _backToDefaultLbl;

		// Token: 0x04000103 RID: 259
		private bool _isOpen;
	}
}
