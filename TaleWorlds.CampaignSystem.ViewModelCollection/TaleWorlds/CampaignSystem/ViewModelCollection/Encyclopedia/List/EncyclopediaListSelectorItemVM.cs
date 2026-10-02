using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E1 RID: 225
	public class EncyclopediaListSelectorItemVM : SelectorItemVM
	{
		// Token: 0x06001564 RID: 5476 RVA: 0x000547A6 File Offset: 0x000529A6
		public EncyclopediaListSelectorItemVM(EncyclopediaListItemComparer comparer)
			: base(comparer.SortController.Name.ToString())
		{
			this.Comparer = comparer;
		}

		// Token: 0x040009BE RID: 2494
		public EncyclopediaListItemComparer Comparer;
	}
}
