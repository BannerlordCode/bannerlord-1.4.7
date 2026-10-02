using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000095 RID: 149
	public interface INavigationHandler
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060012A8 RID: 4776
		// (set) Token: 0x060012A9 RID: 4777
		bool IsNavigationLocked { get; set; }

		// Token: 0x060012AA RID: 4778
		INavigationElement[] GetElements();

		// Token: 0x060012AB RID: 4779
		INavigationElement GetElement(string id);

		// Token: 0x060012AC RID: 4780
		bool IsAnyElementActive();
	}
}
