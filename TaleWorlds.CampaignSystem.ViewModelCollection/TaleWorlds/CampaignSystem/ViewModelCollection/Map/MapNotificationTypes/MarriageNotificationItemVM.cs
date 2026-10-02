using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000047 RID: 71
	public class MarriageNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001F990 File Offset: 0x0001DB90
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x0001F998 File Offset: 0x0001DB98
		public Hero Suitor { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001F9A1 File Offset: 0x0001DBA1
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x0001F9A9 File Offset: 0x0001DBA9
		public Hero Maiden { get; private set; }

		// Token: 0x0600061E RID: 1566 RVA: 0x0001F9B4 File Offset: 0x0001DBB4
		public MarriageNotificationItemVM(MarriageMapNotification data)
			: base(data)
		{
			MarriageNotificationItemVM <>4__this = this;
			this.Suitor = data.Suitor;
			this.Maiden = data.Maiden;
			base.NotificationIdentifier = "marriage";
			this._onInspect = delegate
			{
				MBInformationManager.ShowSceneNotification(new MarriageSceneNotificationItem(data.Suitor, data.Maiden, data.CreationTime, SceneNotificationData.RelevantContextType.Any));
				<>4__this.ExecuteRemove();
			};
		}
	}
}
