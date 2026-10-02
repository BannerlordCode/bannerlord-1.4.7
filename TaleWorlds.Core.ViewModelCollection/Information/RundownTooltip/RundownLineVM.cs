using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Information.RundownTooltip
{
	// Token: 0x0200001B RID: 27
	public class RundownLineVM : ViewModel
	{
		// Token: 0x06000182 RID: 386 RVA: 0x000054CB File Offset: 0x000036CB
		public RundownLineVM(string name, float value)
		{
			this.Name = name;
			this.ValueAsString = string.Format("{0:0.##}", value);
			this.Value = value;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000054F7 File Offset: 0x000036F7
		// (set) Token: 0x06000184 RID: 388 RVA: 0x000054FF File Offset: 0x000036FF
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00005522 File Offset: 0x00003722
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000552A File Offset: 0x0000372A
		[DataSourceProperty]
		public string ValueAsString
		{
			get
			{
				return this._valueAsString;
			}
			set
			{
				if (value != this._valueAsString)
				{
					this._valueAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueAsString");
				}
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000554D File Offset: 0x0000374D
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00005555 File Offset: 0x00003755
		[DataSourceProperty]
		public float Value
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
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x0400009A RID: 154
		private string _name;

		// Token: 0x0400009B RID: 155
		private string _valueAsString;

		// Token: 0x0400009C RID: 156
		private float _value;
	}
}
