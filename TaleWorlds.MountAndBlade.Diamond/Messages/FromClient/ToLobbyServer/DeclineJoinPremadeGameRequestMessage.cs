using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200008B RID: 139
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class DeclineJoinPremadeGameRequestMessage : Message
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00003D66 File Offset: 0x00001F66
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x00003D6E File Offset: 0x00001F6E
		[JsonProperty]
		public Guid PartyId { get; private set; }

		// Token: 0x060002A8 RID: 680 RVA: 0x00003D77 File Offset: 0x00001F77
		public DeclineJoinPremadeGameRequestMessage()
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00003D7F File Offset: 0x00001F7F
		public DeclineJoinPremadeGameRequestMessage(Guid partyId)
		{
			this.PartyId = partyId;
		}
	}
}
