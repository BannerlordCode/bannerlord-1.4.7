using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000054 RID: 84
	public class MPLobbyFriendServiceVM : ViewModel
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00016D88 File Offset: 0x00014F88
		public IEnumerable<MPLobbyPlayerBaseVM> AllFriends
		{
			get
			{
				return this.InGameFriends.FriendList.Union<MPLobbyFriendItemVM>(this.OnlineFriends.FriendList.Union<MPLobbyFriendItemVM>(this.OfflineFriends.FriendList));
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00016DB8 File Offset: 0x00014FB8
		public MPLobbyFriendServiceVM(IFriendListService friendListService, Action<PlayerId> onFriendRequestAnswered, Action<MPLobbyPlayerBaseVM> activatePlayerActions)
		{
			this.FriendListService = friendListService;
			this._onFriendRequestAnswered = onFriendRequestAnswered;
			this._activatePlayerActions = activatePlayerActions;
			this._playerStateComparer = new MPLobbyFriendServiceVM.PlayerStateComparer();
			this.InGameFriends = new MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType.InGame);
			this.OnlineFriends = new MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType.Online);
			this.OfflineFriends = new MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType.Offline);
			this.FriendRequests = new MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType.FriendRequests);
			this.PendingRequests = new MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType.PendingRequests);
			FriendListServiceType friendListServiceType = friendListService.GetFriendListServiceType();
			this._isInGameFriendsRelevant = friendListServiceType != FriendListServiceType.Bannerlord && friendListServiceType != FriendListServiceType.RecentPlayers && friendListServiceType != FriendListServiceType.Clan && friendListServiceType != FriendListServiceType.PlayStation;
			PlatformServices.Instance.OnBlockedUserListUpdated += this.BlockedUserListChanged;
			this.FriendListService.OnUserStatusChanged += this.UserOnlineStatusChanged;
			this.FriendListService.OnFriendRemoved += this.FriendRemoved;
			this.FriendListService.OnFriendListChanged += this.FriendListChanged;
			this.ServiceName = friendListService.GetServiceCodeName();
			this.RefreshValues();
			this.UpdateCanInviteOtherPlayersToParty();
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00016EC0 File Offset: 0x000150C0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InGameText = new TextObject("{=uUoSmCBS}In Bannerlord", null).ToString();
			this.OnlineText = new TextObject("{=V305MaOP}Online", null).ToString();
			this.OfflineText = new TextObject("{=Zv1lg272}Offline", null).ToString();
			this.ServiceNameHint = new HintViewModel(this.FriendListService.GetServiceLocalizedName(), null);
			this.InGameFriends.RefreshValues();
			this.OnlineFriends.RefreshValues();
			this.OfflineFriends.RefreshValues();
			this.FriendRequests.RefreshValues();
			this.PendingRequests.RefreshValues();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00016F64 File Offset: 0x00015164
		public override void OnFinalize()
		{
			PlatformServices.Instance.OnBlockedUserListUpdated -= this.BlockedUserListChanged;
			this.FriendListService.OnUserStatusChanged -= this.UserOnlineStatusChanged;
			this.FriendListService.OnFriendRemoved -= this.FriendRemoved;
			this.FriendListService.OnFriendListChanged -= this.FriendListChanged;
			base.OnFinalize();
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00016FD2 File Offset: 0x000151D2
		public void OnStateActivate()
		{
			this._isPartyAvailable = NetworkMain.GameClient.PartySystemAvailable;
			this.IsInGameStatusActive = this.FriendListService.InGameStatusFetchable && this._isInGameFriendsRelevant;
			this.GetFriends();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00017008 File Offset: 0x00015208
		private async void GetFriends()
		{
			IEnumerable<PlayerId> allFriends = this.FriendListService.GetAllFriends();
			if (allFriends != null && !this._populatingFriends)
			{
				this._populatingFriends = true;
				this.InGameFriends.ClearFriends();
				this.OnlineFriends.ClearFriends();
				this.OfflineFriends.ClearFriends();
				foreach (PlayerId playerId in allFriends)
				{
					await this.CreateAndAddFriendToList(playerId);
				}
				IEnumerator<PlayerId> enumerator = null;
				this._lastStateRequestTimePassed = 11f;
				this._populatingFriends = false;
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00017044 File Offset: 0x00015244
		public void OnTick(float dt)
		{
			this.UpdateFriendStates(dt);
			this._lastUpdateTimePassed += dt;
			if (this._lastUpdateTimePassed >= 2f)
			{
				this._lastUpdateTimePassed = 0f;
				if (this.FriendListService.AllowsFriendOperations)
				{
					this.TickFriendOperations(dt);
				}
			}
			MPLobbyFriendGroupVM inGameFriends = this.InGameFriends;
			if (inGameFriends != null)
			{
				inGameFriends.Tick();
			}
			MPLobbyFriendGroupVM onlineFriends = this.OnlineFriends;
			if (onlineFriends != null)
			{
				onlineFriends.Tick();
			}
			MPLobbyFriendGroupVM offlineFriends = this.OfflineFriends;
			if (offlineFriends != null)
			{
				offlineFriends.Tick();
			}
			MPLobbyFriendGroupVM friendRequests = this.FriendRequests;
			if (friendRequests != null)
			{
				friendRequests.Tick();
			}
			MPLobbyFriendGroupVM pendingRequests = this.PendingRequests;
			if (pendingRequests == null)
			{
				return;
			}
			pendingRequests.Tick();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000170E8 File Offset: 0x000152E8
		private void TimeoutProcessedFriendRequests()
		{
			foreach (PlayerId playerId in MPLobbyFriendServiceVM._friendRequestsInProcess.Keys.ToArray<PlayerId>())
			{
				if ((long)Environment.TickCount - MPLobbyFriendServiceVM._friendRequestsInProcess[playerId] > 10000L)
				{
					MPLobbyFriendServiceVM._friendRequestsInProcess.Remove(playerId);
				}
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00017144 File Offset: 0x00015344
		private void BlockFriendRequest(PlayerId friendId)
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(friendId);
			NetworkMain.GameClient.RespondToFriendRequest(friendId, flag, false, true);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00017178 File Offset: 0x00015378
		private void ProcessFriendRequest(PlayerId friendId)
		{
			if (MPLobbyFriendServiceVM._friendRequestsInProcess.ContainsKey(friendId))
			{
				return;
			}
			MPLobbyFriendServiceVM._friendRequestsInProcess[friendId] = (long)Environment.TickCount;
			PermissionResult <>9__1;
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, false, delegate(bool privilegeResult)
			{
				if (!privilegeResult)
				{
					this.BlockFriendRequest(friendId);
					return;
				}
				if (friendId.ProvidedType != NetworkMain.GameClient.PlayerID.ProvidedType)
				{
					this.AddFriendRequestItem(friendId);
					return;
				}
				IPlatformServices instance = PlatformServices.Instance;
				Permission permission = Permission.CommunicateUsingText;
				PlayerId friendId2 = friendId;
				PermissionResult permissionResult2;
				if ((permissionResult2 = <>9__1) == null)
				{
					permissionResult2 = (<>9__1 = delegate(bool permissionResult)
					{
						if (!permissionResult)
						{
							this.BlockFriendRequest(friendId);
							return;
						}
						this.AddFriendRequestItem(friendId);
					});
				}
				instance.CheckPermissionWithUser(permission, friendId2, permissionResult2);
			});
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000171DC File Offset: 0x000153DC
		private void AddFriendRequestItem(PlayerId playerID)
		{
			MPLobbyFriendItemVM mplobbyFriendItemVM = new MPLobbyFriendItemVM(playerID, this._activatePlayerActions, null, this._onFriendRequestAnswered);
			mplobbyFriendItemVM.IsFriendRequest = true;
			mplobbyFriendItemVM.CanRemove = false;
			this.FriendRequests.AddFriend(mplobbyFriendItemVM);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00017218 File Offset: 0x00015418
		private void TickFriendOperations(float dt)
		{
			IEnumerable<PlayerId> receivedRequests = this.FriendListService.GetReceivedRequests();
			if (receivedRequests != null)
			{
				this.GotAnyFriendRequests = receivedRequests.Any<PlayerId>();
				this.TimeoutProcessedFriendRequests();
				using (IEnumerator<PlayerId> enumerator = receivedRequests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerId friendId2 = enumerator.Current;
						if (this.FriendRequests.FriendList.FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == friendId2) == null)
						{
							this.ProcessFriendRequest(friendId2);
						}
					}
				}
				int num;
				int j;
				for (j = this.FriendRequests.FriendList.Count - 1; j >= 0; j = num - 1)
				{
					if (!receivedRequests.FirstOrDefault<PlayerId>((PlayerId p) => p == this.FriendRequests.FriendList[j].ProvidedID).IsValid)
					{
						this.FriendRequests.RemoveFriend(this.FriendRequests.FriendList[j]);
					}
					num = j;
				}
			}
			else
			{
				this.GotAnyFriendRequests = false;
			}
			IEnumerable<PlayerId> pendingRequests = this.FriendListService.GetPendingRequests();
			if (pendingRequests != null)
			{
				this.GotAnyPendingRequests = pendingRequests.Any<PlayerId>();
				using (IEnumerator<PlayerId> enumerator = pendingRequests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerId friendId = enumerator.Current;
						if (this.PendingRequests.FriendList.FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == friendId) == null)
						{
							MPLobbyFriendItemVM mplobbyFriendItemVM = new MPLobbyFriendItemVM(friendId, this._activatePlayerActions, null, null);
							mplobbyFriendItemVM.IsPendingRequest = true;
							mplobbyFriendItemVM.CanRemove = false;
							this.PendingRequests.AddFriend(mplobbyFriendItemVM);
						}
					}
				}
				int num;
				int i;
				for (i = this.PendingRequests.FriendList.Count - 1; i >= 0; i = num - 1)
				{
					if (!pendingRequests.FirstOrDefault<PlayerId>((PlayerId p) => p == this.PendingRequests.FriendList[i].ProvidedID).IsValid)
					{
						this.PendingRequests.RemoveFriend(this.PendingRequests.FriendList[i]);
					}
					num = i;
				}
				return;
			}
			this.GotAnyPendingRequests = false;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00017480 File Offset: 0x00015680
		private void UpdateFriendStates(float dt)
		{
			if (!NetworkMain.GameClient.AtLobby || this._isStateRequestActive)
			{
				return;
			}
			this._lastStateRequestTimePassed += dt;
			if (this._lastStateRequestTimePassed >= 10f)
			{
				List<PlayerId> list = new List<PlayerId>();
				list.AddRange(this.InGameFriends.FriendList.Select<MPLobbyFriendItemVM, PlayerId>((MPLobbyFriendItemVM p) => p.ProvidedID));
				list.AddRange(this.OnlineFriends.FriendList.Select<MPLobbyFriendItemVM, PlayerId>((MPLobbyFriendItemVM p) => p.ProvidedID));
				this._lastStateRequestTimePassed = 0f;
				this.UpdatePlayerStates(list);
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00017544 File Offset: 0x00015744
		private async void UpdatePlayerStates(List<PlayerId> players)
		{
			if (players != null && players.Count > 0)
			{
				this._isStateRequestActive = true;
				List<ValueTuple<PlayerId, AnotherPlayerData>> list = await NetworkMain.GameClient.GetOtherPlayersState(players);
				if (list != null)
				{
					foreach (ValueTuple<PlayerId, AnotherPlayerData> valueTuple in list)
					{
						PlayerId item = valueTuple.Item1;
						AnotherPlayerData item2 = valueTuple.Item2;
						MPLobbyPlayerBaseVM friendWithID = this.GetFriendWithID(item);
						if (friendWithID != null)
						{
							friendWithID.UpdatePlayerState(item2);
						}
					}
					this.InGameFriends.FriendList.Sort(this._playerStateComparer);
					this.OnlineFriends.FriendList.Sort(this._playerStateComparer);
					this.OfflineFriends.FriendList.Sort(this._playerStateComparer);
				}
				this._isStateRequestActive = false;
				this.UpdateCanInviteOtherPlayersToParty();
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00017585 File Offset: 0x00015785
		private void BlockedUserListChanged()
		{
			this.GetFriends();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001758D File Offset: 0x0001578D
		private void FriendListChanged()
		{
			this.GetFriends();
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00017595 File Offset: 0x00015795
		public void ForceRefresh()
		{
			this.GetFriends();
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000175A0 File Offset: 0x000157A0
		private async void UserOnlineStatusChanged(PlayerId providedId)
		{
			await this.CreateAndAddFriendToList(providedId);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000175E1 File Offset: 0x000157E1
		private void FriendRemoved(PlayerId providedId)
		{
			this.RemoveFriend(providedId);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000175EC File Offset: 0x000157EC
		private void RemoveFriend(PlayerId providedId)
		{
			MPLobbyFriendItemVM mplobbyFriendItemVM = this.InGameFriends.FriendList.ToList<MPLobbyFriendItemVM>().FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == providedId);
			if (mplobbyFriendItemVM != null)
			{
				this.InGameFriends.RemoveFriend(mplobbyFriendItemVM);
				return;
			}
			mplobbyFriendItemVM = this.OnlineFriends.FriendList.ToList<MPLobbyFriendItemVM>().FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == providedId);
			if (mplobbyFriendItemVM != null)
			{
				this.OnlineFriends.RemoveFriend(mplobbyFriendItemVM);
				return;
			}
			mplobbyFriendItemVM = this.OfflineFriends.FriendList.ToList<MPLobbyFriendItemVM>().FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == providedId);
			if (mplobbyFriendItemVM != null)
			{
				this.OfflineFriends.RemoveFriend(mplobbyFriendItemVM);
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001769C File Offset: 0x0001589C
		private async Task CreateAndAddFriendToList(PlayerId playerId)
		{
			if (!MultiplayerPlayerHelper.IsBlocked(playerId))
			{
				this.RemoveFriend(playerId);
				MPLobbyPlayerBaseVM.OnlineStatus onlineStatus = await this.GetOnlineStatus(playerId);
				MPLobbyFriendItemVM mplobbyFriendItemVM = new MPLobbyFriendItemVM(playerId, this._activatePlayerActions, new Action<PlayerId>(this.ExecuteInviteToClan), null)
				{
					CanRemove = (this.FriendListService.AllowsFriendOperations && this.FriendListService.IncludeInAllFriends)
				};
				if (this._isInGameFriendsRelevant)
				{
					if (onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.InGame)
					{
						this.InGameFriends.AddFriend(mplobbyFriendItemVM);
					}
					else if (onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.Online)
					{
						this.OnlineFriends.AddFriend(mplobbyFriendItemVM);
					}
					else if (onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.Offline)
					{
						this.OfflineFriends.AddFriend(mplobbyFriendItemVM);
					}
				}
				else if (onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.InGame || onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.Online)
				{
					this.OnlineFriends.AddFriend(mplobbyFriendItemVM);
				}
				else
				{
					this.OfflineFriends.AddFriend(mplobbyFriendItemVM);
				}
				mplobbyFriendItemVM.OnStatusChanged(onlineStatus, this.IsInGameStatusActive);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000176EC File Offset: 0x000158EC
		private MPLobbyPlayerBaseVM GetFriendWithID(PlayerId playerId)
		{
			MPLobbyFriendItemVM mplobbyFriendItemVM = this._onlineFriends.FriendList.ToList<MPLobbyFriendItemVM>().FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == playerId);
			if (mplobbyFriendItemVM != null)
			{
				return mplobbyFriendItemVM;
			}
			MPLobbyFriendItemVM mplobbyFriendItemVM2 = this._inGameFriends.FriendList.ToList<MPLobbyFriendItemVM>().FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == playerId);
			if (mplobbyFriendItemVM2 != null)
			{
				return mplobbyFriendItemVM2;
			}
			MPLobbyFriendItemVM mplobbyFriendItemVM3 = this._offlineFriends.FriendList.ToList<MPLobbyFriendItemVM>().FirstOrDefault<MPLobbyFriendItemVM>((MPLobbyFriendItemVM p) => p.ProvidedID == playerId);
			if (mplobbyFriendItemVM3 != null)
			{
				return mplobbyFriendItemVM3;
			}
			return null;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001777C File Offset: 0x0001597C
		public void UpdateCanInviteOtherPlayersToParty()
		{
			this.OfflineFriends.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
			{
				f.SetOnInvite(null);
			});
			this.PendingRequests.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
			{
				f.SetOnInvite(null);
			});
			this.FriendRequests.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
			{
				f.SetOnInvite(null);
			});
			this.OnlineFriends.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
			{
				f.SetOnInvite(this.GetOnInvite(f.ProvidedID, f.CurrentOnlineStatus, f.State));
			});
			this.InGameFriends.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
			{
				f.SetOnInvite(this.GetOnInvite(f.ProvidedID, f.CurrentOnlineStatus, f.State));
			});
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00017850 File Offset: 0x00015A50
		public void OnFriendListUpdated(bool updateForced = false)
		{
			if (!this._populatingFriends)
			{
				this.InGameFriends.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
				{
					f.UpdateNameAndAvatar(updateForced);
				});
				this.OnlineFriends.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
				{
					f.UpdateNameAndAvatar(updateForced);
				});
				this.OfflineFriends.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
				{
					f.UpdateNameAndAvatar(updateForced);
				});
				this.FriendRequests.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
				{
					f.UpdateNameAndAvatar(updateForced);
				});
				this.PendingRequests.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM f)
				{
					f.UpdateNameAndAvatar(updateForced);
				});
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00017904 File Offset: 0x00015B04
		private async Task<MPLobbyPlayerBaseVM.OnlineStatus> GetOnlineStatus(PlayerId playerId)
		{
			bool flag = await this.FriendListService.GetUserOnlineStatus(playerId);
			bool isOnline = flag;
			bool flag2 = false;
			if (this.IsInGameStatusActive)
			{
				flag2 = await this.FriendListService.IsPlayingThisGame(playerId);
			}
			MPLobbyPlayerBaseVM.OnlineStatus onlineStatus;
			if (isOnline)
			{
				if (!flag2)
				{
					onlineStatus = MPLobbyPlayerBaseVM.OnlineStatus.Online;
				}
				else
				{
					onlineStatus = MPLobbyPlayerBaseVM.OnlineStatus.InGame;
				}
			}
			else
			{
				onlineStatus = MPLobbyPlayerBaseVM.OnlineStatus.Offline;
			}
			return onlineStatus;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00017954 File Offset: 0x00015B54
		private Action<PlayerId> GetOnInvite(PlayerId playerId, MPLobbyPlayerBaseVM.OnlineStatus onlineStatus, AnotherPlayerState state)
		{
			Action<PlayerId> action = null;
			if (PlatformServices.Instance.UsePlatformInvitationService(playerId) && playerId.ProvidedType == NetworkMain.GameClient.PlayerID.ProvidedType)
			{
				if (ApplicationPlatform.CurrentPlatform == Platform.GDKDesktop && state != AnotherPlayerState.AtLobby)
				{
					action = null;
				}
				else
				{
					action = new Action<PlayerId>(this.ExecuteInviteToPlatformSession);
				}
			}
			else if (onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.Offline || onlineStatus == MPLobbyPlayerBaseVM.OnlineStatus.None)
			{
				action = null;
			}
			else if (state == AnotherPlayerState.AtLobby)
			{
				action = new Action<PlayerId>(this.ExecuteInviteToParty);
			}
			return action;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000179C8 File Offset: 0x00015BC8
		private void ExecuteInviteToParty(PlayerId providedId)
		{
			bool dontUseNameForUnknownPlayer = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(providedId);
			PermissionResult <>9__1;
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					PlayerIdProvidedTypes providedType = providedId.ProvidedType;
					LobbyClient gameClient = NetworkMain.GameClient;
					PlayerIdProvidedTypes? playerIdProvidedTypes = ((gameClient != null) ? new PlayerIdProvidedTypes?(gameClient.PlayerID.ProvidedType) : null);
					if (!((providedType == playerIdProvidedTypes.GetValueOrDefault()) & (playerIdProvidedTypes != null)))
					{
						NetworkMain.GameClient.InviteToParty(providedId, dontUseNameForUnknownPlayer);
						return;
					}
					IPlatformServices instance = PlatformServices.Instance;
					Permission permission = Permission.PlayMultiplayer;
					PlayerId providedId2 = providedId;
					PermissionResult permissionResult2;
					if ((permissionResult2 = <>9__1) == null)
					{
						permissionResult2 = (<>9__1 = delegate(bool permissionResult)
						{
							if (permissionResult)
							{
								NetworkMain.GameClient.InviteToParty(providedId, dontUseNameForUnknownPlayer);
								return;
							}
							string text = new TextObject("{=ZwN6rzTC}No permission", null).ToString();
							string text2 = new TextObject("{=wlz3eQWp}No permission to invite player.", null).ToString();
							InformationManager.ShowInquiry(new InquiryData(text, text2, false, true, "", new TextObject("{=dismissnotification}Dismiss", null).ToString(), null, delegate
							{
								InformationManager.HideInquiry();
							}, "event:/ui/notification/quest_update", 0f, null, null, null), false, false);
						});
					}
					instance.CheckPermissionWithUser(permission, providedId2, permissionResult2);
				}
			});
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00017A20 File Offset: 0x00015C20
		private void ExecuteInviteToPlatformSession(PlayerId providedId)
		{
			MPLobbyFriendServiceVM.<>c__DisplayClass43_0 CS$<>8__locals1 = new MPLobbyFriendServiceVM.<>c__DisplayClass43_0();
			CS$<>8__locals1.providedId = providedId;
			CS$<>8__locals1.friend = this.GetFriendWithID(CS$<>8__locals1.providedId);
			CS$<>8__locals1.friend.CanBeInvited = false;
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					IPlatformServices instance = PlatformServices.Instance;
					Permission permission = Permission.PlayMultiplayer;
					PlayerId providedId2 = CS$<>8__locals1.providedId;
					PermissionResult permissionResult2;
					if ((permissionResult2 = CS$<>8__locals1.<>9__1) == null)
					{
						permissionResult2 = (CS$<>8__locals1.<>9__1 = delegate(bool permissionResult)
						{
							MPLobbyFriendServiceVM.<>c__DisplayClass43_0.<<ExecuteInviteToPlatformSession>b__1>d <<ExecuteInviteToPlatformSession>b__1>d;
							<<ExecuteInviteToPlatformSession>b__1>d.<>4__this = CS$<>8__locals1;
							<<ExecuteInviteToPlatformSession>b__1>d.permissionResult = permissionResult;
							<<ExecuteInviteToPlatformSession>b__1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<ExecuteInviteToPlatformSession>b__1>d.<>1__state = -1;
							AsyncVoidMethodBuilder <>t__builder = <<ExecuteInviteToPlatformSession>b__1>d.<>t__builder;
							<>t__builder.Start<MPLobbyFriendServiceVM.<>c__DisplayClass43_0.<<ExecuteInviteToPlatformSession>b__1>d>(ref <<ExecuteInviteToPlatformSession>b__1>d);
						});
					}
					instance.CheckPermissionWithUser(permission, providedId2, permissionResult2);
					return;
				}
				CS$<>8__locals1.friend.CanBeInvited = true;
			});
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00017A70 File Offset: 0x00015C70
		private void ExecuteInviteToClan(PlayerId providedId)
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Clan, true, delegate(bool result)
			{
				if (result)
				{
					bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(providedId);
					NetworkMain.GameClient.InviteToClan(providedId, flag);
				}
			});
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00017AA2 File Offset: 0x00015CA2
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x00017AAA File Offset: 0x00015CAA
		[DataSourceProperty]
		public bool IsInGameStatusActive
		{
			get
			{
				return this._isInGameStatusActive;
			}
			set
			{
				if (value != this._isInGameStatusActive)
				{
					this._isInGameStatusActive = value;
					base.OnPropertyChangedWithValue(value, "IsInGameStatusActive");
				}
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00017AC8 File Offset: 0x00015CC8
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x00017AD0 File Offset: 0x00015CD0
		[DataSourceProperty]
		public MPLobbyFriendGroupVM InGameFriends
		{
			get
			{
				return this._inGameFriends;
			}
			set
			{
				if (value != this._inGameFriends)
				{
					this._inGameFriends = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendGroupVM>(value, "InGameFriends");
				}
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00017AEE File Offset: 0x00015CEE
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x00017AF6 File Offset: 0x00015CF6
		[DataSourceProperty]
		public MPLobbyFriendGroupVM OnlineFriends
		{
			get
			{
				return this._onlineFriends;
			}
			set
			{
				if (value != this._onlineFriends)
				{
					this._onlineFriends = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendGroupVM>(value, "OnlineFriends");
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00017B14 File Offset: 0x00015D14
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x00017B1C File Offset: 0x00015D1C
		[DataSourceProperty]
		public MPLobbyFriendGroupVM OfflineFriends
		{
			get
			{
				return this._offlineFriends;
			}
			set
			{
				if (value != this._offlineFriends)
				{
					this._offlineFriends = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendGroupVM>(value, "OfflineFriends");
				}
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00017B3A File Offset: 0x00015D3A
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00017B42 File Offset: 0x00015D42
		[DataSourceProperty]
		public string InGameText
		{
			get
			{
				return this._inGameText;
			}
			set
			{
				if (value != this._inGameText)
				{
					this._inGameText = value;
					base.OnPropertyChangedWithValue<string>(value, "InGameText");
				}
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00017B65 File Offset: 0x00015D65
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x00017B6D File Offset: 0x00015D6D
		[DataSourceProperty]
		public string OnlineText
		{
			get
			{
				return this._onlineText;
			}
			set
			{
				if (value != this._onlineText)
				{
					this._onlineText = value;
					base.OnPropertyChangedWithValue<string>(value, "OnlineText");
				}
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00017B90 File Offset: 0x00015D90
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00017B98 File Offset: 0x00015D98
		[DataSourceProperty]
		public string OfflineText
		{
			get
			{
				return this._offlineText;
			}
			set
			{
				if (value != this._offlineText)
				{
					this._offlineText = value;
					base.OnPropertyChangedWithValue<string>(value, "OfflineText");
				}
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00017BBB File Offset: 0x00015DBB
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x00017BC3 File Offset: 0x00015DC3
		[DataSourceProperty]
		public string ServiceName
		{
			get
			{
				return this._serviceName;
			}
			set
			{
				if (value != this._serviceName)
				{
					this._serviceName = value;
					base.OnPropertyChangedWithValue<string>(value, "ServiceName");
				}
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00017BE6 File Offset: 0x00015DE6
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x00017BEE File Offset: 0x00015DEE
		[DataSourceProperty]
		public MPLobbyFriendGroupVM FriendRequests
		{
			get
			{
				return this._friendRequests;
			}
			set
			{
				if (value != this._friendRequests)
				{
					this._friendRequests = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendGroupVM>(value, "FriendRequests");
				}
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00017C0C File Offset: 0x00015E0C
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x00017C14 File Offset: 0x00015E14
		[DataSourceProperty]
		public bool GotAnyFriendRequests
		{
			get
			{
				return this._gotAnyFriendRequests;
			}
			set
			{
				if (value != this._gotAnyFriendRequests)
				{
					this._gotAnyFriendRequests = value;
					base.OnPropertyChangedWithValue(value, "GotAnyFriendRequests");
				}
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00017C32 File Offset: 0x00015E32
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x00017C3A File Offset: 0x00015E3A
		[DataSourceProperty]
		public MPLobbyFriendGroupVM PendingRequests
		{
			get
			{
				return this._pendingRequests;
			}
			set
			{
				if (value != this._pendingRequests)
				{
					this._pendingRequests = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendGroupVM>(value, "PendingRequests");
				}
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00017C58 File Offset: 0x00015E58
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00017C60 File Offset: 0x00015E60
		[DataSourceProperty]
		public bool GotAnyPendingRequests
		{
			get
			{
				return this._gotAnyPendingRequests;
			}
			set
			{
				if (value != this._gotAnyPendingRequests)
				{
					this._gotAnyPendingRequests = value;
					base.OnPropertyChangedWithValue(value, "GotAnyPendingRequests");
				}
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00017C7E File Offset: 0x00015E7E
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x00017C86 File Offset: 0x00015E86
		[DataSourceProperty]
		public HintViewModel ServiceNameHint
		{
			get
			{
				return this._serviceNameHint;
			}
			set
			{
				if (value != this._serviceNameHint)
				{
					this._serviceNameHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ServiceNameHint");
				}
			}
		}

		// Token: 0x04000358 RID: 856
		private const string _inviteFailedSoundEvent = "event:/ui/notification/quest_update";

		// Token: 0x04000359 RID: 857
		public readonly IFriendListService FriendListService;

		// Token: 0x0400035A RID: 858
		private readonly Action<MPLobbyPlayerBaseVM> _activatePlayerActions;

		// Token: 0x0400035B RID: 859
		private bool _populatingFriends;

		// Token: 0x0400035C RID: 860
		private bool _isInGameFriendsRelevant;

		// Token: 0x0400035D RID: 861
		private const float UpdateInterval = 2f;

		// Token: 0x0400035E RID: 862
		private float _lastUpdateTimePassed;

		// Token: 0x0400035F RID: 863
		private const float StateRequestInterval = 10f;

		// Token: 0x04000360 RID: 864
		private float _lastStateRequestTimePassed;

		// Token: 0x04000361 RID: 865
		private bool _isStateRequestActive;

		// Token: 0x04000362 RID: 866
		private readonly MPLobbyFriendServiceVM.PlayerStateComparer _playerStateComparer;

		// Token: 0x04000363 RID: 867
		private Action<PlayerId> _onFriendRequestAnswered;

		// Token: 0x04000364 RID: 868
		private bool _isPartyAvailable;

		// Token: 0x04000365 RID: 869
		private static Dictionary<PlayerId, long> _friendRequestsInProcess = new Dictionary<PlayerId, long>();

		// Token: 0x04000366 RID: 870
		private const int BlockedFriendRequestTimeout = 10000;

		// Token: 0x04000367 RID: 871
		private bool _isInGameStatusActive;

		// Token: 0x04000368 RID: 872
		private MPLobbyFriendGroupVM _inGameFriends;

		// Token: 0x04000369 RID: 873
		private MPLobbyFriendGroupVM _onlineFriends;

		// Token: 0x0400036A RID: 874
		private MPLobbyFriendGroupVM _offlineFriends;

		// Token: 0x0400036B RID: 875
		private string _inGameText;

		// Token: 0x0400036C RID: 876
		private string _onlineText;

		// Token: 0x0400036D RID: 877
		private string _offlineText;

		// Token: 0x0400036E RID: 878
		private string _serviceName;

		// Token: 0x0400036F RID: 879
		private HintViewModel _serviceNameHint;

		// Token: 0x04000370 RID: 880
		private MPLobbyFriendGroupVM _friendRequests;

		// Token: 0x04000371 RID: 881
		private bool _gotAnyFriendRequests;

		// Token: 0x04000372 RID: 882
		private MPLobbyFriendGroupVM _pendingRequests;

		// Token: 0x04000373 RID: 883
		private bool _gotAnyPendingRequests;

		// Token: 0x02000103 RID: 259
		private class PlayerStateComparer : IComparer<MPLobbyPlayerBaseVM>
		{
			// Token: 0x06001196 RID: 4502 RVA: 0x00036C68 File Offset: 0x00034E68
			public int Compare(MPLobbyPlayerBaseVM x, MPLobbyPlayerBaseVM y)
			{
				int stateImportanceOrder = this.GetStateImportanceOrder(x.State);
				int stateImportanceOrder2 = this.GetStateImportanceOrder(y.State);
				if (stateImportanceOrder != stateImportanceOrder2)
				{
					return stateImportanceOrder.CompareTo(stateImportanceOrder2);
				}
				return x.Name.CompareTo(y.Name);
			}

			// Token: 0x06001197 RID: 4503 RVA: 0x00036CAD File Offset: 0x00034EAD
			private int GetStateImportanceOrder(AnotherPlayerState state)
			{
				switch (state)
				{
				case AnotherPlayerState.AtLobby:
					return 0;
				case AnotherPlayerState.InParty:
					return 1;
				case AnotherPlayerState.InMultiplayerGame:
					return 2;
				default:
					return int.MaxValue;
				}
			}
		}
	}
}
