using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A0 RID: 160
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentTargetPositionAndDirection : GameNetworkMessage
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0000B591 File Offset: 0x00009791
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0000B599 File Offset: 0x00009799
		public int AgentIndex { get; private set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0000B5A2 File Offset: 0x000097A2
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x0000B5AA File Offset: 0x000097AA
		public Vec2 Position { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0000B5B3 File Offset: 0x000097B3
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0000B5BB File Offset: 0x000097BB
		public Vec3 Direction { get; private set; }

		// Token: 0x06000668 RID: 1640 RVA: 0x0000B5C4 File Offset: 0x000097C4
		public SetAgentTargetPositionAndDirection(int agentIndex, ref Vec2 position, ref Vec3 direction)
		{
			this.AgentIndex = agentIndex;
			this.Position = position;
			this.Direction = direction;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0000B5EB File Offset: 0x000097EB
		public SetAgentTargetPositionAndDirection()
		{
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0000B5F4 File Offset: 0x000097F4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0000B635 File Offset: 0x00009835
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVec2ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0000B662 File Offset: 0x00009862
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0000B66C File Offset: 0x0000986C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set TargetPositionAndDirection: ", this.Position, " ", this.Direction, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
