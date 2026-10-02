using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000018 RID: 24
	public class MissionCustomCameraView : MissionView
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00005CE8 File Offset: 0x00003EE8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag(this.tag))
			{
				Camera camera = Camera.CreateCamera();
				gameEntity.GetCameraParamsFromCameraScript(camera, ref this._dofParams);
				this._cameras.Add(camera);
			}
			base.MissionScreen.CustomCamera = this._cameras[0];
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005D78 File Offset: 0x00003F78
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (base.DebugInput.IsHotKeyReleased("CustomCameraMissionViewHotkeyIncreaseCustomCameraIndex"))
			{
				this._currentCameraIndex++;
				if (this._currentCameraIndex >= this._cameras.Count)
				{
					this._currentCameraIndex = 0;
				}
				base.MissionScreen.CustomCamera = this._cameras[this._currentCameraIndex];
			}
		}

		// Token: 0x04000033 RID: 51
		public string tag = "customcamera";

		// Token: 0x04000034 RID: 52
		private readonly List<Camera> _cameras = new List<Camera>();

		// Token: 0x04000035 RID: 53
		public Vec3 _dofParams;

		// Token: 0x04000036 RID: 54
		private int _currentCameraIndex;
	}
}
