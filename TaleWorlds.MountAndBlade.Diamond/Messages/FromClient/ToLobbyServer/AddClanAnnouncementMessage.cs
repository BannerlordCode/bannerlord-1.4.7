using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000075 RID: 117
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AddClanAnnouncementMessage : Message
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00003979 File Offset: 0x00001B79
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00003981 File Offset: 0x00001B81
		[JsonProperty]
		public string Announcement { get; private set; }

		// Token: 0x06000248 RID: 584 RVA: 0x0000398A File Offset: 0x00001B8A
		public AddClanAnnouncementMessage()
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00003992 File Offset: 0x00001B92
		public AddClanAnnouncementMessage(string announcement)
		{
			this.Announcement = announcement;
		}
	}
}
