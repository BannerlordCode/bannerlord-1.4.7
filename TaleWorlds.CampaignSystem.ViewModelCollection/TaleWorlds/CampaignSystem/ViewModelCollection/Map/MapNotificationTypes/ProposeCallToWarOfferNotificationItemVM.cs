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
	// Token: 0x0200004E RID: 78
	public class ProposeCallToWarOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x00020250 File Offset: 0x0001E450
		public ProposeCallToWarOfferNotificationItemVM(ProposeCallToWarOfferMapNotification data)
			: base(data)
		{
			ProposeCallToWarOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = false;
			this._offeredKingdom = data.OfferedKingdom;
			this._kingdomToCallToWarAgainst = data.KingdomToCallToWarAgainst;
			this._onInspect = delegate
			{
				bool flag = false;
				if (data != null && data.IsValid() && Clan.PlayerClan.Kingdom != null)
				{
					TextObject textObject;
					flag = new ProposeCallToWarAgreementDecision(Clan.PlayerClan, <>4__this._offeredKingdom, <>4__this._kingdomToCallToWarAgainst).CanMakeDecision(out textObject, false);
				}
				if (flag)
				{
					IAllianceCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
					if (campaignBehavior != null)
					{
						campaignBehavior.OnCallToWarAgreementProposedByPlayer(data.OfferedKingdom, data.KingdomToCallToWarAgainst);
					}
					<>4__this.RemoveProposeCallToWarOfferNotification(false);
					return;
				}
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=oGgjuQav}This call to war offer is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				<>4__this.RemoveProposeCallToWarOfferNotification(false);
			};
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEnded));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00020336 File Offset: 0x0001E536
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((faction1 == this._offeredKingdom && faction2 == this._kingdomToCallToWarAgainst) || (faction2 == this._offeredKingdom && faction1 == this._kingdomToCallToWarAgainst))
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00020363 File Offset: 0x0001E563
		private void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			if ((kingdom1 == Clan.PlayerClan.Kingdom && kingdom2 == this._offeredKingdom) || (kingdom2 == Clan.PlayerClan.Kingdom && kingdom1 == this._offeredKingdom))
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00020398 File Offset: 0x0001E598
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			if (kingdom == Clan.PlayerClan.Kingdom || this._offeredKingdom == kingdom || this._kingdomToCallToWarAgainst == kingdom)
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000203C0 File Offset: 0x0001E5C0
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemoveProposeCallToWarOfferNotification(false);
				return;
			}
			if (newKingdom == Clan.PlayerClan.Kingdom)
			{
				this.RemoveProposeCallToWarOfferNotification(true);
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000203E6 File Offset: 0x0001E5E6
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._kingdomToCallToWarAgainst) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._kingdomToCallToWarAgainst))
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0002041B File Offset: 0x0001E61B
		private void RemoveProposeCallToWarOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002042C File Offset: 0x0001E62C
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Clans.Count > 1 && Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision2;
				return (proposeCallToWarAgreementDecision2 = s as ProposeCallToWarAgreementDecision) != null && proposeCallToWarAgreementDecision2.CalledKingdom == this._offeredKingdom;
			}) == null)
			{
				ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision = new ProposeCallToWarAgreementDecision(Clan.PlayerClan, this._offeredKingdom, this._kingdomToCallToWarAgainst);
				TextObject textObject;
				if (proposeCallToWarAgreementDecision.CanMakeDecision(out textObject, false))
				{
					Clan.PlayerClan.Kingdom.AddDecision(proposeCallToWarAgreementDecision, true);
				}
			}
		}

		// Token: 0x040002AB RID: 683
		private readonly Kingdom _offeredKingdom;

		// Token: 0x040002AC RID: 684
		private readonly Kingdom _kingdomToCallToWarAgainst;

		// Token: 0x040002AD RID: 685
		private bool _shouldDecisionBeCreatedOnClosed;
	}
}
