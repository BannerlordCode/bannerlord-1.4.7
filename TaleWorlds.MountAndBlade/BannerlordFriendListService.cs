using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E2 RID: 738
	public class BannerlordFriendListService : IFriendListService
	{
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06002AAD RID: 10925 RVA: 0x000A420C File Offset: 0x000A240C
		// (remove) Token: 0x06002AAE RID: 10926 RVA: 0x000A4244 File Offset: 0x000A2444
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06002AAF RID: 10927 RVA: 0x000A427C File Offset: 0x000A247C
		// (remove) Token: 0x06002AB0 RID: 10928 RVA: 0x000A42B4 File Offset: 0x000A24B4
		public event Action<PlayerId> OnFriendRemoved;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06002AB1 RID: 10929 RVA: 0x000A42EC File Offset: 0x000A24EC
		// (remove) Token: 0x06002AB2 RID: 10930 RVA: 0x000A4324 File Offset: 0x000A2524
		public event Action OnFriendListChanged;

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000A4359 File Offset: 0x000A2559
		bool IFriendListService.InGameStatusFetchable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x000A435C File Offset: 0x000A255C
		bool IFriendListService.AllowsFriendOperations
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000A435F File Offset: 0x000A255F
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return PlatformServices.InvitationServices != null;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x000A4369 File Offset: 0x000A2569
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000A436C File Offset: 0x000A256C
		public BannerlordFriendListService()
		{
			this.Friends = new List<FriendInfo>();
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000A437F File Offset: 0x000A257F
		string IFriendListService.GetServiceCodeName()
		{
			return "TaleWorlds";
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000A4386 File Offset: 0x000A2586
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=!}TaleWorlds", null);
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000A4393 File Offset: 0x000A2593
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.Bannerlord;
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000A4398 File Offset: 0x000A2598
		IEnumerable<PlayerId> IFriendListService.GetPendingRequests()
		{
			return from f in this.Friends
				where f.Status == FriendStatus.Pending
				select f.Id;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000A43F4 File Offset: 0x000A25F4
		IEnumerable<PlayerId> IFriendListService.GetReceivedRequests()
		{
			return from f in this.Friends
				where f.Status == FriendStatus.Received
				select f.Id;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000A4450 File Offset: 0x000A2650
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			return from f in this.Friends
				where f.Status == FriendStatus.Accepted
				select f.Id;
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000A44AC File Offset: 0x000A26AC
		Task<bool> IFriendListService.GetUserOnlineStatus(PlayerId providedId)
		{
			foreach (FriendInfo friendInfo in this.Friends)
			{
				if (friendInfo.Id.Equals(providedId))
				{
					return Task.FromResult<bool>(friendInfo.IsOnline);
				}
			}
			return Task.FromResult<bool>(false);
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000A4520 File Offset: 0x000A2720
		Task<bool> IFriendListService.IsPlayingThisGame(PlayerId providedId)
		{
			return ((IFriendListService)this).GetUserOnlineStatus(providedId);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000A452C File Offset: 0x000A272C
		Task<string> IFriendListService.GetUserName(PlayerId providedId)
		{
			foreach (FriendInfo friendInfo in this.Friends)
			{
				if (friendInfo.Id.Equals(providedId))
				{
					return Task.FromResult<string>(friendInfo.Name);
				}
			}
			return Task.FromResult<string>(null);
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000A45A0 File Offset: 0x000A27A0
		Task<PlayerId> IFriendListService.GetUserWithName(string name)
		{
			foreach (FriendInfo friendInfo in this.Friends)
			{
				if (friendInfo.Name == name)
				{
					return Task.FromResult<PlayerId>(friendInfo.Id);
				}
			}
			return Task.FromResult<PlayerId>(default(PlayerId));
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000A4618 File Offset: 0x000A2818
		public void OnFriendListReceived(FriendInfo[] friends)
		{
			List<FriendInfo> friends2 = this.Friends;
			this.Friends = new List<FriendInfo>(friends);
			List<PlayerId> list = null;
			bool flag = false;
			using (List<FriendInfo>.Enumerator enumerator = this.Friends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FriendInfo friend = enumerator.Current;
					int num = friends2.FindIndex((FriendInfo o) => o.Id.Equals(friend.Id));
					if (num < 0)
					{
						flag = true;
					}
					else
					{
						FriendInfo friendInfo = friends2[num];
						friends2.RemoveAt(num);
						if (friendInfo.Status != friend.Status)
						{
							flag = true;
						}
						else if (friendInfo.IsOnline != friend.IsOnline)
						{
							if (list == null)
							{
								list = new List<PlayerId>();
							}
							list.Add(friendInfo.Id);
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				if (friends2.Count > 0)
				{
					foreach (FriendInfo friendInfo2 in friends2)
					{
						Action<PlayerId> onFriendRemoved = this.OnFriendRemoved;
						if (onFriendRemoved != null)
						{
							onFriendRemoved(friendInfo2.Id);
						}
					}
				}
				if (list != null)
				{
					foreach (PlayerId playerId in list)
					{
						Action<PlayerId> onUserStatusChanged = this.OnUserStatusChanged;
						if (onUserStatusChanged != null)
						{
							onUserStatusChanged(playerId);
						}
					}
				}
				return;
			}
			Action onFriendListChanged = this.OnFriendListChanged;
			if (onFriendListChanged == null)
			{
				return;
			}
			onFriendListChanged();
		}

		// Token: 0x0400103F RID: 4159
		protected List<FriendInfo> Friends;
	}
}
