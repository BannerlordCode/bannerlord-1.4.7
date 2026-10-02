using System;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AA RID: 170
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetServerStatusMessage : Message
	{
	}
}
