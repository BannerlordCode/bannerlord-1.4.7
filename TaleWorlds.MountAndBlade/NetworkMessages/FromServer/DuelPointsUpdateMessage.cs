using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000047 RID: 71
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelPointsUpdateMessage : GameNetworkMessage
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00004D06 File Offset: 0x00002F06
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00004D0E File Offset: 0x00002F0E
		public int Bounty { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00004D17 File Offset: 0x00002F17
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00004D1F File Offset: 0x00002F1F
		public int Score { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00004D28 File Offset: 0x00002F28
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00004D30 File Offset: 0x00002F30
		public int NumberOfWins { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00004D39 File Offset: 0x00002F39
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00004D41 File Offset: 0x00002F41
		public NetworkCommunicator NetworkCommunicator { get; private set; }

		// Token: 0x06000253 RID: 595 RVA: 0x00004D4A File Offset: 0x00002F4A
		public DuelPointsUpdateMessage()
		{
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00004D52 File Offset: 0x00002F52
		public DuelPointsUpdateMessage(DuelMissionRepresentative representative)
		{
			this.Bounty = representative.Bounty;
			this.Score = representative.Score;
			this.NumberOfWins = representative.NumberOfWins;
			this.NetworkCommunicator = representative.GetNetworkPeer();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00004D8A File Offset: 0x00002F8A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Bounty, CompressionMatchmaker.ScoreCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Score, CompressionMatchmaker.ScoreCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.NumberOfWins, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.NetworkCommunicator);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00004DC8 File Offset: 0x00002FC8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Bounty = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.ScoreCompressionInfo, ref flag);
			this.Score = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.ScoreCompressionInfo, ref flag);
			this.NumberOfWins = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.NetworkCommunicator = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00004E1C File Offset: 0x0000301C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00004E24 File Offset: 0x00003024
		protected override string OnGetLogFormat()
		{
			return "PointUpdateMessage";
		}
	}
}
