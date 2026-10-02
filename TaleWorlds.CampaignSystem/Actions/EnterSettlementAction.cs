using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B1 RID: 1201
	public static class EnterSettlementAction
	{
		// Token: 0x06004A91 RID: 19089 RVA: 0x001791F8 File Offset: 0x001773F8
		private static void ApplyInternal(Hero hero, MobileParty mobileParty, Settlement settlement, EnterSettlementAction.EnterSettlementDetail detail, object subject = null, bool isPlayerInvolved = false)
		{
			if (mobileParty != null && mobileParty.IsDisbanding && mobileParty.TargetSettlement == settlement)
			{
				DestroyPartyAction.ApplyForDisbanding(mobileParty, settlement);
			}
			else
			{
				CampaignEventDispatcher.Instance.OnBeforeSettlementEntered(mobileParty, settlement, hero);
				CampaignEventDispatcher.Instance.OnSettlementEntered(mobileParty, settlement, hero);
				CampaignEventDispatcher.Instance.OnAfterSettlementEntered(mobileParty, settlement, hero);
				if (detail == EnterSettlementAction.EnterSettlementDetail.Prisoner)
				{
					if (hero != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, null, hero, false);
					}
					if (mobileParty != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, mobileParty.PrisonRoster.ToFlattenedRoster(), null, false);
					}
				}
				Hero hero2 = ((mobileParty != null) ? mobileParty.LeaderHero : hero);
				if (hero2 != null)
				{
					float currentTime = Campaign.CurrentTime;
					if (hero2.Clan == settlement.OwnerClan)
					{
						Clan clan = hero2.Clan;
						if (((clan != null) ? clan.Leader : null) == hero2)
						{
							settlement.LastVisitTimeOfOwner = currentTime;
						}
					}
				}
				if (mobileParty == MobileParty.MainParty && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
				{
					foreach (MobileParty mobileParty2 in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
					{
						EnterSettlementAction.ApplyForParty(mobileParty2, settlement);
					}
				}
				if (hero != null && mobileParty == null && hero.PartyBelongedTo == null && hero.PartyBelongedToAsPrisoner == null && hero.Clan == Clan.PlayerClan && hero.GovernorOf == null)
				{
					CampaignEventDispatcher.Instance.OnHeroGetsBusy(hero, HeroGetsBusyReasons.BecomeEmissary);
				}
			}
			if (mobileParty != null && mobileParty.IsFleeing())
			{
				mobileParty.Ai.DisableForHours(5);
			}
			if (hero == Hero.MainHero || mobileParty == MobileParty.MainParty)
			{
				Debug.Print(string.Format("Player has entered {0}: {1}", settlement.StringId, settlement), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x001793B8 File Offset: 0x001775B8
		public static void ApplyForParty(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty != null && mobileParty.Army != null && mobileParty.Army.LeaderParty != null && mobileParty.Army.LeaderParty != mobileParty && mobileParty.Army.LeaderParty.CurrentSettlement == settlement && mobileParty.AttachedTo == null)
			{
				mobileParty.Army.AddPartyToMergedParties(mobileParty);
			}
			bool isCurrentlyAtSea = mobileParty.IsCurrentlyAtSea;
			if (!settlement.IsVillage && mobileParty.HasLandNavigationCapability)
			{
				mobileParty.IsCurrentlyAtSea = false;
			}
			mobileParty.CurrentSettlement = settlement;
			if (isCurrentlyAtSea && !mobileParty.IsCurrentlyAtSea && mobileParty.Ships.Any<Ship>() && !mobileParty.Anchor.IsAtSettlement(settlement))
			{
				mobileParty.Anchor.SetSettlement(settlement);
			}
			settlement.SettlementComponent.OnPartyEntered(mobileParty);
			EnterSettlementAction.ApplyInternal(mobileParty.LeaderHero, mobileParty, settlement, EnterSettlementAction.EnterSettlementDetail.WarParty, null, false);
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x00179483 File Offset: 0x00177683
		public static void ApplyForPartyEntersAlley(MobileParty party, Settlement settlement, Alley alley, bool isPlayerInvolved = false)
		{
			EnterSettlementAction.ApplyInternal(null, party, settlement, EnterSettlementAction.EnterSettlementDetail.PartyEntersAlley, alley, isPlayerInvolved);
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x00179490 File Offset: 0x00177690
		public static void ApplyForCharacterOnly(Hero hero, Settlement settlement)
		{
			hero.StayingInSettlement = settlement;
			EnterSettlementAction.ApplyInternal(hero, null, settlement, EnterSettlementAction.EnterSettlementDetail.Character, null, false);
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x001794A4 File Offset: 0x001776A4
		public static void ApplyForPrisoner(Hero hero, Settlement settlement)
		{
			hero.ChangeState(Hero.CharacterStates.Prisoner);
			EnterSettlementAction.ApplyInternal(hero, null, settlement, EnterSettlementAction.EnterSettlementDetail.Prisoner, null, false);
		}

		// Token: 0x02000897 RID: 2199
		private enum EnterSettlementDetail
		{
			// Token: 0x040024B5 RID: 9397
			WarParty,
			// Token: 0x040024B6 RID: 9398
			PartyEntersAlley,
			// Token: 0x040024B7 RID: 9399
			Character,
			// Token: 0x040024B8 RID: 9400
			Prisoner
		}
	}
}
