using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x02000080 RID: 128
	public class MPArmoryCosmeticTauntSlotVM : ViewModel
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000CCA RID: 3274 RVA: 0x00027B3C File Offset: 0x00025D3C
		// (remove) Token: 0x06000CCB RID: 3275 RVA: 0x00027B70 File Offset: 0x00025D70
		public static event Action<MPArmoryCosmeticTauntSlotVM, bool> OnFocusChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000CCC RID: 3276 RVA: 0x00027BA4 File Offset: 0x00025DA4
		// (remove) Token: 0x06000CCD RID: 3277 RVA: 0x00027BD8 File Offset: 0x00025DD8
		public static event Action<MPArmoryCosmeticTauntSlotVM> OnSelected;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000CCE RID: 3278 RVA: 0x00027C0C File Offset: 0x00025E0C
		// (remove) Token: 0x06000CCF RID: 3279 RVA: 0x00027C40 File Offset: 0x00025E40
		public static event Action<MPArmoryCosmeticTauntSlotVM> OnPreview;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000CD0 RID: 3280 RVA: 0x00027C74 File Offset: 0x00025E74
		// (remove) Token: 0x06000CD1 RID: 3281 RVA: 0x00027CA8 File Offset: 0x00025EA8
		public static event Action<MPArmoryCosmeticTauntSlotVM, MPArmoryCosmeticTauntItemVM, bool> OnTauntEquipped;

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00027CDB File Offset: 0x00025EDB
		public MPArmoryCosmeticTauntSlotVM(int slotIndex)
		{
			this.IsEmpty = true;
			this.SlotIndex = slotIndex;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00027CF1 File Offset: 0x00025EF1
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM selectKeyVisual = this.SelectKeyVisual;
			if (selectKeyVisual == null)
			{
				return;
			}
			selectKeyVisual.OnFinalize();
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00027D0C File Offset: 0x00025F0C
		public void AssignTauntItem(MPArmoryCosmeticTauntItemVM tauntItem, bool isSwapping = false)
		{
			MPArmoryCosmeticTauntItemVM assignedTauntItem = this.AssignedTauntItem;
			this.AssignedTauntItem = tauntItem;
			this.IsEmpty = tauntItem == null;
			if (!isSwapping && assignedTauntItem != null)
			{
				assignedTauntItem.IsUsed = false;
			}
			if (this.AssignedTauntItem != null)
			{
				this.AssignedTauntItem.TauntCosmeticElement.UsageIndex = this.SlotIndex;
				this.AssignedTauntItem.IsUsed = true;
			}
			Action<MPArmoryCosmeticTauntSlotVM, MPArmoryCosmeticTauntItemVM, bool> onTauntEquipped = MPArmoryCosmeticTauntSlotVM.OnTauntEquipped;
			if (onTauntEquipped == null)
			{
				return;
			}
			onTauntEquipped(this, assignedTauntItem, isSwapping);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00027D7A File Offset: 0x00025F7A
		public void ExecuteSelect()
		{
			Action<MPArmoryCosmeticTauntSlotVM> onSelected = MPArmoryCosmeticTauntSlotVM.OnSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00027D8C File Offset: 0x00025F8C
		public void ExecutePreview()
		{
			Action<MPArmoryCosmeticTauntSlotVM> onPreview = MPArmoryCosmeticTauntSlotVM.OnPreview;
			if (onPreview == null)
			{
				return;
			}
			onPreview(this);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00027D9E File Offset: 0x00025F9E
		public void ExecuteFocus()
		{
			Action<MPArmoryCosmeticTauntSlotVM, bool> onFocusChanged = MPArmoryCosmeticTauntSlotVM.OnFocusChanged;
			if (onFocusChanged == null)
			{
				return;
			}
			onFocusChanged(this, true);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00027DB1 File Offset: 0x00025FB1
		public void ExecuteUnfocus()
		{
			Action<MPArmoryCosmeticTauntSlotVM, bool> onFocusChanged = MPArmoryCosmeticTauntSlotVM.OnFocusChanged;
			if (onFocusChanged == null)
			{
				return;
			}
			onFocusChanged(this, false);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00027DC4 File Offset: 0x00025FC4
		public void SetSelectKeyVisual(HotKey hotKey)
		{
			this.SelectKeyVisual = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00027DD3 File Offset: 0x00025FD3
		public void SetEmptySlotKeyVisual(HotKey hotKey)
		{
			this.EmptySlotKeyVisual = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00027DE2 File Offset: 0x00025FE2
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x00027DEA File Offset: 0x00025FEA
		[DataSourceProperty]
		public InputKeyItemVM SelectKeyVisual
		{
			get
			{
				return this._selectKeyVisual;
			}
			set
			{
				if (value != this._selectKeyVisual)
				{
					this._selectKeyVisual = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "SelectKeyVisual");
				}
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00027E08 File Offset: 0x00026008
		// (set) Token: 0x06000CDE RID: 3294 RVA: 0x00027E10 File Offset: 0x00026010
		[DataSourceProperty]
		public InputKeyItemVM EmptySlotKeyVisual
		{
			get
			{
				return this._emptySlotKeyVisual;
			}
			set
			{
				if (value != this._emptySlotKeyVisual)
				{
					this._emptySlotKeyVisual = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "EmptySlotKeyVisual");
				}
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00027E2E File Offset: 0x0002602E
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x00027E36 File Offset: 0x00026036
		[DataSourceProperty]
		public bool IsAcceptingTaunts
		{
			get
			{
				return this._isAcceptingTaunts;
			}
			set
			{
				if (value != this._isAcceptingTaunts)
				{
					this._isAcceptingTaunts = value;
					base.OnPropertyChangedWithValue(value, "IsAcceptingTaunts");
				}
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00027E54 File Offset: 0x00026054
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x00027E5C File Offset: 0x0002605C
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00027E7A File Offset: 0x0002607A
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x00027E82 File Offset: 0x00026082
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

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00027EA0 File Offset: 0x000260A0
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00027EA8 File Offset: 0x000260A8
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return this._isEmpty;
			}
			set
			{
				if (value != this._isEmpty)
				{
					this._isEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsEmpty");
				}
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00027EC6 File Offset: 0x000260C6
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00027ECE File Offset: 0x000260CE
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00027EEC File Offset: 0x000260EC
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x00027EF4 File Offset: 0x000260F4
		[DataSourceProperty]
		public MPArmoryCosmeticTauntItemVM AssignedTauntItem
		{
			get
			{
				return this._assignedTauntItem;
			}
			set
			{
				if (value != this._assignedTauntItem)
				{
					this._assignedTauntItem = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntItemVM>(value, "AssignedTauntItem");
				}
			}
		}

		// Token: 0x040005D2 RID: 1490
		public readonly int SlotIndex;

		// Token: 0x040005D3 RID: 1491
		private InputKeyItemVM _selectKeyVisual;

		// Token: 0x040005D4 RID: 1492
		private InputKeyItemVM _emptySlotKeyVisual;

		// Token: 0x040005D5 RID: 1493
		private bool _isAcceptingTaunts;

		// Token: 0x040005D6 RID: 1494
		private bool _isSelected;

		// Token: 0x040005D7 RID: 1495
		private bool _isEnabled;

		// Token: 0x040005D8 RID: 1496
		private bool _isEmpty;

		// Token: 0x040005D9 RID: 1497
		private bool _isFocused;

		// Token: 0x040005DA RID: 1498
		private MPArmoryCosmeticTauntItemVM _assignedTauntItem;
	}
}
