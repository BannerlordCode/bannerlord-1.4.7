using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000458 RID: 1112
	public class CommentOnChangeSettlementOwnerBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004818 RID: 18456 RVA: 0x0016B952 File Offset: 0x00169B52
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0016B96B File Offset: 0x00169B6B
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0016B970 File Offset: 0x00169B70
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero previousOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			ChangeSettlementOwnerLogEntry changeSettlementOwnerLogEntry = new ChangeSettlementOwnerLogEntry(settlement, newOwner, previousOwner, false);
			LogEntry.AddLogEntry(changeSettlementOwnerLogEntry);
			if (newOwner != null && newOwner.IsHumanPlayerCharacter)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementOwnerChangedMapNotification(settlement, newOwner, previousOwner, changeSettlementOwnerLogEntry.GetEncyclopediaText()));
			}
		}
	}
}
