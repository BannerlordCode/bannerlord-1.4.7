using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000134 RID: 308
	public class DefaultPartyImpairmentModel : PartyImpairmentModel
	{
		// Token: 0x0600192A RID: 6442 RVA: 0x0007CFE8 File Offset: 0x0007B1E8
		public override float GetSiegeExpectedVulnerabilityTime()
		{
			float num = ((float)CampaignTime.SunRise + MBRandom.RandomFloatNormal + (float)CampaignTime.HoursInDay - CampaignTime.Now.CurrentHourInDay) % (float)CampaignTime.HoursInDay;
			float num2 = MathF.Pow(MBRandom.RandomFloat, 6f);
			return (((MBRandom.RandomFloatNormal > 0f) ? num2 : (1f - num2)) * (float)CampaignTime.HoursInDay + num) % (float)CampaignTime.HoursInDay;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0007D054 File Offset: 0x0007B254
		public override ExplainedNumber GetDisorganizedStateDuration(MobileParty party)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(6f, false, null);
			bool flag = party.MapEvent != null && (party.MapEvent.IsRaid || party.MapEvent.IsSiegeAssault);
			if (!party.IsCurrentlyAtSea && flag && party.HasPerk(DefaultPerks.Tactics.SwiftRegroup, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Tactics.SwiftRegroup.PrimaryBonus, DefaultPerks.Tactics.SwiftRegroup.Description);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Foragers, party, false, ref explainedNumber, party.IsCurrentlyAtSea);
			return explainedNumber;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0007D0E4 File Offset: 0x0007B2E4
		public override bool CanGetDisorganized(PartyBase party)
		{
			return party.IsActive && party.IsMobile && party.MobileParty.MemberRoster.TotalManCount >= 10 && (party.MobileParty.Army == null || party.MobileParty == party.MobileParty.Army.LeaderParty || party.MobileParty.AttachedTo != null);
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0007D14C File Offset: 0x0007B34C
		public override float GetVulnerabilityStateDuration(PartyBase party)
		{
			return MBRandom.RandomFloatNormal + 4f;
		}

		// Token: 0x0400083B RID: 2107
		private const float BaseDisorganizedStateDuration = 6f;

		// Token: 0x0400083C RID: 2108
		private static readonly TextObject _settlementInvolvedMapEvent = new TextObject("{=KVlPhPSD}Settlement involved map event", null);
	}
}
