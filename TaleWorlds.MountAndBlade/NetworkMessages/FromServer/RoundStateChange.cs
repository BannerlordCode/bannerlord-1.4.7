using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006D RID: 109
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundStateChange : GameNetworkMessage
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00007493 File Offset: 0x00005693
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000749B File Offset: 0x0000569B
		public MultiplayerRoundState RoundState { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003CA RID: 970 RVA: 0x000074A4 File Offset: 0x000056A4
		// (set) Token: 0x060003CB RID: 971 RVA: 0x000074AC File Offset: 0x000056AC
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000074B5 File Offset: 0x000056B5
		// (set) Token: 0x060003CD RID: 973 RVA: 0x000074BD File Offset: 0x000056BD
		public int RemainingTimeOnPreviousState { get; private set; }

		// Token: 0x060003CE RID: 974 RVA: 0x000074C6 File Offset: 0x000056C6
		public RoundStateChange(MultiplayerRoundState roundState, long stateStartTimeInTicks, int remainingTimeOnPreviousState)
		{
			this.RoundState = roundState;
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
			this.RemainingTimeOnPreviousState = remainingTimeOnPreviousState;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000074EA File Offset: 0x000056EA
		public RoundStateChange()
		{
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000074F4 File Offset: 0x000056F4
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundState = (MultiplayerRoundState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissionRoundStateCompressionInfo, ref flag);
			this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			this.RemainingTimeOnPreviousState = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RoundTimeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000753A File Offset: 0x0000573A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.RoundState, CompressionMission.MissionRoundStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.RemainingTimeOnPreviousState, CompressionMission.RoundTimeCompressionInfo);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000756C File Offset: 0x0000576C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00007574 File Offset: 0x00005774
		protected override string OnGetLogFormat()
		{
			return "Changing round state to: " + this.RoundState;
		}
	}
}
