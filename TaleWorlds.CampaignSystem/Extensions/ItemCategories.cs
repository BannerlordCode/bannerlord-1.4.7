using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200016D RID: 365
	public static class ItemCategories
	{
		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x0008D4DA File Offset: 0x0008B6DA
		public static MBReadOnlyList<ItemCategory> All
		{
			get
			{
				return Campaign.Current.AllItemCategories;
			}
		}
	}
}
