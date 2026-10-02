using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200003D RID: 61
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddPeerComponent : GameNetworkMessage
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000453E File Offset: 0x0000273E
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00004546 File Offset: 0x00002746
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000454F File Offset: 0x0000274F
		// (set) Token: 0x060001ED RID: 493 RVA: 0x00004557 File Offset: 0x00002757
		public uint ComponentId { get; private set; }

		// Token: 0x060001EE RID: 494 RVA: 0x00004560 File Offset: 0x00002760
		public AddPeerComponent(NetworkCommunicator peer, uint componentId)
		{
			this.Peer = peer;
			this.ComponentId = componentId;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00004576 File Offset: 0x00002776
		public AddPeerComponent()
		{
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000457E File Offset: 0x0000277E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteUintToPacket(this.ComponentId, CompressionBasic.PeerComponentCompressionInfo);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000459C File Offset: 0x0000279C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.ComponentId = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.PeerComponentCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000045CC File Offset: 0x000027CC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000045D0 File Offset: 0x000027D0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Add component with ID: ",
				this.ComponentId,
				" to peer:",
				this.Peer.UserName,
				" with peer-index:",
				this.Peer.Index
			});
		}
	}
}
