using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000045 RID: 69
	public class KingdomVoteNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0001F4C4 File Offset: 0x0001D6C4
		public KingdomVoteNotificationItemVM(KingdomDecisionMapNotification data)
			: base(data)
		{
			KingdomVoteNotificationItemVM <>4__this = this;
			this._decision = data.Decision;
			this._kingdomOfDecision = data.KingdomOfDecision;
			base.NotificationIdentifier = "vote";
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.KingdomDecisionCancelled.AddNonSerializedListener(this, new Action<KingdomDecision, bool>(this.OnDecisionCancelled));
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnDecisionConcluded));
			this._onInspect = new Action(this.OnInspect);
			this._onInspectOpenKingdom = delegate
			{
				<>4__this.NavigationHandler.OpenKingdom(data.Decision);
			};
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001F588 File Offset: 0x0001D788
		private void OnInspect()
		{
			if (!this._decision.ShouldBeCancelled())
			{
				Kingdom kingdom = Clan.PlayerClan.Kingdom;
				if (kingdom != null && kingdom.UnresolvedDecisions.Any<KingdomDecision>((KingdomDecision d) => d == this._decision))
				{
					TextObject textObject;
					if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
					{
						InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=lSnejlxB}You cannot participate in kingdom decisions right now.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
						return;
					}
					this._onInspectOpenKingdom();
					return;
				}
			}
			InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=i9OsCshW}This kingdom decision is not relevant anymore.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
			base.ExecuteRemove();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001F678 File Offset: 0x0001D878
		private void OnDecisionConcluded(KingdomDecision decision, DecisionOutcome arg2, bool arg3)
		{
			if (decision == this._decision)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001F689 File Offset: 0x0001D889
		private void OnDecisionCancelled(KingdomDecision decision, bool arg2)
		{
			if (decision == this._decision)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001F69A File Offset: 0x0001D89A
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (clan == Clan.PlayerClan)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001F6AA File Offset: 0x0001D8AA
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}

		// Token: 0x04000288 RID: 648
		private KingdomDecision _decision;

		// Token: 0x04000289 RID: 649
		private Kingdom _kingdomOfDecision;

		// Token: 0x0400028A RID: 650
		private Action _onInspectOpenKingdom;
	}
}
