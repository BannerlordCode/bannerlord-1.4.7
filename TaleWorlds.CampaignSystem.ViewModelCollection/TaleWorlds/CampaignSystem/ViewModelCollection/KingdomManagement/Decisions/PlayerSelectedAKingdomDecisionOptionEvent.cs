using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x02000076 RID: 118
	public class PlayerSelectedAKingdomDecisionOptionEvent : EventBase
	{
		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002A9C4 File Offset: 0x00028BC4
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0002A9CC File Offset: 0x00028BCC
		public DecisionOutcome Option { get; private set; }

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002A9D5 File Offset: 0x00028BD5
		public PlayerSelectedAKingdomDecisionOptionEvent(DecisionOutcome option)
		{
			this.Option = option;
		}
	}
}
