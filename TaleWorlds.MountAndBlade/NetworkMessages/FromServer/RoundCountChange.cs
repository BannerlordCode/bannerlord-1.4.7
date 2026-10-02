using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006B RID: 107
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundCountChange : GameNetworkMessage
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000737E File Offset: 0x0000557E
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x00007386 File Offset: 0x00005586
		public int RoundCount { get; private set; }

		// Token: 0x060003BA RID: 954 RVA: 0x0000738F File Offset: 0x0000558F
		public RoundCountChange(int roundCount)
		{
			this.RoundCount = roundCount;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000739E File Offset: 0x0000559E
		public RoundCountChange()
		{
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000073A8 File Offset: 0x000055A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissionRoundCountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000073CA File Offset: 0x000055CA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.RoundCount, CompressionMission.MissionRoundCountCompressionInfo);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000073DC File Offset: 0x000055DC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000073E4 File Offset: 0x000055E4
		protected override string OnGetLogFormat()
		{
			return "Change round count to: " + this.RoundCount;
		}
	}
}
