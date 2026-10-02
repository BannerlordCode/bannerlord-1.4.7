using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B0 RID: 432
	public abstract class RomanceModel : MBGameModel<RomanceModel>
	{
		// Token: 0x06001D4C RID: 7500
		public abstract int GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest);
	}
}
