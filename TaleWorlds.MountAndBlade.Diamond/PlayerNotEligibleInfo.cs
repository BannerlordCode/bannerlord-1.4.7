using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000107 RID: 263
	[Serializable]
	public class PlayerNotEligibleInfo
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x000070C4 File Offset: 0x000052C4
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x000070CC File Offset: 0x000052CC
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x000070D5 File Offset: 0x000052D5
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x000070DD File Offset: 0x000052DD
		[JsonProperty]
		public PlayerNotEligibleError[] Errors { get; private set; }

		// Token: 0x06000598 RID: 1432 RVA: 0x000070E6 File Offset: 0x000052E6
		public PlayerNotEligibleInfo(PlayerId playerId, PlayerNotEligibleError[] errors)
		{
			this.PlayerId = playerId;
			this.Errors = errors;
		}
	}
}
