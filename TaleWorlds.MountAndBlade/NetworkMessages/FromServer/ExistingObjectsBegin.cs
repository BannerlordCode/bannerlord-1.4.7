using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000089 RID: 137
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ExistingObjectsBegin : GameNetworkMessage
	{
		// Token: 0x0600055E RID: 1374 RVA: 0x00009EC0 File Offset: 0x000080C0
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00009EC3 File Offset: 0x000080C3
		protected override void OnWrite()
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00009EC5 File Offset: 0x000080C5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00009EC9 File Offset: 0x000080C9
		protected override string OnGetLogFormat()
		{
			return "Started receiving existing objects";
		}
	}
}
