using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000111 RID: 273
	public class DefaultDisguiseDetectionModel : DisguiseDetectionModel
	{
		// Token: 0x060017CC RID: 6092 RVA: 0x000716EC File Offset: 0x0006F8EC
		public override float CalculateDisguiseDetectionProbability(Settlement settlement)
		{
			float num = 0f;
			int num2 = 0;
			if (settlement.Town != null && settlement.Town.GarrisonParty != null)
			{
				foreach (TroopRosterElement troopRosterElement in settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					num2 += troopRosterElement.Number;
					num += (float)(troopRosterElement.Number * troopRosterElement.Character.Level);
				}
			}
			num /= (float)MathF.Max(1, num2);
			float num3 = 0.3f + 0.003f * (float)Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) - 0.005f * num - MathF.Max(0.15f, 0.00015f * Clan.PlayerClan.Renown);
			if (Hero.MainHero.CharacterObject.GetPerkValue(DefaultPerks.Roguery.TwoFaced) && num3 > 0f)
			{
				num3 += num3 * DefaultPerks.Roguery.TwoFaced.PrimaryBonus;
			}
			return MathF.Clamp(num3, 0f, 1f);
		}
	}
}
