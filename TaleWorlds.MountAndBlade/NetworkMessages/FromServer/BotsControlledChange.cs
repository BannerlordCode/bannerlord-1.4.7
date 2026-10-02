using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004F RID: 79
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BotsControlledChange : GameNetworkMessage
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x000055E0 File Offset: 0x000037E0
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x000055E8 File Offset: 0x000037E8
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x000055F1 File Offset: 0x000037F1
		// (set) Token: 0x060002AA RID: 682 RVA: 0x000055F9 File Offset: 0x000037F9
		public int AliveCount { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00005602 File Offset: 0x00003802
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000560A File Offset: 0x0000380A
		public int TotalCount { get; private set; }

		// Token: 0x060002AD RID: 685 RVA: 0x00005613 File Offset: 0x00003813
		public BotsControlledChange(NetworkCommunicator peer, int aliveCount, int totalCount)
		{
			this.Peer = peer;
			this.AliveCount = aliveCount;
			this.TotalCount = totalCount;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00005630 File Offset: 0x00003830
		public BotsControlledChange()
		{
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00005638 File Offset: 0x00003838
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.AliveCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			this.TotalCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000567A File Offset: 0x0000387A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.AliveCount, CompressionMission.AgentOffsetCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.TotalCount, CompressionMission.AgentOffsetCompressionInfo);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000056A7 File Offset: 0x000038A7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000056B0 File Offset: 0x000038B0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Bot Controlled Count Changed. Peer: ",
				this.Peer.UserName,
				" now has ",
				this.AliveCount,
				" alive bots, out of: ",
				this.TotalCount,
				" total bots."
			});
		}
	}
}
