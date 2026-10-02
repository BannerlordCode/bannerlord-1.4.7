using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007F RID: 127
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeGameTypesMessage : Message
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00003B09 File Offset: 0x00001D09
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00003B11 File Offset: 0x00001D11
		[JsonProperty]
		public string[] GameTypes { get; private set; }

		// Token: 0x0600026F RID: 623 RVA: 0x00003B1A File Offset: 0x00001D1A
		public ChangeGameTypesMessage()
		{
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00003B22 File Offset: 0x00001D22
		public ChangeGameTypesMessage(string[] gameTypes)
		{
			this.GameTypes = gameTypes;
		}
	}
}
