using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000439 RID: 1081
	public class RansomOfferCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06004540 RID: 17728 RVA: 0x00154408 File Offset: 0x00152608
		private static TextObject RansomPanelTitleText
		{
			get
			{
				return new TextObject("{=ho5EndaV}Decision", null);
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06004541 RID: 17729 RVA: 0x00154415 File Offset: 0x00152615
		private static TextObject RansomPanelAffirmativeText
		{
			get
			{
				return new TextObject("{=Y94H6XnK}Accept", null);
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06004542 RID: 17730 RVA: 0x00154422 File Offset: 0x00152622
		private static TextObject RansomPanelNegativeText
		{
			get
			{
				return new TextObject("{=cOgmdp9e}Decline", null);
			}
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x00154430 File Offset: 0x00152630
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnRansomOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnRansomOffered));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.PrisonersChangeInSettlement.AddNonSerializedListener(this, new Action<Settlement, FlattenedTroopRoster, Hero, bool>(this.OnPrisonersChangeInSettlement));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x001544DE File Offset: 0x001526DE
		private void OnHeroPrisonerTaken(PartyBase party, Hero hero)
		{
			this.HandleDeclineRansomOffer(hero);
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x001544E8 File Offset: 0x001526E8
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsPrisoner && hero.Clan != null && hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.MapFaction != null && !hero.PartyBelongedToAsPrisoner.MapFaction.IsBanditFaction && hero != Hero.MainHero && hero.Clan.AliveLords.Count > 1 && hero.MapFaction != null)
			{
				this.ConsiderRansomPrisoner(hero);
			}
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x00154558 File Offset: 0x00152758
		private void ConsiderRansomPrisoner(Hero hero)
		{
			Clan captorClanOfPrisoner = this.GetCaptorClanOfPrisoner(hero);
			if (captorClanOfPrisoner != null)
			{
				Hero hero2 = ((hero.Clan.Leader != hero) ? hero.Clan.Leader : hero.Clan.AliveLords.Where<Hero>((Hero t) => t != hero.Clan.Leader).GetRandomElementInefficiently<Hero>());
				if (hero2 != Hero.MainHero || !hero2.IsPrisoner)
				{
					if (captorClanOfPrisoner == Clan.PlayerClan || hero.Clan == Clan.PlayerClan)
					{
						if (this._currentRansomHero == null && !MobileParty.MainParty.IsInRaftState)
						{
							float num = ((!this._heroesWithDeclinedRansomOffers.Contains(hero)) ? 0.2f : 0.12f);
							if (MBRandom.RandomFloat < num)
							{
								float num2 = (float)new SetPrisonerFreeBarterable(hero, captorClanOfPrisoner.Leader, hero.PartyBelongedToAsPrisoner, hero2).GetUnitValueForFaction(hero.Clan) * 1.1f;
								if (num2 > 1E-05f && (float)(hero2.Gold + 1000) >= num2)
								{
									this.SetCurrentRansomHero(hero, hero2);
									StringHelpers.SetCharacterProperties("CAPTIVE_HERO", hero.CharacterObject, RansomOfferCampaignBehavior.RansomOfferDescriptionText, false);
									Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new RansomOfferMapNotification(hero, RansomOfferCampaignBehavior.RansomOfferDescriptionText));
									return;
								}
							}
						}
					}
					else if (MBRandom.RandomFloat < 0.1f)
					{
						SetPrisonerFreeBarterable setPrisonerFreeBarterable = new SetPrisonerFreeBarterable(hero, captorClanOfPrisoner.Leader, hero.PartyBelongedToAsPrisoner, hero2);
						if (setPrisonerFreeBarterable.GetValueForFaction(captorClanOfPrisoner) + setPrisonerFreeBarterable.GetValueForFaction(hero.Clan) > 0)
						{
							Campaign.Current.BarterManager.ExecuteAiBarter(captorClanOfPrisoner, hero.Clan, captorClanOfPrisoner.Leader, hero2, setPrisonerFreeBarterable);
						}
					}
				}
			}
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x00154758 File Offset: 0x00152958
		private Clan GetCaptorClanOfPrisoner(Hero hero)
		{
			Clan clan;
			if (hero.PartyBelongedToAsPrisoner.IsMobile)
			{
				if ((hero.PartyBelongedToAsPrisoner.MobileParty.IsMilitia || hero.PartyBelongedToAsPrisoner.MobileParty.IsGarrison || hero.PartyBelongedToAsPrisoner.MobileParty.IsCaravan || hero.PartyBelongedToAsPrisoner.MobileParty.IsVillager) && hero.PartyBelongedToAsPrisoner.Owner != null)
				{
					if (hero.PartyBelongedToAsPrisoner.Owner.IsNotable)
					{
						clan = hero.PartyBelongedToAsPrisoner.Owner.CurrentSettlement.OwnerClan;
					}
					else
					{
						clan = hero.PartyBelongedToAsPrisoner.Owner.Clan;
					}
				}
				else if (hero.PartyBelongedToAsPrisoner.MobileParty.IsPatrolParty)
				{
					clan = hero.PartyBelongedToAsPrisoner.MobileParty.HomeSettlement.OwnerClan;
				}
				else
				{
					clan = hero.PartyBelongedToAsPrisoner.MobileParty.ActualClan;
				}
			}
			else
			{
				clan = hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan;
			}
			return clan;
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x00154858 File Offset: 0x00152A58
		public void SetCurrentRansomHero(Hero hero, Hero ransomPayer = null)
		{
			this._currentRansomHero = hero;
			this._currentRansomPayer = ransomPayer;
			this._currentRansomOfferDate = ((hero != null) ? CampaignTime.Now : CampaignTime.Never);
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x00154880 File Offset: 0x00152A80
		private void OnRansomOffered(Hero captiveHero)
		{
			Clan captorClanOfPrisoner = this.GetCaptorClanOfPrisoner(captiveHero);
			Clan clan = ((captiveHero.Clan == Clan.PlayerClan) ? captorClanOfPrisoner : captiveHero.Clan);
			Hero ransomPayer = ((captiveHero.Clan.Leader != captiveHero) ? captiveHero.Clan.Leader : captiveHero.Clan.AliveLords.Where<Hero>((Hero t) => t != captiveHero.Clan.Leader).GetRandomElementInefficiently<Hero>());
			int ransomPrice = (int)((float)new SetPrisonerFreeBarterable(captiveHero, captorClanOfPrisoner.Leader, captiveHero.PartyBelongedToAsPrisoner, ransomPayer).GetUnitValueForFaction(captiveHero.Clan) * 1.1f);
			TextObject textObject = ((captorClanOfPrisoner == Clan.PlayerClan) ? RansomOfferCampaignBehavior.RansomPanelDescriptionPlayerHeldPrisonerText : RansomOfferCampaignBehavior.RansomPanelDescriptionNpcHeldPrisonerText);
			textObject.SetTextVariable("CLAN_NAME", clan.Name);
			textObject.SetTextVariable("GOLD_AMOUNT", ransomPrice);
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			StringHelpers.SetCharacterProperties("CAPTIVE_HERO", captiveHero.CharacterObject, textObject, false);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			InformationManager.ShowInquiry(new InquiryData(RansomOfferCampaignBehavior.RansomPanelTitleText.ToString(), textObject.ToString(), true, true, RansomOfferCampaignBehavior.RansomPanelAffirmativeText.ToString(), RansomOfferCampaignBehavior.RansomPanelNegativeText.ToString(), delegate
			{
				this.AcceptRansomOffer(ransomPrice);
			}, new Action(this.DeclineRansomOffer), "", 0f, null, () => this.IsAffirmativeOptionEnabled(ransomPayer, ransomPrice), null), true, false);
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x00154A34 File Offset: 0x00152C34
		private ValueTuple<bool, string> IsAffirmativeOptionEnabled(Hero ransomPayer, int ransomPrice)
		{
			if (ransomPayer == Hero.MainHero && ransomPayer.Gold < ransomPrice)
			{
				return new ValueTuple<bool, string>(false, "{=d0kbtGYn}You don't have enough gold.");
			}
			return new ValueTuple<bool, string>(true, string.Empty);
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x00154A60 File Offset: 0x00152C60
		private void AcceptRansomOffer(int ransomPrice)
		{
			if (this._heroesWithDeclinedRansomOffers.Contains(this._currentRansomHero))
			{
				this._heroesWithDeclinedRansomOffers.Remove(this._currentRansomHero);
			}
			if (this._currentRansomPayer.Gold < ransomPrice + 1000 && this._currentRansomPayer != Hero.MainHero)
			{
				this._currentRansomPayer.Gold = ransomPrice + 1000;
			}
			GiveGoldAction.ApplyBetweenCharacters(this._currentRansomPayer, this.GetCaptorClanOfPrisoner(this._currentRansomHero).Leader, ransomPrice, false);
			EndCaptivityAction.ApplyByRansom(this._currentRansomHero, this._currentRansomHero.Clan.Leader);
			IStatisticsCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<IStatisticsCampaignBehavior>();
			if (behavior != null)
			{
				behavior.OnPlayerAcceptedRansomOffer(ransomPrice);
			}
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x00154B18 File Offset: 0x00152D18
		private void DeclineRansomOffer()
		{
			if (this._currentRansomHero.IsPrisoner && this._currentRansomHero.IsAlive && !this._heroesWithDeclinedRansomOffers.Contains(this._currentRansomHero))
			{
				this._heroesWithDeclinedRansomOffers.Add(this._currentRansomHero);
			}
			this.SetCurrentRansomHero(null, null);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x00154B6B File Offset: 0x00152D6B
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.HandleDeclineRansomOffer(victim);
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x00154B74 File Offset: 0x00152D74
		private void HandleDeclineRansomOffer(Hero victim)
		{
			if (this._currentRansomHero != null && (victim == this._currentRansomHero || victim == Hero.MainHero))
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
				this.DeclineRansomOffer();
			}
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x00154BA8 File Offset: 0x00152DA8
		private void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster roster, Hero prisoner, bool takenFromDungeon)
		{
			if (!takenFromDungeon && this._currentRansomHero != null)
			{
				if (prisoner == this._currentRansomHero)
				{
					CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
					this.DeclineRansomOffer();
					return;
				}
				if (roster != null)
				{
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in roster)
					{
						if (flattenedTroopRosterElement.Troop.IsHero && flattenedTroopRosterElement.Troop.HeroObject == this._currentRansomHero)
						{
							CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
							this.DeclineRansomOffer();
							break;
						}
					}
				}
			}
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x00154C58 File Offset: 0x00152E58
		private void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification)
		{
			this.HandleDeclineRansomOffer(prisoner);
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x00154C61 File Offset: 0x00152E61
		private void HourlyTick()
		{
			if (this._currentRansomHero != null && this._currentRansomOfferDate.ElapsedDaysUntilNow >= 2f)
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
				this.DeclineRansomOffer();
			}
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x00154C94 File Offset: 0x00152E94
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Hero>>("_heroesWithDeclinedRansomOffers", ref this._heroesWithDeclinedRansomOffers);
			dataStore.SyncData<Hero>("_currentRansomHero", ref this._currentRansomHero);
			dataStore.SyncData<Hero>("_currentRansomPayer", ref this._currentRansomPayer);
			dataStore.SyncData<CampaignTime>("_currentRansomOfferDate", ref this._currentRansomOfferDate);
		}

		// Token: 0x0400137B RID: 4987
		private const float RansomOfferInitialChance = 0.2f;

		// Token: 0x0400137C RID: 4988
		private const float RansomOfferChanceAfterRefusal = 0.12f;

		// Token: 0x0400137D RID: 4989
		private const float RansomOfferChanceForPrisonersKeptByAI = 0.1f;

		// Token: 0x0400137E RID: 4990
		private const float MapNotificationAutoDeclineDurationInDays = 2f;

		// Token: 0x0400137F RID: 4991
		private const int AmountOfGoldLeftAfterRansom = 1000;

		// Token: 0x04001380 RID: 4992
		private static TextObject RansomOfferDescriptionText = new TextObject("{=ZqJ92UN4}A courier with a ransom offer for the freedom of {CAPTIVE_HERO.NAME} has arrived.", null);

		// Token: 0x04001381 RID: 4993
		private static TextObject RansomPanelDescriptionNpcHeldPrisonerText = new TextObject("{=4fXpOe4N}A courier arrives from the {CLAN_NAME}. They hold {CAPTIVE_HERO.NAME} and are demanding {GOLD_AMOUNT}{GOLD_ICON} in ransom.", null);

		// Token: 0x04001382 RID: 4994
		private static TextObject RansomPanelDescriptionPlayerHeldPrisonerText = new TextObject("{=PutoRsWp}A courier arrives from the {CLAN_NAME}. They offer you {GOLD_AMOUNT}{GOLD_ICON} in ransom if you will free {CAPTIVE_HERO.NAME}.", null);

		// Token: 0x04001383 RID: 4995
		private List<Hero> _heroesWithDeclinedRansomOffers = new List<Hero>();

		// Token: 0x04001384 RID: 4996
		private Hero _currentRansomHero;

		// Token: 0x04001385 RID: 4997
		private Hero _currentRansomPayer;

		// Token: 0x04001386 RID: 4998
		private CampaignTime _currentRansomOfferDate;
	}
}
