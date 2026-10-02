using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000071 RID: 113
	public class NumericOptionDataVM : GenericOptionDataVM
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x0001DB78 File Offset: 0x0001BD78
		public NumericOptionDataVM(OptionsVM optionsVM, INumericOptionData option, TextObject name, TextObject description)
			: base(optionsVM, option, name, description, OptionsVM.OptionsDataType.NumericOption)
		{
			this._numericOptionData = option;
			this._initialValue = this._numericOptionData.GetValue(false);
			this.Min = this._numericOptionData.GetMinValue();
			this.Max = this._numericOptionData.GetMaxValue();
			this.IsDiscrete = this._numericOptionData.GetIsDiscrete();
			this.DiscreteIncrementInterval = this._numericOptionData.GetDiscreteIncrementInterval();
			this.UpdateContinuously = this._numericOptionData.GetShouldUpdateContinuously();
			this.OptionValue = this._initialValue;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001DC0C File Offset: 0x0001BE0C
		private string GetValueAsString()
		{
			string text = (this.IsDiscrete ? ((int)this._optionValue).ToString() : this._optionValue.ToString("F"));
			if (this._numericOptionData.IsNative() || this._numericOptionData.IsAction())
			{
				return text;
			}
			ManagedOptions.ManagedOptionsType managedOptionsType = (ManagedOptions.ManagedOptionsType)this._numericOptionData.GetOptionType();
			if (managedOptionsType != ManagedOptions.ManagedOptionsType.AutoSaveInterval)
			{
				return text;
			}
			if ((int)this.Min < (int)this._optionValue)
			{
				return text;
			}
			return new TextObject("{=1JlzQIXE}Disabled", null).ToString();
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001DC99 File Offset: 0x0001BE99
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0001DCA1 File Offset: 0x0001BEA1
		[DataSourceProperty]
		public int DiscreteIncrementInterval
		{
			get
			{
				return this._discreteIncrementInterval;
			}
			set
			{
				if (value != this._discreteIncrementInterval)
				{
					this._discreteIncrementInterval = value;
					base.OnPropertyChangedWithValue(value, "DiscreteIncrementInterval");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0001DCBF File Offset: 0x0001BEBF
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0001DCC7 File Offset: 0x0001BEC7
		[DataSourceProperty]
		public float Min
		{
			get
			{
				return this._min;
			}
			set
			{
				if (value != this._min)
				{
					this._min = value;
					base.OnPropertyChangedWithValue(value, "Min");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0001DCE5 File Offset: 0x0001BEE5
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0001DCED File Offset: 0x0001BEED
		[DataSourceProperty]
		public float Max
		{
			get
			{
				return this._max;
			}
			set
			{
				if (value != this._max)
				{
					this._max = value;
					base.OnPropertyChangedWithValue(value, "Max");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001DD0B File Offset: 0x0001BF0B
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x0001DD13 File Offset: 0x0001BF13
		[DataSourceProperty]
		public float OptionValue
		{
			get
			{
				return this._optionValue;
			}
			set
			{
				if (value != this._optionValue)
				{
					this._optionValue = value;
					base.OnPropertyChangedWithValue(value, "OptionValue");
					base.OnPropertyChanged("OptionValueAsString");
					this.UpdateValue();
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001DD42 File Offset: 0x0001BF42
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x0001DD4A File Offset: 0x0001BF4A
		[DataSourceProperty]
		public bool IsDiscrete
		{
			get
			{
				return this._isDiscrete;
			}
			set
			{
				if (value != this._isDiscrete)
				{
					this._isDiscrete = value;
					base.OnPropertyChangedWithValue(value, "IsDiscrete");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001DD68 File Offset: 0x0001BF68
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0001DD70 File Offset: 0x0001BF70
		[DataSourceProperty]
		public bool UpdateContinuously
		{
			get
			{
				return this._updateContinuously;
			}
			set
			{
				if (value != this._updateContinuously)
				{
					this._updateContinuously = value;
					base.OnPropertyChangedWithValue(value, "UpdateContinuously");
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0001DD8E File Offset: 0x0001BF8E
		[DataSourceProperty]
		public string OptionValueAsString
		{
			get
			{
				return this.GetValueAsString();
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001DD96 File Offset: 0x0001BF96
		public override void UpdateValue()
		{
			this.Option.SetValue(this.OptionValue);
			this.Option.Commit();
			this._optionsVM.SetConfig(this.Option, this.OptionValue);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001DDCB File Offset: 0x0001BFCB
		public override void Cancel()
		{
			this.OptionValue = this._initialValue;
			this.UpdateValue();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001DDDF File Offset: 0x0001BFDF
		public override void SetValue(float value)
		{
			this.OptionValue = value;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001DDE8 File Offset: 0x0001BFE8
		public override void ResetData()
		{
			this.OptionValue = this.Option.GetDefaultValue();
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001DDFB File Offset: 0x0001BFFB
		public override bool IsChanged()
		{
			return this._initialValue != this.OptionValue;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001DE0E File Offset: 0x0001C00E
		public override void ApplyValue()
		{
			if (this._initialValue != this.OptionValue)
			{
				this._initialValue = this.OptionValue;
			}
		}

		// Token: 0x040003EA RID: 1002
		private float _initialValue;

		// Token: 0x040003EB RID: 1003
		private INumericOptionData _numericOptionData;

		// Token: 0x040003EC RID: 1004
		private int _discreteIncrementInterval;

		// Token: 0x040003ED RID: 1005
		private float _min;

		// Token: 0x040003EE RID: 1006
		private float _max;

		// Token: 0x040003EF RID: 1007
		private float _optionValue;

		// Token: 0x040003F0 RID: 1008
		private bool _isDiscrete;

		// Token: 0x040003F1 RID: 1009
		private bool _updateContinuously;
	}
}
