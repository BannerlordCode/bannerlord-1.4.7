using System;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x02000351 RID: 849
	public interface IEncyclopediaLog
	{
		// Token: 0x06003237 RID: 12855
		bool IsVisibleInEncyclopediaPageOf(MBObjectBase obj);

		// Token: 0x06003238 RID: 12856
		TextObject GetEncyclopediaText();

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003239 RID: 12857
		CampaignTime GameTime { get; }
	}
}
