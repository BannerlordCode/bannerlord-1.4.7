using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BE RID: 190
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ReportPlayerMessage : Message
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00004524 File Offset: 0x00002724
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0000452C File Offset: 0x0000272C
		[JsonProperty]
		public Guid GameId { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00004535 File Offset: 0x00002735
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000453D File Offset: 0x0000273D
		[JsonProperty]
		public PlayerId ReportedPlayerId { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00004546 File Offset: 0x00002746
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000454E File Offset: 0x0000274E
		[JsonProperty]
		public string ReportedPlayerName { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00004557 File Offset: 0x00002757
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000455F File Offset: 0x0000275F
		[JsonProperty]
		public PlayerReportType Type { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00004568 File Offset: 0x00002768
		// (set) Token: 0x0600036D RID: 877 RVA: 0x00004570 File Offset: 0x00002770
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x0600036E RID: 878 RVA: 0x00004579 File Offset: 0x00002779
		public ReportPlayerMessage()
		{
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00004581 File Offset: 0x00002781
		public ReportPlayerMessage(Guid gameId, PlayerId reportedPlayerId, string reportedPlayerName, PlayerReportType type, string message)
		{
			this.GameId = gameId;
			this.ReportedPlayerId = reportedPlayerId;
			this.ReportedPlayerName = reportedPlayerName;
			this.Type = type;
			this.Message = message;
		}
	}
}
