using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000079 RID: 121
	public struct PathFaceRecord
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x0000F6C3 File Offset: 0x0000D8C3
		public PathFaceRecord(int index, int groupIndex, int islandIndex)
		{
			this.FaceIndex = index;
			this.FaceGroupIndex = groupIndex;
			this.FaceIslandIndex = islandIndex;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000F6DA File Offset: 0x0000D8DA
		public bool IsValid()
		{
			return this.FaceIndex != -1;
		}

		// Token: 0x04000154 RID: 340
		public int FaceIndex;

		// Token: 0x04000155 RID: 341
		public int FaceGroupIndex;

		// Token: 0x04000156 RID: 342
		public int FaceIslandIndex;

		// Token: 0x04000157 RID: 343
		public static readonly PathFaceRecord NullFaceRecord = new PathFaceRecord(-1, -1, -1);
	}
}
