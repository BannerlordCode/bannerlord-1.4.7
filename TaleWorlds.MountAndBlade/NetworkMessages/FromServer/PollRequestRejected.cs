using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000064 RID: 100
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PollRequestRejected : GameNetworkMessage
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00006C71 File Offset: 0x00004E71
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00006C79 File Offset: 0x00004E79
		public int Reason { get; private set; }

		// Token: 0x06000377 RID: 887 RVA: 0x00006C82 File Offset: 0x00004E82
		public PollRequestRejected(int reason)
		{
			this.Reason = reason;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00006C91 File Offset: 0x00004E91
		public PollRequestRejected()
		{
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00006C9C File Offset: 0x00004E9C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Reason = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerPollRejectReasonCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00006CBE File Offset: 0x00004EBE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Reason, CompressionMission.MultiplayerPollRejectReasonCompressionInfo);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00006CD0 File Offset: 0x00004ED0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00006CD8 File Offset: 0x00004ED8
		protected override string OnGetLogFormat()
		{
			return "Poll request rejected (" + (MultiplayerPollRejectReason)this.Reason + ")";
		}
	}
}
