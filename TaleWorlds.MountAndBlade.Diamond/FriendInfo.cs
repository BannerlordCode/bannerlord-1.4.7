using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000118 RID: 280
	[Serializable]
	public class FriendInfo
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00007F6A File Offset: 0x0000616A
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00007F72 File Offset: 0x00006172
		public PlayerId Id { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00007F7B File Offset: 0x0000617B
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00007F83 File Offset: 0x00006183
		public FriendStatus Status { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00007F8C File Offset: 0x0000618C
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00007F94 File Offset: 0x00006194
		public string Name { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00007F9D File Offset: 0x0000619D
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00007FA5 File Offset: 0x000061A5
		public bool IsOnline { get; set; }
	}
}
