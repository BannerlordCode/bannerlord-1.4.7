using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000070 RID: 112
	[DefaultView]
	public abstract class MissionCheatView : MissionView
	{
		// Token: 0x06000443 RID: 1091
		public abstract bool GetIsCheatsAvailable();

		// Token: 0x06000444 RID: 1092
		public abstract void InitializeScreen();

		// Token: 0x06000445 RID: 1093
		public abstract void FinalizeScreen();
	}
}
