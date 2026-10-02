using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031D RID: 797
	public sealed class NetworkCommunicator : ICommunicator
	{
		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06002D44 RID: 11588 RVA: 0x000AF88C File Offset: 0x000ADA8C
		// (remove) Token: 0x06002D45 RID: 11589 RVA: 0x000AF8C0 File Offset: 0x000ADAC0
		public static event Action<PeerComponent> OnPeerComponentAdded;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06002D46 RID: 11590 RVA: 0x000AF8F4 File Offset: 0x000ADAF4
		// (remove) Token: 0x06002D47 RID: 11591 RVA: 0x000AF928 File Offset: 0x000ADB28
		public static event Action<NetworkCommunicator> OnPeerSynchronized;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06002D48 RID: 11592 RVA: 0x000AF95C File Offset: 0x000ADB5C
		// (remove) Token: 0x06002D49 RID: 11593 RVA: 0x000AF990 File Offset: 0x000ADB90
		public static event Action<NetworkCommunicator> OnPeerAveragePingUpdated;

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002D4A RID: 11594 RVA: 0x000AF9C3 File Offset: 0x000ADBC3
		public VirtualPlayer VirtualPlayer { get; }

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x000AF9CB File Offset: 0x000ADBCB
		// (set) Token: 0x06002D4C RID: 11596 RVA: 0x000AF9D3 File Offset: 0x000ADBD3
		public PlayerConnectionInfo PlayerConnectionInfo { get; private set; }

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x000AF9DC File Offset: 0x000ADBDC
		// (set) Token: 0x06002D4E RID: 11598 RVA: 0x000AF9E4 File Offset: 0x000ADBE4
		public bool QuitFromMission { get; set; }

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002D4F RID: 11599 RVA: 0x000AF9ED File Offset: 0x000ADBED
		// (set) Token: 0x06002D50 RID: 11600 RVA: 0x000AF9F5 File Offset: 0x000ADBF5
		public int SessionKey { get; internal set; }

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002D51 RID: 11601 RVA: 0x000AF9FE File Offset: 0x000ADBFE
		// (set) Token: 0x06002D52 RID: 11602 RVA: 0x000AFA06 File Offset: 0x000ADC06
		public bool JustReconnecting { get; private set; }

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002D53 RID: 11603 RVA: 0x000AFA0F File Offset: 0x000ADC0F
		// (set) Token: 0x06002D54 RID: 11604 RVA: 0x000AFA17 File Offset: 0x000ADC17
		public double AveragePingInMilliseconds
		{
			get
			{
				return this._averagePingInMilliseconds;
			}
			private set
			{
				if (value != this._averagePingInMilliseconds)
				{
					this._averagePingInMilliseconds = value;
					Action<NetworkCommunicator> onPeerAveragePingUpdated = NetworkCommunicator.OnPeerAveragePingUpdated;
					if (onPeerAveragePingUpdated == null)
					{
						return;
					}
					onPeerAveragePingUpdated(this);
				}
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002D55 RID: 11605 RVA: 0x000AFA39 File Offset: 0x000ADC39
		// (set) Token: 0x06002D56 RID: 11606 RVA: 0x000AFA41 File Offset: 0x000ADC41
		public double AverageLossPercent
		{
			get
			{
				return this._averageLossPercent;
			}
			private set
			{
				if (value != this._averageLossPercent)
				{
					this._averageLossPercent = value;
				}
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002D57 RID: 11607 RVA: 0x000AFA53 File Offset: 0x000ADC53
		public bool IsMine
		{
			get
			{
				return GameNetwork.MyPeer == this;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002D58 RID: 11608 RVA: 0x000AFA5D File Offset: 0x000ADC5D
		// (set) Token: 0x06002D59 RID: 11609 RVA: 0x000AFA65 File Offset: 0x000ADC65
		public bool IsAdmin { get; private set; }

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002D5A RID: 11610 RVA: 0x000AFA6E File Offset: 0x000ADC6E
		public int Index
		{
			get
			{
				return this.VirtualPlayer.Index;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002D5B RID: 11611 RVA: 0x000AFA7B File Offset: 0x000ADC7B
		public string UserName
		{
			get
			{
				return this.VirtualPlayer.UserName;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002D5C RID: 11612 RVA: 0x000AFA88 File Offset: 0x000ADC88
		// (set) Token: 0x06002D5D RID: 11613 RVA: 0x000AFA90 File Offset: 0x000ADC90
		public Agent ControlledAgent
		{
			get
			{
				return this._controlledAgent;
			}
			set
			{
				this._controlledAgent = value;
				if (GameNetwork.IsServer)
				{
					Mission mission = ((value != null) ? value.Mission : null);
					UIntPtr uintPtr = ((mission != null) ? mission.Pointer : UIntPtr.Zero);
					int num = ((value == null) ? (-1) : value.Index);
					MBAPI.IMBPeer.SetControlledAgent(this.Index, uintPtr, num);
				}
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002D5E RID: 11614 RVA: 0x000AFAE8 File Offset: 0x000ADCE8
		// (set) Token: 0x06002D5F RID: 11615 RVA: 0x000AFAF0 File Offset: 0x000ADCF0
		public bool IsMuted { get; set; }

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002D60 RID: 11616 RVA: 0x000AFAF9 File Offset: 0x000ADCF9
		// (set) Token: 0x06002D61 RID: 11617 RVA: 0x000AFB01 File Offset: 0x000ADD01
		public int ForcedAvatarIndex { get; set; } = -1;

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x000AFB0A File Offset: 0x000ADD0A
		public bool IsNetworkActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002D63 RID: 11619 RVA: 0x000AFB0D File Offset: 0x000ADD0D
		public bool IsConnectionActive
		{
			get
			{
				return GameNetwork.VirtualPlayers[this.Index] == this.VirtualPlayer && MBAPI.IMBPeer.IsActive(this.Index);
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x000AFB35 File Offset: 0x000ADD35
		// (set) Token: 0x06002D65 RID: 11621 RVA: 0x000AFB6C File Offset: 0x000ADD6C
		public bool IsSynchronized
		{
			get
			{
				if (GameNetwork.IsServer)
				{
					return GameNetwork.VirtualPlayers[this.Index] == this.VirtualPlayer && MBAPI.IMBPeer.GetIsSynchronized(this.Index);
				}
				return this._isSynchronized;
			}
			set
			{
				if (value != this._isSynchronized || this.JustReconnecting)
				{
					if (GameNetwork.IsServer)
					{
						MBAPI.IMBPeer.SetIsSynchronized(this.Index, value);
					}
					this._isSynchronized = value;
					if (this._isSynchronized)
					{
						this.JustReconnecting = false;
						Action<NetworkCommunicator> onPeerSynchronized = NetworkCommunicator.OnPeerSynchronized;
						if (onPeerSynchronized != null)
						{
							onPeerSynchronized(this);
						}
					}
					if (GameNetwork.IsServer && !this.IsServerPeer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SynchronizingDone(this, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer, this);
						GameNetwork.BeginModuleEventAsServer(this);
						GameNetwork.WriteMessage(new SynchronizingDone(this, value));
						GameNetwork.EndModuleEventAsServer();
						if (value)
						{
							MBDebug.Print("Server: " + this.UserName + " is now synchronized.", 0, Debug.DebugColor.White, 17179869184UL);
							return;
						}
						MBDebug.Print("Server: " + this.UserName + " is not synchronized.", 0, Debug.DebugColor.White, 17179869184UL);
					}
				}
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002D66 RID: 11622 RVA: 0x000AFC5D File Offset: 0x000ADE5D
		public bool IsServerPeer
		{
			get
			{
				return this._isServerPeer;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002D67 RID: 11623 RVA: 0x000AFC65 File Offset: 0x000ADE65
		// (set) Token: 0x06002D68 RID: 11624 RVA: 0x000AFC6D File Offset: 0x000ADE6D
		public ServerPerformanceState ServerPerformanceProblemState
		{
			get
			{
				return this._serverPerformanceProblemState;
			}
			private set
			{
				if (value != this._serverPerformanceProblemState)
				{
					this._serverPerformanceProblemState = value;
				}
			}
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000AFC7F File Offset: 0x000ADE7F
		private NetworkCommunicator(int index, string name, PlayerId playerID)
		{
			this.VirtualPlayer = new VirtualPlayer(index, name, playerID, this);
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000AFCA0 File Offset: 0x000ADEA0
		internal static NetworkCommunicator CreateAsServer(PlayerConnectionInfo playerConnectionInfo, int index, bool isAdmin)
		{
			NetworkCommunicator networkCommunicator = new NetworkCommunicator(index, playerConnectionInfo.Name, playerConnectionInfo.PlayerID);
			networkCommunicator.PlayerConnectionInfo = playerConnectionInfo;
			networkCommunicator.IsAdmin = isAdmin;
			MBNetworkPeer mbnetworkPeer = new MBNetworkPeer(networkCommunicator);
			MBAPI.IMBPeer.SetUserData(index, mbnetworkPeer);
			return networkCommunicator;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000AFCE0 File Offset: 0x000ADEE0
		internal static NetworkCommunicator CreateAsClient(string name, int index)
		{
			return new NetworkCommunicator(index, name, PlayerId.Empty);
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000AFCF0 File Offset: 0x000ADEF0
		void ICommunicator.OnAddComponent(PeerComponent component)
		{
			if (GameNetwork.IsServer)
			{
				if (!this.IsServerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(this);
					GameNetwork.WriteMessage(new AddPeerComponent(this, component.TypeId));
					GameNetwork.EndModuleEventAsServer();
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new AddPeerComponent(this, component.TypeId));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer | GameNetwork.EventBroadcastFlags.AddToMissionRecord, this);
			}
			Action<PeerComponent> onPeerComponentAdded = NetworkCommunicator.OnPeerComponentAdded;
			if (onPeerComponentAdded == null)
			{
				return;
			}
			onPeerComponentAdded(component);
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000AFD58 File Offset: 0x000ADF58
		void ICommunicator.OnRemoveComponent(PeerComponent component)
		{
			if (GameNetwork.IsServer)
			{
				if (!this.IsServerPeer && (this.IsSynchronized || !this.JustReconnecting))
				{
					GameNetwork.BeginModuleEventAsServer(this);
					GameNetwork.WriteMessage(new RemovePeerComponent(this, component.TypeId));
					GameNetwork.EndModuleEventAsServer();
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RemovePeerComponent(this, component.TypeId));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer | GameNetwork.EventBroadcastFlags.AddToMissionRecord, this);
			}
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000AFDBE File Offset: 0x000ADFBE
		void ICommunicator.OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component)
		{
			GameNetwork.BeginModuleEventAsServer(peer);
			GameNetwork.WriteMessage(new AddPeerComponent(this, component.TypeId));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000AFDDC File Offset: 0x000ADFDC
		internal void SetServerPeer(bool serverPeer)
		{
			this._isServerPeer = serverPeer;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000AFDE5 File Offset: 0x000ADFE5
		internal double RefreshAndGetAveragePingInMilliseconds()
		{
			this.AveragePingInMilliseconds = MBAPI.IMBPeer.GetAveragePingInMilliseconds(this.Index);
			return this.AveragePingInMilliseconds;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000AFE03 File Offset: 0x000AE003
		internal void SetAveragePingInMillisecondsAsClient(double pingValue)
		{
			this.AveragePingInMilliseconds = pingValue;
			Agent controlledAgent = this.ControlledAgent;
			if (controlledAgent == null)
			{
				return;
			}
			controlledAgent.SetAveragePingInMilliseconds(this.AveragePingInMilliseconds);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000AFE22 File Offset: 0x000AE022
		internal double RefreshAndGetAverageLossPercent()
		{
			this.AverageLossPercent = MBAPI.IMBPeer.GetAverageLossPercent(this.Index);
			return this.AverageLossPercent;
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000AFE40 File Offset: 0x000AE040
		internal void SetAverageLossPercentAsClient(double lossValue)
		{
			this.AverageLossPercent = lossValue;
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000AFE49 File Offset: 0x000AE049
		internal void SetServerPerformanceProblemStateAsClient(ServerPerformanceState serverPerformanceProblemState)
		{
			this.ServerPerformanceProblemState = serverPerformanceProblemState;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000AFE52 File Offset: 0x000AE052
		public void SetRelevantGameOptions(bool sendMeBloodEvents, bool sendMeSoundEvents)
		{
			MBAPI.IMBPeer.SetRelevantGameOptions(this.Index, sendMeBloodEvents, sendMeSoundEvents);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000AFE66 File Offset: 0x000AE066
		public uint GetHost()
		{
			return MBAPI.IMBPeer.GetHost(this.Index);
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000AFE78 File Offset: 0x000AE078
		public uint GetReversedHost()
		{
			return MBAPI.IMBPeer.GetReversedHost(this.Index);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000AFE8A File Offset: 0x000AE08A
		public ushort GetPort()
		{
			return MBAPI.IMBPeer.GetPort(this.Index);
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000AFE9C File Offset: 0x000AE09C
		public void UpdateConnectionInfoForReconnect(PlayerConnectionInfo playerConnectionInfo, bool isAdmin)
		{
			this.PlayerConnectionInfo = playerConnectionInfo;
			this.IsAdmin = isAdmin;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000AFEAC File Offset: 0x000AE0AC
		public void UpdateIndexForReconnectingPlayer(int newIndex)
		{
			this.JustReconnecting = true;
			this.VirtualPlayer.UpdateIndexForReconnectingPlayer(newIndex);
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000AFEC1 File Offset: 0x000AE0C1
		public void UpdateForJoiningCustomGame(bool isAdmin)
		{
			this.IsAdmin = isAdmin;
		}

		// Token: 0x040011DF RID: 4575
		private double _averagePingInMilliseconds;

		// Token: 0x040011E0 RID: 4576
		private double _averageLossPercent;

		// Token: 0x040011E2 RID: 4578
		private Agent _controlledAgent;

		// Token: 0x040011E3 RID: 4579
		private bool _isServerPeer;

		// Token: 0x040011E4 RID: 4580
		private bool _isSynchronized;

		// Token: 0x040011E7 RID: 4583
		private ServerPerformanceState _serverPerformanceProblemState;
	}
}
