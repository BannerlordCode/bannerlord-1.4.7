using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.Personal
{
	// Token: 0x0200005E RID: 94
	public class SPPersonalKillNotificationVM : ViewModel
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x0001AFAE File Offset: 0x000191AE
		public SPPersonalKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<SPPersonalKillNotificationItemVM>();
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001AFC4 File Offset: 0x000191C4
		public void OnPersonalKill(int damageAmount, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, bool isUnconscious)
		{
			this.NotificationList.Add(new SPPersonalKillNotificationItemVM(damageAmount, isMountDamage, isFriendlyFire, isHeadshot, killedAgentName, isUnconscious, new Action<SPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001AFF6 File Offset: 0x000191F6
		public void OnPersonalHit(int damageAmount, bool isMountDamage, bool isFriendlyFire, string killedAgentName)
		{
			this.NotificationList.Add(new SPPersonalKillNotificationItemVM(damageAmount, isMountDamage, isFriendlyFire, killedAgentName, new Action<SPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001B019 File Offset: 0x00019219
		public void OnPersonalMessage(string message)
		{
			this.NotificationList.Add(new SPPersonalKillNotificationItemVM(message, new Action<SPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001B038 File Offset: 0x00019238
		private void RemoveItem(SPPersonalKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0001B047 File Offset: 0x00019247
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0001B04F File Offset: 0x0001924F
		[DataSourceProperty]
		public MBBindingList<SPPersonalKillNotificationItemVM> NotificationList
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
					base.OnPropertyChangedWithValue<MBBindingList<SPPersonalKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x0400035A RID: 858
		private MBBindingList<SPPersonalKillNotificationItemVM> _notificationList;
	}
}
