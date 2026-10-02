using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BD RID: 189
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RemoveFriendMessage : Message
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000044FC File Offset: 0x000026FC
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00004504 File Offset: 0x00002704
		[JsonProperty]
		public PlayerId FriendId { get; private set; }

		// Token: 0x06000362 RID: 866 RVA: 0x0000450D File Offset: 0x0000270D
		public RemoveFriendMessage()
		{
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00004515 File Offset: 0x00002715
		public RemoveFriendMessage(PlayerId friendId)
		{
			this.FriendId = friendId;
		}
	}
}
