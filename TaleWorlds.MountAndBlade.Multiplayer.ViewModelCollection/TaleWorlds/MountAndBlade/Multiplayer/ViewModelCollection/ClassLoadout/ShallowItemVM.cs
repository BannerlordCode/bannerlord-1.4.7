using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AA RID: 170
	public class ShallowItemVM : ViewModel
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0003218A File Offset: 0x0003038A
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x00032192 File Offset: 0x00030392
		public ShallowItemVM.ItemGroup Type { get; private set; }

		// Token: 0x06001035 RID: 4149 RVA: 0x0003219C File Offset: 0x0003039C
		public ShallowItemVM(Action<ShallowItemVM> onSelect)
		{
			this.ItemInformationList = new MBBindingList<ShallowItemVM.ArmoryItemFlagVM>();
			this.PropertyList = new MBBindingList<ShallowItemPropertyVM>();
			this.AlternativeUsageSelector = new SelectorVM<AlternativeUsageItemOptionVM>(new List<string>(), 0, new Action<SelectorVM<AlternativeUsageItemOptionVM>>(this.OnAlternativeUsageChanged));
			this._onSelect = onSelect;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x000321EC File Offset: 0x000303EC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshWith(this._equipmentIndex, this._equipment);
			this.PropertyList.ApplyActionOnAllItems(delegate(ShallowItemPropertyVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0003223B File Offset: 0x0003043B
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._equipment = null;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003224C File Offset: 0x0003044C
		public void RefreshWith(EquipmentIndex equipmentIndex, Equipment equipment)
		{
			this._equipment = equipment;
			this._equipmentIndex = equipmentIndex;
			ItemObject itemObject = ((equipment != null) ? equipment[equipmentIndex].Item : null);
			if (itemObject == null || (equipmentIndex == EquipmentIndex.ArmorItemEndSlot && !itemObject.HasHorseComponent) || (equipmentIndex != EquipmentIndex.ArmorItemEndSlot && (itemObject.PrimaryWeapon == null || itemObject.PrimaryWeapon.IsAmmo)))
			{
				this.IsValid = false;
				this.Icon = new ItemImageIdentifierVM(null, "");
				return;
			}
			this.IsValid = true;
			this.Name = itemObject.Name.ToString();
			this.Icon = new ItemImageIdentifierVM(itemObject, "");
			this.Type = ShallowItemVM.GetItemGroupType(itemObject);
			this.TypeAsString = ((this.Type == ShallowItemVM.ItemGroup.None) ? "" : this.Type.ToString());
			this.HasAnyAlternativeUsage = false;
			this.AlternativeUsageSelector.ItemList.Clear();
			if (itemObject.PrimaryWeapon != null)
			{
				for (int i = 0; i < itemObject.Weapons.Count; i++)
				{
					WeaponComponentData weaponComponentData = itemObject.Weapons[i];
					if (ShallowItemVM.IsItemUsageApplicable(weaponComponentData))
					{
						TextObject textObject = GameTexts.FindText("str_weapon_usage", weaponComponentData.WeaponDescriptionId);
						this.AlternativeUsageSelector.AddItem(new AlternativeUsageItemOptionVM(weaponComponentData.WeaponDescriptionId, textObject, textObject, this.AlternativeUsageSelector, i));
						this.HasAnyAlternativeUsage = true;
					}
				}
			}
			this.AlternativeUsageSelector.SelectedIndex = -1;
			this.AlternativeUsageSelector.SelectedIndex = 0;
			this._latestUsageOption = this.AlternativeUsageSelector.ItemList.FirstOrDefault<AlternativeUsageItemOptionVM>();
			if (this._latestUsageOption != null)
			{
				this._latestUsageOption.IsSelected = true;
			}
			this.AlternativeUsageSelector.SetOnChangeAction(new Action<SelectorVM<AlternativeUsageItemOptionVM>>(this.OnAlternativeUsageChanged));
			this.RefreshItemPropertyList(this._equipmentIndex, this._equipment, this.AlternativeUsageSelector.SelectedIndex);
			this._isInitialized = true;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00032424 File Offset: 0x00030624
		private void OnAlternativeUsageChanged(SelectorVM<AlternativeUsageItemOptionVM> selector)
		{
			if (this._isInitialized && selector.SelectedIndex >= 0)
			{
				if (this._latestUsageOption != null)
				{
					this._latestUsageOption.IsSelected = false;
				}
				this.RefreshItemPropertyList(this._equipmentIndex, this._equipment, selector.SelectedIndex);
				if (selector.SelectedItem != null)
				{
					selector.SelectedItem.IsSelected = true;
				}
			}
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00032484 File Offset: 0x00030684
		private void RefreshItemPropertyList(EquipmentIndex equipmentIndex, Equipment equipment, int alternativeIndex)
		{
			ItemObject item = equipment[equipmentIndex].Item;
			ItemModifier itemModifier = equipment[equipmentIndex].ItemModifier;
			this.PropertyList.Clear();
			if (item.PrimaryWeapon != null)
			{
				WeaponComponentData weaponComponentData = item.Weapons[alternativeIndex];
				ItemObject.ItemTypeEnum itemTypeFromWeaponClass = WeaponComponentData.GetItemTypeFromWeaponClass(weaponComponentData.WeaponClass);
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.OneHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Polearm)
				{
					if (weaponComponentData.SwingDamageType != DamageTypes.Invalid)
					{
						this.AddProperty(new TextObject("{=yJsE4Ayo}Swing Spd.", null), (float)weaponComponentData.GetModifiedSwingSpeed(itemModifier) / 145f, weaponComponentData.GetModifiedSwingSpeed(itemModifier));
						this.AddProperty(new TextObject("{=RNgWFLIO}Swing Dmg.", null), (float)weaponComponentData.GetModifiedSwingDamage(itemModifier) / 143f, weaponComponentData.GetModifiedSwingDamage(itemModifier));
					}
					if (weaponComponentData.ThrustDamageType != DamageTypes.Invalid)
					{
						this.AddProperty(new TextObject("{=J0vjDOFO}Thrust Spd.", null), (float)weaponComponentData.GetModifiedThrustSpeed(itemModifier) / 114f, weaponComponentData.GetModifiedThrustSpeed(itemModifier));
						this.AddProperty(new TextObject("{=Ie9I2Bha}Thrust Dmg.", null), (float)weaponComponentData.GetModifiedThrustDamage(itemModifier) / 86f, weaponComponentData.GetModifiedThrustDamage(itemModifier));
					}
					this.AddProperty(new TextObject("{=ftoSCQ0x}Length", null), (float)weaponComponentData.WeaponLength / 315f, weaponComponentData.WeaponLength);
					this.AddProperty(new TextObject("{=oibdTnXP}Handling", null), (float)weaponComponentData.GetModifiedHandling(itemModifier) / 120f, weaponComponentData.GetModifiedHandling(itemModifier));
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Thrown)
				{
					this.AddProperty(new TextObject("{=ftoSCQ0x}Length", null), (float)weaponComponentData.WeaponLength / 147f, weaponComponentData.WeaponLength);
					this.AddProperty(new TextObject("{=s31DnnAf}Damage", null), (float)weaponComponentData.GetModifiedThrustDamage(itemModifier) / 94f, weaponComponentData.GetModifiedThrustDamage(itemModifier));
					this.AddProperty(new TextObject("{=QfTt7YRB}Fire Rate", null), (float)weaponComponentData.GetModifiedMissileSpeed(itemModifier) / 115f, weaponComponentData.GetModifiedMissileSpeed(itemModifier));
					this.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null), (float)weaponComponentData.Accuracy / 300f, weaponComponentData.Accuracy);
					this.AddProperty(new TextObject("{=b31ITmm0}Stack Amnt.", null), (float)weaponComponentData.GetModifiedStackCount(itemModifier) / 40f, (int)weaponComponentData.GetModifiedStackCount(itemModifier));
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Shield)
				{
					this.AddProperty(new TextObject("{=6GSXsdeX}Speed", null), (float)weaponComponentData.GetModifiedThrustSpeed(itemModifier) / 120f, weaponComponentData.GetModifiedThrustSpeed(itemModifier));
					this.AddProperty(new TextObject("{=GGseMDd3}Durability", null), (float)weaponComponentData.GetModifiedMaximumHitPoints(itemModifier) / 500f, (int)weaponComponentData.GetModifiedMaximumHitPoints(itemModifier));
					this.AddProperty(new TextObject("{=ahiBhAqU}Armor", null), (float)weaponComponentData.GetModifiedArmor(itemModifier) / 40f, weaponComponentData.GetModifiedArmor(itemModifier));
					this.AddProperty(new TextObject("{=4Dd2xgPm}Weight", null), item.Weight / 40f, (int)item.Weight);
				}
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Bow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Sling)
				{
					int num = 0;
					float num2 = 0f;
					int num3 = 0;
					for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
					{
						ItemObject item2 = equipment[equipmentIndex2].Item;
						ItemModifier itemModifier2 = equipment[equipmentIndex2].ItemModifier;
						if (item2 != null && item2.PrimaryWeapon.IsAmmo)
						{
							num += (int)item2.PrimaryWeapon.GetModifiedStackCount(itemModifier2);
							num3 += item2.PrimaryWeapon.GetModifiedThrustDamage(itemModifier2);
							num2 += 1f;
						}
					}
					num3 = MathF.Round((float)num3 / num2);
					this.AddProperty(new TextObject("{=ftoSCQ0x}Length", null), (float)weaponComponentData.WeaponLength / 123f, weaponComponentData.WeaponLength);
					this.AddProperty(new TextObject("{=s31DnnAf}Damage", null), (float)(weaponComponentData.GetModifiedThrustDamage(itemModifier) + num3) / 70f, weaponComponentData.GetModifiedThrustDamage(itemModifier) + num3);
					this.AddProperty(new TextObject("{=QfTt7YRB}Fire Rate", null), (float)weaponComponentData.GetModifiedSwingSpeed(itemModifier) / 120f, weaponComponentData.GetModifiedSwingSpeed(itemModifier));
					this.AddProperty(new TextObject("{=TAnabTdy}Accuracy", null), (float)weaponComponentData.Accuracy / 105f, weaponComponentData.Accuracy);
					this.AddProperty(new TextObject("{=yUpH2mQ4}Ammo", null), (float)num / 90f, num);
				}
				this.ItemInformationList.Clear();
				List<ValueTuple<string, TextObject>> weaponFlagDetails = ShallowItemVM.GetWeaponFlagDetails(weaponComponentData.WeaponFlags);
				for (int i = 0; i < weaponFlagDetails.Count; i++)
				{
					ShallowItemVM.ArmoryItemFlagVM armoryItemFlagVM = new ShallowItemVM.ArmoryItemFlagVM(weaponFlagDetails[i].Item1, weaponFlagDetails[i].Item2);
					this.ItemInformationList.Add(armoryItemFlagVM);
				}
			}
			if (item.HorseComponent != null)
			{
				EquipmentElement equipmentElement = equipment[EquipmentIndex.ArmorItemEndSlot];
				EquipmentElement equipmentElement2 = equipment[EquipmentIndex.HorseHarness];
				int modifiedMountCharge = equipmentElement.GetModifiedMountCharge(in equipmentElement2);
				int num4 = (int)(4.33f * (float)equipmentElement.GetModifiedMountSpeed(in equipmentElement2));
				int modifiedMountManeuver = equipmentElement.GetModifiedMountManeuver(in equipmentElement2);
				int modifiedMountHitPoints = equipmentElement.GetModifiedMountHitPoints();
				int modifiedMountBodyArmor = equipmentElement2.GetModifiedMountBodyArmor();
				this.AddProperty(new TextObject("{=DAVb2Pzg}Charge Dmg.", null), (float)modifiedMountCharge / 35f, modifiedMountCharge);
				this.AddProperty(new TextObject("{=6GSXsdeX}Speed", null), (float)num4 / 303.1f, num4);
				this.AddProperty(new TextObject("{=rg7OuWS2}Maneuver", null), (float)modifiedMountManeuver / 70f, modifiedMountManeuver);
				this.AddProperty(new TextObject("{=oBbiVeKE}Hit Points", null), (float)modifiedMountHitPoints / 300f, modifiedMountHitPoints);
				this.AddProperty(new TextObject("{=kftE5nvv}Horse Armor", null), (float)modifiedMountBodyArmor / 100f, modifiedMountBodyArmor);
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x000329E8 File Offset: 0x00030BE8
		private static List<ValueTuple<string, TextObject>> GetWeaponFlagDetails(WeaponFlags weaponFlags)
		{
			List<ValueTuple<string, TextObject>> list = new List<ValueTuple<string, TextObject>>();
			if (weaponFlags.HasAnyFlag(WeaponFlags.BonusAgainstShield))
			{
				string text = "WeaponFlagIcons\\bonus_against_shield";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_bonus_against_shield", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown))
			{
				string text = "WeaponFlagIcons\\can_knock_down";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_knockdown", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanDismount) && !weaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				string text = "WeaponFlagIcons\\can_dismount";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_dismount", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanHook) && !weaponFlags.HasAnyFlag(WeaponFlags.CanDismount))
			{
				string text = "WeaponFlagIcons\\can_dismount";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_hook", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAllFlags(WeaponFlags.CanDismount | WeaponFlags.CanHook))
			{
				string text = "WeaponFlagIcons\\can_dismount";
				TextObject textObject = new TextObject("{=7HA99oUg}Both swing and thrust attacks can dismount riders", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CanCrushThrough))
			{
				string text = "WeaponFlagIcons\\can_crush_through";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_can_crush_through", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithTwoHand))
			{
				string text = "WeaponFlagIcons\\not_usable_with_two_hand";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_not_usable_two_hand", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand))
			{
				string text = "WeaponFlagIcons\\not_usable_with_one_hand";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_not_usable_one_hand", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			if (weaponFlags.HasAnyFlag(WeaponFlags.CantReloadOnHorseback))
			{
				string text = "WeaponFlagIcons\\cant_reload_on_horseback";
				TextObject textObject = GameTexts.FindText("str_inventory_flag_cant_reload_on_horseback", null);
				list.Add(new ValueTuple<string, TextObject>(text, textObject));
			}
			return list;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00032BA7 File Offset: 0x00030DA7
		private void AddProperty(TextObject name, float fraction, int value)
		{
			this.PropertyList.Add(new ShallowItemPropertyVM(name, MathF.Round(fraction * 1000f), value));
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00032BC8 File Offset: 0x00030DC8
		private static ShallowItemVM.ItemGroup GetItemGroupType(ItemObject item)
		{
			if (item.WeaponComponent != null)
			{
				switch (item.WeaponComponent.PrimaryWeapon.WeaponClass)
				{
				case WeaponClass.OneHandedSword:
				case WeaponClass.TwoHandedSword:
					return ShallowItemVM.ItemGroup.Sword;
				case WeaponClass.OneHandedAxe:
				case WeaponClass.TwoHandedAxe:
					return ShallowItemVM.ItemGroup.Axe;
				case WeaponClass.Mace:
				case WeaponClass.TwoHandedMace:
					return ShallowItemVM.ItemGroup.Mace;
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
					return ShallowItemVM.ItemGroup.Spear;
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
				case WeaponClass.SlingStone:
				case WeaponClass.Cartridge:
				case WeaponClass.Musket:
					return ShallowItemVM.ItemGroup.Ammo;
				case WeaponClass.Bow:
					return ShallowItemVM.ItemGroup.Bow;
				case WeaponClass.Crossbow:
					return ShallowItemVM.ItemGroup.Crossbow;
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
					return ShallowItemVM.ItemGroup.Stone;
				case WeaponClass.ThrowingAxe:
					return ShallowItemVM.ItemGroup.ThrowingAxe;
				case WeaponClass.ThrowingKnife:
					return ShallowItemVM.ItemGroup.ThrowingKnife;
				case WeaponClass.Javelin:
					return ShallowItemVM.ItemGroup.Javelin;
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					return ShallowItemVM.ItemGroup.Shield;
				}
				return ShallowItemVM.ItemGroup.None;
			}
			if (item.HasHorseComponent)
			{
				return ShallowItemVM.ItemGroup.Mount;
			}
			return ShallowItemVM.ItemGroup.None;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00032C95 File Offset: 0x00030E95
		[UsedImplicitly]
		public void OnSelect()
		{
			this._onSelect(this);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00032CA3 File Offset: 0x00030EA3
		public static bool IsItemUsageApplicable(WeaponComponentData weapon)
		{
			WeaponDescription weaponDescription = ((weapon != null && weapon.WeaponDescriptionId != null) ? MBObjectManager.Instance.GetObject<WeaponDescription>(weapon.WeaponDescriptionId) : null);
			return weaponDescription != null && !weaponDescription.IsHiddenFromUI;
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x00032CD1 File Offset: 0x00030ED1
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x00032CD9 File Offset: 0x00030ED9
		[DataSourceProperty]
		public MBBindingList<ShallowItemVM.ArmoryItemFlagVM> ItemInformationList
		{
			get
			{
				return this._itemInformationList;
			}
			set
			{
				if (value != this._itemInformationList)
				{
					this._itemInformationList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ShallowItemVM.ArmoryItemFlagVM>>(value, "ItemInformationList");
				}
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00032CF7 File Offset: 0x00030EF7
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00032CFF File Offset: 0x00030EFF
		[DataSourceProperty]
		public MBBindingList<ShallowItemPropertyVM> PropertyList
		{
			get
			{
				return this._propertyList;
			}
			set
			{
				if (value != this._propertyList)
				{
					this._propertyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ShallowItemPropertyVM>>(value, "PropertyList");
				}
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00032D1D File Offset: 0x00030F1D
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x00032D25 File Offset: 0x00030F25
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

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x00032D48 File Offset: 0x00030F48
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x00032D50 File Offset: 0x00030F50
		[DataSourceProperty]
		public ItemImageIdentifierVM Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				if (value != this._icon)
				{
					this._icon = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Icon");
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x00032D6E File Offset: 0x00030F6E
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x00032D76 File Offset: 0x00030F76
		[DataSourceProperty]
		public string TypeAsString
		{
			get
			{
				return this._typeAsString;
			}
			set
			{
				if (value != this._typeAsString)
				{
					this._typeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeAsString");
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x00032D99 File Offset: 0x00030F99
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x00032DA1 File Offset: 0x00030FA1
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

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x00032DBF File Offset: 0x00030FBF
		// (set) Token: 0x0600104D RID: 4173 RVA: 0x00032DC7 File Offset: 0x00030FC7
		[DataSourceProperty]
		public bool HasAnyAlternativeUsage
		{
			get
			{
				return this._hasAnyAlternativeUsage;
			}
			set
			{
				if (value != this._hasAnyAlternativeUsage)
				{
					this._hasAnyAlternativeUsage = value;
					base.OnPropertyChangedWithValue(value, "HasAnyAlternativeUsage");
				}
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x00032DE5 File Offset: 0x00030FE5
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x00032DED File Offset: 0x00030FED
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (value != this._isValid)
				{
					this._isValid = value;
					base.OnPropertyChangedWithValue(value, "IsValid");
				}
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x00032E0B File Offset: 0x0003100B
		// (set) Token: 0x06001051 RID: 4177 RVA: 0x00032E13 File Offset: 0x00031013
		[DataSourceProperty]
		public SelectorVM<AlternativeUsageItemOptionVM> AlternativeUsageSelector
		{
			get
			{
				return this._alternativeUsageSelector;
			}
			set
			{
				if (value != this._alternativeUsageSelector)
				{
					this._alternativeUsageSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<AlternativeUsageItemOptionVM>>(value, "AlternativeUsageSelector");
				}
			}
		}

		// Token: 0x0400078D RID: 1933
		private readonly Action<ShallowItemVM> _onSelect;

		// Token: 0x0400078F RID: 1935
		private AlternativeUsageItemOptionVM _latestUsageOption;

		// Token: 0x04000790 RID: 1936
		private Equipment _equipment;

		// Token: 0x04000791 RID: 1937
		private EquipmentIndex _equipmentIndex;

		// Token: 0x04000792 RID: 1938
		private bool _isInitialized;

		// Token: 0x04000793 RID: 1939
		private ItemImageIdentifierVM _icon;

		// Token: 0x04000794 RID: 1940
		private string _name;

		// Token: 0x04000795 RID: 1941
		private string _typeAsString;

		// Token: 0x04000796 RID: 1942
		private bool _isValid;

		// Token: 0x04000797 RID: 1943
		private bool _isSelected;

		// Token: 0x04000798 RID: 1944
		private bool _hasAnyAlternativeUsage;

		// Token: 0x04000799 RID: 1945
		private MBBindingList<ShallowItemVM.ArmoryItemFlagVM> _itemInformationList;

		// Token: 0x0400079A RID: 1946
		private MBBindingList<ShallowItemPropertyVM> _propertyList;

		// Token: 0x0400079B RID: 1947
		private SelectorVM<AlternativeUsageItemOptionVM> _alternativeUsageSelector;

		// Token: 0x0200018C RID: 396
		public enum ItemGroup
		{
			// Token: 0x04000A3E RID: 2622
			None,
			// Token: 0x04000A3F RID: 2623
			Spear,
			// Token: 0x04000A40 RID: 2624
			Javelin,
			// Token: 0x04000A41 RID: 2625
			Bow,
			// Token: 0x04000A42 RID: 2626
			Crossbow,
			// Token: 0x04000A43 RID: 2627
			Sword,
			// Token: 0x04000A44 RID: 2628
			Axe,
			// Token: 0x04000A45 RID: 2629
			Mace,
			// Token: 0x04000A46 RID: 2630
			ThrowingAxe,
			// Token: 0x04000A47 RID: 2631
			ThrowingKnife,
			// Token: 0x04000A48 RID: 2632
			Ammo,
			// Token: 0x04000A49 RID: 2633
			Shield,
			// Token: 0x04000A4A RID: 2634
			Mount,
			// Token: 0x04000A4B RID: 2635
			Stone
		}

		// Token: 0x0200018D RID: 397
		public class ArmoryItemFlagVM : ViewModel
		{
			// Token: 0x060012ED RID: 4845 RVA: 0x0003AE8E File Offset: 0x0003908E
			public ArmoryItemFlagVM(string icon, TextObject hintText)
			{
				this.Icon = "SPGeneral\\" + icon;
				this.Hint = new HintViewModel(hintText, null);
			}

			// Token: 0x170005A9 RID: 1449
			// (get) Token: 0x060012EE RID: 4846 RVA: 0x0003AEB4 File Offset: 0x000390B4
			// (set) Token: 0x060012EF RID: 4847 RVA: 0x0003AEBC File Offset: 0x000390BC
			[DataSourceProperty]
			public string Icon
			{
				get
				{
					return this._icon;
				}
				set
				{
					if (value != this._icon)
					{
						this._icon = value;
						base.OnPropertyChangedWithValue<string>(value, "Icon");
					}
				}
			}

			// Token: 0x170005AA RID: 1450
			// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0003AEDF File Offset: 0x000390DF
			// (set) Token: 0x060012F1 RID: 4849 RVA: 0x0003AEE7 File Offset: 0x000390E7
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

			// Token: 0x04000A4C RID: 2636
			private string _icon;

			// Token: 0x04000A4D RID: 2637
			private HintViewModel _hint;
		}
	}
}
