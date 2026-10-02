using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x0200039D RID: 925
	public class AnimalSpawnSettings : ScriptComponentBehavior
	{
		// Token: 0x060034C8 RID: 13512 RVA: 0x000D9541 File Offset: 0x000D7741
		public static void CheckAndSetAnimalAgentFlags(GameEntity spawnEntity, Agent animalAgent)
		{
			if (spawnEntity.HasScriptOfType<AnimalSpawnSettings>() && spawnEntity.GetFirstScriptOfType<AnimalSpawnSettings>().DisableWandering)
			{
				animalAgent.SetAgentFlags(animalAgent.GetAgentFlags() & ~AgentFlag.CanWander);
			}
		}

		// Token: 0x04001667 RID: 5735
		public bool DisableWandering = true;
	}
}
