using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004E RID: 78
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BotData : GameNetworkMessage
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000297 RID: 663 RVA: 0x000053FB File Offset: 0x000035FB
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00005403 File Offset: 0x00003603
		public BattleSideEnum Side { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000540C File Offset: 0x0000360C
		// (set) Token: 0x0600029A RID: 666 RVA: 0x00005414 File Offset: 0x00003614
		public int KillCount { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000541D File Offset: 0x0000361D
		// (set) Token: 0x0600029C RID: 668 RVA: 0x00005425 File Offset: 0x00003625
		public int AssistCount { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000542E File Offset: 0x0000362E
		// (set) Token: 0x0600029E RID: 670 RVA: 0x00005436 File Offset: 0x00003636
		public int DeathCount { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000543F File Offset: 0x0000363F
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x00005447 File Offset: 0x00003647
		public int AliveBotCount { get; private set; }

		// Token: 0x060002A1 RID: 673 RVA: 0x00005450 File Offset: 0x00003650
		public BotData(BattleSideEnum side, int kill, int assist, int death, int alive)
		{
			this.Side = side;
			this.KillCount = kill;
			this.AssistCount = assist;
			this.DeathCount = death;
			this.AliveBotCount = alive;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000547D File Offset: 0x0000367D
		public BotData()
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00005488 File Offset: 0x00003688
		protected override bool OnRead()
		{
			bool flag = true;
			this.Side = (BattleSideEnum)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			this.KillCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.AssistCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.DeathCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.AliveBotCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000054F4 File Offset: 0x000036F4
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.Side, CompressionMission.TeamSideCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.KillCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AssistCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DeathCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AliveBotCount, CompressionMission.AgentCompressionInfo);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00005551 File Offset: 0x00003751
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00005558 File Offset: 0x00003758
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "BOTS for side: ", this.Side, ", Kill: ", this.KillCount, " Death: ", this.DeathCount, " Assist: ", this.AssistCount, ", Alive: ", this.AliveBotCount });
		}
	}
}
