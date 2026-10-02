using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000122 RID: 290
	public class ClanFinanceExpenseItemVM : ViewModel
	{
		// Token: 0x06001A7C RID: 6780 RVA: 0x00063DEC File Offset: 0x00061FEC
		public ClanFinanceExpenseItemVM(MobileParty mobileParty)
		{
			this._mobileParty = mobileParty;
			this.CurrentWageTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyWageTooltip(mobileParty));
			this.MinWage = 100;
			this.MaxWage = 2000;
			this.CurrentWage = this._mobileParty.TotalWage;
			this.CurrentWageValueText = this.CurrentWage.ToString();
			this.IsUnlimitedWage = !this._mobileParty.HasLimitedWage();
			this.CurrentWageLimit = ((this._mobileParty.PaymentLimit == Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit) ? 2000 : this._mobileParty.PaymentLimit);
			this.IsEnabled = true;
			this.RefreshValues();
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00063EC4 File Offset: 0x000620C4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CurrentWageText = new TextObject("{=pnFgwLYG}Current Wage", null).ToString();
			this.CurrentWageLimitText = new TextObject("{=sWWxrafa}Current Limit", null).ToString();
			this.TitleText = new TextObject("{=qdoJOH0j}Party Wage", null).ToString();
			this.UnlimitedWageText = new TextObject("{=WySAapWO}Unlimited Wage", null).ToString();
			this.WageLimitHint = new HintViewModel(new TextObject("{=w0slxNAl}If limit is lower than current wage, party will not recruit troops until wage is reduced to the limit. If limit is higher than current wage, party will keep recruiting.", null), null);
			this.UpdateCurrentWageLimitText();
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00063F4C File Offset: 0x0006214C
		private void OnCurrentWageLimitUpdated(int newValue)
		{
			if (!this.IsUnlimitedWage)
			{
				this._mobileParty.SetWagePaymentLimit(newValue);
			}
			this.UpdateCurrentWageLimitText();
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00063F68 File Offset: 0x00062168
		private void OnUnlimitedWageToggled(bool newValue)
		{
			this.CurrentWageLimit = 2000;
			if (newValue)
			{
				this._mobileParty.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit);
			}
			else
			{
				this._mobileParty.SetWagePaymentLimit(2000);
			}
			this.UpdateCurrentWageLimitText();
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00063FBC File Offset: 0x000621BC
		private void UpdateCurrentWageLimitText()
		{
			this.CurrentWageLimitValueText = (this.IsUnlimitedWage ? new TextObject("{=lC5xsoSh}Unlimited", null).ToString() : this.CurrentWageLimit.ToString());
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06001A81 RID: 6785 RVA: 0x00063FF7 File Offset: 0x000621F7
		// (set) Token: 0x06001A82 RID: 6786 RVA: 0x00063FFF File Offset: 0x000621FF
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

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06001A83 RID: 6787 RVA: 0x0006401D File Offset: 0x0006221D
		// (set) Token: 0x06001A84 RID: 6788 RVA: 0x00064025 File Offset: 0x00062225
		[DataSourceProperty]
		public HintViewModel WageLimitHint
		{
			get
			{
				return this._wageLimitHint;
			}
			set
			{
				if (value != this._wageLimitHint)
				{
					this._wageLimitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "WageLimitHint");
				}
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06001A85 RID: 6789 RVA: 0x00064043 File Offset: 0x00062243
		// (set) Token: 0x06001A86 RID: 6790 RVA: 0x0006404B File Offset: 0x0006224B
		[DataSourceProperty]
		public BasicTooltipViewModel CurrentWageTooltip
		{
			get
			{
				return this._currentWageTooltip;
			}
			set
			{
				if (value != this._currentWageTooltip)
				{
					this._currentWageTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CurrentWageTooltip");
				}
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x00064069 File Offset: 0x00062269
		// (set) Token: 0x06001A88 RID: 6792 RVA: 0x00064071 File Offset: 0x00062271
		[DataSourceProperty]
		public string CurrentWageText
		{
			get
			{
				return this._currentWageText;
			}
			set
			{
				if (value != this._currentWageText)
				{
					this._currentWageText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageText");
				}
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x00064094 File Offset: 0x00062294
		// (set) Token: 0x06001A8A RID: 6794 RVA: 0x0006409C File Offset: 0x0006229C
		[DataSourceProperty]
		public string CurrentWageLimitText
		{
			get
			{
				return this._currentWageLimitText;
			}
			set
			{
				if (value != this._currentWageLimitText)
				{
					this._currentWageLimitText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageLimitText");
				}
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000640BF File Offset: 0x000622BF
		// (set) Token: 0x06001A8C RID: 6796 RVA: 0x000640C7 File Offset: 0x000622C7
		[DataSourceProperty]
		public string CurrentWageValueText
		{
			get
			{
				return this._currentWageValueText;
			}
			set
			{
				if (value != this._currentWageValueText)
				{
					this._currentWageValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageValueText");
				}
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000640EA File Offset: 0x000622EA
		// (set) Token: 0x06001A8E RID: 6798 RVA: 0x000640F2 File Offset: 0x000622F2
		[DataSourceProperty]
		public string CurrentWageLimitValueText
		{
			get
			{
				return this._currentWageLimitValueText;
			}
			set
			{
				if (value != this._currentWageLimitValueText)
				{
					this._currentWageLimitValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageLimitValueText");
				}
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x00064115 File Offset: 0x00062315
		// (set) Token: 0x06001A90 RID: 6800 RVA: 0x0006411D File Offset: 0x0006231D
		[DataSourceProperty]
		public string UnlimitedWageText
		{
			get
			{
				return this._unlimitedWageText;
			}
			set
			{
				if (value != this._unlimitedWageText)
				{
					this._unlimitedWageText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnlimitedWageText");
				}
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x00064140 File Offset: 0x00062340
		// (set) Token: 0x06001A92 RID: 6802 RVA: 0x00064148 File Offset: 0x00062348
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x0006416B File Offset: 0x0006236B
		// (set) Token: 0x06001A94 RID: 6804 RVA: 0x00064173 File Offset: 0x00062373
		[DataSourceProperty]
		public int CurrentWage
		{
			get
			{
				return this._currentWage;
			}
			set
			{
				if (value != this._currentWage)
				{
					this._currentWage = value;
					base.OnPropertyChangedWithValue(value, "CurrentWage");
				}
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00064191 File Offset: 0x00062391
		// (set) Token: 0x06001A96 RID: 6806 RVA: 0x00064199 File Offset: 0x00062399
		[DataSourceProperty]
		public int CurrentWageLimit
		{
			get
			{
				return this._currentWageLimit;
			}
			set
			{
				if (value != this._currentWageLimit)
				{
					this._currentWageLimit = value;
					base.OnPropertyChangedWithValue(value, "CurrentWageLimit");
					this.OnCurrentWageLimitUpdated(value);
				}
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x000641BE File Offset: 0x000623BE
		// (set) Token: 0x06001A98 RID: 6808 RVA: 0x000641C6 File Offset: 0x000623C6
		[DataSourceProperty]
		public int MinWage
		{
			get
			{
				return this._minWage;
			}
			set
			{
				if (value != this._minWage)
				{
					this._minWage = value;
					base.OnPropertyChangedWithValue(value, "MinWage");
				}
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x000641E4 File Offset: 0x000623E4
		// (set) Token: 0x06001A9A RID: 6810 RVA: 0x000641EC File Offset: 0x000623EC
		[DataSourceProperty]
		public int MaxWage
		{
			get
			{
				return this._maxWage;
			}
			set
			{
				if (value != this._maxWage)
				{
					this._maxWage = value;
					base.OnPropertyChangedWithValue(value, "MaxWage");
				}
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0006420A File Offset: 0x0006240A
		// (set) Token: 0x06001A9C RID: 6812 RVA: 0x00064212 File Offset: 0x00062412
		[DataSourceProperty]
		public bool IsUnlimitedWage
		{
			get
			{
				return this._isUnlimitedWage;
			}
			set
			{
				if (value != this._isUnlimitedWage)
				{
					this._isUnlimitedWage = value;
					base.OnPropertyChangedWithValue(value, "IsUnlimitedWage");
					this.OnUnlimitedWageToggled(value);
				}
			}
		}

		// Token: 0x04000C49 RID: 3145
		private const int UIWageSliderMaxLimit = 2000;

		// Token: 0x04000C4A RID: 3146
		private const int UIWageSliderMinLimit = 100;

		// Token: 0x04000C4B RID: 3147
		private readonly MobileParty _mobileParty;

		// Token: 0x04000C4C RID: 3148
		private bool _isEnabled;

		// Token: 0x04000C4D RID: 3149
		private int _minWage;

		// Token: 0x04000C4E RID: 3150
		private int _maxWage;

		// Token: 0x04000C4F RID: 3151
		private int _currentWage;

		// Token: 0x04000C50 RID: 3152
		private int _currentWageLimit;

		// Token: 0x04000C51 RID: 3153
		private string _currentWageText;

		// Token: 0x04000C52 RID: 3154
		private string _currentWageLimitText;

		// Token: 0x04000C53 RID: 3155
		private string _currentWageValueText;

		// Token: 0x04000C54 RID: 3156
		private string _currentWageLimitValueText;

		// Token: 0x04000C55 RID: 3157
		private string _unlimitedWageText;

		// Token: 0x04000C56 RID: 3158
		private string _titleText;

		// Token: 0x04000C57 RID: 3159
		private bool _isUnlimitedWage;

		// Token: 0x04000C58 RID: 3160
		private HintViewModel _wageLimitHint;

		// Token: 0x04000C59 RID: 3161
		private BasicTooltipViewModel _currentWageTooltip;
	}
}
