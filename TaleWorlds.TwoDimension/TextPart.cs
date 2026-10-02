using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000012 RID: 18
	public class TextPart
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00006060 File Offset: 0x00004260
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00006068 File Offset: 0x00004268
		internal TextMeshGenerator TextMeshGenerator { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00006071 File Offset: 0x00004271
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00006079 File Offset: 0x00004279
		public TextDrawObject DrawObject2D { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00006082 File Offset: 0x00004282
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x0000608A File Offset: 0x0000428A
		public Font DefaultFont { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00006093 File Offset: 0x00004293
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x0000609B File Offset: 0x0000429B
		public float WordWidth { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000060A4 File Offset: 0x000042A4
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000060AC File Offset: 0x000042AC
		public Vector2 PartPosition { get; set; }

		// Token: 0x060000CA RID: 202 RVA: 0x000060B5 File Offset: 0x000042B5
		internal TextPart()
		{
		}
	}
}
