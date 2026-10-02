using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000270 RID: 624
	// (Invoke) Token: 0x060022FB RID: 8955
	public delegate void PlayerTurnToChooseFormationToLeadEvent(Dictionary<int, Agent> lockedFormationIndicesAndSergeants, List<int> remainingFormationIndices);
}
