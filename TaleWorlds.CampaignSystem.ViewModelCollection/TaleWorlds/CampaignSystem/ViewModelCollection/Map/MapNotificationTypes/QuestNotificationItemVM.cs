using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004F RID: 79
	public class QuestNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000645 RID: 1605 RVA: 0x000204F4 File Offset: 0x0001E6F4
		public QuestNotificationItemVM(QuestBase quest, InformationData data, Action<QuestBase> onQuestNotificationInspect, Action<MapNotificationItemBaseVM> onRemove)
			: base(data)
		{
			this._quest = quest;
			this._onQuestNotificationInspect = onQuestNotificationInspect;
			this._onInspect = (this._onInspectAction = delegate
			{
				this._onQuestNotificationInspect(this._quest);
			});
			base.NotificationIdentifier = "quest";
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0002053C File Offset: 0x0001E73C
		public QuestNotificationItemVM(IssueBase issue, InformationData data, Action<IssueBase> onIssueNotificationInspect, Action<MapNotificationItemBaseVM> onRemove)
			: base(data)
		{
			this._issue = issue;
			this._onIssueNotificationInspect = onIssueNotificationInspect;
			this._onInspect = (this._onInspectAction = delegate
			{
				this._onIssueNotificationInspect(this._issue);
			});
			base.NotificationIdentifier = "quest";
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00020584 File Offset: 0x0001E784
		public override void ManualRefreshRelevantStatus()
		{
			base.ManualRefreshRelevantStatus();
		}

		// Token: 0x040002AE RID: 686
		private QuestBase _quest;

		// Token: 0x040002AF RID: 687
		private IssueBase _issue;

		// Token: 0x040002B0 RID: 688
		private Action<QuestBase> _onQuestNotificationInspect;

		// Token: 0x040002B1 RID: 689
		private Action<IssueBase> _onIssueNotificationInspect;

		// Token: 0x040002B2 RID: 690
		protected Action _onInspectAction;
	}
}
