using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000023 RID: 35
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithFormationAndNumber : GameNetworkMessage
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000368B File Offset: 0x0000188B
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00003693 File Offset: 0x00001893
		public OrderType OrderType { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000369C File Offset: 0x0000189C
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000036A4 File Offset: 0x000018A4
		public int FormationIndex { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000036AD File Offset: 0x000018AD
		// (set) Token: 0x06000112 RID: 274 RVA: 0x000036B5 File Offset: 0x000018B5
		public int Number { get; private set; }

		// Token: 0x06000113 RID: 275 RVA: 0x000036BE File Offset: 0x000018BE
		public ApplyOrderWithFormationAndNumber(OrderType orderType, int formationIndex, int number)
		{
			this.OrderType = orderType;
			this.FormationIndex = formationIndex;
			this.Number = number;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000036DB File Offset: 0x000018DB
		public ApplyOrderWithFormationAndNumber()
		{
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000036E4 File Offset: 0x000018E4
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.Number = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.DebugIntNonCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000372A File Offset: 0x0000192A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Number, CompressionBasic.DebugIntNonCompressionInfo);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000375C File Offset: 0x0000195C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003764 File Offset: 0x00001964
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to formation with index: ", this.FormationIndex, " and number: ", this.Number });
		}
	}
}
