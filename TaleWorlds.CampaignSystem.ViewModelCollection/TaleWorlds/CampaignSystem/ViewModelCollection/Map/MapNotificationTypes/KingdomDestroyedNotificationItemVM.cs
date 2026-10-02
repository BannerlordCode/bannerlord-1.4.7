using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000044 RID: 68
	public class KingdomDestroyedNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x0001F45C File Offset: 0x0001D65C
		public KingdomDestroyedNotificationItemVM(KingdomDestroyedMapNotification data)
			: base(data)
		{
			KingdomDestroyedNotificationItemVM <>4__this = this;
			base.NotificationIdentifier = "kingdomdestroyed";
			this._onInspect = delegate
			{
				<>4__this.OnInspect(data);
			};
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001F4A6 File Offset: 0x0001D6A6
		private void OnInspect(KingdomDestroyedMapNotification data)
		{
			MBInformationManager.ShowSceneNotification(new KingdomDestroyedSceneNotificationItem(data.DestroyedKingdom, data.CreationTime));
			base.ExecuteRemove();
		}
	}
}
