using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FD RID: 1021
	public abstract class BattleSpawnModel : MBGameModel<BattleSpawnModel>
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x000E4ED7 File Offset: 0x000E30D7
		public virtual void OnMissionStart()
		{
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000E4ED9 File Offset: 0x000E30D9
		public virtual void OnMissionEnd()
		{
		}

		// Token: 0x0600379C RID: 14236
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public abstract List<ValueTuple<IAgentOriginBase, int>> GetInitialSpawnAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins);

		// Token: 0x0600379D RID: 14237
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public abstract List<ValueTuple<IAgentOriginBase, int>> GetReinforcementAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins);
	}
}
