using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200039E RID: 926
	public interface IPartyScreenPrisonHandler
	{
		// Token: 0x06003585 RID: 13701
		void ExecuteTakeAllPrisonersScript();

		// Token: 0x06003586 RID: 13702
		void ExecuteDoneScript();

		// Token: 0x06003587 RID: 13703
		void ExecuteResetScript();

		// Token: 0x06003588 RID: 13704
		void ExecuteSellAllPrisoners();
	}
}
