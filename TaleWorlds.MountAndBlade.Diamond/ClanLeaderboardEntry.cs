using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010C RID: 268
	[Serializable]
	public class ClanLeaderboardEntry
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000072B2 File Offset: 0x000054B2
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x000072BA File Offset: 0x000054BA
		public Guid ClanId { get; private set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000072C3 File Offset: 0x000054C3
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x000072CB File Offset: 0x000054CB
		public string Name { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x000072D4 File Offset: 0x000054D4
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x000072DC File Offset: 0x000054DC
		public string Tag { get; private set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x000072E5 File Offset: 0x000054E5
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x000072ED File Offset: 0x000054ED
		public string Sigil { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x000072F6 File Offset: 0x000054F6
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x000072FE File Offset: 0x000054FE
		public int WinCount { get; private set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00007307 File Offset: 0x00005507
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x0000730F File Offset: 0x0000550F
		public int LossCount { get; private set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00007318 File Offset: 0x00005518
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x00007320 File Offset: 0x00005520
		public float Score { get; private set; }

		// Token: 0x060005C8 RID: 1480 RVA: 0x00007329 File Offset: 0x00005529
		[JsonConstructor]
		public ClanLeaderboardEntry(Guid clanId, string name, string tag, string sigil, int winCount, int lossCount, float score)
		{
			this.ClanId = clanId;
			this.Name = name;
			this.Tag = tag;
			this.Sigil = sigil;
			this.WinCount = winCount;
			this.LossCount = lossCount;
			this.Score = score;
		}
	}
}
