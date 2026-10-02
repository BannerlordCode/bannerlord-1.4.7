using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001C RID: 28
	[Flags]
	public enum BlurBehindConstraints : uint
	{
		// Token: 0x0400007E RID: 126
		Enable = 1U,
		// Token: 0x0400007F RID: 127
		BlurRegion = 2U,
		// Token: 0x04000080 RID: 128
		TransitionOnMaximized = 4U
	}
}
