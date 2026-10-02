using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009B RID: 155
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentIsPlayer : GameNetworkMessage
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0000B135 File Offset: 0x00009335
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0000B13D File Offset: 0x0000933D
		public int AgentIndex { get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0000B146 File Offset: 0x00009346
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0000B14E File Offset: 0x0000934E
		public bool IsPlayer { get; private set; }

		// Token: 0x06000632 RID: 1586 RVA: 0x0000B157 File Offset: 0x00009357
		public SetAgentIsPlayer(int agentIndex, bool isPlayer)
		{
			this.AgentIndex = agentIndex;
			this.IsPlayer = isPlayer;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000B16D File Offset: 0x0000936D
		public SetAgentIsPlayer()
		{
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000B178 File Offset: 0x00009378
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsPlayer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0000B1A2 File Offset: 0x000093A2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayer);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0000B1BA File Offset: 0x000093BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0000B1C2 File Offset: 0x000093C2
		protected override string OnGetLogFormat()
		{
			return "Set Controller is player on Agent with agent-index: " + this.AgentIndex + (this.IsPlayer ? " - TRUE." : " - FALSE.");
		}
	}
}
