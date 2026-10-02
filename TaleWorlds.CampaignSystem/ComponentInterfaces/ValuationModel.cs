using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BB RID: 443
	public abstract class ValuationModel : MBGameModel<ValuationModel>
	{
		// Token: 0x06001DC5 RID: 7621
		public abstract float GetValueOfTroop(CharacterObject troop);

		// Token: 0x06001DC6 RID: 7622
		public abstract float GetMilitaryValueOfParty(MobileParty party);

		// Token: 0x06001DC7 RID: 7623
		public abstract float GetValueOfHero(Hero hero);
	}
}
