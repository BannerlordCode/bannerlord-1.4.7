using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002F RID: 47
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestStopUsingObject : GameNetworkMessage
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00003E57 File Offset: 0x00002057
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00003E5A File Offset: 0x0000205A
		protected override void OnWrite()
		{
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00003E5C File Offset: 0x0000205C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00003E64 File Offset: 0x00002064
		protected override string OnGetLogFormat()
		{
			return "Request to stop using UsableMissionObject";
		}
	}
}
