using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000174 RID: 372
	public class EncyclopediaFilterItem
	{
		// Token: 0x06001B57 RID: 6999 RVA: 0x0008D9B0 File Offset: 0x0008BBB0
		public EncyclopediaFilterItem(TextObject name, Predicate<object> predicate)
		{
			this.Name = name;
			this.Predicate = predicate;
			this.IsActive = false;
		}

		// Token: 0x04000933 RID: 2355
		public readonly TextObject Name;

		// Token: 0x04000934 RID: 2356
		public readonly Predicate<object> Predicate;

		// Token: 0x04000935 RID: 2357
		public bool IsActive;
	}
}
