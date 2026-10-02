using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000024 RID: 36
	public class OrderTroopItemVM : OrderSubjectVM
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000328 RID: 808 RVA: 0x0000C638 File Offset: 0x0000A838
		// (remove) Token: 0x06000329 RID: 809 RVA: 0x0000C66C File Offset: 0x0000A86C
		public static event Action<OrderTroopItemVM, bool> OnSelectionChange;

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000C69F File Offset: 0x0000A89F
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000C6A7 File Offset: 0x0000A8A7
		public bool ContainsDeadTroop { get; private set; }

		// Token: 0x0600032C RID: 812 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		public OrderTroopItemVM(Formation formation, Action<OrderTroopItemVM> setSelected, Func<Formation, int> getMorale)
		{
			this.IsValid = true;
			this.ActiveFormationClasses = new MBBindingList<OrderTroopItemFormationClassVM>();
			this.ActiveFilters = new MBBindingList<OrderTroopItemFilterVM>();
			this.InitialFormationClass = formation.FormationIndex;
			this.SetFormationClassFromFormation(formation);
			this.Formation = formation;
			this.FormationIndex = formation.Index;
			this.FormationName = (this.FormationIndex + 1).ToString();
			this.SetSelected = setSelected;
			this.CurrentMemberCount = (formation.IsPlayerTroopInFormation ? (formation.CountOfUnits - 1) : formation.CountOfUnits);
			this.Morale = getMorale(formation);
			base.UnderAttackOfType = 0;
			base.BehaviorType = 0;
			this.UpdateSelectionKeyInfo();
			this.UpdateVisuals();
			this.Formation.OnUnitCountChanged += this.FormationOnOnUnitCountChanged;
			this.RefreshValues();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000C786 File Offset: 0x0000A986
		public OrderTroopItemVM()
		{
			this.IsValid = false;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000C798 File Offset: 0x0000A998
		public override void OnFinalize()
		{
			if (this.IsValid)
			{
				this.Formation.OnUnitCountChanged -= this.FormationOnOnUnitCountChanged;
			}
			InputKeyItemVM applySelectionKey = base.ApplySelectionKey;
			if (applySelectionKey != null)
			{
				applySelectionKey.OnFinalize();
			}
			InputKeyItemVM toggleSelectionKey = base.ToggleSelectionKey;
			if (toggleSelectionKey == null)
			{
				return;
			}
			toggleSelectionKey.OnFinalize();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000C7E5 File Offset: 0x0000A9E5
		protected override void OnSelectionStateChanged(bool isSelected)
		{
			Action<OrderTroopItemVM, bool> onSelectionChange = OrderTroopItemVM.OnSelectionChange;
			if (onSelectionChange == null)
			{
				return;
			}
			onSelectionChange(this, isSelected);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000C7F8 File Offset: 0x0000A9F8
		private void FormationOnOnUnitCountChanged(Formation formation)
		{
			this.CurrentMemberCount = (formation.IsPlayerTroopInFormation ? (formation.CountOfUnits - 1) : formation.CountOfUnits);
			this.UpdateVisuals();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000C81E File Offset: 0x0000AA1E
		public void OnFormationAgentRemoved(Agent agent)
		{
			if (!agent.IsActive())
			{
				this.ContainsDeadTroop = true;
			}
			this.UpdateVisuals();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000C838 File Offset: 0x0000AA38
		public virtual void UpdateVisuals()
		{
			Formation formation = this.Formation;
			bool flag;
			if (formation == null)
			{
				flag = null != null;
			}
			else
			{
				Agent captain = formation.Captain;
				flag = ((captain != null) ? captain.Character : null) != null;
			}
			if (flag)
			{
				if (this.CaptainImageIdentifier == null || this.Formation.Captain.Character != this._cachedCaptain)
				{
					CharacterImageIdentifierVM captainImageIdentifier = this.CaptainImageIdentifier;
					if (captainImageIdentifier != null)
					{
						captainImageIdentifier.OnFinalize();
					}
					this.CaptainImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Formation.Captain.Character));
					this.HasCaptain = true;
					this._cachedCaptain = this.Formation.Captain.Character;
					return;
				}
			}
			else
			{
				CharacterImageIdentifierVM captainImageIdentifier2 = this.CaptainImageIdentifier;
				if (captainImageIdentifier2 != null)
				{
					captainImageIdentifier2.OnFinalize();
				}
				this.CaptainImageIdentifier = null;
				this.HasCaptain = false;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C8F3 File Offset: 0x0000AAF3
		public virtual void Update()
		{
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000C8F8 File Offset: 0x0000AAF8
		public void UpdateSelectionKeyInfo()
		{
			if (this.Formation == null)
			{
				return;
			}
			if (Input.IsGamepadActive)
			{
				GameKey gameKey = HotKeyManager.GetCategory("MissionOrderHotkeyCategory").GetGameKey(91);
				InputKeyItemVM toggleSelectionKey = base.ToggleSelectionKey;
				if (toggleSelectionKey != null)
				{
					toggleSelectionKey.OnFinalize();
				}
				base.ToggleSelectionKey = InputKeyItemVM.CreateFromGameKey(gameKey, true);
				gameKey = HotKeyManager.GetCategory("MissionOrderHotkeyCategory").GetGameKey(90);
				InputKeyItemVM applySelectionKey = base.ApplySelectionKey;
				if (applySelectionKey != null)
				{
					applySelectionKey.OnFinalize();
				}
				base.ApplySelectionKey = InputKeyItemVM.CreateFromGameKey(gameKey, true);
				return;
			}
			int num = -1;
			if (this.Formation.Index == 0)
			{
				num = 79;
			}
			else if (this.Formation.Index == 1)
			{
				num = 80;
			}
			else if (this.Formation.Index == 2)
			{
				num = 81;
			}
			else if (this.Formation.Index == 3)
			{
				num = 82;
			}
			else if (this.Formation.Index == 4)
			{
				num = 83;
			}
			else if (this.Formation.Index == 5)
			{
				num = 84;
			}
			else if (this.Formation.Index == 6)
			{
				num = 85;
			}
			else if (this.Formation.Index == 7)
			{
				num = 86;
			}
			if (num == -1)
			{
				return;
			}
			GameKey gameKey2 = HotKeyManager.GetCategory("MissionOrderHotkeyCategory").GetGameKey(num);
			InputKeyItemVM applySelectionKey2 = base.ApplySelectionKey;
			if (applySelectionKey2 != null)
			{
				applySelectionKey2.OnFinalize();
			}
			base.ApplySelectionKey = InputKeyItemVM.CreateFromGameKey(gameKey2, false);
			InputKeyItemVM toggleSelectionKey2 = base.ToggleSelectionKey;
			if (toggleSelectionKey2 != null)
			{
				toggleSelectionKey2.OnFinalize();
			}
			base.ToggleSelectionKey = null;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		public bool SetFormationClassFromFormation(Formation formation)
		{
			bool flag = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Infantry) > 0;
			bool flag2 = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Ranged) > 0;
			bool flag3 = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Cavalry) > 0;
			bool flag4 = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.HorseArcher) > 0;
			if (flag && this._cachedInfantryItem == null)
			{
				this._cachedInfantryItem = new OrderTroopItemFormationClassVM(formation, FormationClass.Infantry);
				this.ActiveFormationClasses.Add(this._cachedInfantryItem);
			}
			else if (!flag)
			{
				this.ActiveFormationClasses.Remove(this._cachedInfantryItem);
				this._cachedInfantryItem = null;
			}
			if (flag2 && this._cachedRangedItem == null)
			{
				this._cachedRangedItem = new OrderTroopItemFormationClassVM(formation, FormationClass.Ranged);
				this.ActiveFormationClasses.Add(this._cachedRangedItem);
			}
			else if (!flag2)
			{
				this.ActiveFormationClasses.Remove(this._cachedRangedItem);
				this._cachedRangedItem = null;
			}
			if (flag3 && this._cachedCavalryItem == null)
			{
				this._cachedCavalryItem = new OrderTroopItemFormationClassVM(formation, FormationClass.Cavalry);
				this.ActiveFormationClasses.Add(this._cachedCavalryItem);
			}
			else if (!flag3)
			{
				this.ActiveFormationClasses.Remove(this._cachedCavalryItem);
				this._cachedCavalryItem = null;
			}
			if (flag4 && this._cachedHorseArcherItem == null)
			{
				this._cachedHorseArcherItem = new OrderTroopItemFormationClassVM(formation, FormationClass.HorseArcher);
				this.ActiveFormationClasses.Add(this._cachedHorseArcherItem);
			}
			else if (!flag4)
			{
				this.ActiveFormationClasses.Remove(this._cachedHorseArcherItem);
				this._cachedHorseArcherItem = null;
			}
			foreach (OrderTroopItemFormationClassVM orderTroopItemFormationClassVM in this.ActiveFormationClasses)
			{
				orderTroopItemFormationClassVM.UpdateTroopCount();
			}
			this.UpdateVisuals();
			return false;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		public void UpdateFilterData(List<FormationFilterType> usedFilters)
		{
			this.ActiveFilters.Clear();
			foreach (FormationFilterType formationFilterType in usedFilters)
			{
				this.ActiveFilters.Add(new OrderTroopItemFilterVM((int)formationFilterType));
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000CC60 File Offset: 0x0000AE60
		public void ExecuteAction()
		{
			this.SetSelected(this);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000CC70 File Offset: 0x0000AE70
		public virtual void RefreshTargetedOrderVisual()
		{
			bool flag = false;
			string text = null;
			string text2 = null;
			for (int i = 0; i < base.ActiveOrders.Count; i++)
			{
				OrderItemVM orderItemVM = base.ActiveOrders[i];
				if (orderItemVM.Order.IsTargeted())
				{
					Formation targetFormation = this.Formation.TargetFormation;
					if (targetFormation != null)
					{
						text2 = MissionFormationMarkerTargetVM.GetFormationType(targetFormation.PhysicalClass);
						flag = true;
					}
					text = orderItemVM.OrderIconId;
				}
			}
			this.HasTarget = flag;
			this.CurrentOrderIconId = text;
			this.CurrentTargetFormationType = text2;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000CCF2 File Offset: 0x0000AEF2
		public virtual TextObject GetVisibleNameOfFormationForMessage()
		{
			return GameTexts.FindText("str_formation_class_string", this.Formation.PhysicalClass.GetName());
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000CD0E File Offset: 0x0000AF0E
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000CD16 File Offset: 0x0000AF16
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000CD34 File Offset: 0x0000AF34
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000CD3C File Offset: 0x0000AF3C
		[DataSourceProperty]
		public int FormationIndex
		{
			get
			{
				return this._formationIndex;
			}
			set
			{
				if (value != this._formationIndex)
				{
					this._formationIndex = value;
					base.OnPropertyChangedWithValue(value, "FormationIndex");
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000CD5A File Offset: 0x0000AF5A
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000CD62 File Offset: 0x0000AF62
		[DataSourceProperty]
		public int CurrentMemberCount
		{
			get
			{
				return this._currentMemberCount;
			}
			set
			{
				if (value != this._currentMemberCount)
				{
					this._currentMemberCount = value;
					base.OnPropertyChangedWithValue(value, "CurrentMemberCount");
					this.HaveTroops = value > 0;
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000CD8A File Offset: 0x0000AF8A
		// (set) Token: 0x06000341 RID: 833 RVA: 0x0000CD92 File Offset: 0x0000AF92
		[DataSourceProperty]
		public int Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				if (value != this._morale)
				{
					this._morale = value;
					base.OnPropertyChangedWithValue(value, "Morale");
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000CDB0 File Offset: 0x0000AFB0
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
		[DataSourceProperty]
		public float AmmoPercentage
		{
			get
			{
				return this._ammoPercentage;
			}
			set
			{
				if (value != this._ammoPercentage)
				{
					this._ammoPercentage = value;
					base.OnPropertyChangedWithValue(value, "AmmoPercentage");
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000CDD6 File Offset: 0x0000AFD6
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000CDDE File Offset: 0x0000AFDE
		[DataSourceProperty]
		public bool IsAmmoAvailable
		{
			get
			{
				return this._isAmmoAvailable;
			}
			set
			{
				if (value != this._isAmmoAvailable)
				{
					this._isAmmoAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAmmoAvailable");
				}
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000CDFC File Offset: 0x0000AFFC
		// (set) Token: 0x06000347 RID: 839 RVA: 0x0000CE04 File Offset: 0x0000B004
		[DataSourceProperty]
		public bool HaveTroops
		{
			get
			{
				return this._haveTroops;
			}
			set
			{
				if (value != this._haveTroops)
				{
					this._haveTroops = value;
					base.OnPropertyChangedWithValue(value, "HaveTroops");
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000CE22 File Offset: 0x0000B022
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0000CE2A File Offset: 0x0000B02A
		[DataSourceProperty]
		public bool HasTarget
		{
			get
			{
				return this._hasTarget;
			}
			set
			{
				if (value != this._hasTarget)
				{
					this._hasTarget = value;
					base.OnPropertyChangedWithValue(value, "HasTarget");
				}
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000CE48 File Offset: 0x0000B048
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000CE50 File Offset: 0x0000B050
		[DataSourceProperty]
		public bool IsTargetRelevant
		{
			get
			{
				return this._isTargetRelevant;
			}
			set
			{
				if (value != this._isTargetRelevant)
				{
					this._isTargetRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsTargetRelevant");
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000CE6E File Offset: 0x0000B06E
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000CE76 File Offset: 0x0000B076
		[DataSourceProperty]
		public bool HasCaptain
		{
			get
			{
				return this._hasCaptain;
			}
			set
			{
				if (value != this._hasCaptain)
				{
					this._hasCaptain = value;
					base.OnPropertyChangedWithValue(value, "HasCaptain");
				}
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000CE94 File Offset: 0x0000B094
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000CE9C File Offset: 0x0000B09C
		[DataSourceProperty]
		public string CurrentOrderIconId
		{
			get
			{
				return this._currentOrderIconId;
			}
			set
			{
				if (value != this._currentOrderIconId)
				{
					this._currentOrderIconId = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentOrderIconId");
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000CEBF File Offset: 0x0000B0BF
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000CEC7 File Offset: 0x0000B0C7
		[DataSourceProperty]
		public string CurrentTargetFormationType
		{
			get
			{
				return this._currentTargetFormationType;
			}
			set
			{
				if (value != this._currentTargetFormationType)
				{
					this._currentTargetFormationType = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentTargetFormationType");
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000CEEA File Offset: 0x0000B0EA
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000CEF2 File Offset: 0x0000B0F2
		[DataSourceProperty]
		public string FormationName
		{
			get
			{
				return this._formationName;
			}
			set
			{
				if (value != this._formationName)
				{
					this._formationName = value;
					base.OnPropertyChangedWithValue<string>(value, "FormationName");
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000CF15 File Offset: 0x0000B115
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000CF1D File Offset: 0x0000B11D
		[DataSourceProperty]
		public CharacterImageIdentifierVM CaptainImageIdentifier
		{
			get
			{
				return this._captainImageIdentifier;
			}
			set
			{
				if (value != this._captainImageIdentifier)
				{
					this._captainImageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "CaptainImageIdentifier");
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000CF3B File Offset: 0x0000B13B
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000CF43 File Offset: 0x0000B143
		[DataSourceProperty]
		public MBBindingList<OrderTroopItemFormationClassVM> ActiveFormationClasses
		{
			get
			{
				return this._activeFormationClasses;
			}
			set
			{
				if (value != this._activeFormationClasses)
				{
					this._activeFormationClasses = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderTroopItemFormationClassVM>>(value, "ActiveFormationClasses");
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000CF61 File Offset: 0x0000B161
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000CF69 File Offset: 0x0000B169
		[DataSourceProperty]
		public MBBindingList<OrderTroopItemFilterVM> ActiveFilters
		{
			get
			{
				return this._activeFilters;
			}
			set
			{
				if (value != this._activeFilters)
				{
					this._activeFilters = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderTroopItemFilterVM>>(value, "ActiveFilters");
				}
			}
		}

		// Token: 0x04000160 RID: 352
		public FormationClass InitialFormationClass;

		// Token: 0x04000161 RID: 353
		public Formation Formation;

		// Token: 0x04000162 RID: 354
		public Type MachineType;

		// Token: 0x04000163 RID: 355
		public Action<OrderTroopItemVM> SetSelected;

		// Token: 0x04000165 RID: 357
		private OrderTroopItemFormationClassVM _cachedInfantryItem;

		// Token: 0x04000166 RID: 358
		private OrderTroopItemFormationClassVM _cachedRangedItem;

		// Token: 0x04000167 RID: 359
		private OrderTroopItemFormationClassVM _cachedCavalryItem;

		// Token: 0x04000168 RID: 360
		private OrderTroopItemFormationClassVM _cachedHorseArcherItem;

		// Token: 0x04000169 RID: 361
		private BasicCharacterObject _cachedCaptain;

		// Token: 0x0400016A RID: 362
		private bool _isValid;

		// Token: 0x0400016B RID: 363
		private int _formationIndex;

		// Token: 0x0400016C RID: 364
		private int _currentMemberCount;

		// Token: 0x0400016D RID: 365
		private int _morale;

		// Token: 0x0400016E RID: 366
		private float _ammoPercentage;

		// Token: 0x0400016F RID: 367
		private bool _isAmmoAvailable;

		// Token: 0x04000170 RID: 368
		private bool _haveTroops;

		// Token: 0x04000171 RID: 369
		private bool _hasTarget;

		// Token: 0x04000172 RID: 370
		private bool _isTargetRelevant;

		// Token: 0x04000173 RID: 371
		private bool _hasCaptain;

		// Token: 0x04000174 RID: 372
		private string _currentOrderIconId;

		// Token: 0x04000175 RID: 373
		private string _currentTargetFormationType;

		// Token: 0x04000176 RID: 374
		private string _formationName;

		// Token: 0x04000177 RID: 375
		private CharacterImageIdentifierVM _captainImageIdentifier;

		// Token: 0x04000178 RID: 376
		private MBBindingList<OrderTroopItemFormationClassVM> _activeFormationClasses;

		// Token: 0x04000179 RID: 377
		private MBBindingList<OrderTroopItemFilterVM> _activeFilters;
	}
}
