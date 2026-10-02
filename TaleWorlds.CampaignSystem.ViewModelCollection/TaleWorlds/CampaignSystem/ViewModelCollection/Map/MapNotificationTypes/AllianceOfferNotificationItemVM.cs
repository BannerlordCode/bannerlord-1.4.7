using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200003E RID: 62
	public class AllianceOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005DB RID: 1499 RVA: 0x0001ED58 File Offset: 0x0001CF58
		public AllianceOfferNotificationItemVM(AllianceOfferMapNotification data)
			: base(data)
		{
			AllianceOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = false;
			this._offeringKingdom = data.OfferingKingdom;
			this._onInspect = delegate
			{
				bool flag = false;
				if (data != null && data.IsValid() && Clan.PlayerClan.Kingdom != null)
				{
					TextObject textObject;
					flag = new StartAllianceDecision(Clan.PlayerClan, <>4__this._offeringKingdom).CanMakeDecision(out textObject, false);
				}
				if (flag)
				{
					IAllianceCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
					if (campaignBehavior != null)
					{
						campaignBehavior.OnAllianceOfferedToPlayer(data.OfferingKingdom);
					}
					<>4__this.RemoveAllianceOfferNotification(false);
					return;
				}
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=4vPm9bFW}This alliance offer is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				<>4__this.RemoveAllianceOfferNotification(false);
			};
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnAllianceStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceStarted));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001EE16 File Offset: 0x0001D016
		private void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
			if ((kingdom1 == Clan.PlayerClan.Kingdom && kingdom2 == this._offeringKingdom) || (kingdom2 == Clan.PlayerClan.Kingdom && kingdom1 == this._offeringKingdom))
			{
				this.RemoveAllianceOfferNotification(false);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001EE4B File Offset: 0x0001D04B
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			if (kingdom == Clan.PlayerClan.Kingdom || this._offeringKingdom == kingdom)
			{
				this.RemoveAllianceOfferNotification(false);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001EE6A File Offset: 0x0001D06A
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemoveAllianceOfferNotification(false);
				return;
			}
			if (newKingdom == Clan.PlayerClan.Kingdom)
			{
				this.RemoveAllianceOfferNotification(true);
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001EE90 File Offset: 0x0001D090
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._offeringKingdom) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._offeringKingdom))
			{
				this.RemoveAllianceOfferNotification(false);
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001EEC5 File Offset: 0x0001D0C5
		private void RemoveAllianceOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001EED4 File Offset: 0x0001D0D4
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Clans.Count > 1 && Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				StartAllianceDecision startAllianceDecision2;
				return (startAllianceDecision2 = s as StartAllianceDecision) != null && startAllianceDecision2.KingdomToStartAllianceWith == this._offeringKingdom;
			}) == null)
			{
				StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Clan.PlayerClan, this._offeringKingdom);
				TextObject textObject;
				if (startAllianceDecision.CanMakeDecision(out textObject, false))
				{
					Clan.PlayerClan.Kingdom.AddDecision(startAllianceDecision, true);
				}
			}
		}

		// Token: 0x04000283 RID: 643
		private readonly Kingdom _offeringKingdom;

		// Token: 0x04000284 RID: 644
		private bool _shouldDecisionBeCreatedOnClosed;
	}
}
