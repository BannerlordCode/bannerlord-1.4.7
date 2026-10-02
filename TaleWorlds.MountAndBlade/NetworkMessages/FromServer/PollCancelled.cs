using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000062 RID: 98
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PollCancelled : GameNetworkMessage
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00006BB7 File Offset: 0x00004DB7
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00006BBA File Offset: 0x00004DBA
		protected override void OnWrite()
		{
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00006BBC File Offset: 0x00004DBC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00006BC4 File Offset: 0x00004DC4
		protected override string OnGetLogFormat()
		{
			return "Poll cancelled.";
		}
	}
}
