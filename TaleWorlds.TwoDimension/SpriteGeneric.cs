using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000032 RID: 50
	public class SpriteGeneric : Sprite
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00009300 File Offset: 0x00007500
		public override Texture Texture
		{
			get
			{
				return this.SpritePart.Texture;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000930D File Offset: 0x0000750D
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00009315 File Offset: 0x00007515
		public SpritePart SpritePart { get; private set; }

		// Token: 0x0600023F RID: 575 RVA: 0x0000931E File Offset: 0x0000751E
		public override Vec2 GetMinUvs()
		{
			if (this.SpritePart != null)
			{
				return new Vec2(this.SpritePart.MinU, this.SpritePart.MinV);
			}
			return Vec2.Zero;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009349 File Offset: 0x00007549
		public override Vec2 GetMaxUvs()
		{
			if (this.SpritePart != null)
			{
				return new Vec2(this.SpritePart.MaxU, this.SpritePart.MaxV);
			}
			return Vec2.Zero;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009374 File Offset: 0x00007574
		public SpriteGeneric(string name, SpritePart spritePart, in SpriteNinePatchParameters ninePatchParameters)
			: base(name, spritePart.Width, spritePart.Height, ninePatchParameters)
		{
			this.SpritePart = spritePart;
		}
	}
}
