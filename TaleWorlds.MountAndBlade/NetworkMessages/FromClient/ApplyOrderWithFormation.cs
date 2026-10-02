using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000022 RID: 34
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithFormation : GameNetworkMessage
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000035B1 File Offset: 0x000017B1
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000035B9 File Offset: 0x000017B9
		public OrderType OrderType { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000035C2 File Offset: 0x000017C2
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000035CA File Offset: 0x000017CA
		public int FormationIndex { get; private set; }

		// Token: 0x06000107 RID: 263 RVA: 0x000035D3 File Offset: 0x000017D3
		public ApplyOrderWithFormation(OrderType orderType, int formationIndex)
		{
			this.OrderType = orderType;
			this.FormationIndex = formationIndex;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000035E9 File Offset: 0x000017E9
		public ApplyOrderWithFormation()
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000035F4 File Offset: 0x000017F4
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003628 File Offset: 0x00001828
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000364A File Offset: 0x0000184A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00003652 File Offset: 0x00001852
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to formation with index: ", this.FormationIndex });
		}
	}
}
