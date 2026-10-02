using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000462 RID: 1122
	public class CommentOnLeaveFactionBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004841 RID: 18497 RVA: 0x0016BDC6 File Offset: 0x00169FC6
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanLeaveKingdom));
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x0016BDDF File Offset: 0x00169FDF
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0016BDE1 File Offset: 0x00169FE1
		private void OnClanLeaveKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			LogEntry.AddLogEntry(new ClanChangeKingdomLogEntry(clan, oldKingdom, newKingdom, detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion));
		}
	}
}
