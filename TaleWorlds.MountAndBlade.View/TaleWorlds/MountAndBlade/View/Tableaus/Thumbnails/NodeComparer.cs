using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004B RID: 75
	public class NodeComparer : IComparer<ThumbnailCacheNode>
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00011321 File Offset: 0x0000F521
		public int Compare(ThumbnailCacheNode x, ThumbnailCacheNode y)
		{
			return x.FrameNo.CompareTo(y.FrameNo);
		}
	}
}
