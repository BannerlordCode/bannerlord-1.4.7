using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000015 RID: 21
	public class SavedGamePropertyVM : ViewModel
	{
		// Token: 0x060001AE RID: 430 RVA: 0x000082F4 File Offset: 0x000064F4
		public SavedGamePropertyVM(SavedGamePropertyVM.SavedGameProperty type, TextObject value, TextObject hint)
		{
			this.PropertyType = type.ToString();
			this._valueText = value;
			this.Hint = new HintViewModel(hint, null);
			this.RefreshValues();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008329 File Offset: 0x00006529
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Value = this._valueText.ToString();
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008342 File Offset: 0x00006542
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000834A File Offset: 0x0000654A
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00008368 File Offset: 0x00006568
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00008370 File Offset: 0x00006570
		[DataSourceProperty]
		public string PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (value != this._propertyType)
				{
					this._propertyType = value;
					base.OnPropertyChangedWithValue<string>(value, "PropertyType");
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00008393 File Offset: 0x00006593
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000839B File Offset: 0x0000659B
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x040000B9 RID: 185
		private TextObject _valueText;

		// Token: 0x040000BA RID: 186
		private HintViewModel _hint;

		// Token: 0x040000BB RID: 187
		private string _propertyType;

		// Token: 0x040000BC RID: 188
		private string _value;

		// Token: 0x02000078 RID: 120
		public enum SavedGameProperty
		{
			// Token: 0x0400034E RID: 846
			None = -1,
			// Token: 0x0400034F RID: 847
			Health,
			// Token: 0x04000350 RID: 848
			Gold,
			// Token: 0x04000351 RID: 849
			Influence,
			// Token: 0x04000352 RID: 850
			PartySize,
			// Token: 0x04000353 RID: 851
			Food,
			// Token: 0x04000354 RID: 852
			Fiefs,
			// Token: 0x04000355 RID: 853
			Ships
		}
	}
}
