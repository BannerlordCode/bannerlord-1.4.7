using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000029 RID: 41
	public class StringItemWithHintVM : ViewModel
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x00005D1F File Offset: 0x00003F1F
		public StringItemWithHintVM(string text, TextObject hint)
		{
			this.Text = text;
			this.Hint = new HintViewModel(hint, null);
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00005D3B File Offset: 0x00003F3B
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00005D43 File Offset: 0x00003F43
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00005D66 File Offset: 0x00003F66
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00005D6E File Offset: 0x00003F6E
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

		// Token: 0x040000BA RID: 186
		private string _text;

		// Token: 0x040000BB RID: 187
		private HintViewModel _hint;
	}
}
