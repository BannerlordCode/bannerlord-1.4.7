using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000032 RID: 50
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectAllFormations : GameNetworkMessage
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00003F3B File Offset: 0x0000213B
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00003F3E File Offset: 0x0000213E
		protected override void OnWrite()
		{
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00003F40 File Offset: 0x00002140
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00003F45 File Offset: 0x00002145
		protected override string OnGetLogFormat()
		{
			return "Select all formations";
		}
	}
}
