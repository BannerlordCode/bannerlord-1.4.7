using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010F RID: 271
	[Serializable]
	public class ClanStats
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x000073B6 File Offset: 0x000055B6
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x000073BE File Offset: 0x000055BE
		public int WinCount { get; private set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x000073C7 File Offset: 0x000055C7
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x000073CF File Offset: 0x000055CF
		public int LossCount { get; private set; }

		// Token: 0x060005D4 RID: 1492 RVA: 0x000073D8 File Offset: 0x000055D8
		public ClanStats(int winCount, int lossCount)
		{
			this.WinCount = winCount;
			this.LossCount = lossCount;
		}
	}
}
