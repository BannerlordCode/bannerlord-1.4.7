using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CE RID: 206
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UpdateRoundScores : GameNetworkMessage
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0000E629 File Offset: 0x0000C829
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0000E631 File Offset: 0x0000C831
		public int AttackerTeamScore { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0000E63A File Offset: 0x0000C83A
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x0000E642 File Offset: 0x0000C842
		public int DefenderTeamScore { get; private set; }

		// Token: 0x06000879 RID: 2169 RVA: 0x0000E64B File Offset: 0x0000C84B
		public UpdateRoundScores(int attackerTeamScore, int defenderTeamScore)
		{
			this.AttackerTeamScore = attackerTeamScore;
			this.DefenderTeamScore = defenderTeamScore;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0000E661 File Offset: 0x0000C861
		public UpdateRoundScores()
		{
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0000E66C File Offset: 0x0000C86C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AttackerTeamScore = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamScoreCompressionInfo, ref flag);
			this.DefenderTeamScore = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamScoreCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.AttackerTeamScore, CompressionMission.TeamScoreCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DefenderTeamScore, CompressionMission.TeamScoreCompressionInfo);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0000E6C2 File Offset: 0x0000C8C2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission | MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0000E6CA File Offset: 0x0000C8CA
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Update round score. Attackers: ", this.AttackerTeamScore, ", defenders: ", this.DefenderTeamScore });
		}
	}
}
