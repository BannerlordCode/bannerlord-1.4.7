using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000027 RID: 39
	public class TextMaterial : Material
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000073B4 File Offset: 0x000055B4
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000073BC File Offset: 0x000055BC
		public Texture Texture { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000073C5 File Offset: 0x000055C5
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000073CD File Offset: 0x000055CD
		public Color Color { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000073D6 File Offset: 0x000055D6
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000073DE File Offset: 0x000055DE
		public float SmoothingConstant { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000073E7 File Offset: 0x000055E7
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000073EF File Offset: 0x000055EF
		public bool Smooth { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000073F8 File Offset: 0x000055F8
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00007400 File Offset: 0x00005600
		public float ScaleFactor { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007409 File Offset: 0x00005609
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00007411 File Offset: 0x00005611
		public Color GlowColor { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000741A File Offset: 0x0000561A
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00007422 File Offset: 0x00005622
		public Color OutlineColor { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000742B File Offset: 0x0000562B
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00007433 File Offset: 0x00005633
		public float OutlineAmount { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000743C File Offset: 0x0000563C
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00007444 File Offset: 0x00005644
		public float GlowRadius { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000744D File Offset: 0x0000564D
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00007455 File Offset: 0x00005655
		public float Blur { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000745E File Offset: 0x0000565E
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00007466 File Offset: 0x00005666
		public float ShadowOffset { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000746F File Offset: 0x0000566F
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00007477 File Offset: 0x00005677
		public float ShadowAngle { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00007480 File Offset: 0x00005680
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00007488 File Offset: 0x00005688
		public float ColorFactor { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00007491 File Offset: 0x00005691
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00007499 File Offset: 0x00005699
		public float AlphaFactor { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000074A2 File Offset: 0x000056A2
		// (set) Token: 0x060001BA RID: 442 RVA: 0x000074AA File Offset: 0x000056AA
		public float HueFactor { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000074B3 File Offset: 0x000056B3
		// (set) Token: 0x060001BC RID: 444 RVA: 0x000074BB File Offset: 0x000056BB
		public float SaturationFactor { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000074C4 File Offset: 0x000056C4
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000074CC File Offset: 0x000056CC
		public float ValueFactor { get; set; }

		// Token: 0x060001BF RID: 447 RVA: 0x000074D5 File Offset: 0x000056D5
		public TextMaterial()
			: this(null, 0)
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000074DF File Offset: 0x000056DF
		public TextMaterial(Texture texture)
			: this(texture, 0)
		{
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000074E9 File Offset: 0x000056E9
		public TextMaterial(Texture texture, int renderOrder)
			: this(texture, renderOrder, true)
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000074F4 File Offset: 0x000056F4
		public TextMaterial(Texture texture, int renderOrder, bool blending)
			: base(blending, renderOrder)
		{
			this.Texture = texture;
			this.ScaleFactor = 1f;
			this.SmoothingConstant = 0.47f;
			this.Smooth = true;
			this.Color = new Color(1f, 1f, 1f, 1f);
			this.GlowColor = new Color(0f, 0f, 0f, 1f);
			this.OutlineColor = new Color(0f, 0f, 0f, 1f);
			this.OutlineAmount = 0f;
			this.GlowRadius = 0f;
			this.Blur = 0f;
			this.ShadowOffset = 0f;
			this.ShadowAngle = 0f;
			this.ColorFactor = 1f;
			this.AlphaFactor = 1f;
			this.HueFactor = 0f;
			this.SaturationFactor = 0f;
			this.ValueFactor = 0f;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000075F8 File Offset: 0x000057F8
		public void CopyFrom(TextMaterial sourceMaterial)
		{
			this.Texture = sourceMaterial.Texture;
			this.Color = sourceMaterial.Color;
			this.ScaleFactor = sourceMaterial.ScaleFactor;
			this.SmoothingConstant = sourceMaterial.SmoothingConstant;
			this.Smooth = sourceMaterial.Smooth;
			this.GlowColor = sourceMaterial.GlowColor;
			this.OutlineColor = sourceMaterial.OutlineColor;
			this.OutlineAmount = sourceMaterial.OutlineAmount;
			this.GlowRadius = sourceMaterial.GlowRadius;
			this.Blur = sourceMaterial.Blur;
			this.ShadowOffset = sourceMaterial.ShadowOffset;
			this.ShadowAngle = sourceMaterial.ShadowAngle;
			this.ColorFactor = sourceMaterial.ColorFactor;
			this.AlphaFactor = sourceMaterial.AlphaFactor;
			this.HueFactor = sourceMaterial.HueFactor;
			this.SaturationFactor = sourceMaterial.SaturationFactor;
			this.ValueFactor = sourceMaterial.ValueFactor;
		}
	}
}
