using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FD RID: 253
	[Serializable]
	public struct BattleServerInformationForClient
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00006DA5 File Offset: 0x00004FA5
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00006DAD File Offset: 0x00004FAD
		public string MatchId { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00006DB6 File Offset: 0x00004FB6
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00006DBE File Offset: 0x00004FBE
		public string ServerAddress { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00006DC7 File Offset: 0x00004FC7
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x00006DCF File Offset: 0x00004FCF
		public ushort ServerPort { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00006DD8 File Offset: 0x00004FD8
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public int PeerIndex { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00006DE9 File Offset: 0x00004FE9
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x00006DF1 File Offset: 0x00004FF1
		public int TeamNo { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00006DFA File Offset: 0x00004FFA
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x00006E02 File Offset: 0x00005002
		public int SessionKey { get; set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x00006E0B File Offset: 0x0000500B
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00006E13 File Offset: 0x00005013
		public string SceneName { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00006E1C File Offset: 0x0000501C
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00006E24 File Offset: 0x00005024
		public string GameType { get; set; }
	}
}
