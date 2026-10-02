using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.BattleServerManager.BattleServerManager
{
	// Token: 0x020000E8 RID: 232
	[MessageDescription("BattleServerManager", "BattleServerManager", true)]
	[Serializable]
	public class BattleEndedProcessResultsMessage : Message
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00005088 File Offset: 0x00003288
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x00005090 File Offset: 0x00003290
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00005099 File Offset: 0x00003299
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x000050A1 File Offset: 0x000032A1
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDateEntries { get; private set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000050AA File Offset: 0x000032AA
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x000050B2 File Offset: 0x000032B2
		[JsonProperty]
		public string BattleGameType { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000050BB File Offset: 0x000032BB
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x000050C3 File Offset: 0x000032C3
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000050CC File Offset: 0x000032CC
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x000050D4 File Offset: 0x000032D4
		[TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })]
		[JsonProperty]
		public List<ValueTuple<PlayerBattleInfo, bool, bool>> PlayersForResults
		{
			[return: TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })]
			get;
			[param: TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })]
			private set;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000050DD File Offset: 0x000032DD
		public BattleEndedProcessResultsMessage()
		{
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000050E5 File Offset: 0x000032E5
		public BattleEndedProcessResultsMessage(BattleResult battleResult, List<BadgeDataEntry> badgeDateEntries, string battleGameType, string region, [TupleElementNames(new string[] { "playerBattleInfo", "shouldSendMessage", "hasLeftGame" })] List<ValueTuple<PlayerBattleInfo, bool, bool>> playersForResults)
		{
			this.BattleResult = battleResult;
			this.BadgeDateEntries = badgeDateEntries;
			this.BattleGameType = battleGameType;
			this.Region = region;
			this.PlayersForResults = playersForResults;
		}
	}
}
