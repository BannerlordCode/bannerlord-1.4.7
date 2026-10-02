using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200045E RID: 1118
	public class CommentOnDefeatCharacterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004831 RID: 18481 RVA: 0x0016BC99 File Offset: 0x00169E99
		public override void RegisterEvents()
		{
			CampaignEvents.CharacterDefeated.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnCharacterDefeated));
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x0016BCB2 File Offset: 0x00169EB2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x0016BCB4 File Offset: 0x00169EB4
		private void OnCharacterDefeated(Hero winner, Hero loser)
		{
			LogEntry.AddLogEntry(new DefeatCharacterLogEntry(winner, loser));
		}
	}
}
