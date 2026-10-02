using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000E RID: 14
	public class MPVoicePlayerVM : MPPlayerVM
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004467 File Offset: 0x00002667
		public MPVoicePlayerVM(MissionPeer peer)
			: base(peer)
		{
			this.UpdatesSinceSilence = 0;
			this.IsMyPeer = peer.IsMine;
		}

		// Token: 0x0400006A RID: 106
		public const int UpdatesRequiredToRemoveForSilence = 30;

		// Token: 0x0400006B RID: 107
		public readonly bool IsMyPeer;

		// Token: 0x0400006C RID: 108
		public int UpdatesSinceSilence;
	}
}
