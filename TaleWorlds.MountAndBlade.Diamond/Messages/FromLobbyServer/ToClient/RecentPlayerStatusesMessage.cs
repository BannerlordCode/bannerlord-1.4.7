using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000061 RID: 97
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RecentPlayerStatusesMessage : Message
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00003613 File Offset: 0x00001813
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000361B File Offset: 0x0000181B
		[JsonProperty]
		public FriendInfo[] Friends { get; private set; }

		// Token: 0x060001F4 RID: 500 RVA: 0x00003624 File Offset: 0x00001824
		public RecentPlayerStatusesMessage()
		{
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000362C File Offset: 0x0000182C
		public RecentPlayerStatusesMessage(FriendInfo[] friends)
		{
			this.Friends = friends;
		}
	}
}
