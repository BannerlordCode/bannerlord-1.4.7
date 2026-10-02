using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SandBox
{
	// Token: 0x02000010 RID: 16
	public class BoostSkillCheatGroup : GameplayCheatGroup
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003900 File Offset: 0x00001B00
		public override IEnumerable<GameplayCheatBase> GetCheats()
		{
			foreach (SkillObject skillObject in Skills.All)
			{
				yield return new BoostSkillCheatGroup.BoostSkillCheeat(skillObject);
			}
			List<SkillObject>.Enumerator enumerator = default(List<SkillObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003909 File Offset: 0x00001B09
		public override TextObject GetName()
		{
			return new TextObject("{=SFn4UFd4}Boost Skill", null);
		}

		// Token: 0x02000112 RID: 274
		public class BoostSkillCheeat : GameplayCheatItem
		{
			// Token: 0x06000D79 RID: 3449 RVA: 0x000617D3 File Offset: 0x0005F9D3
			public BoostSkillCheeat(SkillObject skillToBoost)
			{
				this._skillToBoost = skillToBoost;
			}

			// Token: 0x06000D7A RID: 3450 RVA: 0x000617E4 File Offset: 0x0005F9E4
			public override void ExecuteCheat()
			{
				int num = 50;
				if (Hero.MainHero.GetSkillValue(this._skillToBoost) + num > 330)
				{
					num = 330 - Hero.MainHero.GetSkillValue(this._skillToBoost);
				}
				Hero.MainHero.HeroDeveloper.ChangeSkillLevel(this._skillToBoost, num, false);
			}

			// Token: 0x06000D7B RID: 3451 RVA: 0x0006183B File Offset: 0x0005FA3B
			public override TextObject GetName()
			{
				return this._skillToBoost.GetName();
			}

			// Token: 0x040005D4 RID: 1492
			private readonly SkillObject _skillToBoost;
		}
	}
}
