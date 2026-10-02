using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F2 RID: 754
	public interface IGameNetworkHandler
	{
		// Token: 0x06002B5A RID: 11098
		void OnNewPlayerConnect(PlayerConnectionInfo playerConnectionInfo, NetworkCommunicator networkPeer);

		// Token: 0x06002B5B RID: 11099
		void OnInitialize();

		// Token: 0x06002B5C RID: 11100
		void OnPlayerConnectedToServer(NetworkCommunicator peer);

		// Token: 0x06002B5D RID: 11101
		void OnPlayerDisconnectedFromServer(NetworkCommunicator peer);

		// Token: 0x06002B5E RID: 11102
		void OnDisconnectedFromServer();

		// Token: 0x06002B5F RID: 11103
		void OnStartMultiplayer();

		// Token: 0x06002B60 RID: 11104
		void OnStartReplay();

		// Token: 0x06002B61 RID: 11105
		void OnEndMultiplayer();

		// Token: 0x06002B62 RID: 11106
		void OnEndReplay();

		// Token: 0x06002B63 RID: 11107
		void OnHandleConsoleCommand(string command);
	}
}
