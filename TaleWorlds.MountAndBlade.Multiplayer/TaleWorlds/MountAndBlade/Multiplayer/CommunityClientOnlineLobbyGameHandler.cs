using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000051 RID: 81
	public class CommunityClientOnlineLobbyGameHandler : ICommunityClientHandler
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public LobbyState LobbyState { get; private set; }

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B8F5 File Offset: 0x00009AF5
		public CommunityClientOnlineLobbyGameHandler(LobbyState lobbyState)
		{
			this.LobbyState = lobbyState;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B904 File Offset: 0x00009B04
		void ICommunityClientHandler.OnQuitFromGame()
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

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B954 File Offset: 0x00009B54
		void ICommunityClientHandler.OnJoinCustomGameResponse(string address, int port, PlayerJoinGameResponseDataFromHost response)
		{
			if (Game.Current != null)
			{
				GameStateManager gameStateManager = Game.Current.GameStateManager;
				if (response != null)
				{
					LobbyGameStateCommunityClient lobbyGameStateCommunityClient = Game.Current.GameStateManager.CreateState<LobbyGameStateCommunityClient>();
					lobbyGameStateCommunityClient.SetStartingParameters(NetworkMain.CommunityClient, address, port, response.PeerIndex, response.SessionKey);
					Game.Current.GameStateManager.PushState(lobbyGameStateCommunityClient, 0);
					Debug.Print("Join game successful", 0, Debug.DebugColor.Green, 17592186044416UL);
				}
			}
		}
	}
}
