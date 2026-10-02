using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008C RID: 140
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FormationWipedMessage : GameNetworkMessage
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x00009F8E File Offset: 0x0000818E
		protected override void OnWrite()
		{
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00009F90 File Offset: 0x00008190
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00009F93 File Offset: 0x00008193
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00009F9B File Offset: 0x0000819B
		protected override string OnGetLogFormat()
		{
			return "FormationWipedMessage";
		}
	}
}
