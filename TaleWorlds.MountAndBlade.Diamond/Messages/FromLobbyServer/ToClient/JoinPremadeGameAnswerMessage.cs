using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004D RID: 77
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinPremadeGameAnswerMessage : Message
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000031FF File Offset: 0x000013FF
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00003207 File Offset: 0x00001407
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x06000195 RID: 405 RVA: 0x00003210 File Offset: 0x00001410
		public JoinPremadeGameAnswerMessage()
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00003218 File Offset: 0x00001418
		public JoinPremadeGameAnswerMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
