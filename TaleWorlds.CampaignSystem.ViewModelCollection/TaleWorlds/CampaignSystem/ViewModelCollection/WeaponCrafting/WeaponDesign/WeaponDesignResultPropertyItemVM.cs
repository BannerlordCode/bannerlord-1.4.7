using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010C RID: 268
	public class WeaponDesignResultPropertyItemVM : ViewModel
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x0005B708 File Offset: 0x00059908
		public WeaponDesignResultPropertyItemVM(TextObject description, float value, float changeAmount, bool showFloatingPoint)
		{
			this._description = description;
			this.InitialValue = value;
			this.ChangeAmount = changeAmount;
			this.ShowFloatingPoint = showFloatingPoint;
			this.IsOrderResult = false;
			this.OrderRequirementTooltip = new HintViewModel();
			this.CraftedValueTooltip = new HintViewModel();
			this.BonusPenaltyTooltip = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0005B768 File Offset: 0x00059968
		public WeaponDesignResultPropertyItemVM(TextObject description, float craftedValue, float requiredValue, float changeAmount, bool showFloatingPoint, bool isExceedingBeneficial, bool showTooltip = true)
		{
			this._showTooltip = showTooltip;
			this._description = description;
			this.TargetValue = requiredValue;
			this.InitialValue = craftedValue;
			this.ChangeAmount = changeAmount;
			this._isExceedingBeneficial = isExceedingBeneficial;
			this.IsOrderResult = true;
			this.ShowFloatingPoint = showFloatingPoint;
			this.OrderRequirementTooltip = new HintViewModel();
			this.CraftedValueTooltip = new HintViewModel();
			this.BonusPenaltyTooltip = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0005B7E0 File Offset: 0x000599E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject description = this._description;
			this.PropertyLbl = ((description != null) ? description.ToString() : null);
			TextObject textObject = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject.SetTextVariable("STR", CampaignUIHelper.GetFormattedItemPropertyText(this.TargetValue, this.ShowFloatingPoint));
			this.RequiredValueText = ((this.TargetValue == 0f) ? string.Empty : textObject.ToString());
			this.HasBenefit = (this._isExceedingBeneficial ? (this.InitialValue + this.ChangeAmount >= this.TargetValue) : (this.InitialValue + this.ChangeAmount <= this.TargetValue));
			this.OrderRequirementTooltip.HintText = (this._showTooltip ? GameTexts.FindText("str_crafting_order_requirement_tooltip", null) : TextObject.GetEmpty());
			this.CraftedValueTooltip.HintText = (this._showTooltip ? GameTexts.FindText("str_crafting_crafted_value_tooltip", null) : TextObject.GetEmpty());
			this.BonusPenaltyTooltip.HintText = (this._showTooltip ? GameTexts.FindText("str_crafting_bonus_penalty_tooltip", null) : TextObject.GetEmpty());
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0005B903 File Offset: 0x00059B03
		// (set) Token: 0x060017F6 RID: 6134 RVA: 0x0005B90B File Offset: 0x00059B0B
		[DataSourceProperty]
		public string PropertyLbl
		{
			get
			{
				return this._propertyLbl;
			}
			set
			{
				if (value != this._propertyLbl)
				{
					this._propertyLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PropertyLbl");
				}
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x0005B92E File Offset: 0x00059B2E
		// (set) Token: 0x060017F8 RID: 6136 RVA: 0x0005B936 File Offset: 0x00059B36
		[DataSourceProperty]
		public float InitialValue
		{
			get
			{
				return this._propertyValue;
			}
			set
			{
				if (value == 0f || value != this._propertyValue)
				{
					this._propertyValue = value;
					base.OnPropertyChangedWithValue(value, "InitialValue");
				}
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x0005B95C File Offset: 0x00059B5C
		// (set) Token: 0x060017FA RID: 6138 RVA: 0x0005B964 File Offset: 0x00059B64
		[DataSourceProperty]
		public float TargetValue
		{
			get
			{
				return this._requiredValue;
			}
			set
			{
				if (value != this._requiredValue)
				{
					this._requiredValue = value;
					base.OnPropertyChangedWithValue(value, "TargetValue");
				}
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060017FB RID: 6139 RVA: 0x0005B982 File Offset: 0x00059B82
		// (set) Token: 0x060017FC RID: 6140 RVA: 0x0005B98A File Offset: 0x00059B8A
		[DataSourceProperty]
		public string RequiredValueText
		{
			get
			{
				return this._requiredValueText;
			}
			set
			{
				if (value != this._requiredValueText)
				{
					this._requiredValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "RequiredValueText");
				}
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x0005B9AD File Offset: 0x00059BAD
		// (set) Token: 0x060017FE RID: 6142 RVA: 0x0005B9B5 File Offset: 0x00059BB5
		[DataSourceProperty]
		public float ChangeAmount
		{
			get
			{
				return this._changeAmount;
			}
			set
			{
				if (this._changeAmount != value)
				{
					this._changeAmount = value;
					base.OnPropertyChangedWithValue(value, "ChangeAmount");
				}
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x0005B9D3 File Offset: 0x00059BD3
		// (set) Token: 0x06001800 RID: 6144 RVA: 0x0005B9DB File Offset: 0x00059BDB
		[DataSourceProperty]
		public bool ShowFloatingPoint
		{
			get
			{
				return this._showFloatingPoint;
			}
			set
			{
				if (this._showFloatingPoint != value)
				{
					this._showFloatingPoint = value;
					base.OnPropertyChangedWithValue(value, "ShowFloatingPoint");
				}
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x0005B9F9 File Offset: 0x00059BF9
		// (set) Token: 0x06001802 RID: 6146 RVA: 0x0005BA01 File Offset: 0x00059C01
		[DataSourceProperty]
		public bool IsOrderResult
		{
			get
			{
				return this._isOrderResult;
			}
			set
			{
				if (value != this._isOrderResult)
				{
					this._isOrderResult = value;
					base.OnPropertyChangedWithValue(value, "IsOrderResult");
				}
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x0005BA1F File Offset: 0x00059C1F
		// (set) Token: 0x06001804 RID: 6148 RVA: 0x0005BA27 File Offset: 0x00059C27
		[DataSourceProperty]
		public bool HasBenefit
		{
			get
			{
				return this._hasBenefit;
			}
			set
			{
				if (value != this._hasBenefit)
				{
					this._hasBenefit = value;
					base.OnPropertyChangedWithValue(value, "HasBenefit");
				}
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x0005BA45 File Offset: 0x00059C45
		// (set) Token: 0x06001806 RID: 6150 RVA: 0x0005BA4D File Offset: 0x00059C4D
		[DataSourceProperty]
		public HintViewModel OrderRequirementTooltip
		{
			get
			{
				return this._orderRequirementTooltip;
			}
			set
			{
				if (value != this._orderRequirementTooltip)
				{
					this._orderRequirementTooltip = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OrderRequirementTooltip");
				}
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06001807 RID: 6151 RVA: 0x0005BA6B File Offset: 0x00059C6B
		// (set) Token: 0x06001808 RID: 6152 RVA: 0x0005BA73 File Offset: 0x00059C73
		[DataSourceProperty]
		public HintViewModel CraftedValueTooltip
		{
			get
			{
				return this._craftedValueTooltip;
			}
			set
			{
				if (value != this._craftedValueTooltip)
				{
					this._craftedValueTooltip = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CraftedValueTooltip");
				}
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x0005BA91 File Offset: 0x00059C91
		// (set) Token: 0x0600180A RID: 6154 RVA: 0x0005BA99 File Offset: 0x00059C99
		[DataSourceProperty]
		public HintViewModel BonusPenaltyTooltip
		{
			get
			{
				return this._bonusPenaltyTooltip;
			}
			set
			{
				if (value != this._bonusPenaltyTooltip)
				{
					this._bonusPenaltyTooltip = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BonusPenaltyTooltip");
				}
			}
		}

		// Token: 0x04000AFA RID: 2810
		private readonly TextObject _description;

		// Token: 0x04000AFB RID: 2811
		private bool _isExceedingBeneficial;

		// Token: 0x04000AFC RID: 2812
		private bool _showTooltip;

		// Token: 0x04000AFD RID: 2813
		private string _propertyLbl;

		// Token: 0x04000AFE RID: 2814
		private float _propertyValue;

		// Token: 0x04000AFF RID: 2815
		private float _requiredValue;

		// Token: 0x04000B00 RID: 2816
		private string _requiredValueText;

		// Token: 0x04000B01 RID: 2817
		private float _changeAmount;

		// Token: 0x04000B02 RID: 2818
		private bool _showFloatingPoint;

		// Token: 0x04000B03 RID: 2819
		private bool _isOrderResult;

		// Token: 0x04000B04 RID: 2820
		private bool _hasBenefit;

		// Token: 0x04000B05 RID: 2821
		private HintViewModel _orderRequirementTooltip;

		// Token: 0x04000B06 RID: 2822
		private HintViewModel _craftedValueTooltip;

		// Token: 0x04000B07 RID: 2823
		private HintViewModel _bonusPenaltyTooltip;
	}
}
