using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004C RID: 76
	public interface IMbEvent<out T1, out T2> : IMbEventBase
	{
		// Token: 0x06000891 RID: 2193
		void AddNonSerializedListener(object owner, Action<T1, T2> action);
	}
}
