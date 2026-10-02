using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes
{
	// Token: 0x0200017D RID: 381
	public class TauntCosmeticElement : CosmeticElement
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00011BED File Offset: 0x0000FDED
		public static int MaxNumberOfTaunts
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00011BF0 File Offset: 0x0000FDF0
		public TextObject Name { get; }

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00011BF8 File Offset: 0x0000FDF8
		public TauntCosmeticElement(int index, string id, CosmeticsManager.CosmeticRarity rarity, int cost, string name)
			: base(id, rarity, cost, CosmeticsManager.CosmeticType.Taunt)
		{
			this.UsageIndex = index;
			this.Name = new TextObject(name, null);
		}
	}
}
