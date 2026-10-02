using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000048 RID: 72
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelPreparationStartedForTheFirstTime : GameNetworkMessage
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00004E2B File Offset: 0x0000302B
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00004E33 File Offset: 0x00003033
		public NetworkCommunicator RequesterPeer { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00004E3C File Offset: 0x0000303C
		// (set) Token: 0x0600025C RID: 604 RVA: 0x00004E44 File Offset: 0x00003044
		public NetworkCommunicator RequesteePeer { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00004E4D File Offset: 0x0000304D
		// (set) Token: 0x0600025E RID: 606 RVA: 0x00004E55 File Offset: 0x00003055
		public int AreaIndex { get; private set; }

		// Token: 0x0600025F RID: 607 RVA: 0x00004E5E File Offset: 0x0000305E
		public DuelPreparationStartedForTheFirstTime(NetworkCommunicator requesterPeer, NetworkCommunicator requesteePeer, int areaIndex)
		{
			this.RequesterPeer = requesterPeer;
			this.RequesteePeer = requesteePeer;
			this.AreaIndex = areaIndex;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00004E7B File Offset: 0x0000307B
		public DuelPreparationStartedForTheFirstTime()
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00004E84 File Offset: 0x00003084
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequesterPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.RequesteePeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.AreaIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.DuelAreaIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00004EC2 File Offset: 0x000030C2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequesterPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequesteePeer);
			GameNetworkMessage.WriteIntToPacket(this.AreaIndex, CompressionMission.DuelAreaIndexCompressionInfo);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00004EEA File Offset: 0x000030EA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00004EF4 File Offset: 0x000030F4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Duel started between agent with name: ",
				this.RequesteePeer.UserName,
				" and index: ",
				this.RequesteePeer.Index,
				" and agent with name: ",
				this.RequesterPeer.UserName,
				" and index: ",
				this.RequesterPeer.Index
			});
		}
	}
}
