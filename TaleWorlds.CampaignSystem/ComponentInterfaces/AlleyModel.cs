using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F7 RID: 503
	public abstract class AlleyModel : MBGameModel<AlleyModel>
	{
		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06001F64 RID: 8036
		public abstract CampaignTime DestroyAlleyAfterDaysWhenLeaderIsDeath { get; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06001F65 RID: 8037
		public abstract int MinimumTroopCountInPlayerOwnedAlley { get; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06001F66 RID: 8038
		public abstract int MaximumTroopCountInPlayerOwnedAlley { get; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001F67 RID: 8039
		public abstract float GetDailyCrimeRatingOfAlley { get; }

		// Token: 0x06001F68 RID: 8040
		public abstract float GetDailyXpGainForAssignedClanMember(Hero assignedHero);

		// Token: 0x06001F69 RID: 8041
		public abstract float GetDailyXpGainForMainHero();

		// Token: 0x06001F6A RID: 8042
		public abstract float GetInitialXpGainForMainHero();

		// Token: 0x06001F6B RID: 8043
		public abstract float GetXpGainAfterSuccessfulAlleyDefenseForMainHero();

		// Token: 0x06001F6C RID: 8044
		public abstract TroopRoster GetTroopsOfAIOwnedAlley(Alley alley);

		// Token: 0x06001F6D RID: 8045
		public abstract TroopRoster GetTroopsOfAlleyForBattleMission(Alley alley);

		// Token: 0x06001F6E RID: 8046
		public abstract int GetDailyIncomeOfAlley(Alley alley);

		// Token: 0x06001F6F RID: 8047
		public abstract List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(Alley alley);

		// Token: 0x06001F70 RID: 8048
		public abstract TroopRoster GetTroopsToRecruitFromAlleyDependingOnAlleyRandom(Alley alley, float random);

		// Token: 0x06001F71 RID: 8049
		public abstract TextObject GetDisabledReasonTextForHero(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail);

		// Token: 0x06001F72 RID: 8050
		public abstract float GetAlleyAttackResponseTimeInDays(TroopRoster troopRoster);
	}
}
