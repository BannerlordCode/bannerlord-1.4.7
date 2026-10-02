using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002C RID: 44
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class CreateBanner : GameNetworkMessage
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00003C9C File Offset: 0x00001E9C
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00003CA4 File Offset: 0x00001EA4
		public string BannerCode { get; private set; }

		// Token: 0x0600015F RID: 351 RVA: 0x00003CAD File Offset: 0x00001EAD
		public CreateBanner(string bannerCode)
		{
			this.BannerCode = bannerCode;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00003CBC File Offset: 0x00001EBC
		public CreateBanner()
		{
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00003CC4 File Offset: 0x00001EC4
		protected override bool OnRead()
		{
			bool flag = true;
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003CE1 File Offset: 0x00001EE1
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003CEE File Offset: 0x00001EEE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00003CF6 File Offset: 0x00001EF6
		protected override string OnGetLogFormat()
		{
			return "Clients has updated his banner";
		}
	}
}
