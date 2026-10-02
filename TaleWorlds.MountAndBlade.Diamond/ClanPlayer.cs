using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200010D RID: 269
	[Serializable]
	public class ClanPlayer
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00007366 File Offset: 0x00005566
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x0000736E File Offset: 0x0000556E
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00007377 File Offset: 0x00005577
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0000737F File Offset: 0x0000557F
		[JsonProperty]
		public Guid ClanId { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00007388 File Offset: 0x00005588
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x00007390 File Offset: 0x00005590
		[JsonProperty]
		public ClanPlayerRole Role { get; private set; }

		// Token: 0x060005CF RID: 1487 RVA: 0x00007399 File Offset: 0x00005599
		public ClanPlayer(PlayerId playerId, Guid clanId, ClanPlayerRole role)
		{
			this.PlayerId = playerId;
			this.ClanId = clanId;
			this.Role = role;
		}
	}
}
