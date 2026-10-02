using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006A RID: 106
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UpdateIntermissionVotingManagerValues : GameNetworkMessage
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00007282 File Offset: 0x00005482
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000728A File Offset: 0x0000548A
		public bool IsAutomatedBattleSwitchingEnabled { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00007293 File Offset: 0x00005493
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000729B File Offset: 0x0000549B
		public bool IsMapVoteEnabled { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x000072A4 File Offset: 0x000054A4
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x000072AC File Offset: 0x000054AC
		public bool IsCultureVoteEnabled { get; private set; }

		// Token: 0x060003B3 RID: 947 RVA: 0x000072B5 File Offset: 0x000054B5
		public UpdateIntermissionVotingManagerValues()
		{
			this.IsAutomatedBattleSwitchingEnabled = MultiplayerIntermissionVotingManager.Instance.IsAutomatedBattleSwitchingEnabled;
			this.IsMapVoteEnabled = MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled;
			this.IsCultureVoteEnabled = MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000072ED File Offset: 0x000054ED
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000072F5 File Offset: 0x000054F5
		protected override string OnGetLogFormat()
		{
			return string.Format("IsAutomatedBattleSwitchingEnabled: {0}, IsMapVoteEnabled: {1}, IsCultureVoteEnabled: {2}", this.IsAutomatedBattleSwitchingEnabled, this.IsMapVoteEnabled, this.IsCultureVoteEnabled);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00007324 File Offset: 0x00005524
		protected override bool OnRead()
		{
			bool flag = true;
			this.IsAutomatedBattleSwitchingEnabled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsMapVoteEnabled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsCultureVoteEnabled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000735B File Offset: 0x0000555B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.IsAutomatedBattleSwitchingEnabled);
			GameNetworkMessage.WriteBoolToPacket(this.IsMapVoteEnabled);
			GameNetworkMessage.WriteBoolToPacket(this.IsCultureVoteEnabled);
		}
	}
}
