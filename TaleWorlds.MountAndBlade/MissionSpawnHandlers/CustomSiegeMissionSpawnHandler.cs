using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003CA RID: 970
	public class CustomSiegeMissionSpawnHandler : CustomMissionSpawnHandler
	{
		// Token: 0x06003625 RID: 13861 RVA: 0x000DF825 File Offset: 0x000DDA25
		public CustomSiegeMissionSpawnHandler(IBattleCombatant defenderBattleCombatant, IBattleCombatant attackerBattleCombatant, bool spawnWithHorses)
		{
			this._battleCombatants = new CustomBattleCombatant[]
			{
				(CustomBattleCombatant)defenderBattleCombatant,
				(CustomBattleCombatant)attackerBattleCombatant
			};
			this._spawnWithHorses = spawnWithHorses;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000DF854 File Offset: 0x000DDA54
		public override void AfterStart()
		{
			int numberOfHealthyMembers = this._battleCombatants[0].NumberOfHealthyMembers;
			int numberOfHealthyMembers2 = this._battleCombatants[1].NumberOfHealthyMembers;
			int num = numberOfHealthyMembers;
			int num2 = numberOfHealthyMembers2;
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, this._spawnWithHorses);
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, this._spawnWithHorses);
			MissionSpawnSettings missionSpawnSettings = CustomMissionSpawnHandler.CreateCustomBattleWaveSpawnSettings();
			this._missionAgentSpawnLogic.InitWithSinglePhase(numberOfHealthyMembers, numberOfHealthyMembers2, num, num2, false, false, in missionSpawnSettings);
		}

		// Token: 0x04001737 RID: 5943
		private CustomBattleCombatant[] _battleCombatants;

		// Token: 0x04001738 RID: 5944
		private bool _spawnWithHorses;
	}
}
