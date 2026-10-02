using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000283 RID: 643
	public class EquipmentControllerLeaveLogic : MissionLogic
	{
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x00080D23 File Offset: 0x0007EF23
		// (set) Token: 0x060023E2 RID: 9186 RVA: 0x00080D2B File Offset: 0x0007EF2B
		public bool IsEquipmentSelectionActive { get; private set; }

		// Token: 0x060023E3 RID: 9187 RVA: 0x00080D34 File Offset: 0x0007EF34
		public void SetIsEquipmentSelectionActive(bool isActive)
		{
			this.IsEquipmentSelectionActive = isActive;
			Debug.Print("IsEquipmentSelectionActive: " + isActive.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x00080D5F File Offset: 0x0007EF5F
		public override InquiryData OnEndMissionRequest(out bool canLeave)
		{
			canLeave = !this.IsEquipmentSelectionActive;
			return null;
		}
	}
}
