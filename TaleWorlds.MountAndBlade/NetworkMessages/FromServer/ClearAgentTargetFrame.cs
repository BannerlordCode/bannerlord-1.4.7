using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007D RID: 125
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ClearAgentTargetFrame : GameNetworkMessage
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x000085DD File Offset: 0x000067DD
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x000085E5 File Offset: 0x000067E5
		public int AgentIndex { get; private set; }

		// Token: 0x06000484 RID: 1156 RVA: 0x000085EE File Offset: 0x000067EE
		public ClearAgentTargetFrame(int agentIndex)
		{
			this.AgentIndex = agentIndex;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000085FD File Offset: 0x000067FD
		public ClearAgentTargetFrame()
		{
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00008608 File Offset: 0x00006808
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00008625 File Offset: 0x00006825
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00008632 File Offset: 0x00006832
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000863A File Offset: 0x0000683A
		protected override string OnGetLogFormat()
		{
			return "Clear target frame on agent with agent-index: " + this.AgentIndex;
		}
	}
}
