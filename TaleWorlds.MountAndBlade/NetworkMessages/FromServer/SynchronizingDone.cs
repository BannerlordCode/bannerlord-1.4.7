using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D9 RID: 217
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizingDone : GameNetworkMessage
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0000EEEB File Offset: 0x0000D0EB
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0000EEF3 File Offset: 0x0000D0F3
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x0000EF04 File Offset: 0x0000D104
		public bool Synchronized { get; private set; }

		// Token: 0x060008DF RID: 2271 RVA: 0x0000EF0D File Offset: 0x0000D10D
		public SynchronizingDone(NetworkCommunicator peer, bool synchronized)
		{
			this.Peer = peer;
			this.Synchronized = synchronized;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0000EF23 File Offset: 0x0000D123
		public SynchronizingDone()
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0000EF2C File Offset: 0x0000D12C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Synchronized = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0000EF57 File Offset: 0x0000D157
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteBoolToPacket(this.Synchronized);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0000EF6F File Offset: 0x0000D16F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000EF74 File Offset: 0x0000D174
		protected override string OnGetLogFormat()
		{
			string text = string.Concat(new object[]
			{
				"peer with name: ",
				this.Peer.UserName,
				", and index: ",
				this.Peer.Index
			});
			if (!this.Synchronized)
			{
				return "Synchronized: FALSE for " + text + " (Peer will not receive broadcasted messages)";
			}
			return "Synchronized: TRUE for " + text + " (received all initial data from the server and will now receive broadcasted messages)";
		}
	}
}
