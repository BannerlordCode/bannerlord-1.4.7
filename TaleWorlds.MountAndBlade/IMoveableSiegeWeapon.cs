using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000352 RID: 850
	public interface IMoveableSiegeWeapon
	{
		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060030B9 RID: 12473
		SiegeWeaponMovementComponent MovementComponent { get; }

		// Token: 0x060030BA RID: 12474
		void HighlightPath();

		// Token: 0x060030BB RID: 12475
		void SwitchGhostEntityMovementMode(bool isGhostEnabled);

		// Token: 0x060030BC RID: 12476
		MatrixFrame GetInitialFrame();
	}
}
