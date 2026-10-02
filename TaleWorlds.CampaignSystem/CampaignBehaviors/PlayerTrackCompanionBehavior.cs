using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000432 RID: 1074
	public class PlayerTrackCompanionBehavior : CampaignBehaviorBase
	{
		// Token: 0x060044F7 RID: 17655 RVA: 0x0015234C File Offset: 0x0015054C
		public override void RegisterEvents()
		{
			CampaignEvents.CharacterBecameFugitiveEvent.AddNonSerializedListener(this, new Action<Hero, bool>(this.HeroBecameFugitive));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.CompanionRemoved));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.SettlementEntered));
			CampaignEvents.NewCompanionAdded.AddNonSerializedListener(this, new Action<Hero>(this.CompanionAdded));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00152411 File Offset: 0x00150611
		private void OnHeroTeleportationRequested(Hero hero, Settlement settlement, MobileParty party, TeleportHeroAction.TeleportationDetail detail)
		{
			if (hero.IsPlayerCompanion && party == MobileParty.MainParty && detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToParty && this._scatteredCompanions.ContainsKey(hero))
			{
				this._scatteredCompanions.Remove(hero);
			}
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00152444 File Offset: 0x00150644
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)))
			{
				foreach (Hero hero in this._scatteredCompanions.Keys.ToList<Hero>())
				{
					if (hero.PartyBelongedTo != null || hero.GovernorOf != null || Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
					{
						this._scatteredCompanions.Remove(hero);
					}
				}
			}
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x001524F4 File Offset: 0x001506F4
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, CampaignTime>>("ScatteredCompanions", ref this._scatteredCompanions);
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00152508 File Offset: 0x00150708
		private void AddHeroToScatteredCompanions(Hero hero)
		{
			if (hero.IsPlayerCompanion)
			{
				if (!this._scatteredCompanions.ContainsKey(hero))
				{
					this._scatteredCompanions.Add(hero, CampaignTime.Now);
					return;
				}
				this._scatteredCompanions[hero] = CampaignTime.Now;
			}
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x00152543 File Offset: 0x00150743
		private void HeroBecameFugitive(Hero hero, bool showNotification)
		{
			this.AddHeroToScatteredCompanions(hero);
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x0015254C File Offset: 0x0015074C
		private void OnHeroPrisonerReleased(Hero releasedHero, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification)
		{
			this.AddHeroToScatteredCompanions(releasedHero);
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x00152558 File Offset: 0x00150758
		private void SettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party == MobileParty.MainParty)
			{
				foreach (Hero hero2 in this._scatteredCompanions.Keys.ToMBList<Hero>())
				{
					if (hero2.CurrentSettlement == settlement)
					{
						TextObject textObject = new TextObject("{=ahpSGaow}You hear that your companion {COMPANION.LINK}, who was separated from you after a battle, is currently in this settlement.", null);
						StringHelpers.SetCharacterProperties("COMPANION", hero2.CharacterObject, textObject, false);
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=dx0hmeH6}Tracking", null).ToString(), textObject.ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
						this._scatteredCompanions.Remove(hero2);
					}
				}
			}
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0015263C File Offset: 0x0015083C
		private void CompanionAdded(Hero companion)
		{
			if (this._scatteredCompanions.ContainsKey(companion))
			{
				this._scatteredCompanions.Remove(companion);
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00152659 File Offset: 0x00150859
		private void CompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (this._scatteredCompanions.ContainsKey(companion))
			{
				this._scatteredCompanions.Remove(companion);
			}
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00152676 File Offset: 0x00150876
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsPlayerCompanion && this._scatteredCompanions.ContainsKey(mobileParty.LeaderHero))
			{
				this._scatteredCompanions.Remove(mobileParty.LeaderHero);
			}
		}

		// Token: 0x04001376 RID: 4982
		private Dictionary<Hero, CampaignTime> _scatteredCompanions = new Dictionary<Hero, CampaignTime>();
	}
}
