using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A0 RID: 160
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerByUsernameAndIdMessage : Message
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00003F87 File Offset: 0x00002187
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00003F8F File Offset: 0x0000218F
		[JsonProperty]
		public string Username { get; private set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00003F98 File Offset: 0x00002198
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00003FA0 File Offset: 0x000021A0
		[JsonProperty]
		public int UserId { get; private set; }

		// Token: 0x060002E1 RID: 737 RVA: 0x00003FA9 File Offset: 0x000021A9
		public GetPlayerByUsernameAndIdMessage()
		{
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00003FB1 File Offset: 0x000021B1
		public GetPlayerByUsernameAndIdMessage(string username, int userId)
		{
			this.Username = username;
			this.UserId = userId;
		}
	}
}
