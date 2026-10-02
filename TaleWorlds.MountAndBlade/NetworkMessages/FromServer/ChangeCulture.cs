using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000051 RID: 81
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ChangeCulture : GameNetworkMessage
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002BD RID: 701 RVA: 0x000057CA File Offset: 0x000039CA
		// (set) Token: 0x060002BE RID: 702 RVA: 0x000057D2 File Offset: 0x000039D2
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002BF RID: 703 RVA: 0x000057DB File Offset: 0x000039DB
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x000057E3 File Offset: 0x000039E3
		public BasicCultureObject Culture { get; private set; }

		// Token: 0x060002C1 RID: 705 RVA: 0x000057EC File Offset: 0x000039EC
		public ChangeCulture()
		{
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000057F4 File Offset: 0x000039F4
		public ChangeCulture(MissionPeer peer, BasicCultureObject culture)
		{
			this.Peer = peer.GetNetworkPeer();
			this.Culture = culture;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000580F File Offset: 0x00003A0F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Culture, CompressionBasic.GUIDCompressionInfo);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000582C File Offset: 0x00003A2C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Culture = (BasicCultureObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00005866 File Offset: 0x00003A66
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000586E File Offset: 0x00003A6E
		protected override string OnGetLogFormat()
		{
			return "Requested culture: " + this.Culture.Name;
		}
	}
}
