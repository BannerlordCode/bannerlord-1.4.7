using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000025 RID: 37
	public class PrimitivePolygonMaterial : Material
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007116 File Offset: 0x00005316
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000711E File Offset: 0x0000531E
		public Color Color { get; private set; }

		// Token: 0x06000166 RID: 358 RVA: 0x00007127 File Offset: 0x00005327
		public PrimitivePolygonMaterial(Color color)
			: this(color, 0)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007131 File Offset: 0x00005331
		public PrimitivePolygonMaterial(Color color, int renderOrder)
			: this(color, renderOrder, true)
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000713C File Offset: 0x0000533C
		public PrimitivePolygonMaterial(Color color, int renderOrder, bool blending)
			: base(blending, renderOrder)
		{
			this.Color = color;
		}
	}
}
