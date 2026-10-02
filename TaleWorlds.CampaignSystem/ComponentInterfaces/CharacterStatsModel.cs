using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000185 RID: 389
	public abstract class CharacterStatsModel : MBGameModel<CharacterStatsModel>
	{
		// Token: 0x06001BDB RID: 7131
		public abstract ExplainedNumber MaxHitpoints(CharacterObject character, bool includeDescriptions = false);

		// Token: 0x06001BDC RID: 7132
		public abstract int GetTier(CharacterObject character);

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001BDD RID: 7133
		public abstract int MaxCharacterTier { get; }

		// Token: 0x06001BDE RID: 7134
		public abstract int WoundedHitPointLimit(Hero hero);
	}
}
