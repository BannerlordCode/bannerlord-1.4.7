using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023F RID: 575
	public class InitialState : GameState
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x00074A75 File Offset: 0x00072C75
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600212E RID: 8494 RVA: 0x00074A78 File Offset: 0x00072C78
		// (remove) Token: 0x0600212F RID: 8495 RVA: 0x00074AB0 File Offset: 0x00072CB0
		public event OnInitialMenuOptionInvokedDelegate OnInitialMenuOptionInvoked;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06002130 RID: 8496 RVA: 0x00074AE8 File Offset: 0x00072CE8
		// (remove) Token: 0x06002131 RID: 8497 RVA: 0x00074B20 File Offset: 0x00072D20
		public event OnGameContentUpdatedDelegate OnGameContentUpdated;

		// Token: 0x06002132 RID: 8498 RVA: 0x00074B55 File Offset: 0x00072D55
		protected override void OnActivate()
		{
			base.OnActivate();
			MBMusicManager mbmusicManager = MBMusicManager.Current;
			if (mbmusicManager == null)
			{
				return;
			}
			mbmusicManager.UnpauseMusicManagerSystem();
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00074B6C File Offset: 0x00072D6C
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00074B75 File Offset: 0x00072D75
		public void OnExecutedInitialStateOption(InitialStateOption target)
		{
			OnInitialMenuOptionInvokedDelegate onInitialMenuOptionInvoked = this.OnInitialMenuOptionInvoked;
			if (onInitialMenuOptionInvoked == null)
			{
				return;
			}
			onInitialMenuOptionInvoked(target);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x00074B88 File Offset: 0x00072D88
		public void RefreshContentState()
		{
			OnGameContentUpdatedDelegate onGameContentUpdated = this.OnGameContentUpdated;
			if (onGameContentUpdated == null)
			{
				return;
			}
			onGameContentUpdated();
		}
	}
}
