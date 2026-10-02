using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008A RID: 138
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ExistingObjectsEnd : GameNetworkMessage
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00009ED8 File Offset: 0x000080D8
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00009EDB File Offset: 0x000080DB
		protected override void OnWrite()
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00009EDD File Offset: 0x000080DD
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00009EE1 File Offset: 0x000080E1
		protected override string OnGetLogFormat()
		{
			return "Finished receiving existing objects";
		}
	}
}
