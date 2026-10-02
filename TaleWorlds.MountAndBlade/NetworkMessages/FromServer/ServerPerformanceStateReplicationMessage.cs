using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D6 RID: 214
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ServerPerformanceStateReplicationMessage : GameNetworkMessage
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0000EBCC File Offset: 0x0000CDCC
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
		internal ServerPerformanceState ServerPerformanceProblemState { get; private set; }

		// Token: 0x060008BD RID: 2237 RVA: 0x0000EBDD File Offset: 0x0000CDDD
		public ServerPerformanceStateReplicationMessage()
		{
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000EBE5 File Offset: 0x0000CDE5
		internal ServerPerformanceStateReplicationMessage(ServerPerformanceState serverPerformanceProblemState)
		{
			this.ServerPerformanceProblemState = serverPerformanceProblemState;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000EBF4 File Offset: 0x0000CDF4
		protected override bool OnRead()
		{
			bool flag = true;
			this.ServerPerformanceProblemState = (ServerPerformanceState)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ServerPerformanceStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0000EC16 File Offset: 0x0000CE16
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.ServerPerformanceProblemState, CompressionBasic.ServerPerformanceStateCompressionInfo);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000EC28 File Offset: 0x0000CE28
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000EC30 File Offset: 0x0000CE30
		protected override string OnGetLogFormat()
		{
			return "ServerPerformanceStateReplicationMessage";
		}
	}
}
