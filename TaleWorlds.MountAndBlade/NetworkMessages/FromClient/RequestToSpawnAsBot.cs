using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000030 RID: 48
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestToSpawnAsBot : GameNetworkMessage
	{
		// Token: 0x0600017D RID: 381 RVA: 0x00003E73 File Offset: 0x00002073
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00003E76 File Offset: 0x00002076
		protected override void OnWrite()
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00003E78 File Offset: 0x00002078
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00003E80 File Offset: 0x00002080
		protected override string OnGetLogFormat()
		{
			return "Request to spawn as a bot";
		}
	}
}
