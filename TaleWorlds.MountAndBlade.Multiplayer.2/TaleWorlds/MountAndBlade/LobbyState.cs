using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.AccessProvider.Test;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000F RID: 15
	public class LobbyState : GameState
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003CC1 File Offset: 0x00001EC1
		private bool AutoConnect
		{
			get
			{
				return TestCommonBase.BaseInstance == null || !TestCommonBase.BaseInstance.IsTestEnabled;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003CD9 File Offset: 0x00001ED9
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003CDC File Offset: 0x00001EDC
		public override bool IsMusicMenuState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003CDF File Offset: 0x00001EDF
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003CE7 File Offset: 0x00001EE7
		public bool IsLoggingIn { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003CF0 File Offset: 0x00001EF0
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003CF8 File Offset: 0x00001EF8
		public ILobbyStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003D01 File Offset: 0x00001F01
		public LobbyClient LobbyClient
		{
			get
			{
				return NetworkMain.GameClient;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003D08 File Offset: 0x00001F08
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003D10 File Offset: 0x00001F10
		public NewsManager NewsManager { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600009F RID: 159 RVA: 0x00003D1C File Offset: 0x00001F1C
		// (remove) Token: 0x060000A0 RID: 160 RVA: 0x00003D54 File Offset: 0x00001F54
		public event Action<GameServerEntry> ClientRefusedToJoinCustomServer;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003D89 File Offset: 0x00001F89
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003D91 File Offset: 0x00001F91
		public bool? HasMultiplayerPrivilege { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003D9A File Offset: 0x00001F9A
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003DA2 File Offset: 0x00001FA2
		public bool? HasCrossplayPrivilege { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003DAB File Offset: 0x00001FAB
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003DB3 File Offset: 0x00001FB3
		public bool? HasUserGeneratedContentPrivilege { get; private set; }

		// Token: 0x060000A7 RID: 167 RVA: 0x00003DBC File Offset: 0x00001FBC
		public LobbyState()
		{
			this._registeredPermissionEvents = new ConcurrentDictionary<ValueTuple<PlayerId, Permission>, bool>();
			this._onCustomServerActionRequestedForServerEntry = new List<Func<GameServerEntry, List<CustomServerAction>>>();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003DE5 File Offset: 0x00001FE5
		public void InitializeLogic(ILobbyStateHandler lobbyStateHandler)
		{
			this.Handler = lobbyStateHandler;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003DF0 File Offset: 0x00001FF0
		protected override async void OnInitialize()
		{
			base.OnInitialize();
			MultiplayerLocalDataManager.InitializeManager();
			CommunityClient communityClient = NetworkMain.CommunityClient;
			CommunityClientOnlineLobbyGameHandler communityClientOnlineLobbyGameHandler = new CommunityClientOnlineLobbyGameHandler(this);
			communityClient.Handler = communityClientOnlineLobbyGameHandler;
			this.LobbyClient.SetLoadedModules(Utilities.GetModulesNames());
			PlatformServices.Instance.OnSignInStateUpdated += this.OnPlatformSignInStateUpdated;
			PlatformServices.Instance.OnNameUpdated += this.OnPlayerNameUpdated;
			foreach (IFriendListService friendListService in PlatformServices.Instance.GetFriendListServices())
			{
				Type type = friendListService.GetType();
				if (type == typeof(BannerlordFriendListService))
				{
					this._bannerlordFriendListService = (BannerlordFriendListService)friendListService;
				}
				else if (type == typeof(RecentPlayersFriendListService))
				{
					this._recentPlayersFriendListService = (RecentPlayersFriendListService)friendListService;
				}
				else if (type == typeof(ClanFriendListService))
				{
					this._clanFriendListService = (ClanFriendListService)friendListService;
				}
			}
			this.NewsManager = new NewsManager();
			this.NewsManager.SetNewsSourceURL(this.GetApplicableNewsSourceURL());
			RecentPlayersManager.Initialize();
			this._onCustomServerActionRequestedForServerEntry = new List<Func<GameServerEntry, List<CustomServerAction>>>();
			this._lobbyGameClientManager = new LobbyGameClientHandler();
			this._lobbyGameClientManager.LobbyState = this;
			this.NewsManager.UpdateNewsItems(false);
			if (this.HasMultiplayerPrivilege.GetValueOrDefault() && this.AutoConnect)
			{
				await this.TryLogin();
			}
			else
			{
				this.SetConnectionState(false);
				this.OnResume();
			}
			if (PlatformServices.SessionInvitationType != SessionInvitationType.None)
			{
				this.OnSessionInvitationAccepted(PlatformServices.SessionInvitationType);
			}
			else if (PlatformServices.IsPlatformRequestedMultiplayer)
			{
				this.OnPlatformRequestedMultiplayer();
			}
			PlatformServices.OnSessionInvitationAccepted = (Action<SessionInvitationType>)Delegate.Combine(PlatformServices.OnSessionInvitationAccepted, new Action<SessionInvitationType>(this.OnSessionInvitationAccepted));
			PlatformServices.OnPlatformRequestedMultiplayer = (Action)Delegate.Combine(PlatformServices.OnPlatformRequestedMultiplayer, new Action(this.OnPlatformRequestedMultiplayer));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003E29 File Offset: 0x00002029
		protected override void OnActivate()
		{
			base.OnActivate();
			MultiplayerIntermissionVotingManager.Instance.UsableMaps.Clear();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003E40 File Offset: 0x00002040
		private void OnPlayerNameUpdated(string newName)
		{
			this.LobbyClient.OnPlayerNameUpdated(newName);
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerNameUpdated(newName);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003E60 File Offset: 0x00002060
		protected override void OnFinalize()
		{
			base.OnFinalize();
			MultiplayerLocalDataManager.FinalizeManager();
			PlatformServices.OnPlatformRequestedMultiplayer = (Action)Delegate.Remove(PlatformServices.OnPlatformRequestedMultiplayer, new Action(this.OnPlatformRequestedMultiplayer));
			PlatformServices.OnSessionInvitationAccepted = (Action<SessionInvitationType>)Delegate.Remove(PlatformServices.OnSessionInvitationAccepted, new Action<SessionInvitationType>(this.OnSessionInvitationAccepted));
			PlatformServices.Instance.OnSignInStateUpdated -= this.OnPlatformSignInStateUpdated;
			PlatformServices.Instance.OnNameUpdated -= this.OnPlayerNameUpdated;
			this.LobbyClient.RemoveLobbyClientHandler();
			RecentPlayersManager.Serialize();
			this.NewsManager.OnFinalize();
			this.NewsManager = null;
			this._onCustomServerActionRequestedForServerEntry.Clear();
			this._onCustomServerActionRequestedForServerEntry = null;
			foreach (ValueTuple<PlayerId, Permission> valueTuple in this._registeredPermissionEvents.Keys)
			{
				if (PlatformServices.Instance.UnregisterPermissionChangeEvent(valueTuple.Item1, valueTuple.Item2, new PermissionChanged(this.MultiplayerPermissionWithPlayerChanged)))
				{
					bool flag;
					this._registeredPermissionEvents.TryRemove(new ValueTuple<PlayerId, Permission>(valueTuple.Item1, valueTuple.Item2), out flag);
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003F9C File Offset: 0x0000219C
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			MultiplayerLocalDataManager.Instance.Tick(dt);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003FB0 File Offset: 0x000021B0
		private string GetApplicableNewsSourceURL()
		{
			bool flag = this.NewsManager.LocalizationID == "zh";
			bool isInPreviewMode = this.NewsManager.IsInPreviewMode;
			string text = (flag ? "zh" : "en");
			if (!isInPreviewMode)
			{
				return "https://cdn.taleworlds.com/upload/bannerlordnews/NewsFeed_" + text + ".json";
			}
			return "https://cdn.taleworlds.com/upload/bannerlordnews/NewsFeed_" + text + "_preview.json";
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004011 File Offset: 0x00002211
		private string GetApplicableAnnouncementsURL()
		{
			return "https://cdn.taleworlds.com/bannerlord-ingame/LobbyNewsFeed.json";
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004018 File Offset: 0x00002218
		[Conditional("_RGL_KEEP_ASSERTS")]
		private void CheckValidityOfItems()
		{
			foreach (ItemObject itemObject in MBObjectManager.Instance.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.IsUsingTeamColor)
				{
					MetaMesh copy = MetaMesh.GetCopy(itemObject.MultiMeshName, false, false);
					for (int i = 0; i < copy.MeshCount; i++)
					{
						Material material = copy.GetMeshAtIndex(i).GetMaterial();
						if (material.Name != "vertex_color_lighting_skinned" && material.Name != "vertex_color_lighting" && material.GetTexture(Material.MBTextureType.DiffuseMap2) == null)
						{
							MBDebug.ShowWarning("Item object(" + itemObject.Name + ") has 'Using Team Color' flag but does not have a mask texture in diffuse2 slot. ");
							break;
						}
					}
				}
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000040F8 File Offset: 0x000022F8
		public async Task UpdateHasMultiplayerPrivilege()
		{
			TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
			PlatformServices.Instance.CheckPrivilege(Privilege.Multiplayer, true, delegate(bool result)
			{
				tsc.SetResult(result);
			});
			bool flag = await tsc.Task;
			this.HasMultiplayerPrivilege = new bool?(flag);
			Action<bool> onMultiplayerPrivilegeUpdated = this.OnMultiplayerPrivilegeUpdated;
			if (onMultiplayerPrivilegeUpdated != null)
			{
				onMultiplayerPrivilegeUpdated(this.HasMultiplayerPrivilege.Value);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004140 File Offset: 0x00002340
		public async Task UpdateHasCrossplayPrivilege()
		{
			TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
			PlatformServices.Instance.CheckPrivilege(Privilege.Crossplay, false, delegate(bool result)
			{
				tsc.SetResult(result);
			});
			bool flag = await tsc.Task;
			this.HasCrossplayPrivilege = new bool?(flag);
			Action<bool> onCrossplayPrivilegeUpdated = this.OnCrossplayPrivilegeUpdated;
			if (onCrossplayPrivilegeUpdated != null)
			{
				onCrossplayPrivilegeUpdated(this.HasCrossplayPrivilege.Value);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004185 File Offset: 0x00002385
		public void OnClientRefusedToJoinCustomServer(GameServerEntry serverEntry)
		{
			Action<GameServerEntry> clientRefusedToJoinCustomServer = this.ClientRefusedToJoinCustomServer;
			if (clientRefusedToJoinCustomServer == null)
			{
				return;
			}
			clientRefusedToJoinCustomServer(serverEntry);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004198 File Offset: 0x00002398
		public async Task UpdateHasUserGeneratedContentPrivilege(bool showResolveUI)
		{
			TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
			PlatformServices.Instance.CheckPrivilege(Privilege.UserGeneratedContent, showResolveUI, delegate(bool result)
			{
				tsc.SetResult(result);
			});
			bool flag = await tsc.Task;
			this.HasUserGeneratedContentPrivilege = new bool?(flag);
			Action<bool> onUserGeneratedContentPrivilegeUpdated = this.OnUserGeneratedContentPrivilegeUpdated;
			if (onUserGeneratedContentPrivilegeUpdated != null)
			{
				onUserGeneratedContentPrivilegeUpdated(this.HasUserGeneratedContentPrivilege.Value);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000041E8 File Offset: 0x000023E8
		public async Task TryLogin()
		{
			this.IsLoggingIn = true;
			LobbyClient gameClient = this.LobbyClient;
			if (gameClient.IsIdle)
			{
				await this.UpdateHasMultiplayerPrivilege();
				if (!this.HasMultiplayerPrivilege.Value)
				{
					this.ShowFeedback(new TextObject("{=lVfmVHbz}Login Failed", null).ToString(), new TextObject("{=cS0Hafjl}Player does not have access to multiplayer.", null).ToString());
					this.IsLoggingIn = false;
					return;
				}
				await this.UpdateHasCrossplayPrivilege();
				await this.UpdateHasUserGeneratedContentPrivilege(false);
				ILoginAccessProvider loginAccessProvider = await PlatformServices.Instance.CreateLobbyClientLoginProvider();
				string userName = loginAccessProvider.GetUserName();
				Func<Task<bool>> func = null;
				if (PlatformServices.InvitationServices != null)
				{
					func = async () => await PlatformServices.InvitationServices.OnLogin();
				}
				LobbyClient lobbyClient = gameClient;
				ILobbyClientSessionHandler lobbyGameClientManager = this._lobbyGameClientManager;
				ILoginAccessProvider loginAccessProvider2 = loginAccessProvider;
				string text = userName;
				bool? hasUserGeneratedContentPrivilege = this.HasUserGeneratedContentPrivilege;
				bool flag = true;
				LobbyClientConnectResult lobbyClientConnectResult = await lobbyClient.Connect(lobbyGameClientManager, loginAccessProvider2, text, (hasUserGeneratedContentPrivilege.GetValueOrDefault() == flag) & (hasUserGeneratedContentPrivilege != null), PlatformServices.Instance.GetInitParams(), func);
				if (lobbyClientConnectResult.Connected)
				{
					Game.Current.GetGameHandler<ChatBox>().OnLogin();
					this.OnResume();
				}
				else
				{
					this.ShowFeedback(new TextObject("{=lVfmVHbz}Login Failed", null).ToString(), lobbyClientConnectResult.Error.ToString());
					this.SetConnectionState(false);
					this.OnResume();
				}
			}
			this.IsLoggingIn = false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004230 File Offset: 0x00002430
		public async Task TryLogin(string userName, string password)
		{
			this.IsLoggingIn = true;
			LobbyClientConnectResult lobbyClientConnectResult = await NetworkMain.GameClient.Connect(this._lobbyGameClientManager, new TestLoginAccessProvider(), userName, true, PlatformServices.Instance.GetInitParams(), null);
			if (!lobbyClientConnectResult.Connected)
			{
				this.ShowFeedback(new TextObject("{=lVfmVHbz}Login Failed", null).ToString(), lobbyClientConnectResult.Error.ToString());
			}
			this.IsLoggingIn = false;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004280 File Offset: 0x00002480
		public void HostGame()
		{
			if (string.IsNullOrEmpty(MultiplayerOptions.OptionType.ServerName.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)))
			{
				MultiplayerOptions.OptionType.ServerName.SetValue(NetworkMain.GameClient.Name, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			string strValue = MultiplayerOptions.OptionType.GamePassword.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue2 = MultiplayerOptions.OptionType.AdminPassword.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string text = ((!string.IsNullOrEmpty(strValue)) ? Common.CalculateMD5Hash(strValue) : null);
			string text2 = ((!string.IsNullOrEmpty(strValue2)) ? Common.CalculateMD5Hash(strValue2) : null);
			MultiplayerOptions.OptionType.GamePassword.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			MultiplayerOptions.OptionType.AdminPassword.SetValue(text2, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue3 = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string gameModule = MultiplayerGameTypes.GetGameTypeInfo(strValue3).GameModule;
			string strValue4 = MultiplayerOptions.OptionType.Map.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string text3 = null;
			UniqueSceneId uniqueSceneId;
			if (Utilities.TryGetUniqueIdentifiersForScene(strValue4, out uniqueSceneId))
			{
				text3 = uniqueSceneId.Serialize();
			}
			NetworkMain.GameClient.RegisterCustomGame(gameModule, strValue3, MultiplayerOptions.OptionType.ServerName.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), strValue4, text3, MultiplayerOptions.OptionType.GamePassword.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), MultiplayerOptions.OptionType.AdminPassword.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), 9999);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004360 File Offset: 0x00002560
		public void CreatePremadeGame()
		{
			string strValue = MultiplayerOptions.OptionType.ServerName.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue2 = MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue3 = MultiplayerOptions.OptionType.Map.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue4 = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue5 = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue6 = MultiplayerOptions.OptionType.GamePassword.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			PremadeGameType premadeGameType = (PremadeGameType)Enum.GetValues(typeof(PremadeGameType)).GetValue(MultiplayerOptions.OptionType.PremadeGameType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			if (premadeGameType == PremadeGameType.Clan)
			{
				bool flag = true;
				using (List<PartyPlayerInLobbyClient>.Enumerator enumerator = NetworkMain.GameClient.PlayersInParty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PartyPlayerInLobbyClient partyPlayer = enumerator.Current;
						if (NetworkMain.GameClient.PlayersInClan.FirstOrDefault<ClanPlayer>((ClanPlayer clanPlayer) => clanPlayer.PlayerId == partyPlayer.PlayerId) == null)
						{
							flag = false;
						}
					}
				}
				if (!flag)
				{
					this.ShowFeedback(new TextObject("{=oZrVNUOk}Error", null).ToString(), new TextObject("{=uNrXwGzr}Only practice matches are allowed with your current party. All members should be in the same clan for a clan match.", null).ToString());
					return;
				}
			}
			if (strValue != null && !strValue.IsEmpty<char>() && premadeGameType != PremadeGameType.Invalid)
			{
				NetworkMain.GameClient.CreatePremadeGame(strValue, strValue2, strValue3, strValue4, strValue5, strValue6, premadeGameType);
				return;
			}
			if (premadeGameType == PremadeGameType.Invalid)
			{
				this.ShowFeedback(new TextObject("{=oZrVNUOk}Error", null).ToString(), new TextObject("{=PfnS8HUd}Premade game type is invalid!", null).ToString());
				return;
			}
			this.ShowFeedback(new TextObject("{=oZrVNUOk}Error", null).ToString(), new TextObject("{=EgTUzWUz}Name Can't Be Empty!", null).ToString());
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000044E8 File Offset: 0x000026E8
		public string ShowFeedback(string title, string message)
		{
			if (this.Handler != null)
			{
				return this.Handler.ShowFeedback(title, message);
			}
			return null;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004501 File Offset: 0x00002701
		public string ShowFeedback(InquiryData inquiryData)
		{
			if (this.Handler != null)
			{
				return this.Handler.ShowFeedback(inquiryData);
			}
			return null;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004519 File Offset: 0x00002719
		public void DismissFeedback(string messageId)
		{
			if (this.Handler != null)
			{
				this.Handler.DismissFeedback(messageId);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000452F File Offset: 0x0000272F
		public void OnPause()
		{
			if (this.Handler != null)
			{
				this.Handler.OnPause();
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004544 File Offset: 0x00002744
		public void OnResume()
		{
			if (this.Handler != null)
			{
				this.Handler.OnResume();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004559 File Offset: 0x00002759
		public void OnRequestedToSearchBattle()
		{
			if (this.Handler != null)
			{
				this.Handler.OnRequestedToSearchBattle();
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000456E File Offset: 0x0000276E
		public void OnUpdateFindingGame(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo = null)
		{
			if (this.Handler != null)
			{
				this.Handler.OnUpdateFindingGame(matchmakingWaitTimeStats, gameTypeInfo);
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004585 File Offset: 0x00002785
		public void OnRequestedToCancelSearchBattle()
		{
			if (this.Handler != null)
			{
				this.Handler.OnRequestedToCancelSearchBattle();
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000459A File Offset: 0x0000279A
		public void OnCancelFindingGame()
		{
			if (this.Handler != null)
			{
				this.Handler.OnSearchBattleCanceled();
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000045B0 File Offset: 0x000027B0
		public void OnDisconnected(TextObject feedback)
		{
			if (this.Handler != null)
			{
				this.Handler.OnDisconnected();
			}
			if (feedback != null)
			{
				string text = new TextObject("{=MbXatV1Q}Disconnected", null).ToString();
				this.ShowFeedback(text, feedback.ToString());
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000045F8 File Offset: 0x000027F8
		public void OnPlayerDataReceived(PlayerData playerData)
		{
			if (this.Handler != null)
			{
				this.Handler.OnPlayerDataReceived(playerData);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000460E File Offset: 0x0000280E
		public void OnPendingRejoin()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPendingRejoin();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004620 File Offset: 0x00002820
		public void OnEnterBattleWithParty(string[] selectedGameTypes)
		{
			if (this.Handler != null)
			{
				this.Handler.OnEnterBattleWithParty(selectedGameTypes);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004638 File Offset: 0x00002838
		public async void OnPartyInvitationReceived(string inviterPlayerName, PlayerId playerId)
		{
			while (this.IsLoggingIn)
			{
				Debug.Print("Waiting for logging in to be done..", 0, Debug.DebugColor.White, 17592186044416UL);
				await Task.Delay(100);
			}
			if (PermaMuteList.IsPlayerMuted(playerId))
			{
				this.LobbyClient.DeclinePartyInvitation();
			}
			else if (this.Handler != null)
			{
				PermissionResult <>9__1;
				PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool privilegeResult)
				{
					if (!privilegeResult)
					{
						this.LobbyClient.DeclinePartyInvitation();
						return;
					}
					if (playerId.ProvidedType == NetworkMain.GameClient.PlayerID.ProvidedType)
					{
						IPlatformServices instance = PlatformServices.Instance;
						Permission permission = Permission.CommunicateUsingText;
						PlayerId playerId2 = playerId;
						PermissionResult permissionResult2;
						if ((permissionResult2 = <>9__1) == null)
						{
							permissionResult2 = (<>9__1 = delegate(bool permissionResult)
							{
								if (!permissionResult)
								{
									this.LobbyClient.DeclinePartyInvitation();
									return;
								}
								ILobbyStateHandler handler2 = this.Handler;
								if (handler2 == null)
								{
									return;
								}
								handler2.OnPartyInvitationReceived(playerId);
							});
						}
						instance.CheckPermissionWithUser(permission, playerId2, permissionResult2);
						return;
					}
					ILobbyStateHandler handler = this.Handler;
					if (handler == null)
					{
						return;
					}
					handler.OnPartyInvitationReceived(playerId);
				});
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000467C File Offset: 0x0000287C
		public async void OnPartyJoinRequestReceived(PlayerId joiningPlayerId, PlayerId viaPlayerId, string viaFriendName)
		{
			while (this.IsLoggingIn)
			{
				Debug.Print("Waiting for logging in to be done..", 0, Debug.DebugColor.White, 17592186044416UL);
				await Task.Delay(100);
			}
			if (PermaMuteList.IsPlayerMuted(joiningPlayerId))
			{
				this.LobbyClient.DeclinePartyJoinRequest(joiningPlayerId, PartyJoinDeclineReason.NoPlatformPermission);
			}
			else if (this.Handler != null)
			{
				if (joiningPlayerId.ProvidedType != NetworkMain.GameClient.PlayerID.ProvidedType)
				{
					ILobbyStateHandler handler = this.Handler;
					if (handler != null)
					{
						handler.OnPartyJoinRequestReceived(joiningPlayerId, viaPlayerId, viaFriendName, !this.LobbyClient.IsInParty);
					}
				}
				else
				{
					PlatformServices.Instance.CheckPermissionWithUser(Permission.CommunicateUsingText, joiningPlayerId, delegate(bool permissionResult)
					{
						if (!permissionResult)
						{
							this.LobbyClient.DeclinePartyJoinRequest(joiningPlayerId, PartyJoinDeclineReason.NoPlatformPermission);
							return;
						}
						ILobbyStateHandler handler2 = this.Handler;
						if (handler2 == null)
						{
							return;
						}
						handler2.OnPartyJoinRequestReceived(joiningPlayerId, viaPlayerId, viaFriendName, !this.LobbyClient.IsInParty);
					});
				}
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000046CD File Offset: 0x000028CD
		public void OnAdminMessageReceived(string message)
		{
			if (this.Handler != null)
			{
				this.Handler.OnAdminMessageReceived(message);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000046E3 File Offset: 0x000028E3
		public void OnPartyInvitationInvalidated()
		{
			if (this.Handler != null)
			{
				this.Handler.OnPartyInvitationInvalidated();
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000046F8 File Offset: 0x000028F8
		public void OnPlayerInvitedToParty(PlayerId playerId)
		{
			if (this.Handler != null)
			{
				this.Handler.OnPlayerInvitedToParty(playerId);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004710 File Offset: 0x00002910
		public void OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason)
		{
			if (playerId.Equals(this.LobbyClient.PlayerID))
			{
				IPlatformInvitationServices invitationServices = PlatformServices.InvitationServices;
				if (invitationServices != null)
				{
					invitationServices.OnLeftParty();
				}
			}
			if (PlatformServices.Instance.UnregisterPermissionChangeEvent(playerId, Permission.PlayMultiplayer, new PermissionChanged(this.MultiplayerPermissionWithPlayerChanged)))
			{
				bool flag;
				this._registeredPermissionEvents.TryRemove(new ValueTuple<PlayerId, Permission>(playerId, Permission.PlayMultiplayer), out flag);
			}
			if (this.Handler != null)
			{
				this.Handler.OnPlayerRemovedFromParty(playerId, reason);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004788 File Offset: 0x00002988
		public void OnPlayersAddedToParty([TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })] List<ValueTuple<PlayerId, string, bool>> addedPlayers, [TupleElementNames(new string[] { "PlayerId", "PlayerName" })] List<ValueTuple<PlayerId, string>> invitedPlayers)
		{
			using (List<ValueTuple<PlayerId, string, bool>>.Enumerator enumerator = addedPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<PlayerId, string, bool> player = enumerator.Current;
					PlayerId item = player.Item1;
					if (item.ProvidedType != this.LobbyClient.PlayerID.ProvidedType)
					{
						ILobbyStateHandler handler = this.Handler;
						if (handler != null)
						{
							handler.OnPlayerAddedToParty(player.Item1, player.Item2, player.Item3);
						}
					}
					else
					{
						PlatformServices.Instance.CheckPermissionWithUser(Permission.PlayMultiplayer, player.Item1, delegate(bool hasPermission)
						{
							if (!hasPermission)
							{
								NetworkMain.GameClient.KickPlayerFromParty(NetworkMain.GameClient.PlayerID);
								return;
							}
							if (PlatformServices.Instance.RegisterPermissionChangeEvent(player.Item1, Permission.PlayMultiplayer, new PermissionChanged(this.MultiplayerPermissionWithPlayerChanged)))
							{
								bool flag;
								this._registeredPermissionEvents.TryRemove(new ValueTuple<PlayerId, Permission>(player.Item1, Permission.PlayMultiplayer), out flag);
							}
							ILobbyStateHandler handler3 = this.Handler;
							if (handler3 == null)
							{
								return;
							}
							handler3.OnPlayerAddedToParty(player.Item1, player.Item2, player.Item3);
						});
					}
				}
			}
			if (this.Handler != null)
			{
				foreach (ValueTuple<PlayerId, string> valueTuple in invitedPlayers)
				{
					PlayerId playerId = valueTuple.Item1;
					if (playerId.ProvidedType != this.LobbyClient.PlayerID.ProvidedType)
					{
						ILobbyStateHandler handler2 = this.Handler;
						if (handler2 != null)
						{
							handler2.OnPlayerInvitedToParty(playerId);
						}
					}
					else
					{
						PlatformServices.Instance.CheckPermissionWithUser(Permission.PlayMultiplayer, playerId, delegate(bool hasPermission)
						{
							if (hasPermission)
							{
								ILobbyStateHandler handler4 = this.Handler;
								if (handler4 == null)
								{
									return;
								}
								handler4.OnPlayerInvitedToParty(playerId);
							}
						});
					}
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004924 File Offset: 0x00002B24
		private void MultiplayerPermissionWithPlayerChanged(PlayerId targetPlayerId, Permission permission, bool hasPermission)
		{
			if (!hasPermission && NetworkMain.GameClient.PlayersInParty.FirstOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.PlayerId == targetPlayerId) != null)
			{
				NetworkMain.GameClient.KickPlayerFromParty(NetworkMain.GameClient.PlayerID);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004974 File Offset: 0x00002B74
		public void OnGameClientStateChange(LobbyClient.State state)
		{
			if (!this.LobbyClient.IsInGame)
			{
				PlatformServices.MultiplayerGameStateChanged(false);
			}
			ILobbyStateHandler handler = this.Handler;
			if (handler != null)
			{
				handler.OnGameClientStateChange(state);
			}
			if (state == LobbyClient.State.SessionRequested)
			{
				MPPerkSelectionManager.Instance.InitializeForUser(this.LobbyClient.Name, this.LobbyClient.PlayerID);
			}
			else if (state == LobbyClient.State.Idle)
			{
				MPPerkSelectionManager.FreeInstance();
			}
			else if (!this.LobbyClient.AtLobby)
			{
				MPPerkSelectionManager.Instance.ResetPendingChanges();
			}
			PlatformServices.LobbyClientStateChanged(state == LobbyClient.State.AtLobby, !this.LobbyClient.IsInParty || this.LobbyClient.IsPartyLeader);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004A11 File Offset: 0x00002C11
		public void SetConnectionState(bool isAuthenticated)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler != null)
			{
				handler.SetConnectionState(isAuthenticated);
			}
			PlatformServices.ConnectionStateChanged(isAuthenticated);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004A2B File Offset: 0x00002C2B
		public void OnActivateHome()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnActivateHome();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004A3D File Offset: 0x00002C3D
		public void OnActivateCustomServer()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnActivateCustomServer();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004A4F File Offset: 0x00002C4F
		public void OnActivateMatchmaking()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnActivateMatchmaking();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004A61 File Offset: 0x00002C61
		public void OnActivateProfile()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnActivateProfile();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004A73 File Offset: 0x00002C73
		public void OnClanInvitationReceived(string clanName, string clanTag, bool isCreation)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationReceived(clanName, clanTag, isCreation);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004A88 File Offset: 0x00002C88
		public void OnClanInvitationAnswered(PlayerId playerId, ClanCreationAnswer answer)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationAnswered(playerId, answer);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004A9C File Offset: 0x00002C9C
		public void OnClanCreationSuccessful()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationSuccessful();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004AAE File Offset: 0x00002CAE
		public void OnClanCreationFailed()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationFailed();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004AC0 File Offset: 0x00002CC0
		public void OnClanCreationStarted()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationStarted();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004AD2 File Offset: 0x00002CD2
		public void OnClanInfoChanged()
		{
			ClanFriendListService clanFriendListService = this._clanFriendListService;
			if (clanFriendListService != null)
			{
				clanFriendListService.OnClanInfoChanged(this.LobbyClient.PlayerInfosInClan);
			}
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInfoChanged();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004B00 File Offset: 0x00002D00
		public void OnPremadeGameEligibilityStatusReceived(bool isEligible)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPremadeGameEligibilityStatusReceived(isEligible);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004B13 File Offset: 0x00002D13
		public void OnPremadeGameCreated()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPremadeGameCreated();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004B25 File Offset: 0x00002D25
		public void OnPremadeGameListReceived()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPremadeGameListReceived();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004B37 File Offset: 0x00002D37
		public void OnPremadeGameCreationCancelled()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPremadeGameCreationCancelled();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004B49 File Offset: 0x00002D49
		public void OnJoinPremadeGameRequested(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnJoinPremadeGameRequested(clanName, clanSigilCode, partyId, challengerPlayerIDs, challengerPartyLeaderID, premadeGameType);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004B64 File Offset: 0x00002D64
		public void OnJoinPremadeGameRequestSuccessful()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnJoinPremadeGameRequestSuccessful();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004B76 File Offset: 0x00002D76
		public void OnActivateArmory()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnActivateArmory();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004B88 File Offset: 0x00002D88
		public void OnActivateOptions()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnActivateOptions();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004B9A File Offset: 0x00002D9A
		public void OnDeactivateOptions()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnDeactivateOptions();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004BAC File Offset: 0x00002DAC
		public void OnCustomGameServerListReceived(AvailableCustomGames customGameServerList)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnCustomGameServerListReceived(customGameServerList);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004BBF File Offset: 0x00002DBF
		public void OnMatchmakerGameOver(int oldExp, int newExp, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo, BattleCancelReason battleCancelReason)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMatchmakerGameOver(oldExp, newExp, badgesEarned, lootGained, oldRankBarInfo, newRankBarInfo, battleCancelReason);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004BDC File Offset: 0x00002DDC
		public void OnBattleServerLost()
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleServerLost();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004BEE File Offset: 0x00002DEE
		public void OnRemovedFromMatchmakerGame(DisconnectType disconnectType)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRemovedFromMatchmakerGame(disconnectType);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004C01 File Offset: 0x00002E01
		public void OnRemovedFromCustomGame(DisconnectType disconnectType)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRemovedFromCustomGame(disconnectType);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004C14 File Offset: 0x00002E14
		public void OnPlayerAssignedPartyLeader(PlayerId partyLeaderId)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerAssignedPartyLeader(partyLeaderId);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004C27 File Offset: 0x00002E27
		public void OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSuggestedToParty(playerId, playerName, suggestingPlayerId, suggestingPlayerName);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004C3E File Offset: 0x00002E3E
		public void OnJoinCustomGameFailureResponse(CustomGameJoinResponse response)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnJoinCustomGameFailureResponse(response);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004C51 File Offset: 0x00002E51
		public void OnServerStatusReceived(ServerStatus serverStatus)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnServerStatusReceived(serverStatus);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004C64 File Offset: 0x00002E64
		public void OnFriendListReceived(FriendInfo[] friends)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler != null)
			{
				handler.OnFriendListUpdated();
			}
			BannerlordFriendListService bannerlordFriendListService = this._bannerlordFriendListService;
			if (bannerlordFriendListService == null)
			{
				return;
			}
			bannerlordFriendListService.OnFriendListReceived(friends);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004C88 File Offset: 0x00002E88
		public void OnRecentPlayerStatusesReceived(FriendInfo[] friends)
		{
			RecentPlayersFriendListService recentPlayersFriendListService = this._recentPlayersFriendListService;
			if (recentPlayersFriendListService == null)
			{
				return;
			}
			recentPlayersFriendListService.OnFriendListReceived(friends);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004C9B File Offset: 0x00002E9B
		public void OnBattleServerInformationReceived(BattleServerInformationForClient battleServerInformation)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleServerInformationReceived(battleServerInformation);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004CAE File Offset: 0x00002EAE
		public void OnRejoinBattleRequestAnswered(bool isSuccessful)
		{
			ILobbyStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRejoinBattleRequestAnswered(isSuccessful);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004CC1 File Offset: 0x00002EC1
		internal void OnSigilChanged()
		{
			if (this.Handler != null)
			{
				this.Handler.OnSigilChanged();
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004CD6 File Offset: 0x00002ED6
		public void OnNotificationsReceived(LobbyNotification[] notifications)
		{
			if (this.Handler != null)
			{
				this.Handler.OnNotificationsReceived(notifications);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004CEC File Offset: 0x00002EEC
		private void OnPlatformSignInStateUpdated(bool isSignedIn, TextObject message)
		{
			if (!isSignedIn && this.LobbyClient.Connected)
			{
				this.LobbyClient.Logout(message ?? new TextObject("{=oPOa77dI}Logged out of platform", null));
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004D1C File Offset: 0x00002F1C
		[Conditional("DEBUG")]
		private void PrintCompressionInfoKey()
		{
			try
			{
				List<Type> list = new List<Type>();
				Assembly[] array = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where assembly.GetName().Name.StartsWith("TaleWorlds.")
					select assembly).ToArray<Assembly>();
				Assembly[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Type type = array2[i].GetTypesSafe(null).FirstOrDefault<Type>((Type ty) => ty.Name.Contains("CompressionInfo"));
					if (type != null)
					{
						list.AddRange(type.GetNestedTypes());
						break;
					}
				}
				List<FieldInfo> list2 = new List<FieldInfo>();
				array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					foreach (Type type2 in array2[i].GetTypesSafe(null))
					{
						foreach (FieldInfo fieldInfo in type2.GetFields())
						{
							if (list.Contains(fieldInfo.FieldType))
							{
								list2.Add(fieldInfo);
							}
						}
					}
				}
				int num = 0;
				foreach (FieldInfo fieldInfo2 in list2)
				{
					object value = fieldInfo2.GetValue(null);
					MethodInfo method = fieldInfo2.FieldType.GetMethod("GetHashKey", BindingFlags.Instance | BindingFlags.NonPublic);
					num += (int)method.Invoke(value, new object[0]);
				}
				Debug.Print("CompressionInfoKey: " + num, 0, Debug.DebugColor.Cyan, 17179869184UL);
			}
			catch
			{
				Debug.Print("CompressionInfoKey checking failed.", 0, Debug.DebugColor.Cyan, 17179869184UL);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004F30 File Offset: 0x00003130
		public async Task<bool> OnInviteToPlatformSession(PlayerId playerId)
		{
			bool flag;
			if (!this.LobbyClient.Connected)
			{
				flag = false;
			}
			else
			{
				bool flag2 = false;
				if ((!this.LobbyClient.IsInParty || this.LobbyClient.IsPartyLeader) && this.LobbyClient.PlayersInParty.Count < Parameters.MaxPlayerCountInParty && PlatformServices.Instance.UsePlatformInvitationService(playerId))
				{
					flag2 = await PlatformServices.InvitationServices.OnInviteToPlatformSession(playerId);
				}
				if (!flag2)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=ljHPjjmX}Could not invite player to the game", null).ToString()));
				}
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004F80 File Offset: 0x00003180
		public async void OnPlatformRequestedMultiplayer()
		{
			PlatformServices.OnPlatformMultiplayerRequestHandled();
			await this.UpdateHasMultiplayerPrivilege();
			if (this.HasMultiplayerPrivilege != null && this.HasMultiplayerPrivilege.Value)
			{
				if (this.LobbyClient.IsIdle)
				{
					await this.TryLogin();
					int waitTime = 0;
					while (this.LobbyClient.CurrentState != LobbyClient.State.Idle && this.LobbyClient.CurrentState != LobbyClient.State.AtLobby && waitTime < 3000)
					{
						await Task.Delay(100);
						waitTime += 100;
					}
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004FBC File Offset: 0x000031BC
		public async void OnSessionInvitationAccepted(SessionInvitationType targetGameType)
		{
			if (targetGameType == SessionInvitationType.Multiplayer)
			{
				PlatformServices.OnSessionInvitationHandled();
				await this.UpdateHasMultiplayerPrivilege();
				if (this.HasMultiplayerPrivilege != null && this.HasMultiplayerPrivilege.Value)
				{
					await Task.Delay(2000);
					if (this.LobbyClient.IsIdle)
					{
						await this.TryLogin();
						int waitTime = 0;
						while (this.LobbyClient.CurrentState != LobbyClient.State.Idle && this.LobbyClient.CurrentState != LobbyClient.State.AtLobby && waitTime < 3000)
						{
							await Task.Delay(100);
							waitTime += 100;
						}
					}
					LobbyClient.State currentState = this.LobbyClient.CurrentState;
				}
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005000 File Offset: 0x00003200
		public List<CustomServerAction> GetCustomActionsForServer(GameServerEntry gameServerEntry)
		{
			List<CustomServerAction> list = new List<CustomServerAction>();
			for (int i = 0; i < this._onCustomServerActionRequestedForServerEntry.Count; i++)
			{
				List<CustomServerAction> list2 = this._onCustomServerActionRequestedForServerEntry[i](gameServerEntry);
				if (list2 != null && list2.Count > 0)
				{
					list.AddRange(list2);
				}
			}
			return list;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005050 File Offset: 0x00003250
		public void RegisterForCustomServerAction(Func<GameServerEntry, List<CustomServerAction>> action)
		{
			if (this._onCustomServerActionRequestedForServerEntry != null)
			{
				this._onCustomServerActionRequestedForServerEntry.Add(action);
				return;
			}
			Debug.FailedAssert("Lobby state is finalized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\LobbyState.cs", "RegisterForCustomServerAction", 1179);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005080 File Offset: 0x00003280
		public void UnregisterForCustomServerAction(Func<GameServerEntry, List<CustomServerAction>> action)
		{
			if (this._onCustomServerActionRequestedForServerEntry != null)
			{
				this._onCustomServerActionRequestedForServerEntry.Remove(action);
				return;
			}
			Debug.FailedAssert("Lobby state is finalized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\LobbyState.cs", "UnregisterForCustomServerAction", 1191);
		}

		// Token: 0x0400001D RID: 29
		private const string _newsSourceURLBase = "https://cdn.taleworlds.com/upload/bannerlordnews/NewsFeed_";

		// Token: 0x0400001E RID: 30
		private BannerlordFriendListService _bannerlordFriendListService;

		// Token: 0x0400001F RID: 31
		private RecentPlayersFriendListService _recentPlayersFriendListService;

		// Token: 0x04000020 RID: 32
		private ClanFriendListService _clanFriendListService;

		// Token: 0x04000022 RID: 34
		private readonly object _sessionInvitationDataLock = new object();

		// Token: 0x04000023 RID: 35
		private ILobbyStateHandler _handler;

		// Token: 0x04000024 RID: 36
		private LobbyGameClientHandler _lobbyGameClientManager;

		// Token: 0x04000025 RID: 37
		[TupleElementNames(new string[] { "PlayerId", "Permission" })]
		private ConcurrentDictionary<ValueTuple<PlayerId, Permission>, bool> _registeredPermissionEvents;

		// Token: 0x04000026 RID: 38
		private List<Func<GameServerEntry, List<CustomServerAction>>> _onCustomServerActionRequestedForServerEntry;

		// Token: 0x04000028 RID: 40
		public Action<bool> OnMultiplayerPrivilegeUpdated;

		// Token: 0x04000029 RID: 41
		public Action<bool> OnCrossplayPrivilegeUpdated;

		// Token: 0x0400002A RID: 42
		public Action<bool> OnUserGeneratedContentPrivilegeUpdated;
	}
}
