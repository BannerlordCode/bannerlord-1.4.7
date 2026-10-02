using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D3 RID: 211
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class LossReplicationMessage : GameNetworkMessage
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0000EAAB File Offset: 0x0000CCAB
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x0000EAB3 File Offset: 0x0000CCB3
		internal int LossValue { get; private set; }

		// Token: 0x060008AB RID: 2219 RVA: 0x0000EABC File Offset: 0x0000CCBC
		public LossReplicationMessage()
		{
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
		internal LossReplicationMessage(int lossValue)
		{
			this.LossValue = lossValue;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
		protected override bool OnRead()
		{
			bool flag = true;
			this.LossValue = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.LossValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000EAF6 File Offset: 0x0000CCF6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.LossValue, CompressionBasic.LossValueCompressionInfo);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000EB08 File Offset: 0x0000CD08
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000EB10 File Offset: 0x0000CD10
		protected override string OnGetLogFormat()
		{
			return "LossReplicationMessage";
		}
	}
}
