using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BB RID: 187
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RemoveClanAnnouncementMessage : Message
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000358 RID: 856 RVA: 0x000044AC File Offset: 0x000026AC
		// (set) Token: 0x06000359 RID: 857 RVA: 0x000044B4 File Offset: 0x000026B4
		[JsonProperty]
		public int AnnouncementId { get; private set; }

		// Token: 0x0600035A RID: 858 RVA: 0x000044BD File Offset: 0x000026BD
		public RemoveClanAnnouncementMessage()
		{
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000044C5 File Offset: 0x000026C5
		public RemoveClanAnnouncementMessage(int announcementId)
		{
			this.AnnouncementId = announcementId;
		}
	}
}
