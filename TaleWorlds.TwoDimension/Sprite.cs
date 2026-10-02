using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200002E RID: 46
	public abstract class Sprite
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000204 RID: 516
		public abstract Texture Texture { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000849A File Offset: 0x0000669A
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000084A2 File Offset: 0x000066A2
		public string Name { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000207 RID: 519 RVA: 0x000084AB File Offset: 0x000066AB
		// (set) Token: 0x06000208 RID: 520 RVA: 0x000084B3 File Offset: 0x000066B3
		public int Width { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000209 RID: 521 RVA: 0x000084BC File Offset: 0x000066BC
		// (set) Token: 0x0600020A RID: 522 RVA: 0x000084C4 File Offset: 0x000066C4
		public int Height { get; private set; }

		// Token: 0x0600020B RID: 523
		public abstract Vec2 GetMinUvs();

		// Token: 0x0600020C RID: 524
		public abstract Vec2 GetMaxUvs();

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600020D RID: 525 RVA: 0x000084CD File Offset: 0x000066CD
		// (set) Token: 0x0600020E RID: 526 RVA: 0x000084D5 File Offset: 0x000066D5
		public SpriteNinePatchParameters NinePatchParameters { get; private set; }

		// Token: 0x0600020F RID: 527 RVA: 0x000084DE File Offset: 0x000066DE
		protected Sprite(string name, int width, int height, SpriteNinePatchParameters ninePatchParameters)
		{
			this.Name = name;
			this.Width = width;
			this.Height = height;
			this.NinePatchParameters = ninePatchParameters;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008503 File Offset: 0x00006703
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return base.ToString();
			}
			return this.Name;
		}
	}
}
