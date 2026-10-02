using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000125 RID: 293
	public class ClanFinanceIncomeItemBaseVM : ViewModel
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00064237 File Offset: 0x00062437
		// (set) Token: 0x06001A9E RID: 6814 RVA: 0x0006423F File Offset: 0x0006243F
		public IncomeTypes IncomeTypeAsEnum
		{
			get
			{
				return this._incomeTypeAsEnum;
			}
			protected set
			{
				if (value != this._incomeTypeAsEnum)
				{
					this._incomeTypeAsEnum = value;
					this.IncomeType = (int)value;
				}
			}
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00064258 File Offset: 0x00062458
		protected ClanFinanceIncomeItemBaseVM(Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
		{
			this._onSelection = onSelection;
			this._onRefresh = onRefresh;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00064279 File Offset: 0x00062479
		protected virtual void PopulateStatsList()
		{
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x0006427B File Offset: 0x0006247B
		protected virtual void PopulateActionList()
		{
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0006427D File Offset: 0x0006247D
		public void OnIncomeSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0006428C File Offset: 0x0006248C
		protected string DetermineIncomeText(int incomeAmount)
		{
			if (incomeAmount == 0)
			{
				return GameTexts.FindText("str_clan_finance_value_zero", null).ToString();
			}
			GameTexts.SetVariable("IS_POSITIVE", (this.Income > 0) ? 1 : 0);
			GameTexts.SetVariable("NUMBER", MathF.Abs(this.Income));
			return GameTexts.FindText("str_clan_finance_value", null).ToString();
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x000642E9 File Offset: 0x000624E9
		// (set) Token: 0x06001AA5 RID: 6821 RVA: 0x000642F1 File Offset: 0x000624F1
		[DataSourceProperty]
		public MBBindingList<SelectableItemPropertyVM> ItemProperties
		{
			get
			{
				return this._itemProperties;
			}
			set
			{
				if (value != this._itemProperties)
				{
					this._itemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<SelectableItemPropertyVM>>(value, "ItemProperties");
				}
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x0006430F File Offset: 0x0006250F
		// (set) Token: 0x06001AA7 RID: 6823 RVA: 0x00064317 File Offset: 0x00062517
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

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x0006433A File Offset: 0x0006253A
		// (set) Token: 0x06001AA9 RID: 6825 RVA: 0x00064342 File Offset: 0x00062542
		[DataSourceProperty]
		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				if (value != this._location)
				{
					this._location = value;
					base.OnPropertyChangedWithValue<string>(value, "Location");
				}
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x00064365 File Offset: 0x00062565
		// (set) Token: 0x06001AAB RID: 6827 RVA: 0x0006436D File Offset: 0x0006256D
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

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x0006438B File Offset: 0x0006258B
		// (set) Token: 0x06001AAD RID: 6829 RVA: 0x00064393 File Offset: 0x00062593
		[DataSourceProperty]
		public string IncomeValueText
		{
			get
			{
				return this._incomeValueText;
			}
			set
			{
				if (value != this._incomeValueText)
				{
					this._incomeValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeValueText");
				}
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06001AAE RID: 6830 RVA: 0x000643B6 File Offset: 0x000625B6
		// (set) Token: 0x06001AAF RID: 6831 RVA: 0x000643BE File Offset: 0x000625BE
		[DataSourceProperty]
		public string ImageName
		{
			get
			{
				return this._imageName;
			}
			set
			{
				if (value != this._imageName)
				{
					this._imageName = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageName");
				}
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x000643E1 File Offset: 0x000625E1
		// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x000643E9 File Offset: 0x000625E9
		[DataSourceProperty]
		public int Income
		{
			get
			{
				return this._income;
			}
			set
			{
				if (value != this._income)
				{
					this._income = value;
					base.OnPropertyChangedWithValue(value, "Income");
				}
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x00064407 File Offset: 0x00062607
		// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x0006440F File Offset: 0x0006260F
		[DataSourceProperty]
		public ImageIdentifierVM Visual
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
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x0006442D File Offset: 0x0006262D
		// (set) Token: 0x06001AB5 RID: 6837 RVA: 0x00064435 File Offset: 0x00062635
		[DataSourceProperty]
		public int IncomeType
		{
			get
			{
				return this._incomeType;
			}
			set
			{
				if (value != this._incomeType)
				{
					this._incomeType = value;
					base.OnPropertyChangedWithValue(value, "IncomeType");
				}
			}
		}

		// Token: 0x04000C63 RID: 3171
		protected Action _onRefresh;

		// Token: 0x04000C64 RID: 3172
		protected Action<ClanFinanceIncomeItemBaseVM> _onSelection;

		// Token: 0x04000C65 RID: 3173
		protected IncomeTypes _incomeTypeAsEnum;

		// Token: 0x04000C66 RID: 3174
		private int _incomeType;

		// Token: 0x04000C67 RID: 3175
		private string _name;

		// Token: 0x04000C68 RID: 3176
		private string _location;

		// Token: 0x04000C69 RID: 3177
		private string _incomeValueText;

		// Token: 0x04000C6A RID: 3178
		private string _imageName;

		// Token: 0x04000C6B RID: 3179
		private int _income;

		// Token: 0x04000C6C RID: 3180
		private bool _isSelected;

		// Token: 0x04000C6D RID: 3181
		private ImageIdentifierVM _visual;

		// Token: 0x04000C6E RID: 3182
		private MBBindingList<SelectableItemPropertyVM> _itemProperties = new MBBindingList<SelectableItemPropertyVM>();
	}
}
