using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000049 RID: 73
	public class MercenaryOfferMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000623 RID: 1571 RVA: 0x0001FB28 File Offset: 0x0001DD28
		public MercenaryOfferMapNotificationItemVM(MercenaryOfferMapNotification data)
			: base(data)
		{
			this._offeredKingdom = data.OfferedKingdom;
			base.NotificationIdentifier = "vote";
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferedToPlayer(this._offeredKingdom);
				this._playerInspectedNotification = true;
				base.ExecuteRemove();
			};
			CampaignEvents.OnVassalOrMercenaryServiceOfferCanceledEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnVassalOrMercenaryServiceOfferCanceled));
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001FB7C File Offset: 0x0001DD7C
		private void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<MercenaryOfferMapNotification>((MercenaryOfferMapNotification x) => x.OfferedKingdom == offeredKingdom))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001FBB9 File Offset: 0x0001DDB9
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (!this._playerInspectedNotification)
			{
				IVassalAndMercenaryOfferCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IVassalAndMercenaryOfferCampaignBehavior>();
				if (campaignBehavior == null)
				{
					return;
				}
				campaignBehavior.CancelVassalOrMercenaryServiceOffer(this._offeredKingdom);
			}
		}

		// Token: 0x0400029F RID: 671
		private bool _playerInspectedNotification;

		// Token: 0x040002A0 RID: 672
		private readonly Kingdom _offeredKingdom;
	}
}
