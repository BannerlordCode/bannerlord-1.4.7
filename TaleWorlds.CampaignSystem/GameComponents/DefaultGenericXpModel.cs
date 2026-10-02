using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000119 RID: 281
	public class DefaultGenericXpModel : GenericXpModel
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x00073B82 File Offset: 0x00071D82
		public override float GetXpMultiplier(Hero hero)
		{
			if (hero.IsPlayerCompanion && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.NaturalLeader))
			{
				return 1.2f;
			}
			return 1f;
		}
	}
}
