using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000098 RID: 152
	[EngineStruct("rglTwo_dimension_text_mesh_draw_data", false, null)]
	public struct TwoDimensionTextMeshDrawData
	{
		// Token: 0x040001EA RID: 490
		public MatrixFrame MatrixFrame;

		// Token: 0x040001EB RID: 491
		public Vec3 ClipRectInfo;

		// Token: 0x040001EC RID: 492
		public float ScreenWidth;

		// Token: 0x040001ED RID: 493
		public float ScreenHeight;

		// Token: 0x040001EE RID: 494
		public Vec2 ScreenScale;

		// Token: 0x040001EF RID: 495
		public uint Color;

		// Token: 0x040001F0 RID: 496
		public float ScaleFactor;

		// Token: 0x040001F1 RID: 497
		public float SmoothingConstant;

		// Token: 0x040001F2 RID: 498
		public float ColorFactor;

		// Token: 0x040001F3 RID: 499
		public float AlphaFactor;

		// Token: 0x040001F4 RID: 500
		public float HueFactor;

		// Token: 0x040001F5 RID: 501
		public float SaturationFactor;

		// Token: 0x040001F6 RID: 502
		public float ValueFactor;

		// Token: 0x040001F7 RID: 503
		public uint GlowColor;

		// Token: 0x040001F8 RID: 504
		public Vec3 OutlineColor;

		// Token: 0x040001F9 RID: 505
		public float OutlineAmount;

		// Token: 0x040001FA RID: 506
		public float GlowRadius;

		// Token: 0x040001FB RID: 507
		public float Blur;

		// Token: 0x040001FC RID: 508
		public float ShadowOffset;

		// Token: 0x040001FD RID: 509
		public float ShadowAngle;

		// Token: 0x040001FE RID: 510
		public int Layer;

		// Token: 0x040001FF RID: 511
		public ulong HashCode1;

		// Token: 0x04000200 RID: 512
		public ulong HashCode2;
	}
}
