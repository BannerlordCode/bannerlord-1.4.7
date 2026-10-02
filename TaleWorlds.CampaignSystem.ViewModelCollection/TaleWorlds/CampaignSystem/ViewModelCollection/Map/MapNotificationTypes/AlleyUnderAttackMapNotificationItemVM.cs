using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200003D RID: 61
	public class AlleyUnderAttackMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005D8 RID: 1496 RVA: 0x0001ECC0 File Offset: 0x0001CEC0
		public AlleyUnderAttackMapNotificationItemVM(AlleyUnderAttackMapNotification data)
			: base(data)
		{
			this._alley = data.Alley;
			base.NotificationIdentifier = "alley_under_attack";
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEnter));
			this._onInspect = delegate
			{
				base.GoToMapPosition(this._alley.Settlement.Position);
			};
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001ED14 File Offset: 0x0001CF14
		private void OnSettlementEnter(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party != null && party.IsMainParty && settlement == this._alley.Settlement)
			{
				CampaignEventDispatcher.Instance.RemoveListeners(this);
				base.ExecuteRemove();
			}
		}

		// Token: 0x04000282 RID: 642
		private Alley _alley;
	}
}
