using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031F RID: 799
	public class NetworkCommunication : INetworkCommunication
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x000AFF0B File Offset: 0x000AE10B
		VirtualPlayer INetworkCommunication.MyPeer
		{
			get
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				if (myPeer == null)
				{
					return null;
				}
				return myPeer.VirtualPlayer;
			}
		}
	}
}
