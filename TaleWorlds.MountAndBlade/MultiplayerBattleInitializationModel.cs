using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000203 RID: 515
	public class MultiplayerBattleInitializationModel : BattleInitializationModel
	{
		// Token: 0x06001E00 RID: 7680 RVA: 0x00067903 File Offset: 0x00065B03
		public override List<FormationClass> GetAllAvailableTroopTypes()
		{
			return new List<FormationClass>();
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0006790A File Offset: 0x00065B0A
		protected override bool CanPlayerSideDeployWithOrderOfBattleAux()
		{
			return false;
		}
	}
}
