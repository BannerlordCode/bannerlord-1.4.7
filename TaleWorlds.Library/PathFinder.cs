using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200007A RID: 122
	public abstract class PathFinder
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
		public PathFinder()
		{
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000F6FF File Offset: 0x0000D8FF
		public virtual void Destroy()
		{
		}

		// Token: 0x0600045D RID: 1117
		public abstract void Initialize(Vec3 bbSize);

		// Token: 0x0600045E RID: 1118
		public abstract bool FindPath(Vec3 wSource, Vec3 wDestination, List<Vec3> path, float craftWidth = 5f);

		// Token: 0x04000158 RID: 344
		public static float BuildingCost = 5000f;

		// Token: 0x04000159 RID: 345
		public static float WaterCost = 400f;

		// Token: 0x0400015A RID: 346
		public static float ShallowWaterCost = 100f;
	}
}
