using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000057 RID: 87
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerAssignedPartyLeaderMessage : Message
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000033C0 File Offset: 0x000015C0
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000033C8 File Offset: 0x000015C8
		[JsonProperty]
		public PlayerId PartyLeaderId { get; private set; }

		// Token: 0x060001BF RID: 447 RVA: 0x000033D1 File Offset: 0x000015D1
		public PlayerAssignedPartyLeaderMessage()
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000033D9 File Offset: 0x000015D9
		public PlayerAssignedPartyLeaderMessage(PlayerId partyLeaderId)
		{
			this.PartyLeaderId = partyLeaderId;
		}
	}
}
