using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000044 RID: 68
	public interface IMbEvent<out T> : IMbEventBase
	{
		// Token: 0x06000875 RID: 2165
		void AddNonSerializedListener(object owner, Action<T> action);
	}
}
