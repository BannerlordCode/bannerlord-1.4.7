using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C6 RID: 1222
	public static class SellPrisonersAction
	{
		// Token: 0x06004AE6 RID: 19174 RVA: 0x0017B4DC File Offset: 0x001796DC
		private static void ApplyInternal(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners, bool applyConsequences)
		{
			Settlement settlement = sellerParty.Settlement ?? ((buyerParty != null) ? buyerParty.Settlement : null);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			int num = 0;
			bool flag = false;
			foreach (TroopRosterElement troopRosterElement in prisoners.GetTroopRoster())
			{
				CharacterObject character = troopRosterElement.Character;
				if (!character.IsHero)
				{
					if (applyConsequences)
					{
						sellerParty.PrisonRoster.AddToCounts(character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, 0, true, -1);
					}
				}
				else if (character.HeroObject != Hero.MainHero)
				{
					if (buyerParty != null)
					{
						if (!buyerParty.MapFaction.IsAtWarWith(character.HeroObject.MapFaction))
						{
							if (character.HeroObject.Clan == Clan.PlayerClan)
							{
								EndCaptivityAction.ApplyByReleasedByCompensation(character.HeroObject);
							}
							else
							{
								EndCaptivityAction.ApplyByRansom(character.HeroObject, null);
							}
						}
						else
						{
							if (sellerParty.MapFaction == buyerParty.MapFaction && sellerParty != PartyBase.MainParty)
							{
								flag = true;
								troopRoster.Add(troopRosterElement);
							}
							TransferPrisonerAction.Apply(character, sellerParty, buyerParty);
						}
					}
					else
					{
						EndCaptivityAction.ApplyByRansom(character.HeroObject, null);
					}
					if (settlement != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, null, character.HeroObject, false);
					}
				}
				if (applyConsequences && !flag && character != Hero.MainHero.CharacterObject)
				{
					int num2 = Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(character, (sellerParty != null) ? sellerParty.LeaderHero : null);
					num += troopRosterElement.Number * num2;
				}
			}
			if (applyConsequences && !flag && num > 0)
			{
				if (sellerParty.IsMobile)
				{
					Hero hero = null;
					if (sellerParty.LeaderHero != null && sellerParty.LeaderHero.HeroState == Hero.CharacterStates.Active)
					{
						hero = sellerParty.LeaderHero;
					}
					else if (sellerParty.Owner != null && sellerParty.Owner.HeroState == Hero.CharacterStates.Active)
					{
						hero = sellerParty.Owner;
					}
					else
					{
						Clan actualClan = sellerParty.MobileParty.ActualClan;
						if (((actualClan != null) ? actualClan.Leader : null) != null)
						{
							hero = sellerParty.MobileParty.ActualClan.Leader;
						}
					}
					GiveGoldAction.ApplyBetweenCharacters(null, hero, num, false);
				}
				else
				{
					Clan clan;
					if (buyerParty == null)
					{
						clan = null;
					}
					else
					{
						Settlement settlement2 = buyerParty.Settlement;
						clan = ((settlement2 != null) ? settlement2.OwnerClan : null);
					}
					bool flag2 = clan != Clan.PlayerClan;
					GiveGoldAction.ApplyForPartyToSettlement(null, sellerParty.Settlement, num, flag2);
				}
			}
			if (sellerParty.IsMobile)
			{
				SkillLevelingManager.OnPrisonerSell(sellerParty.MobileParty, in prisoners);
			}
			CampaignEventDispatcher.Instance.OnPrisonerSold(sellerParty, buyerParty, prisoners);
			if (settlement != null && troopRoster.Count > 0)
			{
				CampaignEventDispatcher.Instance.OnPrisonerDonatedToSettlement(sellerParty.MobileParty, troopRoster.ToFlattenedRoster(), settlement);
			}
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x0017B79C File Offset: 0x0017999C
		public static void ApplyForAllPrisoners(PartyBase sellerParty, PartyBase buyerParty)
		{
			SellPrisonersAction.ApplyInternal(sellerParty, buyerParty, sellerParty.PrisonRoster.CloneRosterData(), true);
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0017B7B1 File Offset: 0x001799B1
		public static void ApplyForSelectedPrisoners(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
			SellPrisonersAction.ApplyInternal(sellerParty, buyerParty, prisoners, true);
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x0017B7BC File Offset: 0x001799BC
		public static void ApplyByPartyScreen(TroopRoster prisoners)
		{
			SellPrisonersAction.ApplyInternal(PartyBase.MainParty, Hero.MainHero.CurrentSettlement.Party, prisoners, false);
		}
	}
}
