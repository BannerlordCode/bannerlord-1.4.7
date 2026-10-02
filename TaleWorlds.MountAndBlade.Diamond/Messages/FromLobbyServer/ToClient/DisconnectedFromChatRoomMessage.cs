using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace TaleWorlds.MountAndBlade.Diamond.Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200016D RID: 365
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class DisconnectedFromChatRoomMessage : Message
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00010261 File Offset: 0x0000E461
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x00010269 File Offset: 0x0000E469
		[JsonProperty]
		public Guid RoomId { get; private set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00010272 File Offset: 0x0000E472
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0001027A File Offset: 0x0000E47A
		[JsonProperty]
		public string RoomName { get; private set; }

		// Token: 0x06000A1B RID: 2587 RVA: 0x00010283 File Offset: 0x0000E483
		public DisconnectedFromChatRoomMessage()
		{
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001028B File Offset: 0x0000E48B
		public DisconnectedFromChatRoomMessage(Guid roomId, string roomName)
		{
			this.RoomId = roomId;
			this.RoomName = roomName;
		}
	}
}
