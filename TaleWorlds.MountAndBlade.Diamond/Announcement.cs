using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000E9 RID: 233
	[Serializable]
	public class Announcement
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00005112 File Offset: 0x00003312
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x0000511A File Offset: 0x0000331A
		public int Id { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00005123 File Offset: 0x00003323
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x0000512B File Offset: 0x0000332B
		public Guid BattleId { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00005134 File Offset: 0x00003334
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x0000513C File Offset: 0x0000333C
		public AnnouncementType Type { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00005145 File Offset: 0x00003345
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x0000514D File Offset: 0x0000334D
		public string Text { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00005156 File Offset: 0x00003356
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x0000515E File Offset: 0x0000335E
		public bool IsEnabled { get; set; }

		// Token: 0x06000479 RID: 1145 RVA: 0x00005167 File Offset: 0x00003367
		public Announcement()
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000516F File Offset: 0x0000336F
		public Announcement(int id, Guid battleId, AnnouncementType type, string text, bool isEnabled)
		{
			this.Id = id;
			this.BattleId = battleId;
			this.Type = type;
			this.Text = text;
			this.IsEnabled = isEnabled;
		}
	}
}
