using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200015D RID: 349
	[Serializable]
	public class SupportedFeatures
	{
		// Token: 0x060009B2 RID: 2482 RVA: 0x0000EF9C File Offset: 0x0000D19C
		public SupportedFeatures()
		{
			this.Features = -1;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0000EFAB File Offset: 0x0000D1AB
		public SupportedFeatures(int features)
		{
			this.Features = features;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000EFBC File Offset: 0x0000D1BC
		public bool SupportsFeatures(Features feature)
		{
			return (this.Features & (int)feature) == (int)feature;
		}

		// Token: 0x040004B0 RID: 1200
		public int Features;
	}
}
