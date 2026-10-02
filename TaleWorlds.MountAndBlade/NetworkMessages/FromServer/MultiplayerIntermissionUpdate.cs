using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005D RID: 93
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionUpdate : GameNetworkMessage
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00006460 File Offset: 0x00004660
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00006468 File Offset: 0x00004668
		public MultiplayerIntermissionState IntermissionState { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00006471 File Offset: 0x00004671
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00006479 File Offset: 0x00004679
		public float IntermissionTimer { get; private set; }

		// Token: 0x06000347 RID: 839 RVA: 0x00006482 File Offset: 0x00004682
		public MultiplayerIntermissionUpdate()
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000648A File Offset: 0x0000468A
		public MultiplayerIntermissionUpdate(MultiplayerIntermissionState intermissionState, float intermissionTimer)
		{
			this.IntermissionState = intermissionState;
			this.IntermissionTimer = intermissionTimer;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000064A0 File Offset: 0x000046A0
		protected override bool OnRead()
		{
			bool flag = true;
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionStateCompressionInfo, ref flag);
			this.IntermissionState = (MultiplayerIntermissionState)num;
			this.IntermissionTimer = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.IntermissionTimerCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000064D6 File Offset: 0x000046D6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.IntermissionState, CompressionBasic.IntermissionStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.IntermissionTimer, CompressionBasic.IntermissionTimerCompressionInfo);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000064F8 File Offset: 0x000046F8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00006500 File Offset: 0x00004700
		protected override string OnGetLogFormat()
		{
			return "Receiving runtime intermission state.";
		}
	}
}
