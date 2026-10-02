using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000032 RID: 50
	public abstract class BannerThumbnailCreationBaseData : ThumbnailCreationData
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00009BA8 File Offset: 0x00007DA8
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00009BB0 File Offset: 0x00007DB0
		public Banner Banner { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00009BB9 File Offset: 0x00007DB9
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00009BC1 File Offset: 0x00007DC1
		public BannerDebugInfo DebugInfo { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00009BCA File Offset: 0x00007DCA
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00009BD2 File Offset: 0x00007DD2
		public bool IsTableauOrNineGrid { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00009BDB File Offset: 0x00007DDB
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00009BE3 File Offset: 0x00007DE3
		public bool IsLarge { get; private set; }

		// Token: 0x06000170 RID: 368 RVA: 0x00009BEC File Offset: 0x00007DEC
		public BannerThumbnailCreationBaseData(Banner banner, Action<Texture> setAction, Action cancelAction, BannerDebugInfo debugInfo, bool isTableauOrNineGrid, bool isLarge)
			: base("", setAction, cancelAction)
		{
			this.Banner = banner;
			this.DebugInfo = debugInfo;
			this.IsTableauOrNineGrid = isTableauOrNineGrid;
			this.IsLarge = isLarge;
			base.RenderId = this.CreateRenderId();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00009C28 File Offset: 0x00007E28
		private string CreateRenderId()
		{
			string text = "BannerThumbnail";
			if (this.IsTableauOrNineGrid)
			{
				if (this.IsLarge)
				{
					text = "BannerTableauLarge";
				}
				else
				{
					text = "BannerTableauSmall";
				}
			}
			return text + ":" + this.Banner.BannerCode;
		}
	}
}
