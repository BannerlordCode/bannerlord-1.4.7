using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B2 RID: 178
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PartyMessage : Message
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00004299 File Offset: 0x00002499
		// (set) Token: 0x06000329 RID: 809 RVA: 0x000042A1 File Offset: 0x000024A1
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x0600032A RID: 810 RVA: 0x000042AA File Offset: 0x000024AA
		public PartyMessage()
		{
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000042B2 File Offset: 0x000024B2
		public PartyMessage(string message)
		{
			this.Message = message;
		}
	}
}
