using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008F RID: 143
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class InitializeFormation : GameNetworkMessage
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0000A5B3 File Offset: 0x000087B3
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0000A5BB File Offset: 0x000087BB
		public int FormationIndex { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0000A5C4 File Offset: 0x000087C4
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0000A5CC File Offset: 0x000087CC
		public int TeamIndex { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0000A5D5 File Offset: 0x000087D5
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x0000A5DD File Offset: 0x000087DD
		public string BannerCode { get; private set; }

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000A5E6 File Offset: 0x000087E6
		public InitializeFormation(Formation formation, int teamIndex, string bannerCode)
		{
			this.FormationIndex = (int)formation.FormationIndex;
			this.TeamIndex = teamIndex;
			this.BannerCode = ((!string.IsNullOrEmpty(bannerCode)) ? bannerCode : string.Empty);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000A617 File Offset: 0x00008817
		public InitializeFormation()
		{
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0000A620 File Offset: 0x00008820
		protected override bool OnRead()
		{
			bool flag = true;
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000A65C File Offset: 0x0000885C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000A684 File Offset: 0x00008884
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000A688 File Offset: 0x00008888
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Initialize formation with index: ", this.FormationIndex, ", for team: ", this.TeamIndex });
		}
	}
}
