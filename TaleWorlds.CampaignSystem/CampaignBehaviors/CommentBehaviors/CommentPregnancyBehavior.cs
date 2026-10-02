using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000465 RID: 1125
	public class CommentPregnancyBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600484D RID: 18509 RVA: 0x0016BEBF File Offset: 0x0016A0BF
		public override void RegisterEvents()
		{
			CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnChildConceived));
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x0016BED8 File Offset: 0x0016A0D8
		private void OnChildConceived(Hero mother)
		{
			LogEntry.AddLogEntry(new PregnancyLogEntry(mother));
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x0016BEE5 File Offset: 0x0016A0E5
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
