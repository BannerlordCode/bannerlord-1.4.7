using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.OpenGL
{
	// Token: 0x02000038 RID: 56
	internal enum BlendingSourceFactor : uint
	{
		// Token: 0x04000235 RID: 565
		Zero,
		// Token: 0x04000236 RID: 566
		One,
		// Token: 0x04000237 RID: 567
		SourceColor = 768U,
		// Token: 0x04000238 RID: 568
		OneMinusSourceColor,
		// Token: 0x04000239 RID: 569
		SourceAlpha,
		// Token: 0x0400023A RID: 570
		OneMinusSourceAlpha,
		// Token: 0x0400023B RID: 571
		DestinationAlpha,
		// Token: 0x0400023C RID: 572
		OneMinusDestinationAlpha,
		// Token: 0x0400023D RID: 573
		DestinationColor,
		// Token: 0x0400023E RID: 574
		OneMinusDestinationColor,
		// Token: 0x0400023F RID: 575
		SourceAlphaSaturate
	}
}
