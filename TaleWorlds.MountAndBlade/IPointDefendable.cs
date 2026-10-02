using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000155 RID: 341
	public interface IPointDefendable
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060011E9 RID: 4585
		IEnumerable<DefencePoint> DefencePoints { get; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060011EA RID: 4586
		FormationAI.BehaviorSide DefenseSide { get; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060011EB RID: 4587
		WorldFrame MiddleFrame { get; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060011EC RID: 4588
		WorldFrame DefenseWaitFrame { get; }
	}
}
