using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000053 RID: 83
	public class SettlementUnderSiegeMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000653 RID: 1619 RVA: 0x00020768 File Offset: 0x0001E968
		public SettlementUnderSiegeMapNotificationItemVM(SettlementUnderSiegeMapNotification data)
			: base(data)
		{
			this._settlement = data.BesiegedSettlement;
			base.NotificationIdentifier = "settlementundersiege";
			this._onInspect = delegate
			{
				base.GoToMapPosition(this._settlement.Position);
			};
			CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventEnded));
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000207BC File Offset: 0x0001E9BC
		private void OnSiegeEventEnded(SiegeEvent obj)
		{
			if (obj.BesiegedSettlement == this._settlement)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000207D2 File Offset: 0x0001E9D2
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnSiegeEventEndedEvent.ClearListeners(this);
		}

		// Token: 0x040002B9 RID: 697
		private Settlement _settlement;
	}
}
