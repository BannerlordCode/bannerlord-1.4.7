using System;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics
{
	// Token: 0x02000179 RID: 377
	public class CosmeticElement
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x000115DF File Offset: 0x0000F7DF
		public bool IsFree
		{
			get
			{
				return this.Cost <= 0;
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000115ED File Offset: 0x0000F7ED
		public CosmeticElement(string id, CosmeticsManager.CosmeticRarity rarity, int cost, CosmeticsManager.CosmeticType type)
		{
			this.UsageIndex = -1;
			this.Id = id;
			this.Rarity = rarity;
			this.Cost = cost;
			this.Type = type;
		}

		// Token: 0x04000521 RID: 1313
		public int UsageIndex;

		// Token: 0x04000522 RID: 1314
		public string Id;

		// Token: 0x04000523 RID: 1315
		public CosmeticsManager.CosmeticRarity Rarity;

		// Token: 0x04000524 RID: 1316
		public int Cost;

		// Token: 0x04000525 RID: 1317
		public CosmeticsManager.CosmeticType Type;
	}
}
