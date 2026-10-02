using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000029 RID: 41
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CreatePremadeGameAnswerMessage : Message
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00002AAC File Offset: 0x00000CAC
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00002AB4 File Offset: 0x00000CB4
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000E2 RID: 226 RVA: 0x00002ABD File Offset: 0x00000CBD
		public CreatePremadeGameAnswerMessage()
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00002AC5 File Offset: 0x00000CC5
		public CreatePremadeGameAnswerMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
