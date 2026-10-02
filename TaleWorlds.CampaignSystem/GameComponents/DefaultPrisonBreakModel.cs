using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000142 RID: 322
	public class DefaultPrisonBreakModel : PrisonBreakModel
	{
		// Token: 0x060019A9 RID: 6569 RVA: 0x000810FC File Offset: 0x0007F2FC
		public override int GetNumberOfGuardsToSpawn(Settlement settlement)
		{
			int num = (int)Math.Ceiling((double)(2f + settlement.Town.Security / 30f));
			int num2 = settlement.Town.GetWallLevel() - 1;
			return num + num2;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00081138 File Offset: 0x0007F338
		public override bool CanPlayerStagePrisonBreak(Settlement settlement)
		{
			bool flag = false;
			if (settlement.IsFortification)
			{
				MobileParty garrisonParty = settlement.Town.GarrisonParty;
				bool flag2 = (garrisonParty != null && garrisonParty.PrisonRoster.TotalHeroes > 0) || settlement.Party.PrisonRoster.TotalHeroes > 0;
				flag = settlement.MapFaction != Clan.PlayerClan.MapFaction && !DiplomacyHelper.IsSameFactionAndNotEliminated(settlement.MapFaction, Clan.PlayerClan.MapFaction) && flag2;
			}
			return flag;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x000811B8 File Offset: 0x0007F3B8
		public override int GetPrisonBreakStartCost(Hero prisonerHero)
		{
			int num = MathF.Ceiling((float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(prisonerHero.CharacterObject, null) / 2000f * prisonerHero.CurrentSettlement.Town.Security * 40f - (float)(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) * 10));
			num = ((num < 100) ? 0 : (num / 100 * 100));
			return num + 1000;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0008122F File Offset: 0x0007F42F
		public override int GetRelationRewardOnPrisonBreak(Hero prisonerHero)
		{
			return 15;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00081233 File Offset: 0x0007F433
		public override float GetRogueryRewardOnPrisonBreak(Hero prisonerHero, bool isSuccess)
		{
			return (float)(isSuccess ? MBRandom.RandomInt(2000, 4500) : MBRandom.RandomInt(500, 1000));
		}

		// Token: 0x0400088A RID: 2186
		private const int BasePrisonBreakCost = 1000;
	}
}
