using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009D RID: 157
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentPeer : GameNetworkMessage
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0000B2AD File Offset: 0x000094AD
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x0000B2B5 File Offset: 0x000094B5
		public int AgentIndex { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0000B2BE File Offset: 0x000094BE
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0000B2C6 File Offset: 0x000094C6
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x06000646 RID: 1606 RVA: 0x0000B2CF File Offset: 0x000094CF
		public SetAgentPeer(int agentIndex, NetworkCommunicator peer)
		{
			this.AgentIndex = agentIndex;
			this.Peer = peer;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000B2E5 File Offset: 0x000094E5
		public SetAgentPeer()
		{
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0000B2F0 File Offset: 0x000094F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			return flag;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0000B31B File Offset: 0x0000951B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0000B333 File Offset: 0x00009533
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.Agents;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0000B33C File Offset: 0x0000953C
		protected override string OnGetLogFormat()
		{
			if (this.AgentIndex < 0)
			{
				return "Ignoring the message for invalid agent.";
			}
			return string.Concat(new object[]
			{
				"Set NetworkPeer ",
				(this.Peer != null) ? "" : "(to NULL) ",
				"on Agent with agent-index: ",
				this.AgentIndex
			});
		}
	}
}
