using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000455 RID: 1109
	public class CommentCharacterBornBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600480C RID: 18444 RVA: 0x0016B7CB File Offset: 0x001699CB
		public override void RegisterEvents()
		{
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.HeroCreated));
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0016B7E4 File Offset: 0x001699E4
		private void HeroCreated(Hero hero, bool isBornNaturally)
		{
			if (isBornNaturally)
			{
				LogEntry.AddLogEntry(new CharacterBornLogEntry(hero));
			}
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0016B7F4 File Offset: 0x001699F4
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
