using System;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x02000043 RID: 67
	public class StoryModeHeroDeathProbabilityCalculationModel : HeroDeathProbabilityCalculationModel
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x00019080 File Offset: 0x00017280
		public override float CalculateHeroDeathProbability(Hero hero)
		{
			if (hero == StoryModeHeroes.ElderBrother && !StoryModeManager.Current.MainStoryLine.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.CalculateHeroDeathProbability(hero);
		}
	}
}
