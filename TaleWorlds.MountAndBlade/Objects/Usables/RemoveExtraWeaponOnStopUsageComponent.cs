using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003AA RID: 938
	public class RemoveExtraWeaponOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x0600352A RID: 13610 RVA: 0x000DAAC4 File Offset: 0x000D8CC4
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			if (!GameNetwork.IsClientOrReplay && !userAgent.Equipment[EquipmentIndex.ExtraWeaponSlot].IsEmpty && !Mission.Current.MissionIsEnding)
			{
				userAgent.Mission.AddTickActionMT(Mission.MissionTickAction.RemoveEquippedWeapon, userAgent, 4, 0);
			}
		}
	}
}
