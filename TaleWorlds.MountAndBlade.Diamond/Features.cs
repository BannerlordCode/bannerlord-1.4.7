using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000116 RID: 278
	[Flags]
	public enum Features
	{
		// Token: 0x0400026B RID: 619
		None = 0,
		// Token: 0x0400026C RID: 620
		Matchmaking = 1,
		// Token: 0x0400026D RID: 621
		CustomGame = 2,
		// Token: 0x0400026E RID: 622
		Party = 4,
		// Token: 0x0400026F RID: 623
		Clan = 8,
		// Token: 0x04000270 RID: 624
		BannerlordFriendList = 16,
		// Token: 0x04000271 RID: 625
		TextChat = 32,
		// Token: 0x04000272 RID: 626
		All = -1
	}
}
