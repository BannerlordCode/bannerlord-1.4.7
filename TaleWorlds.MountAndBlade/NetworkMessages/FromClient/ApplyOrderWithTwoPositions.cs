using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000027 RID: 39
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithTwoPositions : GameNetworkMessage
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00003A43 File Offset: 0x00001C43
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00003A4B File Offset: 0x00001C4B
		public OrderType OrderType { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00003A54 File Offset: 0x00001C54
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00003A5C File Offset: 0x00001C5C
		public Vec3 Position1 { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00003A65 File Offset: 0x00001C65
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00003A6D File Offset: 0x00001C6D
		public Vec3 Position2 { get; private set; }

		// Token: 0x0600013D RID: 317 RVA: 0x00003A76 File Offset: 0x00001C76
		public ApplyOrderWithTwoPositions(OrderType orderType, Vec3 position1, Vec3 position2)
		{
			this.OrderType = orderType;
			this.Position1 = position1;
			this.Position2 = position2;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00003A93 File Offset: 0x00001C93
		public ApplyOrderWithTwoPositions()
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003A9C File Offset: 0x00001C9C
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.Position1 = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.OrderPositionCompressionInfo, ref flag);
			this.Position2 = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.OrderPositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00003AE2 File Offset: 0x00001CE2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Position1, CompressionMission.OrderPositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Position2, CompressionMission.OrderPositionCompressionInfo);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00003B14 File Offset: 0x00001D14
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00003B1C File Offset: 0x00001D1C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to position 1: ", this.Position1, " and position 2: ", this.Position2 });
		}
	}
}
