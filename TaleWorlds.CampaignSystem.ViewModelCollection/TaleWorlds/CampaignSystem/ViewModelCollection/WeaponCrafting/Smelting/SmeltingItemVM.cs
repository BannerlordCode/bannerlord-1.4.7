using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting
{
	// Token: 0x02000111 RID: 273
	public class SmeltingItemVM : ViewModel
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600190E RID: 6414 RVA: 0x0005FA4A File Offset: 0x0005DC4A
		// (set) Token: 0x0600190D RID: 6413 RVA: 0x0005FA41 File Offset: 0x0005DC41
		public EquipmentElement EquipmentElement { get; private set; }

		// Token: 0x0600190F RID: 6415 RVA: 0x0005FA54 File Offset: 0x0005DC54
		public SmeltingItemVM(EquipmentElement equipmentElement, Action<SmeltingItemVM> onSelection, Action<SmeltingItemVM, bool> onItemLockedStateChange, bool isLocked, int numOfItems)
		{
			this._onSelection = onSelection;
			this._onItemLockedStateChange = onItemLockedStateChange;
			this.EquipmentElement = equipmentElement;
			this.Yield = new MBBindingList<CraftingResourceItemVM>();
			this.InputMaterials = new MBBindingList<CraftingResourceItemVM>();
			this.LockHint = new HintViewModel(GameTexts.FindText("str_lock_in_inventory", null).SetTextVariable("TRANSFERABLE", GameTexts.FindText("str_items", null).ToString()), null);
			int[] smeltingOutputForItem = Campaign.Current.Models.SmithingModel.GetSmeltingOutputForItem(equipmentElement.Item);
			for (int i = 0; i < smeltingOutputForItem.Length; i++)
			{
				if (smeltingOutputForItem[i] > 0)
				{
					this.Yield.Add(new CraftingResourceItemVM((CraftingMaterials)i, smeltingOutputForItem[i], 0));
				}
				else if (smeltingOutputForItem[i] < 0)
				{
					this.InputMaterials.Add(new CraftingResourceItemVM((CraftingMaterials)i, -smeltingOutputForItem[i], 0));
				}
			}
			this.IsLocked = isLocked;
			this.Visual = new ItemImageIdentifierVM(equipmentElement.Item, "");
			this.NumOfItems = numOfItems;
			this.HasMoreThanOneItem = this.NumOfItems > 1;
			this.RefreshValues();
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0005FB64 File Offset: 0x0005DD64
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.EquipmentElement.Item.Name.ToString();
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0005FB95 File Offset: 0x0005DD95
		public void ExecuteSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0005FBA3 File Offset: 0x0005DDA3
		public void ExecuteShowItemTooltip()
		{
			InformationManager.ShowTooltip(typeof(ItemObject), new object[] { this.EquipmentElement });
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0005FBC8 File Offset: 0x0005DDC8
		public void ExecuteHideItemTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06001914 RID: 6420 RVA: 0x0005FBCF File Offset: 0x0005DDCF
		// (set) Token: 0x06001915 RID: 6421 RVA: 0x0005FBD7 File Offset: 0x0005DDD7
		[DataSourceProperty]
		public ItemImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x0005FBF5 File Offset: 0x0005DDF5
		// (set) Token: 0x06001917 RID: 6423 RVA: 0x0005FBFD File Offset: 0x0005DDFD
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> Yield
		{
			get
			{
				return this._yield;
			}
			set
			{
				if (value != this._yield)
				{
					this._yield = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "Yield");
				}
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06001918 RID: 6424 RVA: 0x0005FC1B File Offset: 0x0005DE1B
		// (set) Token: 0x06001919 RID: 6425 RVA: 0x0005FC23 File Offset: 0x0005DE23
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> InputMaterials
		{
			get
			{
				return this._inputMaterials;
			}
			set
			{
				if (value != this._inputMaterials)
				{
					this._inputMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "InputMaterials");
				}
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600191A RID: 6426 RVA: 0x0005FC41 File Offset: 0x0005DE41
		// (set) Token: 0x0600191B RID: 6427 RVA: 0x0005FC49 File Offset: 0x0005DE49
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x0005FC6C File Offset: 0x0005DE6C
		// (set) Token: 0x0600191D RID: 6429 RVA: 0x0005FC74 File Offset: 0x0005DE74
		[DataSourceProperty]
		public int NumOfItems
		{
			get
			{
				return this._numOfItems;
			}
			set
			{
				if (value != this._numOfItems)
				{
					this._numOfItems = value;
					base.OnPropertyChangedWithValue(value, "NumOfItems");
				}
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x0005FC92 File Offset: 0x0005DE92
		// (set) Token: 0x0600191F RID: 6431 RVA: 0x0005FC9A File Offset: 0x0005DE9A
		[DataSourceProperty]
		public bool HasMoreThanOneItem
		{
			get
			{
				return this._hasMoreThanOneItem;
			}
			set
			{
				if (value != this._hasMoreThanOneItem)
				{
					this._hasMoreThanOneItem = value;
					base.OnPropertyChangedWithValue(value, "HasMoreThanOneItem");
				}
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x0005FCB8 File Offset: 0x0005DEB8
		// (set) Token: 0x06001921 RID: 6433 RVA: 0x0005FCC0 File Offset: 0x0005DEC0
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

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x0005FCDE File Offset: 0x0005DEDE
		// (set) Token: 0x06001923 RID: 6435 RVA: 0x0005FCE6 File Offset: 0x0005DEE6
		[DataSourceProperty]
		public HintViewModel LockHint
		{
			get
			{
				return this._lockHint;
			}
			set
			{
				if (value != this._lockHint)
				{
					this._lockHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LockHint");
				}
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x0005FD04 File Offset: 0x0005DF04
		// (set) Token: 0x06001925 RID: 6437 RVA: 0x0005FD0C File Offset: 0x0005DF0C
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
					this._onItemLockedStateChange(this, value);
				}
			}
		}

		// Token: 0x04000B82 RID: 2946
		private readonly Action<SmeltingItemVM> _onSelection;

		// Token: 0x04000B83 RID: 2947
		private readonly Action<SmeltingItemVM, bool> _onItemLockedStateChange;

		// Token: 0x04000B84 RID: 2948
		private ItemImageIdentifierVM _visual;

		// Token: 0x04000B85 RID: 2949
		private string _name;

		// Token: 0x04000B86 RID: 2950
		private int _numOfItems;

		// Token: 0x04000B87 RID: 2951
		private MBBindingList<CraftingResourceItemVM> _inputMaterials;

		// Token: 0x04000B88 RID: 2952
		private MBBindingList<CraftingResourceItemVM> _yield;

		// Token: 0x04000B89 RID: 2953
		private HintViewModel _lockHint;

		// Token: 0x04000B8A RID: 2954
		private bool _isSelected;

		// Token: 0x04000B8B RID: 2955
		private bool _isLocked;

		// Token: 0x04000B8C RID: 2956
		private bool _hasMoreThanOneItem;
	}
}
