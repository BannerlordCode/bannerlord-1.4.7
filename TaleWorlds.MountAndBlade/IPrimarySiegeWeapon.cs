using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000348 RID: 840
	public interface IPrimarySiegeWeapon
	{
		// Token: 0x06002F52 RID: 12114
		bool HasCompletedAction();

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002F53 RID: 12115
		float SiegeWeaponPriority { get; }

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002F54 RID: 12116
		int OverTheWallNavMeshID { get; }

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002F55 RID: 12117
		bool HoldLadders { get; }

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002F56 RID: 12118
		bool SendLadders { get; }

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002F57 RID: 12119
		MissionObject TargetCastlePosition { get; }

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002F58 RID: 12120
		FormationAI.BehaviorSide WeaponSide { get; }

		// Token: 0x06002F59 RID: 12121
		bool GetNavmeshFaceIds(out List<int> navmeshFaceIds);
	}
}
