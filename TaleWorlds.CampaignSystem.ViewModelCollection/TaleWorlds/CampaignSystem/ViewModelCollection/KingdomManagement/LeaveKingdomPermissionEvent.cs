using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x02000066 RID: 102
	public class LeaveKingdomPermissionEvent : EventBase
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x00024BBB File Offset: 0x00022DBB
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x00024BC3 File Offset: 0x00022DC3
		public Action<bool, TextObject> IsLeaveKingdomPossbile { get; private set; }

		// Token: 0x060007F6 RID: 2038 RVA: 0x00024BCC File Offset: 0x00022DCC
		public LeaveKingdomPermissionEvent(Action<bool, TextObject> isLeaveKingdomPossbile)
		{
			this.IsLeaveKingdomPossbile = isLeaveKingdomPossbile;
		}
	}
}
