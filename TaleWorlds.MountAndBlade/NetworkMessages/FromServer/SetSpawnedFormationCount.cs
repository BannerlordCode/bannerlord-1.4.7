using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B9 RID: 185
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSpawnedFormationCount : GameNetworkMessage
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0000CD6A File Offset: 0x0000AF6A
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x0000CD72 File Offset: 0x0000AF72
		public int NumOfFormationsTeamOne { get; private set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0000CD7B File Offset: 0x0000AF7B
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x0000CD83 File Offset: 0x0000AF83
		public int NumOfFormationsTeamTwo { get; private set; }

		// Token: 0x0600076F RID: 1903 RVA: 0x0000CD8C File Offset: 0x0000AF8C
		public SetSpawnedFormationCount(int numFormationsTeamOne, int numFormationsTeamTwo)
		{
			this.NumOfFormationsTeamOne = numFormationsTeamOne;
			this.NumOfFormationsTeamTwo = numFormationsTeamTwo;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000CDA2 File Offset: 0x0000AFA2
		public SetSpawnedFormationCount()
		{
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000CDAC File Offset: 0x0000AFAC
		protected override bool OnRead()
		{
			bool flag = true;
			this.NumOfFormationsTeamOne = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.NumOfFormationsTeamTwo = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000CDE0 File Offset: 0x0000AFE0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.NumOfFormationsTeamOne, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.NumOfFormationsTeamTwo, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000CE02 File Offset: 0x0000B002
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000CE06 File Offset: 0x0000B006
		protected override string OnGetLogFormat()
		{
			return "Syncing formation count";
		}
	}
}
