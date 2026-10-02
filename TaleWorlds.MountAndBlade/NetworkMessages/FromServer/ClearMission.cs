using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007E RID: 126
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ClearMission : GameNetworkMessage
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x00008659 File Offset: 0x00006859
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000865C File Offset: 0x0000685C
		protected override void OnWrite()
		{
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000865E File Offset: 0x0000685E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00008666 File Offset: 0x00006866
		protected override string OnGetLogFormat()
		{
			return "Clear Mission";
		}
	}
}
