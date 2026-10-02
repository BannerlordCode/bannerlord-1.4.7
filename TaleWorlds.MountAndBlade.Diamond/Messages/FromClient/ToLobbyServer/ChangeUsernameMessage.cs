using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000082 RID: 130
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeUsernameMessage : Message
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00003B81 File Offset: 0x00001D81
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00003B89 File Offset: 0x00001D89
		[JsonProperty]
		public string Username { get; private set; }

		// Token: 0x0600027B RID: 635 RVA: 0x00003B92 File Offset: 0x00001D92
		public ChangeUsernameMessage()
		{
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00003B9A File Offset: 0x00001D9A
		public ChangeUsernameMessage(string username)
		{
			this.Username = username;
		}
	}
}
