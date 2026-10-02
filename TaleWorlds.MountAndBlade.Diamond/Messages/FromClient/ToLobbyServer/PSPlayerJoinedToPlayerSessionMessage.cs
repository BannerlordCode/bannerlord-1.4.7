using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B6 RID: 182
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PSPlayerJoinedToPlayerSessionMessage : Message
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00004351 File Offset: 0x00002551
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00004359 File Offset: 0x00002559
		[JsonProperty]
		public ulong InviterPlayerAccountId { get; private set; }

		// Token: 0x0600033C RID: 828 RVA: 0x00004362 File Offset: 0x00002562
		public PSPlayerJoinedToPlayerSessionMessage()
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000436A File Offset: 0x0000256A
		public PSPlayerJoinedToPlayerSessionMessage(ulong inviterPlayerAccountId)
		{
			this.InviterPlayerAccountId = inviterPlayerAccountId;
		}
	}
}
