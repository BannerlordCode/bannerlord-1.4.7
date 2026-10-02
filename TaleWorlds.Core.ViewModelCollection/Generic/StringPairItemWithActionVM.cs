using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x0200002B RID: 43
	public class StringPairItemWithActionVM : ViewModel
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00005E25 File Offset: 0x00004025
		public StringPairItemWithActionVM(Action<object> onExecute, string definition, string value, object identifier)
		{
			this._onExecute = onExecute;
			this.Identifier = identifier;
			this.Definition = definition;
			this.Value = value;
			this.Hint = new HintViewModel();
			this.IsEnabled = true;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00005E5C File Offset: 0x0000405C
		public void ExecuteAction()
		{
			this._onExecute(this.Identifier);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00005E6F File Offset: 0x0000406F
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00005E77 File Offset: 0x00004077
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00005E9A File Offset: 0x0000409A
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00005EA2 File Offset: 0x000040A2
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00005EC5 File Offset: 0x000040C5
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00005ECD File Offset: 0x000040CD
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00005EEB File Offset: 0x000040EB
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00005EF3 File Offset: 0x000040F3
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

		// Token: 0x040000BF RID: 191
		public object Identifier;

		// Token: 0x040000C0 RID: 192
		protected Action<object> _onExecute;

		// Token: 0x040000C1 RID: 193
		private string _definition;

		// Token: 0x040000C2 RID: 194
		private string _value;

		// Token: 0x040000C3 RID: 195
		private HintViewModel _hint;

		// Token: 0x040000C4 RID: 196
		private bool _isEnabled;
	}
}
