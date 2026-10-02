using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000033 RID: 51
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectAllSiegeWeapons : GameNetworkMessage
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00003F54 File Offset: 0x00002154
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00003F57 File Offset: 0x00002157
		protected override void OnWrite()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00003F59 File Offset: 0x00002159
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00003F61 File Offset: 0x00002161
		protected override string OnGetLogFormat()
		{
			return "Select all siege weapons.";
		}
	}
}
