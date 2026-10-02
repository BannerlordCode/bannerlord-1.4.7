using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000028 RID: 40
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CreateClanAnswerMessage : Message
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002A84 File Offset: 0x00000C84
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00002A8C File Offset: 0x00000C8C
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000DE RID: 222 RVA: 0x00002A95 File Offset: 0x00000C95
		public CreateClanAnswerMessage()
		{
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00002A9D File Offset: 0x00000C9D
		public CreateClanAnswerMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
