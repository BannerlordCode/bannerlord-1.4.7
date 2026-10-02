using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010A RID: 266
	[Serializable]
	public class ClanInfo
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00007165 File Offset: 0x00005365
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0000716D File Offset: 0x0000536D
		[JsonProperty]
		public Guid ClanId { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00007176 File Offset: 0x00005376
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0000717E File Offset: 0x0000537E
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00007187 File Offset: 0x00005387
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x0000718F File Offset: 0x0000538F
		[JsonProperty]
		public string Tag { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00007198 File Offset: 0x00005398
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x000071A0 File Offset: 0x000053A0
		[JsonProperty]
		public string Faction { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x000071A9 File Offset: 0x000053A9
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x000071B1 File Offset: 0x000053B1
		[JsonProperty]
		public string Sigil { get; private set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x000071BA File Offset: 0x000053BA
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x000071C2 File Offset: 0x000053C2
		[JsonProperty]
		public string InformationText { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x000071CB File Offset: 0x000053CB
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x000071D3 File Offset: 0x000053D3
		[JsonProperty]
		public ClanPlayer[] Players { get; private set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x000071DC File Offset: 0x000053DC
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x000071E4 File Offset: 0x000053E4
		[JsonProperty]
		public ClanAnnouncement[] Announcements { get; private set; }

		// Token: 0x060005B2 RID: 1458 RVA: 0x000071F0 File Offset: 0x000053F0
		public ClanInfo(Guid clanId, string name, string tag, string faction, string sigil, string information, ClanPlayer[] players, ClanAnnouncement[] announcements)
		{
			this.ClanId = clanId;
			this.Name = name;
			this.Tag = tag;
			this.Faction = faction;
			this.Sigil = sigil;
			this.Players = players;
			this.InformationText = information;
			this.Announcements = announcements;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00007240 File Offset: 0x00005440
		public static ClanInfo CreateUnavailableClanInfo()
		{
			return new ClanInfo(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new ClanPlayer[0], new ClanAnnouncement[0]);
		}
	}
}
