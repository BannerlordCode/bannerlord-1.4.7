using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029D RID: 669
	public abstract class MissionNetwork : MissionLogic, IUdpNetworkHandler
	{
		// Token: 0x060024E7 RID: 9447 RVA: 0x00086994 File Offset: 0x00084B94
		public override void OnAfterMissionCreated()
		{
			this._missionNetworkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegistererContainer();
			this.AddRemoveMessageHandlers(this._missionNetworkMessageHandlerRegisterer);
			this._missionNetworkMessageHandlerRegisterer.RegisterMessages();
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000869B8 File Offset: 0x00084BB8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			GameNetwork.AddNetworkHandler(this);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000869C6 File Offset: 0x00084BC6
		public override void OnRemoveBehavior()
		{
			GameNetwork.RemoveNetworkHandler(this);
			base.OnRemoveBehavior();
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000869D4 File Offset: 0x00084BD4
		protected virtual void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000869D6 File Offset: 0x00084BD6
		public virtual void OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000869D8 File Offset: 0x00084BD8
		public virtual void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000869DA File Offset: 0x00084BDA
		void IUdpNetworkHandler.OnUdpNetworkHandlerTick(float dt)
		{
			this.OnUdpNetworkHandlerTick();
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000869E2 File Offset: 0x00084BE2
		void IUdpNetworkHandler.OnUdpNetworkHandlerClose()
		{
			this.OnUdpNetworkHandlerClose();
			GameNetwork.NetworkMessageHandlerRegistererContainer missionNetworkMessageHandlerRegisterer = this._missionNetworkMessageHandlerRegisterer;
			if (missionNetworkMessageHandlerRegisterer == null)
			{
				return;
			}
			missionNetworkMessageHandlerRegisterer.UnregisterMessages();
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000869FA File Offset: 0x00084BFA
		void IUdpNetworkHandler.HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
			this.HandleNewClientConnect(clientConnectionInfo);
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x00086A03 File Offset: 0x00084C03
		void IUdpNetworkHandler.HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			this.HandleEarlyNewClientAfterLoadingFinished(networkPeer);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00086A0C File Offset: 0x00084C0C
		void IUdpNetworkHandler.HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			this.HandleNewClientAfterLoadingFinished(networkPeer);
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x00086A15 File Offset: 0x00084C15
		void IUdpNetworkHandler.HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			this.HandleLateNewClientAfterLoadingFinished(networkPeer);
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00086A1E File Offset: 0x00084C1E
		void IUdpNetworkHandler.HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			this.HandleNewClientAfterSynchronized(networkPeer);
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00086A27 File Offset: 0x00084C27
		void IUdpNetworkHandler.HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			this.HandleLateNewClientAfterSynchronized(networkPeer);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x00086A30 File Offset: 0x00084C30
		void IUdpNetworkHandler.HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
			this.HandleEarlyPlayerDisconnect(networkPeer);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00086A39 File Offset: 0x00084C39
		void IUdpNetworkHandler.HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			this.HandlePlayerDisconnect(networkPeer);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00086A42 File Offset: 0x00084C42
		void IUdpNetworkHandler.OnEveryoneUnSynchronized()
		{
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00086A44 File Offset: 0x00084C44
		void IUdpNetworkHandler.OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00086A46 File Offset: 0x00084C46
		void IUdpNetworkHandler.OnDisconnectedFromServer()
		{
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x00086A48 File Offset: 0x00084C48
		protected virtual void OnUdpNetworkHandlerTick()
		{
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x00086A4A File Offset: 0x00084C4A
		protected virtual void OnUdpNetworkHandlerClose()
		{
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x00086A4C File Offset: 0x00084C4C
		protected virtual void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x00086A4E File Offset: 0x00084C4E
		protected virtual void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00086A50 File Offset: 0x00084C50
		protected virtual void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x00086A52 File Offset: 0x00084C52
		protected virtual void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x00086A54 File Offset: 0x00084C54
		protected virtual void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x00086A56 File Offset: 0x00084C56
		protected virtual void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x00086A58 File Offset: 0x00084C58
		protected virtual void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00086A5A File Offset: 0x00084C5A
		protected virtual void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x04000E53 RID: 3667
		private GameNetwork.NetworkMessageHandlerRegistererContainer _missionNetworkMessageHandlerRegisterer;
	}
}
