using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000058 RID: 88
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MissionStateChange : GameNetworkMessage
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00006129 File Offset: 0x00004329
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00006131 File Offset: 0x00004331
		public MissionLobbyComponent.MultiplayerGameState CurrentState { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000613A File Offset: 0x0000433A
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00006142 File Offset: 0x00004342
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x06000319 RID: 793 RVA: 0x0000614B File Offset: 0x0000434B
		public MissionStateChange(MissionLobbyComponent.MultiplayerGameState currentState, long stateStartTimeInTicks)
		{
			this.CurrentState = currentState;
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00006168 File Offset: 0x00004368
		public MissionStateChange()
		{
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00006170 File Offset: 0x00004370
		protected override bool OnRead()
		{
			bool flag = true;
			this.CurrentState = (MissionLobbyComponent.MultiplayerGameState)GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.MissionCurrentStateCompressionInfo, ref flag);
			if (this.CurrentState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000061AC File Offset: 0x000043AC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.CurrentState, CompressionMatchmaker.MissionCurrentStateCompressionInfo);
			if (this.CurrentState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000061D6 File Offset: 0x000043D6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000061DE File Offset: 0x000043DE
		protected override string OnGetLogFormat()
		{
			return "Mission State has changed to: " + this.CurrentState;
		}
	}
}
