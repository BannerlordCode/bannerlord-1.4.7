using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005B RID: 91
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionMapItemAdded : GameNetworkMessage
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000632C File Offset: 0x0000452C
		// (set) Token: 0x06000332 RID: 818 RVA: 0x00006334 File Offset: 0x00004534
		public string MapId { get; private set; }

		// Token: 0x06000333 RID: 819 RVA: 0x0000633D File Offset: 0x0000453D
		public MultiplayerIntermissionMapItemAdded()
		{
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00006345 File Offset: 0x00004545
		public MultiplayerIntermissionMapItemAdded(string mapId)
		{
			this.MapId = mapId;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00006354 File Offset: 0x00004554
		protected override bool OnRead()
		{
			bool flag = true;
			this.MapId = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00006371 File Offset: 0x00004571
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.MapId);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000637E File Offset: 0x0000457E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00006386 File Offset: 0x00004586
		protected override string OnGetLogFormat()
		{
			return "Adding map for voting with id: " + this.MapId + ".";
		}
	}
}
