using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007B RID: 123
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CancelBattleRequestMessage : Message
	{
	}
}
