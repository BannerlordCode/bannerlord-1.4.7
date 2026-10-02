using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000067 RID: 103
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ServerStatusMessage : Message
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00003744 File Offset: 0x00001944
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000374C File Offset: 0x0000194C
		[JsonProperty]
		public ServerStatus ServerStatus { get; private set; }

		// Token: 0x06000211 RID: 529 RVA: 0x00003755 File Offset: 0x00001955
		public ServerStatusMessage()
		{
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000375D File Offset: 0x0000195D
		public ServerStatusMessage(ServerStatus serverStatus)
		{
			this.ServerStatus = serverStatus;
		}
	}
}
