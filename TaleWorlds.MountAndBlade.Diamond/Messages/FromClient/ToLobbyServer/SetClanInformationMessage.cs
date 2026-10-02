using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C4 RID: 196
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class SetClanInformationMessage : Message
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000389 RID: 905 RVA: 0x000046B6 File Offset: 0x000028B6
		// (set) Token: 0x0600038A RID: 906 RVA: 0x000046BE File Offset: 0x000028BE
		[JsonProperty]
		public string Information { get; private set; }

		// Token: 0x0600038B RID: 907 RVA: 0x000046C7 File Offset: 0x000028C7
		public SetClanInformationMessage()
		{
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000046CF File Offset: 0x000028CF
		public SetClanInformationMessage(string information)
		{
			this.Information = information;
		}
	}
}
