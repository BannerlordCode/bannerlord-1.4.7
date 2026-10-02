using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003C9 RID: 969
	public class CustomSallyOutMissionController : SallyOutMissionController
	{
		// Token: 0x06003623 RID: 13859 RVA: 0x000DF7DE File Offset: 0x000DD9DE
		public CustomSallyOutMissionController(IBattleCombatant defenderBattleCombatant, IBattleCombatant attackerBattleCombatant)
			: base(true)
		{
			this._battleCombatants = new CustomBattleCombatant[]
			{
				(CustomBattleCombatant)defenderBattleCombatant,
				(CustomBattleCombatant)attackerBattleCombatant
			};
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000DF805 File Offset: 0x000DDA05
		protected override void GetInitialTroopCounts(out int besiegedTotalTroopCount, out int besiegerTotalTroopCount)
		{
			besiegedTotalTroopCount = this._battleCombatants[0].NumberOfHealthyMembers;
			besiegerTotalTroopCount = this._battleCombatants[1].NumberOfHealthyMembers;
		}

		// Token: 0x04001736 RID: 5942
		private readonly CustomBattleCombatant[] _battleCombatants;
	}
}
