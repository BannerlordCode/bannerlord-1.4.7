using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000051 RID: 81
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class LobbyNotificationsMessage : Message
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00003318 File Offset: 0x00001518
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00003320 File Offset: 0x00001520
		[JsonProperty]
		public LobbyNotification[] Notifications { get; private set; }

		// Token: 0x060001AE RID: 430 RVA: 0x00003329 File Offset: 0x00001529
		public LobbyNotificationsMessage()
		{
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00003331 File Offset: 0x00001531
		public LobbyNotificationsMessage(LobbyNotification[] notifications)
		{
			this.Notifications = notifications;
		}
	}
}
