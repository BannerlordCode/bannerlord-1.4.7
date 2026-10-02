using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002A RID: 42
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class CancelCheering : GameNetworkMessage
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00003C73 File Offset: 0x00001E73
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00003C76 File Offset: 0x00001E76
		protected override void OnWrite()
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003C78 File Offset: 0x00001E78
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003C7C File Offset: 0x00001E7C
		protected override string OnGetLogFormat()
		{
			return "FromClient.CancelCheering";
		}
	}
}
