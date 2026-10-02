using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes
{
	// Token: 0x0200017B RID: 379
	public class ClothingCosmeticElement : CosmeticElement
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00011BBD File Offset: 0x0000FDBD
		public ClothingCosmeticElement(string id, CosmeticsManager.CosmeticRarity rarity, int cost, List<string> replaceItemsId, List<Tuple<string, string>> replaceItemless)
			: base(id, rarity, cost, CosmeticsManager.CosmeticType.Clothing)
		{
			this.ReplaceItemsId = replaceItemsId;
			this.ReplaceItemless = replaceItemless;
		}

		// Token: 0x04000528 RID: 1320
		public readonly List<string> ReplaceItemsId;

		// Token: 0x04000529 RID: 1321
		public readonly List<Tuple<string, string>> ReplaceItemless;
	}
}
