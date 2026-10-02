using System;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016B RID: 363
	public abstract class GameBadgeTracker
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x00010106 File Offset: 0x0000E306
		public virtual void OnPlayerJoin(PlayerData playerData)
		{
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00010108 File Offset: 0x0000E308
		public virtual void OnKill(KillData killData)
		{
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001010A File Offset: 0x0000E30A
		public virtual void OnStartingNextBattle()
		{
		}
	}
}
