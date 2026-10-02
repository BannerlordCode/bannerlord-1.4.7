using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200000A RID: 10
	public abstract class LobbyGameState : GameState, IUdpNetworkHandler
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003831 File Offset: 0x00001A31
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000383C File Offset: 0x00001A3C
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.StartMultiplayer();
			GameNetwork.AddNetworkHandler(this);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003850 File Offset: 0x00001A50
		protected override void OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003858 File Offset: 0x00001A58
		protected override void OnFinalize()
		{
			base.OnFinalize();
			GameNetwork.RemoveNetworkHandler(this);
			GameNetwork.EndMultiplayer();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000386B File Offset: 0x00001A6B
		void IUdpNetworkHandler.OnUdpNetworkHandlerClose()
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000386D File Offset: 0x00001A6D
		void IUdpNetworkHandler.OnUdpNetworkHandlerTick(float dt)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000386F File Offset: 0x00001A6F
		void IUdpNetworkHandler.HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003871 File Offset: 0x00001A71
		void IUdpNetworkHandler.HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003873 File Offset: 0x00001A73
		void IUdpNetworkHandler.HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003875 File Offset: 0x00001A75
		void IUdpNetworkHandler.HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003877 File Offset: 0x00001A77
		void IUdpNetworkHandler.HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003879 File Offset: 0x00001A79
		void IUdpNetworkHandler.HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000387B File Offset: 0x00001A7B
		void IUdpNetworkHandler.HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000387D File Offset: 0x00001A7D
		void IUdpNetworkHandler.HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000387F File Offset: 0x00001A7F
		void IUdpNetworkHandler.OnEveryoneUnSynchronized()
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003881 File Offset: 0x00001A81
		void IUdpNetworkHandler.OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003883 File Offset: 0x00001A83
		void IUdpNetworkHandler.OnDisconnectedFromServer()
		{
			this.OnDisconnectedFromServer();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000388B File Offset: 0x00001A8B
		protected virtual void OnDisconnectedFromServer()
		{
		}

		// Token: 0x06000080 RID: 128
		protected abstract void StartMultiplayer();
	}
}
