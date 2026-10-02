using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200045B RID: 1115
	public class CommentOnClanDestroyedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004825 RID: 18469 RVA: 0x0016BBC0 File Offset: 0x00169DC0
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0016BBD9 File Offset: 0x00169DD9
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x0016BBDB File Offset: 0x00169DDB
		private void OnClanDestroyed(Clan destroyedClan)
		{
			LogEntry.AddLogEntry(new ClanDestroyedLogEntry(destroyedClan));
		}
	}
}
