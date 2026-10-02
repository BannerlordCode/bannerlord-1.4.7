using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000077 RID: 119
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AddFriendMessage : Message
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000252 RID: 594 RVA: 0x000039F9 File Offset: 0x00001BF9
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00003A01 File Offset: 0x00001C01
		[JsonProperty]
		public PlayerId FriendId { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00003A0A File Offset: 0x00001C0A
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00003A12 File Offset: 0x00001C12
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000256 RID: 598 RVA: 0x00003A1B File Offset: 0x00001C1B
		public AddFriendMessage()
		{
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00003A23 File Offset: 0x00001C23
		public AddFriendMessage(PlayerId friendId, bool dontUseNameForUnknownPlayer)
		{
			this.FriendId = friendId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
