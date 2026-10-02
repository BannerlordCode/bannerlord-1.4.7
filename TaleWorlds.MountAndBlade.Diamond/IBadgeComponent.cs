using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011C RID: 284
	public interface IBadgeComponent
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600065D RID: 1629
		Dictionary<ValueTuple<PlayerId, string, string>, int> DataDictionary { get; }

		// Token: 0x0600065E RID: 1630
		void OnPlayerJoin(PlayerData playerData);

		// Token: 0x0600065F RID: 1631
		void OnStartingNextBattle();
	}
}
