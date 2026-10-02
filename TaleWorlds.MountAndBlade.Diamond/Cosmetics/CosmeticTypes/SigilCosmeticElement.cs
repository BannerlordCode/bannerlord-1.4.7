using System;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes
{
	// Token: 0x0200017C RID: 380
	public class SigilCosmeticElement : CosmeticElement
	{
		// Token: 0x06000AA2 RID: 2722 RVA: 0x00011BD9 File Offset: 0x0000FDD9
		public SigilCosmeticElement(string id, CosmeticsManager.CosmeticRarity rarity, int cost, string bannerCode)
			: base(id, rarity, cost, CosmeticsManager.CosmeticType.Sigil)
		{
			this.BannerCode = bannerCode;
		}

		// Token: 0x0400052A RID: 1322
		public string BannerCode;
	}
}
