using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004AD RID: 1197
	public static class DisbandPartyAction
	{
		// Token: 0x06004A82 RID: 19074 RVA: 0x00178EE4 File Offset: 0x001770E4
		public static void StartDisband(MobileParty disbandParty)
		{
			if (disbandParty.IsDisbanding)
			{
				return;
			}
			if (disbandParty.MemberRoster.TotalManCount == 0)
			{
				DestroyPartyAction.Apply(null, disbandParty);
				return;
			}
			IDisbandPartyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
			if (campaignBehavior != null && campaignBehavior.IsPartyWaitingForDisband(disbandParty))
			{
				return;
			}
			if (disbandParty.Army != null)
			{
				if (disbandParty == disbandParty.Army.LeaderParty)
				{
					DisbandArmyAction.ApplyByUnknownReason(disbandParty.Army);
				}
				else
				{
					disbandParty.Army = null;
				}
			}
			TextObject textObject = new TextObject("{=ithcVNfA}{CLAN_NAME}{.o} Party", null);
			textObject.SetTextVariable("CLAN_NAME", (disbandParty.ActualClan != null) ? disbandParty.ActualClan.Name : CampaignData.NeutralFactionName);
			disbandParty.Party.SetCustomName(textObject);
			CampaignEventDispatcher.Instance.OnPartyDisbandStarted(disbandParty);
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x00178F9A File Offset: 0x0017719A
		public static void CancelDisband(MobileParty disbandParty)
		{
			CampaignEventDispatcher.Instance.OnPartyDisbandCanceled(disbandParty);
			disbandParty.IsDisbanding = false;
			disbandParty.Party.SetCustomName(TextObject.GetEmpty());
			disbandParty.SetMoveModeHold();
		}
	}
}
