using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000075 RID: 117
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AgentTeleportToFrame : GameNetworkMessage
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00007D07 File Offset: 0x00005F07
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x00007D0F File Offset: 0x00005F0F
		public int AgentIndex { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00007D18 File Offset: 0x00005F18
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x00007D20 File Offset: 0x00005F20
		public Vec3 Position { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00007D29 File Offset: 0x00005F29
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x00007D31 File Offset: 0x00005F31
		public Vec2 Direction { get; private set; }

		// Token: 0x0600042E RID: 1070 RVA: 0x00007D3A File Offset: 0x00005F3A
		public AgentTeleportToFrame(int agentIndex, Vec3 position, Vec2 direction)
		{
			this.AgentIndex = agentIndex;
			this.Position = position;
			this.Direction = direction.Normalized();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00007D5D File Offset: 0x00005F5D
		public AgentTeleportToFrame()
		{
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00007D68 File Offset: 0x00005F68
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00007DA9 File Offset: 0x00005FA9
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec2ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00007DD6 File Offset: 0x00005FD6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00007DE0 File Offset: 0x00005FE0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Teleporting agent with agent-index: ", this.AgentIndex, " to frame with position: ", this.Position, " and direction: ", this.Direction });
		}
	}
}
