using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C7 RID: 199
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class StopUsingObject : GameNetworkMessage
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0000DF06 File Offset: 0x0000C106
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0000DF0E File Offset: 0x0000C10E
		public int AgentIndex { get; private set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0000DF17 File Offset: 0x0000C117
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0000DF1F File Offset: 0x0000C11F
		public bool IsSuccessful { get; private set; }

		// Token: 0x06000829 RID: 2089 RVA: 0x0000DF28 File Offset: 0x0000C128
		public StopUsingObject(int agentIndex, bool isSuccessful)
		{
			this.AgentIndex = agentIndex;
			this.IsSuccessful = isSuccessful;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0000DF3E File Offset: 0x0000C13E
		public StopUsingObject()
		{
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000DF48 File Offset: 0x0000C148
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsSuccessful = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0000DF72 File Offset: 0x0000C172
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsSuccessful);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0000DF8A File Offset: 0x0000C18A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed | MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0000DF92 File Offset: 0x0000C192
		protected override string OnGetLogFormat()
		{
			return "Stop using Object on Agent with agent-index: " + this.AgentIndex;
		}
	}
}
