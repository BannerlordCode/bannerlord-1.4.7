using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018F RID: 399
	public abstract class CampaignShipDamageModel : MBGameModel<CampaignShipDamageModel>
	{
		// Token: 0x06001C33 RID: 7219
		public abstract int GetHourlyShipDamage(MobileParty owner, Ship ship);

		// Token: 0x06001C34 RID: 7220
		public abstract float GetEstimatedSafeSailDuration(MobileParty mobileParty);

		// Token: 0x06001C35 RID: 7221
		public abstract float GetShipDamage(Ship ship, Ship rammingShip, float rawDamage);
	}
}
