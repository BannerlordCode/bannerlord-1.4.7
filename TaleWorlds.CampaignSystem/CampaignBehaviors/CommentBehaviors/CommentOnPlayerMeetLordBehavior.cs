using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000464 RID: 1124
	public class CommentOnPlayerMeetLordBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004849 RID: 18505 RVA: 0x0016BE75 File Offset: 0x0016A075
		public override void RegisterEvents()
		{
			CampaignEvents.OnPlayerMetHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnPlayerMetCharacter));
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x0016BE8E File Offset: 0x0016A08E
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x0016BE90 File Offset: 0x0016A090
		private void OnPlayerMetCharacter(Hero hero)
		{
			if (hero.Mother != Hero.MainHero && hero.Father != Hero.MainHero)
			{
				LogEntry.AddLogEntry(new PlayerMeetLordLogEntry(hero));
			}
		}
	}
}
