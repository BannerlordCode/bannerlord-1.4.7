using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000019 RID: 25
	public struct BlendFunction
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00005CA5 File Offset: 0x00003EA5
		public BlendFunction(AlphaFormatFlags op, byte flags, byte alpha, AlphaFormatFlags format)
		{
			this.BlendOp = (byte)op;
			this.BlendFlags = flags;
			this.SourceConstantAlpha = alpha;
			this.AlphaFormat = (byte)format;
		}

		// Token: 0x04000074 RID: 116
		public byte BlendOp;

		// Token: 0x04000075 RID: 117
		public byte BlendFlags;

		// Token: 0x04000076 RID: 118
		public byte SourceConstantAlpha;

		// Token: 0x04000077 RID: 119
		public byte AlphaFormat;

		// Token: 0x04000078 RID: 120
		public static readonly BlendFunction Default = new BlendFunction(AlphaFormatFlags.Over, 0, byte.MaxValue, AlphaFormatFlags.Alpha);
	}
}
