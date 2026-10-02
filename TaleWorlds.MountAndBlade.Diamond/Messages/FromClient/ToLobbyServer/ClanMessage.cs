using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000085 RID: 133
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ClanMessage : Message
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00003BF9 File Offset: 0x00001DF9
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00003C01 File Offset: 0x00001E01
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000287 RID: 647 RVA: 0x00003C0A File Offset: 0x00001E0A
		public ClanMessage()
		{
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00003C12 File Offset: 0x00001E12
		public ClanMessage(string message)
		{
			this.Message = message;
		}
	}
}
