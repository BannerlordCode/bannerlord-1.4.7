using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000031 RID: 49
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class FriendListMessage : Message
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00002C2C File Offset: 0x00000E2C
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00002C34 File Offset: 0x00000E34
		[JsonProperty]
		public FriendInfo[] Friends { get; private set; }

		// Token: 0x06000107 RID: 263 RVA: 0x00002C3D File Offset: 0x00000E3D
		public FriendListMessage()
		{
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00002C45 File Offset: 0x00000E45
		public FriendListMessage(FriendInfo[] friends)
		{
			this.Friends = friends;
		}
	}
}
