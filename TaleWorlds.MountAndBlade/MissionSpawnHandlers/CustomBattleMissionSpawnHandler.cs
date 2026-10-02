using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003C7 RID: 967
	public class CustomBattleMissionSpawnHandler : CustomMissionSpawnHandler
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x000DF70A File Offset: 0x000DD90A
		public CustomBattleMissionSpawnHandler(CustomBattleCombatant defenderParty, CustomBattleCombatant attackerParty)
		{
			this._defenderParty = defenderParty;
			this._attackerParty = attackerParty;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000DF720 File Offset: 0x000DD920
		public override void AfterStart()
		{
			int numberOfHealthyMembers = this._defenderParty.NumberOfHealthyMembers;
			int numberOfHealthyMembers2 = this._attackerParty.NumberOfHealthyMembers;
			int num = numberOfHealthyMembers;
			int num2 = numberOfHealthyMembers2;
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, true);
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, true);
			MissionSpawnSettings missionSpawnSettings = CustomMissionSpawnHandler.CreateCustomBattleWaveSpawnSettings();
			this._missionAgentSpawnLogic.InitWithSinglePhase(numberOfHealthyMembers, numberOfHealthyMembers2, num, num2, true, true, in missionSpawnSettings);
		}

		// Token: 0x04001733 RID: 5939
		private CustomBattleCombatant _defenderParty;

		// Token: 0x04001734 RID: 5940
		private CustomBattleCombatant _attackerParty;
	}
}
