using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000025 RID: 37
	public class BindingListStringItem : ViewModel
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00005B66 File Offset: 0x00003D66
		public BindingListStringItem(string value)
		{
			this.Item = value;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00005B75 File Offset: 0x00003D75
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00005B7D File Offset: 0x00003D7D
		[DataSourceProperty]
		public string Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue<string>(value, "Item");
				}
			}
		}

		// Token: 0x040000AE RID: 174
		private string _item;
	}
}
