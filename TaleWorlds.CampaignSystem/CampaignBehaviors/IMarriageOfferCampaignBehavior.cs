using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000403 RID: 1027
	public interface IMarriageOfferCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x0600406A RID: 16490
		void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden);

		// Token: 0x0600406B RID: 16491
		void OnMarriageOfferCanceled(Hero suitor, Hero maiden);

		// Token: 0x0600406C RID: 16492
		MBBindingList<TextObject> GetMarriageAcceptedConsequences();

		// Token: 0x0600406D RID: 16493
		void OnMarriageOfferAcceptedOnPopUp();

		// Token: 0x0600406E RID: 16494
		void OnMarriageOfferDeclinedOnPopUp();

		// Token: 0x0600406F RID: 16495
		bool IsHeroEngaged(Hero hero);
	}
}
