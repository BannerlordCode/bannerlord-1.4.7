using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005A RID: 90
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionCultureItemVoteCountChanged : GameNetworkMessage
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00006269 File Offset: 0x00004469
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00006271 File Offset: 0x00004471
		public int CultureItemIndex { get; private set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000627A File Offset: 0x0000447A
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00006282 File Offset: 0x00004482
		public int VoteCount { get; private set; }

		// Token: 0x0600032B RID: 811 RVA: 0x0000628B File Offset: 0x0000448B
		public MultiplayerIntermissionCultureItemVoteCountChanged()
		{
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00006293 File Offset: 0x00004493
		public MultiplayerIntermissionCultureItemVoteCountChanged(int cultureItemIndex, int voteCount)
		{
			this.CultureItemIndex = cultureItemIndex;
			this.VoteCount = voteCount;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000062AC File Offset: 0x000044AC
		protected override bool OnRead()
		{
			bool flag = true;
			this.CultureItemIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.CultureIndexCompressionInfo, ref flag);
			this.VoteCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionVoterCountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000062E0 File Offset: 0x000044E0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.CultureItemIndex, CompressionBasic.CultureIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.VoteCount, CompressionBasic.IntermissionVoterCountCompressionInfo);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00006302 File Offset: 0x00004502
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000630A File Offset: 0x0000450A
		protected override string OnGetLogFormat()
		{
			return string.Format("Vote count changed for culture with index: {0}, vote count: {1}.", this.CultureItemIndex, this.VoteCount);
		}
	}
}
