using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000024 RID: 36
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithFormationAndPercentage : GameNetworkMessage
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000037BE File Offset: 0x000019BE
		// (set) Token: 0x0600011A RID: 282 RVA: 0x000037C6 File Offset: 0x000019C6
		public OrderType OrderType { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000037CF File Offset: 0x000019CF
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000037D7 File Offset: 0x000019D7
		public int FormationIndex { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000037E0 File Offset: 0x000019E0
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000037E8 File Offset: 0x000019E8
		public int Percentage { get; private set; }

		// Token: 0x0600011F RID: 287 RVA: 0x000037F1 File Offset: 0x000019F1
		public ApplyOrderWithFormationAndPercentage(OrderType orderType, int formationIndex, int percentage)
		{
			this.OrderType = orderType;
			this.FormationIndex = formationIndex;
			this.Percentage = percentage;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000380E File Offset: 0x00001A0E
		public ApplyOrderWithFormationAndPercentage()
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003818 File Offset: 0x00001A18
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.Percentage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PercentageCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000385E File Offset: 0x00001A5E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Percentage, CompressionBasic.PercentageCompressionInfo);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00003890 File Offset: 0x00001A90
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00003898 File Offset: 0x00001A98
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to formation with index: ", this.FormationIndex, " and percentage: ", this.Percentage });
		}
	}
}
