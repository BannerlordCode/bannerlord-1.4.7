using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041D RID: 1053
	public class NotablePowerManagementBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004354 RID: 17236 RVA: 0x0014647C File Offset: 0x0014467C
		public override void RegisterEvents()
		{
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x001464CE File Offset: 0x001446CE
		private void OnHeroCreated(Hero hero, bool isMaternal)
		{
			if (hero.IsNotable)
			{
				hero.AddPower((float)Campaign.Current.Models.NotablePowerModel.GetInitialPower(hero));
			}
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x001464F4 File Offset: 0x001446F4
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsAlive && hero.IsNotable)
			{
				hero.AddPower(Campaign.Current.Models.NotablePowerModel.CalculateDailyPowerChangeForHero(hero, false).ResultNumber);
				this.BalanceGoldAndPowerOfNotable(hero);
			}
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x0014653C File Offset: 0x0014473C
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent mapEvent)
		{
			foreach (Hero hero in mapEvent.MapEventSettlement.Notables)
			{
				hero.AddPower(-5f);
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x00146598 File Offset: 0x00144798
		private void BalanceGoldAndPowerOfNotable(Hero notable)
		{
			if (notable.Gold > 10500)
			{
				int num = (notable.Gold - 10000) / 500;
				GiveGoldAction.ApplyBetweenCharacters(notable, null, num * 500, true);
				notable.AddPower((float)num);
				return;
			}
			if (notable.Gold < 4500 && notable.Power > 0f)
			{
				int num2 = (5000 - notable.Gold) / 500;
				GiveGoldAction.ApplyBetweenCharacters(null, notable, num2 * 500, true);
				notable.AddPower((float)(-(float)num2));
			}
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x00146622 File Offset: 0x00144822
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04001341 RID: 4929
		private const int GoldLimitForNotablesToStartGainingPower = 10000;

		// Token: 0x04001342 RID: 4930
		private const int GoldLimitForNotablesToStartLosingPower = 5000;

		// Token: 0x04001343 RID: 4931
		private const int GoldNeededToGainOnePower = 500;
	}
}
