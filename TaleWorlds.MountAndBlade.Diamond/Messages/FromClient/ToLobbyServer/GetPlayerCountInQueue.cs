using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A2 RID: 162
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerCountInQueue : Message
	{
	}
}
