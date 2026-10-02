using System;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B9 RID: 697
	public class MultiplayerGameNotificationsComponent : MissionNetwork
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x00096B5F File Offset: 0x00094D5F
		public static int NotificationCount
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x00096B63 File Offset: 0x00094D63
		public void WarmupEnding()
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleWarmupEnding, 30, -1, null, null);
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x00096B74 File Offset: 0x00094D74
		public void GameOver(Team winnerTeam)
		{
			if (winnerTeam == null)
			{
				this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDraw, -1, -1, null, null);
				return;
			}
			Team team = ((winnerTeam.Side == BattleSideEnum.Attacker) ? base.Mission.Teams.Defender : base.Mission.Teams.Attacker);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverVictory, -1, -1, winnerTeam, null);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDefeat, -1, -1, team, null);
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x00096BD2 File Offset: 0x00094DD2
		public void PreparationStarted()
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattlePreparationStart, -1, -1, null, null);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x00096BE0 File Offset: 0x00094DE0
		public void FlagsXRemoved(FlagCapturePoint removedFlag)
		{
			int flagChar = removedFlag.FlagChar;
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemoved, flagChar, -1, null, null);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x00096C00 File Offset: 0x00094E00
		public void FlagXRemaining(FlagCapturePoint remainingFlag)
		{
			int flagChar = remainingFlag.FlagChar;
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemaining, flagChar, -1, null, null);
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x00096C1F File Offset: 0x00094E1F
		public void FlagsWillBeRemovedInXSeconds(int timeLeft)
		{
			this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagsWillBeRemoved, new int[] { timeLeft });
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00096C34 File Offset: 0x00094E34
		public void FlagXCapturedByTeamX(SynchedMissionObject flag, Team capturingTeam)
		{
			FlagCapturePoint flagCapturePoint = flag as FlagCapturePoint;
			int num = ((flagCapturePoint != null) ? flagCapturePoint.FlagChar : 65);
			Team team = ((capturingTeam.Side == BattleSideEnum.Attacker) ? base.Mission.Teams.Defender : base.Mission.Teams.Attacker);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByYourTeam, num, -1, capturingTeam, null);
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByOtherTeam, num, -1, team, null);
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00096C9C File Offset: 0x00094E9C
		public void GoldCarriedFromPreviousRound(int carriedGoldAmount, NetworkCommunicator syncToPeer)
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GoldCarriedFromPreviousRound, carriedGoldAmount, -1, null, syncToPeer);
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00096CB7 File Offset: 0x00094EB7
		public void PlayerIsInactive(NetworkCommunicator peer)
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsInactive, -1, -1, null, peer);
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x00096CC5 File Offset: 0x00094EC5
		public void FormationAutoFollowEnforced(NetworkCommunicator peer)
		{
			this.HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FormationAutoFollowEnforced, -1, -1, null, peer);
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x00096CD4 File Offset: 0x00094ED4
		public void PollRejected(MultiplayerPollRejectReason reason)
		{
			if (reason == MultiplayerPollRejectReason.TooManyPollRequests)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.TooManyPollRequests, Array.Empty<int>());
				return;
			}
			if (reason == MultiplayerPollRejectReason.HasOngoingPoll)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.HasOngoingPoll, Array.Empty<int>());
				return;
			}
			if (reason == MultiplayerPollRejectReason.NotEnoughPlayersToOpenPoll)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.NotEnoughPlayersToOpenPoll, new int[] { 3 });
				return;
			}
			if (reason == MultiplayerPollRejectReason.KickPollTargetNotSynced)
			{
				this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.KickPollTargetNotSynced, Array.Empty<int>());
				return;
			}
			Debug.FailedAssert("Notification of a PollRejectReason is missing (" + reason + ")", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameNotificationsComponent.cs", "PollRejected", 153);
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x00096D56 File Offset: 0x00094F56
		public void PlayerKicked(NetworkCommunicator kickedPeer)
		{
			this.ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsKicked, new int[] { kickedPeer.Index });
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x00096D6F File Offset: 0x00094F6F
		private void HandleNewNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum notification, int param1 = -1, int param2 = -1, Team syncToTeam = null, NetworkCommunicator syncToPeer = null)
		{
			if (syncToPeer != null)
			{
				this.SendNotificationToPeer(syncToPeer, notification, param1, param2);
				return;
			}
			if (syncToTeam != null)
			{
				this.SendNotificationToTeam(syncToTeam, notification, param1, param2);
				return;
			}
			this.SendNotificationToEveryone(notification, param1, param2);
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x00096D9C File Offset: 0x00094F9C
		private void ShowNotification(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum notification, params int[] parameters)
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				NotificationProperty notificationProperty = (NotificationProperty)notification.GetType().GetField(notification.ToString()).GetCustomAttributesSafe(typeof(NotificationProperty), false)
					.Single<object>();
				if (notificationProperty != null)
				{
					int[] array = parameters.Where<int>((int x) => x != -1).ToArray<int>();
					TextObject textObject = this.ToNotificationString(notification, notificationProperty, array);
					string text = this.ToSoundString(notification, notificationProperty, array);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, text);
				}
			}
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x00096E35 File Offset: 0x00095035
		private void SendNotificationToEveryone(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, int param1 = -1, int param2 = -1)
		{
			this.ShowNotification(message, new int[] { param1, param2 });
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new NotificationMessage((int)message, param1, param2));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x00096E65 File Offset: 0x00095065
		private void SendNotificationToPeer(NetworkCommunicator peer, MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, int param1 = -1, int param2 = -1)
		{
			if (peer.IsServerPeer)
			{
				this.ShowNotification(message, new int[] { param1, param2 });
				return;
			}
			GameNetwork.BeginModuleEventAsServer(peer);
			GameNetwork.WriteMessage(new NotificationMessage((int)message, param1, param2));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x00096EA0 File Offset: 0x000950A0
		private void SendNotificationToTeam(Team team, MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, int param1 = -1, int param2 = -1)
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (!GameNetwork.IsDedicatedServer && ((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.Team.IsEnemyOf(team))
			{
				this.ShowNotification(message, new int[] { param1, param2 });
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && !component.IsMine && !component.Team.IsEnemyOf(team))
				{
					GameNetwork.BeginModuleEventAsServer(component.Peer);
					GameNetwork.WriteMessage(new NotificationMessage((int)message, param1, param2));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x00096F80 File Offset: 0x00095180
		private string ToSoundString(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum value, NotificationProperty attribute, params int[] parameters)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(attribute.SoundIdTwo))
			{
				text = attribute.SoundIdOne;
			}
			else if (value != MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleYouHaveXTheRound)
			{
				if (value != MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByYourTeam)
				{
					if (value == MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByOtherTeam)
					{
						text = attribute.SoundIdTwo;
					}
				}
				else
				{
					text = attribute.SoundIdOne;
				}
			}
			else
			{
				Team team = ((parameters[0] == 0) ? Mission.Current.AttackerTeam : Mission.Current.DefenderTeam);
				Team team2 = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
				text = attribute.SoundIdOne;
				if (team2 != null && team2 != team)
				{
					text = attribute.SoundIdTwo;
				}
			}
			return text;
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x00097017 File Offset: 0x00095217
		private TextObject ToNotificationString(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum value, NotificationProperty attribute, params int[] parameters)
		{
			if (parameters.Length != 0)
			{
				this.SetGameTextVariables(value, parameters);
			}
			return GameTexts.FindText(attribute.StringId, null);
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x00097034 File Offset: 0x00095234
		private void SetGameTextVariables(MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum message, params int[] parameters)
		{
			if (parameters.Length == 0)
			{
				return;
			}
			switch (message)
			{
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleWarmupEnding:
				GameTexts.SetVariable("SECONDS_LEFT", parameters[0]);
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattlePreparationStart:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDraw:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverVictory:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GameOverDefeat:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsInactive:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.HasOngoingPoll:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.TooManyPollRequests:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.KickPollTargetNotSynced:
				break;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.BattleYouHaveXTheRound:
			{
				Team team = ((parameters[0] == 0) ? Mission.Current.AttackerTeam : Mission.Current.DefenderTeam);
				Team team2 = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
				if (team2 != null)
				{
					GameTexts.SetVariable("IS_WINNER", (team2 == team) ? 1 : 0);
					return;
				}
				break;
			}
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemoved:
				GameTexts.SetVariable("PARAM1", ((char)parameters[0]).ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXRemaining:
				GameTexts.SetVariable("PARAM1", ((char)parameters[0]).ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagsWillBeRemoved:
				GameTexts.SetVariable("PARAM1", parameters[0]);
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByYourTeam:
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.FlagXCapturedByOtherTeam:
				GameTexts.SetVariable("PARAM1", ((char)parameters[0]).ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.GoldCarriedFromPreviousRound:
				GameTexts.SetVariable("PARAM1", parameters[0].ToString());
				return;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.NotEnoughPlayersToOpenPoll:
				GameTexts.SetVariable("MIN_PARTICIPANT_COUNT", parameters[0]);
				break;
			case MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum.PlayerIsKicked:
				GameTexts.SetVariable("PLAYER_NAME", GameNetwork.FindNetworkPeer(parameters[0]).UserName);
				return;
			default:
				return;
			}
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x00097181 File Offset: 0x00095381
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<NotificationMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventServerMessage));
			}
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x0009719C File Offset: 0x0009539C
		private void HandleServerEventServerMessage(GameNetworkMessage baseMessage)
		{
			NotificationMessage notificationMessage = (NotificationMessage)baseMessage;
			this.ShowNotification((MultiplayerGameNotificationsComponent.MultiplayerNotificationEnum)notificationMessage.Message, new int[] { notificationMessage.ParameterOne, notificationMessage.ParameterTwo });
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000971D4 File Offset: 0x000953D4
		protected override void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
			bool isServerPeer = clientConnectionInfo.NetworkPeer.IsServerPeer;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000971E2 File Offset: 0x000953E2
		protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			bool isServer = GameNetwork.IsServer;
		}

		// Token: 0x0200059F RID: 1439
		private enum MultiplayerNotificationEnum
		{
			// Token: 0x04001EA0 RID: 7840
			[NotificationProperty("str_battle_warmup_ending_in_x_seconds", "event:/ui/mission/multiplayer/lastmanstanding", "")]
			BattleWarmupEnding,
			// Token: 0x04001EA1 RID: 7841
			[NotificationProperty("str_battle_preparation_start", "event:/ui/mission/multiplayer/roundstart", "")]
			BattlePreparationStart,
			// Token: 0x04001EA2 RID: 7842
			[NotificationProperty("str_round_result_win_lose", "event:/ui/mission/multiplayer/victory", "event:/ui/mission/multiplayer/defeat")]
			BattleYouHaveXTheRound,
			// Token: 0x04001EA3 RID: 7843
			[NotificationProperty("str_mp_mission_game_over_draw", "", "")]
			GameOverDraw,
			// Token: 0x04001EA4 RID: 7844
			[NotificationProperty("str_mp_mission_game_over_victory", "", "")]
			GameOverVictory,
			// Token: 0x04001EA5 RID: 7845
			[NotificationProperty("str_mp_mission_game_over_defeat", "", "")]
			GameOverDefeat,
			// Token: 0x04001EA6 RID: 7846
			[NotificationProperty("str_mp_flag_removed", "event:/ui/mission/multiplayer/pointsremoved", "")]
			FlagXRemoved,
			// Token: 0x04001EA7 RID: 7847
			[NotificationProperty("str_sergeant_a_one_flag_remaining", "event:/ui/mission/multiplayer/pointsremoved", "")]
			FlagXRemaining,
			// Token: 0x04001EA8 RID: 7848
			[NotificationProperty("str_sergeant_a_flags_will_be_removed", "event:/ui/mission/multiplayer/pointwarning", "")]
			FlagsWillBeRemoved,
			// Token: 0x04001EA9 RID: 7849
			[NotificationProperty("str_sergeant_a_flag_captured_by_your_team", "event:/ui/mission/multiplayer/pointcapture", "event:/ui/mission/multiplayer/pointlost")]
			FlagXCapturedByYourTeam,
			// Token: 0x04001EAA RID: 7850
			[NotificationProperty("str_sergeant_a_flag_captured_by_other_team", "event:/ui/mission/multiplayer/pointcapture", "event:/ui/mission/multiplayer/pointlost")]
			FlagXCapturedByOtherTeam,
			// Token: 0x04001EAB RID: 7851
			[NotificationProperty("str_gold_carried_from_previous_round", "", "")]
			GoldCarriedFromPreviousRound,
			// Token: 0x04001EAC RID: 7852
			[NotificationProperty("str_player_is_inactive", "", "")]
			PlayerIsInactive,
			// Token: 0x04001EAD RID: 7853
			[NotificationProperty("str_has_ongoing_poll", "", "")]
			HasOngoingPoll,
			// Token: 0x04001EAE RID: 7854
			[NotificationProperty("str_too_many_poll_requests", "", "")]
			TooManyPollRequests,
			// Token: 0x04001EAF RID: 7855
			[NotificationProperty("str_kick_poll_target_not_synced", "", "")]
			KickPollTargetNotSynced,
			// Token: 0x04001EB0 RID: 7856
			[NotificationProperty("str_not_enough_players_to_open_poll", "", "")]
			NotEnoughPlayersToOpenPoll,
			// Token: 0x04001EB1 RID: 7857
			[NotificationProperty("str_player_is_kicked", "", "")]
			PlayerIsKicked,
			// Token: 0x04001EB2 RID: 7858
			[NotificationProperty("str_formation_autofollow_enforced", "", "")]
			FormationAutoFollowEnforced,
			// Token: 0x04001EB3 RID: 7859
			Count
		}
	}
}
