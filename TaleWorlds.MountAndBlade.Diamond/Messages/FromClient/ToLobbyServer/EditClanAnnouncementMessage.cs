using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000090 RID: 144
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class EditClanAnnouncementMessage : Message
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00003DE6 File Offset: 0x00001FE6
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00003DEE File Offset: 0x00001FEE
		[JsonProperty]
		public int AnnouncementId { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00003DF7 File Offset: 0x00001FF7
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00003DFF File Offset: 0x00001FFF
		[JsonProperty]
		public string Text { get; private set; }

		// Token: 0x060002B7 RID: 695 RVA: 0x00003E08 File Offset: 0x00002008
		public EditClanAnnouncementMessage()
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00003E10 File Offset: 0x00002010
		public EditClanAnnouncementMessage(int announcementId, string text)
		{
			this.AnnouncementId = announcementId;
			this.Text = text;
		}
	}
}
