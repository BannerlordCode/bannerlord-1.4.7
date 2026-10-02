using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000C1 RID: 193
	public class SettlementOverlayLeaveCharacterPermissionEvent : EventBase
	{
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x0004DB93 File Offset: 0x0004BD93
		// (set) Token: 0x06001328 RID: 4904 RVA: 0x0004DB9B File Offset: 0x0004BD9B
		public Action<bool, TextObject> IsLeaveAvailable { get; private set; }

		// Token: 0x06001329 RID: 4905 RVA: 0x0004DBA4 File Offset: 0x0004BDA4
		public SettlementOverlayLeaveCharacterPermissionEvent(Action<bool, TextObject> isLeaveAvailable)
		{
			this.IsLeaveAvailable = isLeaveAvailable;
		}
	}
}
