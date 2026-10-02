using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005D RID: 93
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerRemovedFromPartyMessage : Message
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x000034C0 File Offset: 0x000016C0
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x000034C8 File Offset: 0x000016C8
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000034D1 File Offset: 0x000016D1
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x000034D9 File Offset: 0x000016D9
		[JsonProperty]
		public PartyRemoveReason Reason { get; private set; }

		// Token: 0x060001DA RID: 474 RVA: 0x000034E2 File Offset: 0x000016E2
		public PlayerRemovedFromPartyMessage()
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000034EA File Offset: 0x000016EA
		public PlayerRemovedFromPartyMessage(PlayerId playerId, PartyRemoveReason reason)
		{
			this.PlayerId = playerId;
			this.Reason = reason;
		}
	}
}
