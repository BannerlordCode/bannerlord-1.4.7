using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011D RID: 285
	public class DefaultHeroDeathProbabilityCalculationModel : HeroDeathProbabilityCalculationModel
	{
		// Token: 0x06001835 RID: 6197 RVA: 0x0007540C File Offset: 0x0007360C
		public override float CalculateHeroDeathProbability(Hero hero)
		{
			return this.CalculateHeroDeathProbabilityInternal(hero);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00075418 File Offset: 0x00073618
		private float CalculateHeroDeathProbabilityInternal(Hero hero)
		{
			float num = 0f;
			if (!CampaignOptions.IsLifeDeathCycleDisabled)
			{
				int becomeOldAge = Campaign.Current.Models.AgeModel.BecomeOldAge;
				int num2 = Campaign.Current.Models.AgeModel.MaxAge - 1;
				if (hero.Age > (float)becomeOldAge)
				{
					if (hero.Age < (float)num2)
					{
						float num3 = 0.3f * ((hero.Age - (float)becomeOldAge) / (float)(Campaign.Current.Models.AgeModel.MaxAge - becomeOldAge));
						float num4 = 1f - MathF.Pow(1f - num3, 1f / (float)CampaignTime.DaysInYear);
						num += num4;
					}
					else if (hero.Age >= (float)num2)
					{
						num += 1f;
					}
				}
			}
			return num;
		}
	}
}
