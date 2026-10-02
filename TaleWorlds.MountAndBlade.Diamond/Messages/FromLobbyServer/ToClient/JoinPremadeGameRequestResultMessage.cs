using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004F RID: 79
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinPremadeGameRequestResultMessage : Message
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000032E8 File Offset: 0x000014E8
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000032F0 File Offset: 0x000014F0
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060001A9 RID: 425 RVA: 0x000032F9 File Offset: 0x000014F9
		public JoinPremadeGameRequestResultMessage()
		{
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00003301 File Offset: 0x00001501
		public JoinPremadeGameRequestResultMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
