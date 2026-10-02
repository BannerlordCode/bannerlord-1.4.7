using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000056 RID: 86
	public class MissionMainAgentEquipmentControllerVM : ViewModel
	{
		// Token: 0x0600070D RID: 1805 RVA: 0x00019AE4 File Offset: 0x00017CE4
		public MissionMainAgentEquipmentControllerVM(Action<EquipmentIndex> onDropEquipment, Action<SpawnedItemEntity, EquipmentIndex> onEquipItem)
		{
			this._onDropEquipment = onDropEquipment;
			this._onEquipItem = onEquipItem;
			this.DropActions = new MBBindingList<EquipmentActionItemVM>();
			this.EquipActions = new MBBindingList<EquipmentActionItemVM>();
			this.RefreshValues();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00019B32 File Offset: 0x00017D32
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._dropLocalizedText = GameTexts.FindText("str_inventory_drop", null);
			this._replaceWithLocalizedText = GameTexts.FindText("str_replace_with", null);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00019B5C File Offset: 0x00017D5C
		public void OnDropControllerToggle(bool isActive)
		{
			this.SelectedItemText = "";
			if (isActive && Agent.Main != null)
			{
				this.DropActions.Clear();
				this.DropActions.Add(new EquipmentActionItemVM(GameTexts.FindText("str_cancel", null).ToString(), "None", null, new Action<EquipmentActionItemVM>(this.OnItemSelected), false));
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					MissionWeapon missionWeapon = Agent.Main.Equipment[equipmentIndex];
					if (!missionWeapon.IsEmpty)
					{
						string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon.Item);
						bool flag = this.IsWieldedWeaponAtIndex(equipmentIndex);
						string weaponName = this.GetWeaponName(missionWeapon);
						this.DropActions.Add(new EquipmentActionItemVM(weaponName, itemTypeAsString, equipmentIndex, new Action<EquipmentActionItemVM>(this.OnItemSelected), flag));
					}
				}
			}
			else
			{
				EquipmentActionItemVM equipmentActionItemVM = this.DropActions.SingleOrDefault<EquipmentActionItemVM>((EquipmentActionItemVM a) => a.IsSelected);
				if (equipmentActionItemVM != null)
				{
					this.HandleDropItemActionSelection(equipmentActionItemVM.Identifier);
				}
			}
			this.IsDropControllerActive = isActive;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00019C74 File Offset: 0x00017E74
		private void HandleDropItemActionSelection(object selectedItem)
		{
			if (selectedItem is EquipmentIndex)
			{
				EquipmentIndex equipmentIndex = (EquipmentIndex)selectedItem;
				this._onDropEquipment(equipmentIndex);
				return;
			}
			if (selectedItem != null)
			{
				Debug.FailedAssert("Unidentified action on drop wheel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentEquipmentControllerVM.cs", "HandleDropItemActionSelection", 106);
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00019CB8 File Offset: 0x00017EB8
		public void SetCurrentFocusedWeaponEntity(SpawnedItemEntity weaponEntity)
		{
			this._focusedWeaponEntity = weaponEntity;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00019CC4 File Offset: 0x00017EC4
		public void OnEquipControllerToggle(bool isActive)
		{
			this.SelectedItemText = "";
			this.FocusedItemText = "";
			if (isActive && Agent.Main != null)
			{
				this.EquipActions.Clear();
				this.EquipActions.Add(new EquipmentActionItemVM(GameTexts.FindText("str_cancel", null).ToString(), "None", null, new Action<EquipmentActionItemVM>(this.OnItemSelected), false));
				if (this._focusedWeaponEntity.WeaponCopy.Item.Type == ItemObject.ItemTypeEnum.Shield && this.DoesPlayerHaveAtLeastOneShield())
				{
					this._pickText.SetTextVariable("ITEM_NAME", this._focusedWeaponEntity.WeaponCopy.Item.Name.ToString());
					this.FocusedItemText = this._pickText.ToString();
					for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
					{
						MissionWeapon missionWeapon = Agent.Main.Equipment[equipmentIndex];
						if (!missionWeapon.IsEmpty && missionWeapon.Item.Type == ItemObject.ItemTypeEnum.Shield)
						{
							string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon.Item);
							bool flag = this.IsWieldedWeaponAtIndex(equipmentIndex);
							string weaponName = this.GetWeaponName(missionWeapon);
							this.EquipActions.Add(new EquipmentActionItemVM(weaponName, itemTypeAsString, equipmentIndex, new Action<EquipmentActionItemVM>(this.OnItemSelected), flag));
						}
					}
				}
				else
				{
					Agent main = Agent.Main;
					if (main != null && main.CanInteractableWeaponBePickedUp(this._focusedWeaponEntity))
					{
						this._pickText.SetTextVariable("ITEM_NAME", this._focusedWeaponEntity.WeaponCopy.Item.Name.ToString());
						this.FocusedItemText = this._pickText.ToString();
						bool flag2 = Agent.Main.WillDropWieldedShield(this._focusedWeaponEntity);
						for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
						{
							MissionWeapon missionWeapon2 = Mission.Current.MainAgent.Equipment[equipmentIndex2];
							if (!missionWeapon2.IsEmpty && (!flag2 || missionWeapon2.IsShield()))
							{
								string itemTypeAsString2 = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(missionWeapon2.Item);
								bool flag3 = this.IsWieldedWeaponAtIndex(equipmentIndex2);
								string weaponName2 = this.GetWeaponName(missionWeapon2);
								this.EquipActions.Add(new EquipmentActionItemVM(weaponName2, itemTypeAsString2, equipmentIndex2, new Action<EquipmentActionItemVM>(this.OnItemSelected), flag3));
							}
						}
					}
					else
					{
						this.FocusedItemText = this._focusedWeaponEntity.WeaponCopy.Item.Name.ToString();
						EquipmentActionItemVM equipmentActionItemVM = new EquipmentActionItemVM(GameTexts.FindText("str_pickup_to_equip", null).ToString(), "PickUp", this._focusedWeaponEntity, new Action<EquipmentActionItemVM>(this.OnItemSelected), false)
						{
							IsSelected = true
						};
						this.EquipActions.Add(equipmentActionItemVM);
					}
				}
				EquipmentIndex itemIndexThatQuickPickUpWouldReplace = MissionEquipment.SelectWeaponPickUpSlot(Agent.Main, this._focusedWeaponEntity.WeaponCopy, this._focusedWeaponEntity.IsStuckMissile());
				EquipmentActionItemVM equipmentActionItemVM2 = this.EquipActions.SingleOrDefault<EquipmentActionItemVM>(delegate(EquipmentActionItemVM a)
				{
					object identifier;
					if ((identifier = a.Identifier) is EquipmentIndex)
					{
						EquipmentIndex equipmentIndex3 = (EquipmentIndex)identifier;
						return equipmentIndex3 == itemIndexThatQuickPickUpWouldReplace;
					}
					return false;
				});
				if (equipmentActionItemVM2 != null)
				{
					equipmentActionItemVM2.IsSelected = true;
				}
			}
			else
			{
				EquipmentActionItemVM equipmentActionItemVM3 = this.EquipActions.SingleOrDefault<EquipmentActionItemVM>((EquipmentActionItemVM a) => a.IsSelected);
				if (equipmentActionItemVM3 != null)
				{
					this.HandleEquipItemActionSelection(equipmentActionItemVM3.Identifier);
				}
			}
			this.IsEquipControllerActive = isActive;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001A016 File Offset: 0x00018216
		public void OnCancelEquipController()
		{
			this.IsEquipControllerActive = false;
			this.EquipActions.Clear();
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001A02A File Offset: 0x0001822A
		public void OnCancelDropController()
		{
			this.IsDropControllerActive = false;
			this.DropActions.Clear();
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001A040 File Offset: 0x00018240
		private void HandleEquipItemActionSelection(object selectedItem)
		{
			if (selectedItem is EquipmentIndex)
			{
				EquipmentIndex equipmentIndex = (EquipmentIndex)selectedItem;
				if (this._focusedWeaponEntity != null)
				{
					this._onEquipItem(this._focusedWeaponEntity, equipmentIndex);
					return;
				}
			}
			SpawnedItemEntity spawnedItemEntity;
			if ((spawnedItemEntity = selectedItem as SpawnedItemEntity) != null)
			{
				this._onEquipItem(spawnedItemEntity, EquipmentIndex.None);
				return;
			}
			if (selectedItem != null)
			{
				Debug.FailedAssert("Unidentified action on drop wheel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentEquipmentControllerVM.cs", "HandleEquipItemActionSelection", 223);
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001A0B0 File Offset: 0x000182B0
		private void OnItemSelected(EquipmentActionItemVM item)
		{
			if (this.IsEquipControllerActive)
			{
				if (item.Identifier == null || item.Identifier is SpawnedItemEntity)
				{
					this.EquipText = "";
				}
				else
				{
					this.EquipText = this._replaceWithLocalizedText.ToString();
				}
			}
			else if (item.Identifier == null)
			{
				this.DropText = "";
			}
			else
			{
				this.DropText = this._dropLocalizedText.ToString();
			}
			this.SelectedItemText = item.ActionText;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001A12C File Offset: 0x0001832C
		private string GetWeaponName(MissionWeapon weapon)
		{
			string text = weapon.Item.Name.ToString();
			WeaponComponentData currentUsageItem = weapon.CurrentUsageItem;
			if (currentUsageItem != null && currentUsageItem.IsShield)
			{
				text = string.Concat(new object[] { text, " (", weapon.HitPoints, " / ", weapon.ModifiedMaxHitPoints, ")" });
			}
			else
			{
				WeaponComponentData currentUsageItem2 = weapon.CurrentUsageItem;
				if (currentUsageItem2 != null && currentUsageItem2.IsConsumable && weapon.ModifiedMaxAmount > 1)
				{
					text = string.Concat(new object[] { text, " (", weapon.Amount, " / ", weapon.ModifiedMaxAmount, ")" });
				}
			}
			return text;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001A20E File Offset: 0x0001840E
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0001A216 File Offset: 0x00018416
		[DataSourceProperty]
		public bool IsDropControllerActive
		{
			get
			{
				return this._isDropControllerActive;
			}
			set
			{
				if (value != this._isDropControllerActive)
				{
					this._isDropControllerActive = value;
					base.OnPropertyChangedWithValue(value, "IsDropControllerActive");
				}
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001A234 File Offset: 0x00018434
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0001A23C File Offset: 0x0001843C
		[DataSourceProperty]
		public bool IsEquipControllerActive
		{
			get
			{
				return this._isEquipControllerActive;
			}
			set
			{
				if (value != this._isEquipControllerActive)
				{
					this._isEquipControllerActive = value;
					base.OnPropertyChangedWithValue(value, "IsEquipControllerActive");
				}
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001A25A File Offset: 0x0001845A
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001A262 File Offset: 0x00018462
		[DataSourceProperty]
		public string DropText
		{
			get
			{
				return this._dropText;
			}
			set
			{
				if (value != this._dropText)
				{
					this._dropText = value;
					base.OnPropertyChangedWithValue<string>(value, "DropText");
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001A285 File Offset: 0x00018485
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0001A28D File Offset: 0x0001848D
		[DataSourceProperty]
		public string EquipText
		{
			get
			{
				return this._equipText;
			}
			set
			{
				if (value != this._equipText)
				{
					this._equipText = value;
					base.OnPropertyChangedWithValue<string>(value, "EquipText");
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001A2B0 File Offset: 0x000184B0
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001A2B8 File Offset: 0x000184B8
		[DataSourceProperty]
		public string FocusedItemText
		{
			get
			{
				return this._focusedItemText;
			}
			set
			{
				if (value != this._focusedItemText)
				{
					this._focusedItemText = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusedItemText");
				}
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001A2DB File Offset: 0x000184DB
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001A2E3 File Offset: 0x000184E3
		[DataSourceProperty]
		public string SelectedItemText
		{
			get
			{
				return this._selectedItemText;
			}
			set
			{
				if (value != this._selectedItemText)
				{
					this._selectedItemText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedItemText");
				}
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001A306 File Offset: 0x00018506
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001A30E File Offset: 0x0001850E
		[DataSourceProperty]
		public MBBindingList<EquipmentActionItemVM> DropActions
		{
			get
			{
				return this._dropActions;
			}
			set
			{
				if (value != this._dropActions)
				{
					this._dropActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<EquipmentActionItemVM>>(value, "DropActions");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001A32C File Offset: 0x0001852C
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001A334 File Offset: 0x00018534
		[DataSourceProperty]
		public MBBindingList<EquipmentActionItemVM> EquipActions
		{
			get
			{
				return this._equipActions;
			}
			set
			{
				if (value != this._equipActions)
				{
					this._equipActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<EquipmentActionItemVM>>(value, "EquipActions");
				}
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001A354 File Offset: 0x00018554
		public static string GetItemTypeAsString(ItemObject item)
		{
			if (item.ItemComponent is WeaponComponent)
			{
				switch ((item.ItemComponent as WeaponComponent).PrimaryWeapon.WeaponClass)
				{
				case WeaponClass.Dagger:
				case WeaponClass.OneHandedSword:
				case WeaponClass.TwoHandedSword:
					return "Sword";
				case WeaponClass.OneHandedAxe:
				case WeaponClass.TwoHandedAxe:
					return "Axe";
				case WeaponClass.Mace:
				case WeaponClass.TwoHandedMace:
					return "Mace";
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
					return "Spear";
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
				case WeaponClass.SlingStone:
				case WeaponClass.Cartridge:
				case WeaponClass.Musket:
					return "Ammo";
				case WeaponClass.Bow:
					return "Bow";
				case WeaponClass.Crossbow:
					return "Crossbow";
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
					return "Stone";
				case WeaponClass.ThrowingAxe:
					return "ThrowingAxe";
				case WeaponClass.ThrowingKnife:
					return "ThrowingKnife";
				case WeaponClass.Javelin:
					return "Javelin";
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					return "Shield";
				case WeaponClass.Banner:
					return "Banner";
				}
				return "None";
			}
			if (item.ItemComponent is HorseComponent)
			{
				return "Mount";
			}
			return "None";
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001A478 File Offset: 0x00018678
		private bool DoesPlayerHaveAtLeastOneShield()
		{
			EquipmentIndex offhandWieldedItemIndex = Agent.Main.GetOffhandWieldedItemIndex();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (equipmentIndex != offhandWieldedItemIndex && !Agent.Main.Equipment[equipmentIndex].IsEmpty && Mission.Current.MainAgent.Equipment[equipmentIndex].Item.Type == ItemObject.ItemTypeEnum.Shield)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001A4E2 File Offset: 0x000186E2
		private bool IsWieldedWeaponAtIndex(EquipmentIndex index)
		{
			return index == Agent.Main.GetPrimaryWieldedItemIndex() || index == Agent.Main.GetOffhandWieldedItemIndex();
		}

		// Token: 0x04000320 RID: 800
		private TextObject _replaceWithLocalizedText;

		// Token: 0x04000321 RID: 801
		private TextObject _dropLocalizedText;

		// Token: 0x04000322 RID: 802
		private SpawnedItemEntity _focusedWeaponEntity;

		// Token: 0x04000323 RID: 803
		private readonly Action<EquipmentIndex> _onDropEquipment;

		// Token: 0x04000324 RID: 804
		private readonly Action<SpawnedItemEntity, EquipmentIndex> _onEquipItem;

		// Token: 0x04000325 RID: 805
		private readonly TextObject _pickText = new TextObject("{=d5SNB0HV}Pick {ITEM_NAME}", null);

		// Token: 0x04000326 RID: 806
		private bool _isDropControllerActive;

		// Token: 0x04000327 RID: 807
		private bool _isEquipControllerActive;

		// Token: 0x04000328 RID: 808
		private string _selectedItemText;

		// Token: 0x04000329 RID: 809
		private string _dropText;

		// Token: 0x0400032A RID: 810
		private string _equipText;

		// Token: 0x0400032B RID: 811
		private string _focusedItemText;

		// Token: 0x0400032C RID: 812
		private MBBindingList<EquipmentActionItemVM> _dropActions;

		// Token: 0x0400032D RID: 813
		private MBBindingList<EquipmentActionItemVM> _equipActions;

		// Token: 0x020000EF RID: 239
		public enum ItemGroup
		{
			// Token: 0x04000664 RID: 1636
			None,
			// Token: 0x04000665 RID: 1637
			Spear,
			// Token: 0x04000666 RID: 1638
			Javelin,
			// Token: 0x04000667 RID: 1639
			Bow,
			// Token: 0x04000668 RID: 1640
			Crossbow,
			// Token: 0x04000669 RID: 1641
			Sword,
			// Token: 0x0400066A RID: 1642
			Axe,
			// Token: 0x0400066B RID: 1643
			Mace,
			// Token: 0x0400066C RID: 1644
			ThrowingAxe,
			// Token: 0x0400066D RID: 1645
			ThrowingKnife,
			// Token: 0x0400066E RID: 1646
			Ammo,
			// Token: 0x0400066F RID: 1647
			Shield,
			// Token: 0x04000670 RID: 1648
			Mount,
			// Token: 0x04000671 RID: 1649
			Banner,
			// Token: 0x04000672 RID: 1650
			Stone
		}
	}
}
