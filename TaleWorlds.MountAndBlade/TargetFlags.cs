using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000374 RID: 884
	[Flags]
	public enum TargetFlags
	{
		// Token: 0x0400154D RID: 5453
		None = 0,
		// Token: 0x0400154E RID: 5454
		IsMoving = 1,
		// Token: 0x0400154F RID: 5455
		IsFlammable = 2,
		// Token: 0x04001550 RID: 5456
		IsStructure = 4,
		// Token: 0x04001551 RID: 5457
		IsSiegeEngine = 8,
		// Token: 0x04001552 RID: 5458
		IsAttacker = 16,
		// Token: 0x04001553 RID: 5459
		IsSmall = 32,
		// Token: 0x04001554 RID: 5460
		NotAThreat = 64,
		// Token: 0x04001555 RID: 5461
		DebugThreat = 128,
		// Token: 0x04001556 RID: 5462
		IsSiegeTower = 256,
		// Token: 0x04001557 RID: 5463
		IsShip = 512
	}
}
