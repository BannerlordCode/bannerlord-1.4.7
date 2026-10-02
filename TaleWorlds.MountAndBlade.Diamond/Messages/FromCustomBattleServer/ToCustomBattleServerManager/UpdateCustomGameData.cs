using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000D RID: 13
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class UpdateCustomGameData : Message
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002598 File Offset: 0x00000798
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000025A0 File Offset: 0x000007A0
		[JsonProperty]
		public string NewGameType { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000025A9 File Offset: 0x000007A9
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000025B1 File Offset: 0x000007B1
		[JsonProperty]
		public string NewMap { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000025BA File Offset: 0x000007BA
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000025C2 File Offset: 0x000007C2
		[JsonProperty]
		public int NewMaxNumberOfPlayers { get; private set; }

		// Token: 0x06000069 RID: 105 RVA: 0x000025CB File Offset: 0x000007CB
		public UpdateCustomGameData()
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000025D3 File Offset: 0x000007D3
		public UpdateCustomGameData(string newGameType, string newMap, int newMaxNumberOfPlayers)
		{
			this.NewGameType = newGameType;
			this.NewMap = newMap;
			this.NewMaxNumberOfPlayers = newMaxNumberOfPlayers;
		}
	}
}
