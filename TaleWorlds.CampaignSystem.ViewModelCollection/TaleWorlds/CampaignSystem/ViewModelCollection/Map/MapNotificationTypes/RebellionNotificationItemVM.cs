using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000051 RID: 81
	public class RebellionNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x00020664 File Offset: 0x0001E864
		public RebellionNotificationItemVM(SettlementRebellionMapNotification data)
			: base(data)
		{
			this._settlement = data.RebelliousSettlement;
			this._onInspect = (this._onInspectAction = delegate
			{
				base.GoToMapPosition(this._settlement.Position);
			});
			base.NotificationIdentifier = "rebellion";
		}

		// Token: 0x040002B5 RID: 693
		private Settlement _settlement;

		// Token: 0x040002B6 RID: 694
		protected Action _onInspectAction;
	}
}
