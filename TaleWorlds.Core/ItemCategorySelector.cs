using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000072 RID: 114
	public abstract class ItemCategorySelector : MBGameModel<ItemCategorySelector>
	{
		// Token: 0x060007EC RID: 2028
		public abstract ItemCategory GetItemCategoryForItem(ItemObject itemObject);
	}
}
