using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006A RID: 106
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class SystemMessage : Message
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600021B RID: 539 RVA: 0x000037BC File Offset: 0x000019BC
		// (set) Token: 0x0600021C RID: 540 RVA: 0x000037C4 File Offset: 0x000019C4
		[JsonProperty]
		public ServerInfoMessage Message { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000037CD File Offset: 0x000019CD
		// (set) Token: 0x0600021E RID: 542 RVA: 0x000037D5 File Offset: 0x000019D5
		[JsonProperty]
		public List<string> Parameters { get; private set; }

		// Token: 0x0600021F RID: 543 RVA: 0x000037DE File Offset: 0x000019DE
		public SystemMessage()
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000037E6 File Offset: 0x000019E6
		public SystemMessage(ServerInfoMessage message, params string[] arguments)
		{
			this.Message = message;
			this.Parameters = new List<string>(arguments);
		}
	}
}
