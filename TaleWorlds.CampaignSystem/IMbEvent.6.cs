using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000052 RID: 82
	public interface IMbEvent<out T1, out T2, out T3, out T4, out T5> : IMbEventBase
	{
		// Token: 0x060008A6 RID: 2214
		void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5> action);
	}
}
