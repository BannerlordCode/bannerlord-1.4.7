using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030B RID: 779
	public static class PeerExtensions
	{
		// Token: 0x06002C90 RID: 11408 RVA: 0x000AB88D File Offset: 0x000A9A8D
		public static void SendExistingObjects(this NetworkCommunicator peer, Mission mission)
		{
			MBAPI.IMBPeer.SendExistingObjects(peer.Index, mission.Pointer);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000AB8A5 File Offset: 0x000A9AA5
		public static VirtualPlayer GetPeer(this PeerComponent peerComponent)
		{
			return peerComponent.Peer;
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000AB8AD File Offset: 0x000A9AAD
		public static NetworkCommunicator GetNetworkPeer(this PeerComponent peerComponent)
		{
			return peerComponent.Peer.Communicator as NetworkCommunicator;
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000AB8BF File Offset: 0x000A9ABF
		public static T GetComponent<T>(this NetworkCommunicator networkPeer) where T : PeerComponent
		{
			return networkPeer.VirtualPlayer.GetComponent<T>();
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000AB8CC File Offset: 0x000A9ACC
		public static void RemoveComponent<T>(this NetworkCommunicator networkPeer, bool synched = true) where T : PeerComponent
		{
			networkPeer.VirtualPlayer.RemoveComponent<T>(true);
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000AB8DA File Offset: 0x000A9ADA
		public static void RemoveComponent(this NetworkCommunicator networkPeer, PeerComponent component)
		{
			networkPeer.VirtualPlayer.RemoveComponent(component);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000AB8E8 File Offset: 0x000A9AE8
		public static PeerComponent GetComponent(this NetworkCommunicator networkPeer, uint componentId)
		{
			return networkPeer.VirtualPlayer.GetComponent(componentId);
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000AB8F6 File Offset: 0x000A9AF6
		public static void AddComponent(this NetworkCommunicator networkPeer, Type peerComponentType)
		{
			networkPeer.VirtualPlayer.AddComponent(peerComponentType);
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000AB905 File Offset: 0x000A9B05
		public static void AddComponent(this NetworkCommunicator networkPeer, uint componentId)
		{
			networkPeer.VirtualPlayer.AddComponent(componentId);
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000AB914 File Offset: 0x000A9B14
		public static T AddComponent<T>(this NetworkCommunicator networkPeer) where T : PeerComponent, new()
		{
			if (networkPeer.GetComponent<T>() != null)
			{
				return networkPeer.TellClientToAddComponent<T>();
			}
			return networkPeer.VirtualPlayer.AddComponent<T>();
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000AB938 File Offset: 0x000A9B38
		public static T TellClientToAddComponent<T>(this NetworkCommunicator networkPeer) where T : PeerComponent, new()
		{
			T component = networkPeer.GetComponent<T>();
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new AddPeerComponent(networkPeer, component.TypeId));
			GameNetwork.EndModuleEventAsServer();
			return component;
		}
	}
}
