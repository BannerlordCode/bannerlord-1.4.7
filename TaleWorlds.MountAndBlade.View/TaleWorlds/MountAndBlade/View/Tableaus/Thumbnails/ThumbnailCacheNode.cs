using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004A RID: 74
	public class ThumbnailCacheNode
	{
		// Token: 0x06000270 RID: 624 RVA: 0x000112F5 File Offset: 0x0000F4F5
		public ThumbnailCacheNode()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x000112FD File Offset: 0x0000F4FD
		public ThumbnailCacheNode(string key, Texture value, int frameNo)
		{
			this.Key = key;
			this.Value = value;
			this.FrameNo = frameNo;
			this.ReferenceCount = 0;
		}

		// Token: 0x0400014A RID: 330
		public string Key;

		// Token: 0x0400014B RID: 331
		public Texture Value;

		// Token: 0x0400014C RID: 332
		public int FrameNo;

		// Token: 0x0400014D RID: 333
		public int ReferenceCount;
	}
}
