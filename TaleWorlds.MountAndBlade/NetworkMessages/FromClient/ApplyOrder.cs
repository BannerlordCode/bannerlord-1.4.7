using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000020 RID: 32
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrder : GameNetworkMessage
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003467 File Offset: 0x00001667
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000346F File Offset: 0x0000166F
		public OrderType OrderType { get; private set; }

		// Token: 0x060000F3 RID: 243 RVA: 0x00003478 File Offset: 0x00001678
		public ApplyOrder(OrderType orderType)
		{
			this.OrderType = orderType;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003487 File Offset: 0x00001687
		public ApplyOrder()
		{
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00003490 File Offset: 0x00001690
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000034B2 File Offset: 0x000016B2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000034C4 File Offset: 0x000016C4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Orders;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000034CC File Offset: 0x000016CC
		protected override string OnGetLogFormat()
		{
			return "Apply order: " + this.OrderType;
		}
	}
}
