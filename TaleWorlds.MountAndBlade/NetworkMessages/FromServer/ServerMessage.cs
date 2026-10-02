using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000041 RID: 65
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ServerMessage : GameNetworkMessage
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00004848 File Offset: 0x00002A48
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00004850 File Offset: 0x00002A50
		public string Message { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00004859 File Offset: 0x00002A59
		// (set) Token: 0x06000215 RID: 533 RVA: 0x00004861 File Offset: 0x00002A61
		public bool IsMessageTextId { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000486A File Offset: 0x00002A6A
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00004872 File Offset: 0x00002A72
		public bool IsAdminAnnouncement { get; private set; }

		// Token: 0x06000218 RID: 536 RVA: 0x0000487B File Offset: 0x00002A7B
		public ServerMessage(string message, bool isMessageTextId = false, bool isAdminAnnouncement = false)
		{
			this.Message = message;
			this.IsMessageTextId = isMessageTextId;
			this.IsAdminAnnouncement = isAdminAnnouncement;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00004898 File Offset: 0x00002A98
		public ServerMessage()
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000048A0 File Offset: 0x00002AA0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.Message);
			GameNetworkMessage.WriteBoolToPacket(this.IsMessageTextId);
			GameNetworkMessage.WriteBoolToPacket(this.IsAdminAnnouncement);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000048C4 File Offset: 0x00002AC4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsMessageTextId = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsAdminAnnouncement = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000048FB File Offset: 0x00002AFB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000048FF File Offset: 0x00002AFF
		protected override string OnGetLogFormat()
		{
			return "Message from server: " + this.Message;
		}
	}
}
