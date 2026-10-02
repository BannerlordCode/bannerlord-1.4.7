using System;

namespace TaleWorlds.MountAndBlade.Network.Messages
{
	// Token: 0x020003BE RID: 958
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DeletePlayer : GameNetworkMessage
	{
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060035A0 RID: 13728 RVA: 0x000DD3A9 File Offset: 0x000DB5A9
		// (set) Token: 0x060035A1 RID: 13729 RVA: 0x000DD3B1 File Offset: 0x000DB5B1
		public int PlayerIndex { get; private set; }

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060035A2 RID: 13730 RVA: 0x000DD3BA File Offset: 0x000DB5BA
		// (set) Token: 0x060035A3 RID: 13731 RVA: 0x000DD3C2 File Offset: 0x000DB5C2
		public bool AddToDisconnectList { get; private set; }

		// Token: 0x060035A4 RID: 13732 RVA: 0x000DD3CB File Offset: 0x000DB5CB
		public DeletePlayer(int playerIndex, bool addToDisconnectList)
		{
			this.PlayerIndex = playerIndex;
			this.AddToDisconnectList = addToDisconnectList;
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000DD3E1 File Offset: 0x000DB5E1
		public DeletePlayer()
		{
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000DD3E9 File Offset: 0x000DB5E9
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.PlayerIndex, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.AddToDisconnectList);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000DD408 File Offset: 0x000DB608
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.AddToDisconnectList = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000DD437 File Offset: 0x000DB637
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000DD43B File Offset: 0x000DB63B
		protected override string OnGetLogFormat()
		{
			return "Delete player with index" + this.PlayerIndex;
		}
	}
}
