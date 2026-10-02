using System;
using TaleWorlds.Core;

namespace Sandbox.View.GameStates
{
	// Token: 0x02000028 RID: 40
	public class PreloadState : GameState
	{
		// Token: 0x0600017D RID: 381 RVA: 0x00011438 File Offset: 0x0000F638
		public PreloadState()
		{
			this.LoadDelayInFrames = 1;
			this.SaveToLoad = string.Empty;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00011452 File Offset: 0x0000F652
		public PreloadState(string saveName)
		{
			this.LoadDelayInFrames = 2;
			this.SaveToLoad = saveName;
		}

		// Token: 0x04000008 RID: 8
		public readonly string SaveToLoad;

		// Token: 0x04000009 RID: 9
		public readonly int LoadDelayInFrames;
	}
}
