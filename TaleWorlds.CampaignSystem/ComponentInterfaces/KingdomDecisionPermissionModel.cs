using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AA RID: 426
	public abstract class KingdomDecisionPermissionModel : MBGameModel<KingdomDecisionPermissionModel>
	{
		// Token: 0x06001D23 RID: 7459
		public abstract bool IsPolicyDecisionAllowed(PolicyObject policy);

		// Token: 0x06001D24 RID: 7460
		public abstract bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x06001D25 RID: 7461
		public abstract bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x06001D26 RID: 7462
		public abstract bool IsStartAllianceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x06001D27 RID: 7463
		public abstract bool IsAnnexationDecisionAllowed(Settlement annexedSettlement);

		// Token: 0x06001D28 RID: 7464
		public abstract bool IsExpulsionDecisionAllowed(Clan expelledClan);

		// Token: 0x06001D29 RID: 7465
		public abstract bool IsKingSelectionDecisionAllowed(Kingdom kingdom);
	}
}
