using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BC RID: 188
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RemoveClanOfficerRoleForPlayerMessage : Message
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600035C RID: 860 RVA: 0x000044D4 File Offset: 0x000026D4
		// (set) Token: 0x0600035D RID: 861 RVA: 0x000044DC File Offset: 0x000026DC
		[JsonProperty]
		public PlayerId RemovedOfficerId { get; private set; }

		// Token: 0x0600035E RID: 862 RVA: 0x000044E5 File Offset: 0x000026E5
		public RemoveClanOfficerRoleForPlayerMessage()
		{
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000044ED File Offset: 0x000026ED
		public RemoveClanOfficerRoleForPlayerMessage(PlayerId removedOfficerId)
		{
			this.RemovedOfficerId = removedOfficerId;
		}
	}
}
