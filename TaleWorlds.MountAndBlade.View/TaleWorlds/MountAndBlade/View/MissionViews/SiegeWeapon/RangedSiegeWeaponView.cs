using System;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;

namespace TaleWorlds.MountAndBlade.View.MissionViews.SiegeWeapon
{
	// Token: 0x020000A6 RID: 166
	public class RangedSiegeWeaponView : UsableMissionObjectComponent
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x000281A4 File Offset: 0x000263A4
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x000281AC File Offset: 0x000263AC
		public RangedSiegeWeapon RangedSiegeWeapon { get; private set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x000281B5 File Offset: 0x000263B5
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x000281BD File Offset: 0x000263BD
		public MissionScreen MissionScreen { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x000281C6 File Offset: 0x000263C6
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x000281CE File Offset: 0x000263CE
		public Camera Camera { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x000281D7 File Offset: 0x000263D7
		public GameEntity CameraHolder
		{
			get
			{
				return this.RangedSiegeWeapon.CameraHolder;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x000281E4 File Offset: 0x000263E4
		public Agent PilotAgent
		{
			get
			{
				return this.RangedSiegeWeapon.PilotAgent;
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000281F1 File Offset: 0x000263F1
		public void Initialize(RangedSiegeWeapon rangedSiegeWeapon, MissionScreen missionScreen)
		{
			this.RangedSiegeWeapon = rangedSiegeWeapon;
			this.MissionScreen = missionScreen;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00028201 File Offset: 0x00026401
		protected override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			if (this.CameraHolder != null)
			{
				this.CreateCamera();
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00028220 File Offset: 0x00026420
		protected override void OnMissionReset()
		{
			base.OnMissionReset();
			if (this.CameraHolder != null)
			{
				this._cameraYaw = this._cameraInitialYaw;
				this._cameraPitch = this._cameraInitialPitch;
				this.ApplyCameraRotation();
				this._isInWeaponCameraMode = false;
				this.ResetCamera();
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0002826C File Offset: 0x0002646C
		public override bool IsOnTickRequired()
		{
			return true;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0002826F File Offset: 0x0002646F
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!GameNetwork.IsReplay)
			{
				this.HandleUserInput(dt);
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00028288 File Offset: 0x00026488
		protected virtual void HandleUserInput(float dt)
		{
			if (this.CameraHolder != null && ((this.PilotAgent != null && this.PilotAgent.IsMainAgent) || this.RangedSiegeWeapon.PlayerForceUse))
			{
				if (!this._isInWeaponCameraMode)
				{
					this._isInWeaponCameraMode = true;
					this.StartUsingWeaponCamera();
				}
				if (this.RangedSiegeWeapon.PlayerForceUse)
				{
					this.HandleUserCameraRotation(dt);
				}
			}
			if (this._isInWeaponCameraMode && (this.PilotAgent == null || !this.PilotAgent.IsMainAgent) && !this.RangedSiegeWeapon.PlayerForceUse)
			{
				this._isInWeaponCameraMode = false;
				this.ResetCamera();
			}
			this.HandleUserAiming(dt);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0002832C File Offset: 0x0002652C
		private void CreateCamera()
		{
			this.Camera = Camera.CreateCamera();
			float aspectRatio = Screen.AspectRatio;
			this.Camera.SetFovVertical(1.0471976f, aspectRatio, 0.1f, 12500f);
			this.Camera.Entity = this.CameraHolder;
			MatrixFrame frame = this.CameraHolder.GetFrame();
			Vec3 eulerAngles = frame.rotation.GetEulerAngles();
			this._cameraYaw = eulerAngles.z;
			this._cameraPitch = eulerAngles.x;
			this._cameraRoll = eulerAngles.y;
			this._cameraPositionOffset = frame.origin;
			this._cameraPositionOffset.RotateAboutZ(-this._cameraYaw);
			this._cameraPositionOffset.RotateAboutX(-this._cameraPitch);
			this._cameraPositionOffset.RotateAboutY(-this._cameraRoll);
			this._cameraInitialYaw = this._cameraYaw;
			this._cameraInitialPitch = this._cameraPitch;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0002840D File Offset: 0x0002660D
		protected virtual void StartUsingWeaponCamera()
		{
			this.MissionScreen.CustomCamera = this.Camera;
			Agent.Main.IsLookDirectionLocked = true;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0002842C File Offset: 0x0002662C
		private void ResetCamera()
		{
			if (this.MissionScreen.CustomCamera == this.Camera)
			{
				this.MissionScreen.CustomCamera = null;
				if (Agent.Main != null)
				{
					Agent.Main.IsLookDirectionLocked = false;
					this.MissionScreen.SetExtraCameraParameters(false, 0f);
				}
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00028480 File Offset: 0x00026680
		protected virtual void HandleUserCameraRotation(float dt)
		{
			float cameraYaw = this._cameraYaw;
			float cameraPitch = this._cameraPitch;
			if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(10))
			{
				this._cameraYaw = this._cameraInitialYaw;
				this._cameraPitch = this._cameraInitialPitch;
			}
			this._cameraYaw += this.MissionScreen.SceneLayer.Input.GetMouseMoveX() * dt * 0.2f;
			this._cameraPitch += this.MissionScreen.SceneLayer.Input.GetMouseMoveY() * dt * 0.2f;
			this._cameraYaw = MBMath.ClampFloat(this._cameraYaw, 1.5707964f, 4.712389f);
			this._cameraPitch = MBMath.ClampFloat(this._cameraPitch, 1.0471976f, 1.7453294f);
			if (cameraPitch != this._cameraPitch || cameraYaw != this._cameraYaw)
			{
				this.ApplyCameraRotation();
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0002856C File Offset: 0x0002676C
		private void ApplyCameraRotation()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.rotation.RotateAboutUp(this._cameraYaw);
			identity.rotation.RotateAboutSide(this._cameraPitch);
			identity.rotation.RotateAboutForward(this._cameraRoll);
			identity.Strafe(this._cameraPositionOffset.x);
			identity.Advance(this._cameraPositionOffset.y);
			identity.Elevate(this._cameraPositionOffset.z);
			this.CameraHolder.SetFrame(ref identity, true);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000285FC File Offset: 0x000267FC
		private void HandleUserAiming(float dt)
		{
			bool flag = false;
			float num = 0f;
			float num2 = 0f;
			if (this.PilotAgent != null && (this.PilotAgent.IsMainAgent || this.RangedSiegeWeapon.PlayerForceUse))
			{
				if (this.UsesMouseForAiming)
				{
					InputContext input = this.MissionScreen.SceneLayer.Input;
					float num3 = dt * 1666.6666f;
					float num4 = input.GetMouseMoveX() + num3 * input.GetGameKeyAxis("CameraAxisX");
					float num5 = input.GetMouseMoveY() + -num3 * input.GetGameKeyAxis("CameraAxisY");
					if (NativeConfig.InvertMouse)
					{
						num5 *= -1f;
					}
					Vec2 vec = new Vec2(-num4, -num5);
					if (vec.IsNonZero())
					{
						float num6 = vec.Normalize();
						num6 = MathF.Min(5f, MathF.Pow(num6, 1.5f) * 0.025f);
						vec *= num6;
						num = vec.x;
						num2 = vec.y;
					}
				}
				else
				{
					if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(2))
					{
						num = 1f;
					}
					else if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(3))
					{
						num = -1f;
					}
					if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(0))
					{
						num2 = 1f;
					}
					else if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(1))
					{
						num2 = -1f;
					}
				}
				if (num != 0f)
				{
					flag = true;
				}
				if (num2 != 0f)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.RangedSiegeWeapon.GiveInput(num, num2);
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000287A1 File Offset: 0x000269A1
		protected override void OnMissionObjectDisabled()
		{
			this.ResetCamera();
		}

		// Token: 0x04000306 RID: 774
		private float _cameraYaw;

		// Token: 0x04000307 RID: 775
		private float _cameraPitch;

		// Token: 0x04000308 RID: 776
		private float _cameraRoll;

		// Token: 0x04000309 RID: 777
		private float _cameraInitialYaw;

		// Token: 0x0400030A RID: 778
		private float _cameraInitialPitch;

		// Token: 0x0400030B RID: 779
		private Vec3 _cameraPositionOffset;

		// Token: 0x0400030C RID: 780
		private bool _isInWeaponCameraMode;

		// Token: 0x0400030D RID: 781
		protected bool UsesMouseForAiming;
	}
}
