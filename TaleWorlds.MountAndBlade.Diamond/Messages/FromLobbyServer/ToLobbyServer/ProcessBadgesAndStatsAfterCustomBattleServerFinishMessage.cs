using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToLobbyServer
{
	// Token: 0x0200006F RID: 111
	[MessageDescription("CustomBattleServerManager", "CustomBattleServerManager", true)]
	[Serializable]
	public class ProcessBadgesAndStatsAfterCustomBattleServerFinishMessage : Message
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000235 RID: 565 RVA: 0x000038D1 File Offset: 0x00001AD1
		// (set) Token: 0x06000236 RID: 566 RVA: 0x000038D9 File Offset: 0x00001AD9
		[JsonProperty]
		public List<BadgeDataEntry> BadgeDataEntries { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000038E2 File Offset: 0x00001AE2
		// (set) Token: 0x06000238 RID: 568 RVA: 0x000038EA File Offset: 0x00001AEA
		[JsonProperty]
		public PlayerId[] PlayerIds { get; private set; }

		// Token: 0x06000239 RID: 569 RVA: 0x000038F3 File Offset: 0x00001AF3
		public ProcessBadgesAndStatsAfterCustomBattleServerFinishMessage()
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x000038FB File Offset: 0x00001AFB
		public ProcessBadgesAndStatsAfterCustomBattleServerFinishMessage(List<BadgeDataEntry> badgeDataEntries, PlayerId[] playerIds)
		{
			this.BadgeDataEntries = badgeDataEntries;
			this.PlayerIds = playerIds;
		}
	}
}
