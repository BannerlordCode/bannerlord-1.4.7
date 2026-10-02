using System;
using SandBox.Missions.MissionLogics.Hideout;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001A RID: 26
	internal class MissionHideoutAmbushBossFightCinematicView : MissionView
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x000091DC File Offset: 0x000073DC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (!this._isInitialized)
			{
				this.InitializeView();
				return;
			}
			if (!Game.Current.GameStateManager.ActiveStateDisabledByUser && (this._currentState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.Cinematic || this._nextState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.Cinematic))
			{
				this.UpdateCamera(dt);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000922C File Offset: 0x0000742C
		private void SetCameraFrame(Vec3 position, Vec3 direction, out MatrixFrame cameraFrame)
		{
			cameraFrame.origin = position;
			cameraFrame.rotation.s = Vec3.Side;
			cameraFrame.rotation.f = Vec3.Up;
			cameraFrame.rotation.u = -direction;
			cameraFrame.rotation.Orthonormalize();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000927C File Offset: 0x0000747C
		private void SetupCamera()
		{
			this._camera = Camera.CreateCamera();
			Camera combatCamera = base.MissionScreen.CombatCamera;
			if (combatCamera != null)
			{
				this._camera.FillParametersFrom(combatCamera);
			}
			else
			{
				Debug.FailedAssert("Combat camera is null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Missions\\MissionHideoutAmbushBossFightCinematicView.cs", "SetupCamera", 66);
			}
			Vec3 vec;
			this._cinematicLogicController.GetBossStandingEyePosition(out vec);
			Vec3 vec2;
			this._cinematicLogicController.GetPlayerStandingEyePosition(out vec2);
			Vec3 vec3 = (vec - vec2).NormalizedCopy();
			float num;
			float num2;
			float num3;
			this._cinematicLogicController.GetScenePrefabParameters(out num, out num2, out num3);
			float num4 = num + num2 + 1.5f * num3;
			this._cameraSpeed = num4 / MathF.Max(this._cinematicLogicController.CinematicDuration, 0.1f);
			this._cameraMoveDir = -vec3;
			this.SetCameraFrame(vec, vec3, out this._cameraFrame);
			Vec3 vec4 = this._cameraFrame.origin + this._cameraOffset.x * this._cameraFrame.rotation.s + this._cameraOffset.y * this._cameraFrame.rotation.f + this._cameraOffset.z * this._cameraFrame.rotation.u;
			Vec3 vec5 = (vec - vec4).NormalizedCopy();
			this.SetCameraFrame(vec4, vec5, out this._cameraFrame);
			this._camera.Frame = this._cameraFrame;
			base.MissionScreen.CustomCamera = this._camera;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00009414 File Offset: 0x00007614
		private void UpdateCamera(float dt)
		{
			Vec3 vec = this._cameraFrame.origin + this._cameraMoveDir * this._cameraSpeed * dt;
			Vec3 vec2;
			this._cinematicLogicController.GetBossStandingEyePosition(out vec2);
			Vec3 vec3 = (vec2 - vec).NormalizedCopy();
			this.SetCameraFrame(vec, vec3, out this._cameraFrame);
			this._camera.Frame = this._cameraFrame;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00009485 File Offset: 0x00007685
		private void ReleaseCamera()
		{
			base.MissionScreen.UpdateFreeCamera(base.MissionScreen.CustomCamera.Frame);
			base.MissionScreen.CustomCamera = null;
			this._camera.ReleaseCamera();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000094B9 File Offset: 0x000076B9
		private void OnCinematicStateChanged(HideoutAmbushBossFightCinematicController.HideoutCinematicState state)
		{
			if (this._isInitialized)
			{
				this._currentState = state;
				if (this._currentState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.PreCinematic)
				{
					this.SetupCamera();
					return;
				}
				if (this._currentState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.PostCinematic)
				{
					this.ReleaseCamera();
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000094E9 File Offset: 0x000076E9
		private void OnCinematicTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState nextState, float duration)
		{
			if (this._isInitialized)
			{
				if (nextState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.InitialFadeOut || nextState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.PostCinematic)
				{
					ScreenFadeController.BeginFadeOut(duration);
				}
				else if (nextState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.Cinematic || nextState == HideoutAmbushBossFightCinematicController.HideoutCinematicState.Completed)
				{
					ScreenFadeController.BeginFadeIn(duration);
				}
				this._nextState = nextState;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00009518 File Offset: 0x00007718
		private void InitializeView()
		{
			this._cinematicLogicController = base.Mission.GetMissionBehavior<HideoutAmbushBossFightCinematicController>();
			this._isInitialized = this._cinematicLogicController != null;
			if (this._cinematicLogicController != null)
			{
				this._cinematicLogicController.OnCinematicStateChanged += this.OnCinematicStateChanged;
				this._cinematicLogicController.OnCinematicTransition += this.OnCinematicTransition;
			}
		}

		// Token: 0x0400005E RID: 94
		private bool _isInitialized;

		// Token: 0x0400005F RID: 95
		private HideoutAmbushBossFightCinematicController _cinematicLogicController;

		// Token: 0x04000060 RID: 96
		private HideoutAmbushBossFightCinematicController.HideoutCinematicState _currentState;

		// Token: 0x04000061 RID: 97
		private HideoutAmbushBossFightCinematicController.HideoutCinematicState _nextState;

		// Token: 0x04000062 RID: 98
		private Camera _camera;

		// Token: 0x04000063 RID: 99
		private MatrixFrame _cameraFrame = MatrixFrame.Identity;

		// Token: 0x04000064 RID: 100
		private readonly Vec3 _cameraOffset = new Vec3(0.3f, 0.3f, 1.2f, -1f);

		// Token: 0x04000065 RID: 101
		private Vec3 _cameraMoveDir = Vec3.Forward;

		// Token: 0x04000066 RID: 102
		private float _cameraSpeed;
	}
}
