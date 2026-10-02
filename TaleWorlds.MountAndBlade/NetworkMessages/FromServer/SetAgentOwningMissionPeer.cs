using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009C RID: 156
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentOwningMissionPeer : GameNetworkMessage
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0000B1ED File Offset: 0x000093ED
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x0000B1F5 File Offset: 0x000093F5
		public int AgentIndex { get; private set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0000B1FE File Offset: 0x000093FE
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x0000B206 File Offset: 0x00009406
		public VirtualPlayer Peer { get; private set; }

		// Token: 0x0600063C RID: 1596 RVA: 0x0000B20F File Offset: 0x0000940F
		public SetAgentOwningMissionPeer(int agentIndex, VirtualPlayer peer)
		{
			this.AgentIndex = agentIndex;
			this.Peer = peer;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0000B225 File Offset: 0x00009425
		public SetAgentOwningMissionPeer()
		{
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0000B230 File Offset: 0x00009430
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Peer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref flag, true);
			return flag;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0000B25B File Offset: 0x0000945B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVirtualPlayerReferenceToPacket(this.Peer);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0000B273 File Offset: 0x00009473
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0000B27B File Offset: 0x0000947B
		protected override string OnGetLogFormat()
		{
			string text = "SetAgentOwningMissionPeer for agent-index: {0} to {1}";
			object obj = this.AgentIndex;
			VirtualPlayer peer = this.Peer;
			return string.Format(text, obj, ((peer != null) ? peer.UserName : null) ?? "null");
		}
	}
}
