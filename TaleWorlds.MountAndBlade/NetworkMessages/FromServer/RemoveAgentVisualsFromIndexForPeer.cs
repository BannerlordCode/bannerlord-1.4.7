using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000094 RID: 148
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveAgentVisualsFromIndexForPeer : GameNetworkMessage
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0000A9F6 File Offset: 0x00008BF6
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0000A9FE File Offset: 0x00008BFE
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0000AA07 File Offset: 0x00008C07
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x0000AA0F File Offset: 0x00008C0F
		public int VisualsIndex { get; private set; }

		// Token: 0x060005DE RID: 1502 RVA: 0x0000AA18 File Offset: 0x00008C18
		public RemoveAgentVisualsFromIndexForPeer(NetworkCommunicator peer, int index)
		{
			this.Peer = peer;
			this.VisualsIndex = index;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0000AA2E File Offset: 0x00008C2E
		public RemoveAgentVisualsFromIndexForPeer()
		{
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0000AA38 File Offset: 0x00008C38
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VisualsIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000AA68 File Offset: 0x00008C68
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.VisualsIndex, CompressionMission.AgentOffsetCompressionInfo);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0000AA85 File Offset: 0x00008C85
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0000AA8D File Offset: 0x00008C8D
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Removing AgentVisuals with Index: ",
				this.VisualsIndex,
				", for peer: ",
				this.Peer.UserName
			});
		}
	}
}
