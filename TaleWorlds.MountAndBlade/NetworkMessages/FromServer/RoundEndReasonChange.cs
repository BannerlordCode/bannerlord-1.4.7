using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006C RID: 108
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundEndReasonChange : GameNetworkMessage
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x000073FB File Offset: 0x000055FB
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00007403 File Offset: 0x00005603
		public RoundEndReason RoundEndReason { get; private set; }

		// Token: 0x060003C2 RID: 962 RVA: 0x0000740C File Offset: 0x0000560C
		public RoundEndReasonChange()
		{
			this.RoundEndReason = RoundEndReason.Invalid;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000741B File Offset: 0x0000561B
		public RoundEndReasonChange(RoundEndReason roundEndReason)
		{
			this.RoundEndReason = roundEndReason;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000742A File Offset: 0x0000562A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.RoundEndReason, CompressionMission.RoundEndReasonCompressionInfo);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000743C File Offset: 0x0000563C
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundEndReason = (RoundEndReason)GameNetworkMessage.ReadIntFromPacket(CompressionMission.RoundEndReasonCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000745E File Offset: 0x0000565E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00007468 File Offset: 0x00005668
		protected override string OnGetLogFormat()
		{
			return "Change round end reason to: " + this.RoundEndReason.ToString();
		}
	}
}
