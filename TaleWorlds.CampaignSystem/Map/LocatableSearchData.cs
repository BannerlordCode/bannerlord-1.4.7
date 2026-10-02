using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000221 RID: 545
	public struct LocatableSearchData<T>
	{
		// Token: 0x060020DD RID: 8413 RVA: 0x00091ACC File Offset: 0x0008FCCC
		public LocatableSearchData(Vec2 position, float radius, int minX, int minY, int maxX, int maxY)
		{
			this.Position = position;
			this.RadiusSquared = radius * radius;
			this.MinY = minY;
			this.MaxXInclusive = maxX;
			this.MaxYInclusive = maxY;
			this.CurrentX = minX;
			this.CurrentY = minY - 1;
			this.CurrentLocatable = null;
		}

		// Token: 0x040009A4 RID: 2468
		public readonly Vec2 Position;

		// Token: 0x040009A5 RID: 2469
		public readonly float RadiusSquared;

		// Token: 0x040009A6 RID: 2470
		public readonly int MinY;

		// Token: 0x040009A7 RID: 2471
		public readonly int MaxXInclusive;

		// Token: 0x040009A8 RID: 2472
		public readonly int MaxYInclusive;

		// Token: 0x040009A9 RID: 2473
		public int CurrentX;

		// Token: 0x040009AA RID: 2474
		public int CurrentY;

		// Token: 0x040009AB RID: 2475
		internal ILocatable<T> CurrentLocatable;
	}
}
