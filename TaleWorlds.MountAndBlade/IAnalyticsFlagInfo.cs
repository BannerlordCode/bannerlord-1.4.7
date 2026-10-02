using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000263 RID: 611
	public interface IAnalyticsFlagInfo : IMissionBehavior
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002269 RID: 8809
		MBReadOnlyList<FlagCapturePoint> AllCapturePoints { get; }

		// Token: 0x0600226A RID: 8810
		Team GetFlagOwnerTeam(FlagCapturePoint flag);
	}
}
