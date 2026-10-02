using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000065 RID: 101
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RequestJoinPartyMessage : Message
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000036AB File Offset: 0x000018AB
		// (set) Token: 0x06000202 RID: 514 RVA: 0x000036B3 File Offset: 0x000018B3
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000036BC File Offset: 0x000018BC
		// (set) Token: 0x06000204 RID: 516 RVA: 0x000036C4 File Offset: 0x000018C4
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000036CD File Offset: 0x000018CD
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000036D5 File Offset: 0x000018D5
		[JsonProperty]
		public PlayerId ViaPlayerId { get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000207 RID: 519 RVA: 0x000036DE File Offset: 0x000018DE
		// (set) Token: 0x06000208 RID: 520 RVA: 0x000036E6 File Offset: 0x000018E6
		[JsonProperty]
		public string ViaPlayerName { get; private set; }

		// Token: 0x06000209 RID: 521 RVA: 0x000036EF File Offset: 0x000018EF
		public RequestJoinPartyMessage()
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000036F7 File Offset: 0x000018F7
		public RequestJoinPartyMessage(PlayerId playerId, string playerName, PlayerId viaPlayerId, string viaPlayerName)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.ViaPlayerId = viaPlayerId;
			this.ViaPlayerName = viaPlayerName;
		}
	}
}
