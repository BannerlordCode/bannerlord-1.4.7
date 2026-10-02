using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010B RID: 267
	[Serializable]
	public class ClanLeaderboardInfo
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00007271 File Offset: 0x00005471
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x00007278 File Offset: 0x00005478
		public static ClanLeaderboardInfo Empty { get; private set; } = new ClanLeaderboardInfo(new ClanLeaderboardEntry[0]);

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00007292 File Offset: 0x00005492
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x0000729A File Offset: 0x0000549A
		[JsonProperty]
		public ClanLeaderboardEntry[] ClanEntries { get; private set; }

		// Token: 0x060005B9 RID: 1465 RVA: 0x000072A3 File Offset: 0x000054A3
		public ClanLeaderboardInfo(ClanLeaderboardEntry[] entries)
		{
			this.ClanEntries = entries;
		}
	}
}
