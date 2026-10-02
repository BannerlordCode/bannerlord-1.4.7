using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000048 RID: 72
	public interface ReferenceIMBEvent<T1, T2> : IMbEventBase
	{
		// Token: 0x06000883 RID: 2179
		void AddNonSerializedListener(object owner, ReferenceAction<T1, T2> action);
	}
}
