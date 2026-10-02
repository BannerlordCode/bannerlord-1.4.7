using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004B RID: 75
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelSessionStarted : GameNetworkMessage
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000279 RID: 633 RVA: 0x000050BE File Offset: 0x000032BE
		// (set) Token: 0x0600027A RID: 634 RVA: 0x000050C6 File Offset: 0x000032C6
		public NetworkCommunicator RequesterPeer { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600027B RID: 635 RVA: 0x000050CF File Offset: 0x000032CF
		// (set) Token: 0x0600027C RID: 636 RVA: 0x000050D7 File Offset: 0x000032D7
		public NetworkCommunicator RequestedPeer { get; private set; }

		// Token: 0x0600027D RID: 637 RVA: 0x000050E0 File Offset: 0x000032E0
		public DuelSessionStarted(NetworkCommunicator requesterPeer, NetworkCommunicator requestedPeer)
		{
			this.RequesterPeer = requesterPeer;
			this.RequestedPeer = requestedPeer;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000050F6 File Offset: 0x000032F6
		public DuelSessionStarted()
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00005100 File Offset: 0x00003300
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequesterPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.RequestedPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000512C File Offset: 0x0000332C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequesterPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequestedPeer);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00005144 File Offset: 0x00003344
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000514C File Offset: 0x0000334C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Duel session started between agent with name: ",
				this.RequestedPeer.UserName,
				" and index: ",
				this.RequestedPeer.Index,
				" and agent with name: ",
				this.RequesterPeer.UserName,
				" and index: ",
				this.RequesterPeer.Index
			});
		}
	}
}
