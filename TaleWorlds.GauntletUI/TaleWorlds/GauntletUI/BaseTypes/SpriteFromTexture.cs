using System;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000060 RID: 96
	internal class SpriteFromTexture : Sprite
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001BC87 File Offset: 0x00019E87
		public override Texture Texture
		{
			get
			{
				return this._texture;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001BC8F File Offset: 0x00019E8F
		public override Vec2 GetMinUvs()
		{
			return Vec2.Zero;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001BC96 File Offset: 0x00019E96
		public override Vec2 GetMaxUvs()
		{
			return Vec2.One;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001BC9D File Offset: 0x00019E9D
		public SpriteFromTexture(Texture texture, int width, int height)
			: base("Sprite", width, height, SpriteNinePatchParameters.Empty)
		{
			this._texture = texture;
		}

		// Token: 0x04000306 RID: 774
		private Texture _texture;
	}
}
