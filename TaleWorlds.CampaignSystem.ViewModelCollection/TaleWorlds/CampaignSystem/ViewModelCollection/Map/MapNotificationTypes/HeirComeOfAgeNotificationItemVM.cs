using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000043 RID: 67
	public class HeirComeOfAgeNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x0001F3B8 File Offset: 0x0001D5B8
		public HeirComeOfAgeNotificationItemVM(HeirComeOfAgeMapNotification data)
			: base(data)
		{
			HeirComeOfAgeNotificationItemVM <>4__this = this;
			base.NotificationIdentifier = "comeofage";
			this._onInspect = delegate
			{
				<>4__this.OnInspect(data);
			};
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001F404 File Offset: 0x0001D604
		private void OnInspect(HeirComeOfAgeMapNotification data)
		{
			SceneNotificationData sceneNotificationData;
			if (data.ComeOfAgeHero.IsFemale)
			{
				sceneNotificationData = new HeirComingOfAgeFemaleSceneNotificationItem(data.MentorHero, data.ComeOfAgeHero, data.CreationTime);
			}
			else
			{
				sceneNotificationData = new HeirComingOfAgeSceneNotificationItem(data.MentorHero, data.ComeOfAgeHero, data.CreationTime);
			}
			MBInformationManager.ShowSceneNotification(sceneNotificationData);
			base.ExecuteRemove();
		}
	}
}
