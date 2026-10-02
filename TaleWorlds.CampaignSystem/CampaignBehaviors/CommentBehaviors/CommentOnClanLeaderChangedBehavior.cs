using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200045C RID: 1116
	public class CommentOnClanLeaderChangedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004829 RID: 18473 RVA: 0x0016BBF0 File Offset: 0x00169DF0
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(CommentOnClanLeaderChangedBehavior.OnClanLeaderChanged));
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0016BC09 File Offset: 0x00169E09
		private static void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			LogEntry.AddLogEntry(new ClanLeaderChangedLogEntry(oldLeader, newLeader));
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0016BC17 File Offset: 0x00169E17
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
