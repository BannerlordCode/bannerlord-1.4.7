using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000DA RID: 218
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SendVoiceToPlay : GameNetworkMessage
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0000EFE7 File Offset: 0x0000D1E7
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0000EFEF File Offset: 0x0000D1EF
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x0000F000 File Offset: 0x0000D200
		public byte[] Buffer { get; private set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0000F009 File Offset: 0x0000D209
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x0000F011 File Offset: 0x0000D211
		public int BufferLength { get; private set; }

		// Token: 0x060008EB RID: 2283 RVA: 0x0000F01A File Offset: 0x0000D21A
		public SendVoiceToPlay()
		{
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000F022 File Offset: 0x0000D222
		public SendVoiceToPlay(NetworkCommunicator peer, byte[] buffer, int bufferLength)
		{
			this.Peer = peer;
			this.Buffer = buffer;
			this.BufferLength = bufferLength;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000F03F File Offset: 0x0000D23F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteByteArrayToPacket(this.Buffer, 0, this.BufferLength);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0000F060 File Offset: 0x0000D260
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Buffer = new byte[1440];
			this.BufferLength = GameNetworkMessage.ReadByteArrayFromPacket(this.Buffer, 0, 1440, ref flag);
			return flag;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0000F0A7 File Offset: 0x0000D2A7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0000F0AB File Offset: 0x0000D2AB
		protected override string OnGetLogFormat()
		{
			return string.Empty;
		}
	}
}
