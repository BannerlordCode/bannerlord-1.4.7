using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002B RID: 43
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ClearSelectedFormations : GameNetworkMessage
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00003C8B File Offset: 0x00001E8B
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003C8E File Offset: 0x00001E8E
		protected override void OnWrite()
		{
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00003C90 File Offset: 0x00001E90
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00003C95 File Offset: 0x00001E95
		protected override string OnGetLogFormat()
		{
			return "Clear Selected Formations";
		}
	}
}
