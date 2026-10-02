using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000050 RID: 80
	public class RansomNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600064A RID: 1610 RVA: 0x000205B4 File Offset: 0x0001E7B4
		public RansomNotificationItemVM(RansomOfferMapNotification data)
			: base(data)
		{
			RansomNotificationItemVM <>4__this = this;
			this._hero = data.CaptiveHero;
			this._onInspect = delegate
			{
				<>4__this._playerInspectedNotification = true;
				CampaignEventDispatcher.Instance.OnRansomOfferedToPlayer(data.CaptiveHero);
				<>4__this.ExecuteRemove();
			};
			CampaignEvents.OnRansomOfferCancelledEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnRansomOfferCancelled));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00020626 File Offset: 0x0001E826
		private void OnRansomOfferCancelled(Hero captiveHero)
		{
			if (captiveHero == this._hero)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00020637 File Offset: 0x0001E837
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnRansomOfferCancelledEvent.ClearListeners(this);
			if (!this._playerInspectedNotification)
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._hero);
			}
		}

		// Token: 0x040002B3 RID: 691
		private bool _playerInspectedNotification;

		// Token: 0x040002B4 RID: 692
		private Hero _hero;
	}
}
