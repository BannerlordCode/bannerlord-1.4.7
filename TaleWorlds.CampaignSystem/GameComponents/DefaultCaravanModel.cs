using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000100 RID: 256
	public class DefaultCaravanModel : CaravanModel
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00069163 File Offset: 0x00067363
		public override int MaxNumberOfItemsToBuyFromSingleCategory
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x0006916C File Offset: 0x0006736C
		public override float GetEliteCaravanSpawnChance(Hero hero)
		{
			float num = 0f;
			if (hero.Power >= 112f)
			{
				num = hero.Power * 0.0045f - 0.5f;
			}
			return num;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000691A0 File Offset: 0x000673A0
		public override int GetPowerChangeAfterCaravanCreation(Hero hero, MobileParty caravanParty)
		{
			if (hero.Power >= 50f)
			{
				return -30;
			}
			return 0;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000691B4 File Offset: 0x000673B4
		public override bool CanHeroCreateCaravan(Hero hero)
		{
			if (hero.IsMerchant && hero.PartyBelongedTo == null)
			{
				if (hero.OwnedCaravans.Count<CaravanPartyComponent>((CaravanPartyComponent x) => !x.MobileParty.Ai.IsDisabled) == 0 && hero.IsActive && !hero.IsTemplate)
				{
					return hero.CanLeadParty();
				}
			}
			return false;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00069218 File Offset: 0x00067418
		public override int GetCaravanFormingCost(bool largerCaravan, bool navalCaravan)
		{
			int num = (largerCaravan ? 22500 : 15000);
			if (CharacterObject.PlayerCharacter.Culture.HasFeat(DefaultCulturalFeats.AseraiTraderFeat))
			{
				return MathF.Round((float)num * DefaultCulturalFeats.AseraiTraderFeat.EffectBonus);
			}
			return num;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00069260 File Offset: 0x00067460
		public override int GetInitialTradeGold(Hero owner, bool navalCaravan, bool largeCaravan)
		{
			int num = 10000;
			int num2 = ((owner == Hero.MainHero) ? 5000 : 0);
			if (largeCaravan)
			{
				num = 17500;
			}
			return num + num2;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00069290 File Offset: 0x00067490
		public override int GetMaxGoldToSpendOnOneItemCategory(MobileParty caravan, ItemCategory itemCategory)
		{
			return 1500;
		}
	}
}
