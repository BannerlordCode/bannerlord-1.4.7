using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C2 RID: 194
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RequestToJoinPremadeGameMessage : Message
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000464E File Offset: 0x0000284E
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00004656 File Offset: 0x00002856
		[JsonProperty]
		public Guid GameId { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000465F File Offset: 0x0000285F
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00004667 File Offset: 0x00002867
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x06000383 RID: 899 RVA: 0x00004670 File Offset: 0x00002870
		public RequestToJoinPremadeGameMessage()
		{
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00004678 File Offset: 0x00002878
		public RequestToJoinPremadeGameMessage(Guid gameId, string password)
		{
			this.GameId = gameId;
			this.Password = password;
		}
	}
}
