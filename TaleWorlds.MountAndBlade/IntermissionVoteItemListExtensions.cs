using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000302 RID: 770
	public static class IntermissionVoteItemListExtensions
	{
		// Token: 0x06002BD2 RID: 11218 RVA: 0x000A84F0 File Offset: 0x000A66F0
		public static bool ContainsItem(this List<IntermissionVoteItem> intermissionVoteItems, string id)
		{
			return intermissionVoteItems != null && intermissionVoteItems.FirstOrDefault<IntermissionVoteItem>((IntermissionVoteItem item) => item.Id == id) != null;
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000A8524 File Offset: 0x000A6724
		public static IntermissionVoteItem Add(this List<IntermissionVoteItem> intermissionVoteItems, string id)
		{
			IntermissionVoteItem intermissionVoteItem = null;
			if (intermissionVoteItems != null)
			{
				int count = intermissionVoteItems.Count;
				IntermissionVoteItem intermissionVoteItem2 = new IntermissionVoteItem(id, count);
				intermissionVoteItems.Add(intermissionVoteItem2);
				intermissionVoteItem = intermissionVoteItem2;
			}
			return intermissionVoteItem;
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000A8550 File Offset: 0x000A6750
		public static IntermissionVoteItem GetItem(this List<IntermissionVoteItem> intermissionVoteItems, string id)
		{
			IntermissionVoteItem intermissionVoteItem = null;
			if (intermissionVoteItems != null)
			{
				intermissionVoteItem = intermissionVoteItems.FirstOrDefault<IntermissionVoteItem>((IntermissionVoteItem item) => item.Id == id);
			}
			return intermissionVoteItem;
		}
	}
}
