using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000066 RID: 102
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizeMissionTimeTracker : GameNetworkMessage
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00006DD8 File Offset: 0x00004FD8
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public float CurrentTime { get; private set; }

		// Token: 0x06000389 RID: 905 RVA: 0x00006DE9 File Offset: 0x00004FE9
		public SynchronizeMissionTimeTracker(float currentTime)
		{
			this.CurrentTime = currentTime;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00006DF8 File Offset: 0x00004FF8
		public SynchronizeMissionTimeTracker()
		{
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00006E00 File Offset: 0x00005000
		protected override bool OnRead()
		{
			bool flag = true;
			this.CurrentTime = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00006E22 File Offset: 0x00005022
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteFloatToPacket(this.CurrentTime, CompressionMatchmaker.MissionTimeCompressionInfo);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00006E34 File Offset: 0x00005034
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00006E3C File Offset: 0x0000503C
		protected override string OnGetLogFormat()
		{
			return this.CurrentTime + " seconds have elapsed since the start of the mission.";
		}
	}
}
