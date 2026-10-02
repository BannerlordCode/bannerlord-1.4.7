using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000100 RID: 256
	[Serializable]
	public class ClanAnnouncement
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00006E9E File Offset: 0x0000509E
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00006EA6 File Offset: 0x000050A6
		[JsonProperty]
		public int Id { get; private set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00006EAF File Offset: 0x000050AF
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x00006EB7 File Offset: 0x000050B7
		[JsonProperty]
		public string Announcement { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00006EC0 File Offset: 0x000050C0
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00006EC8 File Offset: 0x000050C8
		[JsonProperty]
		public PlayerId AuthorId { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00006ED1 File Offset: 0x000050D1
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00006ED9 File Offset: 0x000050D9
		[JsonProperty]
		public DateTime CreationTime { get; private set; }

		// Token: 0x06000574 RID: 1396 RVA: 0x00006EE2 File Offset: 0x000050E2
		public ClanAnnouncement(int id, string announcement, PlayerId authorId, DateTime creationTime)
		{
			this.Id = id;
			this.Announcement = announcement;
			this.AuthorId = authorId;
			this.CreationTime = creationTime;
		}
	}
}
