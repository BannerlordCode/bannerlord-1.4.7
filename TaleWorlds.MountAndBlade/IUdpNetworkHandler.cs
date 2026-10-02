using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000325 RID: 805
	public interface IUdpNetworkHandler
	{
		// Token: 0x06002DAB RID: 11691
		void OnUdpNetworkHandlerClose();

		// Token: 0x06002DAC RID: 11692
		void OnUdpNetworkHandlerTick(float dt);

		// Token: 0x06002DAD RID: 11693
		void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo);

		// Token: 0x06002DAE RID: 11694
		void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer);

		// Token: 0x06002DAF RID: 11695
		void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer);

		// Token: 0x06002DB0 RID: 11696
		void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer);

		// Token: 0x06002DB1 RID: 11697
		void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer);

		// Token: 0x06002DB2 RID: 11698
		void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer);

		// Token: 0x06002DB3 RID: 11699
		void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer);

		// Token: 0x06002DB4 RID: 11700
		void HandlePlayerDisconnect(NetworkCommunicator networkPeer);

		// Token: 0x06002DB5 RID: 11701
		void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer);

		// Token: 0x06002DB6 RID: 11702
		void OnDisconnectedFromServer();

		// Token: 0x06002DB7 RID: 11703
		void OnEveryoneUnSynchronized();
	}
}
