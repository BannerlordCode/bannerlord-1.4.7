using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200007E RID: 126
	public interface IBattleObserver
	{
		// Token: 0x0600087A RID: 2170
		void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0);

		// Token: 0x0600087B RID: 2171
		void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character);

		// Token: 0x0600087C RID: 2172
		void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject skill);

		// Token: 0x0600087D RID: 2173
		void BattleResultsReady();
	}
}
