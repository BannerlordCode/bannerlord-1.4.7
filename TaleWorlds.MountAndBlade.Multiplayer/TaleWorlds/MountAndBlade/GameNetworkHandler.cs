using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Multiplayer.NetworkComponents;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000008 RID: 8
	public class GameNetworkHandler : IGameNetworkHandler
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002BF1 File Offset: 0x00000DF1
		void IGameNetworkHandler.OnNewPlayerConnect(PlayerConnectionInfo playerConnectionInfo, NetworkCommunicator networkPeer)
		{
			if (networkPeer != null)
			{
				GameManagerBase.Current.OnPlayerConnect(networkPeer.VirtualPlayer);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C06 File Offset: 0x00000E06
		void IGameNetworkHandler.OnInitialize()
		{
			MultiplayerGameTypes.Initialize();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C10 File Offset: 0x00000E10
		void IGameNetworkHandler.OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
		{
			if (Mission.Current != null)
			{
				using (List<MissionBehavior>.Enumerator enumerator = Mission.Current.MissionBehaviors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionNetwork missionNetwork;
						if ((missionNetwork = enumerator.Current as MissionNetwork) != null)
						{
							missionNetwork.OnPlayerConnectedToServer(networkPeer);
						}
					}
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C78 File Offset: 0x00000E78
		void IGameNetworkHandler.OnDisconnectedFromServer()
		{
			if (Mission.Current != null)
			{
				BannerlordNetwork.EndMultiplayerLobbyMission();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002C88 File Offset: 0x00000E88
		void IGameNetworkHandler.OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			GameManagerBase.Current.OnPlayerDisconnect(networkPeer.VirtualPlayer);
			if (Mission.Current != null)
			{
				using (List<MissionBehavior>.Enumerator enumerator = Mission.Current.MissionBehaviors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionNetwork missionNetwork;
						if ((missionNetwork = enumerator.Current as MissionNetwork) != null)
						{
							missionNetwork.OnPlayerDisconnectedFromServer(networkPeer);
						}
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002D00 File Offset: 0x00000F00
		void IGameNetworkHandler.OnStartMultiplayer()
		{
			GameNetwork.AddNetworkComponent<BaseNetworkComponentData>();
			GameNetwork.AddNetworkComponent<BaseNetworkComponent>();
			GameNetwork.AddNetworkComponent<LobbyNetworkComponent>();
			GameNetwork.AddNetworkComponent<MultiplayerPermissionHandler>();
			GameNetwork.AddNetworkComponent<NetworkStatusReplicationComponent>();
			GameManagerBase.Current.OnGameNetworkBegin();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D2A File Offset: 0x00000F2A
		void IGameNetworkHandler.OnEndMultiplayer()
		{
			GameManagerBase.Current.OnGameNetworkEnd();
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<LobbyNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<NetworkStatusReplicationComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<MultiplayerPermissionHandler>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponentData>());
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D68 File Offset: 0x00000F68
		void IGameNetworkHandler.OnStartReplay()
		{
			GameNetwork.AddNetworkComponent<BaseNetworkComponentData>();
			GameNetwork.AddNetworkComponent<BaseNetworkComponent>();
			GameNetwork.AddNetworkComponent<LobbyNetworkComponent>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D7C File Offset: 0x00000F7C
		void IGameNetworkHandler.OnEndReplay()
		{
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<LobbyNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponent>());
			GameNetwork.DestroyComponent(GameNetwork.GetNetworkComponent<BaseNetworkComponentData>());
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D9C File Offset: 0x00000F9C
		void IGameNetworkHandler.OnHandleConsoleCommand(string command)
		{
			DedicatedServerConsoleCommandManager.HandleConsoleCommand(command);
		}
	}
}
