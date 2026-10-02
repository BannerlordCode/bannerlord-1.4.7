using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D0 RID: 208
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleEndedMessage : Message
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x000048EE File Offset: 0x00002AEE
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x000048F6 File Offset: 0x00002AF6
		[JsonProperty]
		public BattleResult BattleResult { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000048FF File Offset: 0x00002AFF
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00004907 File Offset: 0x00002B07
		[JsonProperty]
		public GameLog[] GameLogs { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00004910 File Offset: 0x00002B10
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00004918 File Offset: 0x00002B18
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDataEntries { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00004921 File Offset: 0x00002B21
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x00004929 File Offset: 0x00002B29
		[JsonProperty]
		public Dictionary<int, int> TeamScores { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00004932 File Offset: 0x00002B32
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000493A File Offset: 0x00002B3A
		[JsonProperty]
		public Dictionary<string, int> PlayerScores { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00004943 File Offset: 0x00002B43
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000494B File Offset: 0x00002B4B
		[JsonProperty]
		public int GameTime { get; set; }

		// Token: 0x060003CC RID: 972 RVA: 0x00004954 File Offset: 0x00002B54
		public BattleEndedMessage()
		{
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000495C File Offset: 0x00002B5C
		public BattleEndedMessage(BattleResult battleResult, GameLog[] gameLogs, Dictionary<ValueTuple<PlayerId, string, string>, int> badgeDataDictionary, int gameTime, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			this.BattleResult = battleResult;
			this.GameLogs = gameLogs;
			this.BadgeDataEntries = BadgeDataEntry.ToList(badgeDataDictionary);
			this.TeamScores = teamScores;
			this.PlayerScores = playerScores.ToDictionary<KeyValuePair<PlayerId, int>, string, int>((KeyValuePair<PlayerId, int> kvp) => kvp.Key.ToString(), (KeyValuePair<PlayerId, int> kvp) => kvp.Value);
			this.GameTime = gameTime;
		}
	}
}
