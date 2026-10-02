using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F1 RID: 497
	public abstract class BattleCaptainModel : MBGameModel<BattleCaptainModel>
	{
		// Token: 0x06001F49 RID: 8009
		public abstract float GetCaptainRatingForTroopUsages(Hero hero, TroopUsageFlags flag, out List<PerkObject> compatiblePerks);
	}
}
