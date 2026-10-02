using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000189 RID: 393
	public abstract class CharacterDevelopmentModel : MBGameModel<CharacterDevelopmentModel>
	{
		// Token: 0x06001C06 RID: 7174
		public abstract int SkillsRequiredForLevel(int level);

		// Token: 0x06001C07 RID: 7175
		public abstract int GetMaxSkillPoint();

		// Token: 0x06001C08 RID: 7176
		public abstract int GetXpRequiredForSkillLevel(int skillLevel);

		// Token: 0x06001C09 RID: 7177
		public abstract int GetSkillLevelChange(Hero hero, SkillObject skill, float skillXp);

		// Token: 0x06001C0A RID: 7178
		public abstract int GetXpAmountForSkillLevelChange(Hero hero, SkillObject skill, int skillLevelChange);

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001C0B RID: 7179
		public abstract int MaxAttribute { get; }

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001C0C RID: 7180
		public abstract int MaxFocusPerSkill { get; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001C0D RID: 7181
		public abstract int MaxSkillRequiredForEpicPerkBonus { get; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001C0E RID: 7182
		public abstract int MinSkillRequiredForEpicPerkBonus { get; }

		// Token: 0x06001C0F RID: 7183
		public abstract void GetTraitLevelForTraitXp(Hero hero, TraitObject trait, int newValue, out int traitLevel, out int traitXp);

		// Token: 0x06001C10 RID: 7184
		public abstract int GetTraitXpRequiredForTraitLevel(TraitObject trait, int traitLevel);

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001C11 RID: 7185
		public abstract int FocusPointsPerLevel { get; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001C12 RID: 7186
		public abstract int FocusPointsAtStart { get; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001C13 RID: 7187
		public abstract int AttributePointsAtStart { get; }

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001C14 RID: 7188
		public abstract int LevelsPerAttributePoint { get; }

		// Token: 0x06001C15 RID: 7189
		public abstract ExplainedNumber CalculateLearningLimit(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, SkillObject skill, bool includeDescriptions = false);

		// Token: 0x06001C16 RID: 7190
		public abstract ExplainedNumber CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, int skillValue, SkillObject skill, bool includeDescriptions = false);

		// Token: 0x06001C17 RID: 7191
		public abstract SkillObject GetNextSkillToAddFocus(Hero hero);

		// Token: 0x06001C18 RID: 7192
		public abstract CharacterAttribute GetNextAttributeToUpgrade(Hero hero);

		// Token: 0x06001C19 RID: 7193
		public abstract PerkObject GetNextPerkToChoose(Hero hero, PerkObject perk);
	}
}
