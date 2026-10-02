using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D0 RID: 208
	[Flags]
	public enum SkinMask
	{
		// Token: 0x04000629 RID: 1577
		NoneVisible = 0,
		// Token: 0x0400062A RID: 1578
		HeadVisible = 1,
		// Token: 0x0400062B RID: 1579
		BodyVisible = 32,
		// Token: 0x0400062C RID: 1580
		UnderwearVisible = 64,
		// Token: 0x0400062D RID: 1581
		HandsVisible = 128,
		// Token: 0x0400062E RID: 1582
		LegsVisible = 256,
		// Token: 0x0400062F RID: 1583
		AllVisible = 481
	}
}
