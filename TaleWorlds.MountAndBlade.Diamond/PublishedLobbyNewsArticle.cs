using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000153 RID: 339
	public class PublishedLobbyNewsArticle
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0000DD14 File Offset: 0x0000BF14
		public string Title { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0000DD1D File Offset: 0x0000BF1D
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0000DD25 File Offset: 0x0000BF25
		public int Type { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0000DD2E File Offset: 0x0000BF2E
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0000DD36 File Offset: 0x0000BF36
		public string Description { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0000DD3F File Offset: 0x0000BF3F
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0000DD47 File Offset: 0x0000BF47
		public string DateStart { get; set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0000DD50 File Offset: 0x0000BF50
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public string DateEnd { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0000DD61 File Offset: 0x0000BF61
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0000DD69 File Offset: 0x0000BF69
		public bool Pinned { get; set; }
	}
}
