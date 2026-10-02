using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011F RID: 287
	[Flags]
	internal enum InventoryItemType
	{
		// Token: 0x040002A2 RID: 674
		None = 0,
		// Token: 0x040002A3 RID: 675
		Weapon = 1,
		// Token: 0x040002A4 RID: 676
		Shield = 2,
		// Token: 0x040002A5 RID: 677
		HeadArmor = 4,
		// Token: 0x040002A6 RID: 678
		BodyArmor = 8,
		// Token: 0x040002A7 RID: 679
		LegArmor = 16,
		// Token: 0x040002A8 RID: 680
		HandArmor = 32,
		// Token: 0x040002A9 RID: 681
		Horse = 64,
		// Token: 0x040002AA RID: 682
		HorseHarness = 128,
		// Token: 0x040002AB RID: 683
		Goods = 256,
		// Token: 0x040002AC RID: 684
		Book = 512,
		// Token: 0x040002AD RID: 685
		Animal = 1024,
		// Token: 0x040002AE RID: 686
		Cape = 2048,
		// Token: 0x040002AF RID: 687
		HorseCategory = 192,
		// Token: 0x040002B0 RID: 688
		Armors = 2108,
		// Token: 0x040002B1 RID: 689
		Equipable = 2303,
		// Token: 0x040002B2 RID: 690
		All = 4095
	}
}
