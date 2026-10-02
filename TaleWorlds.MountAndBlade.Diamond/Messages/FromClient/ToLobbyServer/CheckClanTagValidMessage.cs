using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000084 RID: 132
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CheckClanTagValidMessage : Message
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00003BD1 File Offset: 0x00001DD1
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00003BD9 File Offset: 0x00001DD9
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x06000283 RID: 643 RVA: 0x00003BE2 File Offset: 0x00001DE2
		public CheckClanTagValidMessage()
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00003BEA File Offset: 0x00001DEA
		public CheckClanTagValidMessage(string clanTag)
		{
			this.ClanTag = clanTag;
		}
	}
}
