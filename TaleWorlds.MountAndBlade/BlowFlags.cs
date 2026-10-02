using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E7 RID: 487
	[Flags]
	[EngineStruct("Blow_flags", true, "bf", false)]
	public enum BlowFlags
	{
		// Token: 0x040009A2 RID: 2466
		None = 0,
		// Token: 0x040009A3 RID: 2467
		KnockBack = 16,
		// Token: 0x040009A4 RID: 2468
		KnockDown = 32,
		// Token: 0x040009A5 RID: 2469
		NoSound = 64,
		// Token: 0x040009A6 RID: 2470
		CrushThrough = 128,
		// Token: 0x040009A7 RID: 2471
		ShrugOff = 256,
		// Token: 0x040009A8 RID: 2472
		MakesRear = 512,
		// Token: 0x040009A9 RID: 2473
		NonTipThrust = 1024,
		// Token: 0x040009AA RID: 2474
		CanDismount = 2048
	}
}
