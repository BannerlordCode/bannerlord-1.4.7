using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000058 RID: 88
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerInvitedToPartyMessage : Message
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000033E8 File Offset: 0x000015E8
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000033F0 File Offset: 0x000015F0
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000033F9 File Offset: 0x000015F9
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00003401 File Offset: 0x00001601
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x060001C5 RID: 453 RVA: 0x0000340A File Offset: 0x0000160A
		public PlayerInvitedToPartyMessage()
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00003412 File Offset: 0x00001612
		public PlayerInvitedToPartyMessage(PlayerId playerId, string playerName)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
		}
	}
}
