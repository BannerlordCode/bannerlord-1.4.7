using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B3 RID: 179
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PlatformPlayerJoinedToPlayerSessionMessage : Message
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000042C1 File Offset: 0x000024C1
		// (set) Token: 0x0600032D RID: 813 RVA: 0x000042C9 File Offset: 0x000024C9
		[JsonProperty]
		public PlayerId InviterPlayerId { get; private set; }

		// Token: 0x0600032E RID: 814 RVA: 0x000042D2 File Offset: 0x000024D2
		public PlatformPlayerJoinedToPlayerSessionMessage()
		{
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000042DA File Offset: 0x000024DA
		public PlatformPlayerJoinedToPlayerSessionMessage(PlayerId inviterPlayerId)
		{
			this.InviterPlayerId = inviterPlayerId;
		}
	}
}
