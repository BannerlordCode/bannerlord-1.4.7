using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006D RID: 109
	public abstract class GenericOptionDataVM : ViewModel
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001D06F File Offset: 0x0001B26F
		public bool IsNative
		{
			get
			{
				return this.Option.IsNative();
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0001D07C File Offset: 0x0001B27C
		public bool IsAction
		{
			get
			{
				return this.Option.IsAction();
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001D08C File Offset: 0x0001B28C
		protected GenericOptionDataVM(OptionsVM optionsVM, IOptionData option, TextObject name, TextObject description, OptionsVM.OptionsDataType typeID)
		{
			this._nameObj = name;
			this._descriptionObj = description;
			this._optionsVM = optionsVM;
			this.Option = option;
			this.OptionTypeID = (int)typeID;
			this.Hint = new HintViewModel();
			this.RefreshValues();
			this.UpdateEnableState();
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001D0E9 File Offset: 0x0001B2E9
		public virtual void UpdateData(bool initUpdate)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001D0EB File Offset: 0x0001B2EB
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
			this.Description = this._descriptionObj.ToString();
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001D115 File Offset: 0x0001B315
		public object GetOptionType()
		{
			return this.Option.GetOptionType();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001D122 File Offset: 0x0001B322
		public IOptionData GetOptionData()
		{
			return this.Option;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001D12A File Offset: 0x0001B32A
		public void ResetToDefault()
		{
			this.SetValue(this.Option.GetDefaultValue());
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001D140 File Offset: 0x0001B340
		public void UpdateEnableState()
		{
			ValueTuple<string, bool> isDisabledAndReasonID = this.Option.GetIsDisabledAndReasonID();
			if (!string.IsNullOrEmpty(isDisabledAndReasonID.Item1))
			{
				this.Hint.HintText = Module.CurrentModule.GlobalTextManager.FindText(isDisabledAndReasonID.Item1, null);
			}
			else
			{
				this.Hint.HintText = TextObject.GetEmpty();
			}
			this.IsEnabled = !isDisabledAndReasonID.Item2;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0001D1B0 File Offset: 0x0001B3B0
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0001D1D3 File Offset: 0x0001B3D3
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0001D1DB File Offset: 0x0001B3DB
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

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0001D1FE File Offset: 0x0001B3FE
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x0001D206 File Offset: 0x0001B406
		[DataSourceProperty]
		public string[] ImageIDs
		{
			get
			{
				return this._imageIDs;
			}
			set
			{
				if (value != this._imageIDs)
				{
					this._imageIDs = value;
					base.OnPropertyChangedWithValue<string[]>(value, "ImageIDs");
				}
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001D224 File Offset: 0x0001B424
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0001D22C File Offset: 0x0001B42C
		[DataSourceProperty]
		public int OptionTypeID
		{
			get
			{
				return this._optionTypeId;
			}
			set
			{
				if (value != this._optionTypeId)
				{
					this._optionTypeId = value;
					base.OnPropertyChangedWithValue(value, "OptionTypeID");
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0001D24A File Offset: 0x0001B44A
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x0001D252 File Offset: 0x0001B452
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

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001D270 File Offset: 0x0001B470
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0001D278 File Offset: 0x0001B478
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

		// Token: 0x06000892 RID: 2194
		public abstract void UpdateValue();

		// Token: 0x06000893 RID: 2195
		public abstract void Cancel();

		// Token: 0x06000894 RID: 2196
		public abstract bool IsChanged();

		// Token: 0x06000895 RID: 2197
		public abstract void SetValue(float value);

		// Token: 0x06000896 RID: 2198
		public abstract void ResetData();

		// Token: 0x06000897 RID: 2199
		public abstract void ApplyValue();

		// Token: 0x040003C6 RID: 966
		private TextObject _nameObj;

		// Token: 0x040003C7 RID: 967
		private TextObject _descriptionObj;

		// Token: 0x040003C8 RID: 968
		protected OptionsVM _optionsVM;

		// Token: 0x040003C9 RID: 969
		protected IOptionData Option;

		// Token: 0x040003CA RID: 970
		private string _description;

		// Token: 0x040003CB RID: 971
		private string _name;

		// Token: 0x040003CC RID: 972
		private int _optionTypeId = -1;

		// Token: 0x040003CD RID: 973
		private string[] _imageIDs;

		// Token: 0x040003CE RID: 974
		private bool _isEnabled = true;

		// Token: 0x040003CF RID: 975
		private HintViewModel _hint;
	}
}
