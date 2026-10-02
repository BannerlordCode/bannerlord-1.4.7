using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000168 RID: 360
	public class DefaultWallHitPointCalculationModel : WallHitPointCalculationModel
	{
		// Token: 0x06001B21 RID: 6945 RVA: 0x0008D169 File Offset: 0x0008B369
		public override float CalculateMaximumWallHitPoint(Town town)
		{
			if (town == null)
			{
				return 0f;
			}
			return this.CalculateMaximumWallHitPointInternal(town);
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0008D17C File Offset: 0x0008B37C
		private float CalculateMaximumWallHitPointInternal(Town town)
		{
			float num = 0f;
			int wallLevel = town.GetWallLevel();
			if (wallLevel == 1)
			{
				num += 30000f;
			}
			else if (wallLevel == 2)
			{
				num += 50000f;
			}
			else if (wallLevel == 3)
			{
				num += 67000f;
			}
			else
			{
				Debug.FailedAssert("Settlement \"" + town.Name + "\" has a wrong wall level set.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultWallHitPointCalculationModel.cs", "CalculateMaximumWallHitPointInternal", 35);
				num += -1f;
			}
			Hero governor = town.Governor;
			if (governor != null && governor.GetPerkValue(DefaultPerks.Engineering.EngineeringGuilds))
			{
				num += num * DefaultPerks.Engineering.EngineeringGuilds.SecondaryBonus;
			}
			return num;
		}
	}
}
