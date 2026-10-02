using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004A RID: 74
	public interface ReferenceIMBEvent<T1, T2, T3> : IMbEventBase
	{
		// Token: 0x0600088A RID: 2186
		void AddNonSerializedListener(object owner, ReferenceAction<T1, T2, T3> action);
	}
}
