using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D8 RID: 216
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemovePeerComponent : GameNetworkMessage
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0000EDF8 File Offset: 0x0000CFF8
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0000EE00 File Offset: 0x0000D000
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0000EE09 File Offset: 0x0000D009
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0000EE11 File Offset: 0x0000D011
		public uint ComponentId { get; private set; }

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000EE1A File Offset: 0x0000D01A
		public RemovePeerComponent(NetworkCommunicator peer, uint componentId)
		{
			this.Peer = peer;
			this.ComponentId = componentId;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000EE30 File Offset: 0x0000D030
		public RemovePeerComponent()
		{
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000EE38 File Offset: 0x0000D038
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteUintToPacket(this.ComponentId, CompressionBasic.PeerComponentCompressionInfo);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000EE58 File Offset: 0x0000D058
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.ComponentId = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.PeerComponentCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000EE88 File Offset: 0x0000D088
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000EE8C File Offset: 0x0000D08C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Remove component with ID: ",
				this.ComponentId,
				" from peer: ",
				this.Peer.UserName,
				" with peer-index: ",
				this.Peer.Index
			});
		}
	}
}
