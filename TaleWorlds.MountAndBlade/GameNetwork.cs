using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F1 RID: 753
	public static class GameNetwork
	{
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x000A50CD File Offset: 0x000A32CD
		public static bool IsServer
		{
			get
			{
				return MBCommon.CurrentGameType == MBCommon.GameType.MultiServer || MBCommon.CurrentGameType == MBCommon.GameType.MultiClientServer;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x000A50E1 File Offset: 0x000A32E1
		public static bool IsServerOrRecorder
		{
			get
			{
				return GameNetwork.IsServer || MBCommon.CurrentGameType == MBCommon.GameType.SingleRecord;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002AEA RID: 10986 RVA: 0x000A50F4 File Offset: 0x000A32F4
		public static bool IsClient
		{
			get
			{
				return MBCommon.CurrentGameType == MBCommon.GameType.MultiClient;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000A50FE File Offset: 0x000A32FE
		public static bool IsReplay
		{
			get
			{
				return MBCommon.CurrentGameType == MBCommon.GameType.SingleReplay;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000A5108 File Offset: 0x000A3308
		public static bool IsClientOrReplay
		{
			get
			{
				return GameNetwork.IsClient || GameNetwork.IsReplay;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002AED RID: 10989 RVA: 0x000A5118 File Offset: 0x000A3318
		public static bool IsDedicatedServer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x000A511B File Offset: 0x000A331B
		public static bool MultiplayerDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x000A511E File Offset: 0x000A331E
		public static bool IsMultiplayer
		{
			get
			{
				return GameNetwork.IsServer || GameNetwork.IsClient;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000A512E File Offset: 0x000A332E
		public static bool IsMultiplayerOrReplay
		{
			get
			{
				return GameNetwork.IsMultiplayer || GameNetwork.IsReplay;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000A513E File Offset: 0x000A333E
		public static bool IsSessionActive
		{
			get
			{
				return GameNetwork.IsServerOrRecorder || GameNetwork.IsClientOrReplay;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x000A514E File Offset: 0x000A334E
		public static IEnumerable<NetworkCommunicator> NetworkPeersIncludingDisconnectedPeers
		{
			get
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					yield return networkCommunicator;
				}
				List<NetworkCommunicator>.Enumerator enumerator = default(List<NetworkCommunicator>.Enumerator);
				int num;
				for (int i = 0; i < GameNetwork.DisconnectedNetworkPeers.Count; i = num + 1)
				{
					yield return GameNetwork.DisconnectedNetworkPeers[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x000A5157 File Offset: 0x000A3357
		// (set) Token: 0x06002AF4 RID: 10996 RVA: 0x000A515E File Offset: 0x000A335E
		public static VirtualPlayer[] VirtualPlayers { get; private set; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000A5166 File Offset: 0x000A3366
		// (set) Token: 0x06002AF6 RID: 10998 RVA: 0x000A516D File Offset: 0x000A336D
		public static List<NetworkCommunicator> NetworkPeers { get; private set; }

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000A5175 File Offset: 0x000A3375
		// (set) Token: 0x06002AF8 RID: 11000 RVA: 0x000A517C File Offset: 0x000A337C
		public static List<NetworkCommunicator> DisconnectedNetworkPeers { get; private set; }

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x000A5184 File Offset: 0x000A3384
		public static int NetworkPeerCount
		{
			get
			{
				return GameNetwork.NetworkPeers.Count;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x000A5190 File Offset: 0x000A3390
		public static bool NetworkPeersValid
		{
			get
			{
				return GameNetwork.NetworkPeers != null;
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000A519A File Offset: 0x000A339A
		private static void AddNetworkPeer(NetworkCommunicator networkPeer)
		{
			GameNetwork.NetworkPeers.Add(networkPeer);
			Debug.Print("AddNetworkPeer: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000A51C8 File Offset: 0x000A33C8
		private static void RemoveNetworkPeer(NetworkCommunicator networkPeer)
		{
			Debug.Print("RemoveNetworkPeer: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.NetworkPeers.Remove(networkPeer);
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000A51F7 File Offset: 0x000A33F7
		private static void AddToDisconnectedPeers(NetworkCommunicator networkPeer)
		{
			Debug.Print("AddToDisconnectedPeers: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.DisconnectedNetworkPeers.Add(networkPeer);
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000A5228 File Offset: 0x000A3428
		public static void ClearAllPeers()
		{
			if (GameNetwork.VirtualPlayers != null)
			{
				for (int i = 0; i < GameNetwork.VirtualPlayers.Length; i++)
				{
					GameNetwork.VirtualPlayers[i] = null;
				}
				GameNetwork.NetworkPeers.Clear();
				GameNetwork.DisconnectedNetworkPeers.Clear();
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000A526C File Offset: 0x000A346C
		public static NetworkCommunicator FindNetworkPeer(int index)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.Index == index)
				{
					return networkCommunicator;
				}
			}
			return null;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000A52C8 File Offset: 0x000A34C8
		public static void Initialize(IGameNetworkHandler handler)
		{
			GameNetwork._handler = handler;
			GameNetwork.VirtualPlayers = new VirtualPlayer[1023];
			GameNetwork.NetworkPeers = new List<NetworkCommunicator>();
			GameNetwork.DisconnectedNetworkPeers = new List<NetworkCommunicator>();
			MBNetwork.Initialize(new NetworkCommunication());
			GameNetwork.NetworkComponents = new List<UdpNetworkComponent>();
			GameNetwork.NetworkHandlers = new List<IUdpNetworkHandler>();
			GameNetwork._handler.OnInitialize();
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000A5328 File Offset: 0x000A3528
		internal static void Tick(float dt)
		{
			int i = 0;
			try
			{
				for (i = 0; i < GameNetwork.NetworkHandlers.Count; i++)
				{
					GameNetwork.NetworkHandlers[i].OnUdpNetworkHandlerTick(dt);
				}
			}
			catch (Exception ex)
			{
				if (GameNetwork.NetworkHandlers.Count > 0 && i < GameNetwork.NetworkHandlers.Count && GameNetwork.NetworkHandlers[i] != null)
				{
					string text = GameNetwork.NetworkHandlers[i].ToString();
					Debug.Print("Exception On Network Component: " + text, 0, Debug.DebugColor.White, 17592186044416UL);
				}
				Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000A53F8 File Offset: 0x000A35F8
		private static void StartMultiplayer()
		{
			VirtualPlayer.Reset();
			GameNetwork._handler.OnStartMultiplayer();
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000A540C File Offset: 0x000A360C
		public static void EndMultiplayer()
		{
			GameNetwork._handler.OnEndMultiplayer();
			for (int i = GameNetwork.NetworkComponents.Count - 1; i >= 0; i--)
			{
				GameNetwork.DestroyComponent(GameNetwork.NetworkComponents[i]);
			}
			for (int j = GameNetwork.NetworkHandlers.Count - 1; j >= 0; j--)
			{
				GameNetwork.RemoveNetworkHandler(GameNetwork.NetworkHandlers[j]);
			}
			if (GameNetwork.IsServer)
			{
				GameNetwork.TerminateServerSide();
			}
			if (GameNetwork.IsClientOrReplay)
			{
				GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.TerminateClientSide();
			}
			Debug.Print("Clearing peers list with count " + GameNetwork.NetworkPeerCount, 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.ClearAllPeers();
			VirtualPlayer.Reset();
			GameNetwork.MyPeer = null;
			Debug.Print("NetworkManager::HandleMultiplayerEnd", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000A54E4 File Offset: 0x000A36E4
		[MBCallback(null, false)]
		internal static void HandleRemovePlayer(MBNetworkPeer peer, bool isTimedOut)
		{
			DisconnectInfo disconnectInfo;
			if ((disconnectInfo = peer.NetworkPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo")) == null)
			{
				(disconnectInfo = new DisconnectInfo()).Type = DisconnectType.QuitFromGame;
			}
			DisconnectInfo disconnectInfo2 = disconnectInfo;
			disconnectInfo2.Type = (isTimedOut ? DisconnectType.TimedOut : disconnectInfo2.Type);
			peer.NetworkPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo2);
			GameNetwork.HandleRemovePlayerInternal(peer.NetworkPeer, peer.NetworkPeer.IsSynchronized && MultiplayerIntermissionVotingManager.Instance.CurrentVoteState == MultiplayerIntermissionState.Idle);
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000A5568 File Offset: 0x000A3768
		internal static void HandleRemovePlayerInternal(NetworkCommunicator networkPeer, bool isDisconnected)
		{
			if (GameNetwork.IsClient && networkPeer.IsMine)
			{
				GameNetwork.HandleDisconnect();
				return;
			}
			GameNetwork._handler.OnPlayerDisconnectedFromServer(networkPeer);
			if (GameNetwork.IsServer)
			{
				foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
				{
					udpNetworkHandler.HandleEarlyPlayerDisconnect(networkPeer);
				}
				foreach (IUdpNetworkHandler udpNetworkHandler2 in GameNetwork.NetworkHandlers)
				{
					udpNetworkHandler2.HandlePlayerDisconnect(networkPeer);
				}
			}
			foreach (IUdpNetworkHandler udpNetworkHandler3 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler3.OnPlayerDisconnectedFromServer(networkPeer);
			}
			GameNetwork.RemoveNetworkPeer(networkPeer);
			if (isDisconnected)
			{
				GameNetwork.AddToDisconnectedPeers(networkPeer);
			}
			GameNetwork.VirtualPlayers[networkPeer.VirtualPlayer.Index] = null;
			if (GameNetwork.IsServer)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (!networkCommunicator.IsServerPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkCommunicator);
						GameNetwork.WriteMessage(new DeletePlayer(networkPeer.Index, isDisconnected));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000A56E4 File Offset: 0x000A38E4
		[MBCallback(null, false)]
		internal static void HandleDisconnect()
		{
			GameNetwork._handler.OnDisconnectedFromServer();
			foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler.OnDisconnectedFromServer();
			}
			GameNetwork.MyPeer = null;
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000A5744 File Offset: 0x000A3944
		public static void StartReplay()
		{
			GameNetwork._handler.OnStartReplay();
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000A5750 File Offset: 0x000A3950
		public static void EndReplay()
		{
			GameNetwork._handler.OnEndReplay();
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000A575C File Offset: 0x000A395C
		public static void PreStartMultiplayerOnServer()
		{
			MBCommon.CurrentGameType = (GameNetwork.IsDedicatedServer ? MBCommon.GameType.MultiServer : MBCommon.GameType.MultiClientServer);
			GameNetwork.ClientPeerIndex = -1;
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000A5774 File Offset: 0x000A3974
		public static void StartMultiplayerOnServer(int port)
		{
			Debug.Print("StartMultiplayerOnServer", 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.PreStartMultiplayerOnServer();
			GameNetwork.InitializeServerSide(port);
			GameNetwork.StartMultiplayer();
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000A579C File Offset: 0x000A399C
		[MBCallback(null, false)]
		internal static bool HandleNetworkPacketAsServer(MBNetworkPeer networkPeer)
		{
			return GameNetwork.HandleNetworkPacketAsServer(networkPeer.NetworkPeer);
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000A57AC File Offset: 0x000A39AC
		internal static bool HandleNetworkPacketAsServer(NetworkCommunicator networkPeer)
		{
			if (networkPeer == null)
			{
				Debug.Print("networkPeer == null", 0, Debug.DebugColor.White, 17592186044416UL);
				return false;
			}
			bool flag = true;
			try
			{
				int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.NetworkComponentEventTypeFromClientCompressionInfo, ref flag);
				if (flag)
				{
					if (num >= 0 && num < GameNetwork._gameNetworkMessageIdsFromClient.Count)
					{
						GameNetworkMessage gameNetworkMessage = Activator.CreateInstance(GameNetwork._gameNetworkMessageIdsFromClient[num]) as GameNetworkMessage;
						gameNetworkMessage.MessageId = num;
						flag = gameNetworkMessage.Read();
						if (flag)
						{
							bool flag2 = false;
							bool flag3 = true;
							List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>> list;
							if (GameNetwork._fromClientBaseMessageHandlers.TryGetValue(num, out list))
							{
								foreach (GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> clientMessageHandlerDelegate in list)
								{
									flag = flag && clientMessageHandlerDelegate(networkPeer, gameNetworkMessage);
									if (!flag)
									{
										break;
									}
								}
								flag3 = false;
								flag2 = list.Count != 0;
							}
							List<object> list2;
							if (GameNetwork._fromClientMessageHandlers.TryGetValue(num, out list2))
							{
								foreach (object obj in list2)
								{
									Delegate @delegate = obj as Delegate;
									flag = flag && (bool)@delegate.DynamicInvokeWithLog(new object[] { networkPeer, gameNetworkMessage });
									if (!flag)
									{
										break;
									}
								}
								flag3 = false;
								flag2 = flag2 || list2.Count != 0;
							}
							if (flag3)
							{
								Debug.FailedAssert("Unknown network messageId " + gameNetworkMessage, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\GameNetwork.cs", "HandleNetworkPacketAsServer", 760);
								flag = false;
							}
							else if (!flag2)
							{
								Debug.Print("Handler not found for network message " + gameNetworkMessage, 0, Debug.DebugColor.White, 17179869184UL);
							}
						}
					}
					else
					{
						Debug.Print("Handler not found for network message " + num.ToString(), 0, Debug.DebugColor.White, 17179869184UL);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Print("error " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				return false;
			}
			return flag;
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000A59EC File Offset: 0x000A3BEC
		[MBCallback(null, false)]
		public static void HandleConsoleCommand(string command)
		{
			if (GameNetwork._handler != null)
			{
				GameNetwork._handler.OnHandleConsoleCommand(command);
			}
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000A5A00 File Offset: 0x000A3C00
		private static void InitializeServerSide(int port)
		{
			MBAPI.IMBNetwork.InitializeServerSide(port);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000A5A0D File Offset: 0x000A3C0D
		private static void TerminateServerSide()
		{
			MBAPI.IMBNetwork.TerminateServerSide();
			if (!GameNetwork.IsDedicatedServer)
			{
				MBCommon.CurrentGameType = MBCommon.GameType.Single;
			}
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000A5A26 File Offset: 0x000A3C26
		private static void PrepareNewUdpSession(int peerIndex, int sessionKey)
		{
			MBAPI.IMBNetwork.PrepareNewUdpSession(peerIndex, sessionKey);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000A5A34 File Offset: 0x000A3C34
		public static string GetActiveUdpSessionsIpAddress()
		{
			return MBAPI.IMBNetwork.GetActiveUdpSessionsIpAddress();
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000A5A40 File Offset: 0x000A3C40
		public static ICommunicator AddNewPlayerOnServer(PlayerConnectionInfo playerConnectionInfo, bool serverPeer, bool isAdmin)
		{
			bool flag = playerConnectionInfo == null;
			int num = (flag ? MBAPI.IMBNetwork.AddNewBotOnServer() : MBAPI.IMBNetwork.AddNewPlayerOnServer(serverPeer));
			Debug.Print(string.Concat(new object[] { "AddNewPlayerOnServer: ", playerConnectionInfo.Name, " index: ", num }), 0, Debug.DebugColor.White, 17179869184UL);
			if (num >= 0)
			{
				int num2 = 0;
				if (!serverPeer)
				{
					num2 = GameNetwork.GetSessionKeyForPlayer();
				}
				int num3 = -1;
				ICommunicator communicator = null;
				if (flag)
				{
					communicator = DummyCommunicator.CreateAsServer(num, "");
				}
				else
				{
					for (int i = 0; i < GameNetwork.DisconnectedNetworkPeers.Count; i++)
					{
						PlayerData parameter = playerConnectionInfo.GetParameter<PlayerData>("PlayerData");
						if (parameter != null && GameNetwork.DisconnectedNetworkPeers[i].VirtualPlayer.Id == parameter.PlayerId)
						{
							num3 = i;
							communicator = GameNetwork.DisconnectedNetworkPeers[i];
							NetworkCommunicator networkCommunicator = communicator as NetworkCommunicator;
							networkCommunicator.UpdateIndexForReconnectingPlayer(num);
							networkCommunicator.UpdateConnectionInfoForReconnect(playerConnectionInfo, isAdmin);
							MBAPI.IMBPeer.SetUserData(num, new MBNetworkPeer(networkCommunicator));
							Debug.Print("RemoveFromDisconnectedPeers: " + networkCommunicator.UserName, 0, Debug.DebugColor.White, 17179869184UL);
							GameNetwork.DisconnectedNetworkPeers.RemoveAt(i);
							break;
						}
					}
					if (communicator == null)
					{
						communicator = NetworkCommunicator.CreateAsServer(playerConnectionInfo, num, isAdmin);
					}
				}
				GameNetwork.VirtualPlayers[communicator.VirtualPlayer.Index] = communicator.VirtualPlayer;
				if (!flag)
				{
					NetworkCommunicator networkCommunicator2 = communicator as NetworkCommunicator;
					if (serverPeer && GameNetwork.IsServer)
					{
						GameNetwork.ClientPeerIndex = num;
						GameNetwork.MyPeer = networkCommunicator2;
					}
					networkCommunicator2.SessionKey = num2;
					networkCommunicator2.SetServerPeer(serverPeer);
					GameNetwork.AddNetworkPeer(networkCommunicator2);
					playerConnectionInfo.NetworkPeer = networkCommunicator2;
					if (!serverPeer)
					{
						GameNetwork.PrepareNewUdpSession(num, num2);
					}
					if (num3 < 0)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator2.Index, playerConnectionInfo.Name, num3, false, false));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord | GameNetwork.EventBroadcastFlags.DontSendToPeers, null);
					}
					foreach (NetworkCommunicator networkCommunicator3 in GameNetwork.NetworkPeers)
					{
						if (networkCommunicator3 != networkCommunicator2 && networkCommunicator3 != GameNetwork.MyPeer)
						{
							GameNetwork.BeginModuleEventAsServer(networkCommunicator3);
							GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator2.Index, playerConnectionInfo.Name, num3, false, false));
							GameNetwork.EndModuleEventAsServer();
						}
						if (!serverPeer)
						{
							bool flag2 = networkCommunicator3 == networkCommunicator2;
							GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
							GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator3.Index, networkCommunicator3.UserName, -1, false, flag2));
							GameNetwork.EndModuleEventAsServer();
						}
					}
					for (int j = 0; j < GameNetwork.DisconnectedNetworkPeers.Count; j++)
					{
						NetworkCommunicator networkCommunicator4 = GameNetwork.DisconnectedNetworkPeers[j];
						GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
						GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator4.Index, networkCommunicator4.UserName, j, true, false));
						GameNetwork.EndModuleEventAsServer();
					}
					foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
					{
						udpNetworkHandler.HandleNewClientConnect(playerConnectionInfo);
					}
					GameNetwork._handler.OnPlayerConnectedToServer(networkCommunicator2);
				}
				return communicator;
			}
			return null;
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000A5D88 File Offset: 0x000A3F88
		public static GameNetwork.AddPlayersResult AddNewPlayersOnServer(PlayerConnectionInfo[] playerConnectionInfos, bool serverPeer)
		{
			bool flag = MBAPI.IMBNetwork.CanAddNewPlayersOnServer(playerConnectionInfos.Length);
			NetworkCommunicator[] array = new NetworkCommunicator[playerConnectionInfos.Length];
			if (flag)
			{
				for (int i = 0; i < array.Length; i++)
				{
					object parameter = playerConnectionInfos[i].GetParameter<object>("IsAdmin");
					bool flag2 = parameter != null && (bool)parameter;
					ICommunicator communicator = GameNetwork.AddNewPlayerOnServer(playerConnectionInfos[i], serverPeer, flag2);
					array[i] = communicator as NetworkCommunicator;
				}
			}
			return new GameNetwork.AddPlayersResult
			{
				NetworkPeers = array,
				Success = flag
			};
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x000A5E0C File Offset: 0x000A400C
		public static void ClientFinishedLoading(NetworkCommunicator networkPeer)
		{
			foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler.HandleEarlyNewClientAfterLoadingFinished(networkPeer);
			}
			foreach (IUdpNetworkHandler udpNetworkHandler2 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler2.HandleNewClientAfterLoadingFinished(networkPeer);
			}
			foreach (IUdpNetworkHandler udpNetworkHandler3 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler3.HandleLateNewClientAfterLoadingFinished(networkPeer);
			}
			networkPeer.IsSynchronized = true;
			foreach (IUdpNetworkHandler udpNetworkHandler4 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler4.HandleNewClientAfterSynchronized(networkPeer);
			}
			foreach (IUdpNetworkHandler udpNetworkHandler5 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler5.HandleLateNewClientAfterSynchronized(networkPeer);
			}
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000A5F60 File Offset: 0x000A4160
		public static void BeginModuleEventAsClient()
		{
			MBAPI.IMBNetwork.BeginModuleEventAsClient(true);
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000A5F6D File Offset: 0x000A416D
		public static void EndModuleEventAsClient()
		{
			MBAPI.IMBNetwork.EndModuleEventAsClient(true);
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000A5F7A File Offset: 0x000A417A
		public static void BeginModuleEventAsClientUnreliable()
		{
			MBAPI.IMBNetwork.BeginModuleEventAsClient(false);
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000A5F87 File Offset: 0x000A4187
		public static void EndModuleEventAsClientUnreliable()
		{
			MBAPI.IMBNetwork.EndModuleEventAsClient(false);
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000A5F94 File Offset: 0x000A4194
		public static void BeginModuleEventAsServer(NetworkCommunicator communicator)
		{
			GameNetwork.BeginModuleEventAsServer(communicator.VirtualPlayer);
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000A5FA1 File Offset: 0x000A41A1
		public static void BeginModuleEventAsServerUnreliable(NetworkCommunicator communicator)
		{
			GameNetwork.BeginModuleEventAsServerUnreliable(communicator.VirtualPlayer);
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000A5FAE File Offset: 0x000A41AE
		public static void BeginModuleEventAsServer(VirtualPlayer peer)
		{
			MBAPI.IMBPeer.BeginModuleEvent(peer.Index, true);
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000A5FC1 File Offset: 0x000A41C1
		public static void EndModuleEventAsServer()
		{
			MBAPI.IMBPeer.EndModuleEvent(true);
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000A5FCE File Offset: 0x000A41CE
		public static void BeginModuleEventAsServerUnreliable(VirtualPlayer peer)
		{
			MBAPI.IMBPeer.BeginModuleEvent(peer.Index, false);
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000A5FE1 File Offset: 0x000A41E1
		public static void EndModuleEventAsServerUnreliable()
		{
			MBAPI.IMBPeer.EndModuleEvent(false);
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000A5FEE File Offset: 0x000A41EE
		public static void BeginBroadcastModuleEvent()
		{
			MBAPI.IMBNetwork.BeginBroadcastModuleEvent();
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000A5FFC File Offset: 0x000A41FC
		public static void EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags broadcastFlags, NetworkCommunicator targetPlayer = null)
		{
			int num = ((targetPlayer != null) ? targetPlayer.Index : (-1));
			MBAPI.IMBNetwork.EndBroadcastModuleEvent((int)broadcastFlags, num, true);
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000A6023 File Offset: 0x000A4223
		public static double ElapsedTimeSinceLastUdpPacketArrived()
		{
			return MBAPI.IMBNetwork.ElapsedTimeSinceLastUdpPacketArrived();
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000A6030 File Offset: 0x000A4230
		public static void EndBroadcastModuleEventUnreliable(GameNetwork.EventBroadcastFlags broadcastFlags, NetworkCommunicator targetPlayer = null)
		{
			int num = ((targetPlayer != null) ? targetPlayer.Index : (-1));
			MBAPI.IMBNetwork.EndBroadcastModuleEvent((int)broadcastFlags, num, false);
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000A6058 File Offset: 0x000A4258
		public static void UnSynchronizeEveryone()
		{
			Debug.Print("UnSynchronizeEveryone is called!", 0, Debug.DebugColor.White, 17179869184UL);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				networkCommunicator.IsSynchronized = false;
			}
			foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler.OnEveryoneUnSynchronized();
			}
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000A60FC File Offset: 0x000A42FC
		public static void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			networkMessageHandlerRegisterer.Register<CreatePlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<CreatePlayer>(GameNetwork.HandleServerEventCreatePlayer));
			networkMessageHandlerRegisterer.Register<DeletePlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<DeletePlayer>(GameNetwork.HandleServerEventDeletePlayer));
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000A6127 File Offset: 0x000A4327
		public static void StartMultiplayerOnClient(string serverAddress, int port, int sessionKey, int playerIndex)
		{
			Debug.Print("StartMultiplayerOnClient", 0, Debug.DebugColor.White, 17592186044416UL);
			MBCommon.CurrentGameType = MBCommon.GameType.MultiClient;
			GameNetwork.ClientPeerIndex = playerIndex;
			GameNetwork.InitializeClientSide(serverAddress, port, sessionKey, playerIndex);
			GameNetwork.StartMultiplayer();
			GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000A6160 File Offset: 0x000A4360
		[MBCallback(null, false)]
		internal static bool HandleNetworkPacketAsClient()
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Network messages should be handled from main thread", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\GameNetwork.cs", "HandleNetworkPacketAsClient", 1204);
			}
			bool flag = true;
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo, ref flag);
			if (flag && num >= 0 && num < GameNetwork._gameNetworkMessageIdsFromServer.Count)
			{
				GameNetworkMessage gameNetworkMessage = Activator.CreateInstance(GameNetwork._gameNetworkMessageIdsFromServer[num]) as GameNetworkMessage;
				gameNetworkMessage.MessageId = num;
				Debug.Print("Reading message: " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
				flag = gameNetworkMessage.Read();
				if (flag)
				{
					if (!NetworkMain.GameClient.IsInGame && !GameNetwork.IsReplay && !NetworkMain.CommunityClient.IsInGame)
					{
						Debug.Print("ignoring post mission message: " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
					}
					else
					{
						bool flag2 = false;
						bool flag3 = true;
						if ((gameNetworkMessage.GetLogFilter() & GameNetwork.MultiplayerLogging) != MultiplayerMessageFilter.None)
						{
							if (GameNetworkMessage.IsClientMissionOver)
							{
								Debug.Print("WARNING: Entering message processing while client mission is over", 0, Debug.DebugColor.White, 17592186044416UL);
							}
							Debug.Print("Processing message: " + gameNetworkMessage.GetType().Name + ": " + gameNetworkMessage.GetLogFormat(), 0, Debug.DebugColor.White, 17179869184UL);
						}
						List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>> list;
						if (GameNetwork._fromServerBaseMessageHandlers.TryGetValue(num, out list))
						{
							foreach (GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> serverMessageHandlerDelegate in list)
							{
								try
								{
									serverMessageHandlerDelegate(gameNetworkMessage);
								}
								catch
								{
									Debug.Print("Exception in handler of " + num.ToString(), 0, Debug.DebugColor.White, 17179869184UL);
									Debug.Print("Exception in handler of " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.Red, 17179869184UL);
								}
							}
							flag3 = false;
							flag2 = list.Count != 0;
						}
						List<object> list2;
						if (GameNetwork._fromServerMessageHandlers.TryGetValue(num, out list2))
						{
							foreach (object obj in list2)
							{
								(obj as Delegate).DynamicInvokeWithLog(new object[] { gameNetworkMessage });
							}
							flag3 = false;
							flag2 = flag2 || list2.Count != 0;
						}
						if (flag3)
						{
							Debug.Print("Invalid messageId " + num.ToString(), 0, Debug.DebugColor.White, 17179869184UL);
							Debug.Print("Invalid messageId " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
						}
						else if (!flag2)
						{
							Debug.Print("No message handler found for " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.Red, 17179869184UL);
						}
					}
				}
				else
				{
					Debug.Print("Invalid message read for: " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
				}
			}
			else
			{
				Debug.Print("Invalid message id read: " + num, 0, Debug.DebugColor.White, 17179869184UL);
			}
			return flag;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000A64A4 File Offset: 0x000A46A4
		private static int GetSessionKeyForPlayer()
		{
			return new Random(DateTime.Now.Millisecond).Next(1, 4001);
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000A64D0 File Offset: 0x000A46D0
		public static NetworkCommunicator HandleNewClientConnect(PlayerConnectionInfo playerConnectionInfo, bool isAdmin)
		{
			NetworkCommunicator networkCommunicator = GameNetwork.AddNewPlayerOnServer(playerConnectionInfo, false, isAdmin) as NetworkCommunicator;
			GameNetwork._handler.OnNewPlayerConnect(playerConnectionInfo, networkCommunicator);
			return networkCommunicator;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000A64F8 File Offset: 0x000A46F8
		public static GameNetwork.AddPlayersResult HandleNewClientsConnect(PlayerConnectionInfo[] playerConnectionInfos, bool isAdmin)
		{
			GameNetwork.AddPlayersResult addPlayersResult = GameNetwork.AddNewPlayersOnServer(playerConnectionInfos, isAdmin);
			if (addPlayersResult.Success)
			{
				for (int i = 0; i < playerConnectionInfos.Length; i++)
				{
					GameNetwork._handler.OnNewPlayerConnect(playerConnectionInfos[i], addPlayersResult.NetworkPeers[i]);
				}
			}
			return addPlayersResult;
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000A653C File Offset: 0x000A473C
		public static void AddNetworkPeerToDisconnectAsServer(NetworkCommunicator networkPeer)
		{
			Debug.Print("adding peer to disconnect index:" + networkPeer.Index, 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.AddPeerToDisconnect(networkPeer);
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new DeletePlayer(networkPeer.Index, false));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000A6594 File Offset: 0x000A4794
		private static void HandleServerEventCreatePlayer(CreatePlayer message)
		{
			int playerIndex = message.PlayerIndex;
			string playerName = message.PlayerName;
			bool isReceiverPeer = message.IsReceiverPeer;
			NetworkCommunicator networkCommunicator;
			if (isReceiverPeer || message.IsNonExistingDisconnectedPeer || message.DisconnectedPeerIndex < 0)
			{
				networkCommunicator = NetworkCommunicator.CreateAsClient(playerName, playerIndex);
			}
			else
			{
				networkCommunicator = GameNetwork.DisconnectedNetworkPeers[message.DisconnectedPeerIndex];
				networkCommunicator.UpdateIndexForReconnectingPlayer(message.PlayerIndex);
				Debug.Print("RemoveFromDisconnectedPeers: " + networkCommunicator.UserName, 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.DisconnectedNetworkPeers.RemoveAt(message.DisconnectedPeerIndex);
			}
			if (isReceiverPeer)
			{
				GameNetwork.MyPeer = networkCommunicator;
			}
			if (message.IsNonExistingDisconnectedPeer)
			{
				GameNetwork.AddToDisconnectedPeers(networkCommunicator);
			}
			else
			{
				GameNetwork.VirtualPlayers[networkCommunicator.VirtualPlayer.Index] = networkCommunicator.VirtualPlayer;
				GameNetwork.AddNetworkPeer(networkCommunicator);
			}
			GameNetwork._handler.OnPlayerConnectedToServer(networkCommunicator);
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000A6664 File Offset: 0x000A4864
		private static void HandleServerEventDeletePlayer(DeletePlayer message)
		{
			NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.FirstOrDefault<NetworkCommunicator>((NetworkCommunicator networkPeer) => networkPeer.Index == message.PlayerIndex);
			if (networkCommunicator != null)
			{
				GameNetwork.HandleRemovePlayerInternal(networkCommunicator, message.AddToDisconnectList);
			}
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000A66A9 File Offset: 0x000A48A9
		public static void InitializeClientSide(string serverAddress, int port, int sessionKey, int playerIndex)
		{
			MBAPI.IMBNetwork.InitializeClientSide(serverAddress, port, sessionKey, playerIndex);
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000A66B9 File Offset: 0x000A48B9
		public static void TerminateClientSide()
		{
			MBAPI.IMBNetwork.TerminateClientSide();
			MBCommon.CurrentGameType = MBCommon.GameType.Single;
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000A66CB File Offset: 0x000A48CB
		public static Type GetSynchedMissionObjectReadableRecordTypeFromIndex(int typeIndex)
		{
			return GameNetwork._synchedMissionObjectClassTypes[typeIndex];
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000A66D8 File Offset: 0x000A48D8
		public static int GetSynchedMissionObjectReadableRecordIndexFromType(Type type)
		{
			for (int i = 0; i < GameNetwork._synchedMissionObjectClassTypes.Count; i++)
			{
				Type type2 = GameNetwork._synchedMissionObjectClassTypes[i];
				DefineSynchedMissionObjectType customAttribute = type2.GetCustomAttribute<DefineSynchedMissionObjectType>();
				DefineSynchedMissionObjectTypeForMod customAttribute2 = type2.GetCustomAttribute<DefineSynchedMissionObjectTypeForMod>();
				Type type3 = ((customAttribute != null) ? customAttribute.Type : null) ?? ((customAttribute2 != null) ? customAttribute2.Type : null);
				Type type4 = type;
				while (type4 != null)
				{
					if (type4 == type3)
					{
						return i;
					}
					type4 = type4.BaseType;
				}
			}
			return -1;
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x000A6754 File Offset: 0x000A4954
		public static void DestroyComponent(UdpNetworkComponent udpNetworkComponent)
		{
			GameNetwork.RemoveNetworkHandler(udpNetworkComponent);
			GameNetwork.NetworkComponents.Remove(udpNetworkComponent);
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000A6768 File Offset: 0x000A4968
		public static T AddNetworkComponent<T>() where T : UdpNetworkComponent
		{
			T t = (T)((object)Activator.CreateInstance(typeof(T), new object[0]));
			GameNetwork.NetworkComponents.Add(t);
			GameNetwork.NetworkHandlers.Add(t);
			return t;
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000A67B1 File Offset: 0x000A49B1
		public static void AddNetworkHandler(IUdpNetworkHandler handler)
		{
			GameNetwork.NetworkHandlers.Add(handler);
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000A67BE File Offset: 0x000A49BE
		public static void RemoveNetworkHandler(IUdpNetworkHandler handler)
		{
			handler.OnUdpNetworkHandlerClose();
			GameNetwork.NetworkHandlers.Remove(handler);
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000A67D4 File Offset: 0x000A49D4
		public static T GetNetworkComponent<T>() where T : UdpNetworkComponent
		{
			using (List<UdpNetworkComponent>.Enumerator enumerator = GameNetwork.NetworkComponents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002B36 RID: 11062 RVA: 0x000A6840 File Offset: 0x000A4A40
		// (set) Token: 0x06002B37 RID: 11063 RVA: 0x000A6847 File Offset: 0x000A4A47
		public static List<UdpNetworkComponent> NetworkComponents { get; private set; }

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x000A684F File Offset: 0x000A4A4F
		// (set) Token: 0x06002B39 RID: 11065 RVA: 0x000A6856 File Offset: 0x000A4A56
		public static List<IUdpNetworkHandler> NetworkHandlers { get; private set; }

		// Token: 0x06002B3A RID: 11066 RVA: 0x000A6860 File Offset: 0x000A4A60
		public static void WriteMessage(GameNetworkMessage message)
		{
			if ((message.GetLogFilter() & GameNetwork.MultiplayerLogging) != MultiplayerMessageFilter.None)
			{
				Debug.Print("Writing message: " + message.GetLogFormat(), 0, Debug.DebugColor.White, 17179869184UL);
			}
			Type type = message.GetType();
			message.MessageId = GameNetwork._gameNetworkMessageTypesAll[type];
			message.Write();
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000A68BC File Offset: 0x000A4ABC
		private static void AddServerMessageHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[typeof(T)];
			GameNetwork._fromServerMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000A68F0 File Offset: 0x000A4AF0
		private static void AddServerBaseMessageHandler(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[messageType];
			GameNetwork._fromServerBaseMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000A691C File Offset: 0x000A4B1C
		private static void AddClientMessageHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[typeof(T)];
			GameNetwork._fromClientMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000A6950 File Offset: 0x000A4B50
		private static void AddClientBaseMessageHandler(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[messageType];
			GameNetwork._fromClientBaseMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000A697C File Offset: 0x000A4B7C
		private static void RemoveServerMessageHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[typeof(T)];
			GameNetwork._fromServerMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000A69B0 File Offset: 0x000A4BB0
		private static void RemoveServerBaseMessageHandler(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[messageType];
			GameNetwork._fromServerBaseMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000A69DC File Offset: 0x000A4BDC
		private static void RemoveClientMessageHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[typeof(T)];
			GameNetwork._fromClientMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000A6A10 File Offset: 0x000A4C10
		internal static void FindGameNetworkMessages()
		{
			Debug.Print("Searching Game NetworkMessages Methods", 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork._fromClientMessageHandlers = new Dictionary<int, List<object>>();
			GameNetwork._fromServerMessageHandlers = new Dictionary<int, List<object>>();
			GameNetwork._fromClientBaseMessageHandlers = new Dictionary<int, List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>>>();
			GameNetwork._fromServerBaseMessageHandlers = new Dictionary<int, List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>>>();
			GameNetwork._gameNetworkMessageTypesAll = new Dictionary<Type, int>();
			GameNetwork._gameNetworkMessageTypesFromClient = new Dictionary<Type, int>();
			GameNetwork._gameNetworkMessageTypesFromServer = new Dictionary<Type, int>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			List<Type> list2 = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (GameNetwork.CheckAssemblyForNetworkMessage(assembly))
				{
					GameNetwork.CollectGameNetworkMessagesFromAssembly(assembly, list, list2);
				}
			}
			list.Sort((Type s1, Type s2) => s1.FullName.CompareTo(s2.FullName));
			list2.Sort((Type s1, Type s2) => s1.FullName.CompareTo(s2.FullName));
			GameNetwork._gameNetworkMessageIdsFromClient = new List<Type>(list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				Type type = list[j];
				GameNetwork._gameNetworkMessageIdsFromClient.Add(type);
				GameNetwork._gameNetworkMessageTypesFromClient.Add(type, j);
				GameNetwork._gameNetworkMessageTypesAll.Add(type, j);
				GameNetwork._fromClientMessageHandlers.Add(j, new List<object>());
				GameNetwork._fromClientBaseMessageHandlers.Add(j, new List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>>());
			}
			GameNetwork._gameNetworkMessageIdsFromServer = new List<Type>(list2.Count);
			for (int k = 0; k < list2.Count; k++)
			{
				Type type2 = list2[k];
				GameNetwork._gameNetworkMessageIdsFromServer.Add(type2);
				GameNetwork._gameNetworkMessageTypesFromServer.Add(type2, k);
				GameNetwork._gameNetworkMessageTypesAll.Add(type2, k);
				GameNetwork._fromServerMessageHandlers.Add(k, new List<object>());
				GameNetwork._fromServerBaseMessageHandlers.Add(k, new List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>>());
			}
			CompressionBasic.NetworkComponentEventTypeFromClientCompressionInfo = new CompressionInfo.Integer(0, list.Count - 1, true);
			CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo = new CompressionInfo.Integer(0, list2.Count - 1, true);
			Debug.Print("Found " + list.Count + " Client Game Network Messages", 0, Debug.DebugColor.White, 17179869184UL);
			Debug.Print("Found " + list2.Count + " Server Game Network Messages", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000A6C70 File Offset: 0x000A4E70
		internal static void FindSynchedMissionObjectTypes()
		{
			Debug.Print("Searching Game SynchedMissionObjects", 0, Debug.DebugColor.White, 17179869184UL);
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			GameNetwork._synchedMissionObjectClassTypes = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (GameNetwork.CheckAssemblyForNetworkMessage(assembly))
				{
					GameNetwork.CollectSynchedMissionObjectTypesFromAssembly(assembly, GameNetwork._synchedMissionObjectClassTypes);
				}
			}
			GameNetwork._synchedMissionObjectClassTypes.Sort((Type s1, Type s2) => s1.FullName.CompareTo(s2.FullName));
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000A6CF8 File Offset: 0x000A4EF8
		private static void RemoveClientBaseMessageHandler(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[messageType];
			GameNetwork._fromClientBaseMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000A6D24 File Offset: 0x000A4F24
		private static bool CheckAssemblyForNetworkMessage(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(GameNetworkMessage));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000A6D79 File Offset: 0x000A4F79
		public static void SetServerBandwidthLimitInMbps(double value)
		{
			MBAPI.IMBNetwork.SetServerBandwidthLimitInMbps(value);
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000A6D86 File Offset: 0x000A4F86
		public static void SetServerTickRate(double value)
		{
			MBAPI.IMBNetwork.SetServerTickRate(value);
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000A6D93 File Offset: 0x000A4F93
		public static void SetServerFrameRate(double value)
		{
			MBAPI.IMBNetwork.SetServerFrameRate(value);
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000A6DA0 File Offset: 0x000A4FA0
		public static void ResetDebugVariables()
		{
			MBAPI.IMBNetwork.ResetDebugVariables();
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000A6DAC File Offset: 0x000A4FAC
		public static void PrintDebugStats()
		{
			MBAPI.IMBNetwork.PrintDebugStats();
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000A6DB8 File Offset: 0x000A4FB8
		public static float GetAveragePacketLossRatio()
		{
			return MBAPI.IMBNetwork.GetAveragePacketLossRatio();
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000A6DC4 File Offset: 0x000A4FC4
		public static void GetDebugUploadsInBits(ref GameNetwork.DebugNetworkPacketStatisticsStruct networkStatisticsStruct, ref GameNetwork.DebugNetworkPositionCompressionStatisticsStruct posStatisticsStruct)
		{
			MBAPI.IMBNetwork.GetDebugUploadsInBits(ref networkStatisticsStruct, ref posStatisticsStruct);
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x000A6DD2 File Offset: 0x000A4FD2
		public static void PrintReplicationTableStatistics()
		{
			MBAPI.IMBNetwork.PrintReplicationTableStatistics();
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000A6DDE File Offset: 0x000A4FDE
		public static void ClearReplicationTableStatistics()
		{
			MBAPI.IMBNetwork.ClearReplicationTableStatistics();
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000A6DEA File Offset: 0x000A4FEA
		public static void ResetDebugUploads()
		{
			MBAPI.IMBNetwork.ResetDebugUploads();
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000A6DF6 File Offset: 0x000A4FF6
		public static void ResetMissionData()
		{
			MBAPI.IMBNetwork.ResetMissionData();
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000A6E02 File Offset: 0x000A5002
		private static void AddPeerToDisconnect(NetworkCommunicator networkPeer)
		{
			MBAPI.IMBNetwork.AddPeerToDisconnect(networkPeer.Index);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000A6E14 File Offset: 0x000A5014
		public static void InitializeCompressionInfos()
		{
			CompressionBasic.ActionCodeCompressionInfo = new CompressionInfo.Integer(ActionIndexCache.act_none.Index, MBAnimation.GetNumActionCodes() - 1, true);
			CompressionBasic.AnimationIndexCompressionInfo = new CompressionInfo.Integer(0, MBAnimation.GetNumAnimations() - 1, true);
			CompressionBasic.CultureIndexCompressionInfo = new CompressionInfo.Integer(-1, MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>().Count - 1, true);
			CompressionBasic.SoundEventsCompressionInfo = new CompressionInfo.Integer(0, SoundEvent.GetTotalEventCount() - 1, true);
			CompressionMission.ActionSetCompressionInfo = new CompressionInfo.Integer(0, MBActionSet.GetNumberOfActionSets() - 1, true);
			CompressionMission.MonsterUsageSetCompressionInfo = new CompressionInfo.Integer(0, MBActionSet.GetNumberOfMonsterUsageSets() - 1, true);
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000A6EA6 File Offset: 0x000A50A6
		[MBCallback(null, false)]
		internal static void SyncRelevantGameOptionsToServer()
		{
			SyncRelevantGameOptionsToServer syncRelevantGameOptionsToServer = new SyncRelevantGameOptionsToServer();
			syncRelevantGameOptionsToServer.InitializeOptions();
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(syncRelevantGameOptionsToServer);
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000A6EC4 File Offset: 0x000A50C4
		private static void CollectGameNetworkMessagesFromAssembly(Assembly assembly, List<Type> gameNetworkMessagesFromClient, List<Type> gameNetworkMessagesFromServer)
		{
			Type typeFromHandle = typeof(GameNetworkMessage);
			bool? flag = null;
			List<Type> typesSafe = assembly.GetTypesSafe(null);
			for (int i = 0; i < typesSafe.Count; i++)
			{
				Type type = typesSafe[i];
				if (typeFromHandle.IsAssignableFrom(type) && type != typeFromHandle && type.IsSealed && !(type.GetConstructor(Type.EmptyTypes) == null))
				{
					DefineGameNetworkMessageType customAttribute = type.GetCustomAttribute<DefineGameNetworkMessageType>();
					if (customAttribute != null)
					{
						if (flag == null || !flag.Value)
						{
							flag = new bool?(false);
							GameNetworkMessageSendType sendType = customAttribute.SendType;
							if (sendType != GameNetworkMessageSendType.FromClient)
							{
								if (sendType - GameNetworkMessageSendType.FromServer <= 1)
								{
									gameNetworkMessagesFromServer.Add(type);
								}
							}
							else
							{
								gameNetworkMessagesFromClient.Add(type);
							}
						}
					}
					else
					{
						DefineGameNetworkMessageTypeForMod customAttribute2 = type.GetCustomAttribute<DefineGameNetworkMessageTypeForMod>();
						if (customAttribute2 != null && (flag == null || flag.Value))
						{
							flag = new bool?(true);
							GameNetworkMessageSendType sendType2 = customAttribute2.SendType;
							if (sendType2 != GameNetworkMessageSendType.FromClient)
							{
								if (sendType2 - GameNetworkMessageSendType.FromServer <= 1)
								{
									gameNetworkMessagesFromServer.Add(type);
								}
							}
							else
							{
								gameNetworkMessagesFromClient.Add(type);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000A6FF4 File Offset: 0x000A51F4
		private static void CollectSynchedMissionObjectTypesFromAssembly(Assembly assembly, List<Type> synchedMissionObjectClassTypes)
		{
			Type typeFromHandle = typeof(ISynchedMissionObjectReadableRecord);
			bool? flag = null;
			List<Type> typesSafe = assembly.GetTypesSafe(null);
			for (int i = 0; i < typesSafe.Count; i++)
			{
				Type type = typesSafe[i];
				if (typeFromHandle.IsAssignableFrom(type) && type != typeFromHandle)
				{
					if (type.GetCustomAttribute<DefineSynchedMissionObjectType>() != null)
					{
						if (flag == null || !flag.Value)
						{
							flag = new bool?(false);
							synchedMissionObjectClassTypes.Add(type);
						}
					}
					else if (type.GetCustomAttribute<DefineSynchedMissionObjectTypeForMod>() != null && (flag == null || flag.Value))
					{
						flag = new bool?(true);
						synchedMissionObjectClassTypes.Add(type);
					}
				}
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002B56 RID: 11094 RVA: 0x000A70B1 File Offset: 0x000A52B1
		// (set) Token: 0x06002B57 RID: 11095 RVA: 0x000A70B8 File Offset: 0x000A52B8
		public static NetworkCommunicator MyPeer { get; private set; }

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000A70C0 File Offset: 0x000A52C0
		public static bool IsMyPeerReady
		{
			get
			{
				return GameNetwork.MyPeer != null && GameNetwork.MyPeer.IsSynchronized;
			}
		}

		// Token: 0x040010C1 RID: 4289
		public const int MaxAutomatedBattleIndex = 10;

		// Token: 0x040010C2 RID: 4290
		public const int MaxPlayerCount = 1023;

		// Token: 0x040010C3 RID: 4291
		private static IGameNetworkHandler _handler;

		// Token: 0x040010C7 RID: 4295
		public static int ClientPeerIndex;

		// Token: 0x040010C8 RID: 4296
		private static MultiplayerMessageFilter MultiplayerLogging = (MultiplayerMessageFilter)(-1);

		// Token: 0x040010CB RID: 4299
		private static Dictionary<Type, int> _gameNetworkMessageTypesAll;

		// Token: 0x040010CC RID: 4300
		private static Dictionary<Type, int> _gameNetworkMessageTypesFromClient;

		// Token: 0x040010CD RID: 4301
		private static List<Type> _gameNetworkMessageIdsFromClient;

		// Token: 0x040010CE RID: 4302
		private static Dictionary<Type, int> _gameNetworkMessageTypesFromServer;

		// Token: 0x040010CF RID: 4303
		private static List<Type> _gameNetworkMessageIdsFromServer;

		// Token: 0x040010D0 RID: 4304
		private static Dictionary<int, List<object>> _fromClientMessageHandlers;

		// Token: 0x040010D1 RID: 4305
		private static Dictionary<int, List<object>> _fromServerMessageHandlers;

		// Token: 0x040010D2 RID: 4306
		private static Dictionary<int, List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>>> _fromClientBaseMessageHandlers;

		// Token: 0x040010D3 RID: 4307
		private static Dictionary<int, List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>>> _fromServerBaseMessageHandlers;

		// Token: 0x040010D4 RID: 4308
		private static List<Type> _synchedMissionObjectClassTypes;

		// Token: 0x020005CB RID: 1483
		public class NetworkMessageHandlerRegisterer
		{
			// Token: 0x06003E72 RID: 15986 RVA: 0x000F5222 File Offset: 0x000F3422
			public NetworkMessageHandlerRegisterer(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode definitionMode)
			{
				this._registerMode = definitionMode;
			}

			// Token: 0x06003E73 RID: 15987 RVA: 0x000F5231 File Offset: 0x000F3431
			public void Register<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddServerMessageHandler<T>(handler);
					return;
				}
				GameNetwork.RemoveServerMessageHandler<T>(handler);
			}

			// Token: 0x06003E74 RID: 15988 RVA: 0x000F5248 File Offset: 0x000F3448
			public void RegisterBaseHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddServerBaseMessageHandler(handler, typeof(T));
					return;
				}
				GameNetwork.RemoveServerBaseMessageHandler(handler, typeof(T));
			}

			// Token: 0x06003E75 RID: 15989 RVA: 0x000F5273 File Offset: 0x000F3473
			public void Register<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddClientMessageHandler<T>(handler);
					return;
				}
				GameNetwork.RemoveClientMessageHandler<T>(handler);
			}

			// Token: 0x06003E76 RID: 15990 RVA: 0x000F528A File Offset: 0x000F348A
			public void RegisterBaseHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddClientBaseMessageHandler(handler, typeof(T));
					return;
				}
				GameNetwork.RemoveClientBaseMessageHandler(handler, typeof(T));
			}

			// Token: 0x04001F46 RID: 8006
			private readonly GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode _registerMode;

			// Token: 0x020006C2 RID: 1730
			public enum RegisterMode
			{
				// Token: 0x0400234A RID: 9034
				Add,
				// Token: 0x0400234B RID: 9035
				Remove
			}
		}

		// Token: 0x020005CC RID: 1484
		public class NetworkMessageHandlerRegistererContainer
		{
			// Token: 0x06003E77 RID: 15991 RVA: 0x000F52B5 File Offset: 0x000F34B5
			public NetworkMessageHandlerRegistererContainer()
			{
				this._fromClientHandlers = new List<Delegate>();
				this._fromServerHandlers = new List<Delegate>();
				this._fromServerBaseHandlers = new List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>>();
				this._fromClientBaseHandlers = new List<Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type>>();
			}

			// Token: 0x06003E78 RID: 15992 RVA: 0x000F52E9 File Offset: 0x000F34E9
			public void RegisterBaseHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler) where T : GameNetworkMessage
			{
				this._fromServerBaseHandlers.Add(new Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>(handler, typeof(T)));
			}

			// Token: 0x06003E79 RID: 15993 RVA: 0x000F5306 File Offset: 0x000F3506
			public void Register<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				this._fromServerHandlers.Add(handler);
			}

			// Token: 0x06003E7A RID: 15994 RVA: 0x000F5314 File Offset: 0x000F3514
			public void RegisterBaseHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler)
			{
				this._fromClientBaseHandlers.Add(new Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type>(handler, typeof(T)));
			}

			// Token: 0x06003E7B RID: 15995 RVA: 0x000F5331 File Offset: 0x000F3531
			public void Register<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				this._fromClientHandlers.Add(handler);
			}

			// Token: 0x06003E7C RID: 15996 RVA: 0x000F5340 File Offset: 0x000F3540
			public void RegisterMessages()
			{
				if (this._fromServerHandlers.Count > 0 || this._fromServerBaseHandlers.Count > 0)
				{
					foreach (Delegate @delegate in this._fromServerHandlers)
					{
						Type type = @delegate.GetType().GenericTypeArguments[0];
						int num = GameNetwork._gameNetworkMessageTypesFromServer[type];
						GameNetwork._fromServerMessageHandlers[num].Add(@delegate);
					}
					using (List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>>.Enumerator enumerator2 = this._fromServerBaseHandlers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type> tuple = enumerator2.Current;
							int num2 = GameNetwork._gameNetworkMessageTypesFromServer[tuple.Item2];
							GameNetwork._fromServerBaseMessageHandlers[num2].Add(tuple.Item1);
						}
						return;
					}
				}
				foreach (Delegate delegate2 in this._fromClientHandlers)
				{
					Type type2 = delegate2.GetType().GenericTypeArguments[0];
					int num3 = GameNetwork._gameNetworkMessageTypesFromClient[type2];
					GameNetwork._fromClientMessageHandlers[num3].Add(delegate2);
				}
				foreach (Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type> tuple2 in this._fromClientBaseHandlers)
				{
					int num4 = GameNetwork._gameNetworkMessageTypesFromClient[tuple2.Item2];
					GameNetwork._fromClientBaseMessageHandlers[num4].Add(tuple2.Item1);
				}
			}

			// Token: 0x06003E7D RID: 15997 RVA: 0x000F5518 File Offset: 0x000F3718
			public void UnregisterMessages()
			{
				if (this._fromServerHandlers.Count > 0 || this._fromServerBaseHandlers.Count > 0)
				{
					foreach (Delegate @delegate in this._fromServerHandlers)
					{
						Type type = @delegate.GetType().GenericTypeArguments[0];
						int num = GameNetwork._gameNetworkMessageTypesFromServer[type];
						GameNetwork._fromServerMessageHandlers[num].Remove(@delegate);
					}
					using (List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>>.Enumerator enumerator2 = this._fromServerBaseHandlers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type> tuple = enumerator2.Current;
							int num2 = GameNetwork._gameNetworkMessageTypesFromServer[tuple.Item2];
							GameNetwork._fromServerBaseMessageHandlers[num2].Remove(tuple.Item1);
						}
						return;
					}
				}
				foreach (Delegate delegate2 in this._fromClientHandlers)
				{
					Type type2 = delegate2.GetType().GenericTypeArguments[0];
					int num3 = GameNetwork._gameNetworkMessageTypesFromClient[type2];
					GameNetwork._fromClientMessageHandlers[num3].Remove(delegate2);
				}
				foreach (Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type> tuple2 in this._fromClientBaseHandlers)
				{
					int num4 = GameNetwork._gameNetworkMessageTypesFromClient[tuple2.Item2];
					GameNetwork._fromClientBaseMessageHandlers[num4].Remove(tuple2.Item1);
				}
			}

			// Token: 0x04001F47 RID: 8007
			private List<Delegate> _fromClientHandlers;

			// Token: 0x04001F48 RID: 8008
			private List<Delegate> _fromServerHandlers;

			// Token: 0x04001F49 RID: 8009
			private List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>> _fromServerBaseHandlers;

			// Token: 0x04001F4A RID: 8010
			private List<Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type>> _fromClientBaseHandlers;
		}

		// Token: 0x020005CD RID: 1485
		[Flags]
		public enum EventBroadcastFlags
		{
			// Token: 0x04001F4C RID: 8012
			None = 0,
			// Token: 0x04001F4D RID: 8013
			ExcludeTargetPlayer = 1,
			// Token: 0x04001F4E RID: 8014
			ExcludeNoBloodStainsOption = 2,
			// Token: 0x04001F4F RID: 8015
			ExcludeNoParticlesOption = 4,
			// Token: 0x04001F50 RID: 8016
			ExcludeNoSoundOption = 8,
			// Token: 0x04001F51 RID: 8017
			AddToMissionRecord = 16,
			// Token: 0x04001F52 RID: 8018
			IncludeUnsynchronizedClients = 32,
			// Token: 0x04001F53 RID: 8019
			ExcludeOtherTeamPlayers = 64,
			// Token: 0x04001F54 RID: 8020
			ExcludePeerTeamPlayers = 128,
			// Token: 0x04001F55 RID: 8021
			DontSendToPeers = 256
		}

		// Token: 0x020005CE RID: 1486
		[EngineStruct("Debug_network_position_compression_statistics_struct", false, null)]
		public struct DebugNetworkPositionCompressionStatisticsStruct
		{
			// Token: 0x04001F56 RID: 8022
			public int totalPositionUpload;

			// Token: 0x04001F57 RID: 8023
			public int totalPositionPrecisionBitCount;

			// Token: 0x04001F58 RID: 8024
			public int totalPositionCoarseBitCountX;

			// Token: 0x04001F59 RID: 8025
			public int totalPositionCoarseBitCountY;

			// Token: 0x04001F5A RID: 8026
			public int totalPositionCoarseBitCountZ;
		}

		// Token: 0x020005CF RID: 1487
		[EngineStruct("Debug_network_packet_statistics_struct", false, null)]
		public struct DebugNetworkPacketStatisticsStruct
		{
			// Token: 0x04001F5B RID: 8027
			public int TotalPackets;

			// Token: 0x04001F5C RID: 8028
			public int TotalUpload;

			// Token: 0x04001F5D RID: 8029
			public int TotalConstantsUpload;

			// Token: 0x04001F5E RID: 8030
			public int TotalReliableEventUpload;

			// Token: 0x04001F5F RID: 8031
			public int TotalReplicationUpload;

			// Token: 0x04001F60 RID: 8032
			public int TotalUnreliableEventUpload;

			// Token: 0x04001F61 RID: 8033
			public int TotalReplicationTableAdderCount;

			// Token: 0x04001F62 RID: 8034
			public int TotalReplicationTableAdderBitCount;

			// Token: 0x04001F63 RID: 8035
			public int TotalReplicationTableAdder;

			// Token: 0x04001F64 RID: 8036
			public double TotalCellPriority;

			// Token: 0x04001F65 RID: 8037
			public double TotalCellAgentPriority;

			// Token: 0x04001F66 RID: 8038
			public double TotalCellCellPriority;

			// Token: 0x04001F67 RID: 8039
			public int TotalCellPriorityChecks;

			// Token: 0x04001F68 RID: 8040
			public int TotalSentCellCount;

			// Token: 0x04001F69 RID: 8041
			public int TotalNotSentCellCount;

			// Token: 0x04001F6A RID: 8042
			public int TotalReplicationWriteCount;

			// Token: 0x04001F6B RID: 8043
			public int CurMaxPacketSizeInBytes;

			// Token: 0x04001F6C RID: 8044
			public double AveragePingTime;

			// Token: 0x04001F6D RID: 8045
			public double AverageDtToSendPacket;

			// Token: 0x04001F6E RID: 8046
			public double TimeOutPeriod;

			// Token: 0x04001F6F RID: 8047
			public double PacingRate;

			// Token: 0x04001F70 RID: 8048
			public double DeliveryRate;

			// Token: 0x04001F71 RID: 8049
			public double RoundTripTime;

			// Token: 0x04001F72 RID: 8050
			public int InflightBitCount;

			// Token: 0x04001F73 RID: 8051
			public int IsCongested;

			// Token: 0x04001F74 RID: 8052
			public int ProbeBwPhaseIndex;

			// Token: 0x04001F75 RID: 8053
			public double LostPercent;

			// Token: 0x04001F76 RID: 8054
			public int LostCount;

			// Token: 0x04001F77 RID: 8055
			public int TotalCountOnLostCheck;
		}

		// Token: 0x020005D0 RID: 1488
		public struct AddPlayersResult
		{
			// Token: 0x04001F78 RID: 8056
			public bool Success;

			// Token: 0x04001F79 RID: 8057
			public NetworkCommunicator[] NetworkPeers;
		}
	}
}
