using System;
using SandBox.Missions.MissionLogics.Hideout;
using SandBox.Objects.Cinematics;
using SandBox.Objects.Usables;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001B RID: 27
	public class MissionHideoutAmbushCinematicView : MissionView
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000095B8 File Offset: 0x000077B8
		protected virtual void SetPlayerMovementEnabled(bool isPlayerMovementEnabled)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000095BC File Offset: 0x000077BC
		public override void AfterStart()
		{
			base.AfterStart();
			this._cameraEntity = base.Mission.Scene.FindEntityWithTag("hideout_ambush_cutscene_camera");
			this._arrowPath = base.Mission.Scene.FindEntityWithTag("hideout_ambush_cutscene_arrow_path");
			this._hideoutAmbushMissionController = base.Mission.GetMissionBehavior<HideoutAmbushMissionController>();
			Vec3 invalid = Vec3.Invalid;
			this._camera = Camera.CreateCamera();
			this._cameraEntity.GetCameraParamsFromCameraScript(this._camera, ref invalid);
			this._camera.SetFovVertical(this._camera.GetFovVertical(), Screen.AspectRatio, this._camera.Near, this._camera.Far);
			this._arrowPath.SetVisibilityExcludeParents(false);
			this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.None;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00009680 File Offset: 0x00007880
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			switch (this._currentHideoutAmbushCinematicState)
			{
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.None:
			{
				HideoutAmbushMissionController hideoutAmbushMissionController = this._hideoutAmbushMissionController;
				if (hideoutAmbushMissionController != null && hideoutAmbushMissionController.IsReadyForCallTroopsCinematic)
				{
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeOut;
					this.SetPlayerMovementEnabled(false);
					return;
				}
				break;
			}
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeOut:
				ScreenFadeController.BeginFadeOutAndIn(0.5f, 0.5f, 0.5f);
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeToCustomCamera;
				return;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeToCustomCamera:
				if (ScreenFadeController.IsFadedOut)
				{
					base.MissionScreen.CustomCamera = this._camera;
					Agent.Main.AgentVisuals.SetVisible(false);
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeIn;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeIn:
				if (!ScreenFadeController.IsFadeActive)
				{
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SendArrow;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SendArrow:
				this._arrowPath.SetVisibilityExcludeParents(true);
				this._timer = new Timer(base.Mission.CurrentTime, 5f, true);
				this._arrowPath.GetFirstScriptOfType<CinematicBurningArrow>().StartMovement();
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Wait;
				return;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Wait:
				if (this._timer.Check(base.Mission.CurrentTime))
				{
					this._timer = null;
					this._arrowPath.SetVisibilityExcludeParents(false);
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeOut;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeOut:
				ScreenFadeController.BeginFadeOutAndIn(0.5f, 0.5f, 0.5f);
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeBackToDefaultCamera;
				return;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeBackToDefaultCamera:
				if (ScreenFadeController.IsFadedOut)
				{
					base.MissionScreen.CustomCamera = null;
					Agent.Main.AgentVisuals.SetVisible(true);
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeIn;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeIn:
				if (!ScreenFadeController.IsFadeActive)
				{
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Ending;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Ending:
				this.SetPlayerMovementEnabled(true);
				this._hideoutAmbushMissionController.OnAgentsShouldBeEnabled();
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Ended;
				break;
			default:
				return;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00009838 File Offset: 0x00007A38
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			base.OnObjectUsed(userAgent, usedObject);
			if (userAgent == Agent.Main && usedObject is StealthAreaUsePoint)
			{
				MissionAgentAlarmStateView missionBehavior = base.Mission.GetMissionBehavior<MissionAgentAlarmStateView>();
				if (missionBehavior != null && missionBehavior.IsReady())
				{
					missionBehavior.SuspendView();
				}
			}
		}

		// Token: 0x04000067 RID: 103
		private const string CameraTag = "hideout_ambush_cutscene_camera";

		// Token: 0x04000068 RID: 104
		private const string ArrowBarrelTag = "hideout_ambush_cutscene_arrow_barrel";

		// Token: 0x04000069 RID: 105
		private const string ArrowPathTag = "hideout_ambush_cutscene_arrow_path";

		// Token: 0x0400006A RID: 106
		private Camera _camera;

		// Token: 0x0400006B RID: 107
		private GameEntity _cameraEntity;

		// Token: 0x0400006C RID: 108
		private GameEntity _arrowPath;

		// Token: 0x0400006D RID: 109
		private HideoutAmbushMissionController _hideoutAmbushMissionController;

		// Token: 0x0400006E RID: 110
		private MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState _currentHideoutAmbushCinematicState;

		// Token: 0x0400006F RID: 111
		private Timer _timer;

		// Token: 0x0200008B RID: 139
		private enum HideoutAmbushCinematicState
		{
			// Token: 0x040002B4 RID: 692
			None,
			// Token: 0x040002B5 RID: 693
			FirstFadeOut,
			// Token: 0x040002B6 RID: 694
			ChangeToCustomCamera,
			// Token: 0x040002B7 RID: 695
			FirstFadeIn,
			// Token: 0x040002B8 RID: 696
			SendArrow,
			// Token: 0x040002B9 RID: 697
			Wait,
			// Token: 0x040002BA RID: 698
			SecondFadeOut,
			// Token: 0x040002BB RID: 699
			ChangeBackToDefaultCamera,
			// Token: 0x040002BC RID: 700
			SecondFadeIn,
			// Token: 0x040002BD RID: 701
			Ending,
			// Token: 0x040002BE RID: 702
			Ended
		}
	}
}
