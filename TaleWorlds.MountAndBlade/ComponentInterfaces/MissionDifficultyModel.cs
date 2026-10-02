using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003F9 RID: 1017
	public abstract class MissionDifficultyModel : MBGameModel<MissionDifficultyModel>
	{
		// Token: 0x06003780 RID: 14208
		public abstract float GetDamageMultiplierOfCombatDifficulty(Agent victimAgent, Agent attackerAgent = null);
	}
}
