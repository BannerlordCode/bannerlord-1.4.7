using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004E RID: 78
	public interface IMbEvent<out T1, out T2, out T3> : IMbEventBase
	{
		// Token: 0x06000898 RID: 2200
		void AddNonSerializedListener(object owner, Action<T1, T2, T3> action);
	}
}
