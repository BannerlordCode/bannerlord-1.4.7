using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002E RID: 46
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class FinishedLoading : GameNetworkMessage
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00003DE7 File Offset: 0x00001FE7
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00003DEF File Offset: 0x00001FEF
		public int BattleIndex { get; private set; }

		// Token: 0x06000171 RID: 369 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public FinishedLoading()
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003E00 File Offset: 0x00002000
		public FinishedLoading(int battleIndex)
		{
			this.BattleIndex = battleIndex;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003E10 File Offset: 0x00002010
		protected override bool OnRead()
		{
			bool flag = true;
			this.BattleIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AutomatedBattleIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003E32 File Offset: 0x00002032
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.BattleIndex, CompressionMission.AutomatedBattleIndexCompressionInfo);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00003E44 File Offset: 0x00002044
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00003E48 File Offset: 0x00002048
		protected override string OnGetLogFormat()
		{
			return "Finished Loading";
		}
	}
}
