using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000039 RID: 57
	public interface IMbEvent
	{
		// Token: 0x060003D7 RID: 983
		void AddNonSerializedListener(object owner, Action action);

		// Token: 0x060003D8 RID: 984
		void ClearListeners(object o);
	}
}
