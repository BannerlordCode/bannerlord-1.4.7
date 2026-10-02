using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003F RID: 63
	public class BannerEditorTextureCreationData : BannerThumbnailCreationBaseData
	{
		// Token: 0x06000236 RID: 566 RVA: 0x0000F010 File Offset: 0x0000D210
		public BannerEditorTextureCreationData(Banner banner, Action<Texture> setAction, Action cancelAction, BannerDebugInfo debugInfo, bool isTableauOrNineGrid, bool isLarge)
			: base(banner, setAction, cancelAction, debugInfo, isTableauOrNineGrid, isLarge)
		{
		}
	}
}
