using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000030 RID: 48
	public class OrderOfBattleFormationClassVM : ViewModel
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000D630 File Offset: 0x0000B830
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000D638 File Offset: 0x0000B838
		public FormationClass Class
		{
			get
			{
				return this._class;
			}
			set
			{
				if (value != this._class)
				{
					if (!this._isFormationClassPreset)
					{
						Action<OrderOfBattleFormationClassVM, FormationClass> onClassChanged = OrderOfBattleFormationClassVM.OnClassChanged;
						if (onClassChanged != null)
						{
							onClassChanged(this, value);
						}
					}
					this._class = value;
					this.IsUnset = this._class == FormationClass.NumberOfAllFormations;
					this.ShownFormationClass = (int)(this.IsUnset ? FormationClass.Infantry : (this._class + 1));
					this.UpdateTroopCountText();
					this._isFormationClassPreset = false;
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000D6A5 File Offset: 0x0000B8A5
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000D6AD File Offset: 0x0000B8AD
		public int PreviousWeight { get; private set; }

		// Token: 0x06000393 RID: 915 RVA: 0x0000D6B6 File Offset: 0x0000B8B6
		public OrderOfBattleFormationClassVM(OrderOfBattleFormationItemVM formationItem, FormationClass formationClass = FormationClass.NumberOfAllFormations)
		{
			this.BelongedFormationItem = formationItem;
			this._isFormationClassPreset = formationClass != FormationClass.NumberOfAllFormations;
			this.Class = formationClass;
			this.PreviousWeight = 0;
			this.OnWeightAdjusted();
			this.RefreshValues();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000D6ED File Offset: 0x0000B8ED
		public override void RefreshValues()
		{
			this.LockWeightHint = new HintViewModel(new TextObject("{=mPCrz4rs}Lock troop percentage from relative changes.", null), null);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000D706 File Offset: 0x0000B906
		private void OnWeightAdjusted()
		{
			if (!this._isLockedOfWeightAdjustments)
			{
				Action<OrderOfBattleFormationClassVM> onWeightAdjustedCallback = OrderOfBattleFormationClassVM.OnWeightAdjustedCallback;
				if (onWeightAdjustedCallback != null)
				{
					onWeightAdjustedCallback(this);
				}
			}
			this.UpdateTroopCountText();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000D728 File Offset: 0x0000B928
		public void UpdateTroopCountText()
		{
			if (this.Class != FormationClass.NumberOfAllFormations && OrderOfBattleFormationClassVM.GetTotalCountOfTroopType != null)
			{
				this.TroopCountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).SetTextVariable("LEFT", OrderOfBattleUIHelper.GetCountOfRealUnitsInClass(this)).SetTextVariable("RIGHT", OrderOfBattleFormationClassVM.GetTotalCountOfTroopType(this.Class))
					.ToString();
				return;
			}
			this.TroopCountText = string.Empty;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000D792 File Offset: 0x0000B992
		public void SetWeightAdjustmentLock(bool isLocked)
		{
			this._isLockedOfWeightAdjustments = isLocked;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000D79B File Offset: 0x0000B99B
		public void UpdateWeightAdjustable()
		{
			bool flag;
			if (this.Class != FormationClass.NumberOfAllFormations)
			{
				Func<OrderOfBattleFormationClassVM, bool> canAdjustWeight = OrderOfBattleFormationClassVM.CanAdjustWeight;
				flag = canAdjustWeight != null && canAdjustWeight(this);
			}
			else
			{
				flag = false;
			}
			this.IsAdjustable = flag;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000D7C2 File Offset: 0x0000B9C2
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000D7CA File Offset: 0x0000B9CA
		[DataSourceProperty]
		public bool IsAdjustable
		{
			get
			{
				return this._isAdjustable;
			}
			set
			{
				if (value != this._isAdjustable)
				{
					this._isAdjustable = value && Mission.Current.PlayerTeam.IsPlayerGeneral;
					base.OnPropertyChangedWithValue(this._isAdjustable, "IsAdjustable");
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000D801 File Offset: 0x0000BA01
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000D809 File Offset: 0x0000BA09
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
				}
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000D827 File Offset: 0x0000BA27
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000D82F File Offset: 0x0000BA2F
		[DataSourceProperty]
		public bool IsUnset
		{
			get
			{
				return this._isUnset;
			}
			set
			{
				if (value != this._isUnset)
				{
					this._isUnset = value;
					base.OnPropertyChangedWithValue(value, "IsUnset");
				}
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000D84D File Offset: 0x0000BA4D
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000D855 File Offset: 0x0000BA55
		[DataSourceProperty]
		public int Weight
		{
			get
			{
				return this._weight;
			}
			set
			{
				if (value != this._weight)
				{
					this.PreviousWeight = this._weight;
					this._weight = value;
					base.OnPropertyChangedWithValue(value, "Weight");
					this.OnWeightAdjusted();
				}
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000D885 File Offset: 0x0000BA85
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000D88D File Offset: 0x0000BA8D
		[DataSourceProperty]
		public int ShownFormationClass
		{
			get
			{
				return this._shownFormationClass;
			}
			set
			{
				if (value != this._shownFormationClass)
				{
					this._shownFormationClass = value;
					base.OnPropertyChangedWithValue(value, "ShownFormationClass");
				}
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000D8AB File Offset: 0x0000BAAB
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000D8B3 File Offset: 0x0000BAB3
		[DataSourceProperty]
		public string TroopCountText
		{
			get
			{
				return this._troopCountText;
			}
			set
			{
				if (value != this._troopCountText)
				{
					this._troopCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "TroopCountText");
				}
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000D8D6 File Offset: 0x0000BAD6
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x0000D8DE File Offset: 0x0000BADE
		[DataSourceProperty]
		public HintViewModel LockWeightHint
		{
			get
			{
				return this._lockWeightHint;
			}
			set
			{
				if (value != this._lockWeightHint)
				{
					this._lockWeightHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LockWeightHint");
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000D8FC File Offset: 0x0000BAFC
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x0000D904 File Offset: 0x0000BB04
		[DataSourceProperty]
		public bool IsWeightHighlightActive
		{
			get
			{
				return this._isWeightHighlightActive;
			}
			set
			{
				if (value != this._isWeightHighlightActive)
				{
					this._isWeightHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsWeightHighlightActive");
				}
			}
		}

		// Token: 0x0400018E RID: 398
		private FormationClass _class;

		// Token: 0x0400018F RID: 399
		private bool _isLockedOfWeightAdjustments;

		// Token: 0x04000191 RID: 401
		public readonly OrderOfBattleFormationItemVM BelongedFormationItem;

		// Token: 0x04000192 RID: 402
		public static Action<OrderOfBattleFormationClassVM> OnWeightAdjustedCallback;

		// Token: 0x04000193 RID: 403
		public static Action<OrderOfBattleFormationClassVM, FormationClass> OnClassChanged;

		// Token: 0x04000194 RID: 404
		public static Func<OrderOfBattleFormationClassVM, bool> CanAdjustWeight;

		// Token: 0x04000195 RID: 405
		public static Func<FormationClass, int> GetTotalCountOfTroopType;

		// Token: 0x04000196 RID: 406
		private bool _isFormationClassPreset;

		// Token: 0x04000197 RID: 407
		private bool _isAdjustable;

		// Token: 0x04000198 RID: 408
		private bool _isLocked;

		// Token: 0x04000199 RID: 409
		private bool _isUnset;

		// Token: 0x0400019A RID: 410
		private int _weight;

		// Token: 0x0400019B RID: 411
		private int _shownFormationClass;

		// Token: 0x0400019C RID: 412
		private string _troopCountText;

		// Token: 0x0400019D RID: 413
		private HintViewModel _lockWeightHint;

		// Token: 0x0400019E RID: 414
		private bool _isWeightHighlightActive;
	}
}
