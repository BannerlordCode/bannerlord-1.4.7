using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009F RID: 159
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentTargetPosition : GameNetworkMessage
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0000B4BF File Offset: 0x000096BF
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x0000B4C7 File Offset: 0x000096C7
		public int AgentIndex { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0000B4D0 File Offset: 0x000096D0
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x0000B4D8 File Offset: 0x000096D8
		public Vec2 Position { get; private set; }

		// Token: 0x0600065C RID: 1628 RVA: 0x0000B4E1 File Offset: 0x000096E1
		public SetAgentTargetPosition(int agentIndex, ref Vec2 position)
		{
			this.AgentIndex = agentIndex;
			this.Position = position;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0000B4FC File Offset: 0x000096FC
		public SetAgentTargetPosition()
		{
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0000B504 File Offset: 0x00009704
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0000B533 File Offset: 0x00009733
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVec2ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0000B550 File Offset: 0x00009750
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0000B558 File Offset: 0x00009758
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Target Position: ", this.Position, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
