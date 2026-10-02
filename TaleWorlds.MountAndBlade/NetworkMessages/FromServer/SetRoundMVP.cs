using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B4 RID: 180
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetRoundMVP : GameNetworkMessage
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x0000C9E1 File Offset: 0x0000ABE1
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x0000C9E9 File Offset: 0x0000ABE9
		public NetworkCommunicator MVPPeer { get; private set; }

		// Token: 0x0600073F RID: 1855 RVA: 0x0000C9F2 File Offset: 0x0000ABF2
		public SetRoundMVP(NetworkCommunicator mvpPeer, int mvpCount)
		{
			this.MVPPeer = mvpPeer;
			this.MVPCount = mvpCount;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0000CA08 File Offset: 0x0000AC08
		public SetRoundMVP()
		{
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0000CA10 File Offset: 0x0000AC10
		protected override bool OnRead()
		{
			bool flag = true;
			this.MVPPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.MVPCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RoundTotalCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0000CA40 File Offset: 0x0000AC40
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.MVPPeer);
			GameNetworkMessage.WriteIntToPacket(this.MVPCount, CompressionBasic.RoundTotalCompressionInfo);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0000CA5D File Offset: 0x0000AC5D
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission | MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0000CA65 File Offset: 0x0000AC65
		protected override string OnGetLogFormat()
		{
			return "MVP selected as: " + this.MVPPeer.UserName + ".";
		}

		// Token: 0x040001A0 RID: 416
		public int MVPCount;
	}
}
