using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000050 RID: 80
	public interface IMbEvent<out T1, out T2, out T3, out T4> : IMbEventBase
	{
		// Token: 0x0600089F RID: 2207
		void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4> action);
	}
}
