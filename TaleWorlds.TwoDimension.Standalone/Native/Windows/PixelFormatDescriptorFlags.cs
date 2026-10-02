using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000023 RID: 35
	[Flags]
	internal enum PixelFormatDescriptorFlags : uint
	{
		// Token: 0x040000A4 RID: 164
		DoubleBuffer = 1U,
		// Token: 0x040000A5 RID: 165
		Stereo = 2U,
		// Token: 0x040000A6 RID: 166
		DrawToWindow = 4U,
		// Token: 0x040000A7 RID: 167
		DrawToBitmap = 8U,
		// Token: 0x040000A8 RID: 168
		SupportGDI = 16U,
		// Token: 0x040000A9 RID: 169
		SupportOpengl = 32U,
		// Token: 0x040000AA RID: 170
		GenericFormat = 64U,
		// Token: 0x040000AB RID: 171
		NeedPalette = 128U,
		// Token: 0x040000AC RID: 172
		NeedSystemPalette = 256U,
		// Token: 0x040000AD RID: 173
		SwapExchange = 512U,
		// Token: 0x040000AE RID: 174
		SwapCopy = 1024U,
		// Token: 0x040000AF RID: 175
		SwapLayerBuffers = 2048U,
		// Token: 0x040000B0 RID: 176
		GenericAccelerated = 4096U,
		// Token: 0x040000B1 RID: 177
		SupportDirectDraw = 8192U,
		// Token: 0x040000B2 RID: 178
		Direct3DAccelerated = 16384U,
		// Token: 0x040000B3 RID: 179
		SupportComposition = 32768U
	}
}
