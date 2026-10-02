using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000063 RID: 99
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PollProgress : GameNetworkMessage
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00006BCB File Offset: 0x00004DCB
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00006BD3 File Offset: 0x00004DD3
		public int VotesAccepted { get; private set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00006BDC File Offset: 0x00004DDC
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00006BE4 File Offset: 0x00004DE4
		public int VotesRejected { get; private set; }

		// Token: 0x0600036F RID: 879 RVA: 0x00006BED File Offset: 0x00004DED
		public PollProgress(int votesAccepted, int votesRejected)
		{
			this.VotesAccepted = votesAccepted;
			this.VotesRejected = votesRejected;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00006C03 File Offset: 0x00004E03
		public PollProgress()
		{
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00006C0C File Offset: 0x00004E0C
		protected override bool OnRead()
		{
			bool flag = true;
			this.VotesAccepted = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.VotesRejected = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00006C40 File Offset: 0x00004E40
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.VotesAccepted, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.VotesRejected, CompressionBasic.PlayerCompressionInfo);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00006C62 File Offset: 0x00004E62
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00006C6A File Offset: 0x00004E6A
		protected override string OnGetLogFormat()
		{
			return "Update on the voting progress.";
		}
	}
}
