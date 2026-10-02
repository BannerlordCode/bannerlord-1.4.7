using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000024 RID: 36
	public abstract class Material
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000070DE File Offset: 0x000052DE
		// (set) Token: 0x06000160 RID: 352 RVA: 0x000070E6 File Offset: 0x000052E6
		public bool Blending { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000070EF File Offset: 0x000052EF
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000070F7 File Offset: 0x000052F7
		public int RenderOrder { get; private set; }

		// Token: 0x06000163 RID: 355 RVA: 0x00007100 File Offset: 0x00005300
		protected Material(bool blending, int renderOrder)
		{
			this.Blending = blending;
			this.RenderOrder = renderOrder;
		}
	}
}
