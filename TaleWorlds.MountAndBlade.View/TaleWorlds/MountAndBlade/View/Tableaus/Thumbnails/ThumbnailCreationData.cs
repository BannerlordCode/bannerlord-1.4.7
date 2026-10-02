using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004D RID: 77
	public abstract class ThumbnailCreationData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000279 RID: 633 RVA: 0x000113F6 File Offset: 0x0000F5F6
		// (set) Token: 0x0600027A RID: 634 RVA: 0x000113FE File Offset: 0x0000F5FE
		public bool IsProcessed { get; internal set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00011407 File Offset: 0x0000F607
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0001140F File Offset: 0x0000F60F
		public string RenderId { get; protected set; }

		// Token: 0x0600027D RID: 637 RVA: 0x00011418 File Offset: 0x0000F618
		public ThumbnailCreationData(string renderId, Action<Texture> setAction, Action cancelAction)
		{
			this.RenderId = renderId;
			this.SetAction = setAction;
			this.CancelAction = cancelAction;
		}

		// Token: 0x04000154 RID: 340
		public readonly Action<Texture> SetAction;

		// Token: 0x04000155 RID: 341
		public readonly Action CancelAction;
	}
}
