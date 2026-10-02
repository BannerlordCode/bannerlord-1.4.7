using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000040 RID: 64
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ServerAdminMessage : GameNetworkMessage
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000047AE File Offset: 0x000029AE
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000047B6 File Offset: 0x000029B6
		public string Message { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000047BF File Offset: 0x000029BF
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000047C7 File Offset: 0x000029C7
		public bool IsAdminBroadcast { get; private set; }

		// Token: 0x0600020C RID: 524 RVA: 0x000047D0 File Offset: 0x000029D0
		public ServerAdminMessage(string message, bool isAdminBroadcast)
		{
			this.Message = message;
			this.IsAdminBroadcast = isAdminBroadcast;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000047E6 File Offset: 0x000029E6
		public ServerAdminMessage()
		{
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000047EE File Offset: 0x000029EE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.Message);
			GameNetworkMessage.WriteBoolToPacket(this.IsAdminBroadcast);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00004808 File Offset: 0x00002A08
		protected override bool OnRead()
		{
			bool flag = true;
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsAdminBroadcast = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00004832 File Offset: 0x00002A32
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00004836 File Offset: 0x00002A36
		protected override string OnGetLogFormat()
		{
			return "Admin message from server: " + this.Message;
		}
	}
}
