using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000036 RID: 54
	[OverrideView(typeof(MissionAgentLockVisualizerView))]
	public class MissionGauntletAgentLockVisualizerView : MissionBattleUIBaseView
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000E248 File Offset: 0x0000C448
		protected override void OnCreateView()
		{
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._missionMainAgentController.OnLockedAgentChanged += this.OnLockedAgentChanged;
			this._missionMainAgentController.OnPotentialLockedAgentChanged += this.OnPotentialLockedAgentChanged;
			this._dataSource = new MissionAgentLockVisualizerVM();
			this._layer = new GauntletLayer("MissionAgentLockVisualizer", 10, false);
			this._layer.LoadMovie("AgentLockTargets", this._dataSource);
			base.MissionScreen.AddLayer(this._layer);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000E2DA File Offset: 0x0000C4DA
		protected override void OnDestroyView()
		{
			base.MissionScreen.RemoveLayer(this._layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layer = null;
			this._missionMainAgentController = null;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000E30D File Offset: 0x0000C50D
		protected override void OnSuspendView()
		{
			if (this._layer != null)
			{
				ScreenManager.SetSuspendLayer(this._layer, true);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000E323 File Offset: 0x0000C523
		protected override void OnResumeView()
		{
			if (this._layer != null)
			{
				ScreenManager.SetSuspendLayer(this._layer, false);
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000E339 File Offset: 0x0000C539
		private void OnPotentialLockedAgentChanged(Agent newPotentialAgent)
		{
			MissionAgentLockVisualizerVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.IsEnabled)
			{
				this._dataSource.OnPossibleLockAgentChange(this._latestPotentialLockedAgent, newPotentialAgent);
				this._latestPotentialLockedAgent = newPotentialAgent;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E368 File Offset: 0x0000C568
		private void OnLockedAgentChanged(Agent newAgent)
		{
			MissionAgentLockVisualizerVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.IsEnabled)
			{
				this._dataSource.OnActiveLockAgentChange(this._latestLockedAgent, newAgent);
				this._latestLockedAgent = newAgent;
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000E398 File Offset: 0x0000C598
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsViewCreated && this._dataSource != null)
			{
				this._dataSource.IsEnabled = this.IsMainAgentAvailable();
				if (this._dataSource.IsEnabled)
				{
					for (int i = 0; i < this._dataSource.AllTrackedAgents.Count; i++)
					{
						MissionAgentLockItemVM missionAgentLockItemVM = this._dataSource.AllTrackedAgents[i];
						float num = 0f;
						float num2 = 0f;
						float num3 = 0f;
						MBWindowManager.WorldToScreenInsideUsableArea(base.MissionScreen.CombatCamera, missionAgentLockItemVM.TrackedAgent.GetChestGlobalPosition(), ref num, ref num2, ref num3);
						missionAgentLockItemVM.Position = new Vec2(num, num2);
					}
				}
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000E44F File Offset: 0x0000C64F
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive();
		}

		// Token: 0x0400013E RID: 318
		private GauntletLayer _layer;

		// Token: 0x0400013F RID: 319
		private MissionAgentLockVisualizerVM _dataSource;

		// Token: 0x04000140 RID: 320
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x04000141 RID: 321
		private Agent _latestLockedAgent;

		// Token: 0x04000142 RID: 322
		private Agent _latestPotentialLockedAgent;
	}
}
