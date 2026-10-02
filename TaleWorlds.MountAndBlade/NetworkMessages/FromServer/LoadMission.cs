using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000090 RID: 144
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class LoadMission : GameNetworkMessage
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000A6C1 File Offset: 0x000088C1
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x0000A6C9 File Offset: 0x000088C9
		public string GameType { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0000A6D2 File Offset: 0x000088D2
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0000A6DA File Offset: 0x000088DA
		public string Map { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000A6E3 File Offset: 0x000088E3
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0000A6EB File Offset: 0x000088EB
		public int BattleIndex { get; private set; }

		// Token: 0x060005B4 RID: 1460 RVA: 0x0000A6F4 File Offset: 0x000088F4
		public LoadMission(string gameType, string map, int battleIndex)
		{
			this.GameType = gameType;
			this.Map = map;
			this.BattleIndex = battleIndex;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0000A711 File Offset: 0x00008911
		public LoadMission()
		{
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000A71C File Offset: 0x0000891C
		protected override bool OnRead()
		{
			bool flag = true;
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.BattleIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AutomatedBattleIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000A758 File Offset: 0x00008958
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
			GameNetworkMessage.WriteIntToPacket(this.BattleIndex, CompressionMission.AutomatedBattleIndexCompressionInfo);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000A780 File Offset: 0x00008980
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000A788 File Offset: 0x00008988
		protected override string OnGetLogFormat()
		{
			return "Load Mission";
		}
	}
}
