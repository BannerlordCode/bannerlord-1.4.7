using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000DF RID: 223
	public class EncyclopediaListItemComparer : IComparer<EncyclopediaListItemVM>
	{
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0005470F File Offset: 0x0005290F
		public EncyclopediaSortController SortController { get; }

		// Token: 0x0600155F RID: 5471 RVA: 0x00054717 File Offset: 0x00052917
		public EncyclopediaListItemComparer(EncyclopediaSortController sortController)
		{
			this.SortController = sortController;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00054728 File Offset: 0x00052928
		private int GetBookmarkComparison(EncyclopediaListItemVM x, EncyclopediaListItemVM y)
		{
			return -x.IsBookmarked.CompareTo(y.IsBookmarked);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0005474C File Offset: 0x0005294C
		public int Compare(EncyclopediaListItemVM x, EncyclopediaListItemVM y)
		{
			int bookmarkComparison = this.GetBookmarkComparison(x, y);
			if (bookmarkComparison != 0)
			{
				return bookmarkComparison;
			}
			return this.SortController.Comparer.Compare(x.ListItem, y.ListItem);
		}
	}
}
