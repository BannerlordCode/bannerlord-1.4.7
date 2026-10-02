using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CC RID: 204
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class TeamSetIsEnemyOf : GameNetworkMessage
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0000E4AF File Offset: 0x0000C6AF
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0000E4B7 File Offset: 0x0000C6B7
		public int Team1Index { get; private set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0000E4C8 File Offset: 0x0000C6C8
		public int Team2Index { get; private set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0000E4D1 File Offset: 0x0000C6D1
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0000E4D9 File Offset: 0x0000C6D9
		public bool IsEnemyOf { get; private set; }

		// Token: 0x06000867 RID: 2151 RVA: 0x0000E4E2 File Offset: 0x0000C6E2
		public TeamSetIsEnemyOf(int team1Index, int team2Index, bool isEnemyOf)
		{
			this.Team1Index = team1Index;
			this.Team2Index = team2Index;
			this.IsEnemyOf = isEnemyOf;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0000E4FF File Offset: 0x0000C6FF
		public TeamSetIsEnemyOf()
		{
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000E507 File Offset: 0x0000C707
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteTeamIndexToPacket(this.Team1Index);
			GameNetworkMessage.WriteTeamIndexToPacket(this.Team2Index);
			GameNetworkMessage.WriteBoolToPacket(this.IsEnemyOf);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000E52C File Offset: 0x0000C72C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Team1Index = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.Team2Index = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.IsEnemyOf = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000E563 File Offset: 0x0000C763
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0000E56C File Offset: 0x0000C76C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				this.Team1Index,
				" is now ",
				this.IsEnemyOf ? "" : "not an ",
				"enemy of ",
				this.Team2Index
			});
		}
	}
}
