using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004B RID: 75
	public class PartyLeaderChangeNotificationVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000629 RID: 1577 RVA: 0x0001FD30 File Offset: 0x0001DF30
		public PartyLeaderChangeNotificationVM(PartyLeaderChangeNotification data)
			: base(data)
		{
			this._party = data.Party;
			base.NotificationIdentifier = "death";
			this._onInspect = delegate
			{
				InformationManager.ShowInquiry(new InquiryData(this._decisionPopupTitleText.ToString(), this._partyLeaderChangePopupText.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					INavigationHandler navigationHandler = base.NavigationHandler;
					if (navigationHandler == null)
					{
						return;
					}
					navigationHandler.OpenClan(this._party.Party);
				}, null, "", 0f, null, null, null), false, false);
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				this._playerInspectedNotification = true;
				base.ExecuteRemove();
			};
			CampaignEvents.OnPartyLeaderChangeOfferCanceledEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyLeaderChangeOfferCanceled));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001FDBD File Offset: 0x0001DFBD
		private void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
			this.CheckAndExecuteRemove(party);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001FDC6 File Offset: 0x0001DFC6
		private void OnMobilePartyDestroyed(MobileParty party, PartyBase destroyerParty)
		{
			this.CheckAndExecuteRemove(party);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
		private void CheckAndExecuteRemove(MobileParty party)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<PartyLeaderChangeNotification>((PartyLeaderChangeNotification x) => x.Party == party))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001FE0D File Offset: 0x0001E00D
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (!this._playerInspectedNotification)
			{
				CampaignEventDispatcher.Instance.OnPartyLeaderChangeOfferCanceled(this._party);
			}
		}

		// Token: 0x040002A2 RID: 674
		private bool _playerInspectedNotification;

		// Token: 0x040002A3 RID: 675
		private readonly MobileParty _party;

		// Token: 0x040002A4 RID: 676
		private TextObject _decisionPopupTitleText = new TextObject("{=nFl0ufe3}A party without a leader", null);

		// Token: 0x040002A5 RID: 677
		private TextObject _partyLeaderChangePopupText = new TextObject("{=OMqHwpXF}One of your parties has lost its leader. It will disband after a day has passed. You can assign a new clan member to lead it, if you wish to keep the party.{newline}{newline}Do you want to assign a new leader?", null);
	}
}
