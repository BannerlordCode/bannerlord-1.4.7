using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000461 RID: 1121
	public class CommentOnKingdomDestroyedBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600483D RID: 18493 RVA: 0x0016BD96 File Offset: 0x00169F96
		public override void RegisterEvents()
		{
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x0016BDAF File Offset: 0x00169FAF
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0016BDB1 File Offset: 0x00169FB1
		private void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			LogEntry.AddLogEntry(new KingdomDestroyedLogEntry(destroyedKingdom));
		}
	}
}
