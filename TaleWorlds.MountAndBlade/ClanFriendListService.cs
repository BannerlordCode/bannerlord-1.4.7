using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EA RID: 490
	public class ClanFriendListService : IFriendListService
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x00061A3F File Offset: 0x0005FC3F
		bool IFriendListService.InGameStatusFetchable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x00061A42 File Offset: 0x0005FC42
		bool IFriendListService.AllowsFriendOperations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x00061A45 File Offset: 0x0005FC45
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x00061A48 File Offset: 0x0005FC48
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00061A4B File Offset: 0x0005FC4B
		public ClanFriendListService()
		{
			this._clanPlayerInfos = new Dictionary<PlayerId, ClanPlayerInfo>();
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00061A5E File Offset: 0x0005FC5E
		string IFriendListService.GetServiceCodeName()
		{
			return "ClanFriends";
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00061A65 File Offset: 0x0005FC65
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=j4F7tTzy}Clan", null);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00061A72 File Offset: 0x0005FC72
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.Clan;
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00061A75 File Offset: 0x0005FC75
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			return this._clanPlayerInfos.Keys;
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001C8B RID: 7307 RVA: 0x00061A84 File Offset: 0x0005FC84
		// (remove) Token: 0x06001C8C RID: 7308 RVA: 0x00061ABC File Offset: 0x0005FCBC
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001C8D RID: 7309 RVA: 0x00061AF4 File Offset: 0x0005FCF4
		// (remove) Token: 0x06001C8E RID: 7310 RVA: 0x00061B2C File Offset: 0x0005FD2C
		public event Action<PlayerId> OnFriendRemoved;

		// Token: 0x06001C8F RID: 7311 RVA: 0x00061B64 File Offset: 0x0005FD64
		async Task<bool> IFriendListService.GetUserOnlineStatus(PlayerId providedId)
		{
			bool flag = false;
			ClanPlayerInfo clanPlayerInfo;
			this._clanPlayerInfos.TryGetValue(providedId, out clanPlayerInfo);
			if (clanPlayerInfo != null)
			{
				flag = clanPlayerInfo.State == AnotherPlayerState.InMultiplayerGame || clanPlayerInfo.State == AnotherPlayerState.AtLobby || clanPlayerInfo.State == AnotherPlayerState.InParty;
			}
			return await Task.FromResult<bool>(flag);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00061BB4 File Offset: 0x0005FDB4
		async Task<bool> IFriendListService.IsPlayingThisGame(PlayerId providedId)
		{
			return await ((IFriendListService)this).GetUserOnlineStatus(providedId);
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00061C04 File Offset: 0x0005FE04
		async Task<string> IFriendListService.GetUserName(PlayerId providedId)
		{
			ClanPlayerInfo clanPlayerInfo;
			this._clanPlayerInfos.TryGetValue(providedId, out clanPlayerInfo);
			return await Task.FromResult<string>((clanPlayerInfo != null) ? clanPlayerInfo.PlayerName : null);
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00061C54 File Offset: 0x0005FE54
		public async Task<PlayerId> GetUserWithName(string name)
		{
			ClanPlayerInfo clanPlayerInfo = this._clanPlayerInfos.Values.FirstOrDefaultQ<ClanPlayerInfo>((ClanPlayerInfo playerInfo) => playerInfo.PlayerName == name);
			return await Task.FromResult<PlayerId>((clanPlayerInfo != null) ? clanPlayerInfo.PlayerId : PlayerId.Empty);
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001C93 RID: 7315 RVA: 0x00061CA4 File Offset: 0x0005FEA4
		// (remove) Token: 0x06001C94 RID: 7316 RVA: 0x00061CDC File Offset: 0x0005FEDC
		public event Action OnFriendListChanged;

		// Token: 0x06001C95 RID: 7317 RVA: 0x00061D11 File Offset: 0x0005FF11
		public IEnumerable<PlayerId> GetPendingRequests()
		{
			return null;
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x00061D14 File Offset: 0x0005FF14
		public IEnumerable<PlayerId> GetReceivedRequests()
		{
			return null;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x00061D18 File Offset: 0x0005FF18
		private void Dummy()
		{
			if (this.OnUserStatusChanged != null)
			{
				this.OnUserStatusChanged(default(PlayerId));
			}
			if (this.OnFriendRemoved != null)
			{
				this.OnFriendRemoved(default(PlayerId));
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00061D60 File Offset: 0x0005FF60
		public void OnClanInfoChanged(List<ClanPlayerInfo> playerInfosInClan)
		{
			this._clanPlayerInfos.Clear();
			if (playerInfosInClan != null)
			{
				foreach (ClanPlayerInfo clanPlayerInfo in playerInfosInClan)
				{
					this._clanPlayerInfos.Add(clanPlayerInfo.PlayerId, clanPlayerInfo);
				}
			}
			Action onFriendListChanged = this.OnFriendListChanged;
			if (onFriendListChanged == null)
			{
				return;
			}
			onFriendListChanged();
		}

		// Token: 0x040009BD RID: 2493
		public const string CodeName = "ClanFriends";

		// Token: 0x040009BE RID: 2494
		private readonly Dictionary<PlayerId, ClanPlayerInfo> _clanPlayerInfos;
	}
}
