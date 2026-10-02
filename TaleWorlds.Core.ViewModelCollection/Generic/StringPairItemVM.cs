using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x0200002A RID: 42
	public class StringPairItemVM : ViewModel
	{
		// Token: 0x060001CA RID: 458 RVA: 0x00005D8C File Offset: 0x00003F8C
		public StringPairItemVM(string definition, string value, BasicTooltipViewModel hint = null)
		{
			this.Definition = definition;
			this.Value = value;
			this.Hint = hint;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00005DA9 File Offset: 0x00003FA9
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00005DB1 File Offset: 0x00003FB1
		[DataSourceProperty]
		public string Definition
		{
			get
			{
				return this._definition;
			}
			set
			{
				if (value != this._definition)
				{
					this._definition = value;
					base.OnPropertyChangedWithValue<string>(value, "Definition");
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00005DD4 File Offset: 0x00003FD4
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00005DDC File Offset: 0x00003FDC
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

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00005DFF File Offset: 0x00003FFF
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00005E07 File Offset: 0x00004007
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
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
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040000BC RID: 188
		private string _definition;

		// Token: 0x040000BD RID: 189
		private string _value;

		// Token: 0x040000BE RID: 190
		private BasicTooltipViewModel _hint;
	}
}
