using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000083 RID: 131
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CheckClanNameValidMessage : Message
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00003BA9 File Offset: 0x00001DA9
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00003BB1 File Offset: 0x00001DB1
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x0600027F RID: 639 RVA: 0x00003BBA File Offset: 0x00001DBA
		public CheckClanNameValidMessage()
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00003BC2 File Offset: 0x00001DC2
		public CheckClanNameValidMessage(string clanName)
		{
			this.ClanName = clanName;
		}
	}
}
