using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A9 RID: 169
	public class ShallowItemPropertyVM : ViewModel
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x000320D7 File Offset: 0x000302D7
		public ShallowItemPropertyVM(TextObject propertyName, int permille, int value)
		{
			this._propertyName = propertyName;
			this.Permille = permille;
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000320FA File Offset: 0x000302FA
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = this._propertyName.ToString();
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x00032113 File Offset: 0x00030313
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0003211B File Offset: 0x0003031B
		[DataSourceProperty]
		public int Value
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

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x00032139 File Offset: 0x00030339
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x00032141 File Offset: 0x00030341
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x00032164 File Offset: 0x00030364
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x0003216C File Offset: 0x0003036C
		[DataSourceProperty]
		public int Permille
		{
			get
			{
				return this._permille;
			}
			set
			{
				if (value != this._permille)
				{
					this._permille = value;
					base.OnPropertyChangedWithValue(value, "Permille");
				}
			}
		}

		// Token: 0x04000789 RID: 1929
		private readonly TextObject _propertyName;

		// Token: 0x0400078A RID: 1930
		private string _nameText;

		// Token: 0x0400078B RID: 1931
		private int _permille;

		// Token: 0x0400078C RID: 1932
		private int _value;
	}
}
