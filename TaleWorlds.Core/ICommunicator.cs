using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000082 RID: 130
	public interface ICommunicator
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600087F RID: 2175
		VirtualPlayer VirtualPlayer { get; }

		// Token: 0x06000880 RID: 2176
		void OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component);

		// Token: 0x06000881 RID: 2177
		void OnAddComponent(PeerComponent component);

		// Token: 0x06000882 RID: 2178
		void OnRemoveComponent(PeerComponent component);

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000883 RID: 2179
		bool IsNetworkActive { get; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000884 RID: 2180
		bool IsConnectionActive { get; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000885 RID: 2181
		bool IsServerPeer { get; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000886 RID: 2182
		// (set) Token: 0x06000887 RID: 2183
		bool IsSynchronized { get; set; }
	}
}
