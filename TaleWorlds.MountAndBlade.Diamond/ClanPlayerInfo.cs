using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000108 RID: 264
	[Serializable]
	public class ClanPlayerInfo
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x000070FC File Offset: 0x000052FC
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00007104 File Offset: 0x00005304
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0000710D File Offset: 0x0000530D
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00007115 File Offset: 0x00005315
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0000711E File Offset: 0x0000531E
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00007126 File Offset: 0x00005326
		[JsonProperty]
		public AnotherPlayerState State { get; private set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0000712F File Offset: 0x0000532F
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00007137 File Offset: 0x00005337
		[JsonProperty]
		public string ActiveBadgeId { get; private set; }

		// Token: 0x060005A1 RID: 1441 RVA: 0x00007140 File Offset: 0x00005340
		public ClanPlayerInfo(PlayerId playerId, string playerName, AnotherPlayerState anotherPlayerState, string activeBadgeId)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.ActiveBadgeId = activeBadgeId;
			this.State = anotherPlayerState;
		}
	}
}
