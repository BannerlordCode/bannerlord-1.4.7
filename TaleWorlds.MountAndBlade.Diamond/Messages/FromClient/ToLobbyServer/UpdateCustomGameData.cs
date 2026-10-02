using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C6 RID: 198
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateCustomGameData : Message
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000471E File Offset: 0x0000291E
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00004726 File Offset: 0x00002926
		[JsonProperty]
		public string NewGameType { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000472F File Offset: 0x0000292F
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00004737 File Offset: 0x00002937
		[JsonProperty]
		public string NewMap { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00004740 File Offset: 0x00002940
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00004748 File Offset: 0x00002948
		[JsonProperty]
		public int NewMaxNumberOfPlayers { get; private set; }

		// Token: 0x06000399 RID: 921 RVA: 0x00004751 File Offset: 0x00002951
		public UpdateCustomGameData()
		{
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00004759 File Offset: 0x00002959
		public UpdateCustomGameData(string newGameType, string newMap, int newMaxNumberOfPlayers)
		{
			this.NewGameType = newGameType;
			this.NewMap = newMap;
			this.NewMaxNumberOfPlayers = newMaxNumberOfPlayers;
		}
	}
}
