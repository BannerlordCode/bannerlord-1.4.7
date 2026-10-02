using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000048 RID: 72
	public class MarriageOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600061F RID: 1567 RVA: 0x0001FA20 File Offset: 0x0001DC20
		public MarriageOfferNotificationItemVM(MarriageOfferMapNotification data)
			: base(data)
		{
			this._suitor = data.Suitor;
			this._maiden = data.Maiden;
			base.NotificationIdentifier = "marriage";
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferedToPlayer(this._suitor, this._maiden);
				this._playerInspectedNotification = true;
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				base.ExecuteRemove();
			};
			CampaignEvents.OnMarriageOfferCanceledEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferCanceled));
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001FA80 File Offset: 0x0001DC80
		private void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<MarriageOfferMapNotification>((MarriageOfferMapNotification x) => x.Suitor == suitor && x.Maiden == maiden))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001FAC4 File Offset: 0x0001DCC4
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (!this._playerInspectedNotification)
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._suitor, this._maiden);
			}
		}

		// Token: 0x0400029C RID: 668
		private bool _playerInspectedNotification;

		// Token: 0x0400029D RID: 669
		private readonly Hero _suitor;

		// Token: 0x0400029E RID: 670
		private readonly Hero _maiden;
	}
}
