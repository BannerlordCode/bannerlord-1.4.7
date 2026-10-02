using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A4 RID: 676
	public interface IRoundComponent : IMissionBehavior
	{
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002550 RID: 9552
		// (remove) Token: 0x06002551 RID: 9553
		event Action OnRoundStarted;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06002552 RID: 9554
		// (remove) Token: 0x06002553 RID: 9555
		event Action OnPreparationEnded;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06002554 RID: 9556
		// (remove) Token: 0x06002555 RID: 9557
		event Action OnPreRoundEnding;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06002556 RID: 9558
		// (remove) Token: 0x06002557 RID: 9559
		event Action OnRoundEnding;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06002558 RID: 9560
		// (remove) Token: 0x06002559 RID: 9561
		event Action OnPostRoundEnded;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x0600255A RID: 9562
		// (remove) Token: 0x0600255B RID: 9563
		event Action OnCurrentRoundStateChanged;

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600255C RID: 9564
		float LastRoundEndRemainingTime { get; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600255D RID: 9565
		float RemainingRoundTime { get; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600255E RID: 9566
		MultiplayerRoundState CurrentRoundState { get; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x0600255F RID: 9567
		int RoundCount { get; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002560 RID: 9568
		BattleSideEnum RoundWinner { get; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002561 RID: 9569
		RoundEndReason RoundEndReason { get; }
	}
}
