using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004E RID: 78
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinPremadeGameRequestMessage : Message
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00003227 File Offset: 0x00001427
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000322F File Offset: 0x0000142F
		[JsonProperty]
		public Guid ChallengerPartyId { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00003238 File Offset: 0x00001438
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00003240 File Offset: 0x00001440
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00003249 File Offset: 0x00001449
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00003251 File Offset: 0x00001451
		[JsonProperty]
		public string Sigil { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000325A File Offset: 0x0000145A
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00003262 File Offset: 0x00001462
		[JsonProperty]
		public PlayerId[] ChallengerPlayers { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000326B File Offset: 0x0000146B
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00003273 File Offset: 0x00001473
		[JsonProperty]
		public PlayerId ChallengerPartyLeaderId { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000327C File Offset: 0x0000147C
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00003284 File Offset: 0x00001484
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x060001A3 RID: 419 RVA: 0x0000328D File Offset: 0x0000148D
		public JoinPremadeGameRequestMessage()
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00003295 File Offset: 0x00001495
		public JoinPremadeGameRequestMessage(Guid challengerPartyId, string clanName, string sigil, PlayerId[] challengerPlayers, PlayerId challengerPartyLeaderId, PremadeGameType premadeGameType)
		{
			this.ChallengerPartyId = challengerPartyId;
			this.ClanName = clanName;
			this.Sigil = sigil;
			this.ChallengerPlayers = challengerPlayers;
			this.ChallengerPartyLeaderId = challengerPartyLeaderId;
			this.PremadeGameType = premadeGameType;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000032CA File Offset: 0x000014CA
		public static JoinPremadeGameRequestMessage CreateClanGameRequest(Guid challengerPartyId, string clanName, string sigil, PlayerId[] challengerPlayers)
		{
			return new JoinPremadeGameRequestMessage(challengerPartyId, clanName, sigil, challengerPlayers, PlayerId.Empty, PremadeGameType.Clan);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000032DB File Offset: 0x000014DB
		public static JoinPremadeGameRequestMessage CreatePracticeGameRequest(Guid challengerPartyId, PlayerId leaderId, PlayerId[] challengerPlayers)
		{
			return new JoinPremadeGameRequestMessage(challengerPartyId, null, null, challengerPlayers, leaderId, PremadeGameType.Practice);
		}
	}
}
