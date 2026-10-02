using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200008D RID: 141
	public class DeploymentView : MissionView
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x0002661D File Offset: 0x0002481D
		public override void AfterStart()
		{
			base.AfterStart();
			this._deploymentHandler = base.Mission.GetMissionBehavior<DeploymentHandler>();
			this.CreateWidgets();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0002663C File Offset: 0x0002483C
		public override void OnRemoveBehavior()
		{
			this.RemoveWidgets();
			base.OnRemoveBehavior();
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0002664A File Offset: 0x0002484A
		protected virtual void CreateWidgets()
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002664C File Offset: 0x0002484C
		protected virtual void RemoveWidgets()
		{
		}

		// Token: 0x040002ED RID: 749
		private DeploymentHandler _deploymentHandler;
	}
}
