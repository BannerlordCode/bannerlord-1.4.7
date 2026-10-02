using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000C8 RID: 200
	public class PlayerToggleTrackSettlementFromEncyclopediaEvent : EventBase
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001333 RID: 4915 RVA: 0x0004DC0B File Offset: 0x0004BE0B
		// (set) Token: 0x06001334 RID: 4916 RVA: 0x0004DC13 File Offset: 0x0004BE13
		public bool IsCurrentlyTracked { get; private set; }

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0004DC1C File Offset: 0x0004BE1C
		// (set) Token: 0x06001336 RID: 4918 RVA: 0x0004DC24 File Offset: 0x0004BE24
		public Settlement ToggledTrackedSettlement { get; private set; }

		// Token: 0x06001337 RID: 4919 RVA: 0x0004DC2D File Offset: 0x0004BE2D
		public PlayerToggleTrackSettlementFromEncyclopediaEvent(Settlement toggleTrackedSettlement, bool isCurrentlyTracked)
		{
			this.ToggledTrackedSettlement = toggleTrackedSettlement;
			this.IsCurrentlyTracked = isCurrentlyTracked;
		}
	}
}
