using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CB RID: 715
	public class VoiceChatHandler : MissionNetwork
	{
		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06002949 RID: 10569 RVA: 0x0009AE04 File Offset: 0x00099004
		// (remove) Token: 0x0600294A RID: 10570 RVA: 0x0009AE3C File Offset: 0x0009903C
		public event Action OnVoiceRecordStarted;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x0600294B RID: 10571 RVA: 0x0009AE74 File Offset: 0x00099074
		// (remove) Token: 0x0600294C RID: 10572 RVA: 0x0009AEAC File Offset: 0x000990AC
		public event Action OnVoiceRecordStopped;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x0600294D RID: 10573 RVA: 0x0009AEE4 File Offset: 0x000990E4
		// (remove) Token: 0x0600294E RID: 10574 RVA: 0x0009AF1C File Offset: 0x0009911C
		public event Action<MissionPeer, bool> OnPeerVoiceStatusUpdated;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x0600294F RID: 10575 RVA: 0x0009AF54 File Offset: 0x00099154
		// (remove) Token: 0x06002950 RID: 10576 RVA: 0x0009AF8C File Offset: 0x0009918C
		public event Action<MissionPeer> OnPeerMuteStatusUpdated;

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x0009AFC1 File Offset: 0x000991C1
		// (set) Token: 0x06002952 RID: 10578 RVA: 0x0009AFCC File Offset: 0x000991CC
		private bool IsVoiceRecordActive
		{
			get
			{
				return this._isVoiceRecordActive;
			}
			set
			{
				if (!this._isVoiceChatDisabled)
				{
					this._isVoiceRecordActive = value;
					if (this._isVoiceRecordActive)
					{
						SoundManager.StartVoiceRecording();
						Action onVoiceRecordStarted = this.OnVoiceRecordStarted;
						if (onVoiceRecordStarted == null)
						{
							return;
						}
						onVoiceRecordStarted();
						return;
					}
					else
					{
						SoundManager.StopVoiceRecording();
						Action onVoiceRecordStopped = this.OnVoiceRecordStopped;
						if (onVoiceRecordStopped == null)
						{
							return;
						}
						onVoiceRecordStopped();
					}
				}
			}
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0009B01B File Offset: 0x0009921B
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<SendVoiceToPlay>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSendVoiceToPlay));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<SendVoiceRecord>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSendVoiceRecord));
			}
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0009B050 File Offset: 0x00099250
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (!GameNetwork.IsDedicatedServer)
			{
				this._playerVoiceDataList = new List<VoiceChatHandler.PeerVoiceData>();
				SoundManager.InitializeVoicePlayEvent();
				this._voiceToSend = new Queue<byte>();
			}
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0009B07C File Offset: 0x0009927C
		public override void AfterStart()
		{
			this.UpdateVoiceChatEnabled();
			if (!this._isVoiceChatDisabled)
			{
				MissionPeer.OnTeamChanged += this.MissionPeerOnTeamChanged;
				Mission.Current.GetMissionBehavior<MissionNetworkComponent>().OnClientSynchronizedEvent += this.OnPlayerSynchronized;
			}
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Combine(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0009B104 File Offset: 0x00099304
		public override void OnRemoveBehavior()
		{
			if (!this._isVoiceChatDisabled)
			{
				MissionPeer.OnTeamChanged -= this.MissionPeerOnTeamChanged;
			}
			if (!GameNetwork.IsDedicatedServer)
			{
				if (this.IsVoiceRecordActive)
				{
					this.IsVoiceRecordActive = false;
				}
				SoundManager.FinalizeVoicePlayEvent();
			}
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Remove(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			base.OnRemoveBehavior();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0009B18B File Offset: 0x0009938B
		public override void OnPreDisplayMissionTick(float dt)
		{
			if (!GameNetwork.IsDedicatedServer && !this._isVoiceChatDisabled)
			{
				this.VoiceTick(dt);
			}
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0009B1A4 File Offset: 0x000993A4
		private bool HandleClientEventSendVoiceRecord(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			SendVoiceRecord sendVoiceRecord = (SendVoiceRecord)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (sendVoiceRecord.BufferLength > 0 && component.Team != null)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
					if (networkCommunicator.IsSynchronized && component2 != null && component2.Team == component.Team && (sendVoiceRecord.ReceiverList == null || sendVoiceRecord.ReceiverList.Contains(networkCommunicator.VirtualPlayer)) && component2 != component)
					{
						GameNetwork.BeginModuleEventAsServerUnreliable(component2.Peer);
						GameNetwork.WriteMessage(new SendVoiceToPlay(peer, sendVoiceRecord.Buffer, sendVoiceRecord.BufferLength));
						GameNetwork.EndModuleEventAsServerUnreliable();
					}
				}
			}
			return true;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x0009B280 File Offset: 0x00099480
		private void HandleServerEventSendVoiceToPlay(GameNetworkMessage baseMessage)
		{
			SendVoiceToPlay sendVoiceToPlay = (SendVoiceToPlay)baseMessage;
			if (!this._isVoiceChatDisabled)
			{
				MissionPeer component = sendVoiceToPlay.Peer.GetComponent<MissionPeer>();
				if (component != null && sendVoiceToPlay.BufferLength > 0 && !component.IsMutedFromGameOrPlatform && !MultiplayerGlobalMutedPlayersManager.IsUserMuted(component.Peer.Id))
				{
					for (int i = 0; i < this._playerVoiceDataList.Count; i++)
					{
						if (this._playerVoiceDataList[i].Peer == component)
						{
							byte[] array = new byte[8640];
							int num;
							this.DecompressVoiceChunk(sendVoiceToPlay.Peer.Index, sendVoiceToPlay.Buffer, sendVoiceToPlay.BufferLength, ref array, out num);
							this._playerVoiceDataList[i].WriteVoiceData(array, num);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0009B342 File Offset: 0x00099542
		private void CheckStopVoiceRecord()
		{
			if (this._stopRecordingOnNextTick)
			{
				this.IsVoiceRecordActive = false;
				this._stopRecordingOnNextTick = false;
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0009B35C File Offset: 0x0009955C
		private void VoiceTick(float dt)
		{
			int num = 120;
			if (this._playedAnyVoicePreviousTick)
			{
				int num2 = MathF.Ceiling(dt * 1000f);
				num = MathF.Min(num, num2);
				this._playedAnyVoicePreviousTick = false;
			}
			foreach (VoiceChatHandler.PeerVoiceData peerVoiceData in this._playerVoiceDataList)
			{
				Action<MissionPeer, bool> onPeerVoiceStatusUpdated = this.OnPeerVoiceStatusUpdated;
				if (onPeerVoiceStatusUpdated != null)
				{
					onPeerVoiceStatusUpdated(peerVoiceData.Peer, peerVoiceData.HasAnyVoiceData());
				}
			}
			int num3 = num * 12;
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < this._playerVoiceDataList.Count; j++)
				{
					this._playerVoiceDataList[j].ProcessVoiceData();
				}
			}
			for (int k = 0; k < this._playerVoiceDataList.Count; k++)
			{
				Queue<short> voiceToPlayForTick = this._playerVoiceDataList[k].GetVoiceToPlayForTick();
				if (voiceToPlayForTick.Count > 0)
				{
					int count = voiceToPlayForTick.Count;
					byte[] array = new byte[count * 2];
					for (int l = 0; l < count; l++)
					{
						byte[] bytes = BitConverter.GetBytes(voiceToPlayForTick.Dequeue());
						array[l * 2] = bytes[0];
						array[l * 2 + 1] = bytes[1];
					}
					SoundManager.UpdateVoiceToPlay(array, array.Length, k);
					this._playedAnyVoicePreviousTick = true;
				}
			}
			if (this.IsVoiceRecordActive)
			{
				byte[] array2 = new byte[72000];
				int num4;
				SoundManager.GetVoiceData(array2, 72000, out num4);
				for (int m = 0; m < num4; m++)
				{
					this._voiceToSend.Enqueue(array2[m]);
				}
				this.CheckStopVoiceRecord();
			}
			while (this._voiceToSend.Count > 0 && (this._voiceToSend.Count >= 1440 || !this.IsVoiceRecordActive))
			{
				int num5 = MathF.Min(this._voiceToSend.Count, 1440);
				byte[] array3 = new byte[1440];
				for (int n = 0; n < num5; n++)
				{
					array3[n] = this._voiceToSend.Dequeue();
				}
				if (GameNetwork.IsClient)
				{
					byte[] array4 = new byte[8640];
					int num6;
					this.CompressVoiceChunk(0, array3, ref array4, out num6);
					GameNetwork.BeginModuleEventAsClientUnreliable();
					GameNetwork.WriteMessage(new SendVoiceRecord(array4, num6));
					GameNetwork.EndModuleEventAsClientUnreliable();
				}
				else if (GameNetwork.IsServer)
				{
					VoiceChatHandler.<>c__DisplayClass38_0 CS$<>8__locals1 = new VoiceChatHandler.<>c__DisplayClass38_0();
					VoiceChatHandler.<>c__DisplayClass38_0 CS$<>8__locals2 = CS$<>8__locals1;
					NetworkCommunicator myPeer = GameNetwork.MyPeer;
					CS$<>8__locals2.myMissionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
					if (CS$<>8__locals1.myMissionPeer != null)
					{
						this._playerVoiceDataList.Single<VoiceChatHandler.PeerVoiceData>((VoiceChatHandler.PeerVoiceData x) => x.Peer == CS$<>8__locals1.myMissionPeer).WriteVoiceData(array3, num5);
					}
				}
			}
			if (!this.IsVoiceRecordActive && base.Mission.InputManager.IsGameKeyPressed(33))
			{
				this.IsVoiceRecordActive = true;
			}
			if (this.IsVoiceRecordActive && base.Mission.InputManager.IsGameKeyReleased(33))
			{
				this._stopRecordingOnNextTick = true;
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0009B664 File Offset: 0x00099864
		private void DecompressVoiceChunk(int clientID, byte[] compressedVoiceBuffer, int compressedBufferLength, ref byte[] voiceBuffer, out int bufferLength)
		{
			SoundManager.DecompressData(clientID, compressedVoiceBuffer, compressedBufferLength, voiceBuffer, out bufferLength);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0009B673 File Offset: 0x00099873
		private void CompressVoiceChunk(int clientIndex, byte[] voiceBuffer, ref byte[] compressedBuffer, out int compressedBufferLength)
		{
			SoundManager.CompressData(clientIndex, voiceBuffer, 1440, compressedBuffer, out compressedBufferLength);
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0009B688 File Offset: 0x00099888
		private VoiceChatHandler.PeerVoiceData GetPlayerVoiceData(MissionPeer missionPeer)
		{
			for (int i = 0; i < this._playerVoiceDataList.Count; i++)
			{
				if (this._playerVoiceDataList[i].Peer == missionPeer)
				{
					return this._playerVoiceDataList[i];
				}
			}
			return null;
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x0009B6D0 File Offset: 0x000998D0
		private void AddPlayerToVoiceChat(MissionPeer missionPeer)
		{
			VirtualPlayer peer = missionPeer.Peer;
			this._playerVoiceDataList.Add(new VoiceChatHandler.PeerVoiceData(missionPeer));
			SoundManager.CreateVoiceEvent();
			PlatformServices.Instance.CheckPermissionWithUser(Permission.CommunicateUsingVoice, missionPeer.Peer.Id, delegate(bool hasPermission)
			{
				if (Mission.Current != null && Mission.Current.CurrentState == Mission.State.Continuing)
				{
					VoiceChatHandler.PeerVoiceData playerVoiceData = this.GetPlayerVoiceData(missionPeer);
					if (playerVoiceData != null)
					{
						if (!hasPermission)
						{
							PlayerIdProvidedTypes providedType = missionPeer.Peer.Id.ProvidedType;
							LobbyClient gameClient = NetworkMain.GameClient;
							PlayerIdProvidedTypes? playerIdProvidedTypes = ((gameClient != null) ? new PlayerIdProvidedTypes?(gameClient.PlayerID.ProvidedType) : null);
							if ((providedType == playerIdProvidedTypes.GetValueOrDefault()) & (playerIdProvidedTypes != null))
							{
								missionPeer.SetMutedFromPlatform(true);
							}
						}
						playerVoiceData.SetReadyOnPlatform();
					}
				}
			});
			missionPeer.SetMuted(PermaMuteList.IsPlayerMuted(missionPeer.Peer.Id) || MultiplayerGlobalMutedPlayersManager.IsUserMuted(missionPeer.Peer.Id));
			SoundManager.AddSoundClientWithId((ulong)((long)peer.Index));
			Action<MissionPeer> onPeerMuteStatusUpdated = this.OnPeerMuteStatusUpdated;
			if (onPeerMuteStatusUpdated == null)
			{
				return;
			}
			onPeerMuteStatusUpdated(missionPeer);
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x0009B79C File Offset: 0x0009999C
		private void RemovePlayerFromVoiceChat(int indexInVoiceDataList)
		{
			VirtualPlayer peer = this._playerVoiceDataList[indexInVoiceDataList].Peer.Peer;
			SoundManager.DeleteSoundClientWithId((ulong)((long)this._playerVoiceDataList[indexInVoiceDataList].Peer.Peer.Index));
			SoundManager.DestroyVoiceEvent(indexInVoiceDataList);
			this._playerVoiceDataList.RemoveAt(indexInVoiceDataList);
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0009B7F3 File Offset: 0x000999F3
		private void MissionPeerOnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (this._localUserInitialized && peer.VirtualPlayer.Id != PlayerId.Empty)
			{
				this.CheckPlayerForVoiceChatOnTeamChange(peer, previousTeam, newTeam);
			}
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x0009B820 File Offset: 0x00099A20
		private void OnPlayerSynchronized(NetworkCommunicator networkPeer)
		{
			if (this._localUserInitialized)
			{
				MissionPeer component = networkPeer.GetComponent<MissionPeer>();
				if (!component.IsMine && component.Team != null)
				{
					this.CheckPlayerForVoiceChatOnTeamChange(networkPeer, null, component.Team);
					return;
				}
			}
			else if (networkPeer.IsMine)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
				this.CheckPlayerForVoiceChatOnTeamChange(GameNetwork.MyPeer, null, missionPeer.Team);
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x0009B888 File Offset: 0x00099A88
		private void CheckPlayerForVoiceChatOnTeamChange(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (GameNetwork.VirtualPlayers[peer.Index] == peer.VirtualPlayer)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
				if (missionPeer != null)
				{
					MissionPeer component = peer.GetComponent<MissionPeer>();
					if (missionPeer == component)
					{
						this._localUserInitialized = true;
						for (int i = this._playerVoiceDataList.Count - 1; i >= 0; i--)
						{
							this.RemovePlayerFromVoiceChat(i);
						}
						if (newTeam == null)
						{
							return;
						}
						using (List<NetworkCommunicator>.Enumerator enumerator = GameNetwork.NetworkPeers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								NetworkCommunicator networkCommunicator = enumerator.Current;
								MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
								if (missionPeer != component2 && ((component2 != null) ? component2.Team : null) != null && component2.Team == newTeam && networkCommunicator.VirtualPlayer.Id != PlayerId.Empty)
								{
									this.AddPlayerToVoiceChat(component2);
								}
							}
							return;
						}
					}
					if (this._localUserInitialized && missionPeer.Team != null)
					{
						if (missionPeer.Team == previousTeam)
						{
							for (int j = 0; j < this._playerVoiceDataList.Count; j++)
							{
								if (this._playerVoiceDataList[j].Peer == component)
								{
									this.RemovePlayerFromVoiceChat(j);
									return;
								}
							}
							return;
						}
						if (missionPeer.Team == newTeam)
						{
							this.AddPlayerToVoiceChat(component);
						}
					}
				}
			}
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x0009B9E8 File Offset: 0x00099BE8
		private void UpdateVoiceChatEnabled()
		{
			float num = 1f;
			this._isVoiceChatDisabled = !BannerlordConfig.EnableVoiceChat || num <= 1E-05f || Game.Current.GetGameHandler<ChatBox>().IsContentRestricted;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0009BA22 File Offset: 0x00099C22
		private void OnNativeOptionChanged(NativeOptions.NativeOptionsType changedNativeOptionsType)
		{
			if (changedNativeOptionsType == NativeOptions.NativeOptionsType.VoiceChatVolume)
			{
				this.UpdateVoiceChatEnabled();
			}
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x0009BA2E File Offset: 0x00099C2E
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionType)
		{
			if (changedManagedOptionType == ManagedOptions.ManagedOptionsType.EnableVoiceChat)
			{
				this.UpdateVoiceChatEnabled();
			}
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x0009BA3C File Offset: 0x00099C3C
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			base.OnPlayerDisconnectedFromServer(networkPeer);
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (((component != null) ? component.Team : null) != null && ((missionPeer != null) ? missionPeer.Team : null) != null && component.Team == missionPeer.Team)
			{
				for (int i = 0; i < this._playerVoiceDataList.Count; i++)
				{
					if (this._playerVoiceDataList[i].Peer == component)
					{
						this.RemovePlayerFromVoiceChat(i);
						return;
					}
				}
			}
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x0009BAC8 File Offset: 0x00099CC8
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (networkPeer.IsMuted)
			{
				MultiplayerGlobalMutedPlayersManager.MutePlayer(networkPeer.VirtualPlayer.Id);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SyncPlayerMuteState(networkPeer.VirtualPlayer.Id, networkPeer.IsMuted));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new SyncMutedPlayers(MultiplayerGlobalMutedPlayersManager.MutedPlayers));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x04000FD0 RID: 4048
		private const int MillisecondsToShorts = 12;

		// Token: 0x04000FD1 RID: 4049
		private const int MillisecondsToBytes = 24;

		// Token: 0x04000FD2 RID: 4050
		private const int OpusFrameSizeCoefficient = 6;

		// Token: 0x04000FD3 RID: 4051
		private const int VoiceFrameRawSizeInMilliseconds = 60;

		// Token: 0x04000FD4 RID: 4052
		public const int VoiceFrameRawSizeInBytes = 1440;

		// Token: 0x04000FD5 RID: 4053
		private const int CompressionMaxChunkSizeInBytes = 8640;

		// Token: 0x04000FD6 RID: 4054
		private const int VoiceRecordMaxChunkSizeInBytes = 72000;

		// Token: 0x04000FDB RID: 4059
		private List<VoiceChatHandler.PeerVoiceData> _playerVoiceDataList;

		// Token: 0x04000FDC RID: 4060
		private bool _isVoiceChatDisabled = true;

		// Token: 0x04000FDD RID: 4061
		private bool _isVoiceRecordActive;

		// Token: 0x04000FDE RID: 4062
		private bool _stopRecordingOnNextTick;

		// Token: 0x04000FDF RID: 4063
		private Queue<byte> _voiceToSend;

		// Token: 0x04000FE0 RID: 4064
		private bool _playedAnyVoicePreviousTick;

		// Token: 0x04000FE1 RID: 4065
		private bool _localUserInitialized;

		// Token: 0x020005AD RID: 1453
		private class PeerVoiceData
		{
			// Token: 0x17000A7D RID: 2685
			// (get) Token: 0x06003E00 RID: 15872 RVA: 0x000F47CB File Offset: 0x000F29CB
			// (set) Token: 0x06003E01 RID: 15873 RVA: 0x000F47D3 File Offset: 0x000F29D3
			public bool IsReadyOnPlatform { get; private set; }

			// Token: 0x06003E02 RID: 15874 RVA: 0x000F47DC File Offset: 0x000F29DC
			public PeerVoiceData(MissionPeer peer)
			{
				this.Peer = peer;
				this._voiceData = new Queue<short>();
				this._voiceToPlayInTick = new Queue<short>();
				this._nextPlayDelayResetTime = MissionTime.Now;
			}

			// Token: 0x06003E03 RID: 15875 RVA: 0x000F480C File Offset: 0x000F2A0C
			public void WriteVoiceData(byte[] dataBuffer, int bufferSize)
			{
				if (this._voiceData.Count == 0 && this._nextPlayDelayResetTime.IsPast)
				{
					this._playDelayRemainingSizeInBytes = 3600;
				}
				for (int i = 0; i < bufferSize; i += 2)
				{
					short num = (short)((int)dataBuffer[i] | ((int)dataBuffer[i + 1] << 8));
					this._voiceData.Enqueue(num);
				}
			}

			// Token: 0x06003E04 RID: 15876 RVA: 0x000F4863 File Offset: 0x000F2A63
			public void SetReadyOnPlatform()
			{
				this.IsReadyOnPlatform = true;
			}

			// Token: 0x06003E05 RID: 15877 RVA: 0x000F486C File Offset: 0x000F2A6C
			public bool ProcessVoiceData()
			{
				if (this.IsReadyOnPlatform && this._voiceData.Count > 0)
				{
					bool flag = this.Peer.IsMutedFromGameOrPlatform || MultiplayerGlobalMutedPlayersManager.IsUserMuted(this.Peer.Peer.Id);
					if (this._playDelayRemainingSizeInBytes > 0)
					{
						this._playDelayRemainingSizeInBytes -= 2;
					}
					else
					{
						short num = this._voiceData.Dequeue();
						this._nextPlayDelayResetTime = MissionTime.Now + MissionTime.Milliseconds(300f);
						if (!flag)
						{
							this._voiceToPlayInTick.Enqueue(num);
						}
					}
					return !flag;
				}
				return false;
			}

			// Token: 0x06003E06 RID: 15878 RVA: 0x000F490C File Offset: 0x000F2B0C
			public Queue<short> GetVoiceToPlayForTick()
			{
				return this._voiceToPlayInTick;
			}

			// Token: 0x06003E07 RID: 15879 RVA: 0x000F4914 File Offset: 0x000F2B14
			public bool HasAnyVoiceData()
			{
				return this.IsReadyOnPlatform && this._voiceData.Count > 0;
			}

			// Token: 0x04001EDE RID: 7902
			private const int PlayDelaySizeInMilliseconds = 150;

			// Token: 0x04001EDF RID: 7903
			private const int PlayDelaySizeInBytes = 3600;

			// Token: 0x04001EE0 RID: 7904
			private const float PlayDelayResetTimeInMilliseconds = 300f;

			// Token: 0x04001EE1 RID: 7905
			public readonly MissionPeer Peer;

			// Token: 0x04001EE3 RID: 7907
			private readonly Queue<short> _voiceData;

			// Token: 0x04001EE4 RID: 7908
			private readonly Queue<short> _voiceToPlayInTick;

			// Token: 0x04001EE5 RID: 7909
			private int _playDelayRemainingSizeInBytes;

			// Token: 0x04001EE6 RID: 7910
			private MissionTime _nextPlayDelayResetTime;
		}
	}
}
