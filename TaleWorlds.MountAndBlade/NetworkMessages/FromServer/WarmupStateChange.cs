using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D0 RID: 208
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class WarmupStateChange : GameNetworkMessage
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0000E824 File Offset: 0x0000CA24
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x0000E82C File Offset: 0x0000CA2C
		public MultiplayerWarmupComponent.WarmupStates WarmupState { get; private set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0000E835 File Offset: 0x0000CA35
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x0000E83D File Offset: 0x0000CA3D
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x0600088D RID: 2189 RVA: 0x0000E846 File Offset: 0x0000CA46
		public WarmupStateChange(MultiplayerWarmupComponent.WarmupStates warmupState, long stateStartTimeInTicks)
		{
			this.WarmupState = warmupState;
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000E863 File Offset: 0x0000CA63
		public WarmupStateChange()
		{
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0000E86B File Offset: 0x0000CA6B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.WarmupState, CompressionMission.MissionRoundStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0000E890 File Offset: 0x0000CA90
		protected override bool OnRead()
		{
			bool flag = true;
			this.WarmupState = (MultiplayerWarmupComponent.WarmupStates)GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissionRoundStateCompressionInfo, ref flag);
			this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0000E8C4 File Offset: 0x0000CAC4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0000E8CC File Offset: 0x0000CACC
		protected override string OnGetLogFormat()
		{
			return "Warmup state set to " + this.WarmupState;
		}
	}
}
