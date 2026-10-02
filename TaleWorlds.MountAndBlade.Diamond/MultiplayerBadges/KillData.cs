using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016A RID: 362
	public struct KillData
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x000100A0 File Offset: 0x0000E2A0
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public PlayerId KillerId { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x000100B1 File Offset: 0x0000E2B1
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x000100B9 File Offset: 0x0000E2B9
		public PlayerId VictimId { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x000100C2 File Offset: 0x0000E2C2
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x000100CA File Offset: 0x0000E2CA
		public string KillerFaction { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x000100D3 File Offset: 0x0000E2D3
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x000100DB File Offset: 0x0000E2DB
		public string VictimFaction { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x000100E4 File Offset: 0x0000E2E4
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x000100EC File Offset: 0x0000E2EC
		public string KillerTroop { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x000100F5 File Offset: 0x0000E2F5
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x000100FD File Offset: 0x0000E2FD
		public string VictimTroop { get; set; }
	}
}
