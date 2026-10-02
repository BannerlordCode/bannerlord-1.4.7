using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003DD RID: 989
	public class CharacterDevelopmentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003CD2 RID: 15570 RVA: 0x00103FC4 File Offset: 0x001021C4
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action(this.OnCharacterCreationIsOver));
			CampaignEvents.OnHeroActivatedEvent.AddNonSerializedListener(this, new Action<Hero, Hero.CharacterStates>(this.OnHeroActivated));
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x00104016 File Offset: 0x00102216
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x00104018 File Offset: 0x00102218
		private void DailyTickHero(Hero hero)
		{
			if (this.ShouldDevelopCharacterStats(hero))
			{
				hero.HeroDeveloper.DevelopCharacterStats();
			}
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x00104030 File Offset: 0x00102230
		private void OnCharacterCreationIsOver()
		{
			if (CampaignOptions.AutoAllocateClanMemberPerks)
			{
				foreach (Hero hero in Campaign.Current.AliveHeroes)
				{
					if (!hero.IsChild && hero.Clan == Clan.PlayerClan && hero != Hero.MainHero)
					{
						hero.HeroDeveloper.DevelopCharacterStats();
					}
				}
			}
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x001040B0 File Offset: 0x001022B0
		private void OnHeroActivated(Hero hero, Hero.CharacterStates previousState)
		{
			if (this.ShouldDevelopCharacterStats(hero))
			{
				hero.HeroDeveloper.DevelopCharacterStats();
			}
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x001040C8 File Offset: 0x001022C8
		private bool ShouldDevelopCharacterStats(Hero hero)
		{
			if (!hero.IsChild && hero.IsAlive && (hero.Clan != Clan.PlayerClan || (hero != Hero.MainHero && CampaignOptions.AutoAllocateClanMemberPerks)))
			{
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				return ((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null;
			}
			return false;
		}
	}
}
