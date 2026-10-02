using System;

namespace TaleWorlds.MountAndBlade.Network.Messages
{
	// Token: 0x020003BD RID: 957
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreatePlayer : GameNetworkMessage
	{
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003590 RID: 13712 RVA: 0x000DD1F1 File Offset: 0x000DB3F1
		// (set) Token: 0x06003591 RID: 13713 RVA: 0x000DD1F9 File Offset: 0x000DB3F9
		public int PlayerIndex { get; private set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003592 RID: 13714 RVA: 0x000DD202 File Offset: 0x000DB402
		// (set) Token: 0x06003593 RID: 13715 RVA: 0x000DD20A File Offset: 0x000DB40A
		public string PlayerName { get; private set; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003594 RID: 13716 RVA: 0x000DD213 File Offset: 0x000DB413
		// (set) Token: 0x06003595 RID: 13717 RVA: 0x000DD21B File Offset: 0x000DB41B
		public int DisconnectedPeerIndex { get; private set; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003596 RID: 13718 RVA: 0x000DD224 File Offset: 0x000DB424
		// (set) Token: 0x06003597 RID: 13719 RVA: 0x000DD22C File Offset: 0x000DB42C
		public bool IsNonExistingDisconnectedPeer { get; private set; }

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x000DD235 File Offset: 0x000DB435
		// (set) Token: 0x06003599 RID: 13721 RVA: 0x000DD23D File Offset: 0x000DB43D
		public bool IsReceiverPeer { get; private set; }

		// Token: 0x0600359A RID: 13722 RVA: 0x000DD246 File Offset: 0x000DB446
		public CreatePlayer(int playerIndex, string playerName, int disconnectedPeerIndex, bool isNonExistingDisconnectedPeer = false, bool isReceiverPeer = false)
		{
			this.PlayerIndex = playerIndex;
			this.PlayerName = playerName;
			this.DisconnectedPeerIndex = disconnectedPeerIndex;
			this.IsNonExistingDisconnectedPeer = isNonExistingDisconnectedPeer;
			this.IsReceiverPeer = isReceiverPeer;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000DD273 File Offset: 0x000DB473
		public CreatePlayer()
		{
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000DD27C File Offset: 0x000DB47C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.PlayerIndex, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteStringToPacket(this.PlayerName);
			GameNetworkMessage.WriteIntToPacket(this.DisconnectedPeerIndex, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsNonExistingDisconnectedPeer);
			GameNetworkMessage.WriteBoolToPacket(this.IsReceiverPeer);
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000DD2CC File Offset: 0x000DB4CC
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.PlayerName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.DisconnectedPeerIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.IsNonExistingDisconnectedPeer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsReceiverPeer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000DD327 File Offset: 0x000DB527
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000DD32C File Offset: 0x000DB52C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create a new player with name: ",
				this.PlayerName,
				" and index: ",
				this.PlayerIndex,
				" and dcedIndex: ",
				this.DisconnectedPeerIndex,
				" which is ",
				(!this.IsNonExistingDisconnectedPeer) ? "not" : "",
				" a NonExistingDisconnectedPeer"
			});
		}
	}
}
