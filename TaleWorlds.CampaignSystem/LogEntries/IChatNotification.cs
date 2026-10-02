using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x02000350 RID: 848
	public interface IChatNotification
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003234 RID: 12852
		bool IsVisibleNotification { get; }

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06003235 RID: 12853
		ChatNotificationType NotificationType { get; }

		// Token: 0x06003236 RID: 12854
		TextObject GetNotificationText();
	}
}
