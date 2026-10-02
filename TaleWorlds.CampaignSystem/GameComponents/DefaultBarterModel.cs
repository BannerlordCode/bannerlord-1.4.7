using System;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F4 RID: 244
	public class DefaultBarterModel : BarterModel
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x000672E0 File Offset: 0x000654E0
		public override int BarterCooldownWithHeroInDays
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x000672E3 File Offset: 0x000654E3
		private int MaximumOverpayRelationBonus
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x000672E6 File Offset: 0x000654E6
		public override float MaximumPercentageOfNpcGoldToSpendAtBarter
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000672F0 File Offset: 0x000654F0
		public override int CalculateOverpayRelationIncreaseCosts(Hero hero, float overpayAmount)
		{
			int num = (int)hero.GetRelationWithPlayer();
			float num2 = MathF.Clamp((float)(num + this.MaximumOverpayRelationBonus), -100f, 100f);
			float num3 = 0f;
			int num4 = num;
			while ((float)num4 < num2)
			{
				int num5 = 1000 + 100 * (num4 * num4);
				if (overpayAmount >= (float)num5)
				{
					overpayAmount -= (float)num5;
					num3 += 1f;
					num4++;
				}
				else
				{
					if (MBRandom.RandomFloat <= overpayAmount / (float)num5)
					{
						num3 += 1f;
						break;
					}
					break;
				}
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.Tribute))
			{
				num3 *= 1f + DefaultPerks.Charm.Tribute.PrimaryBonus;
			}
			return MathF.Ceiling(num3);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00067390 File Offset: 0x00065590
		public override ExplainedNumber GetBarterPenalty(IFaction faction, ItemBarterable itemBarterable, Hero otherHero, PartyBase otherParty)
		{
			ExplainedNumber explainedNumber;
			if (faction == ((otherHero != null) ? otherHero.Clan : null) || faction == ((otherHero != null) ? otherHero.MapFaction : null) || faction == ((otherParty != null) ? otherParty.MapFaction : null))
			{
				explainedNumber = new ExplainedNumber(0.4f, false, null);
				if (otherHero != null && itemBarterable.OriginalOwner != null && otherHero != itemBarterable.OriginalOwner && otherHero.MapFaction != null && otherHero.IsPartyLeader)
				{
					CultureObject culture = otherHero.Culture;
					Hero originalOwner = itemBarterable.OriginalOwner;
					if (culture == ((originalOwner != null) ? originalOwner.Culture : null))
					{
						if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
						{
							explainedNumber.AddFactor(-DefaultPerks.Charm.EffortForThePeople.SecondaryBonus, null);
						}
					}
					else if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						explainedNumber.AddFactor(-DefaultPerks.Charm.SlickNegotiator.SecondaryBonus, null);
					}
					if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Trade.SelfMadeMan))
					{
						explainedNumber.AddFactor(-DefaultPerks.Trade.SelfMadeMan.PrimaryBonus, null);
					}
				}
			}
			else
			{
				Hero originalOwner2 = itemBarterable.OriginalOwner;
				if (faction != ((originalOwner2 != null) ? originalOwner2.Clan : null))
				{
					Hero originalOwner3 = itemBarterable.OriginalOwner;
					if (faction != ((originalOwner3 != null) ? originalOwner3.MapFaction : null))
					{
						PartyBase originalParty = itemBarterable.OriginalParty;
						if (faction != ((originalParty != null) ? originalParty.MapFaction : null))
						{
							explainedNumber = new ExplainedNumber(0f, false, null);
							return explainedNumber;
						}
					}
				}
				if (itemBarterable.ItemRosterElement.EquipmentElement.Item.IsAnimal || itemBarterable.ItemRosterElement.EquipmentElement.Item.IsMountable)
				{
					explainedNumber = new ExplainedNumber(-8.4f, false, null);
				}
				else if (itemBarterable.ItemRosterElement.EquipmentElement.Item.IsFood)
				{
					explainedNumber = new ExplainedNumber(-12.6f, false, null);
				}
				else
				{
					explainedNumber = new ExplainedNumber(-2.1f, false, null);
				}
				if (otherHero != null && otherHero != itemBarterable.OriginalOwner && otherHero.MapFaction != null && otherHero.IsPartyLeader)
				{
					CultureObject culture2 = otherHero.Culture;
					Hero originalOwner4 = itemBarterable.OriginalOwner;
					if (culture2 == ((originalOwner4 != null) ? originalOwner4.Culture : null))
					{
						if (otherHero.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
						{
							explainedNumber.AddFactor(DefaultPerks.Charm.EffortForThePeople.SecondaryBonus, null);
						}
					}
					else if (otherHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						explainedNumber.AddFactor(DefaultPerks.Charm.SlickNegotiator.SecondaryBonus, null);
					}
					if (otherHero.GetPerkValue(DefaultPerks.Trade.SelfMadeMan))
					{
						explainedNumber.AddFactor(DefaultPerks.Trade.SelfMadeMan.PrimaryBonus, null);
					}
				}
			}
			return explainedNumber;
		}
	}
}
