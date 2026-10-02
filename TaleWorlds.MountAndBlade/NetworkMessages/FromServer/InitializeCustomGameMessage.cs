using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008E RID: 142
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class InitializeCustomGameMessage : GameNetworkMessage
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000A4B4 File Offset: 0x000086B4
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x0000A4BC File Offset: 0x000086BC
		public bool InMission { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0000A4C5 File Offset: 0x000086C5
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0000A4CD File Offset: 0x000086CD
		public string GameType { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0000A4D6 File Offset: 0x000086D6
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x0000A4DE File Offset: 0x000086DE
		public string Map { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0000A4E7 File Offset: 0x000086E7
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x0000A4EF File Offset: 0x000086EF
		public int BattleIndex { get; private set; }

		// Token: 0x0600059C RID: 1436 RVA: 0x0000A4F8 File Offset: 0x000086F8
		public InitializeCustomGameMessage(bool inMission, string gameType, string map, int battleIndex)
		{
			this.InMission = inMission;
			this.GameType = gameType;
			this.Map = map;
			this.BattleIndex = battleIndex;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0000A51D File Offset: 0x0000871D
		public InitializeCustomGameMessage()
		{
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0000A528 File Offset: 0x00008728
		protected override bool OnRead()
		{
			bool flag = true;
			this.InMission = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.BattleIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AutomatedBattleIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0000A571 File Offset: 0x00008771
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.InMission);
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
			GameNetworkMessage.WriteIntToPacket(this.BattleIndex, CompressionMission.AutomatedBattleIndexCompressionInfo);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000A5A4 File Offset: 0x000087A4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0000A5AC File Offset: 0x000087AC
		protected override string OnGetLogFormat()
		{
			return "Initialize Custom Game";
		}
	}
}
