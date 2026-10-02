using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009B RID: 155
	public interface ITrackableCampaignObject : ITrackableBase
	{
		// Token: 0x060012CE RID: 4814
		Banner GetBanner();

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060012CF RID: 4815
		bool IsReady { get; }
	}
}
