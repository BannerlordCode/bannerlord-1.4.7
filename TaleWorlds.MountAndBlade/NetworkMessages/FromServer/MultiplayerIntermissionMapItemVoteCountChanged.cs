using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005C RID: 92
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionMapItemVoteCountChanged : GameNetworkMessage
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000639D File Offset: 0x0000459D
		// (set) Token: 0x0600033A RID: 826 RVA: 0x000063A5 File Offset: 0x000045A5
		public int MapItemIndex { get; private set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600033B RID: 827 RVA: 0x000063AE File Offset: 0x000045AE
		// (set) Token: 0x0600033C RID: 828 RVA: 0x000063B6 File Offset: 0x000045B6
		public int VoteCount { get; private set; }

		// Token: 0x0600033D RID: 829 RVA: 0x000063BF File Offset: 0x000045BF
		public MultiplayerIntermissionMapItemVoteCountChanged()
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000063C7 File Offset: 0x000045C7
		public MultiplayerIntermissionMapItemVoteCountChanged(int mapItemIndex, int voteCount)
		{
			this.MapItemIndex = mapItemIndex;
			this.VoteCount = voteCount;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000063E0 File Offset: 0x000045E0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MapItemIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionMapVoteItemCountCompressionInfo, ref flag);
			this.VoteCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionVoterCountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00006414 File Offset: 0x00004614
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MapItemIndex, CompressionBasic.IntermissionMapVoteItemCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.VoteCount, CompressionBasic.IntermissionVoterCountCompressionInfo);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00006436 File Offset: 0x00004636
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000643E File Offset: 0x0000463E
		protected override string OnGetLogFormat()
		{
			return string.Format("Vote count changed for map with index: {0}, vote count: {1}.", this.MapItemIndex, this.VoteCount);
		}
	}
}
