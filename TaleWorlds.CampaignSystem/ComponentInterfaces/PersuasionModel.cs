using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019B RID: 411
	public abstract class PersuasionModel : MBGameModel<PersuasionModel>
	{
		// Token: 0x06001C7B RID: 7291
		public abstract int GetSkillXpFromPersuasion(PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient);

		// Token: 0x06001C7C RID: 7292
		public abstract void GetChances(PersuasionOptionArgs optionArgs, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance, float difficultyMultiplier);

		// Token: 0x06001C7D RID: 7293
		public abstract void GetEffectChances(PersuasionOptionArgs option, out float moveToNextStageChance, out float blockRandomOptionChance, float difficultyMultiplier);

		// Token: 0x06001C7E RID: 7294
		public abstract PersuasionArgumentStrength GetArgumentStrengthBasedOnTargetTraits(CharacterObject character, Tuple<TraitObject, int>[] traitCorrelation);

		// Token: 0x06001C7F RID: 7295
		public abstract float GetDifficulty(PersuasionDifficulty difficulty);

		// Token: 0x06001C80 RID: 7296
		public abstract float CalculateInitialPersuasionProgress(CharacterObject character, float goalValue, float successValue);

		// Token: 0x06001C81 RID: 7297
		public abstract float CalculatePersuasionGoalValue(CharacterObject oneToOneConversationCharacter, float successValue);
	}
}
