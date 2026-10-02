using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000125 RID: 293
	public class DefaultKingdomDecisionPermissionModel : KingdomDecisionPermissionModel
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001868 RID: 6248 RVA: 0x0007624F File Offset: 0x0007444F
		private IAllianceCampaignBehavior AllianceCampaignBehavior
		{
			get
			{
				if (this._allianceCampaignBehavior == null)
				{
					this._allianceCampaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
				}
				return this._allianceCampaignBehavior;
			}
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0007626F File Offset: 0x0007446F
		public override bool IsPolicyDecisionAllowed(PolicyObject policy)
		{
			return true;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00076272 File Offset: 0x00074472
		public override bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = null;
			return true;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00076278 File Offset: 0x00074478
		public override bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = null;
			Kingdom kingdom3 = null;
			if (Campaign.Current.Models.DiplomacyModel.IsAtConstantWar(kingdom1, kingdom2))
			{
				reason = new TextObject("{=eNPupZOp}These kingdoms can not declare peace at this time.", null);
				return false;
			}
			IAllianceCampaignBehavior allianceCampaignBehavior = this.AllianceCampaignBehavior;
			if (allianceCampaignBehavior != null && allianceCampaignBehavior.IsAtWarByCallToWarAgreement(kingdom1, kingdom2, out kingdom3))
			{
				reason = this.GetExplanationForPeaceOfferWithCallToWar(kingdom3, kingdom1, kingdom2);
				return false;
			}
			IAllianceCampaignBehavior allianceCampaignBehavior2 = this.AllianceCampaignBehavior;
			if (allianceCampaignBehavior2 != null && allianceCampaignBehavior2.IsAtWarByCallToWarAgreement(kingdom2, kingdom1, out kingdom3))
			{
				reason = this.GetExplanationForPeaceOfferWithCallToWar(kingdom3, kingdom2, kingdom1);
				return false;
			}
			if (!Campaign.Current.Models.DiplomacyModel.IsPeaceSuitable(kingdom1, kingdom2))
			{
				reason = new TextObject("{=JkQ7fmcX}The enemy is not open to negotiations.", null);
				return false;
			}
			return true;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00076323 File Offset: 0x00074523
		public override bool IsAnnexationDecisionAllowed(Settlement annexedSettlement)
		{
			return true;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00076326 File Offset: 0x00074526
		public override bool IsExpulsionDecisionAllowed(Clan expelledClan)
		{
			return true;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00076329 File Offset: 0x00074529
		public override bool IsKingSelectionDecisionAllowed(Kingdom kingdom)
		{
			return true;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0007632C File Offset: 0x0007452C
		public override bool IsStartAllianceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = null;
			return true;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00076334 File Offset: 0x00074534
		private TextObject GetExplanationForPeaceOfferWithCallToWar(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (calledKingdom == Clan.PlayerClan.Kingdom)
			{
				textObject = new TextObject("{=M6wsjpNN}Your realm is not allowed to negotiate peace with {KINGDOM_TO_CALL_TO_WAR_AGAINST} due to your Call to War Agreement with {CALLING_KINGDOM}.", null);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				textObject.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
			}
			else if (kingdomToCallToWarAgainst == Clan.PlayerClan.Kingdom)
			{
				textObject = new TextObject("{=CiFKYMKb}Your realm is not allowed to negotiate peace with {CALLED_KINGDOM} due to their Call to War Agreement with {CALLING_KINGDOM}.", null);
				textObject.SetTextVariable("CALLED_KINGDOM", calledKingdom.Name);
				textObject.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
			}
			else
			{
				textObject = new TextObject("{=mc0wmdkb}{KINGDOM_NAME} is not allowed to negotiate peace with {CALLED_KINGDOM} due to their Call to War Agreement with {CALLING_KINGDOM}.", null);
				textObject.SetTextVariable("KINGDOM_NAME", kingdomToCallToWarAgainst.Name);
				textObject.SetTextVariable("CALLED_KINGDOM", calledKingdom.Name);
				textObject.SetTextVariable("CALLING_KINGDOM", callingKingdom.Name);
			}
			return textObject;
		}

		// Token: 0x04000804 RID: 2052
		private IAllianceCampaignBehavior _allianceCampaignBehavior;
	}
}
