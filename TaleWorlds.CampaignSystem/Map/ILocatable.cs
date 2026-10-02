using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000220 RID: 544
	internal interface ILocatable<T>
	{
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060020D8 RID: 8408
		// (set) Token: 0x060020D9 RID: 8409
		[CachedData]
		int LocatorNodeIndex { get; set; }

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060020DA RID: 8410
		// (set) Token: 0x060020DB RID: 8411
		[CachedData]
		T NextLocatable { get; set; }

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060020DC RID: 8412
		[CachedData]
		Vec2 GetPosition2D { get; }
	}
}
