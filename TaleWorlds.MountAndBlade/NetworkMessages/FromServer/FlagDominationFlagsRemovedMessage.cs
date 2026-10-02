using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000042 RID: 66
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagDominationFlagsRemovedMessage : GameNetworkMessage
	{
		// Token: 0x0600021F RID: 543 RVA: 0x00004919 File Offset: 0x00002B19
		protected override void OnWrite()
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000491B File Offset: 0x00002B1B
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000491E File Offset: 0x00002B1E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00004926 File Offset: 0x00002B26
		protected override string OnGetLogFormat()
		{
			return "Flags got removed.";
		}
	}
}
