using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200001B RID: 27
	public class MissionPlayerToggledOrderViewEvent : EventBase
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005D18 File Offset: 0x00003F18
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00005D20 File Offset: 0x00003F20
		public bool IsOrderEnabled { get; private set; }

		// Token: 0x060000B2 RID: 178 RVA: 0x00005D29 File Offset: 0x00003F29
		public MissionPlayerToggledOrderViewEvent(bool newIsEnabledState)
		{
			this.IsOrderEnabled = newIsEnabledState;
		}
	}
}
