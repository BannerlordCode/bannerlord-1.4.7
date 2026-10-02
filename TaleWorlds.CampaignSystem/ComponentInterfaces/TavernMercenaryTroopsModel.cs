using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EC RID: 492
	public abstract class TavernMercenaryTroopsModel : MBGameModel<TavernMercenaryTroopsModel>
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06001F1F RID: 7967
		public abstract float RegularMercenariesSpawnChance { get; }
	}
}
