using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000112 RID: 274
	public class DefaultEmissaryModel : EmissaryModel
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x00071818 File Offset: 0x0006FA18
		public override int EmissaryRelationBonusForMainClan
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0007181C File Offset: 0x0006FA1C
		public override bool IsEmissary(Hero hero)
		{
			return (hero.CompanionOf == Clan.PlayerClan || hero.Clan == Clan.PlayerClan) && hero.PartyBelongedTo == null && hero.CurrentSettlement != null && hero.CurrentSettlement.IsFortification && !hero.IsPrisoner && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
		}
	}
}
