using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200008D RID: 141
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class DeclinePartyJoinRequestMessage : Message
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00003D96 File Offset: 0x00001F96
		// (set) Token: 0x060002AC RID: 684 RVA: 0x00003D9E File Offset: 0x00001F9E
		[JsonProperty]
		public PlayerId RequesterPlayerId { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00003DA7 File Offset: 0x00001FA7
		// (set) Token: 0x060002AE RID: 686 RVA: 0x00003DAF File Offset: 0x00001FAF
		[JsonProperty]
		public PartyJoinDeclineReason Reason { get; private set; }

		// Token: 0x060002AF RID: 687 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public DeclinePartyJoinRequestMessage()
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00003DC0 File Offset: 0x00001FC0
		public DeclinePartyJoinRequestMessage(PlayerId requesterPlayerId, PartyJoinDeclineReason reason)
		{
			this.RequesterPlayerId = requesterPlayerId;
			this.Reason = reason;
		}
	}
}
