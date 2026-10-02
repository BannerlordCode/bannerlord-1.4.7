using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AD RID: 173
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InviteToClanMessage : Message
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000310 RID: 784 RVA: 0x000041A1 File Offset: 0x000023A1
		// (set) Token: 0x06000311 RID: 785 RVA: 0x000041A9 File Offset: 0x000023A9
		[JsonProperty]
		public PlayerId InvitedPlayerId { get; private set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000041B2 File Offset: 0x000023B2
		// (set) Token: 0x06000313 RID: 787 RVA: 0x000041BA File Offset: 0x000023BA
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000314 RID: 788 RVA: 0x000041C3 File Offset: 0x000023C3
		public InviteToClanMessage()
		{
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000041CB File Offset: 0x000023CB
		public InviteToClanMessage(PlayerId invitedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.InvitedPlayerId = invitedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
