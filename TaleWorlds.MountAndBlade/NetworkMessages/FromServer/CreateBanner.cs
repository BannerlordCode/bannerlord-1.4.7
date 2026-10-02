using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000083 RID: 131
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateBanner : GameNetworkMessage
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000944C File Offset: 0x0000764C
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00009454 File Offset: 0x00007654
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0000945D File Offset: 0x0000765D
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00009465 File Offset: 0x00007665
		public string BannerCode { get; private set; }

		// Token: 0x06000503 RID: 1283 RVA: 0x0000946E File Offset: 0x0000766E
		public CreateBanner(NetworkCommunicator peer, string bannerCode)
		{
			this.Peer = peer;
			this.BannerCode = bannerCode;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00009484 File Offset: 0x00007684
		public CreateBanner()
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000948C File Offset: 0x0000768C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000094A4 File Offset: 0x000076A4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000094CF File Offset: 0x000076CF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000094D7 File Offset: 0x000076D7
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create banner for peer: ",
				this.Peer.UserName,
				", with index: ",
				this.Peer.Index
			});
		}
	}
}
