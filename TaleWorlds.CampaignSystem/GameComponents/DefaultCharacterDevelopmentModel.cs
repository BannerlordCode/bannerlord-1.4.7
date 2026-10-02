using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000101 RID: 257
	public class DefaultCharacterDevelopmentModel : CharacterDevelopmentModel
	{
		// Token: 0x060016D0 RID: 5840 RVA: 0x0006929F File Offset: 0x0006749F
		public DefaultCharacterDevelopmentModel()
		{
			this.InitializeSkillsRequiredForLevel();
			this.InitializeXpRequiredForSkillLevel();
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x000692D0 File Offset: 0x000674D0
		public void InitializeSkillsRequiredForLevel()
		{
			int num = 1000;
			int num2 = 1;
			this._skillsRequiredForLevel[0] = 0;
			this._skillsRequiredForLevel[1] = 1;
			for (int i = 2; i < this._skillsRequiredForLevel.Length; i++)
			{
				num2 += num;
				this._skillsRequiredForLevel[i] = num2;
				num += 1000 + num / 5;
			}
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00069324 File Offset: 0x00067524
		public void InitializeXpRequiredForSkillLevel()
		{
			int num = 30;
			this._xpRequiredForSkillLevel[0] = num;
			for (int i = 1; i < 1024; i++)
			{
				num += 10 + i;
				this._xpRequiredForSkillLevel[i] = this._xpRequiredForSkillLevel[i - 1] + num;
			}
			if (Campaign.Current.Options.AccelerationMode == GameAccelerationMode.Fast)
			{
				for (int j = 0; j < this._xpRequiredForSkillLevel.Length; j++)
				{
					this._xpRequiredForSkillLevel[j] = (int)((float)this._xpRequiredForSkillLevel[j] * 0.3f);
				}
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x000693A5 File Offset: 0x000675A5
		public override int MaxFocusPerSkill
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060016D4 RID: 5844 RVA: 0x000693A8 File Offset: 0x000675A8
		public override int MaxAttribute
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000693AC File Offset: 0x000675AC
		public override int SkillsRequiredForLevel(int level)
		{
			if (level > 62)
			{
				return Campaign.Current.Models.CharacterDevelopmentModel.GetMaxSkillPoint();
			}
			return this._skillsRequiredForLevel[level];
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x000693D0 File Offset: 0x000675D0
		public override int GetMaxSkillPoint()
		{
			return int.MaxValue;
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x000693D7 File Offset: 0x000675D7
		public override int GetXpRequiredForSkillLevel(int skillLevel)
		{
			if (skillLevel > 1024)
			{
				skillLevel = 1024;
			}
			if (skillLevel <= 0)
			{
				return 0;
			}
			return this._xpRequiredForSkillLevel[skillLevel - 1];
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000693F8 File Offset: 0x000675F8
		public override int GetSkillLevelChange(Hero hero, SkillObject skill, float skillXp)
		{
			CharacterDevelopmentModel characterDevelopmentModel = Campaign.Current.Models.CharacterDevelopmentModel;
			int num = 0;
			int skillValue = hero.GetSkillValue(skill);
			for (int i = 0; i < 1024 - skillValue; i++)
			{
				int num2 = skillValue + i;
				if (num2 < 1023)
				{
					if (skillXp < (float)characterDevelopmentModel.GetXpRequiredForSkillLevel(num2 + 1))
					{
						break;
					}
					num++;
				}
			}
			return num;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00069454 File Offset: 0x00067654
		public override int GetXpAmountForSkillLevelChange(Hero hero, SkillObject skill, int skillLevelChange)
		{
			CharacterDevelopmentModel characterDevelopmentModel = Campaign.Current.Models.CharacterDevelopmentModel;
			int skillValue = hero.GetSkillValue(skill);
			return characterDevelopmentModel.GetXpRequiredForSkillLevel(skillValue + skillLevelChange + 1) - characterDevelopmentModel.GetXpRequiredForSkillLevel(skillValue + 1);
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00069490 File Offset: 0x00067690
		public override void GetTraitLevelForTraitXp(Hero hero, TraitObject trait, int xpValue, out int traitLevel, out int clampedTraitXp)
		{
			clampedTraitXp = xpValue;
			int num = ((trait.MinValue < -1) ? (-6000) : ((trait.MinValue == -1) ? (-2500) : 0));
			int num2 = ((trait.MaxValue > 1) ? 6000 : ((trait.MaxValue == 1) ? 2500 : 0));
			if (xpValue > num2)
			{
				clampedTraitXp = num2;
			}
			else if (xpValue < num)
			{
				clampedTraitXp = num;
			}
			traitLevel = ((clampedTraitXp <= -4000) ? (-2) : ((clampedTraitXp <= -1000) ? (-1) : ((clampedTraitXp < 1000) ? 0 : ((clampedTraitXp < 4000) ? 1 : 2))));
			if (traitLevel < trait.MinValue)
			{
				traitLevel = trait.MinValue;
				return;
			}
			if (traitLevel > trait.MaxValue)
			{
				traitLevel = trait.MaxValue;
			}
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00069559 File Offset: 0x00067759
		public override int GetTraitXpRequiredForTraitLevel(TraitObject trait, int traitLevel)
		{
			if (traitLevel < -1)
			{
				return -4000;
			}
			if (traitLevel == -1)
			{
				return -1000;
			}
			if (traitLevel == 0)
			{
				return 0;
			}
			if (traitLevel != 1)
			{
				return 4000;
			}
			return 1000;
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x00069583 File Offset: 0x00067783
		public override int AttributePointsAtStart
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00069587 File Offset: 0x00067787
		public override int LevelsPerAttributePoint
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x0006958A File Offset: 0x0006778A
		public override int FocusPointsPerLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x0006958D File Offset: 0x0006778D
		public override int FocusPointsAtStart
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x00069590 File Offset: 0x00067790
		public override int MaxSkillRequiredForEpicPerkBonus
		{
			get
			{
				return 250;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00069597 File Offset: 0x00067797
		public override int MinSkillRequiredForEpicPerkBonus
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x000695A0 File Offset: 0x000677A0
		public override ExplainedNumber CalculateLearningLimit(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, SkillObject skill, bool includeDescriptions = false)
		{
			float num = 0f;
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			foreach (CharacterAttribute characterAttribute in skill.Attributes)
			{
				num += (float)characterAttributes.GetPropertyValue(characterAttribute);
			}
			float num2 = num / (float)skill.Attributes.Length;
			explainedNumber.Add(Math.Max(0f, (num2 - 1f) * 10f), DefaultCharacterDevelopmentModel._attributeEffectText, null);
			explainedNumber.Add((float)(focusValue * 30), DefaultCharacterDevelopmentModel._skillFocusText, null);
			explainedNumber.LimitMin(0f);
			return explainedNumber;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00069644 File Offset: 0x00067844
		public override ExplainedNumber CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, int skillValue, SkillObject skill, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1.25f, includeDescriptions, null);
			float num = 0f;
			foreach (CharacterAttribute characterAttribute in skill.Attributes)
			{
				num += (float)characterAttributes.GetPropertyValue(characterAttribute);
			}
			float num2 = num / (float)skill.Attributes.Length;
			explainedNumber.AddFactor(0.4f * num2, DefaultCharacterDevelopmentModel._attributeEffectText);
			int num3 = MathF.Round(Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(characterAttributes, focusValue, skill, false).ResultNumber);
			explainedNumber.AddFactor((float)focusValue * 1f, DefaultCharacterDevelopmentModel._skillFocusText);
			if (skillValue > num3)
			{
				int num4 = skillValue - num3;
				explainedNumber.AddFactor(-1f - 0.1f * (float)num4, DefaultCharacterDevelopmentModel._overLimitText);
			}
			explainedNumber.LimitMin(0f);
			return explainedNumber;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00069728 File Offset: 0x00067928
		public override SkillObject GetNextSkillToAddFocus(Hero hero)
		{
			SkillObject skillObject = null;
			float num = float.MinValue;
			foreach (SkillObject skillObject2 in Skills.All)
			{
				if (hero.HeroDeveloper.CanAddFocusToSkill(skillObject2))
				{
					int focus = hero.HeroDeveloper.GetFocus(skillObject2);
					float num2 = (float)hero.GetSkillValue(skillObject2) - Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(hero.CharacterAttributes, focus, skillObject2, false).ResultNumber;
					if (num2 > num)
					{
						num = num2;
						skillObject = skillObject2;
					}
				}
			}
			return skillObject;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x000697D4 File Offset: 0x000679D4
		public override CharacterAttribute GetNextAttributeToUpgrade(Hero hero)
		{
			CharacterAttribute characterAttribute = null;
			float num = float.MinValue;
			using (List<CharacterAttribute>.Enumerator enumerator = Attributes.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterAttribute currentAttribute = enumerator.Current;
					int attributeValue = hero.GetAttributeValue(currentAttribute);
					if (attributeValue < Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute)
					{
						float num2 = 0f;
						if (attributeValue == 0)
						{
							num2 = float.MaxValue;
						}
						else
						{
							float num3 = 0f;
							List<SkillObject> list = Skills.All.Where<SkillObject>((SkillObject skill) => skill.Attributes.Contains(currentAttribute)).ToList<SkillObject>();
							foreach (SkillObject skillObject in list)
							{
								num3 += MathF.Max(0f, (float)(75 + hero.GetSkillValue(skillObject)) - Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(hero.CharacterAttributes, hero.HeroDeveloper.GetFocus(skillObject), skillObject, false).ResultNumber);
							}
							num2 += num3 / (float)list.Count;
							int num4 = 1;
							foreach (CharacterAttribute characterAttribute2 in Attributes.All)
							{
								if (characterAttribute2 != currentAttribute)
								{
									int attributeValue2 = hero.GetAttributeValue(characterAttribute2);
									if (num4 < attributeValue2)
									{
										num4 = attributeValue2;
									}
								}
							}
							float num5 = MathF.Sqrt((float)num4 / (float)attributeValue);
							num2 *= num5;
						}
						if (num2 > num)
						{
							num = num2;
							characterAttribute = currentAttribute;
						}
					}
				}
			}
			return characterAttribute;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x000699DC File Offset: 0x00067BDC
		public override PerkObject GetNextPerkToChoose(Hero hero, PerkObject perk)
		{
			PerkObject perkObject = perk;
			if (perk.AlternativePerk != null && MBRandom.RandomFloat < 0.5f)
			{
				perkObject = perk.AlternativePerk;
			}
			return perkObject;
		}

		// Token: 0x04000796 RID: 1942
		private const int MaxCharacterLevels = 62;

		// Token: 0x04000797 RID: 1943
		private const int SkillPointsAtLevel1 = 1;

		// Token: 0x04000798 RID: 1944
		private const int SkillPointsGainNeededInitialValue = 1000;

		// Token: 0x04000799 RID: 1945
		private const int SkillPointsGainNeededIncreasePerLevel = 1000;

		// Token: 0x0400079A RID: 1946
		private readonly int[] _skillsRequiredForLevel = new int[63];

		// Token: 0x0400079B RID: 1947
		private const int FocusPointsPerLevelConst = 1;

		// Token: 0x0400079C RID: 1948
		private const int LevelsPerAttributePointConst = 4;

		// Token: 0x0400079D RID: 1949
		private const int FocusPointsAtStartConst = 5;

		// Token: 0x0400079E RID: 1950
		private const int AttributePointsAtStartConst = 15;

		// Token: 0x0400079F RID: 1951
		private const int MaxSkillLevels = 1024;

		// Token: 0x040007A0 RID: 1952
		private readonly int[] _xpRequiredForSkillLevel = new int[1024];

		// Token: 0x040007A1 RID: 1953
		private const int XpRequirementForFirstLevel = 30;

		// Token: 0x040007A2 RID: 1954
		private const int MaxSkillPoint = 2147483647;

		// Token: 0x040007A3 RID: 1955
		private const float BaseLearningRate = 1.25f;

		// Token: 0x040007A4 RID: 1956
		private const int TraitThreshold2 = 4000;

		// Token: 0x040007A5 RID: 1957
		private const int TraitMaxValue1 = 2500;

		// Token: 0x040007A6 RID: 1958
		private const int TraitThreshold1 = 1000;

		// Token: 0x040007A7 RID: 1959
		private const int TraitMaxValue2 = 6000;

		// Token: 0x040007A8 RID: 1960
		private const int SkillLevelVariant = 10;

		// Token: 0x040007A9 RID: 1961
		private static readonly TextObject _attributeEffectText = new TextObject("{=jlrvzwFb}Attribute Effect", null);

		// Token: 0x040007AA RID: 1962
		private static readonly TextObject _skillFocusText = new TextObject("{=MRktqZwu}Skill Focus", null);

		// Token: 0x040007AB RID: 1963
		private static readonly TextObject _overLimitText = new TextObject("{=bcA7ZuyO}Learning Limit Exceeded", null);
	}
}
