using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015C RID: 348
	public class DefaultTavernMercenaryTroopsModel : TavernMercenaryTroopsModel
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x00089E78 File Offset: 0x00088078
		public override float RegularMercenariesSpawnChance
		{
			get
			{
				return 0.7f;
			}
		}
	}
}
