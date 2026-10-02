using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000055 RID: 85
	public class TributeFinishedMapNotificationVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x000208B0 File Offset: 0x0001EAB0
		public TributeFinishedMapNotificationVM(TributeFinishedMapNotification data)
			: base(data)
		{
			TributeFinishedMapNotificationVM <>4__this = this;
			base.NotificationIdentifier = "ransom";
			this._onInspect = delegate
			{
				<>4__this.OnInspect(data.RelatedFaction);
			};
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000208FA File Offset: 0x0001EAFA
		private void OnInspect(IFaction relatedFaction)
		{
			INavigationHandler navigationHandler = base.NavigationHandler;
			if (navigationHandler != null)
			{
				navigationHandler.OpenKingdom(relatedFaction);
			}
			base.ExecuteRemove();
		}
	}
}
