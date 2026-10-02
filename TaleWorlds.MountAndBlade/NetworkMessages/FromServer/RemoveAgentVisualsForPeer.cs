using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000093 RID: 147
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveAgentVisualsForPeer : GameNetworkMessage
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0000A981 File Offset: 0x00008B81
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0000A989 File Offset: 0x00008B89
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x060005D4 RID: 1492 RVA: 0x0000A992 File Offset: 0x00008B92
		public RemoveAgentVisualsForPeer(NetworkCommunicator peer)
		{
			this.Peer = peer;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000A9A1 File Offset: 0x00008BA1
		public RemoveAgentVisualsForPeer()
		{
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0000A9AC File Offset: 0x00008BAC
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0000A9CA File Offset: 0x00008BCA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0000A9D7 File Offset: 0x00008BD7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0000A9DF File Offset: 0x00008BDF
		protected override string OnGetLogFormat()
		{
			return "Removing all AgentVisuals for peer: " + this.Peer.UserName;
		}
	}
}
