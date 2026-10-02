using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000032 RID: 50
	public class OrderOfBattleFormationFilterSelectorItemComparer : IComparer<OrderOfBattleFormationFilterSelectorItemVM>
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x0000DA07 File Offset: 0x0000BC07
		public int Compare(OrderOfBattleFormationFilterSelectorItemVM x, OrderOfBattleFormationFilterSelectorItemVM y)
		{
			return x.FilterType.CompareTo(y.FilterType);
		}
	}
}
