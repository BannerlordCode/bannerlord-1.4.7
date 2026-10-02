using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D4 RID: 212
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PingReplication : GameNetworkMessage
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0000EB17 File Offset: 0x0000CD17
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x0000EB1F File Offset: 0x0000CD1F
		internal NetworkCommunicator Peer { get; private set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0000EB28 File Offset: 0x0000CD28
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0000EB30 File Offset: 0x0000CD30
		internal int PingValue { get; private set; }

		// Token: 0x060008B5 RID: 2229 RVA: 0x0000EB39 File Offset: 0x0000CD39
		public PingReplication()
		{
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0000EB41 File Offset: 0x0000CD41
		internal PingReplication(NetworkCommunicator peer, int ping)
		{
			this.Peer = peer;
			this.PingValue = ping;
			if (this.PingValue > 1023)
			{
				this.PingValue = 1023;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000EB70 File Offset: 0x0000CD70
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			this.PingValue = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PingValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.PingValue, CompressionBasic.PingValueCompressionInfo);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000EBBD File Offset: 0x0000CDBD
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0000EBC5 File Offset: 0x0000CDC5
		protected override string OnGetLogFormat()
		{
			return "PingReplication";
		}

		// Token: 0x040001FB RID: 507
		public const int MaxPingToReplicate = 1023;
	}
}
