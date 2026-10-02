using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006E RID: 110
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundWinnerChange : GameNetworkMessage
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000758B File Offset: 0x0000578B
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00007593 File Offset: 0x00005793
		public BattleSideEnum RoundWinner { get; private set; }

		// Token: 0x060003D6 RID: 982 RVA: 0x0000759C File Offset: 0x0000579C
		public RoundWinnerChange(BattleSideEnum roundWinner)
		{
			this.RoundWinner = roundWinner;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000075AB File Offset: 0x000057AB
		public RoundWinnerChange()
		{
			this.RoundWinner = BattleSideEnum.None;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000075BC File Offset: 0x000057BC
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundWinner = (BattleSideEnum)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000075DE File Offset: 0x000057DE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.RoundWinner, CompressionMission.TeamSideCompressionInfo);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000075F0 File Offset: 0x000057F0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000075F8 File Offset: 0x000057F8
		protected override string OnGetLogFormat()
		{
			return "Change round winner to: " + this.RoundWinner.ToString();
		}
	}
}
