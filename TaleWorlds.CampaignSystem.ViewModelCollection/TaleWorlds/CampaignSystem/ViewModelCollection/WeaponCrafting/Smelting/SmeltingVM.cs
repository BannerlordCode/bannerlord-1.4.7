using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting
{
	// Token: 0x02000113 RID: 275
	public class SmeltingVM : ViewModel
	{
		// Token: 0x06001940 RID: 6464 RVA: 0x000600F0 File Offset: 0x0005E2F0
		public SmeltingVM(Action updateValuesOnSelectItemAction, Action updateValuesOnSmeltItemAction)
		{
			this.SortController = new SmeltingSortControllerVM();
			this._updateValuesOnSelectItemAction = updateValuesOnSelectItemAction;
			this._updateValuesOnSmeltItemAction = updateValuesOnSmeltItemAction;
			this._playerItemRoster = MobileParty.MainParty.ItemRoster;
			this._smithingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			IViewDataTracker campaignBehavior = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this._lockedItemIDs = campaignBehavior.GetInventoryLocks().ToList<string>();
			this.RefreshList();
			this.RefreshValues();
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x00060164 File Offset: 0x0005E364
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SelectAllHint = new HintViewModel(new TextObject("{=k1E9DuKi}Select All", null), null);
			SmeltingItemVM currentSelectedItem = this.CurrentSelectedItem;
			if (currentSelectedItem != null)
			{
				currentSelectedItem.RefreshValues();
			}
			this.SmeltableItemList.ApplyActionOnAllItems(delegate(SmeltingItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000601D4 File Offset: 0x0005E3D4
		internal void OnCraftingHeroChanged(CraftingAvailableHeroItemVM newHero)
		{
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000601D8 File Offset: 0x0005E3D8
		public void RefreshList()
		{
			this.SmeltableItemList = new MBBindingList<SmeltingItemVM>();
			this.SortController.SetListToControl(this.SmeltableItemList);
			for (int i = 0; i < this._playerItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = this._playerItemRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.EquipmentElement.Item.IsCraftedWeapon)
				{
					bool flag = this.IsItemLocked(elementCopyAtIndex.EquipmentElement);
					SmeltingItemVM smeltingItemVM = new SmeltingItemVM(elementCopyAtIndex.EquipmentElement, new Action<SmeltingItemVM>(this.OnItemSelection), new Action<SmeltingItemVM, bool>(this.ProcessLockItem), flag, elementCopyAtIndex.Amount);
					string id = smeltingItemVM.Visual.Id;
					SmeltingItemVM currentSelectedItem = this.CurrentSelectedItem;
					string text;
					if (currentSelectedItem == null)
					{
						text = null;
					}
					else
					{
						ItemImageIdentifierVM visual = currentSelectedItem.Visual;
						text = ((visual != null) ? visual.Id : null);
					}
					if (id == text)
					{
						this.OnItemSelection(smeltingItemVM);
					}
					this.SmeltableItemList.Add(smeltingItemVM);
				}
			}
			if (this.SmeltableItemList.Count == 0)
			{
				this.CurrentSelectedItem = null;
			}
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x000602DC File Offset: 0x0005E4DC
		private void OnItemSelection(SmeltingItemVM newItem)
		{
			if (newItem != this.CurrentSelectedItem)
			{
				if (this.CurrentSelectedItem != null)
				{
					this.CurrentSelectedItem.IsSelected = false;
				}
				this.CurrentSelectedItem = newItem;
				this.CurrentSelectedItem.IsSelected = true;
			}
			this._updateValuesOnSelectItemAction();
			WeaponDesign weaponDesign = this.CurrentSelectedItem.EquipmentElement.Item.WeaponDesign;
			this.WeaponTypeName = ((weaponDesign != null) ? weaponDesign.Template.TemplateName.ToString() : null) ?? string.Empty;
			WeaponDesign weaponDesign2 = this.CurrentSelectedItem.EquipmentElement.Item.WeaponDesign;
			this.WeaponTypeCode = ((weaponDesign2 != null) ? weaponDesign2.Template.StringId : null) ?? string.Empty;
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0006039C File Offset: 0x0005E59C
		public void TrySmeltingSelectedItems(Hero currentCraftingHero)
		{
			if (this._currentSelectedItem != null)
			{
				if (this._currentSelectedItem.IsLocked)
				{
					string text = new TextObject("{=wMiLUTNY}Are you sure you want to smelt this weapon? It is locked in the inventory.", null).ToString();
					InformationManager.ShowInquiry(new InquiryData("", text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						this.SmeltSelectedItems(currentCraftingHero);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				this.SmeltSelectedItems(currentCraftingHero);
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00060440 File Offset: 0x0005E640
		private void ProcessLockItem(SmeltingItemVM item, bool isLocked)
		{
			if (item == null)
			{
				return;
			}
			string itemLockStringID = CampaignUIHelper.GetItemLockStringID(item.EquipmentElement);
			if (isLocked && !this._lockedItemIDs.Contains(itemLockStringID))
			{
				this._lockedItemIDs.Add(itemLockStringID);
				return;
			}
			if (!isLocked && this._lockedItemIDs.Contains(itemLockStringID))
			{
				this._lockedItemIDs.Remove(itemLockStringID);
			}
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0006049C File Offset: 0x0005E69C
		private void SmeltSelectedItems(Hero currentCraftingHero)
		{
			if (this._currentSelectedItem != null && this._smithingBehavior != null)
			{
				ICraftingCampaignBehavior smithingBehavior = this._smithingBehavior;
				if (smithingBehavior != null)
				{
					smithingBehavior.DoSmelting(currentCraftingHero, this._currentSelectedItem.EquipmentElement);
				}
			}
			this.RefreshList();
			this.SortController.SortByCurrentState();
			if (this.CurrentSelectedItem != null)
			{
				int num = this.SmeltableItemList.FindIndex<SmeltingItemVM>((SmeltingItemVM i) => i.EquipmentElement.Item == this.CurrentSelectedItem.EquipmentElement.Item);
				SmeltingItemVM smeltingItemVM = ((num != -1) ? this.SmeltableItemList[num] : this.SmeltableItemList.FirstOrDefault<SmeltingItemVM>());
				this.OnItemSelection(smeltingItemVM);
			}
			this._updateValuesOnSmeltItemAction();
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00060538 File Offset: 0x0005E738
		private bool IsItemLocked(EquipmentElement equipmentElement)
		{
			string itemLockStringID = CampaignUIHelper.GetItemLockStringID(equipmentElement);
			return this._lockedItemIDs.Contains(itemLockStringID);
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00060558 File Offset: 0x0005E758
		public void SaveItemLockStates()
		{
			Campaign.Current.GetCampaignBehavior<IViewDataTracker>().SetInventoryLocks(this._lockedItemIDs);
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x0006056F File Offset: 0x0005E76F
		// (set) Token: 0x0600194B RID: 6475 RVA: 0x00060577 File Offset: 0x0005E777
		[DataSourceProperty]
		public string WeaponTypeName
		{
			get
			{
				return this._weaponTypeName;
			}
			set
			{
				if (value != this._weaponTypeName)
				{
					this._weaponTypeName = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponTypeName");
				}
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600194C RID: 6476 RVA: 0x0006059A File Offset: 0x0005E79A
		// (set) Token: 0x0600194D RID: 6477 RVA: 0x000605A2 File Offset: 0x0005E7A2
		[DataSourceProperty]
		public string WeaponTypeCode
		{
			get
			{
				return this._weaponTypeCode;
			}
			set
			{
				if (value != this._weaponTypeCode)
				{
					this._weaponTypeCode = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponTypeCode");
				}
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x000605C5 File Offset: 0x0005E7C5
		// (set) Token: 0x0600194F RID: 6479 RVA: 0x000605CD File Offset: 0x0005E7CD
		[DataSourceProperty]
		public SmeltingItemVM CurrentSelectedItem
		{
			get
			{
				return this._currentSelectedItem;
			}
			set
			{
				if (value != this._currentSelectedItem)
				{
					this._currentSelectedItem = value;
					base.OnPropertyChangedWithValue<SmeltingItemVM>(value, "CurrentSelectedItem");
					this.IsAnyItemSelected = value != null;
				}
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x000605F5 File Offset: 0x0005E7F5
		// (set) Token: 0x06001951 RID: 6481 RVA: 0x000605FD File Offset: 0x0005E7FD
		[DataSourceProperty]
		public bool IsAnyItemSelected
		{
			get
			{
				return this._isAnyItemSelected;
			}
			set
			{
				if (value != this._isAnyItemSelected)
				{
					this._isAnyItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyItemSelected");
				}
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x0006061B File Offset: 0x0005E81B
		// (set) Token: 0x06001953 RID: 6483 RVA: 0x00060623 File Offset: 0x0005E823
		[DataSourceProperty]
		public MBBindingList<SmeltingItemVM> SmeltableItemList
		{
			get
			{
				return this._smeltableItemList;
			}
			set
			{
				if (value != this._smeltableItemList)
				{
					this._smeltableItemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SmeltingItemVM>>(value, "SmeltableItemList");
				}
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00060641 File Offset: 0x0005E841
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00060649 File Offset: 0x0005E849
		[DataSourceProperty]
		public HintViewModel SelectAllHint
		{
			get
			{
				return this._selectAllHint;
			}
			set
			{
				if (value != this._selectAllHint)
				{
					this._selectAllHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SelectAllHint");
				}
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x00060667 File Offset: 0x0005E867
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x0006066F File Offset: 0x0005E86F
		[DataSourceProperty]
		public SmeltingSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<SmeltingSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000B9A RID: 2970
		private ItemRoster _playerItemRoster;

		// Token: 0x04000B9B RID: 2971
		private Action _updateValuesOnSelectItemAction;

		// Token: 0x04000B9C RID: 2972
		private Action _updateValuesOnSmeltItemAction;

		// Token: 0x04000B9D RID: 2973
		private List<string> _lockedItemIDs;

		// Token: 0x04000B9E RID: 2974
		private readonly ICraftingCampaignBehavior _smithingBehavior;

		// Token: 0x04000B9F RID: 2975
		private string _weaponTypeName;

		// Token: 0x04000BA0 RID: 2976
		private string _weaponTypeCode;

		// Token: 0x04000BA1 RID: 2977
		private SmeltingItemVM _currentSelectedItem;

		// Token: 0x04000BA2 RID: 2978
		private MBBindingList<SmeltingItemVM> _smeltableItemList;

		// Token: 0x04000BA3 RID: 2979
		private SmeltingSortControllerVM _sortController;

		// Token: 0x04000BA4 RID: 2980
		private HintViewModel _selectAllHint;

		// Token: 0x04000BA5 RID: 2981
		private bool _isAnyItemSelected;
	}
}
