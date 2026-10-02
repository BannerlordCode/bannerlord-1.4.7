using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BC RID: 700
	public class MultiplayerPollComponent : MissionNetwork
	{
		// Token: 0x06002803 RID: 10243 RVA: 0x0009792E File Offset: 0x00095B2E
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._notificationsComponent = base.Mission.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x00097958 File Offset: 0x00095B58
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			MultiplayerPollComponent.MultiplayerPoll ongoingPoll = this._ongoingPoll;
			if (ongoingPoll == null)
			{
				return;
			}
			ongoingPoll.Tick();
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x00097974 File Offset: 0x00095B74
		public void Vote(bool accepted)
		{
			if (GameNetwork.IsServer)
			{
				if (GameNetwork.MyPeer != null)
				{
					this.ApplyVote(GameNetwork.MyPeer, accepted);
					return;
				}
			}
			else if (this._ongoingPoll != null && this._ongoingPoll.IsOpen)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new PollResponse(accepted));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000979C8 File Offset: 0x00095BC8
		private void ApplyVote(NetworkCommunicator peer, bool accepted)
		{
			if (this._ongoingPoll != null && this._ongoingPoll.ApplyVote(peer, accepted))
			{
				List<NetworkCommunicator> pollProgressReceivers = this._ongoingPoll.GetPollProgressReceivers();
				int count = pollProgressReceivers.Count;
				for (int i = 0; i < count; i++)
				{
					GameNetwork.BeginModuleEventAsServer(pollProgressReceivers[i]);
					GameNetwork.WriteMessage(new PollProgress(this._ongoingPoll.AcceptedCount, this._ongoingPoll.RejectedCount));
					GameNetwork.EndModuleEventAsServer();
				}
				this.UpdatePollProgress(this._ongoingPoll.AcceptedCount, this._ongoingPoll.RejectedCount);
			}
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x00097A58 File Offset: 0x00095C58
		private void RejectPollOnServer(NetworkCommunicator pollCreatorPeer, MultiplayerPollRejectReason rejectReason)
		{
			if (pollCreatorPeer.IsMine)
			{
				this.RejectPoll(rejectReason);
				return;
			}
			GameNetwork.BeginModuleEventAsServer(pollCreatorPeer);
			GameNetwork.WriteMessage(new PollRequestRejected((int)rejectReason));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x00097A80 File Offset: 0x00095C80
		private void RejectPoll(MultiplayerPollRejectReason rejectReason)
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				this._notificationsComponent.PollRejected(rejectReason);
			}
			Action<MultiplayerPollRejectReason> onPollRejected = this.OnPollRejected;
			if (onPollRejected == null)
			{
				return;
			}
			onPollRejected(rejectReason);
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x00097AA6 File Offset: 0x00095CA6
		private void UpdatePollProgress(int votesAccepted, int votesRejected)
		{
			Action<int, int> onPollUpdated = this.OnPollUpdated;
			if (onPollUpdated == null)
			{
				return;
			}
			onPollUpdated(votesAccepted, votesRejected);
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x00097ABA File Offset: 0x00095CBA
		private void CancelPoll()
		{
			if (this._ongoingPoll != null)
			{
				this._ongoingPoll.Cancel();
				this._ongoingPoll = null;
			}
			Action onPollCancelled = this.OnPollCancelled;
			if (onPollCancelled == null)
			{
				return;
			}
			onPollCancelled();
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x00097AE8 File Offset: 0x00095CE8
		private void OnPollCancelledOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			List<NetworkCommunicator> pollProgressReceivers = multiplayerPoll.GetPollProgressReceivers();
			int count = pollProgressReceivers.Count;
			for (int i = 0; i < count; i++)
			{
				GameNetwork.BeginModuleEventAsServer(pollProgressReceivers[i]);
				GameNetwork.WriteMessage(new PollCancelled());
				GameNetwork.EndModuleEventAsServer();
			}
			this.CancelPoll();
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x00097B30 File Offset: 0x00095D30
		public void RequestKickPlayerPoll(NetworkCommunicator peer, bool banPlayer)
		{
			if (GameNetwork.IsServer)
			{
				if (GameNetwork.MyPeer != null)
				{
					this.OpenKickPlayerPollOnServer(GameNetwork.MyPeer, peer, banPlayer);
					return;
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new KickPlayerPollRequested(peer, banPlayer));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x00097B64 File Offset: 0x00095D64
		private void OpenKickPlayerPollOnServer(NetworkCommunicator pollCreatorPeer, NetworkCommunicator targetPeer, bool banPlayer)
		{
			if (this._ongoingPoll == null)
			{
				bool flag = pollCreatorPeer != null && pollCreatorPeer.IsConnectionActive;
				bool flag2 = targetPeer != null && targetPeer.IsConnectionActive;
				if (flag && flag2)
				{
					if (!targetPeer.IsSynchronized)
					{
						this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.KickPollTargetNotSynced);
						return;
					}
					MissionPeer component = pollCreatorPeer.GetComponent<MissionPeer>();
					if (component != null)
					{
						if (component.RequestedKickPollCount >= 2)
						{
							this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.TooManyPollRequests);
							return;
						}
						List<NetworkCommunicator> list = new List<NetworkCommunicator>();
						foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
						{
							if (networkCommunicator != targetPeer && networkCommunicator.IsSynchronized)
							{
								MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
								if (component2 != null && component2.Team == component.Team)
								{
									list.Add(networkCommunicator);
								}
							}
						}
						int count = list.Count;
						if (count + 1 >= 3)
						{
							this.OpenKickPlayerPoll(targetPeer, pollCreatorPeer, false, list);
							for (int i = 0; i < count; i++)
							{
								GameNetwork.BeginModuleEventAsServer(this._ongoingPoll.ParticipantsToVote[i]);
								GameNetwork.WriteMessage(new KickPlayerPollOpened(pollCreatorPeer, targetPeer, banPlayer));
								GameNetwork.EndModuleEventAsServer();
							}
							GameNetwork.BeginModuleEventAsServer(targetPeer);
							GameNetwork.WriteMessage(new KickPlayerPollOpened(pollCreatorPeer, targetPeer, banPlayer));
							GameNetwork.EndModuleEventAsServer();
							component.IncrementRequestedKickPollCount();
							return;
						}
						this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.NotEnoughPlayersToOpenPoll);
						return;
					}
				}
			}
			else
			{
				this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.HasOngoingPoll);
			}
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x00097CCC File Offset: 0x00095ECC
		private void OpenKickPlayerPoll(NetworkCommunicator targetPeer, NetworkCommunicator pollCreatorPeer, bool banPlayer, List<NetworkCommunicator> participantsToVote)
		{
			MissionPeer component = pollCreatorPeer.GetComponent<MissionPeer>();
			MissionPeer component2 = targetPeer.GetComponent<MissionPeer>();
			this._ongoingPoll = new MultiplayerPollComponent.KickPlayerPoll(this._missionLobbyComponent.MissionType, participantsToVote, targetPeer, component.Team);
			if (GameNetwork.IsServer)
			{
				MultiplayerPollComponent.MultiplayerPoll ongoingPoll = this._ongoingPoll;
				ongoingPoll.OnClosedOnServer = (Action<MultiplayerPollComponent.MultiplayerPoll>)Delegate.Combine(ongoingPoll.OnClosedOnServer, new Action<MultiplayerPollComponent.MultiplayerPoll>(this.OnKickPlayerPollClosedOnServer));
				MultiplayerPollComponent.MultiplayerPoll ongoingPoll2 = this._ongoingPoll;
				ongoingPoll2.OnCancelledOnServer = (Action<MultiplayerPollComponent.MultiplayerPoll>)Delegate.Combine(ongoingPoll2.OnCancelledOnServer, new Action<MultiplayerPollComponent.MultiplayerPoll>(this.OnPollCancelledOnServer));
			}
			Action<MissionPeer, MissionPeer, bool> onKickPollOpened = this.OnKickPollOpened;
			if (onKickPollOpened != null)
			{
				onKickPollOpened(component, component2, banPlayer);
			}
			if (GameNetwork.MyPeer == pollCreatorPeer)
			{
				this.Vote(true);
			}
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x00097D80 File Offset: 0x00095F80
		private void OnKickPlayerPollClosedOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			MultiplayerPollComponent.KickPlayerPoll kickPlayerPoll = multiplayerPoll as MultiplayerPollComponent.KickPlayerPoll;
			bool flag = kickPlayerPoll.GotEnoughAcceptVotesToEnd();
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new KickPlayerPollClosed(kickPlayerPoll.TargetPeer, flag));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			this.CloseKickPlayerPoll(flag, kickPlayerPoll.TargetPeer);
			if (flag)
			{
				DisconnectInfo disconnectInfo = kickPlayerPoll.TargetPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
				disconnectInfo.Type = DisconnectType.KickedByPoll;
				kickPlayerPoll.TargetPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
				GameNetwork.AddNetworkPeerToDisconnectAsServer(kickPlayerPoll.TargetPeer);
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x00097E10 File Offset: 0x00096010
		private void CloseKickPlayerPoll(bool accepted, NetworkCommunicator targetPeer)
		{
			if (this._ongoingPoll != null)
			{
				this._ongoingPoll.Close();
				this._ongoingPoll = null;
			}
			Action onPollClosed = this.OnPollClosed;
			if (onPollClosed != null)
			{
				onPollClosed();
			}
			if (!GameNetwork.IsDedicatedServer && accepted && !targetPeer.IsMine)
			{
				this._notificationsComponent.PlayerKicked(targetPeer);
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x00097E68 File Offset: 0x00096068
		private void OnBanPlayerPollClosedOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			MissionPeer component = (multiplayerPoll as MultiplayerPollComponent.BanPlayerPoll).TargetPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				NetworkCommunicator networkPeer = component.GetNetworkPeer();
				DisconnectInfo disconnectInfo = networkPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
				disconnectInfo.Type = DisconnectType.BannedByPoll;
				networkPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
				GameNetwork.AddNetworkPeerToDisconnectAsServer(networkPeer);
				if (GameNetwork.IsServer)
				{
					CustomGameBannedPlayerManager.AddBannedPlayer(component.Peer.Id, Environment.TickCount + 600000);
				}
				if (GameNetwork.IsDedicatedServer)
				{
					throw new NotImplementedException();
				}
				NetworkMain.GameClient.KickPlayer(component.Peer.Id, true);
			}
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x00097F10 File Offset: 0x00096110
		private void StartChangeGamePollOnServer(NetworkCommunicator pollCreatorPeer, string gameType, string scene)
		{
			if (this._ongoingPoll == null)
			{
				List<NetworkCommunicator> list = GameNetwork.NetworkPeers.ToList<NetworkCommunicator>();
				this._ongoingPoll = new MultiplayerPollComponent.ChangeGamePoll(this._missionLobbyComponent.MissionType, list, gameType, scene);
				if (GameNetwork.IsServer)
				{
					MultiplayerPollComponent.MultiplayerPoll ongoingPoll = this._ongoingPoll;
					ongoingPoll.OnClosedOnServer = (Action<MultiplayerPollComponent.MultiplayerPoll>)Delegate.Combine(ongoingPoll.OnClosedOnServer, new Action<MultiplayerPollComponent.MultiplayerPoll>(this.OnChangeGamePollClosedOnServer));
				}
				if (!GameNetwork.IsDedicatedServer)
				{
					this.ShowChangeGamePoll(gameType, scene);
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new NetworkMessages.FromServer.ChangeGamePoll(gameType, scene));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.HasOngoingPoll);
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x00097FA7 File Offset: 0x000961A7
		private void StartChangeGamePoll(string gameType, string map)
		{
			if (GameNetwork.IsServer)
			{
				if (GameNetwork.MyPeer != null)
				{
					this.StartChangeGamePollOnServer(GameNetwork.MyPeer, gameType, map);
					return;
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new NetworkMessages.FromClient.ChangeGamePoll(gameType, map));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x00097FDB File Offset: 0x000961DB
		private void ShowChangeGamePoll(string gameType, string scene)
		{
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x00097FE0 File Offset: 0x000961E0
		private void OnChangeGamePollClosedOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			MultiplayerPollComponent.ChangeGamePoll changeGamePoll = multiplayerPoll as MultiplayerPollComponent.ChangeGamePoll;
			MultiplayerOptions.OptionType.GameType.SetValue(changeGamePoll.GameType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			MultiplayerOptions.Instance.OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			MultiplayerOptions.OptionType.Map.SetValue(changeGamePoll.MapName, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this._missionLobbyComponent.SetStateEndingAsServer();
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x00098028 File Offset: 0x00096228
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<PollRequestRejected>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPollRequestRejected));
				registerer.RegisterBaseHandler<PollProgress>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdatePollProgress));
				registerer.RegisterBaseHandler<PollCancelled>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPollCancelled));
				registerer.RegisterBaseHandler<KickPlayerPollOpened>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventKickPlayerPollOpened));
				registerer.RegisterBaseHandler<KickPlayerPollClosed>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventKickPlayerPollClosed));
				registerer.RegisterBaseHandler<NetworkMessages.FromServer.ChangeGamePoll>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeGamePoll));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<PollResponse>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventPollResponse));
				registerer.RegisterBaseHandler<KickPlayerPollRequested>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventKickPlayerPollRequested));
				registerer.RegisterBaseHandler<NetworkMessages.FromClient.ChangeGamePoll>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventChangeGamePoll));
			}
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000980E8 File Offset: 0x000962E8
		private bool HandleClientEventChangeGamePoll(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromClient.ChangeGamePoll changeGamePoll = (NetworkMessages.FromClient.ChangeGamePoll)baseMessage;
			this.StartChangeGamePollOnServer(peer, changeGamePoll.GameType, changeGamePoll.Map);
			return true;
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x00098110 File Offset: 0x00096310
		private bool HandleClientEventKickPlayerPollRequested(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			KickPlayerPollRequested kickPlayerPollRequested = (KickPlayerPollRequested)baseMessage;
			this.OpenKickPlayerPollOnServer(peer, kickPlayerPollRequested.PlayerPeer, kickPlayerPollRequested.BanPlayer);
			return true;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x00098138 File Offset: 0x00096338
		private bool HandleClientEventPollResponse(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			PollResponse pollResponse = (PollResponse)baseMessage;
			this.ApplyVote(peer, pollResponse.Accepted);
			return true;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x0009815C File Offset: 0x0009635C
		private void HandleServerEventChangeGamePoll(GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromServer.ChangeGamePoll changeGamePoll = (NetworkMessages.FromServer.ChangeGamePoll)baseMessage;
			this.ShowChangeGamePoll(changeGamePoll.GameType, changeGamePoll.Map);
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x00098184 File Offset: 0x00096384
		private void HandleServerEventKickPlayerPollOpened(GameNetworkMessage baseMessage)
		{
			KickPlayerPollOpened kickPlayerPollOpened = (KickPlayerPollOpened)baseMessage;
			this.OpenKickPlayerPoll(kickPlayerPollOpened.PlayerPeer, kickPlayerPollOpened.InitiatorPeer, kickPlayerPollOpened.BanPlayer, null);
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000981B4 File Offset: 0x000963B4
		private void HandleServerEventUpdatePollProgress(GameNetworkMessage baseMessage)
		{
			PollProgress pollProgress = (PollProgress)baseMessage;
			this.UpdatePollProgress(pollProgress.VotesAccepted, pollProgress.VotesRejected);
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000981DA File Offset: 0x000963DA
		private void HandleServerEventPollCancelled(GameNetworkMessage baseMessage)
		{
			this.CancelPoll();
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000981E4 File Offset: 0x000963E4
		private void HandleServerEventKickPlayerPollClosed(GameNetworkMessage baseMessage)
		{
			KickPlayerPollClosed kickPlayerPollClosed = (KickPlayerPollClosed)baseMessage;
			this.CloseKickPlayerPoll(kickPlayerPollClosed.Accepted, kickPlayerPollClosed.PlayerPeer);
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x0009820C File Offset: 0x0009640C
		private void HandleServerEventPollRequestRejected(GameNetworkMessage baseMessage)
		{
			PollRequestRejected pollRequestRejected = (PollRequestRejected)baseMessage;
			this.RejectPoll((MultiplayerPollRejectReason)pollRequestRejected.Reason);
		}

		// Token: 0x04000F50 RID: 3920
		public const int MinimumParticipantCountRequired = 3;

		// Token: 0x04000F51 RID: 3921
		public Action<MissionPeer, MissionPeer, bool> OnKickPollOpened;

		// Token: 0x04000F52 RID: 3922
		public Action<MultiplayerPollRejectReason> OnPollRejected;

		// Token: 0x04000F53 RID: 3923
		public Action<int, int> OnPollUpdated;

		// Token: 0x04000F54 RID: 3924
		public Action OnPollClosed;

		// Token: 0x04000F55 RID: 3925
		public Action OnPollCancelled;

		// Token: 0x04000F56 RID: 3926
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000F57 RID: 3927
		private MultiplayerGameNotificationsComponent _notificationsComponent;

		// Token: 0x04000F58 RID: 3928
		private MultiplayerPollComponent.MultiplayerPoll _ongoingPoll;

		// Token: 0x020005A2 RID: 1442
		private abstract class MultiplayerPoll
		{
			// Token: 0x17000A74 RID: 2676
			// (get) Token: 0x06003DCF RID: 15823 RVA: 0x000F432D File Offset: 0x000F252D
			public MultiplayerPollComponent.MultiplayerPoll.Type PollType { get; }

			// Token: 0x17000A75 RID: 2677
			// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x000F4335 File Offset: 0x000F2535
			// (set) Token: 0x06003DD1 RID: 15825 RVA: 0x000F433D File Offset: 0x000F253D
			public bool IsOpen { get; private set; }

			// Token: 0x17000A76 RID: 2678
			// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x000F4346 File Offset: 0x000F2546
			private int OpenTime { get; }

			// Token: 0x17000A77 RID: 2679
			// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x000F434E File Offset: 0x000F254E
			// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x000F4356 File Offset: 0x000F2556
			private int CloseTime { get; set; }

			// Token: 0x17000A78 RID: 2680
			// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x000F435F File Offset: 0x000F255F
			public List<NetworkCommunicator> ParticipantsToVote
			{
				get
				{
					return this._participantsToVote;
				}
			}

			// Token: 0x06003DD6 RID: 15830 RVA: 0x000F4368 File Offset: 0x000F2568
			protected MultiplayerPoll(MultiplayerGameType gameType, MultiplayerPollComponent.MultiplayerPoll.Type pollType, List<NetworkCommunicator> participantsToVote)
			{
				this._gameType = gameType;
				this.PollType = pollType;
				if (participantsToVote != null)
				{
					this._participantsToVote = participantsToVote;
				}
				this.OpenTime = Environment.TickCount;
				this.CloseTime = 0;
				this.AcceptedCount = 0;
				this.RejectedCount = 0;
				this.IsOpen = true;
			}

			// Token: 0x06003DD7 RID: 15831 RVA: 0x000F43BA File Offset: 0x000F25BA
			public virtual bool IsCancelled()
			{
				return false;
			}

			// Token: 0x06003DD8 RID: 15832 RVA: 0x000F43BD File Offset: 0x000F25BD
			public virtual List<NetworkCommunicator> GetPollProgressReceivers()
			{
				return GameNetwork.NetworkPeers.ToList<NetworkCommunicator>();
			}

			// Token: 0x06003DD9 RID: 15833 RVA: 0x000F43CC File Offset: 0x000F25CC
			public void Tick()
			{
				if (GameNetwork.IsServer)
				{
					for (int i = this._participantsToVote.Count - 1; i >= 0; i--)
					{
						if (!this._participantsToVote[i].IsConnectionActive)
						{
							this._participantsToVote.RemoveAt(i);
						}
					}
					if (this.IsCancelled())
					{
						Action<MultiplayerPollComponent.MultiplayerPoll> onCancelledOnServer = this.OnCancelledOnServer;
						if (onCancelledOnServer == null)
						{
							return;
						}
						onCancelledOnServer(this);
						return;
					}
					else if (this.OpenTime < Environment.TickCount - 30000 || this.ResultsFinalized())
					{
						Action<MultiplayerPollComponent.MultiplayerPoll> onClosedOnServer = this.OnClosedOnServer;
						if (onClosedOnServer == null)
						{
							return;
						}
						onClosedOnServer(this);
					}
				}
			}

			// Token: 0x06003DDA RID: 15834 RVA: 0x000F445F File Offset: 0x000F265F
			public void Close()
			{
				this.CloseTime = Environment.TickCount;
				this.IsOpen = false;
			}

			// Token: 0x06003DDB RID: 15835 RVA: 0x000F4473 File Offset: 0x000F2673
			public void Cancel()
			{
				this.Close();
			}

			// Token: 0x06003DDC RID: 15836 RVA: 0x000F447C File Offset: 0x000F267C
			public bool ApplyVote(NetworkCommunicator peer, bool accepted)
			{
				bool flag = false;
				if (this._participantsToVote.Contains(peer))
				{
					if (accepted)
					{
						this.AcceptedCount++;
					}
					else
					{
						this.RejectedCount++;
					}
					this._participantsToVote.Remove(peer);
					flag = true;
				}
				return flag;
			}

			// Token: 0x06003DDD RID: 15837 RVA: 0x000F44CC File Offset: 0x000F26CC
			public bool GotEnoughAcceptVotesToEnd()
			{
				bool flag;
				if (this._gameType == MultiplayerGameType.Skirmish || this._gameType == MultiplayerGameType.Captain)
				{
					flag = this.AcceptedByAllParticipants();
				}
				else
				{
					flag = this.AcceptedByMajority();
				}
				return flag;
			}

			// Token: 0x06003DDE RID: 15838 RVA: 0x000F4504 File Offset: 0x000F2704
			private bool GotEnoughRejectVotesToEnd()
			{
				bool flag;
				if (this._gameType == MultiplayerGameType.Skirmish || this._gameType == MultiplayerGameType.Captain)
				{
					flag = this.RejectedByAtLeastOneParticipant();
				}
				else
				{
					flag = this.RejectedByMajority();
				}
				return flag;
			}

			// Token: 0x06003DDF RID: 15839 RVA: 0x000F453B File Offset: 0x000F273B
			private bool AcceptedByAllParticipants()
			{
				return this.AcceptedCount == this.GetPollParticipantCount();
			}

			// Token: 0x06003DE0 RID: 15840 RVA: 0x000F454B File Offset: 0x000F274B
			private bool AcceptedByMajority()
			{
				return (float)this.AcceptedCount / (float)this.GetPollParticipantCount() > 0.50001f;
			}

			// Token: 0x06003DE1 RID: 15841 RVA: 0x000F4563 File Offset: 0x000F2763
			private bool RejectedByAtLeastOneParticipant()
			{
				return this.RejectedCount > 0;
			}

			// Token: 0x06003DE2 RID: 15842 RVA: 0x000F456E File Offset: 0x000F276E
			private bool RejectedByMajority()
			{
				return (float)this.RejectedCount / (float)this.GetPollParticipantCount() > 0.50001f;
			}

			// Token: 0x06003DE3 RID: 15843 RVA: 0x000F4586 File Offset: 0x000F2786
			private int GetPollParticipantCount()
			{
				return this._participantsToVote.Count + this.AcceptedCount + this.RejectedCount;
			}

			// Token: 0x06003DE4 RID: 15844 RVA: 0x000F45A1 File Offset: 0x000F27A1
			private bool ResultsFinalized()
			{
				return this.GotEnoughAcceptVotesToEnd() || this.GotEnoughRejectVotesToEnd() || this._participantsToVote.Count == 0;
			}

			// Token: 0x04001EBF RID: 7871
			private const int TimeoutInSeconds = 30;

			// Token: 0x04001EC0 RID: 7872
			public Action<MultiplayerPollComponent.MultiplayerPoll> OnClosedOnServer;

			// Token: 0x04001EC1 RID: 7873
			public Action<MultiplayerPollComponent.MultiplayerPoll> OnCancelledOnServer;

			// Token: 0x04001EC2 RID: 7874
			public int AcceptedCount;

			// Token: 0x04001EC3 RID: 7875
			public int RejectedCount;

			// Token: 0x04001EC4 RID: 7876
			private readonly List<NetworkCommunicator> _participantsToVote;

			// Token: 0x04001EC5 RID: 7877
			private readonly MultiplayerGameType _gameType;

			// Token: 0x020006C1 RID: 1729
			public enum Type
			{
				// Token: 0x04002346 RID: 9030
				KickPlayer,
				// Token: 0x04002347 RID: 9031
				BanPlayer,
				// Token: 0x04002348 RID: 9032
				ChangeGame
			}
		}

		// Token: 0x020005A3 RID: 1443
		private class KickPlayerPoll : MultiplayerPollComponent.MultiplayerPoll
		{
			// Token: 0x17000A79 RID: 2681
			// (get) Token: 0x06003DE5 RID: 15845 RVA: 0x000F45C3 File Offset: 0x000F27C3
			public NetworkCommunicator TargetPeer { get; }

			// Token: 0x06003DE6 RID: 15846 RVA: 0x000F45CB File Offset: 0x000F27CB
			public KickPlayerPoll(MultiplayerGameType gameType, List<NetworkCommunicator> participantsToVote, NetworkCommunicator targetPeer, Team team)
				: base(gameType, MultiplayerPollComponent.MultiplayerPoll.Type.KickPlayer, participantsToVote)
			{
				this.TargetPeer = targetPeer;
				this._team = team;
			}

			// Token: 0x06003DE7 RID: 15847 RVA: 0x000F45E5 File Offset: 0x000F27E5
			public override bool IsCancelled()
			{
				return !this.TargetPeer.IsConnectionActive || this.TargetPeer.QuitFromMission;
			}

			// Token: 0x06003DE8 RID: 15848 RVA: 0x000F4604 File Offset: 0x000F2804
			public override List<NetworkCommunicator> GetPollProgressReceivers()
			{
				List<NetworkCommunicator> list = new List<NetworkCommunicator>();
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.Team == this._team)
					{
						list.Add(networkCommunicator);
					}
				}
				return list;
			}

			// Token: 0x04001ECA RID: 7882
			public const int RequestLimitPerPeer = 2;

			// Token: 0x04001ECB RID: 7883
			private readonly Team _team;
		}

		// Token: 0x020005A4 RID: 1444
		private class BanPlayerPoll : MultiplayerPollComponent.MultiplayerPoll
		{
			// Token: 0x17000A7A RID: 2682
			// (get) Token: 0x06003DE9 RID: 15849 RVA: 0x000F4678 File Offset: 0x000F2878
			public NetworkCommunicator TargetPeer { get; }

			// Token: 0x06003DEA RID: 15850 RVA: 0x000F4680 File Offset: 0x000F2880
			public BanPlayerPoll(MultiplayerGameType gameType, List<NetworkCommunicator> participantsToVote, NetworkCommunicator targetPeer)
				: base(gameType, MultiplayerPollComponent.MultiplayerPoll.Type.BanPlayer, participantsToVote)
			{
				this.TargetPeer = targetPeer;
			}
		}

		// Token: 0x020005A5 RID: 1445
		private class ChangeGamePoll : MultiplayerPollComponent.MultiplayerPoll
		{
			// Token: 0x17000A7B RID: 2683
			// (get) Token: 0x06003DEB RID: 15851 RVA: 0x000F4692 File Offset: 0x000F2892
			public string GameType { get; }

			// Token: 0x17000A7C RID: 2684
			// (get) Token: 0x06003DEC RID: 15852 RVA: 0x000F469A File Offset: 0x000F289A
			public string MapName { get; }

			// Token: 0x06003DED RID: 15853 RVA: 0x000F46A2 File Offset: 0x000F28A2
			public ChangeGamePoll(MultiplayerGameType currentGameType, List<NetworkCommunicator> participantsToVote, string gameType, string scene)
				: base(currentGameType, MultiplayerPollComponent.MultiplayerPoll.Type.ChangeGame, participantsToVote)
			{
				this.GameType = gameType;
				this.MapName = scene;
			}
		}
	}
}
