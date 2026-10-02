using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000009 RID: 9
	public class LobbyGameClientHandler : ILobbyClientSessionHandler
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002DAC File Offset: 0x00000FAC
		void ILobbyClientSessionHandler.OnConnected()
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002DAE File Offset: 0x00000FAE
		void ILobbyClientSessionHandler.OnCantConnect()
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DB0 File Offset: 0x00000FB0
		void ILobbyClientSessionHandler.OnDisconnected(TextObject feedback)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnDisconnected(feedback);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002DC6 File Offset: 0x00000FC6
		void ILobbyClientSessionHandler.OnPlayerDataReceived(PlayerData playerData)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPlayerDataReceived(playerData);
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002DDC File Offset: 0x00000FDC
		void ILobbyClientSessionHandler.OnPendingRejoin()
		{
			LobbyState lobbyState = this.LobbyState;
			if (lobbyState == null)
			{
				return;
			}
			lobbyState.OnPendingRejoin();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002DEE File Offset: 0x00000FEE
		void ILobbyClientSessionHandler.OnBattleResultReceived()
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002DF0 File Offset: 0x00000FF0
		void ILobbyClientSessionHandler.OnCancelJoiningBattle()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnCancelFindingGame();
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002E05 File Offset: 0x00001005
		void ILobbyClientSessionHandler.OnRejoinRequestRejected()
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002E07 File Offset: 0x00001007
		void ILobbyClientSessionHandler.OnFindGameAnswer(bool successful, string[] selectedAndEnabledGameTypes, bool isRejoin)
		{
			if (successful && this.LobbyState != null)
			{
				this.LobbyState.OnUpdateFindingGame(MatchmakingWaitTimeStats.Empty, selectedAndEnabledGameTypes);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002E25 File Offset: 0x00001025
		void ILobbyClientSessionHandler.OnEnterBattleWithPartyAnswer(string[] selectedGameTypes)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnUpdateFindingGame(MatchmakingWaitTimeStats.Empty, selectedGameTypes);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E40 File Offset: 0x00001040
		void ILobbyClientSessionHandler.OnWhisperMessageReceived(string fromPlayer, string toPlayer, string message)
		{
			if (this.ChatHandler != null)
			{
				this.ChatHandler.ReceiveChatMessage(ChatChannelType.Private, fromPlayer, message);
			}
			ChatBox.AddWhisperMessage(fromPlayer, message);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002E5F File Offset: 0x0000105F
		void ILobbyClientSessionHandler.OnClanMessageReceived(string playerName, string message)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E61 File Offset: 0x00001061
		void ILobbyClientSessionHandler.OnPartyMessageReceived(string playerName, string message)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002E63 File Offset: 0x00001063
		void ILobbyClientSessionHandler.OnSystemMessageReceived(string message)
		{
			InformationManager.DisplayMessage(new InformationMessage(message));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002E70 File Offset: 0x00001070
		void ILobbyClientSessionHandler.OnAdminMessageReceived(string message)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnAdminMessageReceived(message);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002E86 File Offset: 0x00001086
		void ILobbyClientSessionHandler.OnPartyInvitationReceived(string inviterPlayerName, PlayerId inviterPlayerId)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPartyInvitationReceived(inviterPlayerName, inviterPlayerId);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E9D File Offset: 0x0000109D
		void ILobbyClientSessionHandler.OnPartyJoinRequestReceived(PlayerId joiningPlayerId, PlayerId viaPlayerId, string viaFriendName)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPartyJoinRequestReceived(joiningPlayerId, viaPlayerId, viaFriendName);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002EB5 File Offset: 0x000010B5
		void ILobbyClientSessionHandler.OnPartyInvitationInvalidated()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPartyInvitationInvalidated();
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002ECA File Offset: 0x000010CA
		void ILobbyClientSessionHandler.OnPlayerInvitedToParty(PlayerId playerId)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPlayerInvitedToParty(playerId);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002EE0 File Offset: 0x000010E0
		void ILobbyClientSessionHandler.OnPlayersAddedToParty([TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })] List<ValueTuple<PlayerId, string, bool>> addedPlayers, [TupleElementNames(new string[] { "PlayerId", "PlayerName" })] List<ValueTuple<PlayerId, string>> invitedPlayers)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPlayersAddedToParty(addedPlayers, invitedPlayers);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002EF7 File Offset: 0x000010F7
		void ILobbyClientSessionHandler.OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPlayerRemovedFromParty(playerId, reason);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F0E File Offset: 0x0000110E
		void ILobbyClientSessionHandler.OnPlayerAssignedPartyLeader(PlayerId partyLeaderId)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPlayerAssignedPartyLeader(partyLeaderId);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002F24 File Offset: 0x00001124
		void ILobbyClientSessionHandler.OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPlayerSuggestedToParty(playerId, playerName, suggestingPlayerId, suggestingPlayerName);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002F3E File Offset: 0x0000113E
		void ILobbyClientSessionHandler.OnServerStatusReceived(ServerStatus serverStatus)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnServerStatusReceived(serverStatus);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002F54 File Offset: 0x00001154
		void ILobbyClientSessionHandler.OnFriendListReceived(FriendInfo[] friends)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnFriendListReceived(friends);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002F6A File Offset: 0x0000116A
		void ILobbyClientSessionHandler.OnRecentPlayerStatusesReceived(FriendInfo[] friends)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnRecentPlayerStatusesReceived(friends);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F80 File Offset: 0x00001180
		void ILobbyClientSessionHandler.OnClanInvitationReceived(string clanName, string clanTag, bool isCreation)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnClanInvitationReceived(clanName, clanTag, isCreation);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002F98 File Offset: 0x00001198
		void ILobbyClientSessionHandler.OnClanInvitationAnswered(PlayerId playerId, ClanCreationAnswer answer)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnClanInvitationAnswered(playerId, answer);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002FAF File Offset: 0x000011AF
		void ILobbyClientSessionHandler.OnClanCreationSuccessful()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnClanCreationSuccessful();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002FC4 File Offset: 0x000011C4
		void ILobbyClientSessionHandler.OnClanCreationFailed()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnClanCreationFailed();
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002FD9 File Offset: 0x000011D9
		void ILobbyClientSessionHandler.OnClanCreationStarted()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnClanCreationStarted();
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002FEE File Offset: 0x000011EE
		void ILobbyClientSessionHandler.OnClanInfoChanged()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnClanInfoChanged();
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003003 File Offset: 0x00001203
		void ILobbyClientSessionHandler.OnPremadeGameEligibilityStatusReceived(bool isEligible)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPremadeGameEligibilityStatusReceived(isEligible);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003019 File Offset: 0x00001219
		void ILobbyClientSessionHandler.OnPremadeGameCreated()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPremadeGameCreated();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000302E File Offset: 0x0000122E
		void ILobbyClientSessionHandler.OnPremadeGameListReceived()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPremadeGameListReceived();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003043 File Offset: 0x00001243
		void ILobbyClientSessionHandler.OnPremadeGameCreationCancelled()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnPremadeGameCreationCancelled();
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003058 File Offset: 0x00001258
		void ILobbyClientSessionHandler.OnJoinPremadeGameRequested(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnJoinPremadeGameRequested(clanName, clanSigilCode, partyId, challengerPlayerIDs, challengerPartyLeaderID, premadeGameType);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003076 File Offset: 0x00001276
		void ILobbyClientSessionHandler.OnJoinPremadeGameRequestSuccessful()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnJoinPremadeGameRequestSuccessful();
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000308B File Offset: 0x0000128B
		void ILobbyClientSessionHandler.OnSigilChanged()
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnSigilChanged();
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000030A0 File Offset: 0x000012A0
		void ILobbyClientSessionHandler.OnNotificationsReceived(LobbyNotification[] notifications)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnNotificationsReceived(notifications);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000030B6 File Offset: 0x000012B6
		void ILobbyClientSessionHandler.OnGameClientStateChange(LobbyClient.State oldState)
		{
			this.HandleGameClientStateChange(oldState);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000030C0 File Offset: 0x000012C0
		private async void HandleGameClientStateChange(LobbyClient.State oldState)
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			Debug.Print(string.Concat(new object[] { "[][] New MBGameClient State: ", gameClient.CurrentState, " old state:", oldState }), 0, Debug.DebugColor.White, 17592186044416UL);
			switch (gameClient.CurrentState)
			{
			case LobbyClient.State.Idle:
				if (oldState == LobbyClient.State.AtBattle || oldState == LobbyClient.State.HostingCustomGame || oldState == LobbyClient.State.InCustomGame)
				{
					if (Mission.Current != null && !(Game.Current.GameStateManager.ActiveState is MissionState))
					{
						Game.Current.GameStateManager.PopState(0);
					}
					if (Game.Current.GameStateManager.ActiveState is LobbyGameStateCustomGameClient)
					{
						Game.Current.GameStateManager.PopState(0);
					}
					if (Game.Current.GameStateManager.ActiveState is MissionState)
					{
						MissionState missionSystem = (MissionState)Game.Current.GameStateManager.ActiveState;
						while (missionSystem.CurrentMission.CurrentState == Mission.State.NewlyCreated || missionSystem.CurrentMission.CurrentState == Mission.State.Initializing)
						{
							await Task.Delay(1);
						}
						for (int i = 0; i < 3; i++)
						{
							await Task.Delay(1);
						}
						BannerlordNetwork.EndMultiplayerLobbyMission();
						missionSystem = null;
					}
					while (Mission.Current != null)
					{
						await Task.Delay(1);
					}
					this.LobbyState.SetConnectionState(false);
				}
				else if (oldState == LobbyClient.State.AtLobby || oldState == LobbyClient.State.SearchingBattle)
				{
					this.LobbyState.SetConnectionState(false);
				}
				else if (oldState == LobbyClient.State.WaitingToJoinCustomGame)
				{
					this.LobbyState.SetConnectionState(false);
				}
				else if (oldState == LobbyClient.State.Working)
				{
					this.LobbyState.SetConnectionState(false);
				}
				else if (oldState == LobbyClient.State.SessionRequested)
				{
					this.LobbyState.SetConnectionState(false);
				}
				else if (oldState == LobbyClient.State.Connected)
				{
					this.LobbyState.SetConnectionState(false);
				}
				else
				{
					Debug.FailedAssert("Unexpected old state:" + oldState, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\LobbyGameClientHandler.cs", "HandleGameClientStateChange", 414);
				}
				break;
			case LobbyClient.State.AtLobby:
				this.LobbyState.SetConnectionState(true);
				break;
			case LobbyClient.State.RequestingToSearchBattle:
				this.LobbyState.OnRequestedToSearchBattle();
				break;
			case LobbyClient.State.RequestingToCancelSearchBattle:
				this.LobbyState.OnRequestedToCancelSearchBattle();
				break;
			}
			this.LobbyState.OnGameClientStateChange(gameClient.CurrentState);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003101 File Offset: 0x00001301
		void ILobbyClientSessionHandler.OnCustomGameServerListReceived(AvailableCustomGames customGameServerList)
		{
			this.LobbyState.OnCustomGameServerListReceived(customGameServerList);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003110 File Offset: 0x00001310
		void ILobbyClientSessionHandler.OnMatchmakerGameOver(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo, BattleCancelReason battleCancelReason)
		{
			GameStateManager gameStateManager = Game.Current.GameStateManager;
			if (!(gameStateManager.ActiveState is LobbyState))
			{
				if (gameStateManager.ActiveState is MissionState)
				{
					BannerlordNetwork.EndMultiplayerLobbyMission();
				}
				else
				{
					gameStateManager.PopState(0);
				}
			}
			this.LobbyState.OnMatchmakerGameOver(oldExperience, newExperience, badgesEarned, lootGained, oldRankBarInfo, newRankBarInfo, battleCancelReason);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003168 File Offset: 0x00001368
		void ILobbyClientSessionHandler.OnQuitFromMatchmakerGame()
		{
			GameStateManager gameStateManager = Game.Current.GameStateManager;
			if (!(gameStateManager.ActiveState is LobbyState))
			{
				if (gameStateManager.ActiveState is MissionState)
				{
					BannerlordNetwork.EndMultiplayerLobbyMission();
					return;
				}
				gameStateManager.PopState(0);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031A7 File Offset: 0x000013A7
		void ILobbyClientSessionHandler.OnBattleServerInformationReceived(BattleServerInformationForClient battleServerInformation)
		{
			this.HandleBattleJoining(battleServerInformation);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000031B0 File Offset: 0x000013B0
		private async void HandleBattleJoining(BattleServerInformationForClient battleServerInformation)
		{
			if (this.LobbyState != null)
			{
				this.LobbyState.OnBattleServerInformationReceived(battleServerInformation);
			}
			while (GameStateManager.Current.LastOrDefault<LobbyPracticeState>() != null)
			{
				await Task.Delay(5);
			}
			LobbyGameStateMatchmakerClient lobbyGameStateMatchmakerClient = Game.Current.GameStateManager.CreateState<LobbyGameStateMatchmakerClient>();
			lobbyGameStateMatchmakerClient.SetStartingParameters(this, battleServerInformation.PeerIndex, battleServerInformation.SessionKey, battleServerInformation.ServerAddress, (int)battleServerInformation.ServerPort, battleServerInformation.GameType, battleServerInformation.SceneName);
			Game.Current.GameStateManager.PushState(lobbyGameStateMatchmakerClient, 0);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000031F4 File Offset: 0x000013F4
		void ILobbyClientSessionHandler.OnBattleServerLost()
		{
			GameStateManager gameStateManager = Game.Current.GameStateManager;
			if (!(gameStateManager.ActiveState is LobbyState))
			{
				if (gameStateManager.ActiveState is MissionState)
				{
					BannerlordNetwork.EndMultiplayerLobbyMission();
				}
				else
				{
					gameStateManager.PopState(0);
				}
			}
			this.LobbyState.OnBattleServerLost();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003240 File Offset: 0x00001440
		void ILobbyClientSessionHandler.OnRemovedFromMatchmakerGame(DisconnectType disconnectType)
		{
			GameStateManager gameStateManager = Game.Current.GameStateManager;
			if (!(gameStateManager.ActiveState is LobbyState))
			{
				if (gameStateManager.ActiveState is MissionState)
				{
					BannerlordNetwork.EndMultiplayerLobbyMission();
				}
				else
				{
					gameStateManager.PopState(0);
				}
			}
			this.LobbyState.OnRemovedFromMatchmakerGame(disconnectType);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000328C File Offset: 0x0000148C
		void ILobbyClientSessionHandler.OnRejoinBattleRequestAnswered(bool isSuccessful)
		{
			this.LobbyState.OnRejoinBattleRequestAnswered(isSuccessful);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000329C File Offset: 0x0000149C
		void ILobbyClientSessionHandler.OnRegisterCustomGameServerResponse()
		{
			if (!GameNetwork.IsSessionActive)
			{
				LobbyGameStatePlayerBasedCustomServer lobbyGameStatePlayerBasedCustomServer = Game.Current.GameStateManager.CreateState<LobbyGameStatePlayerBasedCustomServer>();
				lobbyGameStatePlayerBasedCustomServer.SetStartingParameters(this);
				Game.Current.GameStateManager.PushState(lobbyGameStatePlayerBasedCustomServer, 0);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000032D8 File Offset: 0x000014D8
		void ILobbyClientSessionHandler.OnCustomGameEnd()
		{
			if (Game.Current != null)
			{
				GameStateManager gameStateManager = Game.Current.GameStateManager;
				if (!(gameStateManager.ActiveState is LobbyState))
				{
					if (Game.Current.GameStateManager.ActiveState is MissionState)
					{
						BannerlordNetwork.EndMultiplayerLobbyMission();
						return;
					}
					gameStateManager.PopState(0);
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003328 File Offset: 0x00001528
		PlayerJoinGameResponseDataFromHost[] ILobbyClientSessionHandler.OnClientWantsToConnectCustomGame(PlayerJoinGameData[] playerJoinData)
		{
			Debug.Print("Game join request with party received", 0, Debug.DebugColor.Green, 17592186044416UL);
			CustomGameJoinResponse customGameJoinResponse = CustomGameJoinResponse.UnspecifiedError;
			List<PlayerJoinGameResponseDataFromHost> list = new List<PlayerJoinGameResponseDataFromHost>();
			if (Mission.Current != null && Mission.Current.CurrentState == Mission.State.Continuing)
			{
				for (int i = 0; i < playerJoinData.Length; i++)
				{
					if (CustomGameBannedPlayerManager.IsUserBanned(playerJoinData[i].PlayerId))
					{
						customGameJoinResponse = CustomGameJoinResponse.PlayerBanned;
					}
				}
				if (customGameJoinResponse != CustomGameJoinResponse.PlayerBanned)
				{
					bool flag = MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) < GameNetwork.NetworkPeerCount + playerJoinData.Length;
					if (!flag)
					{
						List<PlayerConnectionInfo> list2 = new List<PlayerConnectionInfo>();
						foreach (PlayerJoinGameData playerJoinGameData in playerJoinData)
						{
							PlayerConnectionInfo playerConnectionInfo = new PlayerConnectionInfo(playerJoinGameData.PlayerId);
							Dictionary<int, List<int>> usedIndicesFromIds = CosmeticsManagerHelper.GetUsedIndicesFromIds(playerJoinGameData.UsedCosmetics);
							playerConnectionInfo.AddParameter("PlayerData", playerJoinGameData.PlayerData);
							playerConnectionInfo.AddParameter("UsedCosmetics", usedIndicesFromIds);
							playerConnectionInfo.AddParameter("IsAdmin", playerJoinGameData.IsAdmin);
							playerConnectionInfo.AddParameter("IpAddress", playerJoinGameData.IpAddress);
							playerConnectionInfo.Name = playerJoinGameData.Name;
							list2.Add(playerConnectionInfo);
						}
						GameNetwork.AddPlayersResult addPlayersResult = GameNetwork.HandleNewClientsConnect(list2.ToArray(), false);
						if (addPlayersResult.Success)
						{
							for (int j = 0; j < playerJoinData.Length; j++)
							{
								PlayerJoinGameData playerJoinGameData2 = playerJoinData[j];
								NetworkCommunicator networkCommunicator = addPlayersResult.NetworkPeers[j];
								PlayerJoinGameResponseDataFromHost playerJoinGameResponseDataFromHost = new PlayerJoinGameResponseDataFromHost
								{
									PlayerId = playerJoinGameData2.PlayerId,
									PeerIndex = networkCommunicator.Index,
									SessionKey = networkCommunicator.SessionKey,
									CustomGameJoinResponse = CustomGameJoinResponse.Success,
									IsAdmin = networkCommunicator.IsAdmin
								};
								list.Add(playerJoinGameResponseDataFromHost);
							}
							customGameJoinResponse = CustomGameJoinResponse.Success;
						}
						else
						{
							customGameJoinResponse = CustomGameJoinResponse.ErrorOnGameServer;
						}
					}
					else if (flag)
					{
						customGameJoinResponse = CustomGameJoinResponse.ServerCapacityIsFull;
					}
					else
					{
						customGameJoinResponse = CustomGameJoinResponse.UnspecifiedError;
					}
				}
			}
			else
			{
				customGameJoinResponse = CustomGameJoinResponse.CustomGameServerNotAvailable;
			}
			if (customGameJoinResponse != CustomGameJoinResponse.Success)
			{
				foreach (PlayerJoinGameData playerJoinGameData3 in playerJoinData)
				{
					PlayerJoinGameResponseDataFromHost playerJoinGameResponseDataFromHost2 = new PlayerJoinGameResponseDataFromHost
					{
						PlayerId = playerJoinGameData3.PlayerId,
						PeerIndex = -1,
						SessionKey = -1,
						CustomGameJoinResponse = customGameJoinResponse
					};
					list.Add(playerJoinGameResponseDataFromHost2);
				}
			}
			Debug.Print("Responding game join request with " + customGameJoinResponse, 0, Debug.DebugColor.White, 17592186044416UL);
			return list.ToArray();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003560 File Offset: 0x00001760
		void ILobbyClientSessionHandler.OnJoinCustomGameResponse(bool success, JoinGameData joinGameData, CustomGameJoinResponse failureReason, bool isAdmin)
		{
			LobbyGameClientHandler.<>c__DisplayClass54_0 CS$<>8__locals1 = new LobbyGameClientHandler.<>c__DisplayClass54_0();
			CS$<>8__locals1.isAdmin = isAdmin;
			if (success)
			{
				Module.CurrentModule.GetMultiplayerGameMode(joinGameData.GameServerProperties.GameType).JoinCustomGame(joinGameData);
				Task.Run(delegate
				{
					LobbyGameClientHandler.<>c__DisplayClass54_0.<<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d <<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d;
					<<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d.<>4__this = CS$<>8__locals1;
					<<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d.<>1__state = -1;
					AsyncTaskMethodBuilder <>t__builder = <<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d.<>t__builder;
					<>t__builder.Start<LobbyGameClientHandler.<>c__DisplayClass54_0.<<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d>(ref <<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d);
					return <<TaleWorlds-MountAndBlade-Diamond-ILobbyClientSessionHandler-OnJoinCustomGameResponse>b__0>d.<>t__builder.Task;
				});
				Debug.Print("Join game successful", 0, Debug.DebugColor.Green, 17592186044416UL);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000035C0 File Offset: 0x000017C0
		void ILobbyClientSessionHandler.OnJoinCustomGameFailureResponse(CustomGameJoinResponse response)
		{
			this.LobbyState.OnJoinCustomGameFailureResponse(response);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000035D0 File Offset: 0x000017D0
		void ILobbyClientSessionHandler.OnQuitFromCustomGame()
		{
			GameStateManager gameStateManager = Game.Current.GameStateManager;
			if (!(gameStateManager.ActiveState is LobbyState))
			{
				if (gameStateManager.ActiveState is MissionState)
				{
					BannerlordNetwork.EndMultiplayerLobbyMission();
					return;
				}
				gameStateManager.PopState(0);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003610 File Offset: 0x00001810
		void ILobbyClientSessionHandler.OnRemovedFromCustomGame(DisconnectType disconnectType)
		{
			GameStateManager gameStateManager = Game.Current.GameStateManager;
			if (!(gameStateManager.ActiveState is LobbyState))
			{
				if (gameStateManager.ActiveState is MissionState)
				{
					BannerlordNetwork.EndMultiplayerLobbyMission();
				}
				else
				{
					gameStateManager.PopState(0);
				}
			}
			this.LobbyState.OnRemovedFromCustomGame(disconnectType);
			if (this.LobbyState.LobbyClient.IsInParty)
			{
				switch (disconnectType)
				{
				case DisconnectType.QuitFromGame:
				case DisconnectType.TimedOut:
				case DisconnectType.KickedByHost:
				case DisconnectType.KickedByPoll:
				case DisconnectType.BannedByPoll:
				case DisconnectType.Inactivity:
				case DisconnectType.DisconnectedFromLobby:
				case DisconnectType.KickedDueToFriendlyDamage:
				case DisconnectType.PlayStateMismatch:
					this.LobbyState.LobbyClient.KickPlayerFromParty(this.LobbyState.LobbyClient.PlayerID);
					break;
				case DisconnectType.GameEnded:
				case DisconnectType.ServerNotResponding:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000036C1 File Offset: 0x000018C1
		void ILobbyClientSessionHandler.OnEnterCustomBattleWithPartyAnswer()
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000036C4 File Offset: 0x000018C4
		void ILobbyClientSessionHandler.OnClientQuitFromCustomGame(PlayerId playerId)
		{
			if (Mission.Current != null && Mission.Current.CurrentState == Mission.State.Continuing)
			{
				NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.FirstOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.VirtualPlayer.Id == playerId);
				if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
				{
					if (networkCommunicator.GetComponent<MissionPeer>() != null)
					{
						networkCommunicator.QuitFromMission = true;
					}
					GameNetwork.AddNetworkPeerToDisconnectAsServer(networkCommunicator);
					MBDebug.Print("player with id " + playerId + " quit from game", 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003754 File Offset: 0x00001954
		void ILobbyClientSessionHandler.OnAnnouncementReceived(Announcement announcement)
		{
			if (Mission.Current != null && Mission.Current.CurrentState == Mission.State.Continuing)
			{
				if (announcement.Type == AnnouncementType.Chat)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject(announcement.Text, null).ToString(), Color.FromUint(4292235858U)));
					return;
				}
				if (announcement.Type == AnnouncementType.Alert)
				{
					InformationManager.AddSystemNotification(new TextObject(announcement.Text, null).ToString());
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000037C4 File Offset: 0x000019C4
		async Task<bool> ILobbyClientSessionHandler.OnInviteToPlatformSession(PlayerId playerId)
		{
			return await this.LobbyState.OnInviteToPlatformSession(playerId);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003811 File Offset: 0x00001A11
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003819 File Offset: 0x00001A19
		public LobbyState LobbyState { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003822 File Offset: 0x00001A22
		public LobbyClient GameClient
		{
			get
			{
				return NetworkMain.GameClient;
			}
		}

		// Token: 0x04000006 RID: 6
		public IChatHandler ChatHandler;
	}
}
