using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000012 RID: 18
	public interface ILobbyStateHandler
	{
		// Token: 0x06000106 RID: 262
		void SetConnectionState(bool isAuthenticated);

		// Token: 0x06000107 RID: 263
		string ShowFeedback(string title, string feedbackText);

		// Token: 0x06000108 RID: 264
		string ShowFeedback(InquiryData inquiryData);

		// Token: 0x06000109 RID: 265
		void DismissFeedback(string id);

		// Token: 0x0600010A RID: 266
		void OnPause();

		// Token: 0x0600010B RID: 267
		void OnResume();

		// Token: 0x0600010C RID: 268
		void OnDisconnected();

		// Token: 0x0600010D RID: 269
		void OnRequestedToSearchBattle();

		// Token: 0x0600010E RID: 270
		void OnUpdateFindingGame(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo);

		// Token: 0x0600010F RID: 271
		void OnRequestedToCancelSearchBattle();

		// Token: 0x06000110 RID: 272
		void OnSearchBattleCanceled();

		// Token: 0x06000111 RID: 273
		void OnPlayerDataReceived(PlayerData playerData);

		// Token: 0x06000112 RID: 274
		void OnPendingRejoin();

		// Token: 0x06000113 RID: 275
		void OnEnterBattleWithParty(string[] selectedGameTypes);

		// Token: 0x06000114 RID: 276
		void OnPartyInvitationReceived(PlayerId playerId);

		// Token: 0x06000115 RID: 277
		void OnPartyJoinRequestReceived(PlayerId joingPlayerId, PlayerId viaPlayerId, string viaPlayerName, bool newParty);

		// Token: 0x06000116 RID: 278
		void OnPartyInvitationInvalidated();

		// Token: 0x06000117 RID: 279
		void OnPlayerInvitedToParty(PlayerId playerId);

		// Token: 0x06000118 RID: 280
		void OnPlayerAddedToParty(PlayerId playerId, string playerName, bool isPartyLeader);

		// Token: 0x06000119 RID: 281
		void OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason);

		// Token: 0x0600011A RID: 282
		void OnPlayerNameUpdated(string newName);

		// Token: 0x0600011B RID: 283
		void OnGameClientStateChange(LobbyClient.State state);

		// Token: 0x0600011C RID: 284
		void OnAdminMessageReceived(string message);

		// Token: 0x0600011D RID: 285
		void OnActivateHome();

		// Token: 0x0600011E RID: 286
		void OnActivateCustomServer();

		// Token: 0x0600011F RID: 287
		void OnActivateMatchmaking();

		// Token: 0x06000120 RID: 288
		void OnActivateArmory();

		// Token: 0x06000121 RID: 289
		void OnActivateOptions();

		// Token: 0x06000122 RID: 290
		void OnDeactivateOptions();

		// Token: 0x06000123 RID: 291
		void OnCustomGameServerListReceived(AvailableCustomGames customGameServerList);

		// Token: 0x06000124 RID: 292
		void OnMatchmakerGameOver(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo, BattleCancelReason battleCancelReason);

		// Token: 0x06000125 RID: 293
		void OnBattleServerLost();

		// Token: 0x06000126 RID: 294
		void OnRemovedFromMatchmakerGame(DisconnectType disconnectType);

		// Token: 0x06000127 RID: 295
		void OnRemovedFromCustomGame(DisconnectType disconnectType);

		// Token: 0x06000128 RID: 296
		void OnPlayerAssignedPartyLeader(PlayerId partyLeaderId);

		// Token: 0x06000129 RID: 297
		void OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName);

		// Token: 0x0600012A RID: 298
		void OnJoinCustomGameFailureResponse(CustomGameJoinResponse response);

		// Token: 0x0600012B RID: 299
		void OnRejoinBattleRequestAnswered(bool isSuccessful);

		// Token: 0x0600012C RID: 300
		void OnServerStatusReceived(ServerStatus serverStatus);

		// Token: 0x0600012D RID: 301
		void OnBattleServerInformationReceived(BattleServerInformationForClient battleServerInformation);

		// Token: 0x0600012E RID: 302
		void OnActivateProfile();

		// Token: 0x0600012F RID: 303
		void OnClanInvitationReceived(string clanName, string clanTag, bool isCreation);

		// Token: 0x06000130 RID: 304
		void OnClanInvitationAnswered(PlayerId playerId, ClanCreationAnswer answer);

		// Token: 0x06000131 RID: 305
		void OnClanCreationSuccessful();

		// Token: 0x06000132 RID: 306
		void OnClanCreationFailed();

		// Token: 0x06000133 RID: 307
		void OnClanCreationStarted();

		// Token: 0x06000134 RID: 308
		void OnClanInfoChanged();

		// Token: 0x06000135 RID: 309
		void OnPremadeGameEligibilityStatusReceived(bool isEligible);

		// Token: 0x06000136 RID: 310
		void OnPremadeGameCreated();

		// Token: 0x06000137 RID: 311
		void OnPremadeGameListReceived();

		// Token: 0x06000138 RID: 312
		void OnPremadeGameCreationCancelled();

		// Token: 0x06000139 RID: 313
		void OnJoinPremadeGameRequested(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType);

		// Token: 0x0600013A RID: 314
		void OnJoinPremadeGameRequestSuccessful();

		// Token: 0x0600013B RID: 315
		void OnSigilChanged();

		// Token: 0x0600013C RID: 316
		void OnNotificationsReceived(LobbyNotification[] notifications);

		// Token: 0x0600013D RID: 317
		void OnFriendListUpdated();
	}
}
