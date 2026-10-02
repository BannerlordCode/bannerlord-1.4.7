using System;
using System.Collections.Generic;
using StoryMode.Missions;
using StoryMode.View.Missions;
using StoryMode.ViewModelCollection.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace StoryMode.GauntletUI.Missions
{
	// Token: 0x02000049 RID: 73
	[OverrideView(typeof(MissionTrainingFieldObjectiveView))]
	public class MissionGauntletTrainingFieldObjectiveView : MissionView
	{
		// Token: 0x0600015F RID: 351 RVA: 0x00004858 File Offset: 0x00002A58
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			TrainingFieldMissionController missionBehavior = base.Mission.GetMissionBehavior<TrainingFieldMissionController>();
			this._dataSource = new TrainingFieldObjectivesVM();
			this._dataSource.UpdateCurrentObjectiveExplanationText(missionBehavior.InitialCurrentObjective);
			this._layer = new GauntletLayer("TrainingFieldObjectives", 2, false);
			this._layer.LoadMovie("TrainingFieldObjectives", this._dataSource);
			base.MissionScreen.AddLayer(this._layer);
			missionBehavior.TimerTick = new Action<string>(this._dataSource.UpdateTimerText);
			missionBehavior.CurrentObjectiveTick = new Action<TextObject>(this._dataSource.UpdateCurrentObjectiveExplanationText);
			missionBehavior.AllObjectivesTick = new Action<List<TrainingFieldMissionController.TutorialObjective>>(this._dataSource.UpdateObjectivesWith);
			missionBehavior.UIStartTimer = new Action(this.BeginTimer);
			missionBehavior.UIEndTimer = new Func<float>(this.EndTimer);
			missionBehavior.CurrentMouseObjectiveTick = new Action<TrainingFieldMissionController.MouseObjectives, TrainingFieldMissionController.ObjectivePerformingType>(this._dataSource.UpdateCurrentMouseObjective);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004950 File Offset: 0x00002B50
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isTimerActive)
			{
				this._dataSource.UpdateTimerText((base.Mission.CurrentTime - this._beginningTime).ToString("0.0"));
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004996 File Offset: 0x00002B96
		private void BeginTimer()
		{
			this._isTimerActive = true;
			this._beginningTime = base.Mission.CurrentTime;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000049B0 File Offset: 0x00002BB0
		private float EndTimer()
		{
			this._isTimerActive = false;
			this._dataSource.UpdateTimerText("");
			return base.Mission.CurrentTime - this._beginningTime;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000049DB File Offset: 0x00002BDB
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._layer);
			this._dataSource = null;
			this._layer = null;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004A02 File Offset: 0x00002C02
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._layer != null)
			{
				this._layer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00004A27 File Offset: 0x00002C27
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._layer != null)
			{
				this._layer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x0400005F RID: 95
		private TrainingFieldObjectivesVM _dataSource;

		// Token: 0x04000060 RID: 96
		private GauntletLayer _layer;

		// Token: 0x04000061 RID: 97
		private float _beginningTime;

		// Token: 0x04000062 RID: 98
		private bool _isTimerActive;
	}
}
