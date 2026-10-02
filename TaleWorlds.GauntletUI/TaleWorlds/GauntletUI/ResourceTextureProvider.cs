using System;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002D RID: 45
	public class ResourceTextureProvider : TextureProvider
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000EDE6 File Offset: 0x0000CFE6
		protected override Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			return twoDimensionContext.LoadTexture(name);
		}
	}
}
