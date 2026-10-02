using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000086 RID: 134
	public interface IGameStateListener
	{
		// Token: 0x0600089D RID: 2205
		void OnActivate();

		// Token: 0x0600089E RID: 2206
		void OnDeactivate();

		// Token: 0x0600089F RID: 2207
		void OnInitialize();

		// Token: 0x060008A0 RID: 2208
		void OnFinalize();
	}
}
