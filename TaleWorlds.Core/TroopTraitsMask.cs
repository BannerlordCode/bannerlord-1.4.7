using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D7 RID: 215
	[Flags]
	public enum TroopTraitsMask : ushort
	{
		// Token: 0x04000657 RID: 1623
		None = 0,
		// Token: 0x04000658 RID: 1624
		Melee = 1,
		// Token: 0x04000659 RID: 1625
		Ranged = 2,
		// Token: 0x0400065A RID: 1626
		Mount = 4,
		// Token: 0x0400065B RID: 1627
		Armor = 8,
		// Token: 0x0400065C RID: 1628
		Thrown = 16,
		// Token: 0x0400065D RID: 1629
		Spear = 32,
		// Token: 0x0400065E RID: 1630
		Shield = 64,
		// Token: 0x0400065F RID: 1631
		LowTier = 128,
		// Token: 0x04000660 RID: 1632
		HighTier = 256,
		// Token: 0x04000661 RID: 1633
		All = 511
	}
}
