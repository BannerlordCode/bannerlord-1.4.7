using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000043 RID: 67
	public class BannerThumbnailCreationData : BannerThumbnailCreationBaseData
	{
		// Token: 0x06000244 RID: 580 RVA: 0x0000F2CB File Offset: 0x0000D4CB
		public BannerThumbnailCreationData(Banner banner, Action<Texture> setAction, Action cancelAction, BannerDebugInfo debugInfo, bool isTableauOrNineGrid, bool isLarge)
			: base(banner, setAction, cancelAction, debugInfo, isTableauOrNineGrid, isLarge)
		{
		}
	}
}
