using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018D RID: 397
	public abstract class InformationRestrictionModel : MBGameModel<InformationRestrictionModel>
	{
		// Token: 0x06001C2A RID: 7210
		public abstract bool DoesPlayerKnowDetailsOf(Settlement settlement);

		// Token: 0x06001C2B RID: 7211
		public abstract bool DoesPlayerKnowDetailsOf(Hero hero);
	}
}
