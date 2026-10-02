using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006D RID: 109
	public abstract class MissionBattleUIBaseView : MissionView
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		public bool IsViewCreated { get; private set; }

		// Token: 0x06000435 RID: 1077
		protected abstract void OnCreateView();

		// Token: 0x06000436 RID: 1078
		protected abstract void OnDestroyView();

		// Token: 0x06000437 RID: 1079 RVA: 0x0001F5B9 File Offset: 0x0001D7B9
		private void OnEnableView()
		{
			this.OnCreateView();
			this.IsViewCreated = true;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
		private void OnDisableView()
		{
			this.OnDestroyView();
			this.IsViewCreated = false;
		}

		// Token: 0x06000439 RID: 1081
		protected abstract override void OnSuspendView();

		// Token: 0x0600043A RID: 1082
		protected abstract override void OnResumeView();

		// Token: 0x0600043B RID: 1083 RVA: 0x0001F5D7 File Offset: 0x0001D7D7
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			if (GameNetwork.IsMultiplayer)
			{
				this.OnEnableView();
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001F5EC File Offset: 0x0001D7EC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (!GameNetwork.IsMultiplayer && !MBCommon.IsPaused)
			{
				if (!this.IsViewCreated && !BannerlordConfig.HideBattleUI)
				{
					this.OnEnableView();
					return;
				}
				if (this.IsViewCreated && BannerlordConfig.HideBattleUI)
				{
					this.OnDisableView();
				}
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001F639 File Offset: 0x0001D839
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			if (this.IsViewCreated)
			{
				this.OnDisableView();
			}
		}
	}
}
