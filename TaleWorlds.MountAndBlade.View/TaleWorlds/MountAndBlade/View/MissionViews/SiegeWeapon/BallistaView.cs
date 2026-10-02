using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews.SiegeWeapon
{
	// Token: 0x020000A3 RID: 163
	public class BallistaView : RangedSiegeWeaponView
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x00028161 File Offset: 0x00026361
		protected override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			this.UsesMouseForAiming = true;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00028171 File Offset: 0x00026371
		protected override void StartUsingWeaponCamera()
		{
			base.StartUsingWeaponCamera();
			base.MissionScreen.SetExtraCameraParameters(true, 1.5f);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0002818A File Offset: 0x0002638A
		protected override void HandleUserCameraRotation(float dt)
		{
		}
	}
}
