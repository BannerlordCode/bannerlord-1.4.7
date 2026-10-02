using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A4 RID: 164
	public class HeroInformationVM : ViewModel
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x00030215 File Offset: 0x0002E415
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x0003021D File Offset: 0x0002E41D
		public MultiplayerClassDivisions.MPHeroClass HeroClass { get; private set; }

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00030228 File Offset: 0x0002E428
		public HeroInformationVM()
		{
			this._latestSelectedItemGroup = ShallowItemVM.ItemGroup.None;
			this.Item1 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.Item2 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.Item3 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.Item4 = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.ItemHorse = new ShallowItemVM(new Action<ShallowItemVM>(this.UpdateHighlightedItem));
			this.IsArmyAvailable = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0;
			this.SetFirstSelectedItem();
			this.RefreshValues();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000302E4 File Offset: 0x0002E4E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmySizeHint = new HintViewModel(GameTexts.FindText("str_army_size", null), null);
			this.MovementSpeedHint = new HintViewModel(GameTexts.FindText("str_movement_speed", null), null);
			this.HitPointsHint = new HintViewModel(GameTexts.FindText("str_hitpoints", null), null);
			this.ArmorHint = new HintViewModel(GameTexts.FindText("str_armor", null), null);
			this.EquipmentText = GameTexts.FindText("str_equipment", null).ToString();
			ShallowItemVM item = this._item1;
			if (item != null)
			{
				item.RefreshValues();
			}
			ShallowItemVM item2 = this._item2;
			if (item2 != null)
			{
				item2.RefreshValues();
			}
			ShallowItemVM item3 = this._item3;
			if (item3 != null)
			{
				item3.RefreshValues();
			}
			ShallowItemVM item4 = this._item4;
			if (item4 != null)
			{
				item4.RefreshValues();
			}
			ShallowItemVM itemHorse = this._itemHorse;
			if (itemHorse != null)
			{
				itemHorse.RefreshValues();
			}
			ShallowItemVM itemSelected = this._itemSelected;
			if (itemSelected != null)
			{
				itemSelected.RefreshValues();
			}
			if (this.HeroClass != null)
			{
				this.NameText = this.HeroClass.HeroName.ToString();
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000303F0 File Offset: 0x0002E5F0
		public void RefreshWith(MultiplayerClassDivisions.MPHeroClass heroClass, List<IReadOnlyPerkObject> perks)
		{
			this.HeroClass = heroClass;
			Equipment equipment = heroClass.HeroCharacter.Equipment.Clone(false);
			MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(perks);
			IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
			if (enumerable != null)
			{
				foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
				{
					equipment[valueTuple.Item1] = valueTuple.Item2;
				}
			}
			this.ItemHorse.RefreshWith(EquipmentIndex.ArmorItemEndSlot, equipment);
			this.Item1.RefreshWith(EquipmentIndex.WeaponItemBeginSlot, equipment);
			this.Item2.RefreshWith(EquipmentIndex.Weapon1, equipment);
			this.Item3.RefreshWith(EquipmentIndex.Weapon2, equipment);
			this.Item4.RefreshWith(EquipmentIndex.Weapon3, equipment);
			TextObject heroInformation = heroClass.HeroInformation;
			this.Information = ((heroInformation != null) ? heroInformation.ToString() : null);
			this.NameText = heroClass.HeroName.ToString();
			int num = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			if (num == 0)
			{
				num = 25;
				this._armySizeHintWithDefaultValue.SetTextVariable("OPTION_VALUE", 25);
				this.ArmySizeHint.HintText = this._armySizeHintWithDefaultValue;
			}
			else
			{
				this.ArmySizeHint.HintText = GameTexts.FindText("str_army_size", null);
			}
			this.ArmySize = MPPerkObject.GetTroopCount(heroClass, num, onSpawnPerkHandler);
			this.MovementSpeed = (int)(this.HeroClass.HeroMovementSpeedMultiplier * 100f);
			this.HitPoints = heroClass.Health;
			this.Armor = (int)((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetDrivenPropertyBonusOnSpawn(true, DrivenProperty.ArmorTorso, (float)this.HeroClass.ArmorValue) : 0f) + this.HeroClass.ArmorValue;
			if (!this.TrySetSelectedItemByType(this._latestSelectedItemGroup))
			{
				this.SetFirstSelectedItem();
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x000305AC File Offset: 0x0002E7AC
		private bool TrySetSelectedItemByType(ShallowItemVM.ItemGroup itemGroup)
		{
			if (this.Item1.IsValid && this.Item1.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item1);
				return true;
			}
			if (this.Item2.IsValid && this.Item2.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item2);
				return true;
			}
			if (this.Item3.IsValid && this.Item3.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item3);
				return true;
			}
			if (this.Item4.IsValid && this.Item4.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.Item4);
				return true;
			}
			if (this.ItemHorse.IsValid && this.ItemHorse.Type == itemGroup)
			{
				this.UpdateHighlightedItem(this.ItemHorse);
				return true;
			}
			return false;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00030688 File Offset: 0x0002E888
		private void SetFirstSelectedItem()
		{
			ShallowItemVM itemSelected = this.ItemSelected;
			if (itemSelected == null || !itemSelected.IsValid)
			{
				if (this.Item1.IsValid)
				{
					this.UpdateHighlightedItem(this.Item1);
					return;
				}
				if (this.Item2.IsValid)
				{
					this.UpdateHighlightedItem(this.Item2);
					return;
				}
				if (this.Item3.IsValid)
				{
					this.UpdateHighlightedItem(this.Item3);
					return;
				}
				if (this.Item4.IsValid)
				{
					this.UpdateHighlightedItem(this.Item4);
					return;
				}
				if (this.ItemHorse.IsValid)
				{
					this.UpdateHighlightedItem(this.ItemHorse);
				}
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00030730 File Offset: 0x0002E930
		public void UpdateHighlightedItem(ShallowItemVM item)
		{
			this.ItemSelected = item;
			this.Item1.IsSelected = false;
			this.Item2.IsSelected = false;
			this.Item3.IsSelected = false;
			this.Item4.IsSelected = false;
			this.ItemHorse.IsSelected = false;
			item.IsSelected = true;
			this._latestSelectedItemGroup = item.Type;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00030793 File Offset: 0x0002E993
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0003079B File Offset: 0x0002E99B
		[DataSourceProperty]
		public HintViewModel ArmySizeHint
		{
			get
			{
				return this._armySizeHint;
			}
			set
			{
				if (value != this._armySizeHint)
				{
					this._armySizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ArmySizeHint");
				}
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x000307B9 File Offset: 0x0002E9B9
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x000307C1 File Offset: 0x0002E9C1
		[DataSourceProperty]
		public HintViewModel MovementSpeedHint
		{
			get
			{
				return this._movementSpeedHint;
			}
			set
			{
				if (value != this._movementSpeedHint)
				{
					this._movementSpeedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MovementSpeedHint");
				}
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x000307DF File Offset: 0x0002E9DF
		// (set) Token: 0x06000FAB RID: 4011 RVA: 0x000307E7 File Offset: 0x0002E9E7
		[DataSourceProperty]
		public HintViewModel HitPointsHint
		{
			get
			{
				return this._hitPointsHint;
			}
			set
			{
				if (value != this._hitPointsHint)
				{
					this._hitPointsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HitPointsHint");
				}
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00030805 File Offset: 0x0002EA05
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x0003080D File Offset: 0x0002EA0D
		[DataSourceProperty]
		public HintViewModel ArmorHint
		{
			get
			{
				return this._armorHint;
			}
			set
			{
				if (value != this._armorHint)
				{
					this._armorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ArmorHint");
				}
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0003082B File Offset: 0x0002EA2B
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x00030833 File Offset: 0x0002EA33
		[DataSourceProperty]
		public ShallowItemVM Item1
		{
			get
			{
				return this._item1;
			}
			set
			{
				if (value != this._item1)
				{
					this._item1 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item1");
				}
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x00030851 File Offset: 0x0002EA51
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x00030859 File Offset: 0x0002EA59
		[DataSourceProperty]
		public ShallowItemVM Item2
		{
			get
			{
				return this._item2;
			}
			set
			{
				if (value != this._item2)
				{
					this._item2 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item2");
				}
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00030877 File Offset: 0x0002EA77
		// (set) Token: 0x06000FB3 RID: 4019 RVA: 0x0003087F File Offset: 0x0002EA7F
		[DataSourceProperty]
		public ShallowItemVM Item3
		{
			get
			{
				return this._item3;
			}
			set
			{
				if (value != this._item3)
				{
					this._item3 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item3");
				}
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0003089D File Offset: 0x0002EA9D
		// (set) Token: 0x06000FB5 RID: 4021 RVA: 0x000308A5 File Offset: 0x0002EAA5
		[DataSourceProperty]
		public ShallowItemVM Item4
		{
			get
			{
				return this._item4;
			}
			set
			{
				if (value != this._item4)
				{
					this._item4 = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "Item4");
				}
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x000308C3 File Offset: 0x0002EAC3
		// (set) Token: 0x06000FB7 RID: 4023 RVA: 0x000308CB File Offset: 0x0002EACB
		[DataSourceProperty]
		public ShallowItemVM ItemHorse
		{
			get
			{
				return this._itemHorse;
			}
			set
			{
				if (value != this._itemHorse)
				{
					this._itemHorse = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "ItemHorse");
				}
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x000308E9 File Offset: 0x0002EAE9
		// (set) Token: 0x06000FB9 RID: 4025 RVA: 0x000308F1 File Offset: 0x0002EAF1
		[DataSourceProperty]
		public ShallowItemVM ItemSelected
		{
			get
			{
				return this._itemSelected;
			}
			set
			{
				if (value != this._itemSelected)
				{
					this._itemSelected = value;
					base.OnPropertyChangedWithValue<ShallowItemVM>(value, "ItemSelected");
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0003090F File Offset: 0x0002EB0F
		// (set) Token: 0x06000FBB RID: 4027 RVA: 0x00030917 File Offset: 0x0002EB17
		[DataSourceProperty]
		public string Information
		{
			get
			{
				return this._information;
			}
			set
			{
				if (value != this._information)
				{
					this._information = value;
					base.OnPropertyChangedWithValue<string>(value, "Information");
				}
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0003093A File Offset: 0x0002EB3A
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x00030942 File Offset: 0x0002EB42
		[DataSourceProperty]
		public string EquipmentText
		{
			get
			{
				return this._equipmentText;
			}
			set
			{
				if (value != this._equipmentText)
				{
					this._equipmentText = value;
					base.OnPropertyChangedWithValue<string>(value, "EquipmentText");
				}
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00030965 File Offset: 0x0002EB65
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0003096D File Offset: 0x0002EB6D
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00030990 File Offset: 0x0002EB90
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x00030998 File Offset: 0x0002EB98
		[DataSourceProperty]
		public int MovementSpeed
		{
			get
			{
				return this._movementSpeed;
			}
			set
			{
				if (value != this._movementSpeed)
				{
					this._movementSpeed = value;
					base.OnPropertyChangedWithValue(value, "MovementSpeed");
				}
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x000309B6 File Offset: 0x0002EBB6
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x000309BE File Offset: 0x0002EBBE
		[DataSourceProperty]
		public int ArmySize
		{
			get
			{
				return this._armySize;
			}
			set
			{
				if (value != this._armySize)
				{
					this._armySize = value;
					base.OnPropertyChangedWithValue(value, "ArmySize");
				}
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000309DC File Offset: 0x0002EBDC
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x000309E4 File Offset: 0x0002EBE4
		[DataSourceProperty]
		public int HitPoints
		{
			get
			{
				return this._hitPoints;
			}
			set
			{
				if (value != this._hitPoints)
				{
					this._hitPoints = value;
					base.OnPropertyChangedWithValue(value, "HitPoints");
				}
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x00030A02 File Offset: 0x0002EC02
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x00030A0A File Offset: 0x0002EC0A
		[DataSourceProperty]
		public int Armor
		{
			get
			{
				return this._armor;
			}
			set
			{
				if (value != this._armor)
				{
					this._armor = value;
					base.OnPropertyChangedWithValue(value, "Armor");
				}
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x00030A28 File Offset: 0x0002EC28
		// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x00030A30 File Offset: 0x0002EC30
		[DataSourceProperty]
		public bool IsArmyAvailable
		{
			get
			{
				return this._armyAvailable;
			}
			set
			{
				if (value != this._armyAvailable)
				{
					this._armyAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsArmyAvailable");
				}
			}
		}

		// Token: 0x04000742 RID: 1858
		private const int _defaultNumberOfBotsPerFormation = 25;

		// Token: 0x04000743 RID: 1859
		private TextObject _armySizeHintWithDefaultValue = new TextObject("{=aalbxe7z}Army Size", null);

		// Token: 0x04000745 RID: 1861
		private ShallowItemVM.ItemGroup _latestSelectedItemGroup;

		// Token: 0x04000746 RID: 1862
		private HintViewModel _armySizeHint;

		// Token: 0x04000747 RID: 1863
		private HintViewModel _movementSpeedHint;

		// Token: 0x04000748 RID: 1864
		private HintViewModel _hitPointsHint;

		// Token: 0x04000749 RID: 1865
		private HintViewModel _armorHint;

		// Token: 0x0400074A RID: 1866
		private ShallowItemVM _item1;

		// Token: 0x0400074B RID: 1867
		private ShallowItemVM _item2;

		// Token: 0x0400074C RID: 1868
		private ShallowItemVM _item3;

		// Token: 0x0400074D RID: 1869
		private ShallowItemVM _item4;

		// Token: 0x0400074E RID: 1870
		private ShallowItemVM _itemHorse;

		// Token: 0x0400074F RID: 1871
		private ShallowItemVM _itemSelected;

		// Token: 0x04000750 RID: 1872
		private string _information;

		// Token: 0x04000751 RID: 1873
		private string _nameText;

		// Token: 0x04000752 RID: 1874
		private string _equipmentText;

		// Token: 0x04000753 RID: 1875
		private int _movementSpeed;

		// Token: 0x04000754 RID: 1876
		private int _hitPoints;

		// Token: 0x04000755 RID: 1877
		private int _armySize;

		// Token: 0x04000756 RID: 1878
		private int _armor;

		// Token: 0x04000757 RID: 1879
		private bool _armyAvailable;
	}
}
