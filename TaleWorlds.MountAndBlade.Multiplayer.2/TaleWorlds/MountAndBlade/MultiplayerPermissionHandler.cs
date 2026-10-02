using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000024 RID: 36
	public class MultiplayerPermissionHandler : UdpNetworkComponent
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001B9 RID: 441 RVA: 0x00007D8C File Offset: 0x00005F8C
		// (remove) Token: 0x060001BA RID: 442 RVA: 0x00007DC4 File Offset: 0x00005FC4
		public event Action<PlayerId, bool> OnPlayerPlatformMuteChanged;

		// Token: 0x060001BB RID: 443 RVA: 0x00007DF9 File Offset: 0x00005FF9
		public MultiplayerPermissionHandler()
		{
			this._chatBox = Game.Current.GetGameHandler<ChatBox>();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007E1C File Offset: 0x0000601C
		public override void OnUdpNetworkHandlerClose()
		{
			base.OnUdpNetworkHandlerClose();
			this.HandleClientDisconnect();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007E2C File Offset: 0x0000602C
		private void HandleClientDisconnect()
		{
			foreach (ValueTuple<PlayerId, Permission> valueTuple in this._registeredEvents.Keys)
			{
				PlatformServices.Instance.UnregisterPermissionChangeEvent(valueTuple.Item1, valueTuple.Item2, new PermissionChanged(this.VoicePermissionChanged));
				bool flag;
				this._registeredEvents.TryRemove(new ValueTuple<PlayerId, Permission>(valueTuple.Item1, valueTuple.Item2), out flag);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007EBC File Offset: 0x000060BC
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<InitializeLobbyPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventInitializeLobbyPeer));
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00007ED8 File Offset: 0x000060D8
		private void HandleServerEventInitializeLobbyPeer(GameNetworkMessage baseMessage)
		{
			InitializeLobbyPeer initializeLobbyPeer = (InitializeLobbyPeer)baseMessage;
			if (GameNetwork.MyPeer != null && initializeLobbyPeer.Peer != GameNetwork.MyPeer)
			{
				if (PlatformServices.Instance.RegisterPermissionChangeEvent(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingText, new PermissionChanged(this.TextPermissionChanged)))
				{
					this._registeredEvents[new ValueTuple<PlayerId, Permission>(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingText)] = true;
				}
				if (PlatformServices.Instance.RegisterPermissionChangeEvent(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingVoice, new PermissionChanged(this.VoicePermissionChanged)))
				{
					this._registeredEvents[new ValueTuple<PlayerId, Permission>(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingVoice)] = true;
				}
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007F70 File Offset: 0x00006170
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			base.OnPlayerDisconnectedFromServer(networkPeer);
			if (PlatformServices.Instance.UnregisterPermissionChangeEvent(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingText, new PermissionChanged(this.TextPermissionChanged)))
			{
				bool flag;
				this._registeredEvents.TryRemove(new ValueTuple<PlayerId, Permission>(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingText), out flag);
			}
			if (PlatformServices.Instance.UnregisterPermissionChangeEvent(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingVoice, new PermissionChanged(this.VoicePermissionChanged)))
			{
				bool flag;
				this._registeredEvents.TryRemove(new ValueTuple<PlayerId, Permission>(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingVoice), out flag);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000800C File Offset: 0x0000620C
		private void TextPermissionChanged(PlayerId targetPlayerId, Permission permission, bool hasPermission)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (!(targetPlayerId != networkCommunicator.VirtualPlayer.Id))
				{
					networkCommunicator.GetComponent<MissionPeer>();
					bool flag = !hasPermission;
					this._chatBox.SetPlayerMutedFromPlatform(targetPlayerId, flag);
					Action<PlayerId, bool> onPlayerPlatformMuteChanged = this.OnPlayerPlatformMuteChanged;
					if (onPlayerPlatformMuteChanged != null)
					{
						onPlayerPlatformMuteChanged(targetPlayerId, flag);
					}
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008098 File Offset: 0x00006298
		private void VoicePermissionChanged(PlayerId targetPlayerId, Permission permission, bool hasPermission)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (!(targetPlayerId != networkCommunicator.VirtualPlayer.Id))
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					bool flag = !hasPermission;
					component.SetMutedFromPlatform(flag);
					Action<PlayerId, bool> onPlayerPlatformMuteChanged = this.OnPlayerPlatformMuteChanged;
					if (onPlayerPlatformMuteChanged != null)
					{
						onPlayerPlatformMuteChanged(targetPlayerId, flag);
					}
				}
			}
		}

		// Token: 0x04000067 RID: 103
		private ChatBox _chatBox;

		// Token: 0x04000069 RID: 105
		[TupleElementNames(new string[] { "PlayerId", "Permission" })]
		private ConcurrentDictionary<ValueTuple<PlayerId, Permission>, bool> _registeredEvents = new ConcurrentDictionary<ValueTuple<PlayerId, Permission>, bool>();
	}
}
