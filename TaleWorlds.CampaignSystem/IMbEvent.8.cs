using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000056 RID: 86
	public interface IMbEvent<out T1, out T2, out T3, out T4, out T5, out T6, out T7> : IMbEventBase
	{
		// Token: 0x060008B4 RID: 2228
		void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5, T6, T7> action);
	}
}
