using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C7 RID: 199
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateNotificationsMessage : Message
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00004776 File Offset: 0x00002976
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000477E File Offset: 0x0000297E
		[JsonProperty]
		public int[] SeenNotificationIds { get; private set; }

		// Token: 0x0600039D RID: 925 RVA: 0x00004787 File Offset: 0x00002987
		public UpdateNotificationsMessage()
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000478F File Offset: 0x0000298F
		public UpdateNotificationsMessage(int[] seenNotificationIds)
		{
			this.SeenNotificationIds = seenNotificationIds;
		}
	}
}
