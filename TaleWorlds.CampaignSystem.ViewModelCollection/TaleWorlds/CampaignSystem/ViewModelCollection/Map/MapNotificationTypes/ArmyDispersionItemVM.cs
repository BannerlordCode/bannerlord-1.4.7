using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000040 RID: 64
	public class ArmyDispersionItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005EA RID: 1514 RVA: 0x0001F0F4 File Offset: 0x0001D2F4
		public ArmyDispersionItemVM(ArmyDispersionMapNotification data)
			: base(data)
		{
			ArmyDispersionItemVM <>4__this = this;
			base.NotificationIdentifier = "armydispersion";
			this._onInspect = delegate
			{
				INavigationHandler navigationHandler = <>4__this.NavigationHandler;
				if (navigationHandler != null)
				{
					navigationHandler.OpenKingdom(data.DispersedArmy);
				}
				<>4__this.ExecuteRemove();
			};
		}
	}
}
