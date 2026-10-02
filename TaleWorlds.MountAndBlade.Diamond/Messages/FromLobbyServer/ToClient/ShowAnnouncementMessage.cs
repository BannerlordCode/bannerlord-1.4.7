using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000068 RID: 104
	[Serializable]
	public class ShowAnnouncementMessage : Message
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000376C File Offset: 0x0000196C
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00003774 File Offset: 0x00001974
		[JsonProperty]
		public Announcement Announcement { get; private set; }

		// Token: 0x06000215 RID: 533 RVA: 0x0000377D File Offset: 0x0000197D
		public ShowAnnouncementMessage()
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00003785 File Offset: 0x00001985
		public ShowAnnouncementMessage(Announcement announcement)
		{
			this.Announcement = announcement;
		}
	}
}
