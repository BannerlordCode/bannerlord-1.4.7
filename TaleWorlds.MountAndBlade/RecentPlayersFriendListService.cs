using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000386 RID: 902
	public class RecentPlayersFriendListService : BannerlordFriendListService, IFriendListService
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x000D5EA9 File Offset: 0x000D40A9
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x000D5EAC File Offset: 0x000D40AC
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return PlatformServices.InvitationServices != null;
			}
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000D5EB6 File Offset: 0x000D40B6
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=XvSRoOzM}Recently Played Players", null);
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000D5EC3 File Offset: 0x000D40C3
		string IFriendListService.GetServiceCodeName()
		{
			return "RecentlyPlayedPlayers";
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000D5ECA File Offset: 0x000D40CA
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			return RecentPlayersManager.GetPlayersOrdered();
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000D5ED1 File Offset: 0x000D40D1
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.RecentPlayers;
		}
	}
}
