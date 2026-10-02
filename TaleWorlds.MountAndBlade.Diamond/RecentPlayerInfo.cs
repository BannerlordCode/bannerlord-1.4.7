using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000155 RID: 341
	public class RecentPlayerInfo
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0000E304 File Offset: 0x0000C504
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x0000E30C File Offset: 0x0000C50C
		public string PlayerId { get; set; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0000E315 File Offset: 0x0000C515
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0000E31D File Offset: 0x0000C51D
		public string PlayerName { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0000E326 File Offset: 0x0000C526
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0000E32E File Offset: 0x0000C52E
		public int ImportanceScore { get; set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0000E337 File Offset: 0x0000C537
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0000E33F File Offset: 0x0000C53F
		public DateTime InteractionTime { get; set; }
	}
}
