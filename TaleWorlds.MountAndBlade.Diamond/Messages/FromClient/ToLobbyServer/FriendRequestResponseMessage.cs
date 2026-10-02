using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000093 RID: 147
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class FriendRequestResponseMessage : Message
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00003E36 File Offset: 0x00002036
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00003E3E File Offset: 0x0000203E
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00003E47 File Offset: 0x00002047
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00003E4F File Offset: 0x0000204F
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00003E58 File Offset: 0x00002058
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x00003E60 File Offset: 0x00002060
		[JsonProperty]
		public bool IsAccepted { get; private set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00003E69 File Offset: 0x00002069
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00003E71 File Offset: 0x00002071
		[JsonProperty]
		public bool IsBlocked { get; private set; }

		// Token: 0x060002C3 RID: 707 RVA: 0x00003E7A File Offset: 0x0000207A
		public FriendRequestResponseMessage()
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00003E82 File Offset: 0x00002082
		public FriendRequestResponseMessage(PlayerId playerId, bool dontUseNameForUnknownPlayer, bool isAccepted, bool isBlocked)
		{
			this.PlayerId = playerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
			this.IsAccepted = isAccepted;
			this.IsBlocked = isBlocked;
		}
	}
}
