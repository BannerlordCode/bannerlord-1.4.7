using System;
using SandBox.Missions.MissionLogics;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000021 RID: 33
	[OverrideView(typeof(MissionQuestBarView))]
	public class MissionGauntletQuestBarView : MissionQuestBarView
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000BDC0 File Offset: 0x00009FC0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MissionQuestBarVM();
			this._gauntletLayer = new GauntletLayer("MissionQuestBar", 10, false);
			this._gauntletLayer.LoadMovie("MissionQuestBar", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			foreach (MissionBehavior missionBehavior in base.Mission.MissionBehaviors)
			{
				if (missionBehavior is IMissionProgressTracker)
				{
					this._missionProgressTracker = missionBehavior as IMissionProgressTracker;
					break;
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000BE74 File Offset: 0x0000A074
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._dataSource.OnFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource = null;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000BEA6 File Offset: 0x0000A0A6
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._missionProgressTracker != null)
			{
				this._dataSource.UpdateQuestValues(0f, 1f, this._missionProgressTracker.CurrentProgress);
			}
		}

		// Token: 0x04000090 RID: 144
		private const float MinProgressValue = 0f;

		// Token: 0x04000091 RID: 145
		private const float MaxProgressValue = 1f;

		// Token: 0x04000092 RID: 146
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000093 RID: 147
		private MissionQuestBarVM _dataSource;

		// Token: 0x04000094 RID: 148
		private IMissionProgressTracker _missionProgressTracker;
	}
}
