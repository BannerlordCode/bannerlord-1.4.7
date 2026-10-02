using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027A RID: 634
	public class BattleMissionStarterLogic : MissionLogic
	{
		// Token: 0x06002358 RID: 9048 RVA: 0x0007DBFC File Offset: 0x0007BDFC
		public BattleMissionStarterLogic()
		{
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0007DC04 File Offset: 0x0007BE04
		public BattleMissionStarterLogic(IMissionTroopSupplier defenderTroopSupplier = null, IMissionTroopSupplier attackerTroopSupplier = null)
		{
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0007DC0C File Offset: 0x0007BE0C
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}
	}
}
