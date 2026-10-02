using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000162 RID: 354
	public class DefaultValuationModel : ValuationModel
	{
		// Token: 0x06001B03 RID: 6915 RVA: 0x0008BF6F File Offset: 0x0008A16F
		public override float GetMilitaryValueOfParty(MobileParty party)
		{
			return party.Party.CalculateCurrentStrength() * 15f;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0008BF82 File Offset: 0x0008A182
		public override float GetValueOfTroop(CharacterObject troop)
		{
			return troop.GetPower() * 15f;
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0008BF90 File Offset: 0x0008A190
		public override float GetValueOfHero(Hero hero)
		{
			if (hero.Clan != null)
			{
				return ((float)hero.Clan.Gold * 0.15f + (float)((1 + hero.Clan.Tier * hero.Clan.Tier) * 500)) * ((hero.Clan.Leader == hero) ? 4f : 1f);
			}
			return 500f;
		}
	}
}
