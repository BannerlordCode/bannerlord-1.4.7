using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000016 RID: 22
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class AdminMessage : Message
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002738 File Offset: 0x00000938
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00002740 File Offset: 0x00000940
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x0600008D RID: 141 RVA: 0x00002749 File Offset: 0x00000949
		public AdminMessage()
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002751 File Offset: 0x00000951
		public AdminMessage(string message)
		{
			this.Message = message;
		}
	}
}
