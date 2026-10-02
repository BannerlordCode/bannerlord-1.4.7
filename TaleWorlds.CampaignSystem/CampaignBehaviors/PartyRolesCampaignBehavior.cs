using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000429 RID: 1065
	public class PartyRolesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060043D4 RID: 17364 RVA: 0x0014A0CC File Offset: 0x001482CC
		public override void RegisterEvents()
		{
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartySpawned));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
			CampaignEvents.OnHeroGetsBusyEvent.AddNonSerializedListener(this, new Action<Hero, HeroGetsBusyReasons>(this.OnHeroGetsBusy));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0014A17A File Offset: 0x0014837A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0014A17C File Offset: 0x0014837C
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (victim.Clan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(victim);
			}
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0014A192 File Offset: 0x00148392
		private void OnHeroPrisonerTaken(PartyBase party, Hero prisoner)
		{
			if (prisoner.Clan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(prisoner);
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0014A1A8 File Offset: 0x001483A8
		private void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			if (((newGovernor != null) ? newGovernor.Clan : null) == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(newGovernor);
			}
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x0014A1C4 File Offset: 0x001483C4
		private void OnPartySpawned(MobileParty spawnedParty)
		{
			if (spawnedParty.IsLordParty && spawnedParty.ActualClan == Clan.PlayerClan)
			{
				foreach (TroopRosterElement troopRosterElement in spawnedParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character.IsHero)
					{
						this.RemoveAllPartyRolesOfHeroIfExist(troopRosterElement.Character.HeroObject);
					}
				}
			}
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0014A248 File Offset: 0x00148448
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			this.RemoveAllPartyRolesOfHeroIfExist(companion);
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x0014A251 File Offset: 0x00148451
		private void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
			if (hero.Clan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(hero);
			}
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x0014A267 File Offset: 0x00148467
		private void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			if (oldClan == Clan.PlayerClan)
			{
				this.RemoveAllPartyRolesOfHeroIfExist(hero);
			}
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0014A278 File Offset: 0x00148478
		private void RemoveAllPartyRolesOfHeroIfExist(Hero hero)
		{
			foreach (WarPartyComponent warPartyComponent in Clan.PlayerClan.WarPartyComponents)
			{
				warPartyComponent.MobileParty.RemoveAllPartyRolesOfHero(hero);
			}
		}
	}
}
