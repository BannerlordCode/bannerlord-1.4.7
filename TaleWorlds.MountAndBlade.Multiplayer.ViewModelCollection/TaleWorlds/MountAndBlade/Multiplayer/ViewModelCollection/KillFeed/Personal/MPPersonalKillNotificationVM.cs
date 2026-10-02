using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.Personal
{
	// Token: 0x0200008B RID: 139
	public class MPPersonalKillNotificationVM : ViewModel
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x000297BD File Offset: 0x000279BD
		public MPPersonalKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<MPPersonalKillNotificationItemVM>();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000297D0 File Offset: 0x000279D0
		public void OnGoldChange(int changeAmount, GoldGainFlags goldGainType)
		{
			this.NotificationList.Add(new MPPersonalKillNotificationItemVM(changeAmount, goldGainType, new Action<MPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x000297F0 File Offset: 0x000279F0
		public void OnPersonalHit(int damageAmount, bool isFatal, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName)
		{
			this.NotificationList.Add(new MPPersonalKillNotificationItemVM(damageAmount, isFatal, isMountDamage, isFriendlyFire, isHeadshot, killedAgentName, new Action<MPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00029822 File Offset: 0x00027A22
		public void OnPersonalAssist(string killedAgentName)
		{
			this.NotificationList.Add(new MPPersonalKillNotificationItemVM(killedAgentName, new Action<MPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00029841 File Offset: 0x00027A41
		private void RemoveItem(MPPersonalKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00029850 File Offset: 0x00027A50
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x00029858 File Offset: 0x00027A58
		[DataSourceProperty]
		public MBBindingList<MPPersonalKillNotificationItemVM> NotificationList
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
					base.OnPropertyChangedWithValue<MBBindingList<MPPersonalKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x04000626 RID: 1574
		private MBBindingList<MPPersonalKillNotificationItemVM> _notificationList;
	}
}
