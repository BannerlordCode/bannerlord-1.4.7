using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.OpenGL
{
	// Token: 0x02000039 RID: 57
	internal enum BlendingDestinationFactor : uint
	{
		// Token: 0x04000241 RID: 577
		Zero,
		// Token: 0x04000242 RID: 578
		One,
		// Token: 0x04000243 RID: 579
		SourceColor = 768U,
		// Token: 0x04000244 RID: 580
		OneMinusSourceColor,
		// Token: 0x04000245 RID: 581
		SourceAlpha,
		// Token: 0x04000246 RID: 582
		OneMinusSourceAlpha,
		// Token: 0x04000247 RID: 583
		DestinationAlpha,
		// Token: 0x04000248 RID: 584
		OneMinusDestinationAlpha,
		// Token: 0x04000249 RID: 585
		DestinationColor,
		// Token: 0x0400024A RID: 586
		OneMinusDestinationColor,
		// Token: 0x0400024B RID: 587
		SourceAlphaSaturate
	}
}
