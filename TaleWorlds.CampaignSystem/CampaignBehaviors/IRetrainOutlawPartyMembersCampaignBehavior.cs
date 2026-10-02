using System;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200040A RID: 1034
	public interface IRetrainOutlawPartyMembersCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x060040F5 RID: 16629
		int GetRetrainedNumber(CharacterObject character);

		// Token: 0x060040F6 RID: 16630
		void SetRetrainedNumber(CharacterObject character, int number);
	}
}
