using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000319 RID: 793
	internal class MBNetworkPeer : DotNetObject
	{
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000AF557 File Offset: 0x000AD757
		public NetworkCommunicator NetworkPeer { get; }

		// Token: 0x06002D31 RID: 11569 RVA: 0x000AF55F File Offset: 0x000AD75F
		internal MBNetworkPeer(NetworkCommunicator networkPeer)
		{
			this.NetworkPeer = networkPeer;
		}
	}
}
