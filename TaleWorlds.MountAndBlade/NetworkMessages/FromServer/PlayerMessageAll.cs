using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200003E RID: 62
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PlayerMessageAll : GameNetworkMessage
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000462F File Offset: 0x0000282F
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00004637 File Offset: 0x00002837
		public NetworkCommunicator Player { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00004640 File Offset: 0x00002840
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00004648 File Offset: 0x00002848
		public string Message { get; private set; }

		// Token: 0x060001F8 RID: 504 RVA: 0x00004651 File Offset: 0x00002851
		public PlayerMessageAll(NetworkCommunicator player, string message)
		{
			this.Player = player;
			this.Message = message;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00004667 File Offset: 0x00002867
		public PlayerMessageAll()
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000466F File Offset: 0x0000286F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Player);
			GameNetworkMessage.WriteStringToPacket(this.Message);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00004688 File Offset: 0x00002888
		protected override bool OnRead()
		{
			bool flag = true;
			this.Player = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000046B3 File Offset: 0x000028B3
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000046B7 File Offset: 0x000028B7
		protected override string OnGetLogFormat()
		{
			return "Receiving Player message to all: " + this.Message;
		}
	}
}
