using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000046 RID: 70
	public interface ReferenceIMBEvent<T1> : IMbEventBase
	{
		// Token: 0x0600087C RID: 2172
		void AddNonSerializedListener(object owner, ReferenceAction<T1> action);
	}
}
