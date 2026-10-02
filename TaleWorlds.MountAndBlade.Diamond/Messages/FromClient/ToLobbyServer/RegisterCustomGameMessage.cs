using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B9 RID: 185
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RegisterCustomGameMessage : Message
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00004389 File Offset: 0x00002589
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00004391 File Offset: 0x00002591
		[JsonProperty]
		public string GameModule { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000439A File Offset: 0x0000259A
		// (set) Token: 0x06000343 RID: 835 RVA: 0x000043A2 File Offset: 0x000025A2
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000043AB File Offset: 0x000025AB
		// (set) Token: 0x06000345 RID: 837 RVA: 0x000043B3 File Offset: 0x000025B3
		[JsonProperty]
		public string ServerName { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000043BC File Offset: 0x000025BC
		// (set) Token: 0x06000347 RID: 839 RVA: 0x000043C4 File Offset: 0x000025C4
		[JsonProperty]
		public int MaxPlayerCount { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000348 RID: 840 RVA: 0x000043CD File Offset: 0x000025CD
		// (set) Token: 0x06000349 RID: 841 RVA: 0x000043D5 File Offset: 0x000025D5
		[JsonProperty]
		public string Map { get; private set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600034A RID: 842 RVA: 0x000043DE File Offset: 0x000025DE
		// (set) Token: 0x0600034B RID: 843 RVA: 0x000043E6 File Offset: 0x000025E6
		[JsonProperty]
		public string UniqueMapId { get; private set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600034C RID: 844 RVA: 0x000043EF File Offset: 0x000025EF
		// (set) Token: 0x0600034D RID: 845 RVA: 0x000043F7 File Offset: 0x000025F7
		[JsonProperty]
		public int Port { get; private set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00004400 File Offset: 0x00002600
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00004408 File Offset: 0x00002608
		[JsonProperty]
		public string GamePassword { get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00004411 File Offset: 0x00002611
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00004419 File Offset: 0x00002619
		[JsonProperty]
		public string AdminPassword { get; private set; }

		// Token: 0x06000352 RID: 850 RVA: 0x00004422 File Offset: 0x00002622
		public RegisterCustomGameMessage()
		{
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000442C File Offset: 0x0000262C
		public RegisterCustomGameMessage(string gameModule, string gameType, string serverName, int maxPlayerCount, string map, string uniqueMapId, string gamePassword, string adminPassword, int port)
		{
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.ServerName = serverName;
			this.MaxPlayerCount = maxPlayerCount;
			this.Map = map;
			this.UniqueMapId = uniqueMapId;
			this.GamePassword = gamePassword;
			this.AdminPassword = adminPassword;
			this.Port = port;
		}
	}
}
