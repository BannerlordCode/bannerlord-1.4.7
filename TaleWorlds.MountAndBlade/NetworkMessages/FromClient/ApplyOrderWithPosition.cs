using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000026 RID: 38
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithPosition : GameNetworkMessage
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000396A File Offset: 0x00001B6A
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00003972 File Offset: 0x00001B72
		public OrderType OrderType { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000397B File Offset: 0x00001B7B
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00003983 File Offset: 0x00001B83
		public Vec3 Position { get; private set; }

		// Token: 0x06000131 RID: 305 RVA: 0x0000398C File Offset: 0x00001B8C
		public ApplyOrderWithPosition(OrderType orderType, Vec3 position)
		{
			this.OrderType = orderType;
			this.Position = position;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000039A2 File Offset: 0x00001BA2
		public ApplyOrderWithPosition()
		{
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000039AC File Offset: 0x00001BAC
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.OrderPositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000039E0 File Offset: 0x00001BE0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionMission.OrderPositionCompressionInfo);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00003A02 File Offset: 0x00001C02
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00003A0A File Offset: 0x00001C0A
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to position: ", this.Position });
		}
	}
}
