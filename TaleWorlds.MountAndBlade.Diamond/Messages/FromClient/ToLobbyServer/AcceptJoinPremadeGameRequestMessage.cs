using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000072 RID: 114
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AcceptJoinPremadeGameRequestMessage : Message
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00003921 File Offset: 0x00001B21
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00003929 File Offset: 0x00001B29
		[JsonProperty]
		public Guid PartyId { get; private set; }

		// Token: 0x0600023F RID: 575 RVA: 0x00003932 File Offset: 0x00001B32
		public AcceptJoinPremadeGameRequestMessage()
		{
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000393A File Offset: 0x00001B3A
		public AcceptJoinPremadeGameRequestMessage(Guid partyId)
		{
			this.PartyId = partyId;
		}
	}
}
