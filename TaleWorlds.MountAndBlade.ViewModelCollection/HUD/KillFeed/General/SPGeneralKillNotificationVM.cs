using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.General
{
	// Token: 0x02000060 RID: 96
	public class SPGeneralKillNotificationVM : ViewModel
	{
		// Token: 0x060007AF RID: 1967 RVA: 0x0001B43D File Offset: 0x0001963D
		public SPGeneralKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<SPGeneralKillNotificationItemVM>();
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001B450 File Offset: 0x00019650
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning)
		{
			this.NotificationList.Add(new SPGeneralKillNotificationItemVM(affectedAgent, affectorAgent, isHeadshot, isSuicide, isDrowning, new Action<SPGeneralKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001B475 File Offset: 0x00019675
		private void RemoveItem(SPGeneralKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001B484 File Offset: 0x00019684
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0001B48C File Offset: 0x0001968C
		[DataSourceProperty]
		public MBBindingList<SPGeneralKillNotificationItemVM> NotificationList
		{
			get
			{
				return this._notificationList;
			}
			set
			{
				if (value != this._notificationList)
				{
					this._notificationList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPGeneralKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x0400036B RID: 875
		private MBBindingList<SPGeneralKillNotificationItemVM> _notificationList;
	}
}
