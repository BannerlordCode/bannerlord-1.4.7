using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000009 RID: 9
	public class RichTextPart
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000047F0 File Offset: 0x000029F0
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000047F8 File Offset: 0x000029F8
		public string Style { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00004801 File Offset: 0x00002A01
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00004809 File Offset: 0x00002A09
		internal TextMeshGenerator TextMeshGenerator { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004812 File Offset: 0x00002A12
		// (set) Token: 0x06000068 RID: 104 RVA: 0x0000481A File Offset: 0x00002A1A
		public ImageDrawObject ImageDrawObject { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00004823 File Offset: 0x00002A23
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000482B File Offset: 0x00002A2B
		public TextDrawObject TextDrawObject { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004834 File Offset: 0x00002A34
		// (set) Token: 0x0600006C RID: 108 RVA: 0x0000483C File Offset: 0x00002A3C
		public Font DefaultFont { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00004845 File Offset: 0x00002A45
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000484D File Offset: 0x00002A4D
		public float WordWidth { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00004856 File Offset: 0x00002A56
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000485E File Offset: 0x00002A5E
		public Vector2 PartPosition { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004867 File Offset: 0x00002A67
		// (set) Token: 0x06000072 RID: 114 RVA: 0x0000486F File Offset: 0x00002A6F
		public Sprite Sprite { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004878 File Offset: 0x00002A78
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00004880 File Offset: 0x00002A80
		public Vector2 SpritePosition { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00004889 File Offset: 0x00002A89
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00004891 File Offset: 0x00002A91
		public RichTextPartType Type { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000489A File Offset: 0x00002A9A
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000048A2 File Offset: 0x00002AA2
		public float Extend { get; set; }

		// Token: 0x06000079 RID: 121 RVA: 0x000048AB File Offset: 0x00002AAB
		internal RichTextPart()
		{
			this.Style = "Default";
		}
	}
}
