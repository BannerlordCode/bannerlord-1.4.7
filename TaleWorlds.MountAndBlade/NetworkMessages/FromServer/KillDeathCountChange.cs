using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000057 RID: 87
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class KillDeathCountChange : GameNetworkMessage
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00005EF5 File Offset: 0x000040F5
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00005EFD File Offset: 0x000040FD
		public NetworkCommunicator VictimPeer { get; private set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00005F06 File Offset: 0x00004106
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00005F0E File Offset: 0x0000410E
		public NetworkCommunicator AttackerPeer { get; private set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00005F17 File Offset: 0x00004117
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00005F1F File Offset: 0x0000411F
		public int KillCount { get; private set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00005F28 File Offset: 0x00004128
		// (set) Token: 0x0600030A RID: 778 RVA: 0x00005F30 File Offset: 0x00004130
		public int AssistCount { get; private set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00005F39 File Offset: 0x00004139
		// (set) Token: 0x0600030C RID: 780 RVA: 0x00005F41 File Offset: 0x00004141
		public int DeathCount { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00005F4A File Offset: 0x0000414A
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00005F52 File Offset: 0x00004152
		public int Score { get; private set; }

		// Token: 0x0600030F RID: 783 RVA: 0x00005F5B File Offset: 0x0000415B
		public KillDeathCountChange(NetworkCommunicator peer, NetworkCommunicator attackerPeer, int killCount, int assistCount, int deathCount, int score)
		{
			this.VictimPeer = peer;
			this.AttackerPeer = attackerPeer;
			this.KillCount = killCount;
			this.AssistCount = assistCount;
			this.DeathCount = deathCount;
			this.Score = score;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00005F90 File Offset: 0x00004190
		public KillDeathCountChange()
		{
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00005F98 File Offset: 0x00004198
		protected override bool OnRead()
		{
			bool flag = true;
			this.VictimPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.AttackerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			this.KillCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.AssistCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.DeathCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.Score = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.ScoreCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000600C File Offset: 0x0000420C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.VictimPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.AttackerPeer);
			GameNetworkMessage.WriteIntToPacket(this.KillCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AssistCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DeathCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Score, CompressionMatchmaker.ScoreCompressionInfo);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000606F File Offset: 0x0000426F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00006078 File Offset: 0x00004278
		protected override string OnGetLogFormat()
		{
			object[] array = new object[11];
			array[0] = "Kill-Death Count Changed. Peer: ";
			int num = 1;
			NetworkCommunicator victimPeer = this.VictimPeer;
			array[num] = ((victimPeer != null) ? victimPeer.UserName : null) ?? "NULL";
			array[2] = " killed peer: ";
			int num2 = 3;
			NetworkCommunicator attackerPeer = this.AttackerPeer;
			array[num2] = ((attackerPeer != null) ? attackerPeer.UserName : null) ?? "NULL";
			array[4] = " and now has ";
			array[5] = this.KillCount;
			array[6] = " kills, ";
			array[7] = this.AssistCount;
			array[8] = " assists, and ";
			array[9] = this.DeathCount;
			array[10] = " deaths.";
			return string.Concat(array);
		}
	}
}
