using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000303 RID: 771
	public struct ShipTemplateStack
	{
		// Token: 0x06002D37 RID: 11575 RVA: 0x000BF3A9 File Offset: 0x000BD5A9
		public ShipTemplateStack(ShipHull shipHull, int minValue, int maxValue)
		{
			this.ShipHull = shipHull;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x04000D43 RID: 3395
		public ShipHull ShipHull;

		// Token: 0x04000D44 RID: 3396
		public int MinValue;

		// Token: 0x04000D45 RID: 3397
		public int MaxValue;
	}
}
