using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F4 RID: 500
	public abstract class DelayedTeleportationModel : MBGameModel<DelayedTeleportationModel>
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06001F5C RID: 8028
		public abstract float DefaultTeleportationSpeed { get; }

		// Token: 0x06001F5D RID: 8029
		public abstract ExplainedNumber GetTeleportationDelayAsHours(Hero teleportingHero, PartyBase target);

		// Token: 0x06001F5E RID: 8030
		public abstract bool CanPerformImmediateTeleport(Hero hero, MobileParty targetMobileParty, Settlement targetSettlement);
	}
}
