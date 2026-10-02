using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.OpenGL
{
	// Token: 0x0200003C RID: 60
	internal enum AttribueMask : uint
	{
		// Token: 0x04000254 RID: 596
		CurrentBit = 1U,
		// Token: 0x04000255 RID: 597
		PointBit,
		// Token: 0x04000256 RID: 598
		LineBit = 4U,
		// Token: 0x04000257 RID: 599
		PolygonBit = 8U,
		// Token: 0x04000258 RID: 600
		PolygonStippleBit = 16U,
		// Token: 0x04000259 RID: 601
		PixelModeBit = 32U,
		// Token: 0x0400025A RID: 602
		LightingBit = 64U,
		// Token: 0x0400025B RID: 603
		FogBit = 128U,
		// Token: 0x0400025C RID: 604
		DepthBufferBit = 256U,
		// Token: 0x0400025D RID: 605
		AccumBufferBit = 512U,
		// Token: 0x0400025E RID: 606
		StencilBufferBit = 1024U,
		// Token: 0x0400025F RID: 607
		ViewportBit = 2048U,
		// Token: 0x04000260 RID: 608
		TransformBit = 4096U,
		// Token: 0x04000261 RID: 609
		EnableBit = 8192U,
		// Token: 0x04000262 RID: 610
		ColorBufferBit = 16384U,
		// Token: 0x04000263 RID: 611
		HintBit = 32768U,
		// Token: 0x04000264 RID: 612
		EvalBit = 65536U,
		// Token: 0x04000265 RID: 613
		ListBit = 131072U,
		// Token: 0x04000266 RID: 614
		TextureBit = 262144U,
		// Token: 0x04000267 RID: 615
		ScissorBit = 524288U,
		// Token: 0x04000268 RID: 616
		AllAttribBits = 1048575U
	}
}
