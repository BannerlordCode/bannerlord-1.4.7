using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200015B RID: 347
	[Serializable]
	public class ServerStatus
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0000EEC5 File Offset: 0x0000D0C5
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0000EECD File Offset: 0x0000D0CD
		public bool IsMatchmakingEnabled { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0000EED6 File Offset: 0x0000D0D6
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0000EEDE File Offset: 0x0000D0DE
		public bool IsCustomBattleEnabled { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0000EEE7 File Offset: 0x0000D0E7
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0000EEEF File Offset: 0x0000D0EF
		public bool IsPlayerBasedCustomBattleEnabled { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0000EF00 File Offset: 0x0000D100
		public bool IsPremadeGameEnabled { get; set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0000EF09 File Offset: 0x0000D109
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0000EF11 File Offset: 0x0000D111
		public bool IsTestRegionEnabled { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0000EF1A File Offset: 0x0000D11A
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0000EF22 File Offset: 0x0000D122
		public Announcement Announcement { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0000EF2B File Offset: 0x0000D12B
		public ServerNotification[] ServerNotifications { get; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0000EF33 File Offset: 0x0000D133
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0000EF3B File Offset: 0x0000D13B
		public int FriendListUpdatePeriod { get; set; }

		// Token: 0x060009B0 RID: 2480 RVA: 0x0000EF44 File Offset: 0x0000D144
		public ServerStatus()
		{
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0000EF4C File Offset: 0x0000D14C
		public ServerStatus(bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPlayerBasedCustomBattleEnabled, bool isPremadeGameEnabled, bool isTestRegionEnabled, Announcement announcement, ServerNotification[] serverNotifications, int friendListUpdatePeriod)
		{
			this.IsMatchmakingEnabled = isMatchmakingEnabled;
			this.IsCustomBattleEnabled = isCustomBattleEnabled;
			this.IsPlayerBasedCustomBattleEnabled = isPlayerBasedCustomBattleEnabled;
			this.IsPremadeGameEnabled = isPremadeGameEnabled;
			this.IsTestRegionEnabled = isTestRegionEnabled;
			this.Announcement = announcement;
			this.ServerNotifications = serverNotifications;
			this.FriendListUpdatePeriod = friendListUpdatePeriod;
		}
	}
}
