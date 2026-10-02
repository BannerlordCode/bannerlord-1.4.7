using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005F RID: 95
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerSuggestedToPartyMessage : Message
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000357A File Offset: 0x0000177A
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00003582 File Offset: 0x00001782
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000358B File Offset: 0x0000178B
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00003593 File Offset: 0x00001793
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000359C File Offset: 0x0000179C
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x000035A4 File Offset: 0x000017A4
		[JsonProperty]
		public PlayerId SuggestingPlayerId { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000035AD File Offset: 0x000017AD
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000035B5 File Offset: 0x000017B5
		[JsonProperty]
		public string SuggestingPlayerName { get; private set; }

		// Token: 0x060001EC RID: 492 RVA: 0x000035BE File Offset: 0x000017BE
		public PlayerSuggestedToPartyMessage()
		{
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000035C6 File Offset: 0x000017C6
		public PlayerSuggestedToPartyMessage(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.SuggestingPlayerId = suggestingPlayerId;
			this.SuggestingPlayerName = suggestingPlayerName;
		}
	}
}
