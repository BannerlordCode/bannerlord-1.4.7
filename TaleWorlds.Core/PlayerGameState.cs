using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C4 RID: 196
	public abstract class PlayerGameState : GameState
	{
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0002309F File Offset: 0x0002129F
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x000230A7 File Offset: 0x000212A7
		public VirtualPlayer Peer
		{
			get
			{
				return this._peer;
			}
			private set
			{
				this._peer = value;
			}
		}

		// Token: 0x040005FF RID: 1535
		private VirtualPlayer _peer;
	}
}
