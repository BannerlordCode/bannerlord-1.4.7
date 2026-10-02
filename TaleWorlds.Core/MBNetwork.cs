using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000B0 RID: 176
	public static class MBNetwork
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0001E61C File Offset: 0x0001C81C
		// (set) Token: 0x0600094F RID: 2383 RVA: 0x0001E623 File Offset: 0x0001C823
		public static INetworkCommunication NetworkViewCommunication { get; private set; }

		// Token: 0x06000950 RID: 2384 RVA: 0x0001E62B File Offset: 0x0001C82B
		public static void Initialize(INetworkCommunication networkCommunication)
		{
			MBNetwork.NetworkViewCommunication = networkCommunication;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0001E633 File Offset: 0x0001C833
		public static VirtualPlayer MyPeer
		{
			get
			{
				if (MBNetwork.NetworkViewCommunication != null)
				{
					return MBNetwork.NetworkViewCommunication.MyPeer;
				}
				return null;
			}
		}
	}
}
