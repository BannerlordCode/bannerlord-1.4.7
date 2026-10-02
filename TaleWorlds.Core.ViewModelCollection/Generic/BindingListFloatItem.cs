using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000024 RID: 36
	public class BindingListFloatItem : ViewModel
	{
		// Token: 0x060001AF RID: 431 RVA: 0x00005B31 File Offset: 0x00003D31
		public BindingListFloatItem(float value)
		{
			this.Item = value;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00005B40 File Offset: 0x00003D40
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00005B48 File Offset: 0x00003D48
		[DataSourceProperty]
		public float Item
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
					base.OnPropertyChangedWithValue(value, "Item");
				}
			}
		}

		// Token: 0x040000AD RID: 173
		private float _item;
	}
}
