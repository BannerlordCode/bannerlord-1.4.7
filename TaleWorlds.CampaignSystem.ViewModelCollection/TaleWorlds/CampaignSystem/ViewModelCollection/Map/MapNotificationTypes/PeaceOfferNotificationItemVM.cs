using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004D RID: 77
	public class PeaceOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000635 RID: 1589 RVA: 0x0001FFEC File Offset: 0x0001E1EC
		public PeaceOfferNotificationItemVM(PeaceOfferMapNotification data)
			: base(data)
		{
			PeaceOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = true;
			this._opponentFaction = data.OpponentFaction;
			this._tributeAmount = data.TributeAmount;
			this._tributeDurationInDays = data.TributeDurationInDays;
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnPeaceOfferedToPlayer(data.OpponentFaction, data.TributeAmount, data.TributeDurationInDays);
				<>4__this.RemovePeaceOfferNotification(false);
			};
			CampaignEvents.OnPeaceOfferResolvedEvent.AddNonSerializedListener(this, new Action<IFaction>(this.OnPeaceOfferClosed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000200B5 File Offset: 0x0001E2B5
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemovePeaceOfferNotification(false);
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000200C6 File Offset: 0x0001E2C6
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._opponentFaction) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._opponentFaction))
			{
				this.RemovePeaceOfferNotification(false);
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000200FB File Offset: 0x0001E2FB
		private void OnPeaceOfferClosed(IFaction opponentFaction)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<PeaceOfferMapNotification>((PeaceOfferMapNotification x) => x == base.Data))
			{
				this.RemovePeaceOfferNotification(true);
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00020121 File Offset: 0x0001E321
		private void RemovePeaceOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00020130 File Offset: 0x0001E330
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Hero.MainHero.MapFaction.Leader != Hero.MainHero)
			{
				bool flag = false;
				foreach (KingdomDecision kingdomDecision in ((Kingdom)Hero.MainHero.MapFaction).UnresolvedDecisions)
				{
					if (kingdomDecision is MakePeaceKingdomDecision && ((MakePeaceKingdomDecision)kingdomDecision).ProposerClan.MapFaction == Hero.MainHero.MapFaction && ((MakePeaceKingdomDecision)kingdomDecision).FactionToMakePeaceWith == this._opponentFaction)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Hero.MainHero.MapFaction.Leader.Clan, this._opponentFaction, -this._tributeAmount, this._tributeDurationInDays, true, true);
					((Kingdom)Hero.MainHero.MapFaction).AddDecision(makePeaceKingdomDecision, true);
				}
			}
		}

		// Token: 0x040002A7 RID: 679
		private bool _shouldDecisionBeCreatedOnClosed;

		// Token: 0x040002A8 RID: 680
		private readonly IFaction _opponentFaction;

		// Token: 0x040002A9 RID: 681
		private readonly int _tributeAmount;

		// Token: 0x040002AA RID: 682
		private readonly int _tributeDurationInDays;
	}
}
