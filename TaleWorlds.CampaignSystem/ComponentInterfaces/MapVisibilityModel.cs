using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018C RID: 396
	public abstract class MapVisibilityModel : MBGameModel<MapVisibilityModel>
	{
		// Token: 0x06001C24 RID: 7204
		public abstract float MaximumSeeingRange();

		// Token: 0x06001C25 RID: 7205
		public abstract float GetPartySeeingRangeBase(MobileParty party);

		// Token: 0x06001C26 RID: 7206
		public abstract ExplainedNumber GetPartySpottingRange(MobileParty party, bool includeDescriptions = false);

		// Token: 0x06001C27 RID: 7207
		public abstract float GetPartySpottingRatioForMainPartySeeingRange(MobileParty party);

		// Token: 0x06001C28 RID: 7208
		public abstract float GetHideoutSpottingDistance();
	}
}
