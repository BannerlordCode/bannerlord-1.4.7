using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000041 RID: 65
	public class BannerTextureCreationData : BannerThumbnailCreationBaseData
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000F159 File Offset: 0x0000D359
		public BannerTextureCreationData(Banner banner, Action<Texture> setAction, Action cancelAction, BannerDebugInfo debugInfo, bool isTableauOrNineGrid, bool isLarge)
			: base(banner, setAction, cancelAction, debugInfo, isTableauOrNineGrid, isLarge)
		{
			base.RenderId = "Mesh_" + base.RenderId;
		}
	}
}
