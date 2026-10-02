using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D4 RID: 468
	public interface IMusicHandler
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001BDC RID: 7132
		bool IsPausable { get; }

		// Token: 0x06001BDD RID: 7133
		void OnUpdated(float dt);
	}
}
