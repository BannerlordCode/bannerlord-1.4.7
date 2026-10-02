using System;
using StoryMode.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x02000042 RID: 66
	public class StoryModeGenericXpModel : GenericXpModel
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x00019049 File Offset: 0x00017249
		public override float GetXpMultiplier(Hero hero)
		{
			if (((hero != null) ? hero.CurrentSettlement : null) != null && hero.CurrentSettlement.IsTrainingField())
			{
				return 0f;
			}
			return base.BaseModel.GetXpMultiplier(hero);
		}
	}
}
