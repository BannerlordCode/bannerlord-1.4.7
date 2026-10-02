using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.General
{
	// Token: 0x0200008D RID: 141
	public class MPGeneralKillNotificationVM : ViewModel
	{
		// Token: 0x06000DA7 RID: 3495 RVA: 0x00029FA5 File Offset: 0x000281A5
		public MPGeneralKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<MPGeneralKillNotificationItemVM>();
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00029FB8 File Offset: 0x000281B8
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, Agent assistedAgent)
		{
			this.NotificationList.Add(new MPGeneralKillNotificationItemVM(affectedAgent, affectorAgent, assistedAgent, new Action<MPGeneralKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00029FD9 File Offset: 0x000281D9
		private void RemoveItem(MPGeneralKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x00029FE8 File Offset: 0x000281E8
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x00029FF0 File Offset: 0x000281F0
		[DataSourceProperty]
		public MBBindingList<MPGeneralKillNotificationItemVM> NotificationList
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
					base.OnPropertyChangedWithValue<MBBindingList<MPGeneralKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x0400063A RID: 1594
		private MBBindingList<MPGeneralKillNotificationItemVM> _notificationList;
	}
}
