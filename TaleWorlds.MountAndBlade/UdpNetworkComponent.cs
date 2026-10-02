using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000324 RID: 804
	public abstract class UdpNetworkComponent : IUdpNetworkHandler
	{
		// Token: 0x06002D9C RID: 11676 RVA: 0x000B00B4 File Offset: 0x000AE2B4
		protected UdpNetworkComponent()
		{
			this._missionNetworkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegistererContainer();
			this.AddRemoveMessageHandlers(this._missionNetworkMessageHandlerRegisterer);
			this._missionNetworkMessageHandlerRegisterer.RegisterMessages();
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000B00DE File Offset: 0x000AE2DE
		protected virtual void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000B00E0 File Offset: 0x000AE2E0
		public virtual void OnUdpNetworkHandlerClose()
		{
			GameNetwork.NetworkMessageHandlerRegistererContainer missionNetworkMessageHandlerRegisterer = this._missionNetworkMessageHandlerRegisterer;
			if (missionNetworkMessageHandlerRegisterer != null)
			{
				missionNetworkMessageHandlerRegisterer.UnregisterMessages();
			}
			GameNetwork.NetworkComponents.Remove(this);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000B00FF File Offset: 0x000AE2FF
		public virtual void OnUdpNetworkHandlerTick(float dt)
		{
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000B0101 File Offset: 0x000AE301
		public virtual void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000B0103 File Offset: 0x000AE303
		public virtual void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000B0105 File Offset: 0x000AE305
		public virtual void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000B0107 File Offset: 0x000AE307
		public virtual void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000B0109 File Offset: 0x000AE309
		public virtual void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000B010B File Offset: 0x000AE30B
		public virtual void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000B010D File Offset: 0x000AE30D
		public virtual void OnEveryoneUnSynchronized()
		{
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000B010F File Offset: 0x000AE30F
		public void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000B0111 File Offset: 0x000AE311
		public virtual void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000B0113 File Offset: 0x000AE313
		public virtual void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000B0115 File Offset: 0x000AE315
		public virtual void OnDisconnectedFromServer()
		{
		}

		// Token: 0x040011FD RID: 4605
		private GameNetwork.NetworkMessageHandlerRegistererContainer _missionNetworkMessageHandlerRegisterer;
	}
}
