using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000022 RID: 34
	public class OrderTroopItemFilterVM : ViewModel
	{
		// Token: 0x0600031F RID: 799 RVA: 0x0000C520 File Offset: 0x0000A720
		public OrderTroopItemFilterVM(int filterTypeValue)
		{
			this.FilterTypeValue = filterTypeValue;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000C52F File Offset: 0x0000A72F
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000C537 File Offset: 0x0000A737
		[DataSourceProperty]
		public int FilterTypeValue
		{
			get
			{
				return this._filterTypeValue;
			}
			set
			{
				if (value != this._filterTypeValue)
				{
					this._filterTypeValue = value;
					base.OnPropertyChangedWithValue(value, "FilterTypeValue");
				}
			}
		}

		// Token: 0x0400015A RID: 346
		private int _filterTypeValue;
	}
}
