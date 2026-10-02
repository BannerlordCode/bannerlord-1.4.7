using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000052 RID: 82
	public class SettlementOwnerChangedNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x000206C0 File Offset: 0x0001E8C0
		public SettlementOwnerChangedNotificationItemVM(SettlementOwnerChangedMapNotification data)
			: base(data)
		{
			this._settlement = data.Settlement;
			this._newOwner = data.NewOwner;
			base.NotificationIdentifier = "settlementownerchanged";
			this._onInspect = delegate
			{
				base.GoToMapPosition(this._settlement.Position);
				base.ExecuteRemove();
			};
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00020720 File Offset: 0x0001E920
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement == this._settlement && newOwner != this._newOwner)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0002073A File Offset: 0x0001E93A
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnSettlementOwnerChangedEvent.ClearListeners(this);
		}

		// Token: 0x040002B7 RID: 695
		private Settlement _settlement;

		// Token: 0x040002B8 RID: 696
		private Hero _newOwner;
	}
}
