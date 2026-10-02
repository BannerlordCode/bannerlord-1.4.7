using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000021 RID: 33
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithAgent : GameNetworkMessage
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000034E3 File Offset: 0x000016E3
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000034EB File Offset: 0x000016EB
		public OrderType OrderType { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000034F4 File Offset: 0x000016F4
		// (set) Token: 0x060000FC RID: 252 RVA: 0x000034FC File Offset: 0x000016FC
		public int AgentIndex { get; private set; }

		// Token: 0x060000FD RID: 253 RVA: 0x00003505 File Offset: 0x00001705
		public ApplyOrderWithAgent(OrderType orderType, int agentIndex)
		{
			this.OrderType = orderType;
			this.AgentIndex = agentIndex;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000351B File Offset: 0x0000171B
		public ApplyOrderWithAgent()
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003524 File Offset: 0x00001724
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003553 File Offset: 0x00001753
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003570 File Offset: 0x00001770
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003578 File Offset: 0x00001778
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to agent with index: ", this.AgentIndex });
		}
	}
}
