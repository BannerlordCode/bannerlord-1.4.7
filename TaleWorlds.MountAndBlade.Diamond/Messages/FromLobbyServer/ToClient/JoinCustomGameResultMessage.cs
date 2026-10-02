using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class JoinCustomGameResultMessage : Message
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000315D File Offset: 0x0000135D
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00003165 File Offset: 0x00001365
		[JsonProperty]
		public JoinGameData JoinGameData { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000316E File Offset: 0x0000136E
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00003176 File Offset: 0x00001376
		[JsonProperty]
		public bool Success { get; private set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000317F File Offset: 0x0000137F
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00003187 File Offset: 0x00001387
		[JsonProperty]
		public CustomGameJoinResponse Response { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00003190 File Offset: 0x00001390
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00003198 File Offset: 0x00001398
		[JsonProperty]
		public string MatchId { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000031A1 File Offset: 0x000013A1
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000031A9 File Offset: 0x000013A9
		[JsonProperty]
		public bool IsAdmin { get; private set; }

		// Token: 0x0600018F RID: 399 RVA: 0x000031B2 File Offset: 0x000013B2
		public JoinCustomGameResultMessage()
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000031BA File Offset: 0x000013BA
		private JoinCustomGameResultMessage(JoinGameData joinGameData, bool success, CustomGameJoinResponse response, string matchId, bool isAdmin)
		{
			this.JoinGameData = joinGameData;
			this.Success = success;
			this.Response = response;
			this.MatchId = matchId;
			this.IsAdmin = isAdmin;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000031E7 File Offset: 0x000013E7
		public static JoinCustomGameResultMessage CreateSuccess(JoinGameData joinGameData, string matchId, bool isAdmin)
		{
			return new JoinCustomGameResultMessage(joinGameData, true, CustomGameJoinResponse.Success, matchId, isAdmin);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000031F3 File Offset: 0x000013F3
		public static JoinCustomGameResultMessage CreateFailed(CustomGameJoinResponse response)
		{
			return new JoinCustomGameResultMessage(null, false, response, null, false);
		}
	}
}
