using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036D RID: 877
	internal class DropExtraWeaponOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x06003243 RID: 12867 RVA: 0x000CD324 File Offset: 0x000CB524
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			if (isSuccessful && !GameNetwork.IsClientOrReplay && !userAgent.Equipment[EquipmentIndex.ExtraWeaponSlot].IsEmpty && !Mission.Current.MissionIsEnding)
			{
				userAgent.Mission.AddTickAction(Mission.MissionTickAction.DropItem, userAgent, 4, 0);
			}
		}
	}
}
