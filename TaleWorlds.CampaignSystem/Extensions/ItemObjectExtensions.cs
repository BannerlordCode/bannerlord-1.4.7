using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200016F RID: 367
	public static class ItemObjectExtensions
	{
		// Token: 0x06001B3B RID: 6971 RVA: 0x0008D4F2 File Offset: 0x0008B6F2
		public static ItemCategory GetItemCategory(this ItemObject item)
		{
			return item.ItemCategory;
		}
	}
}
